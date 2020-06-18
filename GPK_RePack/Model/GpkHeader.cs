using System;
using System.Collections.Generic;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model
{
    [Serializable]
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

        public byte[] Unk3 = new byte[16];//x64 gpk
        public byte[] FGUID = new byte[16];

        public List<GpkGeneration> Generations = new List<GpkGeneration>();

        public int EngineVersion;
        public int CookerVersion;
        public int CompressionFlags;

        public List<GpkCompressedChunkHeader> ChunkHeaders = new List<GpkCompressedChunkHeader>();

        public void RecalculateCounts(GpkPackage package)
        {
            NameCount = package.NameList.Count;
            ExportCount = package.ExportList.Count;
            ImportCount = package.ImportList.Count;
        }


        public int GetSize()
        {
            return FileVersion >= 0x381 ? 125 : 109;
        }
    }
}
