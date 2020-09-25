using System;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model
{
    [Serializable]
    public class GpkGeneration : IGpkPart
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
