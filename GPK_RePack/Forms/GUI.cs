using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using GPK_RePack.Classes;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Payload;
using GPK_RePack.Classes.Prop;
using GPK_RePack.Editors;
using GPK_RePack.Parser;
using GPK_RePack.Properties;
using GPK_RePack.Saver;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NLog;
using NLog.Config;

namespace GPK_RePack.Forms
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
    <logger name='*' minlevel='%loglevel%' writeTo='logfile' />
    <logger name='*' minlevel='Info' writeTo='form' />
  </rules>
</nlog>";
        #endregion

        #region def

        private Logger logger;
        private Reader reader;
        private Save saver;

        private GpkPackage selectedPackage;
        private GpkExport selectedExport;
        private string selectedClass = "";

        private List<GpkPackage> loadedGpkPackages;
        private List<GpkExport>[] changedExports;

        private readonly DataFormats.Format exportFormat = DataFormats.GetFormat(typeof(GpkExport).FullName);

        private VorbisWaveReader waveReader;
        private WaveOut waveOut;

        #endregion

        #region Main

        private void GUI_Load(object sender, EventArgs e)
        {
            //nlog init
            String config_path = (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            if (!File.Exists(config_path))
            {
                File.WriteAllText(config_path, Resources.Config);
                MessageBox.Show("Setting file was missing. Please restart the application.");
                Environment.Exit(0);
            }


            xml = xml.Replace("%loglevel%", Settings.Default.LogLevel);
            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);
            XmlLoggingConfiguration config = new XmlLoggingConfiguration(xr, null);
            LogManager.Configuration = config;
            logger = LogManager.GetCurrentClassLogger();

            //Our stuff
            logger.Info("Startup");
            reader = new Reader();
            saver = new Save();
            loadedGpkPackages = new List<GpkPackage>();

            //audio
            waveOut = new WaveOut();
            waveOut.PlaybackStopped += WaveOutOnPlaybackStopped;

            if (Settings.Default.SaveDir == "")
                Settings.Default.SaveDir = Directory.GetCurrentDirectory();

            if (Settings.Default.OpenDir == "")
                Settings.Default.OpenDir = Directory.GetCurrentDirectory();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options opt = new Options(this);
            opt.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUI_FormClosing(null, new FormClosingEventArgs(CloseReason.UserClosing, false));
            Environment.Exit(0);
        }

        private void boxLog_TextChanged(object sender, EventArgs e)
        {
            boxLog.SelectionStart = boxLog.TextLength;
            boxLog.ScrollToCaret();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetGUI();
            changedExports = null;
            ResetOggPreview();
            loadedGpkPackages.Clear();
            DrawPackages();
        }

        public void ResetGUI()
        {
            selectedExport = null;
            selectedPackage = null;
            selectedClass = "";
            boxInfo.Text = "";
            boxGeneralButtons.Enabled = false;
            boxDataButtons.Enabled = false;
            boxPropertyButtons.Enabled = false;
            ClearGrid();
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
            if (waveReader != null)
            {
                waveReader.Dispose();
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
            }

        }


        #endregion

        #region load/save
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.ValidateNames = true;
            open.InitialDirectory = Settings.Default.OpenDir;
            open.ShowDialog();

            Settings.Default.OpenDir = open.FileName;
            foreach (var path in open.FileNames)
            {
                if (File.Exists(path))
                {
                    Thread newThread = new Thread(delegate()
                      {
                          GpkPackage tmpPack = reader.ReadGpk(path);
                          if (tmpPack != null)
                          {
                              if (Settings.Default.Debug)
                              {
                                  tmpPack.Changes = true; //tmp, remove after tests
                              }

                              loadedGpkPackages.Add(tmpPack);
                          }
                      });
                    newThread.Start();

                    while (newThread.IsAlive)
                    {
                        Application.DoEvents();
                        boxInfo.Text = String.Format("Progress of loading: {0}/{1}", reader.progress, reader.totalobjects);
                        Thread.Sleep(50);
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


        private void replaceSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool save = false;
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
                            save = true;
                        }
                        catch (Exception ex)
                        {
                            logger.FatalException("Save failure! " + ex, ex);
                        }
                    }
                }


            }

            if (!save)
            {
                logger.Info("Nothing to save in PatchMode!");
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GpkPackage package in loadedGpkPackages)
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

            logger.Info("Saving done!");
        }
        #endregion

        #region diplaygpk
        public void DrawPackages()
        {
            treeMain.Nodes.Clear();

            for (int i = 0; i < loadedGpkPackages.Count; i++)
            {
                GpkPackage package = loadedGpkPackages[i];
                TreeNode nodeP = treeMain.Nodes.Add(i.ToString(), package.Filename);

                switch (Settings.Default.ViewMode)
                {
                    case "normal":
                        if (Settings.Default.ShowImports)
                        {
                            TreeNode nodeI = nodeP.Nodes.Add("Imports");

                            foreach (var tmp in package.ImportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                            {
                                string key = tmp.Value.UID;
                                string value = tmp.Value.ObjectName;
                                if (Settings.Default.UseUID) value = key;

                                nodeI.Nodes.Add(key, value);
                            }
                        }

                        //Exports
                        TreeNode nodeE = nodeP.Nodes.Add("Exports");
                        foreach (var tmp in package.ExportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                        {
                            string key = tmp.Value.UID;
                            string value = tmp.Value.ObjectName;
                            if (Settings.Default.UseUID) value = key;

                            nodeE.Nodes.Add(key, value);
                        }
                        break;
                    case "class":
                        Dictionary<string, TreeNode> classNodes = new Dictionary<string, TreeNode>();
                        if (Settings.Default.ShowImports)
                        {
                            foreach (var tmp in package.ImportList)
                            {
                                string key = tmp.Value.UID;
                                string value = tmp.Value.ObjectName;
                                if (Settings.Default.UseUID) value = key;

                                CheckClassNode(tmp.Value.ClassName, classNodes, nodeP);
                                classNodes[tmp.Value.ClassName].Nodes.Add(key, value);

                            }
                        }

                        foreach (var tmp in package.ExportList)
                        {
                            string key = tmp.Value.UID;
                            string value = tmp.Value.ObjectName;
                            if (Settings.Default.UseUID) value = key;

                            CheckClassNode(tmp.Value.ClassName, classNodes, nodeP);
                            classNodes[tmp.Value.ClassName].Nodes.Add(key, value);
                        }
                        break;
                }
            }
        }

        private void CheckClassNode(string className, Dictionary<string, TreeNode> classNodes, TreeNode mainNode)
        {
            if (!classNodes.ContainsKey(className))
            {
                TreeNode classNode = mainNode.Nodes.Add(className);
                classNodes.Add(className, classNode);
            }
        }


        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetGUI();

            if (e.Node.Level == 0)
            {
                boxGeneralButtons.Enabled = true;
                boxDataButtons.Enabled = true;

                selectedPackage = loadedGpkPackages[Convert.ToInt32(e.Node.Name)];
                boxInfo.Text = selectedPackage.ToString();
            }
            else if (e.Node.Level == 1 && Settings.Default.ViewMode == "class")
            {
                selectedPackage = loadedGpkPackages[Convert.ToInt32(e.Node.Parent.Name)];
                selectedClass = e.Node.Text;

                boxDataButtons.Enabled = true;
            }
            else if (e.Node.Level == 2)
            {
                GpkPackage package = loadedGpkPackages[Convert.ToInt32(e.Node.Parent.Parent.Name)];
                Object selected = package.GetObjectByUID(e.Node.Name);

                if (selected is GpkImport)
                {
                    GpkImport imp = (GpkImport)selected;
                    boxInfo.Text = imp.ToString();

                }
                else if (selected is GpkExport)
                {
                    GpkExport exp = (GpkExport)selected;
                    boxInfo.Text = exp.ToString();


                    boxGeneralButtons.Enabled = true;
                    boxDataButtons.Enabled = true;
                    boxPropertyButtons.Enabled = true;
                    selectedExport = exp;
                    selectedPackage = package;

                    DrawGrid(package, exp);
                }
            }
        }


        private void refreshViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawPackages();
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
                    return;
                }

                SaveFileDialog save = new SaveFileDialog();
                save.FileName = selectedExport.ObjectName;
                save.DefaultExt = ".raw";
                save.InitialDirectory = Settings.Default.SaveDir;
                save.ShowDialog();
                Settings.Default.SaveDir = save.FileName;

                DataTools.WriteExportDataFile(save.FileName, selectedExport);
            }
            else if (selectedPackage != null && selectedClass != "")
            {
                List<GpkExport> exports = selectedPackage.GetExportsByClass(selectedClass);

                if (exports.Count == 0)
                {
                    logger.Info("No exports found for class {0}.", selectedClass);
                    return;
                }


                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = Settings.Default.SaveDir;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Settings.Default.SaveDir = dialog.SelectedPath;

                    foreach (GpkExport exp in exports)
                    {
                        if (exp.data != null)
                        {
                            DataTools.WriteExportDataFile(String.Format("{0}\\{1}.raw", dialog.SelectedPath, exp.ObjectName), exp);
                            logger.Trace("save for " + exp.UID);
                        }
                    }
                }
            }
            else if (selectedPackage != null)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = Settings.Default.SaveDir;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Settings.Default.SaveDir = dialog.SelectedPath;

                    foreach (GpkExport exp in selectedPackage.ExportList.Values)
                    {
                        if (exp.data != null)
                        {
                            DataTools.WriteExportDataFile(String.Format("{0}\\{1}\\{2}.raw", dialog.SelectedPath, exp.ClassName, exp.ObjectName), exp);
                            logger.Trace("save for " + exp.UID);
                        }
                    }
                }
            }

            logger.Info("Data was saved!");
        }


        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }
            if (selectedExport.data == null)
            {
                logger.Trace("no export data");
                return;
            }

            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.ValidateNames = true;
            open.InitialDirectory = Settings.Default.OpenDir;

            open.ShowDialog();

            string path = open.FileName;
            Settings.Default.OpenDir = path;

            if (File.Exists(path))
            {
                byte[] buffer = File.ReadAllBytes(path);


                if (Settings.Default.PatchMode)
                {
                    if (treeMain.SelectedNode.Parent.Parent == null) return;

                    int packageIndex = Convert.ToInt32(treeMain.SelectedNode.Parent.Parent.Name);

                    if (buffer.Length > selectedExport.data.Length)
                    {
                        //Too long, not possible without rebuiling the gpk
                        logger.Info("File size too big for PatchMode. Size: " + buffer.Length + " Maximum Size: " +
                                 selectedExport.data.Length);
                        return;
                    }

                    if (buffer.Length < selectedExport.data.Length)
                    {
                        //Too short, fill it
                        selectedExport.data = new byte[selectedExport.data.Length];
                    }

                    //selectedExport.data = buffer;
                    Array.Copy(buffer, selectedExport.data, buffer.Length);

                    changedExports[packageIndex].Add(selectedExport);

                }
                else
                {
                    //Rebuild Mode
                    //We force the rebuilder to recalculate the size. (atm we dont know how big the propertys are)
                    logger.Trace(String.Format("rebuild mode old size {0} new size {1}", selectedExport.data.Length,
                        buffer.Length));

                    selectedExport.data = buffer;
                    selectedExport.GetDataSize();
                    selectedPackage.Changes = true;
                }


                logger.Info(String.Format("Replaced the data of {0} successfully! Dont forget to save.",
                    selectedExport.ObjectName));
            }



        }


        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPackage != null && selectedExport == null)
            {
                loadedGpkPackages.Remove(selectedPackage);
                DrawPackages();

                logger.Info("Removed package {0}...", selectedPackage.Filename);

                selectedPackage = null;
                boxGeneralButtons.Enabled = false;
            }
            else if (selectedPackage != null && selectedExport != null)
            {
                selectedPackage.ExportList.Remove(selectedPackage.GetObjectKeyByUID(selectedExport.UID));

                logger.Info("Removed object {0}...", selectedExport.UID);

                selectedExport = null;

                treeMain.Nodes.Remove(treeMain.SelectedNode);
            }

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            Clipboard.SetData(exportFormat.Name, selectedExport);
            logger.Info("Made a copy of {0}...", selectedExport.UID);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            var dataObject = Clipboard.GetDataObject();
            GpkExport copyExport = (GpkExport)dataObject.GetData(exportFormat.Name);

            if (copyExport == null)
            {
                logger.Info("copy paste fail");
                return;
            }

            logger.Trace(Settings.Default.CopyMode);
            string option = "";

            switch (Settings.Default.CopyMode)
            {
                case "dataprops":
                    DataTools.ReplaceProperties(copyExport, selectedExport);
                    DataTools.ReplaceData(copyExport, selectedExport);
                    option = "data and properties";
                    break;
                case "data":
                    DataTools.ReplaceData(copyExport, selectedExport);
                    option = "data";
                    break;
                case "props":
                    DataTools.ReplaceProperties(copyExport, selectedExport);
                    option = "properties";
                    break;
                default:
                    logger.Info("Your setting file is broken. Go to settings windows and select a copymode.");
                    break;

            }

            copyExport.GetDataSize();
            treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
            logger.Info("Pasted the {0} of {1} to {2}", option, copyExport.UID, selectedExport.UID);
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            if (selectedExport.data == null)
            {
                logger.Trace("no export data");
                return;
            }

            selectedExport.data = null;
            selectedExport.data_padding = null;
            selectedExport.payload = null;
            selectedExport.GetDataSize();

            treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
        }
        private void GUI_KeyDown(object sender, KeyEventArgs e)
        {
            //Avoid annyoing ding sound - problem that is supresses also keystrokes
            //e.Handled = true;
            //e.SuppressKeyPress = true;

            if (e.Control && e.KeyCode == Keys.C)
            {
                btnCopy_Click(btnCopy, new EventArgs());
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                btnPaste_Click(btnPaste, new EventArgs());
            }
        }

        #endregion

        #region ogg

        private void btnImportOgg_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedExport != null)
                {

                    OpenFileDialog open = new OpenFileDialog();
                    open.Multiselect = false;
                    open.ValidateNames = true;
                    open.InitialDirectory = Settings.Default.OpenDir;

                    open.ShowDialog();
                    Settings.Default.OpenDir = open.FileName;
                    if (File.Exists(open.FileName))
                    {
                        SoundwaveTools.ImportOgg(selectedExport, open.FileName);
                        treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
                        logger.Info("Import successful.");
                    }
                    else
                    {
                        logger.Info("File not found.");
                    }

                }
                else if (selectedPackage != null && selectedClass == "Core.SoundNodeWave")
                {
                    List<GpkExport> exports = selectedPackage.GetExportsByClass(selectedClass);

                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.SelectedPath = Path.GetDirectoryName(Settings.Default.SaveDir);
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        Settings.Default.SaveDir = dialog.SelectedPath;

                        string[] files = Directory.GetFiles(dialog.SelectedPath);

                        foreach (string file in files)
                        {
                            string filename = Path.GetFileName(file); //AttackL_02.ogg
                            string oggname = filename.Remove(filename.Length - 4);

                            if (oggname == "") continue;

                            foreach (GpkExport exp in exports)
                            {
                                if (exp.ObjectName == oggname)
                                {
                                    SoundwaveTools.ImportOgg(exp, file);
                                    logger.Trace("Matched file {0} to export {1}!", filename, exp.ObjectName);
                                    break;
                                }
                            }


                        }


                        logger.Info("Mass import to {0} was successful.", dialog.SelectedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.FatalException("Import failure! " + ex, ex);
            }
        }

        private void btnExtractOGG_Click(object sender, EventArgs e)
        {

            if (selectedExport != null && selectedExport.ClassName == "Core.SoundNodeWave")
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = selectedExport.ObjectName;
                save.InitialDirectory = Settings.Default.SaveDir;
                save.DefaultExt = ".ogg";
                save.ShowDialog();
                Settings.Default.SaveDir = save.FileName;

                SoundwaveTools.ExportOgg(selectedExport, save.FileName);
            }
            else if (selectedPackage != null && selectedClass == "Core.SoundNodeWave")
            {
                List<GpkExport> exports = selectedPackage.GetExportsByClass(selectedClass);

                if (exports.Count == 0)
                {
                    logger.Info("No oggs found for class {0}.", selectedClass);
                    return;
                }


                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = Path.GetDirectoryName(Settings.Default.SaveDir);
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Settings.Default.SaveDir = dialog.SelectedPath;

                    foreach (GpkExport exp in exports)
                    {
                        SoundwaveTools.ExportOgg(exp, String.Format("{0}\\{1}.ogg", dialog.SelectedPath, exp.ObjectName));
                        logger.Trace("ogg save for " + exp.UID);
                    }

                    logger.Info("Mass export to {0} was successful.", dialog.SelectedPath);
                }
            }
        }

        private void btnFakeOGG_Click(object sender, EventArgs e)
        {
            if (selectedExport != null)
            {
                SoundwaveTools.ImportOgg(selectedExport, "fake");
                treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
            }
        }

        private void btnOggPreview_Click(object sender, EventArgs e)
        {
            try
            {

                if (selectedExport != null && selectedExport.payload is Soundwave && waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    Soundwave wave = (Soundwave)selectedExport.payload;
                    waveReader = new VorbisWaveReader(new MemoryStream(wave.oggdata));
                    waveOut.Init(waveReader);
                    waveOut.Play();
                    btnOggPreview.Text = "Stop Preview";
                }
                else if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                {
                    ResetOggPreview();
                }
            }
            catch (Exception ex)
            {
                logger.Info("Playback Error. " + ex);
            }
        }

        private void WaveOutOnPlaybackStopped(object sender, EventArgs eventArgs)
        {
            ResetOggPreview();
        }

        private void ResetOggPreview()
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Stop();
                waveReader.Close();
                waveReader.Dispose();
            }

            waveReader = null;
            btnOggPreview.Text = "Ogg Preview";
        }


        #endregion

        #region misc
        private void setFilesizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageSelected()) return;

            string input = Microsoft.VisualBasic.Interaction.InputBox(string.Format("New filesize for {0}? Old: {1}", selectedPackage.Filename, selectedPackage.OrginalSize), "Filesize");

            int num;
            if (input == "" || !Int32.TryParse(input, out num))
            {
                logger.Info("No/Invalid input");
            }
            else
            {
                logger.Trace(num);
                selectedPackage.OrginalSize = num;
                logger.Info("Set filesize for {0} to {1}", selectedPackage.Filename, selectedPackage.OrginalSize);
            }

        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageSelected()) return;

            try
            {
                string className = Microsoft.VisualBasic.Interaction.InputBox("Classname UID?\nWrite #all to select every object.\nSupported types: Int, Float (x,xx), Bool, String");
                string propName = Microsoft.VisualBasic.Interaction.InputBox("Proprty Name to edit?");
                string propValue = Microsoft.VisualBasic.Interaction.InputBox("Proprty Value:");

                List<GpkExport> exports = selectedPackage.GetExportsByClass(className);

                SoundwaveTools.SetPropertyDetails(exports, propName, propValue);

                logger.Info("Custom set success for {0} Objects.", exports.Count);
            }
            catch (Exception ex)
            {
                logger.Fatal("Custom update fail. Ex " + ex);
            }


        }

        private void setAllVolumeMultipliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageSelected()) return;

            string input = Microsoft.VisualBasic.Interaction.InputBox(String.Format("New VolumeMultiplier for all SoundCues in {0}: \nFormat: x,xx", selectedPackage.Filename));

            float num;
            if (input == "" || !Single.TryParse(input, out num))
            {
                logger.Info("No/Invalid input");
            }
            else
            {
                logger.Trace(num);
                SoundwaveTools.SetAllVolumes(selectedPackage, num);
                logger.Info("Set Volumes for {0} to {1}.", selectedPackage.Filename, num);
            }
        }

        private bool PackageSelected()
        {
            if (selectedPackage == null)
            {
                logger.Info("Select a package!");
                return false;
            }

            return true;
        }

        #endregion

        #region propgrid

        private void ClearGrid()
        {
            gridProps.Rows.Clear();
        }

        private void DrawGrid(GpkPackage package, GpkExport export)
        {
            gridProps.Enabled = true;
            gridProps.Rows.Clear();

            IEnumerable<String> nameQuery = from pair in package.NameList.Values.ToList() select pair.name;
            //IEnumerable<String> uidQuery = from pair in package.UidList.Values.ToList() select pair.name;

            foreach (GpkBaseProperty prop in export.Properties)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.DefaultCellStyle = gridProps.DefaultCellStyle;

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = prop.name;
                row.Cells.Add(nameCell);

                DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                typeCell.Items.AddRange(((DataGridViewComboBoxColumn)gridProps.Columns["type"]).Items);
                typeCell.ValueType = typeof(string);
                typeCell.Value = prop.type;
                row.Cells.Add(typeCell);

                DataGridViewTextBoxCell sizeCell = new DataGridViewTextBoxCell();
                sizeCell.Value = prop.size;
                row.Cells.Add(sizeCell);

                DataGridViewTextBoxCell arrayCell = new DataGridViewTextBoxCell();
                arrayCell.Value = prop.arrayIndex;
                row.Cells.Add(arrayCell);

                DataGridViewComboBoxCell innerCell = new DataGridViewComboBoxCell();
                innerCell.Items.AddRange(nameQuery.ToArray());

                if (prop is GpkStructProperty)
                {
                    GpkStructProperty struc = (GpkStructProperty)prop;
                    innerCell.Value = struc.innerType;
                }
                else
                {
                    innerCell.Value = "None";
                }
                row.Cells.Add(innerCell);

                DataGridViewTextBoxCell valueCell = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell comboCell = null;
                if (prop is GpkArrayProperty)
                {
                    GpkArrayProperty tmpArray = (GpkArrayProperty)prop;
                    valueCell.Value = tmpArray.GetValueHex();
                }
                else if (prop is GpkStructProperty)
                {
                    GpkStructProperty tmpStruct = (GpkStructProperty)prop;
                    valueCell.Value = tmpStruct.GetValueHex();
                }
                else if (prop is GpkNameProperty)
                {
                    GpkNameProperty tmpName = (GpkNameProperty)prop;
                    comboCell = new DataGridViewComboBoxCell();
                    comboCell.Items.AddRange(nameQuery.ToArray());
                    comboCell.Value = tmpName.name;

                }
                else if (prop is GpkObjectProperty)
                {
                    GpkObjectProperty tmpObj = (GpkObjectProperty)prop;
                    comboCell = new DataGridViewComboBoxCell();
                    comboCell.Items.AddRange(package.UidList.Keys.ToArray());
                    comboCell.Value = tmpObj.objectName;

                }
                else if (prop is GpkByteProperty)
                {
                    GpkByteProperty tmpByte = (GpkByteProperty)prop;
                    comboCell = new DataGridViewComboBoxCell();
                    if (tmpByte.size == 8)
                    {
                        comboCell.Items.AddRange(nameQuery.ToArray());
                        comboCell.Value = tmpByte.nameValue;
                    }
                    else
                    {
                        comboCell.Value = tmpByte.byteValue;
                    }
                }
                else if (prop is GpkFloatProperty)
                {
                    GpkFloatProperty tmpFloat = (GpkFloatProperty)prop;
                    valueCell.Value = tmpFloat.value;
                }
                else if (prop is GpkIntProperty)
                {
                    GpkIntProperty tmpInt = (GpkIntProperty)prop;
                    valueCell.Value = tmpInt.value;
                }
                else if (prop is GpkStringProperty)
                {
                    GpkStringProperty tmpString = (GpkStringProperty)prop;
                    valueCell.Value = tmpString.value;
                }
                else if (prop is GpkBoolProperty)
                {
                    GpkBoolProperty tmpBool = (GpkBoolProperty)prop;
                    valueCell.Value = tmpBool.value;
                }
                else
                {
                    logger.Info("LOL");
                }


                if (comboCell == null)
                {
                    row.Cells.Add(valueCell);
                }
                else
                {
                    row.Cells.Add(comboCell);
                }


                gridProps.Rows.Add(row);



            }

        }



        private void gridProps_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            IEnumerable<String> nameQuery = from pair in selectedPackage.NameList.Values.ToList() select pair.name;

            DataGridViewRow row = gridProps.Rows[e.Row.Index];
            row.Cells[0].ValueType = typeof(String);
            row.Cells[0].Value = "[NEW]";
            //row.Cells[1].Value = "FloatProperty"; user should select that on hisself first
            row.Cells[2].ValueType = typeof(String);
            row.Cells[2].Value = "0";
            row.Cells[3].ValueType = typeof(String);
            row.Cells[3].Value = "0";

            row.Cells[4] = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[4];
            cell.ValueType = typeof(String);
            cell.Items.AddRange(nameQuery.ToArray());
            row.Cells[4].Value = "None";

            row.Cells[5].ValueType = typeof(String);
            row.Cells[5].Value = "";
        }

        private void btnPropSave_Click(object sender, EventArgs e)
        {
            //1. compare and alter
            //or 2. read and rebuild  -- this. we skip to the next in case of user input error.

            if (selectedExport == null || selectedPackage == null)
            {
                logger.Info("save failed");
                return;
            }

            List<IProperty> list = new List<IProperty>();
            foreach (DataGridViewRow row in gridProps.Rows)
            {
                try
                {
                    if (!row.IsNewRow)
                    {
                        list.Add(readProperty(row));
                    }
                }
                catch (Exception ex)
                {

                    logger.Info("Failed to save row {0}, {1}!", row.Index, ex);
                }

            }

            selectedExport.Properties = list;
            logger.Info("Saved properties of export {0}.", selectedExport.UID);

        }

        private void btnPropClear_Click(object sender, EventArgs e)
        {
            if (selectedExport == null || selectedPackage == null)
            {
                logger.Info("save failed");
                return;
            }

            selectedExport.Properties.Clear();
            DrawGrid(selectedPackage, selectedExport);
            logger.Info("Cleared!");
        }

        private IProperty readProperty(DataGridViewRow row)
        {
            GpkBaseProperty baseProp = new GpkBaseProperty(row.Cells["name"].Value.ToString(), row.Cells["type"].Value.ToString(), 0, Convert.ToInt32(row.Cells["aIndex"].Value.ToString()));
            IProperty iProp = null;

            //Check & Add name to our namelist
            selectedPackage.AddString(baseProp.name);

            string cellValue = row.Cells["value"].Value.ToString();
            switch (baseProp.type)
            {
                case "StructProperty":
                    GpkStructProperty tmpStruct = new GpkStructProperty(baseProp);
                    tmpStruct.innerType = row.Cells["iType"].Value.ToString();
                    tmpStruct.value = (cellValue).ToBytes();
                    iProp = tmpStruct;
                    break;
                case "ArrayProperty":
                    GpkArrayProperty tmpArray = new GpkArrayProperty(baseProp);
                    tmpArray.value = (cellValue).ToBytes();
                    iProp = tmpArray;
                    break;
                case "ByteProperty":
                    GpkByteProperty tmpByte = new GpkByteProperty(baseProp);

                    if (cellValue.Length > 2)
                    {
                        selectedPackage.AddString(cellValue); //just in case 
                        tmpByte.nameValue = cellValue;

                    }
                    else
                    {
                        tmpByte.byteValue = Convert.ToByte(cellValue);
                    }
                    iProp = tmpByte;
                    break;

                case "NameProperty":
                    GpkNameProperty tmpName = new GpkNameProperty(baseProp);
                    selectedPackage.AddString(cellValue); //just in case 
                    tmpName.value = cellValue;
                    iProp = tmpName;
                    break;
                case "ObjectProperty":
                    GpkObjectProperty tmpObj = new GpkObjectProperty(baseProp);
                    selectedPackage.GetObjectByUID(cellValue); //throws ex if uid is not present
                    tmpObj.objectName = cellValue;
                    iProp = tmpObj;
                    break;

                case "BoolProperty":
                    GpkBoolProperty tmpBool = new GpkBoolProperty(baseProp);
                    tmpBool.value = Convert.ToBoolean(row.Cells[5].Value);
                    iProp = tmpBool;
                    break;

                case "IntProperty":
                    GpkIntProperty tmpInt = new GpkIntProperty(baseProp);
                    tmpInt.value = Convert.ToInt32(row.Cells[5].Value);
                    iProp = tmpInt;
                    break;

                case "FloatProperty":
                    GpkFloatProperty tmpFloat = new GpkFloatProperty(baseProp);
                    tmpFloat.value = Convert.ToSingle(row.Cells[5].Value);
                    iProp = tmpFloat;
                    break;

                case "StrProperty":
                    GpkStringProperty tmpStr = new GpkStringProperty(baseProp);
                    tmpStr.value = (row.Cells[5].Value.ToString());
                    iProp = tmpStr;
                    break;

                default:
                    throw new Exception(
                        string.Format("Unknown Property Type {0}, Prop_Name {1}", baseProp.type, baseProp.name));

            }

            iProp.RecalculateSize();
            return iProp;
        }
        #endregion












    }
}


