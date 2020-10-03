using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Core;

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
            switch (CoreSettings.Default.CopyMode)
            {
                case CopyMode.DataProps:
                    btnDataProps.Checked = true;
                    break;
                case CopyMode.Data:
                    btnData.Checked = true;
                    break;
                case CopyMode.Props:
                    btnProperties.Checked = true;
                    break;
                case CopyMode.All:
                    btnRadioAll.Checked = true;
                    break;
            }

            switch (CoreSettings.Default.LogLevel)
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

            switch (CoreSettings.Default.ViewMode)
            {
                case ViewMode.Normal:
                    btnViewNormal.Checked = true;
                    break;
                case ViewMode.Class:
                    btnViewClass.Checked = true;
                    break;
                case ViewMode.Package:
                    btnViewPack.Checked = true;
                    break;
            }

            boxDebug.Checked = CoreSettings.Default.Debug;
            boxImports.Checked = CoreSettings.Default.ShowImports;
            boxPatchmode.Checked = CoreSettings.Default.PatchMode;
            boxUseUID.Checked = CoreSettings.Default.UseUID;
            boxJitData.Checked = CoreSettings.Default.JitData;
            boxGenerateMipmaps.Checked = CoreSettings.Default.GenerateMipMaps;
            boxScaleFactor.Text = CoreSettings.Default.ScaleFactorHack.ToString();
            boxEnableTexture2D.Checked = CoreSettings.Default.EnableTexture2D;
            boxColorPreview.BackColor = CoreSettings.Default.PreviewColor;
            boxSavefilePostfix.Text = CoreSettings.Default.SaveFileSuffix;
            boxCompression.Checked = CoreSettings.Default.EnableCompression;
            boxLoadMapping.Checked = CoreSettings.Default.LoadMappingOnStart;
        }



        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            CoreSettings.Save();

            new Task(() =>
            {
                NLogConfig.ReloadFileLoggingRule();
                gui.ResetGUI();
                gui.DrawPackages();
            }).Start();
        }


        private void btnDataProps_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.CopyMode = CopyMode.DataProps;
        }

        private void btnData_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.CopyMode = CopyMode.Data;
        }

        private void btnProperties_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.CopyMode = CopyMode.Props;
        }

        private void btnRadioAll_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.CopyMode = CopyMode.All;
        }

        private void btnLogInfo_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.LogLevel = "info";
        }

        private void btnLogDebug_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.LogLevel = "debug";
        }

        private void btnLogTrace_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.LogLevel = "trace";
        }


        private void btnViewNormal_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.ViewMode = ViewMode.Normal;
        }

        private void btnViewClass_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.ViewMode = ViewMode.Class;
        }

        private void btnViewPack_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.ViewMode = ViewMode.Package;
        }

        private void boxDebug_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.Debug = boxDebug.Checked;
        }

        private void boxImports_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.ShowImports = boxImports.Checked;
        }

        private void boxPatchmode_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.PatchMode = boxPatchmode.Checked;
        }

        private void boxUseUID_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.UseUID = boxUseUID.Checked;
        }

        private void boxJitData_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.JitData = boxJitData.Checked;
        }

        private void boxGenerateMipmaps_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.GenerateMipMaps = boxJitData.Checked;
        }

        private void boxScaleFactor_TextChanged(object sender, EventArgs e)
        {
            float result = 1;
            if (float.TryParse(boxScaleFactor.Text, out result))
            {
                CoreSettings.Default.ScaleFactorHack = result;
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
            CoreSettings.Default.EnableTexture2D = boxEnableTexture2D.Checked;
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = CoreSettings.Default.PreviewColor;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CoreSettings.Default.PreviewColor = dialog.Color;
                boxColorPreview.BackColor = dialog.Color;
            }


        }

        private void boxSavefilePostfix_TextChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.SaveFileSuffix = boxSavefilePostfix.Text;
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CoreSettings.Default.SaveFileSuffix == "")
            {
                var result = MessageBox.Show("WARNING: Suffix is empty! Orginal GPKs will be overwritten if you save now.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void boxCompression_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.EnableCompression = boxCompression.Checked;
        }

        private void boxLoadMapping_CheckedChanged(object sender, EventArgs e)
        {
            CoreSettings.Default.LoadMappingOnStart = boxLoadMapping.Checked;
        }
    }
}
