using System;
using System.Collections.Generic;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model
{
    [Serializable]
    public class GpkHeader : IGpkPart
    {
        public int Tag;
        public short FileVersion;
        public short LicenseVersion;

        public int PackageFlags;
        public string PackageName;

        public int NameCount, NameOffset;
        public int ExportCount, ExportOffset;
        public int ImportCount, ImportOffset;
        public int DependsOffset;

        public int HeaderSize; //x64 gpk
        public byte[] Unk1 = new byte[12];//x64 gpk
        public byte[] FGUID = new byte[16];

        public List<GpkGeneration> Generations = new List<GpkGeneration>();

        public int EngineVersion;
        public int CookerVersion;
        public int CompressionFlags;

        public int EstimatedChunkHeaderCount; 
        public List<GpkCompressedChunkHeader> ChunkHeaders = new List<GpkCompressedChunkHeader>();

        public byte[] HeaderPadding = new byte[0];

        public void RecalculateCounts(GpkPackage package)
        {
            NameCount = package.NameList.Count;
            ExportCount = package.ExportList.Count;
            ImportCount = package.ImportList.Count;
        }

        public bool IsCompressed()
        {
            return CompressionFlags != 0;
        }


        public int GetSize()
        {
            //must be uber correct, otherwise pkg compression is borked
            int size = FileVersion >= 0x381 ? 117 : 109;
            if (CoreSettings.Default.EnableCompression)
                size += 16 * ChunkHeaders.Count;
            size += HeaderPadding.Length;
            return size;
        }
    }
}
