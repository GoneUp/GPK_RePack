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


        //this solution sucks, too much duplicate code
        public void decompressPackageFlags(int compFlag)
        {
            if (compFlag == 0)
            {
                //uncompressed
                uncompressedData = (byte[])(compressedData.Clone());
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.ZLIB) > 0)
            {
                decompressZLIB();
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.LZO) > 0)
            {
                decompressLZO();
            }

            if (uncompressedData != null)
            {
                uncompressedDataSize = uncompressedData.Length;
            }
        }

        public void decompressTextureFlags(int compFlag)
        {
            if (compFlag == 0)
            {
                //uncompressed
                uncompressedData = (byte[])(compressedData.Clone());
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.ZLIB) > 0)
            {
                decompressZLIB();
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZO) > 0)
            {
                decompressLZO();
            }

            if (uncompressedData != null)
            {
                uncompressedDataSize = uncompressedData.Length;
            }
        }

        private void decompressZLIB()
        {
            try
            {
                var byteStream = new MemoryStream();
                var outStream = new ZlibStream(byteStream, CompressionMode.Decompress);
                outStream.Write(compressedData, 0, compressedData.Length);
                uncompressedData = byteStream.ToArray();

            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private void decompressLZO()
        {
            uncompressedData = new byte[uncompressedDataSize];
            lock (lockObject)
            {
                lzo.Decompress(compressedData, uncompressedData, uncompressedDataSize);
            }
        }


        public void compressTextureFlags(int compFlag)
        {
            if (compFlag == 0)
            {
                //uncompressed
                compressedData = (byte[])(uncompressedData.Clone());
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.ZLIB) > 0)
            {
                compressZLIB();
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypes)compFlag & CompressionTypes.LZO) > 0)
            {
                compressLZO();
            }

            compressedSize = compressedData.Length;
        }

        public void compressPackageFlags(int compFlag)
        {
            if (compFlag == 0)
            {
                //uncompressed
                compressedData = (byte[])(uncompressedData.Clone());
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.ZLIB) > 0)
            {
                compressZLIB();
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.LZX) > 0)
            {
                logger.Error("Found COMPRESS_LZX, unsupported!");
            }
            else if (((CompressionTypesPackage)compFlag & CompressionTypesPackage.LZO) > 0)
            {
                compressLZO();
            }

            compressedSize = compressedData.Length;
        }

        private void compressZLIB()
        {
            try
            {
                var byteStream = new MemoryStream();
                var outStream = new ZlibStream(byteStream, CompressionMode.Compress);
                outStream.Write(uncompressedData, 0, uncompressedData.Length);
                compressedData = byteStream.ToArray();

            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private void compressLZO()
        {
            lock (lockObject)
            {
                compressedData = lzo.Compress(uncompressedData, false);
            }
        }
    }
}
