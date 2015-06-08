namespace GPK_RePack.Class.Prop
{
    class GpkStringProperty : GpkBaseProperty 
    {
        public long unk;
        public int length;
        public string value;

        public bool IsUnicode = false;

        public GpkStringProperty()
        {
            
        }
        public GpkStringProperty(GpkBaseProperty bp)
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
