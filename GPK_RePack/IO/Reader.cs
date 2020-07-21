using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GPK_RePack.Model;
using GPK_RePack.Model.ExportData;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Payload;
using GPK_RePack.Model.Prop;
using GPK_RePack.Properties;
using NLog;

namespace GPK_RePack.IO
{
    class Reader : IProgress
    {
        private Logger logger;
        public Status stat;

        //Read gpk, possible composite gpk
        //Sane way: read header loop, map existing gpks, extract gpks, pass to parse seprate method

        //Read specific sub-gpk --> extract bytes, pass to parse seprate method

        public List<GpkPackage> ReadGpk(string path, bool skipExportData)
        {
            try
            {
                Stopwatch fileWatch = new Stopwatch();
                List<GpkPackage> tmpPackageList = new List<GpkPackage>();
                BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));

                //orginalsize len file oder len sub gpk
                //

                fileWatch.Start();
                do
                {
                    GpkPackage tmpGPK = new GpkPackage();

                    var fileID = string.Format("{0}_{1}", Path.GetFileName(path), reader.BaseStream.Position);
                    logger = LogManager.GetLogger("[Reader:" + fileID + "]");
                    logger.Debug("Reading Start");

                    tmpGPK.Filename = fileID;
                    tmpGPK.Path = path;

                    tmpGPK.OrginalSize = reader.BaseStream.Length;
                    tmpGPK.CompositeStartOffset = reader.BaseStream.Position;

                    //parsing
                    //just read header to get length and end of file
                    ReadHeader(reader, tmpGPK);
                    tmpPackageList.Add(tmpGPK);

                    //if no compression is found we don't know EndOfFile, so we assume the gpk covers the complete file
                    if (tmpGPK.CompressedEndOffset == 0)
                    {
                        tmpGPK.CompressedEndOffset = tmpGPK.OrginalSize;
                    }

                    //prepare next loop
                    if (tmpGPK.CompressedEndOffset == tmpGPK.OrginalSize)
                        break; //no valid end of data found

                    //we have a composite gpk but we still want a correct compressed filesize
                    tmpGPK.OrginalSize = tmpGPK.CompressedEndOffset;

                    reader.BaseStream.Seek(tmpGPK.CompositeStartOffset + tmpGPK.OrginalSize, SeekOrigin.Begin);
                } while (CheckForAnotherPackage(reader));



                //header is read, let us extract the data and read it
                List<GpkPackage> returnPackageList = new List<GpkPackage>();
                stat = new Status();
                stat.subGpkCount = tmpPackageList.Count;

                foreach (var subGPK in tmpPackageList)
                {
                    
                    reader.BaseStream.Seek(subGPK.CompositeStartOffset, SeekOrigin.Begin);
                    var data = reader.ReadBytes((int)subGPK.OrginalSize);
                    var fullGpk = ReadSubGpkPackage(subGPK, data, skipExportData, stat);
                    returnPackageList.Add(fullGpk);

                    stat.subGpkDone++;
                }

                reader.Close();
                fileWatch.Stop();
                logger.Info("Reading of file {0} complete, took {1}ms!", path, fileWatch.ElapsedMilliseconds);

                if (returnPackageList.Count > 1)
                {
                    returnPackageList.ForEach(val => val.CompositeGpk = true);
                }
                return tmpPackageList;
            }
            catch (Exception ex)
            {
                logger.Fatal("Parse failure!");
                logger.Fatal(ex);
            }

            return null;
        }

        public GpkPackage ReadSubGpkFromComposite(string path, string fileID, int fileOffset, int dataLength)
        {
            BinaryReader reader = null;

            try
            {
                logger = LogManager.GetLogger("ReadSubGpkFromComposite: " + fileID);

                reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
                stat = new Status();

                reader.BaseStream.Seek(fileOffset, SeekOrigin.Begin);
                var data = reader.ReadBytes(dataLength);
                reader.Close();
                reader.Dispose();

                GpkPackage tmpGPK = new GpkPackage();
                tmpGPK.Filename = fileID;
                tmpGPK.Path = path;

                tmpGPK.OrginalSize = dataLength;
                tmpGPK.CompositeStartOffset = fileOffset;


                var fullGpk = ReadSubGpkPackage(tmpGPK, data, false, stat);

                logger.Debug("Done");

                return fullGpk;
            }
            catch (Exception ex)
            {
                logger.Fatal("Parse failure!");
                logger.Fatal(ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return null;
        }

        private GpkPackage ReadSubGpkPackage(GpkPackage package, byte[] data, bool skipExportData, Status stat)
        {
            BinaryReader reader = null;

            try
            {
                reader = new BinaryReader(new MemoryStream(data));
                Stopwatch pkgWatch = new Stopwatch();

                logger = LogManager.GetLogger("[ReadSubGpkPackage:" + package.Filename + "]");
                logger.Debug("Reading Start");
                stat.name = package.Filename;
                pkgWatch.Start();

                //parsing
                ReadHeader(reader, package);
                var file = CheckAndDecompress(reader, package);
                if (file != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = new BinaryReader(new MemoryStream(file));
                }

                ReadNames(reader, package);
                ReadImports(reader, package);
                ReadExports(reader, package);
                ReadDepends(reader, package);
                if (!skipExportData) ReadExportData(reader, package);

                reader.Close();
                reader.Dispose();

                //boring log stuff
                pkgWatch.Stop();
                stat.time = pkgWatch.ElapsedMilliseconds;
                stat.finished = true;
                logger.Info("Reading of package {0} complete, took {1}ms!", package.Filename, pkgWatch.ElapsedMilliseconds);

                return package;
            }
            catch (Exception ex)
            {
                logger.Fatal("Parse failure!");
                logger.Fatal(ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return null;
        }


        private void ReadHeader(BinaryReader reader, GpkPackage package)
        {
            logger.Trace("Header start");

            package.Header.Tag = reader.ReadInt32();
            CheckSignature(package.Header.Tag);

            package.Header.FileVersion = reader.ReadInt16();
            package.Header.LicenseVersion = reader.ReadInt16();

            package.x64 = package.Header.FileVersion >= 0x381;

            package.Header.PackageFlags = reader.ReadInt32();

            int len = reader.ReadInt32();
            package.Header.PackageName = ReadString(reader, len);

            package.Header.Unk1 = reader.ReadInt16();
            package.Header.Unk2 = reader.ReadInt16();

            package.Header.NameCount = reader.ReadInt32();
            package.Header.NameOffset = reader.ReadInt32();
            if (!package.x64) FixNameCount(package);

            package.Header.ExportCount = reader.ReadInt32();
            package.Header.ExportOffset = reader.ReadInt32();

            package.Header.ImportCount = reader.ReadInt32();
            package.Header.ImportOffset = reader.ReadInt32();

            package.Header.DependsOffset = reader.ReadInt32();

            if (package.x64) package.Header.HeaderSize = reader.ReadInt32();

            logger.Debug("NameCount " + package.Header.NameCount);
            logger.Debug("NameOffset " + package.Header.NameOffset);

            logger.Debug("ExportCount " + package.Header.ExportCount);
            logger.Debug("ExportOffset " + package.Header.ExportOffset);

            logger.Debug("ImportCount " + package.Header.ImportCount);
            logger.Debug("ImportOffset " + package.Header.ImportOffset);

            logger.Debug("DependsOffset " + package.Header.DependsOffset);
            logger.Debug("HeaderSize " + package.Header.HeaderSize);

            if (package.x64) package.Header.Unk3 = reader.ReadBytes(12);

            stat.totalobjects = package.Header.NameCount + package.Header.ImportCount + package.Header.ExportCount * 3; //Export, Export Linking, ExportData = *3
            logger.Debug("File Info: NameCount {0}, ImportCount {1}, ExportCount {2}", package.Header.NameCount, package.Header.ImportCount, package.Header.ExportCount);

            package.Header.FGUID = reader.ReadBytes(16);
            //logger.Info("FGUID " + package.Header.FGUID);


            int generation_count = reader.ReadInt32();
            package.Header.Generations.Clear();
            for (int i = 0; i < generation_count; i++)
            {
                GpkGeneration tmpgen = new GpkGeneration();
                tmpgen.ExportCount = reader.ReadInt32();
                tmpgen.NameCount = reader.ReadInt32();
                tmpgen.NetObjectCount = reader.ReadInt32();

                logger.Debug("Generation {0}, ExportCount {1}, NameCount {2}, NetObjectCount {3}", i, tmpgen.ExportCount, tmpgen.NameCount, tmpgen.NetObjectCount);

                package.Header.Generations.Add(tmpgen);
            }


            package.Header.EngineVersion = reader.ReadInt32();
            package.Header.CookerVersion = reader.ReadInt32();

            package.Header.CompressionFlags = reader.ReadInt32();
            int chunkCount = reader.ReadInt32();

            package.Header.ChunkHeaders.Clear();
            for (int i = 0; i < chunkCount; i++)
            {
                var chunk = new GpkCompressedChunkHeader();
                chunk.UncompressedOffset = reader.ReadInt32();
                chunk.UncompressedSize = reader.ReadInt32();
                chunk.CompressedOffset = reader.ReadInt32();
                chunk.CompressedSize = reader.ReadInt32();

                package.Header.ChunkHeaders.Add(chunk);

                if (i == 0)
                {
                    package.UncompressedSize = chunk.CompressedOffset + chunk.UncompressedSize;
                    //header + chunk 1, assumption is that chunk 1 lays behind header
                }
                else
                {
                    package.UncompressedSize += chunk.UncompressedSize;
                }

                int endOfChunk = chunk.CompressedOffset + chunk.CompressedSize;
                if (endOfChunk > package.CompressedEndOffset)
                    package.CompressedEndOffset = endOfChunk;

                logger.Debug("Chunk {0}: UncompressedOffset {1}, UncompressedSize {2}, CompressedOffset {3}, CompressedSize {4}, CompressedEnd {5}",
                    i, chunk.UncompressedOffset, chunk.UncompressedSize, chunk.CompressedOffset, chunk.CompressedSize, endOfChunk);


            }


            if (package.Header.EngineVersion == 0xC0FFEE) logger.Info("Found a old brother ;)");

            logger.Debug("EngineVersion {0}, CookerVersion {1}, compressionFlags {2}, chunkCount {3}, PkgUncompressedSize {4}",
           package.Header.EngineVersion, package.Header.CookerVersion, package.Header.CompressionFlags, package.Header.ChunkHeaders.Count, package.UncompressedSize);

            logger.Debug("Compressed: {0}, FullyCompressed {1}, PackageFlags {2:X}",
                ((GpkPackageFlags)package.Header.PackageFlags & GpkPackageFlags.Compressed),
                ((GpkPackageFlags)package.Header.PackageFlags & GpkPackageFlags.FullyCompressed),
                package.Header.PackageFlags
            );
        }

        private void CheckSignature(int sig)
        {
            //A quick validty check
            if (sig != -1641380927) //0x9E2A83C1
            {
                throw new Exception("Not a valid GPK File. Signature Check failed!");
            }
        }


        private void FixNameCount(GpkPackage package)
        {

            bool t = ((GpkPackageFlags)package.Header.PackageFlags & GpkPackageFlags.BrokenLinks) > 0;
            package.Header.NameCount -= package.Header.NameOffset;
        }


        private byte[] CheckAndDecompress(BinaryReader reader, GpkPackage package)
        {
            logger.Trace("checkAndDecompress start");
            if (package.Header.ChunkHeaders.Count == 0)
                return null;

            byte[] completeFile = new byte[package.UncompressedSize]; //should be the complete file size


            foreach (var header in package.Header.ChunkHeaders)
            {
                reader.BaseStream.Seek(header.CompressedOffset, SeekOrigin.Begin);
                PackageChunkBlock block = new PackageChunkBlock();

                block.signature = reader.ReadInt32();
                block.blocksize = reader.ReadInt32();
                block.compressedSize = reader.ReadInt32();
                block.uncompressedSize_chunkheader = reader.ReadInt32();

                int chunkCount = (block.uncompressedSize_chunkheader + block.blocksize - 1) / block.blocksize;
                byte[] uncompressedBytes = new byte[header.UncompressedSize];
                block.Decompress(uncompressedBytes, chunkCount, reader, package.Header.CompressionFlags);

                Array.ConstrainedCopy(uncompressedBytes, 0, completeFile, header.UncompressedOffset, header.UncompressedSize);
            }

            if (Settings.Default.Debug)
            {
                //dump uncompressed file
                string path = String.Format("{0}\\decomp_{1}", Directory.GetCurrentDirectory(), package.Filename);
                //Task.Factory.StartNew(() => File.WriteAllBytes(path, completeFile));
            }


            return completeFile;
        }


        private void ReadNames(BinaryReader reader, GpkPackage package)
        {
            logger.Debug("Reading Namelist at {0}....", package.Header.NameOffset);
            reader.BaseStream.Seek(package.Header.NameOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.NameCount; i++)
            {
                GpkString tmpString = new GpkString();
                int len = reader.ReadInt32();
                if (len > 0)
                {
                    tmpString.name = Reader.ReadString(reader, len);
                }
                else
                {
                    tmpString.name = Reader.ReadUnicodeString(reader, (len * -1) * 2);
                }

                tmpString.flags = reader.ReadInt64();

                package.NameList.Add(i, tmpString);

                logger.Trace("Name {0}: {1}", i, tmpString.name);
                stat.progress++;
            }

        }

        private void ReadImports(BinaryReader reader, GpkPackage package)
        {
            logger.Debug("Reading Imports at {0}....", package.Header.ImportOffset);
            reader.BaseStream.Seek(package.Header.ImportOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ImportCount; i++)
            {
                GpkImport import = new GpkImport();
                long package_class_index = reader.ReadInt64();
                long class_index = reader.ReadInt64();

                import.PackageRef = reader.ReadInt32();
                long object_index = reader.ReadInt32();
                import.Unk = reader.ReadInt32();

                import.ClassPackage = package.GetString(package_class_index);
                import.ClassName = package.GetString(class_index);
                import.ObjectName = package.GetString(object_index);

                import.UID = GenerateUID(package, import);
                package.ImportList.Add(i, import);

                logger.Trace("Import {0}: ClassPackage {1} Class: {2} Object: {3}", i, import.ClassPackage, import.ClassName, import.ObjectName);
                stat.progress++;
            }
        }

        private void ReadExports(BinaryReader reader, GpkPackage package)
        {
            logger.Debug("Reading Exports at {0}....", package.Header.ExportOffset);
            reader.BaseStream.Seek(package.Header.ExportOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ExportCount; i++)
            {
                GpkExport export = new GpkExport(package);
                export.ClassIndex = reader.ReadInt32();
                export.SuperIndex = reader.ReadInt32();
                export.PackageIndex = reader.ReadInt32();

                long nameIndex = reader.ReadInt32();
                export.ObjectName = package.GetString(nameIndex);

                export.Unk1 = reader.ReadInt64();
                export.Unk2 = reader.ReadInt64();

                export.SerialSize = reader.ReadInt32();

                if (export.SerialSize > 0)
                {
                    export.SerialOffset = reader.ReadInt32();
                }

                export.Unk3 = reader.ReadInt32();
                export.UnkHeaderCount = reader.ReadInt32();
                export.Unk4 = reader.ReadInt32();
                export.Guid = reader.ReadBytes(16);
                export.UnkExtraInts = reader.ReadBytes(export.UnkHeaderCount * 4);

                package.ExportList.Add(i, export);

                logger.Trace("Export {0}: ObjectName: {1}, Data_Size: {2}, Data_Offset {3}, Export_offset {4}", i, export.ObjectName, export.SerialSize, export.SerialOffset, reader.BaseStream.Position);
                stat.progress++;
            }

            //post-processing. needed if a object points to another export.
            logger.Debug("Linking Exports..");
            foreach (KeyValuePair<long, GpkExport> pair in package.ExportList)
            {
                GpkExport export = pair.Value;
                if (export.ClassName == null || export.SuperName == null || export.PackageName == null || export.UID == null)
                {
                    export.ClassName = package.GetObjectName(export.ClassIndex);
                    export.SuperName = package.GetObjectName(export.SuperIndex);
                    export.PackageName = package.GetObjectName(export.PackageIndex);
                    export.UID = GenerateUID(package, export);
                }

                stat.progress++;
            }
        }

        private void ReadDepends(BinaryReader reader, GpkPackage package)
        {
            logger.Debug("Reading Depends at {0}....", package.Header.DependsOffset);
            reader.BaseStream.Seek(package.Header.DependsOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ExportCount; i++)
            {
                package.ExportList[i].DependsTableData = reader.ReadInt32();
                if (package.ExportList[i].DependsTableData != 0)
                {
                    logger.Debug("DEP != 0");
                }

                //string depClass = package.GetObjectName(package.ExportList[i].DependsTableData);
                //logger.Trace("Dep for {0} is {1}", package.ExportList[i].UID, depClass);
            }
        }


        private void ReadExportData(BinaryReader reader, GpkPackage package)
        {
            logger.Debug("Reading ExportsData....");

            long maxValue = 0;
            GpkExport maxExp = null;
            foreach (GpkExport export in package.ExportList.Values)
            {
                try
                {
                    reader.BaseStream.Seek(export.SerialOffset, SeekOrigin.Begin);

                    //int objectindex (netindex)
                    export.NetIndex = reader.ReadInt32();
                    if (export.NetIndex != 0)
                    {
                        //export.netIndexName = package.GetObjectName(netIndex, true);
                    }

                    //dirty hack until we find the begin 
                    long namePosStart = reader.BaseStream.Position;
                    while (true)
                    {
                        long test_nameindex = reader.ReadInt64();
                        if (package.NameList.ContainsKey(test_nameindex))
                        {
                            long tmpNameStartPos = reader.BaseStream.Position - 8;
                            long name_padding_count = tmpNameStartPos - namePosStart;
                            reader.BaseStream.Seek(namePosStart, SeekOrigin.Begin);

                            export.PropertyPadding = new byte[name_padding_count];
                            export.PropertyPadding = reader.ReadBytes((int)name_padding_count);

                            //reader.BaseStream.Seek(name_start, SeekOrigin.Begin);
                            break;
                        }

                        reader.BaseStream.Seek(-7, SeekOrigin.Current);
                    }
                    //bad style :(
                    //logger.Info("First {0} Skip {1}", first, reader.BaseStream.Position - export.SerialOffset);
                    //Props  

                    bool cont = true;
                    while (cont)
                    {
                        cont = ReadPropertyDetails(reader, package, export);
                    }
                    export.PropertySize = (int)reader.BaseStream.Position - export.SerialOffset;

                    //data part
                    long object_end = export.SerialOffset + export.SerialSize;

                    int toread = 0;
                    string tag = "";

                    if (reader.BaseStream.Position <= object_end)
                    {
                        toread = (int)(object_end - reader.BaseStream.Position);
                        export.DataStart = reader.BaseStream.Position;

                        if (Settings.Default.JitData)
                        {
                            //use our dataloader, will be just loaded if needed. more performance, less memory cons. 
                            tag = "JIT";
                            export.Loader = new DataLoader(package.Path, (int)export.DataStart, toread);
                        }
                        else
                        {
                            //load everything aot
                            tag = "AOT";
                            export.Data = new byte[toread];
                            export.Data = reader.ReadBytes(toread);
                            ParsePayload(package, export);

                            if (export.Payload != null) logger.Debug(export.Payload.ToString());
                        }
                    }


                    logger.Trace(String.Format("Export {0}: Read Data ({1} bytes {2}) and {3} Properties ({4} bytes)", export.ObjectName, toread, tag, export.Properties.Count, export.PropertySize));

                    long totalRead = reader.BaseStream.Position - export.SerialOffset;
                    long shouldBe = export.SerialSize;
                    long oursize = export.GetDataSize();
                    if (totalRead != shouldBe || totalRead != oursize)
                    {
                        logger.Debug(String.Format("totalRead {0} GetDataSize {1} shouldBe {2}", totalRead, oursize, shouldBe));
                    }

                    if (reader.BaseStream.Position > maxValue)
                    {
                        maxValue = reader.BaseStream.Position;
                        maxExp = export;
                    }

                }
                catch (Exception ex)
                {
                    logger.Fatal("[ReadExportData] UID: {0} Error: {1}", export.UID, ex);
                }
                //data
                stat.progress++;
            }

            logger.Debug("MAX VALUE " + maxValue);
            logger.Debug("MAX EXPORT " + maxExp.ObjectName);
        }

        public static void ParsePayload(GpkPackage package, GpkExport export)
        {
            switch (export.ClassName)
            {
                case "Core.SoundNodeWave":
                    export.Payload = new Soundwave();
                    break;
                case "Core.SoundCue":
                    export.Payload = new SoundCue();
                    break;
                case "Core.Texture2D":
                    if (Settings.Default.EnableTexture2D)
                    {
                        //gmp hack
                        if (isStrangeCompressedTexture(export))
                            break;

                        export.Payload = new Texture2D();
                    }

                    break;
                case "Core.ObjectRedirector":
                    export.Payload = new ObjectRedirector();
                    break;
                case "Core.ObjectReferencer":
                    export.Payload = new ObjectReferencer();
                    break;
            }

            if (export.Payload != null)
                export.Payload.ReadData(package, export);
        }

        public static Boolean isStrangeCompressedTexture(GpkExport export)
        {
            foreach (GpkBaseProperty prop in export.Properties)
            {
                if (prop.name == "CompressionNoMipmaps" || prop.name == "CompressionNoAlpha")
                    return true;
            }
            return false;
        }

        private Boolean ReadPropertyDetails(BinaryReader reader, GpkPackage package, GpkExport export)
        {
            GpkBaseProperty baseProp = new GpkBaseProperty();

            long nameindex = reader.ReadInt64();


            if (!package.NameList.ContainsKey(nameindex))
            {
                logger.Fatal("name not found " + nameindex);
                if (export.Properties.Count > 0)
                {
                    logger.Fatal("prev " + export.Properties[export.Properties.Count - 1]);
                }
            }

            baseProp.name = package.GetString(nameindex);
            if (baseProp.name.ToLower() == "none") return false;

            long typeindex = reader.ReadInt64();
            if (!package.NameList.ContainsKey(typeindex))
            {
                logger.Fatal("type not found " + typeindex);
            }
            baseProp.type = package.GetString(typeindex);

            baseProp.size = reader.ReadInt32();
            baseProp.arrayIndex = reader.ReadInt32();

            IProperty iProp;

            switch (baseProp.type)
            {
                case "StructProperty":
                    iProp = new GpkStructProperty(baseProp);
                    break;
                case "ArrayProperty":
                    iProp = new GpkArrayProperty(baseProp);
                    break;
                case "BoolProperty":
                    iProp = new GpkBoolProperty(baseProp);
                    break;
                case "ByteProperty":
                    iProp = new GpkByteProperty(baseProp);
                    break;
                case "NameProperty":
                    iProp = new GpkNameProperty(baseProp);
                    break;
                case "IntProperty":
                    iProp = new GpkIntProperty(baseProp);
                    break;
                case "FloatProperty":
                    iProp = new GpkFloatProperty(baseProp);
                    break;
                case "StrProperty":
                    iProp = new GpkStringProperty(baseProp);
                    break;
                case "ObjectProperty":
                    iProp = new GpkObjectProperty(baseProp);
                    break;
                default:
                    throw new Exception(
                        string.Format(
                            "Unknown Property Type {0}, Position {1}, Prop_Name {2}, Export_Name {3}",
                            baseProp.type, reader.BaseStream.Position, baseProp.name, export.ObjectName));

            }

            iProp.ReadData(reader, package);
            iProp.RecalculateSize();
            export.Properties.Add(iProp);

            //logger.Trace(String.Format("Property Type {0}, Position after {1}, Prop_Name {2}, Export_Name {3}", baseProp.type, reader.BaseStream.Position, baseProp.ObjectName, export.ObjectName));

            return true;
        }

        public static string ReadString(BinaryReader reader, int length)
        {
            string text = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(length - 1));
            reader.ReadByte(); //text control char 

            return text;
        }

        public static string ReadUnicodeString(BinaryReader reader, int length)
        {
            string text = UnicodeEncoding.Unicode.GetString(reader.ReadBytes(length - 2));
            reader.ReadBytes(2); //text control char 

            return text;
        }

        public static string GenerateUID(GpkPackage package, GpkExport export)
        {

            string proposedName;
            if (export.PackageName == "none")
            {
                proposedName = export.ObjectName;
            }
            else
            {
                proposedName = export.PackageName + "." + export.ObjectName;
            }

            int counter = 0;
            do
            {
                string tmpName = proposedName;
                if (counter > 0)
                {
                    tmpName += ("_" + counter);
                }

                if (package.UidList.ContainsKey(tmpName) == false)
                {
                    package.UidList.Add(tmpName, "");
                    return tmpName;
                }

                counter++;
            } while (true);

        }

        public static string GenerateUID(GpkPackage package, GpkImport import)
        {
            string proposedName = import.ClassPackage + "." + import.ObjectName;

            int counter = 0;
            do
            {
                string tmpName = proposedName;
                if (counter > 0)
                {
                    tmpName += ("_" + counter);
                }

                if (package.UidList.ContainsKey(tmpName) == false)
                {
                    package.UidList.Add(tmpName, "");
                    return tmpName;
                }

                counter++;
            } while (true);
        }

        private bool CheckForAnotherPackage(BinaryReader reader)
        {
            //length check
            if (reader.BaseStream.Position + 4 > reader.BaseStream.Length)
                return false;

            var sig = reader.ReadUInt32();
            reader.BaseStream.Seek(-4, SeekOrigin.Current);
            return sig == 0x9E2A83C1; //0x9E2A83C1  -1641380927
        }


        public Status GetStatus()
        {
            return stat;
        }
    }
}
