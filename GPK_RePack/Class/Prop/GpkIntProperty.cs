using System;

namespace GPK_RePack.Class.Prop
{
    [Serializable]
    class GpkIntProperty : GpkBaseProperty
    {
        public long unk;
        public int value;

        public GpkIntProperty()
        {

        }
        public GpkIntProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", Name, type, value);
        }
    }

}
