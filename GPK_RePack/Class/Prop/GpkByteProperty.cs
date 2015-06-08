namespace GPK_RePack.Class.Prop
{
    class GpkByteProperty : GpkBaseProperty
    {
        public int size;
        public int arrayIndex;
        public string nameValue; //long index
        public byte byteValue;

        public GpkByteProperty()
        {

        }
        public GpkByteProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("Name: {0} Type: {1} NameValue: {2} NameValue: {3}", Name, type, nameValue, byteValue);
        }
    }

}
