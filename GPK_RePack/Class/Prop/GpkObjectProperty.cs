namespace GPK_RePack.Class.Prop
{
    class GpkObjectProperty : GpkBaseProperty
    {
        public long unk;
        public long value; //long index
        public string ClassName;

        public GpkObjectProperty()
        {

        }
        public GpkObjectProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }
    }

}
