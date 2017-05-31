using System.Windows.Forms;

namespace GPK_RePack.Forms
{
    partial class GUI
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.treeMain = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFilesizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAllPropertysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAllVolumeMultipliersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tESTBigBytePropExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigBytePropImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.boxInfo = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridProps = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.boxImagePreview = new System.Windows.Forms.PictureBox();
            this.boxCommands = new System.Windows.Forms.Panel();
            this.boxPropertyButtons = new System.Windows.Forms.GroupBox();
            this.btnPropClear = new System.Windows.Forms.Button();
            this.btnPropSave = new System.Windows.Forms.Button();
            this.boxGeneralButtons = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.boxDataButtons = new System.Windows.Forms.GroupBox();
            this.btnImageExport = new System.Windows.Forms.Button();
            this.btnImageImport = new System.Windows.Forms.Button();
            this.btnOggPreview = new System.Windows.Forms.Button();
            this.btnFakeOGG = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.btnImportOgg = new System.Windows.Forms.Button();
            this.btnExtractOGG = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.boxLog = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblFiller = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxImagePreview)).BeginInit();
            this.boxCommands.SuspendLayout();
            this.boxPropertyButtons.SuspendLayout();
            this.boxGeneralButtons.SuspendLayout();
            this.boxDataButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.AllowDrop = true;
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.FullRowSelect = true;
            this.treeMain.HideSelection = false;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(306, 485);
            this.treeMain.TabIndex = 1;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.treeMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeMain_DragDrop);
            this.treeMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeMain_DragEnter);
            this.treeMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.miscToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1103, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.replaceSaveToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.refreshViewToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(238, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.saveToolStripMenuItem.Text = "Save (Rebuild Mode)";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // replaceSaveToolStripMenuItem
            // 
            this.replaceSaveToolStripMenuItem.Name = "replaceSaveToolStripMenuItem";
            this.replaceSaveToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.replaceSaveToolStripMenuItem.Text = "Save patched (not recommend)";
            this.replaceSaveToolStripMenuItem.Click += new System.EventHandler(this.replaceSaveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(238, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(238, 6);
            // 
            // refreshViewToolStripMenuItem
            // 
            this.refreshViewToolStripMenuItem.Name = "refreshViewToolStripMenuItem";
            this.refreshViewToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.refreshViewToolStripMenuItem.Text = "Refresh View";
            this.refreshViewToolStripMenuItem.Click += new System.EventHandler(this.refreshViewToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // miscToolStripMenuItem
            // 
            this.miscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFilesizeToolStripMenuItem,
            this.setAllPropertysToolStripMenuItem,
            this.tESTBigBytePropExportToolStripMenuItem,
            this.bigBytePropImportToolStripMenuItem,
            this.addNameToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // setFilesizeToolStripMenuItem
            // 
            this.setFilesizeToolStripMenuItem.Name = "setFilesizeToolStripMenuItem";
            this.setFilesizeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.setFilesizeToolStripMenuItem.Text = "Set Filesize";
            this.setFilesizeToolStripMenuItem.Click += new System.EventHandler(this.setFilesizeToolStripMenuItem_Click);
            // 
            // setAllPropertysToolStripMenuItem
            // 
            this.setAllPropertysToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAllVolumeMultipliersToolStripMenuItem,
            this.customToolStripMenuItem});
            this.setAllPropertysToolStripMenuItem.Name = "setAllPropertysToolStripMenuItem";
            this.setAllPropertysToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.setAllPropertysToolStripMenuItem.Text = "Set all Propertys";
            // 
            // setAllVolumeMultipliersToolStripMenuItem
            // 
            this.setAllVolumeMultipliersToolStripMenuItem.Name = "setAllVolumeMultipliersToolStripMenuItem";
            this.setAllVolumeMultipliersToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.setAllVolumeMultipliersToolStripMenuItem.Text = "Set all VolumeMultipliers";
            this.setAllVolumeMultipliersToolStripMenuItem.Click += new System.EventHandler(this.setAllVolumeMultipliersToolStripMenuItem_Click);
            // 
            // customToolStripMenuItem
            // 
            this.customToolStripMenuItem.Name = "customToolStripMenuItem";
            this.customToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.customToolStripMenuItem.Text = "Custom";
            this.customToolStripMenuItem.Click += new System.EventHandler(this.customToolStripMenuItem_Click);
            // 
            // tESTBigBytePropExportToolStripMenuItem
            // 
            this.tESTBigBytePropExportToolStripMenuItem.Name = "tESTBigBytePropExportToolStripMenuItem";
            this.tESTBigBytePropExportToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.tESTBigBytePropExportToolStripMenuItem.Text = "Big ByteProp Export";
            this.tESTBigBytePropExportToolStripMenuItem.Click += new System.EventHandler(this.BigBytePropExport_Click);
            // 
            // bigBytePropImportToolStripMenuItem
            // 
            this.bigBytePropImportToolStripMenuItem.Name = "bigBytePropImportToolStripMenuItem";
            this.bigBytePropImportToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.bigBytePropImportToolStripMenuItem.Text = "Big ByteProp Import";
            this.bigBytePropImportToolStripMenuItem.Click += new System.EventHandler(this.BigBytePropImport_Click);
            // 
            // addNameToolStripMenuItem
            // 
            this.addNameToolStripMenuItem.Name = "addNameToolStripMenuItem";
            this.addNameToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.addNameToolStripMenuItem.Text = "Add Name";
            this.addNameToolStripMenuItem.Click += new System.EventHandler(this.addNameToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(793, 394);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.boxInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(785, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // boxInfo
            // 
            this.boxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxInfo.Location = new System.Drawing.Point(3, 3);
            this.boxInfo.Multiline = true;
            this.boxInfo.Name = "boxInfo";
            this.boxInfo.ReadOnly = true;
            this.boxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.boxInfo.Size = new System.Drawing.Size(779, 362);
            this.boxInfo.TabIndex = 5;
            this.boxInfo.Text = resources.GetString("boxInfo.Text");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridProps);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(785, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Property Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridProps
            // 
            this.gridProps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridProps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.type,
            this.size,
            this.aIndex,
            this.iType,
            this.value});
            this.gridProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProps.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridProps.Enabled = false;
            this.gridProps.Location = new System.Drawing.Point(3, 3);
            this.gridProps.MultiSelect = false;
            this.gridProps.Name = "gridProps";
            this.gridProps.Size = new System.Drawing.Size(779, 362);
            this.gridProps.TabIndex = 0;
            this.gridProps.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridProps_DefaultValuesNeeded);
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // type
            // 
            this.type.HeaderText = "Type";
            this.type.Items.AddRange(new object[] {
            "ArrayProperty",
            "BoolProperty",
            "ByteProperty",
            "FloatProperty",
            "IntProperty",
            "NameProperty",
            "ObjectProperty",
            "StrProperty",
            "StructProperty"});
            this.type.Name = "type";
            // 
            // size
            // 
            this.size.HeaderText = "Size";
            this.size.Name = "size";
            this.size.ReadOnly = true;
            // 
            // aIndex
            // 
            this.aIndex.HeaderText = "ArrayIndex";
            this.aIndex.Name = "aIndex";
            // 
            // iType
            // 
            this.iType.HeaderText = "InnerType";
            this.iType.Name = "iType";
            // 
            // value
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.boxImagePreview);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(785, 368);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Texture Preview";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // boxImagePreview
            // 
            this.boxImagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxImagePreview.Location = new System.Drawing.Point(3, 3);
            this.boxImagePreview.Name = "boxImagePreview";
            this.boxImagePreview.Size = new System.Drawing.Size(779, 362);
            this.boxImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.boxImagePreview.TabIndex = 0;
            this.boxImagePreview.TabStop = false;
            // 
            // boxCommands
            // 
            this.boxCommands.Controls.Add(this.boxPropertyButtons);
            this.boxCommands.Controls.Add(this.boxGeneralButtons);
            this.boxCommands.Controls.Add(this.boxDataButtons);
            this.boxCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxCommands.Location = new System.Drawing.Point(0, 394);
            this.boxCommands.Name = "boxCommands";
            this.boxCommands.Size = new System.Drawing.Size(793, 91);
            this.boxCommands.TabIndex = 6;
            // 
            // boxPropertyButtons
            // 
            this.boxPropertyButtons.Controls.Add(this.btnPropClear);
            this.boxPropertyButtons.Controls.Add(this.btnPropSave);
            this.boxPropertyButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxPropertyButtons.Enabled = false;
            this.boxPropertyButtons.Location = new System.Drawing.Point(199, 0);
            this.boxPropertyButtons.Name = "boxPropertyButtons";
            this.boxPropertyButtons.Size = new System.Drawing.Size(146, 91);
            this.boxPropertyButtons.TabIndex = 6;
            this.boxPropertyButtons.TabStop = false;
            this.boxPropertyButtons.Text = "Property Commands";
            // 
            // btnPropClear
            // 
            this.btnPropClear.Location = new System.Drawing.Point(17, 58);
            this.btnPropClear.Name = "btnPropClear";
            this.btnPropClear.Size = new System.Drawing.Size(125, 24);
            this.btnPropClear.TabIndex = 5;
            this.btnPropClear.Text = "Clear Properties";
            this.btnPropClear.UseVisualStyleBackColor = true;
            this.btnPropClear.Click += new System.EventHandler(this.btnPropClear_Click);
            // 
            // btnPropSave
            // 
            this.btnPropSave.Location = new System.Drawing.Point(17, 28);
            this.btnPropSave.Name = "btnPropSave";
            this.btnPropSave.Size = new System.Drawing.Size(125, 24);
            this.btnPropSave.TabIndex = 4;
            this.btnPropSave.Text = "Save Properties";
            this.btnPropSave.UseVisualStyleBackColor = true;
            this.btnPropSave.Click += new System.EventHandler(this.btnPropSave_Click);
            // 
            // boxGeneralButtons
            // 
            this.boxGeneralButtons.Controls.Add(this.btnPaste);
            this.boxGeneralButtons.Controls.Add(this.btnCopy);
            this.boxGeneralButtons.Controls.Add(this.btnAdd);
            this.boxGeneralButtons.Controls.Add(this.btnDelete);
            this.boxGeneralButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.boxGeneralButtons.Enabled = false;
            this.boxGeneralButtons.Location = new System.Drawing.Point(0, 0);
            this.boxGeneralButtons.Name = "boxGeneralButtons";
            this.boxGeneralButtons.Size = new System.Drawing.Size(199, 91);
            this.boxGeneralButtons.TabIndex = 5;
            this.boxGeneralButtons.TabStop = false;
            this.boxGeneralButtons.Text = "General";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(98, 58);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(95, 24);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "Paste Object";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(6, 58);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(86, 24);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy Object";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(6, 28);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Object";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(98, 28);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(95, 24);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Remove Object";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // boxDataButtons
            // 
            this.boxDataButtons.Controls.Add(this.btnImageExport);
            this.boxDataButtons.Controls.Add(this.btnImageImport);
            this.boxDataButtons.Controls.Add(this.btnOggPreview);
            this.boxDataButtons.Controls.Add(this.btnFakeOGG);
            this.boxDataButtons.Controls.Add(this.btnDeleteData);
            this.boxDataButtons.Controls.Add(this.btnImportOgg);
            this.boxDataButtons.Controls.Add(this.btnExtractOGG);
            this.boxDataButtons.Controls.Add(this.btnReplace);
            this.boxDataButtons.Controls.Add(this.btnExport);
            this.boxDataButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.boxDataButtons.Enabled = false;
            this.boxDataButtons.Location = new System.Drawing.Point(345, 0);
            this.boxDataButtons.Name = "boxDataButtons";
            this.boxDataButtons.Size = new System.Drawing.Size(448, 91);
            this.boxDataButtons.TabIndex = 4;
            this.boxDataButtons.TabStop = false;
            this.boxDataButtons.Text = "Data Commands";
            // 
            // btnImageExport
            // 
            this.btnImageExport.Location = new System.Drawing.Point(370, 58);
            this.btnImageExport.Name = "btnImageExport";
            this.btnImageExport.Size = new System.Drawing.Size(75, 23);
            this.btnImageExport.TabIndex = 11;
            this.btnImageExport.Text = "Export DDS";
            this.btnImageExport.UseVisualStyleBackColor = true;
            this.btnImageExport.Click += new System.EventHandler(this.btnImageExport_Click);
            // 
            // btnImageImport
            // 
            this.btnImageImport.Location = new System.Drawing.Point(369, 29);
            this.btnImageImport.Name = "btnImageImport";
            this.btnImageImport.Size = new System.Drawing.Size(75, 23);
            this.btnImageImport.TabIndex = 10;
            this.btnImageImport.Text = "Import DDS";
            this.btnImageImport.UseVisualStyleBackColor = true;
            this.btnImageImport.Click += new System.EventHandler(this.btnImageImport_Click);
            // 
            // btnOggPreview
            // 
            this.btnOggPreview.Location = new System.Drawing.Point(282, 58);
            this.btnOggPreview.Name = "btnOggPreview";
            this.btnOggPreview.Size = new System.Drawing.Size(78, 24);
            this.btnOggPreview.TabIndex = 9;
            this.btnOggPreview.Text = "Ogg Preview";
            this.btnOggPreview.UseVisualStyleBackColor = true;
            this.btnOggPreview.Click += new System.EventHandler(this.btnOggPreview_Click);
            // 
            // btnFakeOGG
            // 
            this.btnFakeOGG.Location = new System.Drawing.Point(176, 58);
            this.btnFakeOGG.Name = "btnFakeOGG";
            this.btnFakeOGG.Size = new System.Drawing.Size(100, 24);
            this.btnFakeOGG.TabIndex = 8;
            this.btnFakeOGG.Text = "Insert Emtpy OGG";
            this.btnFakeOGG.UseVisualStyleBackColor = true;
            this.btnFakeOGG.Click += new System.EventHandler(this.btnFakeOGG_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Location = new System.Drawing.Point(233, 28);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteData.TabIndex = 7;
            this.btnDeleteData.Text = "Delete Data";
            this.btnDeleteData.UseVisualStyleBackColor = true;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnImportOgg
            // 
            this.btnImportOgg.Location = new System.Drawing.Point(94, 58);
            this.btnImportOgg.Name = "btnImportOgg";
            this.btnImportOgg.Size = new System.Drawing.Size(76, 24);
            this.btnImportOgg.TabIndex = 6;
            this.btnImportOgg.Text = "Import OGG";
            this.btnImportOgg.UseVisualStyleBackColor = true;
            this.btnImportOgg.Click += new System.EventHandler(this.btnImportOgg_Click);
            // 
            // btnExtractOGG
            // 
            this.btnExtractOGG.Location = new System.Drawing.Point(6, 58);
            this.btnExtractOGG.Name = "btnExtractOGG";
            this.btnExtractOGG.Size = new System.Drawing.Size(82, 24);
            this.btnExtractOGG.TabIndex = 5;
            this.btnExtractOGG.Text = "Export OGG";
            this.btnExtractOGG.UseVisualStyleBackColor = true;
            this.btnExtractOGG.Click += new System.EventHandler(this.btnExtractOGG_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(114, 28);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(113, 24);
            this.btnReplace.TabIndex = 2;
            this.btnReplace.Text = "Replace Raw Data";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(6, 28);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(102, 24);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export Raw Data";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // boxLog
            // 
            this.boxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxLog.Location = new System.Drawing.Point(0, 509);
            this.boxLog.Multiline = true;
            this.boxLog.Name = "boxLog";
            this.boxLog.ReadOnly = true;
            this.boxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.boxLog.Size = new System.Drawing.Size(1103, 145);
            this.boxLog.TabIndex = 5;
            this.boxLog.TextChanged += new System.EventHandler(this.boxLog_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.boxCommands);
            this.splitContainer1.Size = new System.Drawing.Size(1103, 485);
            this.splitContainer1.SplitterDistance = 306;
            this.splitContainer1.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFiller,
            this.lblStatus,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 654);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(1103, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblFiller
            // 
            this.lblFiller.Name = "lblFiller";
            this.lblFiller.Size = new System.Drawing.Size(986, 17);
            this.lblFiller.Spring = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 676);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.boxLog);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GUI";
            this.Text = "Terahelper 0.10 - by GoneUp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeMain_DragDrop);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxImagePreview)).EndInit();
            this.boxCommands.ResumeLayout(false);
            this.boxPropertyButtons.ResumeLayout(false);
            this.boxGeneralButtons.ResumeLayout(false);
            this.boxDataButtons.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox boxDataButtons;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox boxLog;
        private System.Windows.Forms.TextBox boxInfo;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ToolStripMenuItem replaceSaveToolStripMenuItem;
        private System.Windows.Forms.Panel boxCommands;
        private System.Windows.Forms.GroupBox boxGeneralButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExtractOGG;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.Button btnImportOgg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button btnFakeOGG;
        private System.Windows.Forms.ToolStripMenuItem miscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFilesizeToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gridProps;
        private SplitContainer splitContainer1;
        private GroupBox boxPropertyButtons;
        private Button btnPropClear;
        private Button btnPropSave;
        private DataGridViewTextBoxColumn name;
        private DataGridViewComboBoxColumn type;
        private DataGridViewTextBoxColumn size;
        private DataGridViewTextBoxColumn aIndex;
        private DataGridViewTextBoxColumn iType;
        private DataGridViewTextBoxColumn value;
        private Button btnOggPreview;
        private ToolStripMenuItem setAllPropertysToolStripMenuItem;
        private ToolStripMenuItem setAllVolumeMultipliersToolStripMenuItem;
        private ToolStripMenuItem customToolStripMenuItem;
        private ToolStripMenuItem tESTBigBytePropExportToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar ProgressBar;
        private ToolStripStatusLabel lblFiller;
        private ToolStripMenuItem bigBytePropImportToolStripMenuItem;
        private ToolStripMenuItem addNameToolStripMenuItem;
        private TabPage tabPage3;
        private PictureBox boxImagePreview;
        private Button btnImageExport;
        private Button btnImageImport;
    }
}

