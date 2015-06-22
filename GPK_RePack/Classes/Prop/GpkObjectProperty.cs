using System;
using System.IO;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkObjectProperty : GpkBaseProperty, IProperty
    {
        public int value; //long index
        public string objectName;

        public GpkObjectProperty()
        {
            RecalculateSize();
        }
        public GpkObjectProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, objectName);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            writer.Write((int)package.GetObjectIndex(objectName));
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            value = reader.ReadInt32();
            objectName = Reader.GetObject(value, package);
        }

        public int RecalculateSize()
        {
            size = 4;
            return size;
        }
    }

}
