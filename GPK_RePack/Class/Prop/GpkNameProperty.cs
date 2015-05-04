namespace GPK_RePack.Class.Prop
{
    class GpkNameProperty : GpkBaseProperty
    {
        public long unk;
        public string value; //long index

        public GpkNameProperty()
        {

        }
        public GpkNameProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }
    }

}
