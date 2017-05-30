using System;
using System.IO;
using GPK_RePack.IO;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model.Prop
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
                Writer.WriteString(writer, value);
            }
            else
            {
                Writer.WriteUnicodeString(writer, value);
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
                IsUnicode = true;
                value = Reader.ReadUnicodeString(reader, (length * -1) * 2);
            }
        }

        public int RecalculateSize()
        {
            length = value.Length + 1;
            size = length;

            if (IsUnicode)
            {
                //length in file format, unicode is marked with a negative value
                length *= -1;
                size *= 2;
            }

            size+= 4; //header len
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
