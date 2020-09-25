using System;
using System.IO;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model.Prop
{
    [Serializable]
    public class GpkObjectProperty : GpkBaseProperty, IProperty
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
            writer.Write(package.GetObjectIndex(objectName));
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            value = reader.ReadInt32();
            objectName = package.GetObjectName(value);
        }

        public int RecalculateSize()
        {
            size = 4;
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

        public void CheckAndAddNames(GpkPackage package)
        {
           
        }
    }

}
