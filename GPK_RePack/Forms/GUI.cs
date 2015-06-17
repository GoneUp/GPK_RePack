using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using GPK_RePack.Classes;
using GPK_RePack.Classes.Interfaces;
using GPK_RePack.Classes.Prop;
using GPK_RePack.Editors;
using GPK_RePack.Parser;
using GPK_RePack.Properties;
using GPK_RePack.Saver;
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

        private List<GpkPackage> loadedGpkPackages;
        private List<GpkExport>[] changedExports;

        private bool drawing = false;
        private readonly DataFormats.Format exportFormat = DataFormats.GetFormat(typeof(GpkExport).FullName);

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
            ClearGrid();
        }

        private void GUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }


        #endregion

        #region load/save
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.ValidateNames = true;
            open.InitialDirectory = Settings.Default.OpenDir;
            Settings.Default.OpenDir = open.FileName;

            open.ShowDialog();

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
                                nodeI.Nodes.Add(tmp.Value.UID, tmp.Value.ObjectName);
                            }
                        }

                        //Exports
                        TreeNode nodeE = nodeP.Nodes.Add("Exports");
                        foreach (var tmp in package.ExportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                        {
                            nodeE.Nodes.Add(tmp.Value.UID, tmp.Value.ObjectName);
                        }
                        break;
                    case "class":
                        Dictionary<string, TreeNode> classNodes = new Dictionary<string, TreeNode>();
                        if (Settings.Default.ShowImports)
                        {
                            foreach (var tmp in package.ImportList)
                            {
                                if (!classNodes.ContainsKey(tmp.Value.Class))
                                {
                                    TreeNode classNode = nodeP.Nodes.Add(tmp.Value.Class);
                                    classNodes.Add(tmp.Value.Class, classNode);
                                }

                                classNodes[tmp.Value.Class].Nodes.Add(tmp.Value.UID, tmp.Value.ObjectName);

                            }
                        }

                        foreach (var tmp in package.ExportList)
                        {
                            if (!classNodes.ContainsKey(tmp.Value.ClassName))
                            {
                                TreeNode classNode = nodeP.Nodes.Add(tmp.Value.ClassName);
                                classNodes.Add(tmp.Value.ClassName, classNode);
                            }

                            classNodes[tmp.Value.ClassName].Nodes.Add(tmp.Value.UID, tmp.Value.ObjectName);
                        }
                        break;

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
            ClearGrid();

            if (e.Node.Level == 0)
            {
                boxGeneralButtons.Enabled = true;

                selectedPackage = loadedGpkPackages[Convert.ToInt32(e.Node.Name)];
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
                int packageIndex = Convert.ToInt32(treeMain.SelectedNode.Parent.Parent.Name);

                if (Settings.Default.PatchMode)
                {
                    if (buffer.Length > selectedExport.data.Length)
                    {
                        //Too long, not possible without rebuiling the gpk
                        logger.Info("File size too big. Size: " + buffer.Length + " Maximum Size: " +
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

                }
                else
                {
                    //Rebuild Mode
                    //We force the rebuilder to recalculate the size. (atm we dont know how big the propertys are)
                    logger.Trace(String.Format("rebuild mode old size {0} new size {1}", selectedExport.data.Length,
                        buffer.Length));

                    selectedExport.data = buffer;
                    selectedExport.RecalculateSize();
                    selectedPackage.Changes = true;
                }

                changedExports[packageIndex].Add(selectedExport);
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
            switch (Settings.Default.CopyMode)
            {
                case "dataprops":
                    DataTools.ReplaceProperties(copyExport, selectedExport);
                    DataTools.ReplaceData(copyExport, selectedExport);
                    break;
                case "data":
                    DataTools.ReplaceData(copyExport, selectedExport);
                    break;
                case "props":
                    DataTools.ReplaceProperties(copyExport, selectedExport);
                    break;
                default:
                    logger.Info("Your setting file is broken. Go to settings windows and select a copymode.");
                    break;

            }

            copyExport.RecalculateSize();
            treeMain_AfterSelect(treeMain, new TreeViewEventArgs(treeMain.SelectedNode));
            logger.Info("Pasted the data and properties of {0} to {1}", copyExport.UID, selectedExport.UID);
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
            selectedExport.RecalculateSize();

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
            if (selectedExport != null)
            {
                try
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
                    }
                    else
                    {
                        logger.Info("File not found.");
                    }
                }
                catch (Exception ex)
                {
                    logger.FatalException("Import failure! " + ex, ex);
                }
            }
        }

        private void btnExtractOGG_Click(object sender, EventArgs e)
        {

            if (selectedExport != null)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = selectedExport.ObjectName;
                save.InitialDirectory = Settings.Default.SaveDir;
                save.DefaultExt = ".ogg";
                save.ShowDialog();
                Settings.Default.SaveDir = save.FileName;

                SoundwaveTools.ExportOgg(selectedExport, save.FileName);
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


        #endregion

        #region misc
        private void setFilesizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedPackage == null)
            {
                logger.Info("Select a package!");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox("New filesize for " + selectedPackage.Filename + "? Old: " + selectedPackage.OrginalSize, "Filesize");

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
        #endregion

        #region propgrid

        private void ClearGrid()
        {
            drawing = true;
            gridProps.Rows.Clear();
            drawing = false;
        }

        private void DrawGrid(GpkPackage package, GpkExport export)
        {
            drawing = true;

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
                typeCell.Items.AddRange(((DataGridViewComboBoxColumn)gridProps.Columns["colType"]).Items);
                typeCell.Value = prop.type;
                row.Cells.Add(typeCell);

                DataGridViewTextBoxCell sizeCell = new DataGridViewTextBoxCell();
                sizeCell.Value = prop.size;
                row.Cells.Add(sizeCell);

                DataGridViewTextBoxCell arrayCell = new DataGridViewTextBoxCell();
                arrayCell.Value = prop.arrayIndex;
                row.Cells.Add(arrayCell);

                DataGridViewComboBoxCell innerCell = new DataGridViewComboBoxCell();

                if (prop is GpkStructProperty)
                {
                    GpkStructProperty struc = (GpkStructProperty)prop;
                    innerCell.Items.AddRange(nameQuery.ToArray());
                    innerCell.Value = struc.innerType;

                }
                else
                {
                    innerCell.Items.Add("none");
                    innerCell.Value = "none";
                }
                row.Cells.Add(innerCell);
                innerCell.ReadOnly = true;

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
                    innerCell.ReadOnly = false;
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
                    comboCell.Items.AddRange(package.UidList.ToArray());
                    comboCell.Value = tmpObj.value;

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

            drawing = false;
        }

        private void gridProps_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (drawing) return;

                if ((e.RowIndex < 0 || e.RowIndex >= gridProps.RowCount) ||
                    (e.ColumnIndex < 0 || e.ColumnIndex >= gridProps.ColumnCount))
                {
                    return;
                }

                var cell = gridProps[e.ColumnIndex, e.RowIndex];
                if (selectedExport != null && cell != null && selectedPackage != null)
                {
                    IProperty iProp = selectedExport.Properties[e.RowIndex];
                    GpkBaseProperty bProp = (GpkBaseProperty) iProp;

                    switch (e.ColumnIndex)
                    {
                        case 0:
                            //Name
                            bProp.name = cell.Value.ToString();
                            break;
                        case 1:
                            //Type
                            logger.Info("not supported");
                            break;
                        case 2:
                            //Size. Autocomputed. Do nothing.
                            break;
                        case 3:
                            //arrayindex
                            bProp.arrayIndex = Convert.ToInt32(cell.Value);
                            break;
                        case 4:
                            //innertype
                            logger.Info("not supported");
                            break;
                        case 5:
                            //vvalue    
                            if (iProp is GpkArrayProperty)
                            {
                                GpkArrayProperty tmpArray = (GpkArrayProperty)iProp;
                                if (tmpArray.GetValueHex() != cell.Value.ToString())
                                {
                                    tmpArray.value = ((string)cell.Value).ToBytes();

                                }
                            }
                            else if (iProp is GpkStructProperty)
                            {
                                GpkStructProperty tmpStruct = (GpkStructProperty)iProp;
                                if (tmpStruct.GetValueHex() != cell.Value.ToString())
                                {
                                    tmpStruct.value = ((string)cell.Value).ToBytes();
                                }
                            }
                            else if (iProp is GpkNameProperty)
                            {
                                GpkNameProperty tmpName = (GpkNameProperty)iProp;
                                if (tmpName.value != cell.Value.ToString())
                                {
                                    tmpName.value = cell.Value.ToString();
                                }

                            }
                            else if (iProp is GpkObjectProperty)
                            {
                                GpkObjectProperty tmpObj = (GpkObjectProperty)iProp;
                                if (tmpObj.objectName != cell.Value.ToString())
                                {
                                    tmpObj.objectName = cell.Value.ToString();
                                }
                            }
                            else if (iProp is GpkByteProperty)
                            {
                                GpkByteProperty tmpByte = (GpkByteProperty)iProp;
                                if (tmpByte.value != cell.Value.ToString())
                                {
                                    if (tmpByte.size == 8)
                                    {
                                        tmpByte.nameValue = cell.Value.ToString();

                                    }
                                    else
                                    {
                                        tmpByte.byteValue = (byte)cell.Value;
                                    }
                                }
                            }
                            else if (iProp is GpkFloatProperty)
                            {
                                GpkFloatProperty tmpFloat = (GpkFloatProperty)iProp;
                                float fCell = Convert.ToSingle(cell.Value);

                                if (tmpFloat.value != fCell)
                                {
                                    tmpFloat.value = fCell;
                                }
                            }
                            else if (iProp is GpkIntProperty)
                            {
                                GpkIntProperty tmpInt = (GpkIntProperty)iProp;
                                int intCell = Convert.ToInt32(cell.Value);

                                if (tmpInt.value != intCell)
                                {
                                    tmpInt.value = intCell;
                                }
                            }
                            else if (iProp is GpkStringProperty)
                            {
                                GpkStringProperty tmpString = (GpkStringProperty)iProp;

                                if (tmpString.value != cell.Value.ToString())
                                {
                                    tmpString.value = cell.Value.ToString();
                                    tmpString.size = tmpString.length;
                                }
                            }
                            else if (iProp is GpkBoolProperty)
                            {
                                GpkBoolProperty tmpBool = (GpkBoolProperty)iProp;
                                if (tmpBool.value != (bool)cell.Value)
                                {
                                    tmpBool.value = (bool)cell.Value;
                                }
                            }
                            else
                            {
                                logger.Info("LOL");
                            }
                            break;
                    }

                    //update size
                    iProp.RecalculateSize();
                    gridProps[2, e.RowIndex].Value = bProp.size;

                }
            }
            catch (Exception ex)
            {

                logger.Fatal("Propertry Edit failed! " + ex.Message);
            }


        }

        private void gridProps_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (drawing) return;

                var row = gridProps.Rows[e.RowIndex];
                if (selectedExport != null && selectedPackage != null)
                {
                    GpkBaseProperty baseProp = new GpkBaseProperty(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), 0, Convert.ToInt32(row.Cells[3].Value));
                    IProperty iProp = null;

                    switch (baseProp.type)
                    {
                        case "StructProperty":
                        case "ArrayProperty":
                        case "ByteProperty":
                        case "NameProperty":
                        case "ObjectProperty":
                            logger.Info("not supported");
                            return;

                        case "BoolProperty":
                            GpkBoolProperty tmpBool = new GpkBoolProperty();
                            tmpBool.value = Convert.ToBoolean(row.Cells[5].Value);
                            iProp = tmpBool;
                            break;

                        case "IntProperty":
                            GpkIntProperty tmpInt = new GpkIntProperty();
                            tmpInt.value = Convert.ToInt32(row.Cells[5].Value);
                            iProp = tmpInt;
                            break;

                        case "FloatProperty":
                            GpkFloatProperty tmpFloat = new GpkFloatProperty();
                            tmpFloat.value = Convert.ToSingle(row.Cells[5].Value);
                            iProp = tmpFloat;
                            break;

                        case "StrProperty":
                            GpkStringProperty tmpStr = new GpkStringProperty();
                            tmpStr.value = (row.Cells[5].Value.ToString());
                            iProp = tmpStr;
                            break;

                        default:
                            throw new Exception(
                                string.Format("Unknown Property Type {0}, Prop_Name {1}", baseProp.type, baseProp.name));

                    }

                    iProp.RecalculateSize();
                    selectedExport.Properties.Add(iProp);
                }
            }
            catch (Exception ex)
            {

                logger.Fatal("Propertry Add failed! " + ex.Message);
            }


        }

        private void gridProps_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (drawing) return;
            logger.Info("not supported");

        }


        #endregion






    }
}


