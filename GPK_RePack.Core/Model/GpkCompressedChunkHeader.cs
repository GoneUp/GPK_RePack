using System;
using GPK_RePack.Core.Model.Compression;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model
{
    [Serializable]
    public class GpkCompressedChunkHeader : IGpkPart
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
