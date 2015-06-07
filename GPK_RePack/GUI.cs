using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using GPK_RePack.Class;
using GPK_RePack.Class.Prop;
using GPK_RePack.Parser;
using GPK_RePack.Saver;
using NLog;
using NLog.Fluent;

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

        private GpkPackage selectedPackage;
        private GpkExport selectedExport;

        private List<GpkPackage> loadedGpkPackages;
        private List<GpkExport>[] changedExports;

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
            Array.Resize(ref changedExports, loadedGpkPackages.Count);
            for (int i = 0; i < changedExports.Length; i++)
            {
                changedExports[i] = new List<GpkExport>();
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

            for (int i = 0; i < loadedGpkPackages.Count; i++)
            {
                GpkPackage package = loadedGpkPackages[i];
                TreeNode nodeP = treeMain.Nodes.Add(i.ToString(), package.Filename);

                TreeNode nodeI = nodeP.Nodes.Add("Imports");
                TreeNode nodeE = nodeP.Nodes.Add("Exports");


                //Imports 

                foreach (var tmp in package.ImportList.OrderByDescending(pair => pair.Value.Object).Reverse())
                {
                    nodeI.Nodes.Add(tmp.Key.ToString(), tmp.Value.Object);
                }


                //Exports 
                foreach (var tmp in package.ExportList.OrderByDescending(pair => pair.Value.Name).Reverse())
                {
                    nodeE.Nodes.Add(tmp.Key.ToString(), tmp.Value.Name);
                }
            }
        }


        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            boxInfo.Text = "";
            boxButtons.Enabled = false;
            selectedExport = null;
            selectedPackage = null;

            if (e.Node.Level == 2)
            {
                GpkPackage package = loadedGpkPackages[Convert.ToInt32(e.Node.Parent.Parent.Name)];
                if (e.Node.Parent.Text == "Imports")
                {
                    GpkImport imp = package.ImportList[Convert.ToInt32(e.Node.Name)];

                    StringBuilder info = new StringBuilder();
                    info.AppendLine("ClassPackage: " + imp.ClassPackage);
                    info.AppendLine("Class: " + imp.Class);
                    info.AppendLine("Object: " + imp.Object);

                    boxInfo.Text = info.ToString();
                    //ClassPackage Core Class: Class Object: ObjectRedirector

                }
                else if (e.Node.Parent.Text == "Exports")
                {
                    GpkExport exp = package.ExportList[Convert.ToInt32(e.Node.Name)];

                    StringBuilder info = new StringBuilder();
                    info.AppendLine("Name: " + exp.Name);
                    info.AppendLine("Class: " + exp.ClassName);
                    info.AppendLine("Data_Offset: " + exp.SerialOffset);
                    if (exp.data != null)
                    {
                        info.AppendLine("Data_Size: " + exp.data.Length);
                    }
                    else
                    {
                        info.AppendLine("Data_Size: 0");
                    }

                    info.AppendLine("Props: ");
                    foreach (GpkBaseProperty prop in exp.Properties)
                    {
                        info.Append("Name: " + prop.Name);
                        if (prop.value != null)
                        {
                            info.AppendLine("Value: " + prop.value.ToString());
                        }
                        else
                        {
                            info.AppendLine();
                        }
                    }

                    boxInfo.Text = info.ToString();
                    boxButtons.Enabled = true;
                    selectedExport = exp;
                    selectedPackage = package;
                    // Class: DistributionFloatUniform, Name: DistributionFloatUniform, Data_Size: 100, Data_Offset 80654, Export_offset 9514
                }
            }
        }


        #endregion

        #region editgpk

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (selectedExport != null)
            {
                if (selectedExport.data == null)
                {
                    logger.Info("Length is zero. Nothing to export");
                }

                SaveFileDialog save = new SaveFileDialog();
                save.FileName = selectedExport.Name;
                save.DefaultExt = ".raw";
                save.ShowDialog();

                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.BaseStream.Write(selectedExport.data, 0, selectedExport.data.Length);
                writer.Close();
                writer.Dispose();

                logger.Info(String.Format("Data was saved to {0}!", save.FileName));
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                return;
            }
            if (selectedExport.data == null)
            {
                return;
            }

            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.ValidateNames = true;
            open.InitialDirectory = Directory.GetCurrentDirectory();

            open.ShowDialog();

            string path = open.FileName;

            if (File.Exists(path))
            {
                byte[] buffer = File.ReadAllBytes(path);
                int packageIndex = Convert.ToInt32(treeMain.SelectedNode.Parent.Parent.Name);


                if (buffer.Length > selectedExport.data.Length)
                {
                    //Too long, not possible without rebuiling the gpk
                    logger.Info("File size too big. Size: " + buffer.Length + " Maximum Size: " +
                             selectedExport.data.Length);
                }

                if (buffer.Length < selectedExport.data.Length)
                {
                    //Too short, fill it
                    selectedExport.data = new byte[selectedExport.data.Length];
                }

                //selectedExport.data = buffer;
                Array.Copy(buffer, selectedExport.data, buffer.Length);

                changedExports[packageIndex].Add(selectedExport);
                logger.Info(String.Format("Replaced the data of {0} successfully! Dont forget to save.",
                    selectedExport.Name));
            }
        }

        private void replaceSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < changedExports.Length; i++)
            {
                List<GpkExport> list = changedExports[i];
                if (list.Count > 0)
                {
                    GpkPackage package = loadedGpkPackages[i];
                    string savepath = package.Path + "_patched";
                    Save.SaveReplacedExport(package, savepath, list);
                    logger.Info(String.Format("Saved the changed data of package '{0} to {1}'!",
                    package.Filename, savepath));
                }
            }
        }

        #endregion

    }
}


