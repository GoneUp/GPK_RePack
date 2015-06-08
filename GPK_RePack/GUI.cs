using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GPK_RePack.Class;
using GPK_RePack.Class.Prop;
using GPK_RePack.Parser;
using GPK_RePack.Saver;
using NLog;
using NLog.Config;
using NLog.Fluent;

namespace GPK_RePack
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        #region nlogconf
        string xml = @"<?xml version='1.0' encoding='utf-8' ?>
<nlog xmlns='http://www.nlog-project.org/schemas/NLog.xsd'
      xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <targets>
    <target name='logfile' xsi:type='File' fileName='Terahelper.log' deleteOldFileOnStartup='true'/>
    <target xsi:type='FormControl'
        name='form'
        layout='${longdate} ${level:uppercase=true} ${message} ${newline}'
        append='true'
        controlName='boxLog'
        formName='GUI' />
  </targets>

  <rules>
    <logger name='*' minlevel='Trace' writeTo='logfile' />
    <logger name='*' minlevel='Info' writeTo='form' />
  </rules>
</nlog>";
        #endregion

        #region def

        private Logger logger = LogManager.GetCurrentClassLogger();
        private Reader reader;
        private Save saver;

        private GpkPackage selectedPackage;
        private GpkExport selectedExport;

        private List<GpkPackage> loadedGpkPackages;
        private List<GpkExport>[] changedExports;

        #endregion

        #region Main

        private void GUI_Load(object sender, EventArgs e)
        {
            //nlog init
            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);
            XmlLoggingConfiguration config = new XmlLoggingConfiguration(xr, null);
            LogManager.Configuration = config;

            //Our stuff
            reader = new Reader();
            saver = new Save();
            loadedGpkPackages = new List<GpkPackage>();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.ValidateNames = true;
            //open.InitialDirectory = Directory.GetCurrentDirectory();

            open.ShowDialog();

            foreach (var path in open.FileNames)
            {
                if (File.Exists(path))
                {
                    Thread newThread = new Thread(delegate()
                      {
                          GpkPackage tmpPack = reader.ReadGpk(path);
                          tmpPack.Changes = true; //tmp, remove after tests
                          loadedGpkPackages.Add(tmpPack);
                      });
                    newThread.Start();

                    while (newThread.IsAlive)
                    {
                        Application.DoEvents();
                        boxInfo.Text = String.Format("Progress of loading: {0}/{1}", reader.progress, reader.totalobjects);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void boxLog_TextChanged(object sender, EventArgs e)
        {
            boxLog.SelectionStart = boxLog.TextLength;
            boxLog.ScrollToCaret();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changedExports = null;
            selectedExport = null;
            selectedPackage = null;
            boxInfo.Text = "";
            boxDataButtons.Enabled = false;
            boxGeneralButtons.Enabled = false;
            loadedGpkPackages.Clear();
            DrawPackages();
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

                TreeNode nodeE = nodeP.Nodes.Add("Exports");
                TreeNode nodeI = nodeP.Nodes.Add("Imports");

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
            boxDataButtons.Enabled = false;
            boxGeneralButtons.Enabled = false;
            selectedExport = null;
            selectedPackage = null;

            if (e.Node.Level == 0)
            {
                boxGeneralButtons.Enabled = true;

                selectedPackage = loadedGpkPackages[Convert.ToInt32(e.Node.Name)];
            }
            else if (e.Node.Level == 2)
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
                    foreach (object prop in exp.Properties)
                    {
                        info.AppendLine(prop.ToString());
                        /*info.Append("Name: " + prop.Name);
                        if (prop.value != null)
                        {
                            info.AppendLine("Value: " + prop.value.ToString());
                        }
                        else
                        {
                            info.AppendLine();
                        }*/
                    }

                    boxInfo.Text = info.ToString();
                    boxDataButtons.Enabled = true;
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

                if (btnPatchMode.Checked)
                {
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

                }
                else
                {
                    //Rebuild Mode
                    //We force the rebuilder to recalculate the size. (atm we dont know how big the propertys are)
                    logger.Trace(String.Format("rebuild mode old size {0} new size {1}", selectedExport.data.Length,
                        buffer.Length));

                    selectedExport.SerialSize = -1;
                    selectedExport.data = buffer;
                    selectedPackage.Changes = true;
                }

                changedExports[packageIndex].Add(selectedExport);
                logger.Info(String.Format("Replaced the data of {0} successfully! Dont forget to save.",
                    selectedExport.Name));
            }



        }

        private void replaceSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changedExports != null)
            {
                for (int i = 0; i < changedExports.Length; i++)
                {
                    List<GpkExport> list = changedExports[i];
                    if (list.Count > 0)
                    {
                        try
                        {
                            GpkPackage package = loadedGpkPackages[i];
                            string savepath = package.Path + "_patched";
                            saver.SaveReplacedExport(package, savepath, list);
                            logger.Info(String.Format("Saved the changed data of package '{0} to {1}'!",
                                package.Filename, savepath));
                        }
                        catch (Exception ex)
                        {
                            logger.FatalException("Save failure! " + ex, ex);
                        }
                    }
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GpkPackage package in loadedGpkPackages)
            {
                if (package.Changes)
                {
                    try
                    {
                        logger.Info(String.Format("Attemping to save {0}...", package.Filename));
                        string savepath = package.Path + "_rebuild";
                        saver.SaveGpkPackage(package, savepath);
                        logger.Info(String.Format("Saved the package '{0} to {1}'!", package.Filename, savepath));
                    }
                    catch (Exception ex)
                    {
                        logger.FatalException("Save failure! " + ex, ex);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPackage != null)
            {
                loadedGpkPackages.Remove(selectedPackage);
                DrawPackages();

                selectedPackage = null;
                boxGeneralButtons.Enabled = false;
            }

        }
        #endregion






    }
}


