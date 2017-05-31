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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public int sizeX;
        public int sizeY;

        public int compFlag;
        public int uncompressedSize;
        public int compChunkSize;
        public int compChunkOffset;

        public int signature;
        public int blocksize;

        public int compressedSize;
        public int uncompressedSize_chunkheader;

        public byte[] compressedData;
        public byte[] uncompressedData;



      

        public void compress()
        {

        }



        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("Size: {0} x {1} {2}", sizeX, sizeY, Environment.NewLine);
            info.AppendLine("Compression: " + compFlag);
            info.AppendLine("Compressed Size: " + compressedSize);
            info.AppendLine("Uncompressed Size: " + uncompressedSize);
            return info.ToString();
        }
    }
}

