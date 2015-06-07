using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Class;
using GPK_RePack.Parser;
using NLog;

namespace GPK_RePack
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        #region def
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Reader reader;

        private List<GpkPackage> loadedGpkPackages;
        #endregion

        #region Main

        private void GUI_Load(object sender, EventArgs e)
        {
            reader = new Reader();
            loadedGpkPackages = new List<GpkPackage>();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.ValidateNames = true;
            open.InitialDirectory = Directory.GetCurrentDirectory();

            open.ShowDialog();

            foreach (var path in open.FileNames)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        GpkPackage tmpPack = reader.ReadGpk(path);
                        loadedGpkPackages.Add(tmpPack);
                    }
                    catch (Exception ex)
                    {
                        logger.FatalException("Parse failure! " + ex, ex);
                    }
                }
            }

            DrawPackages();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void boxLog_TextChanged(object sender, EventArgs e)
        {
            boxLog.SelectionStart = boxLog.TextLength;
            boxLog.ScrollToCaret();
        }
        #endregion

        #region diplaygpk

        private void DrawPackages()
        {
            treeMain.Nodes.Clear();

            foreach (GpkPackage package in loadedGpkPackages)
            {
                TreeNode nodeP = treeMain.Nodes.Add(package.Filename);

                TreeNode nodeI = nodeP.Nodes.Add("Imports");
                TreeNode nodeE = nodeP.Nodes.Add("Exports");


                //Imports 
                foreach (GpkImport imp in package.ImportList.Values)
                {
                    nodeI.Nodes.Add(imp.Object);
                }

                //Exports 
                foreach (GpkExport exp in package.ExportList.Values)
                {
                    nodeE.Nodes.Add(exp.Name);
                }
            }
        }
        #endregion

        #region editgpk
        #endregion

    }
}
