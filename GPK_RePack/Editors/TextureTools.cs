using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Model;
using GPK_RePack.Model.Payload;
using UpkManager.Dds;

namespace GPK_RePack.Editors
{
    class TextureTools
    {
        public static void exportTexture(GpkExport export, string file)
        {
            Texture2D image = (Texture2D)export.Payload;
            DdsFile ddsFile = new DdsFile();

            Task.Run(() => image.SaveObject(file, new DdsSaveConfig(image.GetFormat(), 0, 0, false, false)));
        }

        public static void importTexture(GpkExport export, string file)
        {
            var texture2d = export.Payload as Texture2D;

            var image = new DdsFile();
            var config = new DdsSaveConfig(texture2d.GetFormat(), 0, 0, false, false);
            image.Load(file);

            if (image.MipMaps.Count == 0 || Settings.Default.GenerateMipMaps)
                image.GenerateMipMaps();
         

            texture2d.maps = new List<MipMap>();
            foreach (DdsMipMap mipMap in image.MipMaps.OrderByDescending(mip => mip.Width))
            {
                byte[] outputData = image.WriteMipMap(mipMap, config);

                var textureMipMap = new MipMap();
                textureMipMap.compFlag = (int) CompressionTypes.LZO;
                textureMipMap.uncompressedData = outputData;
                textureMipMap.uncompressedSize = outputData.Length;
                textureMipMap.uncompressedSize_chunkheader = outputData.Length;
                textureMipMap.sizeX = mipMap.Width;
                textureMipMap.sizeY = mipMap.Height;

                textureMipMap.generateBlocks();
                texture2d.maps.Add(textureMipMap);
            }

        }
    }
}
