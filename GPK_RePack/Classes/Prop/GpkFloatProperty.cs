using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkFloatProperty : GpkBaseProperty, IProperty
    {
        public float value;

        public GpkFloatProperty()
        {
            RecalculateSize();
        }
        public GpkFloatProperty(GpkBaseProperty bp)
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
            writer.Write(value);
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            value = reader.ReadSingle();
        }

        public int RecalculateSize()
        {
            size = 4;
            return size;
        }

        public bool ValidateValue(string input)
        {
            float validate;
            if (float.TryParse(input, out validate))
            {
                return true;
            }

            return false;
        }

        public bool SetValue(string input)
        {
            if (!ValidateValue(input)) return false;
            value = Convert.ToSingle(input);
            return true;
        }
    }

}
