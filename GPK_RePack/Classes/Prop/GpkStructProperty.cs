using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkStructProperty : GpkBaseProperty, IProperty
    {
        public string innerType;
        public byte[] value;

        public GpkStructProperty()
        {
            RecalculateSize();
        }
        public GpkStructProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Length: {2}", name, type, size);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            writer.Write(package.GetStringIndex(innerType));
            writer.Write(value);
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            long structtype = reader.ReadInt64();
            innerType = package.GetString(structtype);
            value = new byte[size];
            value = reader.ReadBytes(size);
        }

        public int RecalculateSize()
        {
            int tmpSize = 0; 
            if (value != null)
            {
                tmpSize += value.Length;
            }
            size = tmpSize;
            return size + 8; //length not included in normal len
        }

        public bool ValidateValue(string input)
        {
            return false;
        }

        public bool SetValue(string input)
        {
            return false;
        }

        public string GetValueHex()
        {
            string hex = "";
            if (value != null) hex = value.ToHex();
            return hex;
        }
    }

}
