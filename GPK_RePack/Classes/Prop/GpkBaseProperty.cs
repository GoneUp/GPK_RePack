using System;

namespace GPK_RePack.Classes.Prop
{
    [Serializable]
    class GpkBaseProperty
    {
        public string Name; //long index

        public string type; //long index

        public object value;

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1}", Name, type);
        }
    }
}
