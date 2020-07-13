using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GPK_RePack.Model;
using GPK_RePack.Model.Payload;
using GPK_RePack.Model.Prop;
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
            try
            {
                Texture2D image = (Texture2D)export.Payload;
                DdsFile ddsFile = new DdsFile();

                if (image == null || ddsFile == null)
                    return;

                image.SaveObject(file, new DdsSaveConfig(image.GetFormat(), 0, 0, false, false));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to export texture");
                logger.Error(ex);
            }
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
                    //textureMipMap.compFlag = 0;
                    textureMipMap.uncompressedData = outputData;
                    textureMipMap.uncompressedSize = outputData.Length;
                    textureMipMap.uncompressedSize_chunkheader = outputData.Length;
                    textureMipMap.sizeX = mipMap.Width;
                    textureMipMap.sizeY = mipMap.Height;

                    textureMipMap.generateBlocks();
                    texture2d.maps.Add(textureMipMap);
                }

                int mipTailBaseIdx = (int)Math.Log(image.Width > image.Height ? image.Width : image.Height, 2);
                ((GpkIntProperty)export.GetProperty("MipTailBaseIdx")).SetValue(mipTailBaseIdx.ToString());

                logger.Info("Imported image from {0}, size {1}x{2}, target format {3}, mipTailBaseIdx {4}", file, image.Width, image.Height, config.FileFormat, mipTailBaseIdx);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to import texture");
                logger.Error(ex);
            }

        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            //convert image pixel format:
            var bs32 = new FormatConvertedBitmap(); //inherits from BitmapSource
            bs32.BeginInit();
            bs32.Source = bitmapsource;
            bs32.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            bs32.EndInit();
            //source = bs32;

            //now convert it to Bitmap:
            Bitmap bmp = new Bitmap(bs32.PixelWidth, bs32.PixelHeight, PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            bs32.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

    }
}
