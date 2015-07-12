using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;
using GPK_RePack.Saver;

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

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            writer.Write(length);

            if (length > 0)
            {
                Save.WriteString(writer, value);
            }
            else
            {
                Save.WriteUnicodeString(writer, value);
            }
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
            if (length > 0)
            {
                tmpSize += length;
            }
            else
            {
                //unicode :O
                tmpSize += (length*-1) * 2;
            }
            size = tmpSize;
            return size;
        }

        public bool ValidateValue(string input)
        {
            return true;
        }

        public bool SetValue(string input)
        {
            value = input;
            return true;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, value);
        }
    }

}
