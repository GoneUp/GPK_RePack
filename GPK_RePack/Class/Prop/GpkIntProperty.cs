namespace GPK_RePack.Class.Prop
{
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
    }

}
