using System;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model
{
    [Serializable]
    class GpkGeneration : IGpkPart
    {
        public int ExportCount;
        public int NameCount;
        public int NetObjectCount;

        public int GetSize()
        {
            return 12;
        }
    }
}
