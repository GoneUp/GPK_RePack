using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes;
using GPK_RePack.Classes.Payload;
using GPK_RePack.Classes.Prop;
using NLog;
using NVorbis;

namespace GPK_RePack.Editors
{
    class SoundwaveTools
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void ImportOgg(GpkExport export, string oggfile)
        {
            Soundwave wave = new Soundwave();
            wave.oggfile = oggfile;

            if (oggfile != "fake")
            {
                wave.oggdata = File.ReadAllBytes(oggfile);
            }
            else
            {
                wave.oggdata = new byte[1];
            }

            export.payload = wave;

            //Refill data buffer with normal soundwave
            export.data = new byte[wave.GetSize()];
            BinaryWriter writer = new BinaryWriter(new MemoryStream(export.data));
            wave.WriteData(writer, new GpkPackage(), export);
            writer.Close();
            writer.Dispose();


            export.RecalculateSize();


            //manipulate the sound duration
            VorbisReader vorbis = null;
            if (oggfile != "fake")
                vorbis = new VorbisReader(new MemoryStream(wave.oggdata), true);

            for (int i = 0; i < export.Properties.Count; i++)
            {
                object prop = export.Properties[i];
                if (prop is GpkFloatProperty)
                {
                    GpkFloatProperty floatProperty = (GpkFloatProperty) prop;
                    if (floatProperty.Name == "Duration")
                    {
                        if (oggfile != "fake")
                        {
                            floatProperty.value = (float) vorbis.TotalTime.TotalSeconds;
                        }
                        else
                        {
                            floatProperty.value = 0.1f;
                        }
                        break;
                    }
                }



                if (oggfile != "fake")
                {
                    logger.Info(String.Format("Soundfile was imported to {0}!", export.UID));
                }
                else
                {
                    logger.Info(String.Format("Fake soundfile was imported to {0}!", export.UID));
                }
            }
        }

        public static void ExportOgg(GpkExport export, string oggfile)
        {
            if (export.payload == null)
            {
                logger.Info("No data. The file cannot be exported to ogg.");
                return;
            }

            if (!(export.payload is Soundwave))
            {
                logger.Info("Wrong payload data. The file cannot be exported to ogg.");
                return;
            }

            Soundwave wave = (Soundwave)export.payload;

            if (wave.oggdata == null)
            {
                logger.Info("Empty Oggdata. The file cannot be exported to ogg.");
                return;
            }


            StreamWriter writer = new StreamWriter(File.OpenWrite(oggfile));
            writer.BaseStream.Write(wave.oggdata, 0, wave.oggdata.Length);
            writer.Close();
            writer.Dispose();

            logger.Info(String.Format("Data was saved to {0}!", Path.GetFileName(oggfile)));
        }
    }
}
