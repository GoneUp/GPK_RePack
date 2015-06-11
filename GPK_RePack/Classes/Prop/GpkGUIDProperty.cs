using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkGUIDProperty : GpkBaseProperty
    {
        public long length;
        public long unk;
        public byte[] value;

        public GpkGUIDProperty()
        {

        }
        public GpkGUIDProperty(GpkBaseProperty bp)
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
