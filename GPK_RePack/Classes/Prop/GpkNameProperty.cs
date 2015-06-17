using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Prop
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

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            throw new NotImplementedException();
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            long index = reader.ReadInt32();
            value = package.NameList[index].name;
            padding = reader.ReadInt32();
        }

        public int RecalculateSize()
        {
            size = 8;
            return size;
        }
    }

}
