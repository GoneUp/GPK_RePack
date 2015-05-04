namespace GPK_RePack.Class.Prop
{
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
    }

}
