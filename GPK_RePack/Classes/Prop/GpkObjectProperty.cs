using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkObjectProperty : GpkBaseProperty
    {
        public int value; //long index
        public string ClassName;

        public GpkObjectProperty()
        {

        }
        public GpkObjectProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Value: {2}", name, type, ClassName);
        }
    }

}
