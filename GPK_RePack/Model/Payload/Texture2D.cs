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
    [Serializable]
    class Texture2D : IPayload
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GpkExport objectExport;
        public byte[] startUnk;
        public string tgaPath;
        public bool inUnicode = false;
        public byte[] guid;

        private const CompressionTypes NothingToDo = CompressionTypes.Unused | CompressionTypes.StoreInSeparatefile;

        public List<MipMap> maps = new List<MipMap>();

        public string GetClassIdent()
        {
            return "Core.Texture2D";
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            writer.Write(startUnk);
            if (inUnicode)
            {
                Writer.WriteUnicodeString(writer, tgaPath, true);
            }
            else
            {
                Writer.WriteString(writer, tgaPath, true);
            }

            writer.Write(maps.Count);

            foreach (var map in maps)
            {
                //refressh block info, compress blocks
                //map.generateBlocks();

                //chunk
                //info
                writer.Write(map.compFlag);
                writer.Write(map.uncompressedSize);

                if (((CompressionTypes)map.compFlag & NothingToDo) == 0)
                {
                    int chunkSize = 16 + map.blocks.Count * 8 + map.compressedSize;
                    if (chunkSize != map.compChunkSize)
                    {
                        logger.Info("fixing chunksize for " + objectExport.ObjectName);
                        map.compChunkSize = chunkSize;
                    }


                    writer.Write(map.compChunkSize);
                    writer.Write((int)(writer.BaseStream.Position + 4)); //chunkoffset

                    if (map.compFlag == 0)
                    {
                        //uncompressed
                        writer.Write(map.uncompressedData);

                    } else {
                        //compressed data
                        //header
                        writer.Write(map.signature);
                        writer.Write(map.blocksize);
                        writer.Write(map.compressedSize);
                        writer.Write(map.uncompressedSize_chunkheader);

                        foreach (var block in map.blocks)
                        {
                            writer.Write(block.compressedSize);
                            writer.Write(block.uncompressedDataSize);
                        }

                        foreach (var block in map.blocks)
                        {
                            writer.Write(block.compressedData);
                        }

                    }

                }
                else
                {
                    writer.Write((int)-1); //chunksize
                    writer.Write((int)-1); //chunkoffset
                    logger.Trace("writing {0}, MipMap {0}, with no data!!", export.ObjectName, map);
                }

                writer.Write(map.sizeX);
                writer.Write(map.sizeY);
            }

            writer.Write(guid);
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
                var temp = ((CompressionTypes)map.compFlag & NothingToDo);

                if (map.compFlag == 0 )
                {
                    map.uncompressedData = reader.ReadBytes(map.uncompressedSize);
                }
                else if (((CompressionTypes)map.compFlag & NothingToDo) == 0)
                {
                    //header
                    map.signature = reader.ReadUInt32(); //0x9e2a83c1
                    Debug.Assert(map.signature == MipMap.DEFAULT_SIGNATURE);

                    map.blocksize = reader.ReadInt32();

                    map.compressedSize = reader.ReadInt32();
                    map.uncompressedSize_chunkheader = reader.ReadInt32();
                    map.uncompressedData = new byte[map.uncompressedSize];

                    int blockCount = (map.uncompressedSize + map.blocksize - 1) / map.blocksize;
                    int blockOffset = 0;


                    for (int j = 0; j < blockCount; ++j)
                    {
                        var block = new ChunkBlock();
                        block.compressedSize = reader.ReadInt32();
                        block.uncompressedDataSize = reader.ReadInt32();

                        map.blocks.Add(block);
                    }


                    foreach (ChunkBlock block in map.blocks)
                    {
                        block.compressedData = reader.ReadBytes(block.compressedSize);
                        block.decompressTextureFlags(map.compFlag);

                        Array.ConstrainedCopy(block.uncompressedData, 0, map.uncompressedData, blockOffset, block.uncompressedDataSize);
                        blockOffset += block.uncompressedDataSize;

                        //save memory
                        block.uncompressedData = null;
                    }


                }
                else
                {
                    logger.Trace("{0}, MipMap {0}, no data!!", export.ObjectName, i);
                }

                map.sizeX = reader.ReadInt32();
                map.sizeY = reader.ReadInt32();


                maps.Add(map);
            }

            guid = reader.ReadBytes(16);
        }




        public int GetSize()
        {
            int tmpSize = 16;
            tmpSize += Writer.GetStringBytes(tgaPath, inUnicode);
            tmpSize += 4;

            foreach (var map in maps)
            {
                //header
                tmpSize += 32;
                tmpSize += map.blocks.Count * 8 + map.compressedSize;
                //sizex, sizey
                tmpSize += 8;
            }

            //guid
            tmpSize += 16;
            return tmpSize;
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
