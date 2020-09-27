using System;
using System.Collections.Generic;
using System.Text;
using GPK_RePack.Core.IO;
using GPK_RePack.Core.Model.ExportData;
using GPK_RePack.Core.Model.Interfaces;
using GPK_RePack.Core.Model.Prop;
using NLog;

namespace GPK_RePack.Core.Model
{
    [Serializable]
    public class GpkExport : IGpkPart
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GpkPackage motherPackage;
        public string UID;

        public int ClassIndex;
        public int SuperIndex;
        public int PackageIndex;

        public string ClassName;
        public string SuperName;
        public string PackageName;

        public string ObjectName;

        public long Unk1;
        public long Unk2;

        public int SerialSize;
        public int SerialOffset;
        public long SerialOffsetPosition;

        public int NetIndex;
        public string NetIndexName = null;


        public int Unk3;
        public int UnkHeaderCount;
        public int Unk4;
        public byte[] Guid;
        public byte[] UnkExtraInts;

        public byte[] PropertyPadding;

        public int DependsTableData;

        public int PropertySize;
        public List<IProperty> Properties;

        public long DataStart;
        public byte[] DataPadding;
        private byte[] m_data;
        private IPayload m_payload;
        public DataLoader Loader;

        public byte[] Data
        {
            get
            {
                if (Loader != null)
                {
                    m_data = Loader.LoadData();
                    Loader = null; //null first, row is important here!
                    Reader.ParsePayload(motherPackage, this);
                    logger.Trace("{0}: JIT Load triggerd", ObjectName);
                }

                return m_data;
            }

            set
            {
                m_data = value;
                Loader = null; //no overrides later
            }
        }

        public IPayload Payload
        {
            get
            {
                if (Loader != null)
                {
                    //trigger data property, loads data from file, reads our payload
                    Data.ToString();

                }

                return m_payload;
            }

            set
            {
                m_payload = value;
                Loader = null; //no overrides later
            }
        }


        public GpkExport(GpkPackage mothership)
        {
            motherPackage = mothership;
            Properties = new List<IProperty>();

        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: Export");
            info.AppendLine("UID: " + UID);
            info.AppendLine("Object: " + ObjectName);
            info.AppendLine("Class: " + ClassName);
            info.AppendLine("Super: " + SuperName);
            info.AppendLine("Package: " + PackageName);
            info.AppendLine("Netindex: " + NetIndex);
            if (NetIndexName != null) info.AppendLine("NetindexObject: " + NetIndexName);

            info.AppendLine("Data_Offset: " + SerialOffset);
            if (Loader != null)
            {
                //loader first, we dont want loading if the data is not really needed
                info.AppendLine(Loader.ToString());
            }
            else if (Payload != null)
            {
                info.AppendLine("Data_Size: " + Payload.GetSize());
            }
            else if (Data != null)
            {
                info.AppendLine("Data_Size: " + Data.Length);
            }
            else
            {
                info.AppendLine("Data_Size: 0");
            }


            info.AppendLine("Properties:");
            foreach (IProperty prop in Properties)
            {
                info.AppendLine(prop.ToString());
            }

            if (Payload != null && Loader == null)
            {
                info.AppendLine("\nPayload:");
                info.Append(Payload);
            }
            return info.ToString();
        }

        public string ToCompactString()
        {
            return String.Format("E;{0};{1};{2};{3};{4};{5}", UID, ObjectName, ClassName, SuperName, PackageName, SerialOffset) ;
        }
        public int GetDataSize()
        {
            int size = 0;

            //props
            size += 4; //netindex
            if (PropertyPadding != null)
            {
                size += PropertyPadding.Length;
            }

            foreach (IProperty iProp in Properties)
            {
                size += iProp.RecalculateSize() + 24;
            }
            //none
            size += 8;
            //finally
            PropertySize = size;

            if (DataPadding != null)
            {
                size += DataPadding.Length;
            }
            //data
            if (Loader != null)
            {
                //loader first. prevents mass loading, eg for package size computing
                size += Loader.Length;
            }
            else if (Payload != null)
            {
                size += Payload.GetSize();
            }
            else if (Data != null)
            {
                size += Data.Length;
            }

            SerialSize = size;

            return size;
        }

        public int GetSize()
        {
            //for export part ony, for data see recal func
            int tmpSize = 0;
            tmpSize += 36; //static
            if (SerialSize > 0) tmpSize += 4;
            tmpSize += 28; //static after serialoffset
            tmpSize += UnkExtraInts.Length;

            return tmpSize;

        }

        //External format, Package.UID
        public string GetNormalizedUID()
        {
            return motherPackage.GetNormalizedFilename() + "." + UID;
        }

        internal void CheckNamePresence(GpkPackage package)
        {
            package.GetStringIndex(ObjectName);

            foreach (IProperty iProp in Properties)
            {
                GpkBaseProperty baseProperty = (GpkBaseProperty)iProp;
                package.GetStringIndex(baseProperty.name);
                package.GetStringIndex(baseProperty.type);

                iProp.CheckAndAddNames(package);
            }
        }
        public IProperty GetProperty(String name)
        {
            return Properties.Find(x => ((GpkBaseProperty) x).name == name);
        }

      
    }
}
