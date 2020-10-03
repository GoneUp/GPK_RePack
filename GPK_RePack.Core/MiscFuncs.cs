﻿using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GPK_RePack.Core
{
    public static class MiscFuncs
    {
        private static readonly Random Randomizer = new Random((int)DateTime.Now.Ticks);

        public static Random Random()
        {
            return Randomizer;
        }

        public static int GetRoundedUtc()
        {
            // ReSharper disable PossibleLossOfFraction
            return (int)Math.Round((double)(GetCurrentMilliseconds() / 1000));
            // ReSharper restore PossibleLossOfFraction
        }

        private static readonly DateTime StaticDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentMilliseconds()
        {
            return (long)(DateTime.UtcNow - StaticDate).TotalMilliseconds;
        }

        private static readonly string[] Baths;

        static MiscFuncs()
        {
            Baths = new string[256];
            for (int i = 0; i < 256; i++)
                Baths[i] = String.Format("{0:X2}", i);
        }

        public static string ToHex(this byte[] array)
        {
            StringBuilder builder = new StringBuilder(array.Length * 2);

            for (int i = 0; i < array.Length; i++)
                builder.Append(Baths[array[i]]);

            return builder.ToString();
        }

        public static string FormatHex(this byte[] data)
        {
            StringBuilder builder = new StringBuilder(data.Length * 4);

            int count = 0;
            int pass = 1;
            foreach (byte b in data)
            {
                if (count == 0)
                    builder.AppendFormat("{0,-6}\t", "[" + (pass - 1) * 16 + "]");

                count++;
                builder.Append(b.ToString("X2"));
                if (count == 4 || count == 8 || count == 12)
                    builder.Append(" ");
                if (count == 16)
                {
                    builder.Append("\t");
                    for (int i = (pass * count) - 16; i < (pass * count); i++)
                    {
                        char c = (char)data[i];
                        if (c > 0x1f && c < 0x80)
                            builder.Append(c);
                        else
                            builder.Append(".");
                    }
                    builder.Append("\r\n");
                    count = 0;
                    pass++;
                }
            }

            //Last line Hex text Adding
            while (count < 16)
            {
                builder.Append("\t");
                count += 3;
            }

            for (int i = (pass * 16) - 16; i < (data.Length - 1); i++)
            {
                char c = (char)data[i];
                if (c > 0x1f && c < 0x80)
                    builder.Append(c);
                else
                    builder.Append(".");
            }
            builder.Append("\r\n");


            return builder.ToString();
        }

        public static byte[] ToBytes(this String hexString)
        {
            try
            {
                byte[] result = new byte[hexString.Length / 2];

                for (int index = 0; index < result.Length; index++)
                {
                    string byteValue = hexString.Substring(index * 2, 2);
                    result[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return result;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid hex string: {0}", hexString);
                throw;
            }
        }

        public static bool IsLuck(byte chance)
        {
            if (chance >= 100)
                return true;

            if (chance <= 0)
                return false;

            return new Random().Next(0, 100) <= chance;
        }

        public static string GenerateSaveDialog(string filename, string ext)
        {
            SaveFileDialog save = new SaveFileDialog(); 
            save.FileName = filename;
            save.DefaultExt = ext;
            save.InitialDirectory = CoreSettings.Default.SaveDir;
            var result = save.ShowDialog();

            if (result == DialogResult.OK)
            {
                CoreSettings.Default.SaveDir = save.FileName;
                return save.FileName; 
            }
            return "";
        }

        [STAThread]
        public static String[] GenerateOpenDialog(bool multiselect, bool useWorkingDir = true, string filter = "")
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = multiselect;
            open.ValidateNames = true;
            if (filter != "") open.Filter = filter;
            open.InitialDirectory = useWorkingDir ? CoreSettings.Default.WorkingDir : CoreSettings.Default.OpenDir;
            open.ShowDialog();

            if (File.Exists(open.FileName))
            {
                if (useWorkingDir)
                {
                    CoreSettings.Default.WorkingDir = Path.GetDirectoryName(open.FileName);
                } else
                {
                    CoreSettings.Default.OpenDir = Path.GetDirectoryName(open.FileName);
                }
                
            }
                
            return open.FileNames;
        }

        public class NodeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                TreeNode tx = (TreeNode)x;
                TreeNode ty = (TreeNode)y;

                return string.Compare(tx.Text, ty.Text);
            }
        }
    }
}