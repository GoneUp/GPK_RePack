using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model;

namespace GPK_RePack.Editors
{
    class DataTools
    {

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

    }
}
