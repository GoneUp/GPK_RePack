using System.Collections.Generic;
using System.IO;
using GPK_RePack.Core.Model;
using NLog;

namespace GPK_RePack.Core.Editors
{
    public class DataTools
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void ReplaceAll(GpkExport source, GpkExport destination)
        {
            GpkPackage sourcePackage = source.motherPackage;
            GpkPackage destinantionPackage = destination.motherPackage;

            //exclude: motherPackage, guid, uid, SerialOffset, SerialOffsetPosition, ObjectName
            destination.ClassName = source.ClassName;
            destination.SuperName = source.SuperName;
            destination.PackageName = source.PackageName;
            //destination.ObjectName = source.ObjectName;

            destination.Unk1 = source.Unk1;
            destination.Unk2 = source.Unk2;

            destination.SerialSize = source.SerialSize;

            destination.NetIndex = source.NetIndex;
            destination.NetIndexName = source.NetIndexName;

            destination.Unk3 = source.Unk3;
            destination.UnkHeaderCount = source.UnkHeaderCount;
            destination.Unk4 = source.Unk4;
            destination.UnkExtraInts = source.UnkExtraInts;

            destination.DependsTableData = source.DependsTableData;

            ReplaceProperties(source, destination);
            ReplaceData(source, destination);

            //regenerate uid
            destinantionPackage.GenerateUID(destination);

            //copy deps
            destinantionPackage.CopyObjectFromPackage(source.ClassName, sourcePackage, false);
            destinantionPackage.CopyObjectFromPackage(source.PackageName, sourcePackage, false);
            destinantionPackage.CopyObjectFromPackage(source.SuperName, sourcePackage, false);
            destinantionPackage.CopyObjectFromPackage(source.NetIndexName, sourcePackage, false);
        }
        public static void ReplaceProperties(GpkExport source, GpkExport destination)
        {
            destination.Properties.Clear();
            destination.Properties.AddRange(source.Properties.ToArray());
            destination.PropertyPadding = source.PropertyPadding;
            destination.PropertySize = source.PropertySize;
            destination.PropertyPadding = source.PropertyPadding;
        }

        public static void ReplaceData(GpkExport source, GpkExport destination)
        {
            destination.DataPadding = source.DataPadding;
            destination.Data = source.Data;
            destination.Payload = source.Payload;
        }

        public static void WriteExportDataFile(string path, GpkExport export)
        {
            WriteExportDataFile(path, export.Data);
        }

        public static void WriteExportDataFile(string path, byte[] data)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            StreamWriter writer = new StreamWriter(File.OpenWrite(path));
            writer.BaseStream.Write(data, 0, data.Length);
            writer.Close();
            writer.Dispose();

        }

        public static void RemoveObjectRedirects(GpkPackage package)
        {
            var keys = new List<long>();
            foreach (KeyValuePair<long, GpkExport> pair in package.ExportList)
            {
                if (pair.Value.ClassName == "Core.ObjectRedirector")
                {
                    keys.Add(pair.Key);
                }
            }

            foreach (long key in keys)
            {
                package.ExportList.Remove(key);
            }

            logger.Info("Removed {0} exports from package {1}", keys.Count, package.Filename);
        }

    }
}
