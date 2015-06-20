using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes;

namespace GPK_RePack.Editors
{
    class DataTools
    {

        public static void ReplaceProperties(GpkExport source, GpkExport destination)
        {
            destination.Properties.Clear();
            destination.Properties.AddRange(source.Properties.ToArray());
            destination.property_padding = source.property_padding;
            destination.property_size = source.property_size;
            destination.padding_unk = source.padding_unk;
        }

        public static void ReplaceData(GpkExport source, GpkExport destination)
        {
            destination.data_padding = source.data_padding;
            destination.data = source.data;
            destination.payload = source.payload;
        }

        public static void WriteExportDataFile(string path, GpkExport export)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            StreamWriter writer = new StreamWriter(File.OpenWrite(path));
            writer.BaseStream.Write(export.data, 0, export.data.Length);
            writer.Close();
            writer.Dispose();

        }
    }
}
