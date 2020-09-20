using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model.Prop;
using Ionic.Zip;
using Ionic.Zlib;
using Lzo64;
using NLog;


namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class MipMap
    {
        public int sizeX;
        public int sizeY;

        public int flags;
        public int uncompressedSize;
        public int compChunkSize;
        public int compChunkOffset;

        public int signature = Constants.DEFAULT_SIGNATURE;
        public int blocksize = Constants.DEFAULT_BLOCKSIZE;

        public int compressedSize;
        public int uncompressedSize_chunkheader;

        public byte[] uncompressedData;

        public GpkNameProperty textureCacheProp;
        public string textureCachePath = "";

        public List<ChunkBlock> blocks = new List<ChunkBlock>();
        internal int blockCount;

        public void generateBlocks()
        {
            blocks.Clear();
            int blockCount = (uncompressedSize + blocksize - 1) / blocksize;

            compressedSize = 0;
            for (int i = 0; i < blockCount; i++)
            {
                //if data was not found, skip
                if (uncompressedData == null)
                    continue;

                int blockOffset = i * blocksize;
                int blockEnd = blockOffset + blocksize;
                if (blockEnd > uncompressedSize)
                    blockEnd = uncompressedSize;

                var block = new ChunkBlock();
                block.uncompressedDataSize = blockEnd - blockOffset;
                block.uncompressedData = new byte[block.uncompressedDataSize];
                Array.ConstrainedCopy(uncompressedData, blockOffset, block.uncompressedData, 0, block.uncompressedData.Length);

                block.compressTextureFlags(flags);

                compressedSize += block.compressedSize;
                blocks.Add(block);
            }
        }


        public override string ToString()
        {
            CompressionTypes NoOp = CompressionTypes.Unused | CompressionTypes.StoreInSeparatefile;

            StringBuilder info = new StringBuilder();
            info.AppendFormat("Size: {0} x {1} {2}", sizeX, sizeY, Environment.NewLine);
            info.AppendLine("Compression: " + flags);

            if (((CompressionTypes)flags & CompressionTypes.StoreInSeparatefile) != 0)
            {

                if (textureCacheProp != null && textureCachePath != null)
                {
                    info.AppendLine(String.Format("Data for this MipMap is stored in tfc {0} @ {1}", textureCacheProp.value, textureCachePath));
                }
                else
                {
                    info.AppendLine("Data for this MipMap is stored in tfc");
                }

                if (compressedSize == 0)
                    info.AppendLine("TFC data was not found. Searched for tfc file at " + textureCachePath);

            }
            else if (((CompressionTypes)flags & NoOp) != 0)
            {
                info.AppendLine("Data for this MipMap is stored external!");
            }



            info.AppendLine("Compressed Size: " + compressedSize);
            info.AppendLine("Uncompressed Size: " + uncompressedSize);
            info.AppendLine("Blocks: " + blocks.Count);
            blocks.ForEach(b => info.AppendFormat("Block: Uncompressed Size: {0}, Compressed Size: {1} {2}", b.uncompressedDataSize, b.compressedSize, Environment.NewLine));
            return info.ToString();
        }
    }
}

