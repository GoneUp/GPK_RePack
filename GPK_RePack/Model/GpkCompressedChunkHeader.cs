using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Payload;

namespace GPK_RePack.Model
{
    [Serializable]
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
        //CompressedSize inlcudes the chunkheader. +16 bytes!

        public PackageChunkBlock writableChunkblock;
    }

    
}
