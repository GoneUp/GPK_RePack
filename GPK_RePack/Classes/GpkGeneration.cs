using System;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes
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
