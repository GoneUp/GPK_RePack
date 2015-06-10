using System;

namespace GPK_RePack.Class.Prop
{
    [Serializable]
    class GpkArrayProperty : GpkBaseProperty
    {
        public long length;
        public byte[] value;

        public GpkArrayProperty()
        {

        }
        public GpkArrayProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Length: {2}", Name, type, length);
        }
    }

}
