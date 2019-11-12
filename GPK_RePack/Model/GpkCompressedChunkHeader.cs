using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model
{

    class GpkCompressedChunkHeader : IGpkPart
    {
        public int GetSize()
        {
            throw new NotImplementedException();
        }


        public int UncompressedOffset;
        public int UncompressedSize;
        public int CompressedOffset;
        public int CompressedSize;
    }
}
