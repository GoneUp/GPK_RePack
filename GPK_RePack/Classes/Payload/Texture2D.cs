using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes.Payload
{
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
            
        }

        public int GetSize()
        {
            throw new NotImplementedException();
        }
    }
}
