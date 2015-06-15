using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkBaseProperty
    {
        public string name; //long index
        public string type; //long index
        public int size;
        public int arrayIndex;
        public object value;

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1}", name, type);
        }
    }
}
