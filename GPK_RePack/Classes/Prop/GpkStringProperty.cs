using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkStringProperty : GpkBaseProperty 
    {
        public int length;
        public string value;

        public bool IsUnicode = false;

        public GpkStringProperty()
        {
            
        }
        public GpkStringProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, value);
        }
    }

}
