namespace GPK_RePack.Class.Prop
{
    class GpkBoolProperty : GpkBaseProperty
    {
        public long unk;
        public bool value;

        public GpkBoolProperty()
        {

        }
        public GpkBoolProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("Name: {0} Type: {1} Value: {2}", Name, type, value);
        }
    }

}
