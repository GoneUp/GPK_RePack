using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GPK_RePack.Classes.ExportData;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Prop;
using GPK_RePack.IO;

namespace GPK_RePack.Classes
{
    [Serializable]
    class GpkExport : IGpkPart
    {
        private GpkPackage motherPackage;
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

        public byte[] PaddingUnk = new byte[28];//28 byte byte padding? + 4 vor letztem
        public byte[] PropertyPadding;

        public int PropertySize;
        public List<IProperty> Properties;

        public long DataStart;
        public byte[] DataPadding;
        private byte[] m_data; 
        public IPayload m_payload;
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

        /*
         * no idea why this is here. maybe its usefull later.
        public GpkExport(GpkExport export)
        {
            //clone class
            motherPackage = export.motherPackage;
            UID = export.UID;

            ClassIndex = export.ClassIndex;
            SuperIndex = export.SuperIndex;
            PackageIndex = export.PackageIndex;

            ClassName = export.ClassName;
            SuperName = export.SuperName;
            PackageName = export.PackageName;
            ObjectName = export.ObjectName;

            Unk1 = export.Unk1;
            Unk2 = export.Unk2;

            SerialSize = export.SerialSize;
            SerialOffset = export.SerialOffset;
            SerialOffsetPosition = export.SerialOffsetPosition;

            NetIndex = export.NetIndex;
            NetIndexName = export.NetIndexName;
            PaddingUnk = export.PaddingUnk;
            PropertyPadding = export.PropertyPadding;
            PropertySize = export.PropertySize;
            Properties = export.Properties;

            DataStart = export.DataStart;
            DataPadding = export.DataPadding;
            Data = export.Data;
            Loader = export.Loader;
            Payload = export.Payload;

        }
         * */

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

            return info.ToString();
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
            tmpSize += PaddingUnk.Length;

            return tmpSize;

        }


    }
}
