using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkNameProperty : GpkBaseProperty
    {
        public long unk;
        public string value; //long index
        public int padding;

        public GpkNameProperty()
        {

        }
        public GpkNameProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", Name, type, value);
        }
    }

}
