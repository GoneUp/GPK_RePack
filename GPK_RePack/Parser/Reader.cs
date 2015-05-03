using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class;
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

        }

        private void ReadExports(BinaryReader reader, GpkPackage package)
        {

        }

        private void ReadExportData(BinaryReader reader, GpkPackage package)
        {

        }
    }
}
