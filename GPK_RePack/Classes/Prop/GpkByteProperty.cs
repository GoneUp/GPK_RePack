using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkByteProperty : GpkBaseProperty
    {
        public string nameValue; //long index
        public byte byteValue;

        public GpkByteProperty()
        {

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
    }

}
