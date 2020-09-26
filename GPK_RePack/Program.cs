using System;
using System.Windows.Forms;
using GPK_RePack.Core;
using GPK_RePack.Forms;

namespace GPK_RePack
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CoreSettings.Load();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI());
        }

        static Program()
        {
            ResourceExtractor.ExtractResourceToFile("GPK_RePack.Resources.lib64.lzo2_64.dll", "lib64\\lzo2_64.dll");
            ResourceExtractor.ExtractResourceToFile("GPK_RePack.Resources.lib64.msvcr100.dll", "lib64\\msvcr100.dll");
     
            ResourceExtractor.ExtractResourceToFile("GPK_RePack.Resources.lib32.lzo2.dll", "lib32\\lzo2.dll");
            ResourceExtractor.ExtractResourceToFile("GPK_RePack.Resources.lib32.msvcr100.dll", "lib32\\msvcr100.dll");
        }
    }
}
