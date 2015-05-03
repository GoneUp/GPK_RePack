using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Class.Prop;

namespace GPK_RePack.Class
{
    class GpkExport
    {
        public int ClassIndex;
        public int SuperIndex;
        public int PackageIndex;

        public string Name;

        public long Unk1;
        public int Unk2;

        public int SerialSize;
        public int SerialOffset;

        //28 byte byte padding? + 4 vor letztem

        public List<GpkBaseProperty> Properties;
        public byte[] data;

        public string ClassName;

        public GpkExport()
        {
            Properties = new List<GpkBaseProperty>();
        }

    }
}
