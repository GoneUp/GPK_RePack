using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
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
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", Name, type, value);
        }
    }

}
