using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZ4;
using NLog;

namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class MipMap
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static int COMPRESS_ZLIB = 1;
        public static int COMPRESS_LZO = 2;
        public static int COMPRESS_LZX = 4;

        public int sizeX;
        public int sizeY;

        public int compFlag;
        public int uncompressedSize;
        public int widthOffset;

        public int unk1;
        public int unk2;

        public int requiredBuffer;
        public int compressedSize;
        public int uncompressedDataSize;


        public byte[] compressedData;
        public byte[] uncompressedData;



        public void decompress()
        {
            if ((compFlag & COMPRESS_ZLIB) > 0)
            {
                logger.Error("Found COMPRESS_ZLIB, unsupported!");
            }
            else if ((compFlag & COMPRESS_LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if ((compFlag & COMPRESS_LZO) > 0)
            {
                uncompressedData = LZ4Codec.Unwrap(compressedData);
                logger.Debug("Extracted size " + uncompressedData.Length);
            }

        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("Size: {0} x {1}\n", sizeX, sizeY);
            info.AppendFormat("Compression: {0}\n", compFlag);
            info.AppendFormat("Compressed Size: {0}\n", compressedSize);
            info.AppendFormat("Uncompressed Size: {0}\n", uncompressedSize);
            return info.ToString();
        }
    }
}

