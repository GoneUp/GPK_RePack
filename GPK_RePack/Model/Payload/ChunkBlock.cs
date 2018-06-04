using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model.Payload;
using Ionic.Zlib;
using Lzo64;
using NLog;

namespace GPK_RePack.Model
{

    [Serializable]
    class ChunkBlock
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static LZOCompressor lzo = new LZOCompressor();
        private static object lockObject = new Object();

        public int compressedSize;
        public int uncompressedDataSize;

        public byte[] compressedData;
        public byte[] uncompressedData;

        public void decompress(int compFlag)
        {

            if (compFlag == 0)
            {
                //uncompressed
                uncompressedData = (byte[])(compressedData.Clone());
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.ZLIB) > 0)
            {
                try
                {
                    var byteStream = new MemoryStream();
                    var outStream = new ZlibStream(byteStream, CompressionMode.Decompress);
                    outStream.Write(compressedData, 0, compressedData.Length);
                    uncompressedData = byteStream.GetBuffer();

                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZO) > 0)
            {
                uncompressedData = new byte[uncompressedDataSize];
                lock (lockObject)
                {
                    lzo.Decompress(compressedData, uncompressedData, uncompressedDataSize);
                }
            }

            if (uncompressedData != null)
            {
                uncompressedDataSize = uncompressedData.Length;
                compressedData = null; //save memory
            }
        }

        public void compress(int compFlag)
        {
            if (compFlag == 0)
            {
                //uncompressed
                compressedData = (byte[])(uncompressedData.Clone());
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.ZLIB) > 0)
            {
                try
                {
                    var byteStream = new MemoryStream();
                    var outStream = new ZlibStream(byteStream, CompressionMode.Compress);
                    outStream.Write(uncompressedData, 0, uncompressedData.Length);
                    compressedData = byteStream.GetBuffer();

                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZO) > 0)
            {
                lock (lockObject)
                {
                    compressedData = lzo.Compress(uncompressedData, false);
                }
            }

            if (compressedData != null)
            {
                compressedSize = compressedData.Length;
            }
        }
    }
}
