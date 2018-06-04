using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Properties;

namespace GPK_RePack.Forms
{
    public partial class Options : Form
    {
        private GUI gui = null;
        public Options(GUI tmpGui)
        {
            gui = tmpGui;
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            switch (Settings.Default.CopyMode)
            {
                case "dataprops":
                    btnDataProps.Checked = true;
                    break;
                case "data":
                    btnData.Checked = true;
                    break;
                case "props":
                    btnProperties.Checked = true;
                    break;
            }

            switch (Settings.Default.LogLevel)
            {
                case "info":
                    btnLogInfo.Checked = true;
                    break;
                case "trace":
                    btnLogTrace.Checked = true;
                    break;
                case "debug":
                    btnLogDebug.Checked = true;
                    break;
            }

            switch (Settings.Default.ViewMode)
            {
                case "normal":
                    btnViewNormal.Checked = true;
                    break;
                case "class":
                    btnViewClass.Checked = true;
                    break;
                case "package":
                    btnViewPack.Checked = true;
                    break;
            }

            boxDebug.Checked = Settings.Default.Debug;
            boxImports.Checked = Settings.Default.ShowImports;
            boxPatchmode.Checked = Settings.Default.PatchMode;
            boxUseUID.Checked = Settings.Default.UseUID;
            boxJitData.Checked = Settings.Default.JitData;
            boxGenerateMipmaps.Checked = Settings.Default.GenerateMipMaps;
            boxScaleFactor.Text = Settings.Default.ScaleFactorHack.ToString();
        }

        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
            gui.ResetGUI();
            gui.DrawPackages();
        }


        private void btnDataProps_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CopyMode = "dataprops";
        }

        private void btnData_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CopyMode = "data";
        }

        private void btnProperties_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CopyMode = "props";
        }

        private void btnLogInfo_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.LogLevel = "info";
        }

        private void btnLogDebug_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.LogLevel = "debug";
        }

        private void btnLogTrace_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.LogLevel = "trace";
        }


        private void btnViewNormal_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ViewMode = "normal";
        }

        private void btnViewClass_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ViewMode = "class";
        }

        private void btnViewPack_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ViewMode = "package";
        }

        private void boxDebug_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Debug = boxDebug.Checked;
        }

        private void boxImports_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowImports = boxImports.Checked;
        }

        private void boxPatchmode_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PatchMode = boxPatchmode.Checked;
        }

        private void boxUseUID_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.UseUID = boxUseUID.Checked;
        }

        private void boxJitData_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.JitData = boxJitData.Checked;
        }

        private void boxGenerateMipmaps_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.GenerateMipMaps = boxJitData.Checked;
        }

        private void boxScaleFactor_TextChanged(object sender, EventArgs e)
        {
            float result = 1;
            if (float.TryParse(boxScaleFactor.Text, out result))
            {
                Settings.Default.ScaleFactorHack = result;
            }
        }

      
    }
}
