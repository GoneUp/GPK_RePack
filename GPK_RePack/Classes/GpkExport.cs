using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Prop;

namespace GPK_RePack.Classes
{
    [Serializable]
    class GpkExport 
    {
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

        public int netIndex;
        public byte[] padding_unk = new byte[28];
        //28 byte byte padding? + 4 vor letztem

        public byte[] property_padding;
        public int property_size;
        public List<IProperty> Properties;

        public long data_start;
        public byte[] data_padding;
        public byte[] data;

        public IPayload payload;

        public GpkExport()
        {
            Properties = new List<IProperty>();
        }

        public GpkExport(GpkExport export)
        {
            //clone class
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

            netIndex = export.netIndex;
            padding_unk = export.padding_unk;
            property_padding = export.property_padding;
            property_size = export.property_size;
            Properties = export.Properties;

            data_start = export.data_start;
            data_padding = export.data_padding;
            data = export.data;
            payload = export.payload;
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
            info.AppendLine("Data_Offset: " + SerialOffset);
            if (data != null)
            {
                info.AppendLine("Data_Size: " + data.Length);
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

        public int RecalculateSize()
        {
            int size = 0;

            //props
            int tmpSize = 0;
            tmpSize += 4; //netindex
            if (property_padding != null)
            {
                tmpSize += property_padding.Length;
            }

            foreach (var property in Properties)
            {
                var baseProp = (GpkBaseProperty) property;
                tmpSize += baseProp.size + 24;
            }
            //none
            tmpSize += 8;
            //finally
            property_size = tmpSize;
            size += tmpSize;

            //data
            if (data != null)
            {
                size += data.Length;
            } 

            SerialSize = size;

            return size;
        }



      
    }
}
