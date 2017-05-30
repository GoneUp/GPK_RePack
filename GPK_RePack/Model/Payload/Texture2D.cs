using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GPK_RePack.IO;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Prop;
using LZ4;
using NLog;

namespace GPK_RePack.Model.Payload
{


    [Serializable]
    class Texture2D : IPayload
    {


        public byte[] startUnk;
        public String tgaPath;
        public byte[] endUnk;

        public List<MipMap> maps = new List<MipMap>();

        public string GetClassIdent()
        {
            return "Core.Texture2D";
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            throw new NotImplementedException();
        }

        public void ReadData(GpkPackage package, GpkExport export)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));
            IProperty formatProp = export.Properties.Find(t => ((GpkBaseProperty)t).name == "Format");
            String format = ((GpkByteProperty)formatProp).nameValue;

            startUnk = reader.ReadBytes(16);
            int len = reader.ReadInt32();
            tgaPath = Reader.ReadString(reader, len);

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                MipMap map = new MipMap();

                map.compFlag = reader.ReadInt32();
                map.uncompressedSize = reader.ReadInt32();
                map.widthOffset = reader.ReadInt32();

                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();

                map.requiredBuffer = reader.ReadInt32();

                map.compressedSize = reader.ReadInt32();
                map.uncompressedDataSize = reader.ReadInt32();

                //should be the same?
                int compressedSize2 = reader.ReadInt32();
                int uncompressedDataSize2 = reader.ReadInt32();

                Debug.Assert(map.compressedSize == compressedSize2);
                Debug.Assert(map.uncompressedDataSize == uncompressedDataSize2);

                map.compressedData = reader.ReadBytes(map.compressedSize);

                map.sizeX = reader.ReadInt32();
                map.sizeY = reader.ReadInt32();

                map.decompress();
                maps.Add(map);
            }

            endUnk = reader.ReadBytes(16);
        }


     

        public int GetSize()
        {
            throw new NotImplementedException();
        }


    }
}
