using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.IO;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Prop;
using Ionic.Zlib;
using Lzo64;
using NLog;
using UpkManager.Dds;
using UpkManager.Dds.Constants;

namespace GPK_RePack.Model.Payload
{

    class ChunkBlock
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                // Create the compressor object
                LZOCompressor lzo = new LZOCompressor();

                uncompressedData = new byte[uncompressedDataSize];
                lzo.Decompress(compressedData, uncompressedData, uncompressedDataSize);
            }
        }
    }

    [Serializable]
    class Texture2D : IPayload
    {

        public GpkExport objectExport;
        public byte[] startUnk;
        public String tgaPath;
        public bool inUnicode = false;
        public byte[] endUnk;

        private const CompressionTypes NothingToDo = CompressionTypes.Unused | CompressionTypes.StoreInSeparatefile;

        public List<MipMap> maps = new List<MipMap>();

        public string GetClassIdent()
        {
            return "Core.Texture2D";
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            throw new NotImplementedException();
        }

        public void ReadData(GpkPackage package, GpkExport export)
        {
            objectExport = export;
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));
            IProperty formatProp = export.Properties.Find(t => ((GpkBaseProperty)t).name == "Format");
            String format = ((GpkByteProperty)formatProp).nameValue;

            startUnk = reader.ReadBytes(16);
            int length = reader.ReadInt32();
            if (length > 0)
            {
                tgaPath = Reader.ReadString(reader, length);
            }
            else
            {
                inUnicode = true;
                tgaPath = Reader.ReadUnicodeString(reader, (length * -1) * 2);
            }


            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                MipMap map = new MipMap();

                //chunk
                //info
                map.compFlag = reader.ReadInt32();

                map.uncompressedSize = reader.ReadInt32();
                map.compChunkSize = reader.ReadInt32();
                map.compChunkOffset = reader.ReadInt32();

                if (((CompressionTypes)map.compFlag & NothingToDo) == 0)
                {
                    //header
                    map.signature = reader.ReadInt32(); //0x9e2a83c1
                    Debug.Assert((uint)map.signature == 0x9e2a83c1);

                    map.blocksize = reader.ReadInt32();

                    map.compressedSize = reader.ReadInt32();
                    map.uncompressedSize_chunkheader = reader.ReadInt32();
                    map.uncompressedData = new byte[map.uncompressedSize];

                    List<ChunkBlock> blocks = new List<ChunkBlock>();
                    int blockCount = (map.uncompressedSize + map.blocksize - 1) / map.blocksize;
                    int blockOffset = 0;

                    if (blockCount > 1)
                        Debug.Print("");



                    for (int j = 0; j < blockCount; ++j)
                    {
                        var block = new ChunkBlock();
                        block.compressedSize = reader.ReadInt32();
                        block.uncompressedDataSize = reader.ReadInt32();

                        blocks.Add(block);
                    }


                    foreach (ChunkBlock block in blocks)
                    {
                        block.compressedData = reader.ReadBytes(block.compressedSize);
                        block.decompress(map.compFlag);

                        Array.ConstrainedCopy(block.uncompressedData, 0, map.uncompressedData, blockOffset, block.uncompressedDataSize);
                        blockOffset += block.uncompressedDataSize;
                    }

      
                }
                else
                {
                    Debug.Print("");
                }

                map.sizeX = reader.ReadInt32();
                map.sizeY = reader.ReadInt32();


                maps.Add(map);
            }

            endUnk = reader.ReadBytes(16);
        }




        public int GetSize()
        {
            throw new NotImplementedException();
        }


        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Tga path: " + tgaPath);
            info.AppendLine("Mipmaps: " + maps.Count);

            for (int i = 0; i < maps.Count; i++)
            {
                info.AppendLine("===================================");
                info.AppendLine("Mipmap " + i);
                info.Append(maps[i]);
            }
            return info.ToString();
        }


        public FileFormat GetFormat()
        {
            GpkByteProperty formatProp = objectExport.GetProperty("Format") as GpkByteProperty;
            if (formatProp == null) return FileFormat.Unknown;

            string format = formatProp.nameValue;

            return DdsPixelFormat.ParseFileFormat(format);
        }

        public Stream GetObjectStream()
        {
            if (maps == null || !maps.Any()) return null;

            FileFormat format;

            MipMap mipMap = maps.Where(mm => mm.uncompressedData != null && mm.uncompressedData.Length > 0).OrderByDescending(mm => mm.sizeX > mm.sizeY ? mm.sizeX : mm.sizeY).FirstOrDefault();

            return mipMap == null ? null : buildDdsImage(maps.IndexOf(mipMap), out format);
        }

        private Stream buildDdsImage(int mipMapIndex, out FileFormat imageFormat)
        {
            MipMap mipMap = maps[mipMapIndex];

            imageFormat = GetFormat();
            DdsHeader ddsHeader = new DdsHeader(new DdsSaveConfig(imageFormat, 0, 0, false, false), mipMap.sizeX, mipMap.sizeY);

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            ddsHeader.Write(writer);
            stream.Write(mipMap.uncompressedData, 0, mipMap.uncompressedData.Length);
            stream.Flush();
            stream.Position = 0;

            return stream;
        }

        public void SaveObject(string filename, object configuration)
        {
            if (maps == null || !maps.Any()) return;

            DdsSaveConfig config = configuration as DdsSaveConfig ?? new DdsSaveConfig(FileFormat.Unknown, 0, 0, false, false);

            FileFormat format;

            MipMap mipMap = maps.Where(mm => mm.uncompressedData != null && mm.uncompressedData.Length > 0).OrderByDescending(mm => mm.sizeX > mm.sizeY ? mm.sizeX : mm.sizeY).FirstOrDefault();

            if (mipMap == null) return;

            Stream memory = buildDdsImage(maps.IndexOf(mipMap), out format);

            if (memory == null) return;

            DdsFile ddsImage = new DdsFile(GetObjectStream());

            FileStream ddsStream = new FileStream(filename, FileMode.Create);

            config.FileFormat = format;

            ddsImage.Save(ddsStream, config);

            ddsStream.Close();

            memory.Close();
        }

    }
}
