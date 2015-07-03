using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkByteProperty : GpkBaseProperty, IProperty
    {
        public string nameValue = null; //long index
        public byte byteValue;

        public GpkByteProperty()
        {
            RecalculateSize();
        }
        public GpkByteProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} NameValue: {2} NameValue: {3}", name, type, nameValue, byteValue);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            if (size == 8)
            {
                writer.Write(package.GetStringIndex(nameValue));
            }
            else
            {
                writer.Write(byteValue);
            }
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            if (size == 8)
            {
                long byteIndex = reader.ReadInt64();
                nameValue = package.GetString(byteIndex);
            }
            else
            {
                byteValue = reader.ReadByte();
            }
        }

        public int RecalculateSize()
        {
            if (nameValue != null)
            {
                size = 8;
            }
            else
            {
                size = 1;
            }
            return size;
        }
    }

}
