using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using GPK_RePack.Editors;
using GPK_RePack.Forms.Helper;
using GPK_RePack.IO;
using GPK_RePack.Model;
using GPK_RePack.Model.Composite;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Payload;
using GPK_RePack.Model.Prop;
using GPK_RePack.Properties;
using GPK_RePack.Updater;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using NLog.Windows.Forms;
using UpkManager.Dds;
using UpkManager.Dds.Constants;

namespace GPK_RePack.Forms
{
    public partial class GUI : Form, UpdaterCheckCallback
    {
        public GUI()
        {
            InitializeComponent();
        }

        #region def

        public static Logger logger;

        private GpkPackage selectedPackage;
        private GpkExport selectedExport;
        private string selectedClass = "";

        private GpkStore gpkStore;
        private List<GpkExport>[] changedExports;

        private readonly DataFormats.Format exportFormat = DataFormats.GetFormat(typeof(GpkExport).FullName);

        private VorbisWaveReader waveReader;
        private WaveOut waveOut;

        private List<TreeNode> searchResultNodes = new List<TreeNode>();
        private int searchResultIndex = 0;
        private TextBoxTempShow tempStatusLabel;
        private TabPage texturePage;
        #endregion

        #region Main

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        private void scaleFont()
        {
            float scaleFactor = 1;
            if (Settings.Default != null)
                scaleFactor = Settings.Default.ScaleFactorHack;

            Font = new Font(Font.Name, 8.25f * scaleFactor, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            statusStrip.Font = Font;
            menuStrip.Font = Font;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            try
            {
                //setting file check
                String config_path = (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                if (!File.Exists(config_path))
                {
                    File.WriteAllText(config_path, Resources.Config);
                    MessageBox.Show("Setting file was missing. Please restart the application.");
                    Environment.Exit(0);
                }

                //nlog init
                NLogConfig.SetDefaultConfig();
                logger = LogManager.GetLogger("GUI");
                Debug.Assert(logger != null);


                //Our stuff
                logger.Info("Startup");
                UpdateCheck.checkForUpdate(this);
                gpkStore = new GpkStore();
                gpkStore.PackagesChanged += DrawPackages;
                tempStatusLabel = new TextBoxTempShow(lblStatus, this);

                //audio
                waveOut = new WaveOut();
                waveOut.PlaybackStopped += WaveOutOnPlaybackStopped;

                if (Settings.Default.SaveDir == "")
                    Settings.Default.SaveDir = Directory.GetCurrentDirectory();

                if (Settings.Default.OpenDir == "")
                    Settings.Default.OpenDir = Directory.GetCurrentDirectory();

                if (Settings.Default.WorkingDir == "")
                    Settings.Default.WorkingDir = Directory.GetCurrentDirectory();

                texturePage = tabTexturePreview;
                hidePreviewTab();

                //mappings
                if (Settings.Default.LoadMappingOnStart && Settings.Default.CookedPCPath != "")
                {
                    new Task(() =>
                    {
                        loadAndParseMapping(Settings.Default.CookedPCPath);
                    }).Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

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


        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetGUI();
            changedExports = null;
            ResetOggPreview();
            gpkStore.clearGpkList();
            //DrawPackages();
            GC.Collect(); //memory cleanup
        }

        public void ResetGUI()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => ResetGUI()));
                return;
            }

            selectedExport = null;
            selectedPackage = null;
            selectedClass = "";
            boxInfo.Text = "";
            boxGeneralButtons.Enabled = false;
            boxDataButtons.Enabled = false;
            boxPropertyButtons.Enabled = false;
            ProgressBar.Value = 0;
            lblStatus.Text = "Ready";
            ClearGrid();
            boxImagePreview.Image = null;
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("Shutdown");
            Settings.Default.Save();
            if (waveReader != null)
            {
                waveReader.Dispose();
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
            }
            LogManager.Flush();

        }


        public void postUpdateResult(bool updateAvailable)
        {
            if (updateAvailable)
            {
                logger.Info("A newer version is available. Download it at https://github.com/GoneUp/GPK_RePack/releases");
            }
        }


        #endregion

        #region load/save
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] files;
            if (sender is String[])
            {
                files = (String[])sender;
            }
            else
            {
                files = MiscFuncs.GenerateOpenDialog(true, this, false);
            }

            if (files.Length == 0) return;

            DateTime start = DateTime.Now;
            List<IProgress> runningReaders = new List<IProgress>();
            List<Task> runningTasks = new List<Task>();


            foreach (var path in files)
            {
                if (File.Exists(path))
                {
                    Task newTask = new Task(() =>
                    {
                        Reader reader = new Reader();
                        gpkStore.loadGpk(path, reader, false);
                        runningReaders.Add(reader);
                    });
                    newTask.Start();
                    runningTasks.Add(newTask);
                }
            }

            //display info while loading
            while (!Task.WaitAll(runningTasks.ToArray(), 50))
            {
                Application.DoEvents();
                DisplayStatus(runningReaders, "Loading", start);
                //Thread.Sleep(50);
            }

            //Diplay end info
            DisplayStatus(runningReaders, "Loading", start);

            //for patchmode
            Array.Resize(ref changedExports, gpkStore.LoadedGpkPackages.Count);
            for (int i = 0; i < changedExports.Length; i++)
            {
                changedExports[i] = new List<GpkExport>();
            }

            //gpkstore events don't work good with more than one package
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
                            Writer tmpS = new Writer();
                            GpkPackage package = gpkStore.LoadedGpkPackages[i];
                            string savepath = package.Path + "_patched";
                            tmpS.SaveReplacedExport(package, savepath, list);
                            logger.Info(string.Format("Saved the changed data of package '{0} to {1}'!",
                                package.Filename, savepath));
                            save = true;
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal(ex, "Save failure! ");
                        }
                    }
                }


            }

            if (!save)
            {
                logger.Info("Nothing to save in PatchMode!");
            }
        }

        private void patchObjectMapperforSelectedPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedPackage == null)
            {
                logger.Info("No package selected");
                return;
            }

            gpkStore.MultiPatchObjectMapper(selectedPackage, Settings.Default.CookedPCPath);
        }

        private void savepaddingStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void btnSavePatchedComposite_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void saveAddedCompositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            List<IProgress> runningSavers = new List<IProgress>();
            List<Task> runningTasks = new List<Task>();
            bool usePadding = sender == savePaddingStripMenuItem;
            bool patchComposite = false;
            bool addComposite = false;
            if (sender == savePatchedCompositeStripMenuItem)
            {
                usePadding = true;
                patchComposite = true;
            }
            else if (sender == saveAddedCompositeToolStripMenuItem)
            {
                addComposite = true;
            }

            if (gpkStore.LoadedGpkPackages.Count == 0)
                return;

            //do it
            this.gpkStore.SaveGpkListToFiles(gpkStore.LoadedGpkPackages, usePadding, patchComposite, addComposite, runningSavers, runningTasks);

            //display info while loading
            while (!Task.WaitAll(runningTasks.ToArray(), 50))
            {
                Application.DoEvents();
                DisplayStatus(runningSavers, "Saving", start);
            }

            //Diplay end info
            DisplayStatus(runningSavers, "Saving", start);

            logger.Info("Saving done!");
        }

        private void DisplayStatus(List<IProgress> list, string tag, DateTime start)
        {
            if (list.Count == 0)
                return;

            long actual = 0, total = 0, finished = 0;
            foreach (IProgress p in list)
            {
                if (p == null) continue;
                Status stat = p.GetStatus();


                if (stat.subGpkCount > 1)
                {
                    //dont show actual objects, just the sub-file count
                    actual += stat.subGpkDone;
                    total += stat.subGpkCount;
                    if (actual == total) finished++;
                }
                else
                {
                    //normal gpk 
                    actual += stat.progress;
                    total += stat.totalobjects;
                    if (stat.finished) finished++;
                }


            }

            if (finished < list.Count)
            {
                if (total > 0) ProgressBar.Value = (int)(((double)actual / (double)total) * 100);
                lblStatus.Text = String.Format("[{0}] Finished {1}/{2}", tag, finished, list.Count);
            }
            else
            {
                total = 0;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(String.Format("[{0} Task Info]", tag));
                foreach (IProgress p in list)
                {
                    var stat = p.GetStatus();
                    total += stat.time;
                    builder.AppendLine(String.Format("Task {0}: {1}ms", stat.name, stat.time));
                }
                builder.AppendLine(string.Format("Avg Worktime: {0}ms", total / list.Count));
                builder.AppendLine(string.Format("Total elapsed Time: {0}ms", (int)DateTime.Now.Subtract(start).TotalMilliseconds));

                boxInfo.Text = builder.ToString();
                ProgressBar.Value = 0;
                lblStatus.Text = "Ready";
            }
        }

        private void treeMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void treeMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                logger.Debug("Drop input: " + file);
            }

            openToolStripMenuItem_Click(files, null);
        }

        #endregion

        #region diplaygpk
        public void DrawPackages()
        {
            //we may get calls out of gpkStore
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => DrawPackages()));
                return;
            }

            treeMain.BeginUpdate();
            treeMain.Nodes.Clear();

            if (Settings.Default.EnableSortTreeNodes)
            {
                treeMain.TreeViewNodeSorter = new MiscFuncs.NodeSorter();
            }

            var toAdd = new List<TreeNode>();

            for (int i = 0; i < gpkStore.LoadedGpkPackages.Count; i++)
            {
                GpkPackage package = gpkStore.LoadedGpkPackages[i];
                //TreeNode nodeP = treeMain.Nodes.Add(i.ToString(), package.Filename);
                TreeNode nodeP = new TreeNode(package.Filename);
                nodeP.Name = i.ToString(); //Key


                Dictionary<string, TreeNode> classNodes = new Dictionary<string, TreeNode>();
                TreeNode nodeI = null;
                TreeNode nodeE = null;

                if (Settings.Default.ShowImports)
                {
                    foreach (var tmp in package.ImportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                    {
                        string key = tmp.Value.UID;
                        string value = tmp.Value.ObjectName;
                        if (Settings.Default.UseUID) value = key;

                        switch (Settings.Default.ViewMode)
                        {
                            case "normal":
                                if (nodeI == null)
                                    nodeI = nodeP.Nodes.Add("Imports");

                                nodeI.Nodes.Add(key, value);
                                break;
                            case "class":
                                CheckClassNode(tmp.Value.ClassName, classNodes, nodeP);
                                classNodes[tmp.Value.ClassName].Nodes.Add(key, value);
                                break;
                        }

                    }
                }

                //Exports
                foreach (var tmp in package.ExportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                {
                    string key = tmp.Value.UID;
                    string value = tmp.Value.ObjectName;
                    if (Settings.Default.UseUID) value = key;

                    switch (Settings.Default.ViewMode)
                    {
                        case "normal":
                            if (nodeE == null)
                                nodeE = nodeP.Nodes.Add("Exports");


                            nodeE.Nodes.Add(key, value);
                            break;
                        case "class":
                            CheckClassNode(tmp.Value.ClassName, classNodes, nodeP);
                            classNodes[tmp.Value.ClassName].Nodes.Add(key, value);
                            break;

                        case "package":
                            CheckClassNode(tmp.Value.PackageName, classNodes, nodeP);
                            classNodes[tmp.Value.PackageName].Nodes.Add(key, value);
                            break;

                    }

                }

                treeMain.Nodes.Add(nodeP);
            }


            treeMain.EndUpdate();
        }




        private void CheckClassNode(string className, Dictionary<string, TreeNode> classNodes, TreeNode mainNode)
        {

            if (!className.Contains("."))
            {
                //base case
                if (!classNodes.ContainsKey(className))
                {
                    TreeNode classNode = mainNode.Nodes.Add(className);
                    classNodes.Add(className, classNode);
                }
            }
            else
            {
                var split = className.Split('.').ToList();
                String toAdd = split.Last();
                split.RemoveAt(split.Count - 1);
                String left = String.Join(".", split);
                //Debug.Print("toadd {0} left {1}", toAdd, left);

                //recursion to add missing nodes
                if (!classNodes.ContainsKey(left))
                {
                    CheckClassNode(left, classNodes, mainNode);
                }

                if (!classNodes.ContainsKey(className))
                {
                    TreeNode classNode = classNodes[left].Nodes.Add(toAdd);
                    classNodes.Add(className, classNode);
                }
            }

        }


        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var tabIndex = tabControl.SelectedIndex;
            ResetGUI();

            try
            {
                if (e.Node.Level == 0)
                {
                    boxGeneralButtons.Enabled = true;
                    boxDataButtons.Enabled = true;

                    selectedPackage = gpkStore.LoadedGpkPackages[Convert.ToInt32(e.Node.Name)];
                    boxInfo.Text = selectedPackage.ToString();
                }
                else if (e.Node.Level == 1 && Settings.Default.ViewMode == "class")
                {
                    selectedPackage = gpkStore.LoadedGpkPackages[Convert.ToInt32(e.Node.Parent.Name)];
                    selectedClass = e.Node.Text;

                    boxDataButtons.Enabled = true;
                }

                //check if we have a leaf
                else if (e.Node.Level == 2 && Settings.Default.ViewMode == "normal" ||
                     e.Node.Nodes.Count == 0)
                {
                    GpkPackage package = gpkStore.LoadedGpkPackages[Convert.ToInt32(getRootNode().Name)];
                    Object selected = package.GetObjectByUID(e.Node.Name);

                    if (selected is GpkImport)
                    {
                        GpkImport imp = (GpkImport)selected;
                        boxInfo.Text = imp.ToString();

                    }
                    else if (selected is GpkExport)
                    {
                        GpkExport exp = (GpkExport)selected;

                        //buttons
                        boxGeneralButtons.Enabled = true;
                        boxDataButtons.Enabled = true;
                        boxPropertyButtons.Enabled = true;
                        selectedExport = exp;
                        selectedPackage = package;

                        refreshExportInfo();
                    }
                }

                if (tabIndex < tabControl.TabCount)
                {
                    tabControl.SelectedIndex = tabIndex;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to display infotext.");
            }
        }

        private TreeNode getRootNode()
        {
            TreeNode node = treeMain.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        private void refreshExportInfo()
        {
            //tabs
            boxInfo.Text = selectedExport.ToString();
            DrawGrid(selectedPackage, selectedExport);

            boxImagePreview.Image = null;
            if (selectedExport.Payload != null && selectedExport.Payload is Texture2D)
            {
                showPreviewTab();
                tabControl_Selected(null, new TabControlEventArgs(tabControl.SelectedTab, tabControl.SelectedIndex, new TabControlAction()));
            }
            else
            {
                hidePreviewTab();
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {

            if (selectedExport == null)
                return;

            if (e.TabPage == tabTexturePreview)
            {
                if (selectedExport.Payload != null && selectedExport.Payload is Texture2D)
                {

                    Texture2D image = (Texture2D)selectedExport.Payload;
                    DdsFile ddsFile = new DdsFile();
                    Stream imageStream = image.GetObjectStream();
                    if (imageStream != null)
                    {
                        ddsFile.Load(image.GetObjectStream());

                        boxImagePreview.Image = TextureTools.BitmapFromSource(ddsFile.BitmapSource);
                        boxImagePreview.BackColor = Settings.Default.PreviewColor;
                        //workaround for a shrinking window
                        scaleFont();
                        resizePiutureBox();
                    }
                }
            }

        }

        private void resizePiutureBox()
        {
            if (boxImagePreview.Image == null)
                return;

            if (boxImagePreview.Image.Height > boxImagePreview.Height || boxImagePreview.Image.Width > boxImagePreview.Width)
            {
                //shrink the file if ´the size is to big
                boxImagePreview.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                boxImagePreview.SizeMode = PictureBoxSizeMode.Normal;
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
                if (selectedExport.Data == null)
                {
                    logger.Info("Length is zero. Nothing to export");
                    return;
                }

                var path = MiscFuncs.GenerateSaveDialog(selectedExport.ObjectName, ".raw");
                if (path == "") return;
                DataTools.WriteExportDataFile(path, selectedExport);
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
                        if (exp.Data != null)
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
                        if (exp.Data != null)
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
            if (selectedExport.Data == null)
            {
                logger.Trace("no export data");
                return;
            }

            String[] files = MiscFuncs.GenerateOpenDialog(false, this);
            if (files.Length == 0) return;
            string path = files[0];

            if (File.Exists(path))
            {
                byte[] buffer = File.ReadAllBytes(path);


                if (Settings.Default.PatchMode)
                {
                    if (treeMain.SelectedNode.Parent.Parent == null) return;
                    int packageIndex = Convert.ToInt32(treeMain.SelectedNode.Parent.Parent.Name);

                    if (buffer.Length > selectedExport.Data.Length)
                    {
                        //Too long, not possible without rebuiling the gpk
                        logger.Info("File size too big for PatchMode. Size: " + buffer.Length + " Maximum Size: " +
                                 selectedExport.Data.Length);
                        return;
                    }

                    //selectedExport.data = buffer;
                    Array.Copy(buffer, selectedExport.Data, buffer.Length);

                    changedExports[packageIndex].Add(selectedExport);

                }
                else
                {
                    //Rebuild Mode
                    //We force the rebuilder to recalculate the size. (atm we dont know how big the propertys are)
                    logger.Trace(String.Format("rebuild mode old size {0} new size {1}", selectedExport.Data.Length,
                        buffer.Length));

                    selectedExport.Data = buffer;
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
                gpkStore.DeleteGpk(selectedPackage);

                logger.Info("Removed package {0}...", selectedPackage.Filename);

                selectedPackage = null;
                boxGeneralButtons.Enabled = false;
                GC.Collect(); //memory cleanup
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

            try
            {
                MemoryStream memorystream = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(memorystream, selectedExport);
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
                logger.Info("Serialize failed, check debug log");
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
            GpkExport copyExport = (GpkExport)Clipboard.GetData(exportFormat.Name);

            if (copyExport == null)
            {
                logger.Info("copy paste fail");
                return;
            }

            logger.Trace(Settings.Default.CopyMode);
            string option = "";

            switch (Settings.Default.CopyMode)
            {
                case "all":
                    DataTools.ReplaceAll(copyExport, selectedExport);
                    option = "everything";
                    break;
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

        private void insertStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedPackage == null)
            {
                logger.Trace("no selected package to insert");
                return;
            }
            GpkExport copyExport = (GpkExport)Clipboard.GetData(exportFormat.Name);

            if (copyExport == null)
            {
                logger.Info("copy paste fail");
                return;
            }

            selectedPackage.CopyObjectFromPackage(copyExport.UID, copyExport.motherPackage, true);

            DrawPackages();
            logger.Info("Insert done");
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            if (selectedExport.Data == null)
            {
                logger.Trace("no export data");
                return;
            }

            selectedExport.Loader = null;
            selectedExport.Data = null;
            selectedExport.DataPadding = null;
            selectedExport.Payload = null;
            selectedExport.GetDataSize();

            treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
        }



        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                //Copypaste
                case Keys.Control | Keys.Shift | Keys.C:
                    btnCopy_Click(btnCopy, new EventArgs());
                    return true;

                case Keys.Control | Keys.Shift | Keys.V:
                    btnPaste_Click(btnPaste, new EventArgs());
                    return true;

                //Search
                case Keys.Control | Keys.F:
                    searchForObjectToolStripMenuItem_Click(null, null);
                    return true;
                case Keys.F3:
                    nextToolStripMenuItem_Click(null, null);
                    return true;

                //menu
                case Keys.Control | Keys.O:
                    openToolStripMenuItem_Click(null, null);
                    return true;
                case Keys.Control | Keys.S:
                    saveToolStripMenuItem_Click(null, null);
                    return true;
                case Keys.Control | Keys.P:
                    savepaddingStripMenuItem_Click(null, null);
                    return true;
                case Keys.Control | Keys.Shift | Keys.P:
                    replaceSaveToolStripMenuItem_Click(null, null);
                    return true;

                //TABS
                case Keys.Control | Keys.D1:
                    tabControl.SelectedIndex = 0;
                    boxInfo.Select(0, 0); //prevent full text selection
                    return true;
                case Keys.Control | Keys.D2:
                    tabControl.SelectedIndex = 1;
                    return true;
                case Keys.Control | Keys.D3:
                    if (tabControl.TabCount > 2)
                        tabControl.SelectedIndex = 2;
                    return true;

            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }

        #endregion

        #region image



        private void btnImageImport_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            if (selectedExport.Payload == null || !(selectedExport.Payload is Texture2D))
            {
                logger.Info("Not a Texture2D object");
                return;
            }

            string[] files = MiscFuncs.GenerateOpenDialog(false, this);
            if (files.Length == 0) return;

            if (files[0] != "" && File.Exists(files[0]))
            {
                TextureTools.importTexture(selectedExport, files[0]);
                refreshExportInfo();
            }
        }


        private void btnImageExport_Click(object sender, EventArgs e)
        {
            if (selectedExport == null)
            {
                logger.Trace("no selected export");
                return;
            }

            if (selectedExport.Payload == null || !(selectedExport.Payload is Texture2D))
            {
                logger.Info("Not a Texture2D object");
                return;
            }

            var path = MiscFuncs.GenerateSaveDialog(selectedExport.ObjectName, ".dds");
            if (path != "")
            {
                new Task(() => TextureTools.exportTexture(selectedExport, path)).Start();
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
                    String[] files = MiscFuncs.GenerateOpenDialog(false, this);
                    if (files.Length == 0) return;

                    if (File.Exists(files[0]))
                    {
                        SoundwaveTools.ImportOgg(selectedExport, files[0]);
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
                logger.Fatal(ex, "Import failure!");
            }
        }

        private void btnExtractOGG_Click(object sender, EventArgs e)
        {

            if (selectedExport != null && selectedExport.ClassName == "Core.SoundNodeWave")
            {
                var path = MiscFuncs.GenerateSaveDialog(selectedExport.ObjectName, ".ogg");
                if (path != "")
                    SoundwaveTools.ExportOgg(selectedExport, path);
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
                if (selectedExport != null && selectedExport.Payload is Soundwave && waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    Soundwave wave = (Soundwave)selectedExport.Payload;
                    waveReader = new VorbisWaveReader(new MemoryStream(wave.oggdata));
                    waveOut.Init(waveReader);
                    waveOut.Play();
                    btnPreviewOgg.Text = "Stop Preview";
                }
                else if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                {
                    ResetOggPreview();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Playback Error");
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
            btnPreviewOgg.Text = "Ogg Preview";
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

        private void addNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageSelected()) return;

            string input = Microsoft.VisualBasic.Interaction.InputBox("Add a new name to the package:");
            if (input != "")
            {
                selectedPackage.AddString(input);
                if (selectedExport != null)
                    DrawGrid(selectedPackage, selectedExport);
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

        private bool ExportSelected()
        {
            if (selectedExport == null)
            {
                logger.Info("Select a export!");
                return false;
            }

            return true;
        }



        private void showPreviewTab()
        {
            if (!tabControl.TabPages.Contains(tabTexturePreview))
                tabControl.TabPages.Add(tabTexturePreview);
        }


        private void hidePreviewTab()
        {
            if (tabControl.TabPages.Contains(tabTexturePreview))
                tabControl.TabPages.Remove(tabTexturePreview);
        }


        private void dumpHeadersMenuItem_Click(object sender, EventArgs e)
        {
            NLogConfig.DisableFormLogging();

            string[] files = MiscFuncs.GenerateOpenDialog(true, this, true, "GPK (*.gpk;*.upk;*.gpk_rebuild)|*.gpk;*.upk;*.gpk_rebuild|All files (*.*)|*.*");
            if (files.Length == 0) return;

            string outfile = MiscFuncs.GenerateSaveDialog("dump", ".txt");

            new Task(() =>
            {
                logger.Info("Dump is running in the background");
                MassDumper.DumpMassHeaders(outfile, files);
                logger.Info("Dump done");

                NLogConfig.EnableFormLogging();
            }).Start();

        }

        private void loggingActiveMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (loggingActiveMenuItem.Checked)
            {
                NLogConfig.EnableFormLogging();
            }
            else
            {
                NLogConfig.DisableFormLogging();
            }
        }

        #region search
        private void searchForObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("String to search?", "Search");

            if (String.IsNullOrEmpty(input))
                return;

            searchResultNodes.Clear();
            searchResultIndex = 0;

            foreach (TreeNode node in Collect(treeMain.Nodes))
            {
                if (node.Text.ToLowerInvariant().Contains(input.ToLowerInvariant().Trim()))
                {
                    searchResultNodes.Add(node);
                }
            }

            if (searchResultNodes.Count > 0)
            {
                tryToSelectNextSearchResult();
            }
            else
            {
                logger.Info(string.Format("Nothing found for '{0}'!", input));
            }


        }

        IEnumerable<TreeNode> Collect(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in Collect(node.Nodes))
                    yield return child;
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryToSelectNextSearchResult();
        }

        private void tryToSelectNextSearchResult()
        {
            if (searchResultNodes.Count == 0 || searchResultIndex >= searchResultNodes.Count)
            {
                SystemSounds.Asterisk.Play();
                searchResultIndex = 0;
                tempStatusLabel.StartTimer("Ready", String.Format("End reached, searching from start.", searchResultIndex, searchResultNodes.Count), 2000);
                return;

            }

            treeMain.SelectedNode = searchResultNodes[searchResultIndex];
            treeMain_AfterSelect(this, new TreeViewEventArgs(searchResultNodes[searchResultIndex]));
            tempStatusLabel.StartTimer("Ready", String.Format("Result {0}/{1}", searchResultIndex + 1, searchResultNodes.Count), 2000);

            searchResultIndex++;
        }


        #endregion //search
        #endregion //misc

        #region composite gpk
        private void decryptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] files = MiscFuncs.GenerateOpenDialog(false, this, true);
            if (files.Length == 0) return;

            string outfile = MiscFuncs.GenerateSaveDialog("decrypt", ".txt");

            new Task(() =>
            {
                logger.Info("Decryption is running in the background");

                MapperTools.DecryptAndWriteFile(files[0], outfile);

                logger.Info("Decryption done");

            }).Start();
        }


        private void datEncryptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] files = MiscFuncs.GenerateOpenDialog(false, this, true);
            if (files.Length == 0) return;

            string outfile = MiscFuncs.GenerateSaveDialog("encrypt", ".txt");

            new Task(() =>
            {
                logger.Info("Encryption is running in the background");

                MapperTools.EncryptAndWriteFile(files[0], outfile);

                logger.Info("Encryption done");

            }).Start();
        }

        private void loadMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gpkStore.CompositeMap.Count > 0)
            {
                new FormMapperView(gpkStore).Show();
                return;
            }

            var dialog = new FolderBrowserDialog();
            if (Settings.Default.CookedPCPath != "")
                dialog.SelectedPath = Settings.Default.CookedPCPath;
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath + "\\";
            Settings.Default.CookedPCPath = path;

            loadAndParseMapping(path);

            new FormMapperView(gpkStore).Show();
        }

        private void loadAndParseMapping(string path)
        {

            gpkStore.BaseSearchPath = path;
            MapperTools.ParseMappings(path, gpkStore);

            int subCount = gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", gpkStore.CompositeMap.Count, subCount);
        }


        private void writeMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (Settings.Default.WorkingDir != "")
                dialog.SelectedPath = Settings.Default.WorkingDir;
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath + "\\";

            MapperTools.WriteMappings(path, gpkStore, true, true);
        }


        private void dumpCompositeTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cookedpc path, outdir path
            var dialog = new FolderBrowserDialog();
            if (Settings.Default.CookedPCPath != "")
                dialog.SelectedPath = Settings.Default.CookedPCPath + "\\";
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;


            var path = dialog.SelectedPath + "\\";
            gpkStore.BaseSearchPath = path;

            Settings.Default.CookedPCPath = path;
            MapperTools.ParseMappings(path, gpkStore);

            int subCount = gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", gpkStore.CompositeMap.Count, subCount);

            //selection
            var text = Microsoft.VisualBasic.Interaction.InputBox("Select range of composite gpks to load. Format: n-n [e.g. 1-5] or empty for all."
                , "Selection", "");
            var filterList = filterCompositeList(text);

            //save dir
            dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Settings.Default.WorkingDir;
            dialog.Description = "Select your output dir";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            logger.Warn("Warning: This function can be ultra long running (hours) and unstable. Monitor logfile and output folder for progress.");
            logger.Warn("Disabling logging, dump is running in the background. Consider setting file logging to only info.");

            NLogConfig.DisableFormLogging();
            var outDir = dialog.SelectedPath;
            new Task(() => MassDumper.DumpMassTextures(gpkStore, outDir, filterList)).Start();
        }
        private void dumpIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cookedpc path, outdir path
            var dialog = new FolderBrowserDialog();
            if (Settings.Default.CookedPCPath != "")
                dialog.SelectedPath = Settings.Default.CookedPCPath;
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath;
            gpkStore.BaseSearchPath = path;
            Settings.Default.CookedPCPath = path;
            MapperTools.ParseMappings(path, gpkStore);

            int subCount = gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", gpkStore.CompositeMap.Count, subCount);
            var list = filterCompositeList("");
            //save dir
            dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Settings.Default.WorkingDir;
            dialog.Description = "Select your output dir";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            logger.Warn("Warning: This function can be ultra long running (hours) and unstable. Monitor logfile and output folder for progress.");
            logger.Warn("Disabling logging, dump is running in the background. Consider setting file logging to only info.");

            NLogConfig.DisableFormLogging();
            var outDir = dialog.SelectedPath;
            new Task(() => MassDumper.DumpMassIcons(gpkStore, outDir, list)).Start();

        }
        private Dictionary<String, List<CompositeMapEntry>> filterCompositeList(string text)
        {
            try
            {
                if (text != "" && text.Split('-').Length > 0)
                {
                    int start = Convert.ToInt32(text.Split('-')[0]) - 1;
                    int end = Convert.ToInt32(text.Split('-')[1]) - 1;
                    var filterCompositeList = gpkStore.CompositeMap.Skip(start).Take(end - start + 1).ToDictionary(k => k.Key, v => v.Value);
                    logger.Info("Filterd down to {0} GPKs.", end - start + 1);
                    return filterCompositeList;
                }
                else
                {
                    return gpkStore.CompositeMap;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "filter fail");
                return gpkStore.CompositeMap;
            }
        }

        private void minimizeGPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageSelected())
                return;

            DataTools.RemoveObjectRedirects(selectedPackage);
            DrawPackages();
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

            foreach (IProperty iProp in export.Properties)
            {
                GpkBaseProperty prop = (GpkBaseProperty)iProp;
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = iProp;
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
                sizeCell.Value = iProp.RecalculateSize();
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
                else if (prop is GpkByteProperty)
                {
                    GpkByteProperty tmpByte = (GpkByteProperty)prop;
                    innerCell.Value = tmpByte.enumType;
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
                    comboCell.Value = tmpName.value;

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
                    if (tmpByte.size == 8 || tmpByte.size == 16)
                    {
                        comboCell = new DataGridViewComboBoxCell();
                        comboCell.Items.AddRange(nameQuery.ToArray());
                        comboCell.Value = tmpByte.nameValue;
                    }
                    else
                    {
                        valueCell.Value = tmpByte.byteValue;
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
                    logger.Info("Unk Prop?!?");
                }

                if (valueCell.Value != null && valueCell.Value.ToString().Length > valueCell.MaxInputLength)
                {
                    valueCell.Value = "[##TOO_LONG##]";
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


            var row = e.Row;
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
            IProperty iProp;

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
                    if (cellValue == "[##TOO_LONG##]")
                    {
                        //use row embeeded property instead
                        tmpArray.value = ((GpkArrayProperty)row.Tag).value;
                    }
                    else
                    {
                        tmpArray.value = (cellValue).ToBytes();
                        tmpArray.size = tmpArray.value.Length;
                        tmpArray.RecalculateSize();
                    }

                    iProp = tmpArray;
                    break;
                case "ByteProperty":
                    GpkByteProperty tmpByte = new GpkByteProperty(baseProp);

                    if (cellValue.Length > 2)
                    {
                        if (selectedPackage.x64)
                        {
                            tmpByte.enumType = row.Cells["iType"].Value.ToString();
                            selectedPackage.AddString(tmpByte.enumType); //just in case 
                        }
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

                case "":
                    //new line, nothing selected
                    throw new Exception(
                       string.Format("You need to select a Property Type for {0}!", baseProp.name));
                default:
                    throw new Exception(
                        string.Format("Unknown Property Type {0}, Prop_Name {1}", baseProp.type, baseProp.name));

            }

            iProp.RecalculateSize();
            return iProp;
        }

        private void BigBytePropExport_Click(object sender, EventArgs e)
        {
            var arrayProp = checkArrayRow();
            if (arrayProp == null || arrayProp.value == null) return;
            byte[] data = new byte[arrayProp.value.Length - 4];
            Array.Copy(arrayProp.value, 4, data, 0, arrayProp.value.Length - 4); //remove count bytes

            String path = MiscFuncs.GenerateSaveDialog(arrayProp.name, ".raw");
            if (path == "") return;

            DataTools.WriteExportDataFile(path, data);
        }

        private void BigBytePropImport_Click(object sender, EventArgs e)
        {
            var arrayProp = checkArrayRow();
            if (arrayProp == null) return;

            String[] files = MiscFuncs.GenerateOpenDialog(false, this);
            if (files.Length == 0) return;
            string path = files[0];
            if (!File.Exists(path)) return;

            byte[] data = File.ReadAllBytes(path);
            //readd count bytes 
            arrayProp.value = new byte[data.Length + 4];
            Array.Copy(BitConverter.GetBytes(data.Length), arrayProp.value, 4);
            Array.Copy(data, 0, arrayProp.value, 4, data.Length);

            DrawGrid(selectedPackage, selectedExport);
        }

        private GpkArrayProperty checkArrayRow()
        {
            if (selectedExport == null) return null;
            if (gridProps.SelectedRows.Count != 1)
            {
                logger.Info("select a complete row (click the arrow in front of it)");
                return null;
            }

            var row = gridProps.SelectedRows[0];
            if (row.Cells["type"].Value.ToString() != "ArrayProperty")
            {
                logger.Info("select a arrayproperty row");
                return null;
            }

            return (GpkArrayProperty)row.Tag;
        }

        private void GUI_Resize(object sender, EventArgs e)
        {
            gridProps.Refresh();
            resizePiutureBox();
        }


        private void btnExportProps_Click(object sender, EventArgs e)
        {
            //JSON?
            //CSV?
            //XML?
            //Name;Type;Size;ArrayIndex;InnerType;Value

            var builder = new StringBuilder();
            builder.AppendLine("Name;Type;Size;ArrayIndex;InnerType;Value");

            foreach (DataGridViewRow row in gridProps.Rows)
            {
                if (row.IsNewRow || row.Cells["name"].Value == null)
                    continue;

                string csvRow = String.Format("{0};{1};{2};{3};{4};{5}",
                    row.Cells["name"].Value.ToString(),
                    row.Cells["type"].Value.ToString(),
                    row.Cells["size"].Value.ToString(),
                    row.Cells["aIndex"].Value.ToString(),
                    row.Cells["iType"].Value.ToString(),
                    row.Cells["value"].Value.ToString()
                    );
                builder.AppendLine(csvRow);
            }


            var path = MiscFuncs.GenerateSaveDialog(selectedExport.ObjectName, ".csv");
            if (path == "") return;

            Task.Factory.StartNew(() => File.WriteAllText(path, builder.ToString(), Encoding.UTF8));

        }



        #endregion

        #region context menu 

        string clickedNode;
        private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeMain.SelectedNode = e.Node;
                clickedNode = e.Node.Name;
                treeContextMenu.Show(treeMain, e.Location);
            }
        }


        private void treeContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //dont keep the menu open
            Task.Factory.StartNew(() => selectContextAction(sender, e));
        }


        [STAThread]
        private void selectContextAction(object sender, ToolStripItemClickedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {

                if (e.ClickedItem == addToolStripMenuItem)
                {

                }
                else if (e.ClickedItem == removeToolStripMenuItem)
                {
                    btnDelete_Click(null, null);
                }
                else if (e.ClickedItem == copyToolStripMenuItem)
                {
                    btnCopy_Click(null, null);
                }
                else if (e.ClickedItem == pasteToolStripMenuItem)
                {
                    btnPaste_Click(null, null);
                }

                //import
                else if (e.ClickedItem == importRawDataToolStripMenuItem)
                {
                    btnReplace_Click(null, null);
                }
                else if (e.ClickedItem == importDDSToolStripMenuItem)
                {
                    btnImageImport_Click(null, null);
                }
                else if (e.ClickedItem == importOGGToolStripMenuItem)
                {
                    btnImportOgg_Click(null, null);
                }

                //export
                else if (e.ClickedItem == exportRawDataToolStripMenuItem)
                {
                    btnExport_Click(null, null);
                }
                else if (e.ClickedItem == exportDDSToolStripMenuItem)
                {
                    btnImageExport_Click(null, null);
                }
                else if (e.ClickedItem == exportOGGToolStripMenuItem)
                {
                    btnExtractOGG_Click(null, null);
                }
                else if (e.ClickedItem == exportPackageToolStripMenuItem)
                {
                    saveSingleGpkPackage();
                }
                //preview ogg

                else if (e.ClickedItem == previewOGGToolStripMenuItem)
                {
                    btnOggPreview_Click(null, null);
                }
            }));
        }

        private void saveSingleGpkPackage()
        {
            if (selectedPackage != null && treeMain.SelectedNode != null && treeMain.SelectedNode.Level == 0)
            {
                var packages = new List<GpkPackage>();
                packages.Add(selectedPackage);
                var writerList = new List<IProgress>();
                var taskList = new List<Task>();

                this.gpkStore.SaveGpkListToFiles(packages, false, false, false, writerList, taskList);

                //wait
                while (!Task.WaitAll(taskList.ToArray(), 50))
                {
                    Application.DoEvents();
                }
                logger.Info("Single export done!");
            }
        }



















        #endregion

  
    }
}


