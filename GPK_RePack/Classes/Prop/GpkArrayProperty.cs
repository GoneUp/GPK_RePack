using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkArrayProperty : GpkBaseProperty, IProperty
    {
        public byte[] value;

        public GpkArrayProperty()
        {
            RecalculateSize();
        }
        public GpkArrayProperty(GpkBaseProperty bp)
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
            writer.Write(value);
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            value = new byte[size];
            value = reader.ReadBytes(size);
            RecalculateSize();
        }

        public int RecalculateSize()
        {
            int tmpSize = 0;
            if (value != null)
            {
                tmpSize += value.Length;
            }
            size = tmpSize;
            return size;
        }

        public string GetValueHex()
        {
            string hex = "";
            if(value != null) hex = value.ToHex();
            return hex;
        }
    }

}
