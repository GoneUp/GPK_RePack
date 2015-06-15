using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkStructProperty : GpkBaseProperty
    {
        public string innerType;
        public byte[] value;

        public GpkStructProperty()
        {

        }
        public GpkStructProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Length: {2}", name, type, size);
        }

        public string GetValueHex()
        {
            string hex = "";
            if (value != null) hex = BitConverter.ToString(value);
            return hex;
        }
    }

}
