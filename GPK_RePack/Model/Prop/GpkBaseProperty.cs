using System;

namespace GPK_RePack.Model.Prop
{
    [Serializable]
    class GpkBaseProperty 
    {
        public string name; //long index
        public string type; //long index
        public int size;
        public int arrayIndex;

        public GpkBaseProperty()
        {
        }

        public GpkBaseProperty(string tmpName, string tmpType, int tmpSize, int tmpAIndex)
        {
            name = tmpName;
            type = tmpType;
            size = tmpSize;
            arrayIndex = tmpAIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1}", name, type);
        }
    }
}
