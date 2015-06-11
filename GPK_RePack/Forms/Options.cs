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
            }

            boxDebug.Checked = Settings.Default.Debug;
            boxImports.Checked = Settings.Default.ShowImports;
        }

        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
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
        
        private void boxDebug_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Debug = boxDebug.Checked;
        }

        private void boxImports_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowImports = boxImports.Checked;
        }

    }
}
