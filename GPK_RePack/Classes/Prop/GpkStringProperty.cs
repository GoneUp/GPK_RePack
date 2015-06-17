using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkStringProperty : GpkBaseProperty , IProperty 
    {
        public int length;
        public string value;

        public bool IsUnicode = false;

        public GpkStringProperty()
        {
            RecalculateSize();
        }
        public GpkStringProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            throw new NotImplementedException();
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            length = reader.ReadInt32(); //inner len

            if (length > 0)
            {
                value = Reader.ReadString(reader, length);
            }
            else
            {
                //unicode :O
                value = Reader.ReadUnicodeString(reader, (length * -1) * 2);
            }
        }

        public int RecalculateSize()
        {
            int tmpSize = 4; //length
            if (value != null)
            {
                tmpSize += value.Length;
            }
            size = tmpSize;
            return size;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, value);
        }
    }

}
