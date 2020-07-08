using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            boxEnableTexture2D.Checked = Settings.Default.EnableTexture2D;
            boxColorPreview.BackColor = Settings.Default.PreviewColor;
            boxSavefilePostfix.Text = Settings.Default.SaveFileSuffix;
        }



        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();

            new Task(() =>
            {
                gui.ResetGUI();
                gui.DrawPackages();
            }).Start();
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

        private void btnOpenSettingsFolder_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            var folder = Path.GetDirectoryName(config.FilePath);
            Process.Start("explorer.exe", folder);

        }

        private void boxEnableTexture2D_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.EnableTexture2D = boxEnableTexture2D.Checked;
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = Settings.Default.PreviewColor;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.PreviewColor = dialog.Color;
                boxColorPreview.BackColor = dialog.Color;
            }


        }

        private void boxSavefilePostfix_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.SaveFileSuffix = boxSavefilePostfix.Text;
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.Default.SaveFileSuffix == "")
            {
                var result = MessageBox.Show("WARNING: Suffix is empty! Orginal GPKs will be overwritten if you save now.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
