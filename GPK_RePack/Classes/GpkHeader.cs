using System.Collections.Generic;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes
{
    class GpkHeader : IGpkPart
    {
        public int Tag;
        public short FileVersion;
        public short LicenseVersion;

        public int PackageFlags;
        public string PackageName;

        public short Unk1;
        public short Unk2;

        public int NameCount, NameOffset;
        public int ExportCount, ExportOffset;
        public int ImportCount, ImportOffset;
        public int DependsOffset;
        
        public byte[] FGUID = new byte[16];

        public List<GpkGeneration> Genrations = new List<GpkGeneration>();

        public int Unk3, Unk4, Unk5, Unk6;
        public int EngineVersion;
        public int CookerVersion;


        public int GetSize()
        {
            return 109;
        }
    }
}
