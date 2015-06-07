using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class;
using GPK_RePack.Class.Prop;
using NLog;
using NLog.Fluent;

namespace GPK_RePack.Parser
{
    class Reader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GpkPackage ReadGpk(string path)
        {
            logger.Info("Reading: " + path);

            GpkPackage package = new GpkPackage();

            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                package.Filename = Path.GetFileName(path);

                ReadHeader(reader, package);
                ReadNames(reader, package);
                ReadImports(reader, package);
                ReadExports(reader, package);
                ReadExportData(reader, package);

                reader.Close();
                reader.Dispose();
            }

            return package;
        }

        private void ReadHeader(BinaryReader reader, GpkPackage package)
        {
            logger.Trace("Header start");

            package.Header.Tag = reader.ReadInt32();
            CheckSignature(package.Header.Tag);

            package.Header.FileVersion = reader.ReadInt16();
            package.Header.LicenseVersion = reader.ReadInt16();

            package.Header.PackageFlags = reader.ReadInt32();

            int len = reader.ReadInt32();
            package.Header.PackageName = ReadString(reader, len);

            package.Header.Unk1 = reader.ReadInt16();
            package.Header.Unk2 = reader.ReadInt16();

            package.Header.NameCount = reader.ReadInt32();
            package.Header.NameOffset = reader.ReadInt32();
            FixNameCount(package);

            package.Header.ExportCount = reader.ReadInt32();
            package.Header.ExportOffset = reader.ReadInt32();

            package.Header.ImportCount = reader.ReadInt32();
            package.Header.ImportOffset = reader.ReadInt32();

            package.Header.DependsOffset = reader.ReadInt32();

            logger.Info("NameCount " + package.Header.NameCount);
            logger.Info("NameOffset " + package.Header.NameOffset);

            logger.Info("ExportCount " + package.Header.ExportCount);
            logger.Info("ExportOffset " + package.Header.ExportOffset);

            logger.Info("ImportCount " + package.Header.ImportCount);
            logger.Info("ImportOffset " + package.Header.ImportOffset);

            logger.Info("DependsOffset " + package.Header.DependsOffset);

            package.Header.FGUID = reader.ReadBytes(16);
            logger.Info("FGUID " + package.Header.FGUID);


            int generation_count = reader.ReadInt32();
            for (int i = 0; i < generation_count; i++)
            {
                GpkGeneration tmpgen = new GpkGeneration();
                tmpgen.ExportCount = reader.ReadInt32();
                tmpgen.NameCount = reader.ReadInt32();
                tmpgen.NetObjectCount = reader.ReadInt32();

                logger.Info(String.Format("Generation {0}, ExportCount {1}, NameCount {2}, NetObjectCount {3}", i, tmpgen.ExportCount, tmpgen.NameCount, tmpgen.NetObjectCount));

                package.Header.Genrations.Add(tmpgen);
            }


            package.Header.Unk3 = reader.ReadInt32();
            package.Header.Unk4 = reader.ReadInt32();
            package.Header.Unk5 = reader.ReadInt32();
            package.Header.Unk6 = reader.ReadInt32();

            package.Header.EngineVersion = reader.ReadInt32();
            package.Header.CookerVersion = reader.ReadInt32();

            logger.Info(String.Format("Unk3 {0}, Unk4 {1}, Unk5 {2}, Unk6 {3}, EngineVersion {4}, CookerVersion {5}",
                package.Header.Unk3, package.Header.Unk4, package.Header.Unk5, package.Header.Unk6, package.Header.EngineVersion, package.Header.CookerVersion));
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
            int t = package.Header.PackageFlags & 8;
            /*
            if ((package.Header.PackageFlags & 8) == 8)
            {
                package.Header.NameCount -= package.Header.NameOffset;
            }
             */
            package.Header.NameCount -= package.Header.NameOffset;
        }

        private void ReadNames(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading Namelist at {0}....", package.Header.NameOffset);
            reader.BaseStream.Seek(package.Header.NameOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.NameCount; i++)
            {
                GpkString tmpString = new GpkString();
                int len = reader.ReadInt32();
                tmpString.name = ReadString(reader, len);
                tmpString.flags = reader.ReadInt64();

                package.NameList.Add(i, tmpString);

                logger.Info(String.Format("Name {0}: {1}", i, tmpString.name));
            }

        }

        private void ReadImports(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading Imports at {0}....", package.Header.ImportOffset);
            reader.BaseStream.Seek(package.Header.ImportOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ImportCount; i++)
            {
                GpkImport import = new GpkImport();
                long package_class_index = reader.ReadInt64();
                long class_index = reader.ReadInt64();
                import.PackageRef = reader.ReadInt32();
                long object_index = reader.ReadInt32();
                reader.ReadInt32();

                import.ClassPackage = package.NameList[package_class_index].name;
                import.Class = package.NameList[class_index].name;
                import.Object = package.NameList[object_index].name;



                package.ImportList.Add(i, import);
                logger.Info(String.Format("Import {0}: ClassPackage {1} Class: {2} Object: {3}", i, import.ClassPackage, import.Class, import.Object));
            }
        }

        private void ReadExports(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading Exports at {0}....", package.Header.ExportOffset);
            reader.BaseStream.Seek(package.Header.ExportOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ExportCount; i++)
            {
                GpkExport export = new GpkExport();
                export.ClassIndex = reader.ReadInt32();
                export.SuperIndex = reader.ReadInt32();
                export.PackageIndex = reader.ReadInt32();

                long nameIndex = reader.ReadInt32();
                export.Name = package.NameList[nameIndex].name;

                export.Unk1 = reader.ReadInt64();
                export.Unk2 = reader.ReadInt64();

                export.SerialSize = reader.ReadInt32();

                if (export.SerialSize > 0)
                {
                    export.SerialOffset = reader.ReadInt32();
                }
                else
                {
                    logger.Trace(1);
                }

                if (export.ClassIndex < 0)
                {
                    export.ClassIndex = ((export.ClassIndex * -1) - 1);
                    export.ClassName = package.ImportList[export.ClassIndex].Object;
                }

                export.padding_unk = reader.ReadBytes(28);

                package.ExportList.Add(i, export);

                logger.Info(String.Format("Export {0}: Class: {1}, Name: {2}, Data_Size: {3}, Data_Offset {4}, Export_offset {5}", i, export.ClassName, export.Name, export.SerialSize, export.SerialOffset, reader.BaseStream.Position));

            }
        }

        private void ReadExportData(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading ExportsData....");


            foreach (GpkExport export in package.ExportList.Values)
            {
                reader.BaseStream.Seek(export.SerialOffset + 4, SeekOrigin.Begin);

                //Props
                while (true)
                {
                    try
                    {
                        GpkBaseProperty baseProp = new GpkBaseProperty();

                        long nameindex = reader.ReadInt64();
                        baseProp.Name = package.NameList[nameindex].name;

                        if (baseProp.Name.ToLower() == "none") break;

                        baseProp.type = reader.ReadInt64();

                        switch (baseProp.type)
                        {

                            case 0:
                            case 7:
                            case 9:
                            case 119: //aggenom struct
                                GpkArrayProperty tmpArray = new GpkArrayProperty(baseProp);
                                tmpArray.length = reader.ReadInt64();
                                tmpArray.value = new byte[tmpArray.length];
                                tmpArray.value = reader.ReadBytes((int) tmpArray.length);

                                export.Properties.Add(tmpArray);
                                break;

                            case 199: //DiffuseColor
                                GpkGUIDProperty tmpGUID = new GpkGUIDProperty(baseProp);
                                tmpGUID.length = reader.ReadInt64();
                                tmpGUID.unk = reader.ReadInt64();
                                tmpGUID.value = new byte[tmpGUID.length];
                                tmpGUID.value = reader.ReadBytes((int) tmpGUID.length);

                                export.Properties.Add(tmpGUID);
                                break;

                            case 1:
                            case 2:
                            case 18:
                                GpkBoolProperty tmpBool = new GpkBoolProperty(baseProp);
                                tmpBool.unk = reader.ReadInt64();
                                tmpBool.value = Convert.ToBoolean(reader.ReadInt32());
                                export.Properties.Add(tmpBool);
                                break;

                            case 4:
                            case 61:
                            case 160:
                                GpkNameProperty tmpName = new GpkNameProperty(baseProp);
                                tmpName.unk = reader.ReadInt64();
                                long index = reader.ReadInt64();
                                tmpName.value = package.NameList[index].name;

                                export.Properties.Add(tmpName);
                                break;

                            case 28:
                            case 43:
                            case 116:
                                GpkIntProperty tmpInt = new GpkIntProperty(baseProp);
                                tmpInt.unk = reader.ReadInt64();
                                tmpInt.value = reader.ReadInt32();

                                export.Properties.Add(tmpInt);
                                break;

                            case 14:
                            case 15:
                            case 45:
                            case 198:
                                GpkStringProperty tmpString = new GpkStringProperty(baseProp);
                                tmpString.unk = reader.ReadInt64(); //outer len
                                tmpString.length = reader.ReadInt32(); //inner len
                                tmpString.value = ReadString(reader, (int) tmpString.length);

                                export.Properties.Add(tmpString);
                                break;

                            case 80:
                            case 164:
                                //Objectprop
                                GpkObjectProperty tmpObj = new GpkObjectProperty(baseProp);
                                tmpObj.unk = reader.ReadInt64(); //4
                                tmpObj.value = reader.ReadInt32();

                                if (tmpObj.value < 0)
                                {
                                    tmpObj.ClassName = package.ImportList[((tmpObj.value*-1) - 1)].Object;
                                }

                                export.Properties.Add(tmpObj);
                                break;

                            case 95:
                                GpkFloatProperty tmpFloat = new GpkFloatProperty(baseProp);
                                tmpFloat.unk = reader.ReadInt64();
                                tmpFloat.value = reader.ReadSingle();

                                export.Properties.Add(tmpFloat);
                                break;
                            default:
                                throw new Exception(
                                    string.Format(
                                        "Unknown Property Type {0}, Position {1}, Prop_Name {2}, Export_Name {3}",
                                        baseProp.type, reader.BaseStream.Position, baseProp.Name, export.Name));


                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                long object_end = export.SerialOffset + export.SerialSize;
                if (reader.BaseStream.Position < object_end)
                {
                    int toread = (int)(object_end - reader.BaseStream.Position);
                    export.data = new byte[toread];
                    export.data = reader.ReadBytes(toread);
                    logger.Info(String.Format("Export {0}: Read Data ({1} bytes) and {2} Properties", export.Name, export.data.Length, export.Properties.Count));
                }
                else
                {
                    logger.Info(String.Format("Export {0}: Read Data (0 bytes) and {1} Properties", export.Name, export.Properties.Count));
                }

                //data

            }
        }

        private string ReadString(BinaryReader reader, int length)
        {
            string text = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(length - 1));
            reader.ReadByte(); //text control char 

            return text;
        }
    }
}
