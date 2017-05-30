using System;
using System.IO;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model.Prop
{
    [Serializable]
    class GpkNameProperty : GpkBaseProperty, IProperty
    {
        public string value; //long index
        public int padding;

        public GpkNameProperty()
        {
            RecalculateSize();
        }
        public GpkNameProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, value);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            writer.Write((int)package.GetStringIndex(value));
            writer.Write(padding); 
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            long index = reader.ReadInt32();
            value = package.GetString(index);
            padding = reader.ReadInt32();
        }

        public int RecalculateSize()
        {
            size = 8;
            return size;
        }

        public bool ValidateValue(string input)
        {
            return false;
        }

        public bool SetValue(string input)
        {
            return false;
        }
    }

}
