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

        public void ReadGpk(string path)
        {
            logger.Info("Reading: " + path);

            GpkPackage package = new GpkPackage();

            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                ReadHeader(reader, package);
                ReadNames(reader, package);
                ReadImports(reader, package);
                ReadExports(reader, package);
                ReadExportData(reader, package);
            }

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
            package.Header.PackageName = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(len));

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

                logger.Info("Generation {0}, ExportCount {1}, NameCount {2}, NetObjectCount {3}", i, tmpgen.ExportCount, tmpgen.NameCount, tmpgen.NetObjectCount);

                package.Header.Genrations.Add(tmpgen);
            }


            package.Header.Unk3 = reader.ReadInt32();
            package.Header.Unk4 = reader.ReadInt32();
            package.Header.Unk5 = reader.ReadInt32();
            package.Header.Unk6 = reader.ReadInt32();

            package.Header.EngineVersion = reader.ReadInt32();
            package.Header.CookerVersion = reader.ReadInt32();

            logger.Info("Unk3 {0}, Unk4 {1}, Unk5 {2}, Unk6 {3}, EngineVersion {4}, CookerVersion {5}",
                package.Header.Unk3, package.Header.Unk4, package.Header.Unk5, package.Header.Unk6, package.Header.EngineVersion, package.Header.CookerVersion);
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
            logger.Info("Reading Namelist....");
            reader.BaseStream.Seek(package.Header.NameOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.NameCount; i++)
            {
                GpkString tmpString = new GpkString();
                int len = reader.ReadInt32();
                tmpString.name = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(len));
                tmpString.flags = reader.ReadInt64();

                package.NameList.Add(i, tmpString);

                logger.Info("Name {0}: {1}", i, tmpString.name);
            }

        }

        private void ReadImports(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading Imports....");
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

                logger.Info("Import {0}: ClassPackage {1}, Class: {2}, Object: {3}", i, import.ClassPackage, import.Class, import.Object);
            }
        }

        private void ReadExports(BinaryReader reader, GpkPackage package)
        {
            logger.Info("Reading Exports....");
            reader.BaseStream.Seek(package.Header.ExportOffset, SeekOrigin.Begin);

            for (int i = 0; i < package.Header.ExportCount; i++)
            {
                GpkExport export = new GpkExport();
                export.ClassIndex = reader.ReadInt32();
                export.SuperIndex = reader.ReadInt32();
                export.PackageIndex = reader.ReadInt32();

                long nameIndex = reader.ReadInt64();
                export.Name = package.NameList[nameIndex].name;

                export.Unk1 = reader.ReadInt64();
                export.Unk2 = reader.ReadInt32();

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
               

                package.ExportList.Add(i, export);

                logger.Info("Export {0}: Class: {1}, Name: {2}, Size: {3}, Offset {4}", i, export.ClassName, export.Name, export.SerialSize, export.SerialOffset);
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
                    GpkBaseProperty baseProp = new GpkBaseProperty();

                    baseProp.NameIndex = reader.ReadInt64();
                    baseProp.Name = package.NameList[baseProp.NameIndex].name;

                    if (baseProp.Name == "none") break;

                    baseProp.type = (PropertyTypes)reader.ReadInt64();

                    switch (baseProp.type)
                    {
                        case PropertyTypes.BoolProp:
                            GpkBoolProperty tmpBool = (GpkBoolProperty)baseProp;
                            tmpBool.unk = reader.ReadInt64();
                            tmpBool.value = Convert.ToBoolean(reader.ReadInt32());
                            export.Properties.Add(tmpBool);
                            break;

                        case PropertyTypes.IntProp:
                            GpkIntProperty tmpInt = (GpkIntProperty)baseProp;
                            tmpInt.unk = reader.ReadInt64();
                            tmpInt.value = reader.ReadInt32();

                            export.Properties.Add(tmpInt);
                            break;

                        case PropertyTypes.NameProp:
                            GpkNameProperty tmpName = (GpkNameProperty)baseProp;
                            tmpName.unk = reader.ReadInt64();
                            long index = reader.ReadInt64();
                            tmpName.value = package.NameList[index].name;

                            export.Properties.Add(tmpName);
                            break;

                        case PropertyTypes.StringProp:
                            GpkStringProperty tmpString = (GpkStringProperty)baseProp;
                            tmpString.unk = reader.ReadInt64();
                            tmpString.length = reader.ReadInt64();
                            tmpString.value = ASCIIEncoding.ASCII.GetString(reader.ReadBytes((int)tmpString.length));

                            export.Properties.Add(tmpString);
                            break;

                        case PropertyTypes.ArrayProp:
                            GpkArrayProperty tmpArray = (GpkArrayProperty)baseProp;
                            tmpArray.length = reader.ReadInt64();
                            tmpArray.data = new byte[tmpArray.length];
                            tmpArray.data = reader.ReadBytes((int)tmpArray.length);

                            export.Properties.Add(tmpArray);
                            break;
                    }
                }


                //data

                //logger.Info("Export {0}: Class: {1}, Name: {2}, Size: {3}, Offset {4}", i, export.ClassName, export.Name, export.SerialSize, export.SerialOffset);
            }
        }
    }
}
