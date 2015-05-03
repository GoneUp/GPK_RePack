namespace GPK_RePack.Class.Prop
{
    class GpkBaseProperty
    {
        public long NameIndex;
        public string Name;

        public PropertyTypes type; //long
    }

    enum PropertyTypes
    {
        IntProp = 28,
        NameProp = 4,
        StringProp = 45,
        BoolProp = 1,
        ArrayProp = 0
    }
}
