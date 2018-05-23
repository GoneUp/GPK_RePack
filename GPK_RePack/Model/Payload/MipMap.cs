using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using Lzo64;
using NLog;


namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class MipMap
    {
        public static int DEFAULT_BLOCKSIZE = 131072;
        public static uint DEFAULT_SIGNATURE = 0x9e2a83c1;

        public int sizeX;
        public int sizeY;

        public int compFlag;
        public int uncompressedSize;
        public int compChunkSize;
        public int compChunkOffset;

        public uint signature = DEFAULT_SIGNATURE;
        public int blocksize = DEFAULT_BLOCKSIZE;

        public int compressedSize;
        public int uncompressedSize_chunkheader;

        public byte[] uncompressedData;

        public List<ChunkBlock> blocks = new List<ChunkBlock>();

        public void generateBlocks()
        {
            blocks.Clear();
            int blockCount = (uncompressedSize + blocksize - 1) / blocksize;

            compressedSize = 0;
            for (int i = 0; i < blockCount; i++)
            {
                int blockOffset = i * blocksize;
                int blockEnd = blockOffset + blocksize;
                if (blockEnd > uncompressedSize)
                    blockEnd = uncompressedSize;

                var block = new ChunkBlock();
                block.uncompressedDataSize = blockEnd - blockOffset;
                block.uncompressedData = new byte[block.uncompressedDataSize];
                Array.ConstrainedCopy(uncompressedData, blockOffset, block.uncompressedData, 0, block.uncompressedData.Length);

                block.compress(compFlag);

                compressedSize += block.compressedSize;
                blocks.Add(block);
            }
        }


        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("Size: {0} x {1} {2}", sizeX, sizeY, Environment.NewLine);
            info.AppendLine("Compression: " + compFlag);
            info.AppendLine("Compressed Size: " + compressedSize);
            info.AppendLine("Uncompressed Size: " + uncompressedSize);
            info.AppendLine("Blocks: " + blocks.Count);
            blocks.ForEach(b => info.AppendFormat("Block: Uncompressed Size: {0}, Compressed Size: {1} {2}", b.uncompressedDataSize, b.compressedSize, Environment.NewLine));
            return info.ToString();
        }
    }
}

