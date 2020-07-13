using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using GPK_RePack.Model;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Prop;
using GPK_RePack.Properties;
using NLog;

namespace GPK_RePack.IO
{
    struct Status
    {
        public int progress;
        public int totalobjects;
        public long time;
        public bool finished;
        public string name;
    }

    class Writer : IProgress
    {
        private Logger logger;
        private long offsetExportPos = 0;
        private long offsetImportPos = 0;
        private long offsetNamePos = 0;
        private long offsetDependsPos = 0;
        private long headerSizeOffset = 0;

        public Status stat = new Status();

        public void SaveReplacedExport(GpkPackage package, string savepath, List<GpkExport> changedExports)
        {
            byte[] buffer = File.ReadAllBytes(package.Path);
            BinaryWriter writer = new BinaryWriter(new MemoryStream(buffer));

            foreach (GpkExport export in changedExports)
            {
                writer.Seek((int)export.DataStart, SeekOrigin.Begin);
                writer.Write(export.Data);
            }

            writer.Close();
            writer.Dispose();

            File.WriteAllBytes(savepath, buffer);
        }

        public void SaveGpkPackage(GpkPackage package, string savepath, bool addPadding)
        {
            //Header 
            //Namelist
            //Imports
            //Exports  
            logger = LogManager.GetLogger("[Save:" + package.Filename + "]");
            logger.Info(String.Format("Attemping to save {0}...", package.Filename));

            Stopwatch watch = new Stopwatch();
            watch.Start();

            stat = new Status();
            stat.name = package.Filename;

            package.PrepareWriting();
            int compuSize = package.GetSize(false);

            stat.totalobjects = package.Header.NameCount + package.Header.ExportCount * 2 + package.Header.ImportCount; //Export, ExportData = x2

            using (BinaryWriter writer = new BinaryWriter(new FileStream(savepath, FileMode.Create)))
            {
                WriteHeader(writer, package);
                WriteNamelist(writer, package);
                WriteImports(writer, package);
                WriteExports(writer, package);
                WriteDepends(writer, package);
                WriteHeaderSize(writer, package);
                WriteExportsData(writer, package);
                if (addPadding)
                {
                    WriteFilePadding(writer, package, compuSize);
                }
                else
                {
                    WriteFileEnding(writer, package, compuSize);
                }


            }

            watch.Stop();
            stat.time = watch.ElapsedMilliseconds;
            stat.finished = true;
            logger.Info(String.Format("Saved the package '{0} to {1}', took {2}ms!", package.Filename, savepath, stat.time));
        }

        private void WriteHeader(BinaryWriter writer, GpkPackage package)
        {
            writer.Write(package.Header.Tag);
            writer.Write(package.Header.FileVersion);
            writer.Write(package.Header.LicenseVersion);
            writer.Write(package.Header.PackageFlags);

            WriteString(writer, package.Header.PackageName, true);

            writer.Write(package.Header.Unk1);
            writer.Write(package.Header.Unk2);

            if (package.x64) writer.Write(package.Header.NameCount);
            else writer.Write(package.Header.NameCount + package.Header.NameOffset); //tera thing

            offsetNamePos = writer.BaseStream.Position;
            writer.Write(package.Header.NameOffset);

            writer.Write(package.Header.ExportCount);
            offsetExportPos = writer.BaseStream.Position;
            writer.Write(package.Header.ExportOffset);

            writer.Write(package.Header.ImportCount);
            offsetImportPos = writer.BaseStream.Position;
            writer.Write(package.Header.ImportOffset);

            offsetDependsPos = writer.BaseStream.Position;
            writer.Write(package.Header.DependsOffset);

            headerSizeOffset = writer.BaseStream.Position;
            if (package.x64) writer.Write(package.Header.HeaderSize);
            if(package.x64) writer.Write(package.Header.Unk3);

            writer.Write(package.Header.FGUID);

            writer.Write(package.Header.Generations.Count);
            for (int i = 0; i < package.Header.Generations.Count; i++)
            {
                GpkGeneration tmpgen = package.Header.Generations[i];
                writer.Write(tmpgen.ExportCount);
                writer.Write(tmpgen.NameCount);
                writer.Write(tmpgen.NetObjectCount);
            }

            //24
            //writer.Write(package.Header.EngineVersion); 
            writer.Write(0xC0FFEEAA); //my signature 
            writer.Write(package.Header.CookerVersion);

            writer.Write((int)0); //compressionFlags, we dont apply this
            writer.Write((long)0); //unk
            writer.Write((int)0); 
           



            logger.Debug("Wrote header pos " + writer.BaseStream.Position);
        }

        private void WriteNamelist(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.NameOffset)
            {
                package.Header.NameOffset = (int)writer.BaseStream.Position;

                writer.BaseStream.Seek(offsetNamePos, SeekOrigin.Begin);
                writer.Write(package.Header.NameOffset);
                writer.BaseStream.Seek(package.Header.NameOffset, SeekOrigin.Begin);

                logger.Debug("name offset mismatch, fixed!");
            }

            foreach (GpkString tmpString in package.NameList.Values)
            {
                WriteString(writer, tmpString.name, true);
                writer.Write(tmpString.flags);
                stat.progress++;
            }

            logger.Debug("Wrote namelist pos " + writer.BaseStream.Position);
        }

        private void WriteImports(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.ImportOffset)
            {
                package.Header.ImportOffset = (int)writer.BaseStream.Position;

                writer.BaseStream.Seek(offsetImportPos, SeekOrigin.Begin);
                writer.Write(package.Header.ImportOffset);
                writer.BaseStream.Seek(package.Header.ImportOffset, SeekOrigin.Begin);

                logger.Debug("import offset mismatch, fixed!");
            }

            foreach (GpkImport imp in package.ImportList.Values)
            {
                writer.Write(package.GetStringIndex(imp.ClassPackage));
                writer.Write(package.GetStringIndex(imp.ClassName));
                writer.Write(imp.PackageRef);
                writer.Write((int)package.GetStringIndex(imp.ObjectName));
                writer.Write(imp.Unk);
                stat.progress++;
            }

            logger.Debug("Wrote imports pos " + writer.BaseStream.Position);
        }

        private void WriteExports(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.ExportOffset)
            {
                package.Header.ExportOffset = (int)writer.BaseStream.Position;

                writer.BaseStream.Seek(offsetExportPos, SeekOrigin.Begin);
                writer.Write(package.Header.ExportOffset);
                writer.BaseStream.Seek(package.Header.ExportOffset, SeekOrigin.Begin);

                logger.Debug("export offset mismatch, fixed!");
            }

            foreach (GpkExport export in package.ExportList.Values)
            {
                writer.Write((int)package.GetObjectIndex(export.ClassName));
                writer.Write((int)package.GetObjectIndex(export.SuperName));
                writer.Write((int)package.GetObjectIndex(export.PackageName));

                writer.Write(Convert.ToInt32(package.GetStringIndex(export.ObjectName)));

                writer.Write(export.Unk1);
                writer.Write(export.Unk2);

                writer.Write(export.SerialSize);

                //save for future use, when we write our data parts we muss seek back and modify this
                export.SerialOffsetPosition = writer.BaseStream.Position;
                if (export.SerialSize > 0) writer.Write(export.SerialOffset);

                writer.Write(export.Unk3);
                writer.Write(export.UnkHeaderCount);
                writer.Write(export.Unk4);
                writer.Write(export.Guid);
                writer.Write(export.UnkExtraInts);
                stat.progress++;
            }

            logger.Debug("Wrote exports pos " + writer.BaseStream.Position);
        }

        private void WriteDepends(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.DependsOffset)
            {
                package.Header.DependsOffset = (int)writer.BaseStream.Position;

                writer.BaseStream.Seek(offsetDependsPos, SeekOrigin.Begin);
                writer.Write(package.Header.DependsOffset);
                writer.BaseStream.Seek(package.Header.DependsOffset, SeekOrigin.Begin);

                logger.Debug("depends offset mismatch, fixed!");
            }

            foreach (GpkExport export in package.ExportList.Values)
            {
                writer.Write(export.DependsTableData);
            }

            logger.Debug("Wrote depends pos " + writer.BaseStream.Position);
        }

        private void WriteHeaderSize(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.HeaderSize)
            {
                package.Header.HeaderSize = (int)writer.BaseStream.Position;

                writer.BaseStream.Seek(headerSizeOffset, SeekOrigin.Begin);
                writer.Write(package.Header.HeaderSize);
                writer.BaseStream.Seek(package.Header.HeaderSize, SeekOrigin.Begin);

                logger.Debug("headersize mismatch, fixed!");
            }
        }

        private void WriteExportsData(BinaryWriter writer, GpkPackage package)
        {
            //puffer, seems random in many files, we use 10 empty bytes
            writer.Write(new byte[package.datapuffer]);

            foreach (GpkExport export in package.ExportList.Values)
            {
                if (export.SerialSize == 0)
                {
                    logger.Trace("skipping export data for " + export.ObjectName);
                }
                else
                {
                    logger.Trace("export data for " + export.ObjectName);
                }

                long data_start = writer.BaseStream.Position;

                if (export.SerialOffset != data_start)
                {
                    //if we have diffrent layout of the data then teh orginal file we need to fix the data pointers
                    logger.Trace(string.Format("fixing export {0} offset old:{1} new:{2}", export.ObjectName, export.SerialOffset, data_start));


                    export.SerialOffset = (int)data_start;
                    writer.BaseStream.Seek(export.SerialOffsetPosition, SeekOrigin.Begin);
                    writer.Write(export.SerialOffset);
                    writer.BaseStream.Seek(data_start, SeekOrigin.Begin);
                }

                if (export.NetIndexName != null)
                {
                    writer.Write((int)package.GetObjectIndex(export.NetIndexName));
                }
                else
                {
                    writer.Write(export.NetIndex);
                }

                if (export.PropertyPadding != null)
                {
                    writer.Write(export.PropertyPadding);
                }

                foreach (IProperty iProp in export.Properties)
                {
                    GpkBaseProperty baseProperty = (GpkBaseProperty)iProp;
                    writer.Write(package.GetStringIndex(baseProperty.name));
                    writer.Write(package.GetStringIndex(baseProperty.type));
                    writer.Write(baseProperty.size);
                    writer.Write(baseProperty.arrayIndex);

                    iProp.WriteData(writer, package);
                }

                //end with a none nameindex
                writer.Write(package.GetStringIndex("None"));


                //check
                long propRealSize = (writer.BaseStream.Position - data_start);
                if (Settings.Default.Debug && propRealSize != export.PropertySize)
                {
                    logger.Trace("Compu Prop Size: {0}, Diff {1} -", export.PropertySize, propRealSize - export.PropertySize);
                }


                if (export.DataPadding != null)
                {
                    writer.Write((export.DataPadding));
                }

                //finally our data 
                if (export.Payload != null)
                {
                    //pos is important. we cant be sure that the data is acurate.
                    export.Payload.WriteData(writer, package, export);
                }
                else if (export.Data != null)
                {
                    writer.Write(export.Data);
                }

                long data_end = writer.BaseStream.Position;
                int data_size = (int)(data_end - data_start);
                if (data_size != export.SerialSize)
                {
                    //maybe replaced data OR some property errors. write new data size.

                    logger.Trace(string.Format("fixing export {0} size old:{1} new:{2}", export.ObjectName, export.SerialSize, data_size));
                    export.SerialSize = data_size;
                    writer.BaseStream.Seek(export.SerialOffsetPosition - 4, SeekOrigin.Begin);
                    writer.Write(export.SerialSize);
                    writer.BaseStream.Seek(data_end, SeekOrigin.Begin);

                }

                logger.Trace("wrote export data for " + export.ObjectName + " end pos " + writer.BaseStream.Position);
                stat.progress++;
            }

            logger.Debug("Wrote export data pos " + writer.BaseStream.Position);
        }

        private void WriteFilePadding(BinaryWriter writer, GpkPackage package, int compuSize)
        {
            long final_size = writer.BaseStream.Position;
            logger.Debug("New size: {0}, Old size: {1}", final_size, package.OrginalSize);
            logger.Debug("Compu Size: {0}, Diff: {1} -", compuSize, final_size - compuSize);


            if (final_size < package.OrginalSize)
            {
                //too short, fill up with 00s ^^
                long missing = package.OrginalSize - final_size;
                writer.Write(new byte[missing]);
                logger.Info(String.Format("Package was filled up with {0} bytes..", missing));
            }
            else if (final_size == package.OrginalSize)
            {
                logger.Info(String.Format("Package size is the old size..."));
            }
            else if (final_size > package.OrginalSize)
            {
                //Too big
                logger.Info("The new package size is bigger than the orginal one! Tera may not acccept this file.");
                logger.Info("New size {0} bytes, Old size {1} bytes. +{2} bytes", final_size, package.OrginalSize, final_size - package.OrginalSize);
            }

        }

        private void WriteFileEnding(BinaryWriter writer, GpkPackage package, int compuSize)
        {
            long final_size = writer.BaseStream.Position + 4;
            writer.Write((int)final_size);

            logger.Debug("New size: {0}, Old size: {1}", final_size, package.OrginalSize);
            logger.Debug("Compu Size: {0}, Diff: {1} -", compuSize, final_size - compuSize);




        }

        public static int GetStringBytes(string text, bool isUnicode)
        {
            //length + string + terminating
            if (isUnicode)
            {
                return 4 + Encoding.Unicode.GetBytes(text).Length + 2;
            }
            else
            {
                return 4 + Encoding.Unicode.GetBytes(text).Length + 1;
            }
        }


        public static void WriteString(BinaryWriter writer, string text, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write(text.Length + 1);
            }
            writer.Write(Encoding.ASCII.GetBytes(text));
            writer.Write(new byte());
        }

        public static void WriteUnicodeString(BinaryWriter writer, string text, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((text.Length + 1) * -1);
            }
            writer.Write(Encoding.Unicode.GetBytes(text));
            writer.Write(new short());
        }

        public Status GetStatus()
        {
            return stat;
        }
    }
}
