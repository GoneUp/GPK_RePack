namespace GPK_RePack.Class.Prop
{
    class GpkStructProperty : GpkBaseProperty
    {
        public string innerType;
        public long length;
        public byte[] value;

        public GpkStructProperty()
        {

        }
        public GpkStructProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("Name: {0} Type: {1} Length: {2} Value: {3}", Name, type, length, value);
        }
    }

}
