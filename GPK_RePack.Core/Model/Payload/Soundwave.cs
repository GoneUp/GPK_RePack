using System;
using System.IO;
using System.Text;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model.Payload
{
    [Serializable]
    public class Soundwave : IPayload
    {
        public string oggfile;
        public byte[] oggdata;

        //http://forums.nexusmods.com/index.php?/topic/1964864-sound-replacement-possible/#entry18577584
        public void ReadData( GpkPackage package, GpkExport export)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));

            reader.ReadBytes(20); //(12bytes 00)
            int ogg_length1 = reader.ReadInt32();
            int ogg_length2 = reader.ReadInt32();
            int offset1 = reader.ReadInt32();
            oggdata = new byte[ogg_length1];
            oggdata = reader.ReadBytes(ogg_length1);

            reader.ReadBytes(32); //2x(12bytes 00 + offset +4)
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            if (oggdata == null)
            {
                throw new Exception("Oggdata is null. Object: " + export.UID);
            }

            writer.Write(new byte[12]);
            writer.Write((int)writer.BaseStream.Position + 4);
            writer.Write(0);
            writer.Write(oggdata.Length);
            writer.Write(oggdata.Length);
            writer.Write((int)writer.BaseStream.Position + 4);

            writer.Write(oggdata);

            writer.Write(new byte[12]);
            writer.Write((int)writer.BaseStream.Position + 4);
            
            writer.Write(new byte[12]);
            writer.Write((int)writer.BaseStream.Position + 4);
        }



        public int GetSize()
        {
            if (oggdata != null)
            {
                return oggdata.Length + 64;
            }

            return 0;
        }

        public string GetClassIdent()
        {
            return "Core.SoundNodeWave";
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: " + GetClassIdent());
            if (oggdata != null)  info.AppendLine("Ogg data: " + oggdata.Length);
            return info.ToString();
        }

    }
}
