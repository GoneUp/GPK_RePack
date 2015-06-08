using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class;

namespace GPK_RePack.Saver
{
    class Save
    {
        public static void SaveReplacedExport(GpkPackage package, string savepath, List<GpkExport> changedExports)
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
    }
}
