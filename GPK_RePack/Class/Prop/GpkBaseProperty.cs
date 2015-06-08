namespace GPK_RePack.Class.Prop
{
    class GpkBaseProperty
    {
        public string Name; //long index

        public string type; //long index

        public object value;

        public override string ToString()
        {
            return string.Format("Name: {0} Type: {1}", Name, type);
        }
    }
}
