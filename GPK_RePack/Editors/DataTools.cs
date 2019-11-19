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
