namespace GPK_RePack.Class.Prop
{
    class GpkFloatProperty : GpkBaseProperty
    {
        public long unk;
        public float value;

        public GpkFloatProperty()
        {

        }
        public GpkFloatProperty(GpkBaseProperty bp)
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
