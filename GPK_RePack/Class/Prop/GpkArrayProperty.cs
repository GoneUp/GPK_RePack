namespace GPK_RePack.Class.Prop
{
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
            return string.Format("Name: {0} Type: {1} Length: {2}", Name, type, length);
        }
    }

}
