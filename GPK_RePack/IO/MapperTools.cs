using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using NLog;

namespace GPK_RePack.IO
{
    class MapperTools
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void DecryptFile(string input, string output)
        {
            try
            {
                byte[] data = File.ReadAllBytes(input);
                int offset = 0;

                // Unscramble (1)
                for (offset = 0; offset < data.Length; offset += 16)
                {
                    if (offset + 16 < data.Length)
                    {
                        byte[] copy = new byte[16];
                        Array.ConstrainedCopy(data, offset, copy, 0, 16);

                        data[offset + 0] = copy[12];
                        data[offset + 1] = copy[6];
                        data[offset + 2] = copy[9];
                        data[offset + 3] = copy[4];
                        data[offset + 4] = copy[3];
                        data[offset + 5] = copy[14];
                        data[offset + 6] = copy[1];
                        data[offset + 7] = copy[10];
                        data[offset + 8] = copy[13];
                        data[offset + 9] = copy[2];
                        data[offset + 10] = copy[7];
                        data[offset + 11] = copy[15];
                        data[offset + 12] = copy[0];
                        data[offset + 13] = copy[8];
                        data[offset + 14] = copy[5];
                        data[offset + 15] = copy[11];
                    }
                }

                // Unscramble (2)
                //int divison, floor it
                if ((data.Length / 2) > 0)
                {
                    var offset1 = 1;
                    var offset2 = data.Length - 1;

                    for (int i = ((data.Length / 2) + 1) / 2; i > 0; i--)
                    {
                        var tmp = data[offset1];
                        data[offset1] = data[offset2];
                        data[offset2] = tmp;
                        offset1 += 2;
                        offset2 -= 2;
                    }

                }

                // Decrypt
                string key = "GeneratePackageMapper";

                offset = 0;
                while (offset < data.Length)
                {
                    for (int i = 0; i < key.Length; i++)
                    {
                        if (offset + i < data.Length)
                        {
                            data[offset + i] ^= Convert.ToByte(key[i]);
                        }
                    }

                    offset += key.Length;
                }

                File.WriteAllBytes(output, data);


            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}
