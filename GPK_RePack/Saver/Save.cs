using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class;
using GPK_RePack.Class.Prop;
using NLog;
using NLog.Fluent;
using NLog.LayoutRenderers;

namespace GPK_RePack.Saver
{
    class Save
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void SaveReplacedExport(GpkPackage package, string savepath, List<GpkExport> changedExports)
        {
            byte[] buffer = File.ReadAllBytes(package.Filename);
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
            using (BinaryWriter writer = new BinaryWriter(new FileStream(savepath, FileMode.Create)))
            {
                WriteHeader(writer, package);
                WriteNamelist(writer, package);
                WriteImports(writer, package);
                WriteExports(writer, package);
                WriteExportsData(writer, package);
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
            writer.Write(package.Header.NameOffset);

            writer.Write(package.Header.ExportCount);
            writer.Write(package.Header.ExportOffset);

            writer.Write(package.Header.ImportCount);
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

            writer.Write(package.Header.EngineVersion);
            writer.Write(package.Header.CookerVersion);

            logger.Debug("Wrote header pos " + writer.BaseStream.Position);
        }

        private void WriteNamelist(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.NameOffset)
            {
                throw new Exception("name offset mismatch!");
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
                throw new Exception("import offset mismatch!");
            }

            foreach (GpkImport imp in package.ImportList.Values)
            {
                writer.Write(GetStringIndex(imp.ClassPackage, package));
                writer.Write(GetStringIndex(imp.Class, package));
                writer.Write(imp.PackageRef);
                writer.Write((int)GetStringIndex(imp.Object, package));
                writer.Write(imp.Unk);
            }

            logger.Debug("Wrote imports pos " + writer.BaseStream.Position);
        }

        private void WriteExports(BinaryWriter writer, GpkPackage package)
        {
            if (writer.BaseStream.Position != package.Header.ExportOffset)
            {
                throw new Exception("export offset mismatch!");
            }

            foreach (GpkExport export in package.ExportList.Values)
            {
                writer.Write((int)GetObjectIndex(export.ClassName, package));
                writer.Write(export.SuperIndex);
                writer.Write(export.PackageIndex);
                writer.Write(Convert.ToInt32(GetStringIndex(export.Name, package)));

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
            writer.Write(new int());

            foreach (GpkExport export in package.ExportList.Values)
            {
                logger.Trace("export data for " + export.Name);
                long data_start = writer.BaseStream.Position;

                if (export.SerialOffset != writer.BaseStream.Position)
                {
                    long actualpos = writer.BaseStream.Position;
                    //if we have diffrent layout of the data then teh orginal file we need to fix the data pointers
                    logger.Trace(string.Format("fixing export {0} offest old:{1} new:{2}", export.Name, export.SerialOffset, actualpos));


                    export.SerialOffset = (int)writer.BaseStream.Position;
                    writer.BaseStream.Seek(export.SerialOffsetPosition, SeekOrigin.Begin);
                    writer.Write(export.SerialOffset);
                    writer.BaseStream.Seek(actualpos, SeekOrigin.Begin);
                }

                writer.Write(export.netIndex);

                if (export.property_padding != null)
                {
                    writer.Write(export.property_padding);
                }
                else
                {
                    logger.Trace(1);
                }


                foreach (object prop in export.Properties)
                {
                    GpkBaseProperty baseProperty = (GpkBaseProperty)prop;
                    writer.Write(GetStringIndex(baseProperty.Name, package));
                    writer.Write(GetStringIndex(baseProperty.type, package));

                    if (prop is GpkArrayProperty)
                    {
                        GpkArrayProperty tmpArray = (GpkArrayProperty)prop;
                        writer.Write(tmpArray.length);
                        writer.Write(tmpArray.value);
                    }
                    else if (prop is GpkStructProperty)
                    {
                        GpkStructProperty tmpStruct = (GpkStructProperty)prop;
                        writer.Write(tmpStruct.length);
                        writer.Write(GetStringIndex(tmpStruct.innerType, package));
                        writer.Write(tmpStruct.value);
                    }
                    else if (prop is GpkBoolProperty)
                    {
                        GpkBoolProperty tmpBool = (GpkBoolProperty)prop;
                        writer.Write(tmpBool.unk);
                        writer.Write(Convert.ToInt32(tmpBool.value));
                    }
                    else if (prop is GpkNameProperty)
                    {
                        GpkNameProperty tmpName = (GpkNameProperty)prop;
                        writer.Write(tmpName.unk);
                        writer.Write((int)GetStringIndex(tmpName.value, package));
                        writer.Write(tmpName.padding);
                    }
                    else if (prop is GpkIntProperty)
                    {
                        GpkIntProperty tmpInt = (GpkIntProperty)prop;
                        writer.Write(tmpInt.unk);
                        writer.Write(tmpInt.value);
                    }
                    else if (prop is GpkFloatProperty)
                    {
                        GpkFloatProperty tmpFloat = (GpkFloatProperty)prop;
                        writer.Write(tmpFloat.unk);
                        writer.Write(tmpFloat.value);
                    }
                    else if (prop is GpkStringProperty)
                    {
                        GpkStringProperty tmpString = (GpkStringProperty)prop;
                        writer.Write(tmpString.unk);
                        writer.Write(tmpString.length);

                        if (tmpString.length > 0)
                        {
                            WriteString(writer, tmpString.value);
                        }
                        else
                        {
                            WriteUnicodeString(writer, tmpString.value);
                        }
                    }
                    else if (prop is GpkObjectProperty)
                    {
                        GpkObjectProperty tmpObj = (GpkObjectProperty)prop;
                        writer.Write(tmpObj.unk);
                        writer.Write((int)GetObjectIndex(tmpObj.ClassName, package));

                    }
                    else if (prop is GpkByteProperty)
                    {
                        GpkByteProperty tmpByte = (GpkByteProperty)prop;
                        writer.Write(tmpByte.size);
                        writer.Write(tmpByte.arrayIndex);

                        if (tmpByte.size == 8)
                        {
                            writer.Write(GetStringIndex(tmpByte.nameValue, package));
                        }
                        else
                        {
                            writer.Write(tmpByte.byteValue);
                        }

                    }
                }

                //end with a none nameindex
                writer.Write(GetStringIndex("None", package));


                if (export.data_padding != null)
                {
                    writer.Write((export.data_padding));
                }

                //finally our data ^^
                if (export.data != null)
                {
                    writer.Write(export.data);
                }

                long data_end = writer.BaseStream.Position;
                int data_size = (int)(data_end - data_start);
                if (data_size != export.SerialSize)
                { 
                    //maybe replaced data OR some property errors. write new data size.

                    logger.Trace(string.Format("fixing export {0} size old:{1} new:{2}", export.Name, export.SerialSize, data_size));
                    export.SerialSize = data_size;
                    writer.BaseStream.Seek(export.SerialOffsetPosition - 4, SeekOrigin.Begin);
                    writer.Write(export.SerialSize);
                    writer.BaseStream.Seek(data_end, SeekOrigin.Begin);
                    
                }

                logger.Trace("wrote export data for " + export.Name + " end pos " + writer.BaseStream.Position);
            }

            logger.Debug("Wrote export data pos " + writer.BaseStream.Position);
        }

        private void WriteString(BinaryWriter writer, string text)
        {
            writer.Write(ASCIIEncoding.ASCII.GetBytes(text));
            writer.Write(new byte());
        }

        private void WriteUnicodeString(BinaryWriter writer, string text)
        {
            writer.Write(UnicodeEncoding.Unicode.GetBytes(text));
            writer.Write(new short());
        }

        private long GetStringIndex(string text, GpkPackage package)
        {
            foreach (KeyValuePair<long, GpkString> pair in package.NameList)
            {
                if (pair.Value.name == text) return pair.Key;
            }

            throw new Exception(string.Format("Name {0} not found!", text));
        }

        private long GetObjectIndex(string text, GpkPackage package)
        {
            if (text == "none") return 0;

            foreach (KeyValuePair<long, GpkImport> pair in package.ImportList)
            {
                if (pair.Value.Object == text)
                {
                    long tmpKey = (pair.Key + 1) * -1; //0 -> 1 --> -1 ## 1 --> 2 --> -2 ##
                    return tmpKey;
                }
            }

            foreach (KeyValuePair<long, GpkExport> pair in package.ExportList)
            {
                if (pair.Value.Name == text)
                {
                    long tmpKey = (pair.Key + 1); //0 -> 1 --> -1 ## 1 --> 2 --> -2 ##
                    return tmpKey;
                }
            }

            throw new Exception(string.Format("Object {0} not found!", text));
        }
    }
}
