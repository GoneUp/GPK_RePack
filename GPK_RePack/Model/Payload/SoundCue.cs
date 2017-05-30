using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class SoundCue : IPayload
    {
        public List<SoundCueObject> cues = new List<SoundCueObject>();

        public void ReadData(GpkPackage package, GpkExport export)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                SoundCueObject cue = new SoundCueObject();
                cue.objectName = package.GetObjectName(reader.ReadInt32());
                cue.Unk2 = reader.ReadInt32();
                cue.Unk3 = reader.ReadInt32();

                cues.Add(cue);
            }
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            writer.Write(cues.Count);
            foreach (SoundCueObject cue in cues)
            {
                writer.Write((int)package.GetObjectIndex(cue.objectName));
                writer.Write(cue.Unk2);
                writer.Write(cue.Unk3);
            }
        }

        public int GetSize()
        {
            int size = 4; //len
            size += (12*cues.Count);
            return size;
        }

        public string GetClassIdent()
        {
            return "Core.SoundCue";
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: " + GetClassIdent());
            info.AppendLine("Cues: " + cues.Count);
            foreach (SoundCueObject cue in cues)
            {
                info.AppendLine(cue.ToString());
            }
            return info.ToString();
        }
    }
}
