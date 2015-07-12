using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes;
using GPK_RePack.Classes.Interfaces;
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
            wave.WriteData(writer, null, export);
            writer.Close();
            writer.Dispose();


            export.GetDataSize();

            if (oggfile == "fake")
            {
                logger.Info(String.Format("Fake soundfile was imported to {0}!", export.UID));
                return;
            }

            //manipulate the sound duration
            VorbisReader vorbis = null;
            vorbis = new VorbisReader(new MemoryStream(wave.oggdata), true);

            /*
             * ObjectName: Duration Type: FloatProperty Value: 0,4694785
ObjectName: NumChannels Type: IntProperty Value: 1
ObjectName: SampleRate Type: IntProperty Value: 22050
ObjectName: SampleDataSize Type: IntProperty Value: 20704
*/

            foreach (IProperty prop in export.Properties)
            {
                if (prop is GpkFloatProperty)
                {
                    GpkFloatProperty floatProperty = (GpkFloatProperty)prop;
                    if (floatProperty.name == "Duration")
                    {
                        floatProperty.value = (float)vorbis.TotalTime.TotalSeconds;
                    }
                }
                else if (prop is GpkIntProperty)
                {
                    GpkIntProperty intProperty = (GpkIntProperty)prop;
                    if (intProperty.name == "NumChannels")
                    {
                        intProperty.value = vorbis.Channels;
                    }
                    if (intProperty.name == "SampleRate")
                    {
                        intProperty.value = vorbis.SampleRate;
                    }
                    if (intProperty.name == "SampleDataSize")
                    {
                        intProperty.value = wave.oggdata.Length;
                    }
                }
            }

            logger.Info(String.Format("Soundfile was imported to {0}!", export.UID));
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


        public static void SetAllVolumes(GpkPackage package, Single volume)
        {
            List<GpkExport> exports = package.GetExportsByClass(new SoundCue().GetClassIdent());

            SetPropertyDetails(exports, "VolumeMultiplier", volume.ToString());
        }

        public static void SetPropertyDetails(List<GpkExport> exports, string name, string input)
        {
            foreach (GpkExport export in exports)
            {
                foreach (IProperty prop in export.Properties)
                {
                    string tmpName = ((GpkBaseProperty) prop).name;

                    if (tmpName == name)
                    {
                        bool success = prop.SetValue(input);
                        if (!success)
                        {
                            logger.Info("Failed to update property. UID {0} PropName {1} Value {2} RealPropInfo {3}", export.UID, name, input, prop);
                        }
                        break;
                    }
                }
            }
        }
    }
}
