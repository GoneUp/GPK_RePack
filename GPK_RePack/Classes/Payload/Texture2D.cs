using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Prop;
using GPK_RePack.IO;

namespace GPK_RePack.Classes.Payload
{
    [Serializable]
    class Texture2D : IPayload
    {
        public byte[] startUnk = new byte[16];

        public string GetClassIdent()
        {
            throw new NotImplementedException();
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            throw new NotImplementedException();
        }

        public void ReadData(GpkPackage package, GpkExport export)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));
            IProperty formatProp = export.Properties.Find(t => ((GpkBaseProperty) t).name == "Format");
            String format = ((GpkByteProperty) formatProp).nameValue;

            reader.ReadBytes(16);
            int len = reader.ReadInt32()*-1 *2;
            string name = Reader.ReadUnicodeString(reader, len);

            int mipMapSize = reader.ReadInt32();


            
        }

        public int GetSize()
        {
            throw new NotImplementedException();
        }
    }
}
