using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkObjectProperty : GpkBaseProperty
    {
        public long unk;
        public int value; //long index
        public string ClassName;

        public GpkObjectProperty()
        {

        }
        public GpkObjectProperty(GpkBaseProperty bp)
        {
            Name = bp.Name;
            type = bp.type;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", Name, type, ClassName);
        }
    }

}
