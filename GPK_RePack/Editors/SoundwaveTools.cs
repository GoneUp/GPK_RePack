using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class;
using NLog;

namespace GPK_RePack.Editors
{
    class SoundwaveTools
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static GpkExport ImportOgg(GpkExport export, string oggfile)
        {
            return null;
        }

        public static void ExportOgg(GpkExport export, string oggfile)
        {
            if (export.data == null)
            {
                logger.Info("No data. The file cannot be exported to ogg.");
                return;
            }

            if (export.data.Length < 32)
            {
                logger.Info("Data size too small. The file cannot be exported to ogg.");
                return;
            }

            byte[] tmpArray = new byte[export.data.Length - 32];
            Array.Copy(export.data, 32, tmpArray, 0, tmpArray.Length);

            StreamWriter writer = new StreamWriter(File.OpenWrite(oggfile));
            writer.BaseStream.Write(tmpArray, 0, tmpArray.Length);
            writer.Close();
            writer.Dispose();

            logger.Info(String.Format("Data was saved to {0}!", Path.GetFileName(oggfile)));
        }
    }
}
