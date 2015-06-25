using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Classes;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Prop;
using GPK_RePack.Properties;
using NLog;
using NLog.Fluent;
using NLog.LayoutRenderers;

namespace GPK_RePack.Saver
{
    class Save
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private long offsetExportPos = 0;
        private long offsetImportPos = 0;
        private long offsetNamePos = 0;

        public void SaveReplacedExport(GpkPackage package, string savepath, List<GpkExport> changedExports)
        {
            byte[] buffer = File.ReadAllBytes(package.Path);
            BinaryWriter writer = new BinaryWriter(new MemoryStream(buffer));

            foreach (GpkExport export in changedExports)
            {
                writer.Seek((int)export.data_start, SeekOrigin.Begin);
                writer.Write(export.data);
            }

            writer.Close();
            writer.Dispose();

            File.WriteAllBytes(savepath, buffer);
        }

        public void SaveGpkPackage(GpkPackage package, string savepath)
        {
            //Header 
            //Namelist
            //Imports
            //Exports
            logger.Debug("Start writing");
            int compuSize = package.GetActualSize();

            using (BinaryWriter writer = new BinaryWriter(new FileStream(savepath, FileMode.Create)))
            {
                WriteHeader(writer, package);
                WriteNamelist(writer, package);
                WriteImports(writer, package);
                WriteExports(writer, package);
                WriteExportsData(writer, package);
                WriteFilePadding(writer, package, compuSize);
            }


        }

        private void WriteHeader(BinaryWriter writer, GpkPackage package)
        {
            writer.Write(package.Header.Tag);
            writer.Write(package.Header.FileVersion);
            writer.Write(package.Header.LicenseVersion);
            writer.Write(package.Header.PackageFlags);

            writer.Write(package.Header.PackageName.Length + 1);
            WriteString(writer, package.Header.PackageName);

            writer.Write(package.Header.Unk1);
            writer.Write(package.Header.Unk2);

            writer.Write(package.Header.NameCount + package.Header.NameOffset); //tera thing
            offsetNamePos = writer.BaseStream.Position;
            writer.Write(package.Header.NameOffset);

            writer.Write(package.Header.ExportCount);
            offsetExportPos = writer.BaseStream.Position;
            writer.Write(package.Header.ExportOffset);

            writer.Write(package.Header.ImportCount);
            offsetImportPos = writer.BaseStream.Position;
            writer.Write(package.Header.ImportOffset);

            writer.Write(package.Header.DependsOffset);

            writer.Write(package.Header.FGUID);

            writer.Write(package.Header.Genrations.Count);
            for (int i = 0; i < package.Header.Genrations.Count; i++)
            {
                GpkGeneration tmpgen = package.Header.Genrations[i];
                writer.Write(tmpgen.ExportCount);
                writer.Write(tmpgen.NameCount);
                writer.Write(tmpgen.NetObjectCount);
            }

            writer.Write(package.Header.Unk3);
            writer.Write(package.Header.Unk4);
            writer.Write(package.Header.Unk5);
            writer.Write(package.Header.Unk6);

            //writer.Write(package.Header.EngineVersion); 
            writer.Write(0xC0FFEE); //my signature ^^
            writer.Write(package.Header.CookerVersion);


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
                writer.Write(tmpString.name.Length + 1);
                WriteString(writer, tmpString.name);
                writer.Write(tmpString.flags);
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
                writer.Write(package.GetStringIndex(imp.Class));
                writer.Write(imp.PackageRef);
                writer.Write((int)package.GetStringIndex(imp.ObjectName));
                writer.Write(imp.Unk);
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

                writer.Write(export.padding_unk);
            }

            logger.Debug("Wrote exports pos " + writer.BaseStream.Position);
        }

        private void WriteExportsData(BinaryWriter writer, GpkPackage package)
        {
            //puffer, seems random in many files
            writer.Write(new byte[10]);

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

                if (export.SerialOffset != writer.BaseStream.Position)
                {
                    //if we have diffrent layout of the data then teh orginal file we need to fix the data pointers
                    logger.Trace(string.Format("fixing export {0} offset old:{1} new:{2}", export.ObjectName, export.SerialOffset, data_start));


                    export.SerialOffset = (int)writer.BaseStream.Position;
                    writer.BaseStream.Seek(export.SerialOffsetPosition, SeekOrigin.Begin);
                    writer.Write(export.SerialOffset);
                    writer.BaseStream.Seek(data_start, SeekOrigin.Begin);
                }

                writer.Write(package.GetObjectIndex(export.netIndex));
                if (export.property_padding != null)
                {
                    writer.Write(export.property_padding);
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
                if (Settings.Default.Debug && propRealSize != export.property_size)
                {
                    logger.Trace("Compu Size: {0}, Diff {1} -", export.property_size, propRealSize - export.property_size);
                }


                if (export.data_padding != null)
                {
                    writer.Write((export.data_padding));
                }

                //finally our data ^^
                if (export.payload != null)
                {
                    //pos is important. we cant be sure that the data is acurate.
                    export.payload.WriteData(writer, package, export);
                }
                else if (export.data != null)
                {
                    writer.Write(export.data);
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
            }

            logger.Debug("Wrote export data pos " + writer.BaseStream.Position);
        }

        private void WriteFilePadding(BinaryWriter writer, GpkPackage package, int compuSize)
        {
            long final_size = writer.BaseStream.Position;
            logger.Debug(String.Format("New size {0}, Old size {1}", final_size, package.OrginalSize));
            logger.Debug("Compu Size: {0}, Diff {1} -", compuSize, final_size - compuSize);


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
                logger.Info(String.Format("The new package size is bigger than the orginal one! Tera may not acccept this file."));
                logger.Info(String.Format("New size {0} bytes, Old size {1} bytes. +{2} bytes", final_size, package.OrginalSize, final_size - package.OrginalSize));
            }

        }

        public static void WriteString(BinaryWriter writer, string text)
        {
            writer.Write(ASCIIEncoding.ASCII.GetBytes(text));
            writer.Write(new byte());
        }

        public static void WriteUnicodeString(BinaryWriter writer, string text)
        {
            writer.Write(UnicodeEncoding.Unicode.GetBytes(text));
            writer.Write(new short());
        }

    }
}
