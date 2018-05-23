using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GPK_RePack.Model;
using GPK_RePack.Model.Payload;
using GPK_RePack.Properties;
using NLog;
using UpkManager.Dds;

namespace GPK_RePack.Editors
{
    class TextureTools
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void exportTexture(GpkExport export, string file)
        {
            Texture2D image = (Texture2D)export.Payload;
            DdsFile ddsFile = new DdsFile();

            Task.Run(() => image.SaveObject(file, new DdsSaveConfig(image.GetFormat(), 0, 0, false, false)));
        }

        public static void importTexture(GpkExport export, string file)
        {
            try
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
                    textureMipMap.compFlag = (int)CompressionTypes.LZO;
                    textureMipMap.uncompressedData = outputData;
                    textureMipMap.uncompressedSize = outputData.Length;
                    textureMipMap.uncompressedSize_chunkheader = outputData.Length;
                    textureMipMap.sizeX = mipMap.Width;
                    textureMipMap.sizeY = mipMap.Height;

                    textureMipMap.generateBlocks();
                    texture2d.maps.Add(textureMipMap);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to import texture");
            }

        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

    }
}
