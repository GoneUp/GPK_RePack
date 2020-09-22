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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.treeMain = new System.Windows.Forms.TreeView();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRawDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importOGGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDDSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRawDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportOGGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDDSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.previewOGGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePaddingStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.patchObjectMapperforSelectedPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.savePatchedCompositeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAddedCompositeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compositeGPKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datEncryptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpCompositeTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFilesizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAllPropertysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAllVolumeMultipliersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tESTBigBytePropExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigBytePropImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpHeadersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingActiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.searchForObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.boxInfo = new System.Windows.Forms.TextBox();
            this.boxDataButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnExportDDS = new System.Windows.Forms.Button();
            this.btnFakeOGG = new System.Windows.Forms.Button();
            this.btnExportRaw = new System.Windows.Forms.Button();
            this.btnImportDDS = new System.Windows.Forms.Button();
            this.btnImportRaw = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.btnExportOgg = new System.Windows.Forms.Button();
            this.btnImportOgg = new System.Windows.Forms.Button();
            this.btnPreviewOgg = new System.Windows.Forms.Button();
            this.tabPropertys = new System.Windows.Forms.TabPage();
            this.gridProps = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.boxPropertyButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnPropSave = new System.Windows.Forms.Button();
            this.btnPropClear = new System.Windows.Forms.Button();
            this.btnExportProps = new System.Windows.Forms.Button();
            this.tabTexturePreview = new System.Windows.Forms.TabPage();
            this.boxImagePreview = new System.Windows.Forms.PictureBox();
            this.boxGeneralButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.splitContainerTreeInfo = new System.Windows.Forms.SplitContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblFiller = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainerLog_InfoTree = new System.Windows.Forms.SplitContainer();
            this.boxLog = new System.Windows.Forms.RichTextBox();
            this.minimizeGPKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.treeContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabInfo.SuspendLayout();
            this.boxDataButtons.SuspendLayout();
            this.tabPropertys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).BeginInit();
            this.boxPropertyButtons.SuspendLayout();
            this.tabTexturePreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxImagePreview)).BeginInit();
            this.boxGeneralButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeInfo)).BeginInit();
            this.splitContainerTreeInfo.Panel1.SuspendLayout();
            this.splitContainerTreeInfo.Panel2.SuspendLayout();
            this.splitContainerTreeInfo.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLog_InfoTree)).BeginInit();
            this.splitContainerLog_InfoTree.Panel1.SuspendLayout();
            this.splitContainerLog_InfoTree.Panel2.SuspendLayout();
            this.splitContainerLog_InfoTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.AllowDrop = true;
            this.treeMain.ContextMenuStrip = this.treeContextMenu;
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.FullRowSelect = true;
            this.treeMain.HideSelection = false;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(321, 449);
            this.treeMain.TabIndex = 1;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.treeMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseClick);
            this.treeMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeMain_DragDrop);
            this.treeMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeMain_DragEnter);
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator5,
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.insertStripMenuItem,
            this.toolStripSeparator6,
            this.previewOGGToolStripMenuItem});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(161, 208);
            this.treeContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.treeContextMenu_ItemClicked);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importRawDataToolStripMenuItem,
            this.importOGGToolStripMenuItem,
            this.importDDSToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.treeContextMenu_ItemClicked);
            // 
            // importRawDataToolStripMenuItem
            // 
            this.importRawDataToolStripMenuItem.Name = "importRawDataToolStripMenuItem";
            this.importRawDataToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.importRawDataToolStripMenuItem.Text = "Raw Data";
            // 
            // importOGGToolStripMenuItem
            // 
            this.importOGGToolStripMenuItem.Name = "importOGGToolStripMenuItem";
            this.importOGGToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.importOGGToolStripMenuItem.Text = "OGG";
            // 
            // importDDSToolStripMenuItem
            // 
            this.importDDSToolStripMenuItem.Name = "importDDSToolStripMenuItem";
            this.importDDSToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.importDDSToolStripMenuItem.Text = "DDS";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportRawDataToolStripMenuItem,
            this.exportOGGToolStripMenuItem,
            this.exportDDSToolStripMenuItem,
            this.exportPackageToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.treeContextMenu_ItemClicked);
            // 
            // exportRawDataToolStripMenuItem
            // 
            this.exportRawDataToolStripMenuItem.Name = "exportRawDataToolStripMenuItem";
            this.exportRawDataToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.exportRawDataToolStripMenuItem.Text = "Raw Data";
            // 
            // exportOGGToolStripMenuItem
            // 
            this.exportOGGToolStripMenuItem.Name = "exportOGGToolStripMenuItem";
            this.exportOGGToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.exportOGGToolStripMenuItem.Text = "OGG";
            // 
            // exportDDSToolStripMenuItem
            // 
            this.exportDDSToolStripMenuItem.Name = "exportDDSToolStripMenuItem";
            this.exportDDSToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.exportDDSToolStripMenuItem.Text = "DDS";
            // 
            // exportPackageToolStripMenuItem
            // 
            this.exportPackageToolStripMenuItem.Name = "exportPackageToolStripMenuItem";
            this.exportPackageToolStripMenuItem.Size = new System.Drawing.Size(136, 24);
            this.exportPackageToolStripMenuItem.Text = "Package";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(157, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Enabled = false;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // insertStripMenuItem
            // 
            this.insertStripMenuItem.Name = "insertStripMenuItem";
            this.insertStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.insertStripMenuItem.Text = "Insert";
            this.insertStripMenuItem.Click += new System.EventHandler(this.insertStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(157, 6);
            // 
            // previewOGGToolStripMenuItem
            // 
            this.previewOGGToolStripMenuItem.Name = "previewOGGToolStripMenuItem";
            this.previewOGGToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.previewOGGToolStripMenuItem.Text = "Preview OGG";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.compositeGPKToolStripMenuItem,
            this.miscToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1163, 27);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.savePaddingStripMenuItem,
            this.replaceSaveToolStripMenuItem,
            this.toolStripSeparator7,
            this.toolStripMenuItem1,
            this.patchObjectMapperforSelectedPackageToolStripMenuItem,
            this.toolStripSeparator8,
            this.toolStripMenuItem2,
            this.savePatchedCompositeStripMenuItem,
            this.saveAddedCompositeToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.refreshViewToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(52, 23);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(411, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.saveToolStripMenuItem.Text = "Save (Rebuild Mode)";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // savePaddingStripMenuItem
            // 
            this.savePaddingStripMenuItem.Name = "savePaddingStripMenuItem";
            this.savePaddingStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.savePaddingStripMenuItem.Text = "Save (Rebuild Mode, with Padding)";
            this.savePaddingStripMenuItem.Click += new System.EventHandler(this.savepaddingStripMenuItem_Click);
            // 
            // replaceSaveToolStripMenuItem
            // 
            this.replaceSaveToolStripMenuItem.Name = "replaceSaveToolStripMenuItem";
            this.replaceSaveToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.replaceSaveToolStripMenuItem.Text = "Save patched (not recommend)";
            this.replaceSaveToolStripMenuItem.Click += new System.EventHandler(this.replaceSaveToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(411, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(414, 24);
            this.toolStripMenuItem1.Text = "ObjectMapper.dat Modification";
            // 
            // patchObjectMapperforSelectedPackageToolStripMenuItem
            // 
            this.patchObjectMapperforSelectedPackageToolStripMenuItem.Name = "patchObjectMapperforSelectedPackageToolStripMenuItem";
            this.patchObjectMapperforSelectedPackageToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.patchObjectMapperforSelectedPackageToolStripMenuItem.Text = "Patch ObjectMapper (for selected package)";
            this.patchObjectMapperforSelectedPackageToolStripMenuItem.Click += new System.EventHandler(this.patchObjectMapperforSelectedPackageToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(411, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(414, 24);
            this.toolStripMenuItem2.Text = "CompositePackageMapper.dat Modification";
            // 
            // savePatchedCompositeStripMenuItem
            // 
            this.savePatchedCompositeStripMenuItem.Name = "savePatchedCompositeStripMenuItem";
            this.savePatchedCompositeStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.savePatchedCompositeStripMenuItem.Text = "Save and patch orginal Composite (not recommended)";
            this.savePatchedCompositeStripMenuItem.Click += new System.EventHandler(this.btnSavePatchedComposite_Click);
            // 
            // saveAddedCompositeToolStripMenuItem
            // 
            this.saveAddedCompositeToolStripMenuItem.Name = "saveAddedCompositeToolStripMenuItem";
            this.saveAddedCompositeToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.saveAddedCompositeToolStripMenuItem.Text = "Save and add new Composite GPK (not recommended)";
            this.saveAddedCompositeToolStripMenuItem.Click += new System.EventHandler(this.saveAddedCompositeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(411, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(411, 6);
            // 
            // refreshViewToolStripMenuItem
            // 
            this.refreshViewToolStripMenuItem.Name = "refreshViewToolStripMenuItem";
            this.refreshViewToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.refreshViewToolStripMenuItem.Text = "Refresh View";
            this.refreshViewToolStripMenuItem.Click += new System.EventHandler(this.refreshViewToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(414, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // compositeGPKToolStripMenuItem
            // 
            this.compositeGPKToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decryptionToolStripMenuItem,
            this.datEncryptionToolStripMenuItem,
            this.loadMappingToolStripMenuItem,
            this.writeMappingsToolStripMenuItem,
            this.toolStripSeparator10,
            this.minimizeGPKToolStripMenuItem,
            this.toolStripSeparator9,
            this.dumpCompositeTexturesToolStripMenuItem,
            this.dumpIconsToolStripMenuItem});
            this.compositeGPKToolStripMenuItem.Name = "compositeGPKToolStripMenuItem";
            this.compositeGPKToolStripMenuItem.Size = new System.Drawing.Size(117, 23);
            this.compositeGPKToolStripMenuItem.Text = "Composite GPK";
            // 
            // decryptionToolStripMenuItem
            // 
            this.decryptionToolStripMenuItem.Name = "decryptionToolStripMenuItem";
            this.decryptionToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.decryptionToolStripMenuItem.Text = ".dat Decryption";
            this.decryptionToolStripMenuItem.Click += new System.EventHandler(this.decryptionToolStripMenuItem_Click);
            // 
            // datEncryptionToolStripMenuItem
            // 
            this.datEncryptionToolStripMenuItem.Name = "datEncryptionToolStripMenuItem";
            this.datEncryptionToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.datEncryptionToolStripMenuItem.Text = ".dat Encryption";
            this.datEncryptionToolStripMenuItem.Click += new System.EventHandler(this.datEncryptionToolStripMenuItem_Click);
            // 
            // loadMappingToolStripMenuItem
            // 
            this.loadMappingToolStripMenuItem.Name = "loadMappingToolStripMenuItem";
            this.loadMappingToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.loadMappingToolStripMenuItem.Text = "Load/Show Mapping";
            this.loadMappingToolStripMenuItem.Click += new System.EventHandler(this.loadMappingToolStripMenuItem_Click);
            // 
            // writeMappingsToolStripMenuItem
            // 
            this.writeMappingsToolStripMenuItem.Name = "writeMappingsToolStripMenuItem";
            this.writeMappingsToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.writeMappingsToolStripMenuItem.Text = "Write Mappings";
            this.writeMappingsToolStripMenuItem.Click += new System.EventHandler(this.writeMappingsToolStripMenuItem_Click);
            // 
            // dumpCompositeTexturesToolStripMenuItem
            // 
            this.dumpCompositeTexturesToolStripMenuItem.Name = "dumpCompositeTexturesToolStripMenuItem";
            this.dumpCompositeTexturesToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.dumpCompositeTexturesToolStripMenuItem.Text = "Dump CompositeTextures";
            this.dumpCompositeTexturesToolStripMenuItem.Click += new System.EventHandler(this.dumpCompositeTexturesToolStripMenuItem_Click);
            // 
            // dumpIconsToolStripMenuItem
            // 
            this.dumpIconsToolStripMenuItem.Name = "dumpIconsToolStripMenuItem";
            this.dumpIconsToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.dumpIconsToolStripMenuItem.Text = "Dump icons";
            this.dumpIconsToolStripMenuItem.Click += new System.EventHandler(this.dumpIconsToolStripMenuItem_Click);
            // 
            // miscToolStripMenuItem
            // 
            this.miscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFilesizeToolStripMenuItem,
            this.setAllPropertysToolStripMenuItem,
            this.tESTBigBytePropExportToolStripMenuItem,
            this.bigBytePropImportToolStripMenuItem,
            this.addNameToolStripMenuItem,
            this.dumpHeadersMenuItem,
            this.loggingActiveMenuItem,
            this.toolStripSeparator4,
            this.searchForObjectToolStripMenuItem,
            this.nextToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(49, 23);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // setFilesizeToolStripMenuItem
            // 
            this.setFilesizeToolStripMenuItem.Name = "setFilesizeToolStripMenuItem";
            this.setFilesizeToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.setFilesizeToolStripMenuItem.Text = "Set Filesize";
            this.setFilesizeToolStripMenuItem.Click += new System.EventHandler(this.setFilesizeToolStripMenuItem_Click);
            // 
            // setAllPropertysToolStripMenuItem
            // 
            this.setAllPropertysToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAllVolumeMultipliersToolStripMenuItem,
            this.customToolStripMenuItem});
            this.setAllPropertysToolStripMenuItem.Name = "setAllPropertysToolStripMenuItem";
            this.setAllPropertysToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.setAllPropertysToolStripMenuItem.Text = "Set all Properties";
            // 
            // setAllVolumeMultipliersToolStripMenuItem
            // 
            this.setAllVolumeMultipliersToolStripMenuItem.Name = "setAllVolumeMultipliersToolStripMenuItem";
            this.setAllVolumeMultipliersToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.setAllVolumeMultipliersToolStripMenuItem.Text = "Set all VolumeMultipliers";
            this.setAllVolumeMultipliersToolStripMenuItem.Click += new System.EventHandler(this.setAllVolumeMultipliersToolStripMenuItem_Click);
            // 
            // customToolStripMenuItem
            // 
            this.customToolStripMenuItem.Name = "customToolStripMenuItem";
            this.customToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.customToolStripMenuItem.Text = "Custom";
            this.customToolStripMenuItem.Click += new System.EventHandler(this.customToolStripMenuItem_Click);
            // 
            // tESTBigBytePropExportToolStripMenuItem
            // 
            this.tESTBigBytePropExportToolStripMenuItem.Name = "tESTBigBytePropExportToolStripMenuItem";
            this.tESTBigBytePropExportToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.tESTBigBytePropExportToolStripMenuItem.Text = "Big ByteProp Export";
            this.tESTBigBytePropExportToolStripMenuItem.Click += new System.EventHandler(this.BigBytePropExport_Click);
            // 
            // bigBytePropImportToolStripMenuItem
            // 
            this.bigBytePropImportToolStripMenuItem.Name = "bigBytePropImportToolStripMenuItem";
            this.bigBytePropImportToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.bigBytePropImportToolStripMenuItem.Text = "Big ByteProp Import";
            this.bigBytePropImportToolStripMenuItem.Click += new System.EventHandler(this.BigBytePropImport_Click);
            // 
            // addNameToolStripMenuItem
            // 
            this.addNameToolStripMenuItem.Name = "addNameToolStripMenuItem";
            this.addNameToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.addNameToolStripMenuItem.Text = "Add Name";
            this.addNameToolStripMenuItem.Click += new System.EventHandler(this.addNameToolStripMenuItem_Click);
            // 
            // dumpHeadersMenuItem
            // 
            this.dumpHeadersMenuItem.Name = "dumpHeadersMenuItem";
            this.dumpHeadersMenuItem.Size = new System.Drawing.Size(236, 24);
            this.dumpHeadersMenuItem.Text = "Dump GPK Headers";
            this.dumpHeadersMenuItem.Click += new System.EventHandler(this.dumpHeadersMenuItem_Click);
            // 
            // loggingActiveMenuItem
            // 
            this.loggingActiveMenuItem.Checked = true;
            this.loggingActiveMenuItem.CheckOnClick = true;
            this.loggingActiveMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loggingActiveMenuItem.Name = "loggingActiveMenuItem";
            this.loggingActiveMenuItem.Size = new System.Drawing.Size(236, 24);
            this.loggingActiveMenuItem.Text = "Textbox Logging active";
            this.loggingActiveMenuItem.CheckedChanged += new System.EventHandler(this.loggingActiveMenuItem_CheckedChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(233, 6);
            // 
            // searchForObjectToolStripMenuItem
            // 
            this.searchForObjectToolStripMenuItem.Name = "searchForObjectToolStripMenuItem";
            this.searchForObjectToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.searchForObjectToolStripMenuItem.Text = "Search for object (CTRL-F)";
            this.searchForObjectToolStripMenuItem.Click += new System.EventHandler(this.searchForObjectToolStripMenuItem_Click);
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.nextToolStripMenuItem.Text = "Next result (F3)";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabInfo);
            this.tabControl.Controls.Add(this.tabPropertys);
            this.tabControl.Controls.Add(this.tabTexturePreview);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(838, 521);
            this.tabControl.TabIndex = 3;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabInfo
            // 
            this.tabInfo.Controls.Add(this.boxInfo);
            this.tabInfo.Controls.Add(this.boxDataButtons);
            this.tabInfo.Location = new System.Drawing.Point(4, 22);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabInfo.Size = new System.Drawing.Size(830, 495);
            this.tabInfo.TabIndex = 0;
            this.tabInfo.Text = "Info";
            this.tabInfo.UseVisualStyleBackColor = true;
            // 
            // boxInfo
            // 
            this.boxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxInfo.Location = new System.Drawing.Point(3, 3);
            this.boxInfo.Multiline = true;
            this.boxInfo.Name = "boxInfo";
            this.boxInfo.ReadOnly = true;
            this.boxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.boxInfo.Size = new System.Drawing.Size(824, 381);
            this.boxInfo.TabIndex = 5;
            this.boxInfo.Text = resources.GetString("boxInfo.Text");
            // 
            // boxDataButtons
            // 
            this.boxDataButtons.AutoSize = true;
            this.boxDataButtons.ColumnCount = 3;
            this.boxDataButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.boxDataButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.boxDataButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.boxDataButtons.Controls.Add(this.btnExportDDS, 2, 0);
            this.boxDataButtons.Controls.Add(this.btnFakeOGG, 1, 2);
            this.boxDataButtons.Controls.Add(this.btnExportRaw, 0, 0);
            this.boxDataButtons.Controls.Add(this.btnImportDDS, 2, 1);
            this.boxDataButtons.Controls.Add(this.btnImportRaw, 0, 1);
            this.boxDataButtons.Controls.Add(this.btnDeleteData, 0, 2);
            this.boxDataButtons.Controls.Add(this.btnExportOgg, 1, 0);
            this.boxDataButtons.Controls.Add(this.btnImportOgg, 1, 1);
            this.boxDataButtons.Controls.Add(this.btnPreviewOgg, 2, 2);
            this.boxDataButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxDataButtons.Enabled = false;
            this.boxDataButtons.Location = new System.Drawing.Point(3, 384);
            this.boxDataButtons.Name = "boxDataButtons";
            this.boxDataButtons.RowCount = 3;
            this.boxDataButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.boxDataButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.boxDataButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.boxDataButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.boxDataButtons.Size = new System.Drawing.Size(824, 108);
            this.boxDataButtons.TabIndex = 6;
            // 
            // btnExportDDS
            // 
            this.btnExportDDS.AutoSize = true;
            this.btnExportDDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportDDS.Location = new System.Drawing.Point(551, 3);
            this.btnExportDDS.Name = "btnExportDDS";
            this.btnExportDDS.Size = new System.Drawing.Size(270, 30);
            this.btnExportDDS.TabIndex = 11;
            this.btnExportDDS.Text = "Export DDS";
            this.btnExportDDS.UseVisualStyleBackColor = true;
            this.btnExportDDS.Click += new System.EventHandler(this.btnImageExport_Click);
            // 
            // btnFakeOGG
            // 
            this.btnFakeOGG.AutoSize = true;
            this.btnFakeOGG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFakeOGG.Location = new System.Drawing.Point(277, 75);
            this.btnFakeOGG.Name = "btnFakeOGG";
            this.btnFakeOGG.Size = new System.Drawing.Size(268, 30);
            this.btnFakeOGG.TabIndex = 8;
            this.btnFakeOGG.Text = "Import Emtpy OGG";
            this.btnFakeOGG.UseVisualStyleBackColor = true;
            this.btnFakeOGG.Click += new System.EventHandler(this.btnFakeOGG_Click);
            // 
            // btnExportRaw
            // 
            this.btnExportRaw.AutoSize = true;
            this.btnExportRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportRaw.Location = new System.Drawing.Point(3, 3);
            this.btnExportRaw.Name = "btnExportRaw";
            this.btnExportRaw.Size = new System.Drawing.Size(268, 30);
            this.btnExportRaw.TabIndex = 1;
            this.btnExportRaw.Text = "Export Raw Data";
            this.btnExportRaw.UseVisualStyleBackColor = true;
            this.btnExportRaw.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImportDDS
            // 
            this.btnImportDDS.AutoSize = true;
            this.btnImportDDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportDDS.Location = new System.Drawing.Point(551, 39);
            this.btnImportDDS.Name = "btnImportDDS";
            this.btnImportDDS.Size = new System.Drawing.Size(270, 30);
            this.btnImportDDS.TabIndex = 10;
            this.btnImportDDS.Text = "Import DDS";
            this.btnImportDDS.UseVisualStyleBackColor = true;
            this.btnImportDDS.Click += new System.EventHandler(this.btnImageImport_Click);
            // 
            // btnImportRaw
            // 
            this.btnImportRaw.AutoSize = true;
            this.btnImportRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportRaw.Location = new System.Drawing.Point(3, 39);
            this.btnImportRaw.Name = "btnImportRaw";
            this.btnImportRaw.Size = new System.Drawing.Size(268, 30);
            this.btnImportRaw.TabIndex = 2;
            this.btnImportRaw.Text = "Import Raw Data";
            this.btnImportRaw.UseVisualStyleBackColor = true;
            this.btnImportRaw.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.AutoSize = true;
            this.btnDeleteData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteData.Location = new System.Drawing.Point(3, 75);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(268, 30);
            this.btnDeleteData.TabIndex = 7;
            this.btnDeleteData.Text = "Delete Data";
            this.btnDeleteData.UseVisualStyleBackColor = true;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnExportOgg
            // 
            this.btnExportOgg.AutoSize = true;
            this.btnExportOgg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportOgg.Location = new System.Drawing.Point(277, 3);
            this.btnExportOgg.Name = "btnExportOgg";
            this.btnExportOgg.Size = new System.Drawing.Size(268, 30);
            this.btnExportOgg.TabIndex = 5;
            this.btnExportOgg.Text = "Export OGG";
            this.btnExportOgg.UseVisualStyleBackColor = true;
            this.btnExportOgg.Click += new System.EventHandler(this.btnExtractOGG_Click);
            // 
            // btnImportOgg
            // 
            this.btnImportOgg.AutoSize = true;
            this.btnImportOgg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportOgg.Location = new System.Drawing.Point(277, 39);
            this.btnImportOgg.Name = "btnImportOgg";
            this.btnImportOgg.Size = new System.Drawing.Size(268, 30);
            this.btnImportOgg.TabIndex = 6;
            this.btnImportOgg.Text = "Import OGG";
            this.btnImportOgg.UseVisualStyleBackColor = true;
            this.btnImportOgg.Click += new System.EventHandler(this.btnImportOgg_Click);
            // 
            // btnPreviewOgg
            // 
            this.btnPreviewOgg.AutoSize = true;
            this.btnPreviewOgg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPreviewOgg.Location = new System.Drawing.Point(551, 75);
            this.btnPreviewOgg.Name = "btnPreviewOgg";
            this.btnPreviewOgg.Size = new System.Drawing.Size(270, 30);
            this.btnPreviewOgg.TabIndex = 9;
            this.btnPreviewOgg.Text = "Preview OGG";
            this.btnPreviewOgg.UseVisualStyleBackColor = true;
            this.btnPreviewOgg.Click += new System.EventHandler(this.btnOggPreview_Click);
            // 
            // tabPropertys
            // 
            this.tabPropertys.Controls.Add(this.gridProps);
            this.tabPropertys.Controls.Add(this.boxPropertyButtons);
            this.tabPropertys.Location = new System.Drawing.Point(4, 22);
            this.tabPropertys.Name = "tabPropertys";
            this.tabPropertys.Padding = new System.Windows.Forms.Padding(3);
            this.tabPropertys.Size = new System.Drawing.Size(830, 495);
            this.tabPropertys.TabIndex = 1;
            this.tabPropertys.Text = "Property Details";
            this.tabPropertys.UseVisualStyleBackColor = true;
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
            this.gridProps.Size = new System.Drawing.Size(824, 457);
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
            // boxPropertyButtons
            // 
            this.boxPropertyButtons.ColumnCount = 3;
            this.boxPropertyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.boxPropertyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.boxPropertyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.boxPropertyButtons.Controls.Add(this.btnPropSave, 0, 0);
            this.boxPropertyButtons.Controls.Add(this.btnPropClear, 2, 0);
            this.boxPropertyButtons.Controls.Add(this.btnExportProps, 1, 0);
            this.boxPropertyButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxPropertyButtons.Enabled = false;
            this.boxPropertyButtons.Location = new System.Drawing.Point(3, 460);
            this.boxPropertyButtons.Name = "boxPropertyButtons";
            this.boxPropertyButtons.RowCount = 1;
            this.boxPropertyButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.boxPropertyButtons.Size = new System.Drawing.Size(824, 32);
            this.boxPropertyButtons.TabIndex = 6;
            // 
            // btnPropSave
            // 
            this.btnPropSave.AutoSize = true;
            this.btnPropSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPropSave.Location = new System.Drawing.Point(3, 3);
            this.btnPropSave.Name = "btnPropSave";
            this.btnPropSave.Size = new System.Drawing.Size(268, 26);
            this.btnPropSave.TabIndex = 4;
            this.btnPropSave.Text = "Save Properties";
            this.btnPropSave.UseVisualStyleBackColor = true;
            this.btnPropSave.Click += new System.EventHandler(this.btnPropSave_Click);
            // 
            // btnPropClear
            // 
            this.btnPropClear.AutoSize = true;
            this.btnPropClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPropClear.Location = new System.Drawing.Point(551, 3);
            this.btnPropClear.Name = "btnPropClear";
            this.btnPropClear.Size = new System.Drawing.Size(270, 26);
            this.btnPropClear.TabIndex = 5;
            this.btnPropClear.Text = "Clear Properties";
            this.btnPropClear.UseVisualStyleBackColor = true;
            this.btnPropClear.Click += new System.EventHandler(this.btnPropClear_Click);
            // 
            // btnExportProps
            // 
            this.btnExportProps.AutoSize = true;
            this.btnExportProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportProps.Location = new System.Drawing.Point(277, 3);
            this.btnExportProps.Name = "btnExportProps";
            this.btnExportProps.Size = new System.Drawing.Size(268, 26);
            this.btnExportProps.TabIndex = 6;
            this.btnExportProps.Text = "Export to File";
            this.btnExportProps.UseVisualStyleBackColor = true;
            this.btnExportProps.Click += new System.EventHandler(this.btnExportProps_Click);
            // 
            // tabTexturePreview
            // 
            this.tabTexturePreview.Controls.Add(this.boxImagePreview);
            this.tabTexturePreview.Location = new System.Drawing.Point(4, 22);
            this.tabTexturePreview.Name = "tabTexturePreview";
            this.tabTexturePreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabTexturePreview.Size = new System.Drawing.Size(830, 495);
            this.tabTexturePreview.TabIndex = 2;
            this.tabTexturePreview.Text = "Texture Preview";
            this.tabTexturePreview.UseVisualStyleBackColor = true;
            // 
            // boxImagePreview
            // 
            this.boxImagePreview.BackColor = System.Drawing.Color.Gray;
            this.boxImagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxImagePreview.Image = ((System.Drawing.Image)(resources.GetObject("boxImagePreview.Image")));
            this.boxImagePreview.Location = new System.Drawing.Point(3, 3);
            this.boxImagePreview.Name = "boxImagePreview";
            this.boxImagePreview.Size = new System.Drawing.Size(824, 489);
            this.boxImagePreview.TabIndex = 0;
            this.boxImagePreview.TabStop = false;
            // 
            // boxGeneralButtons
            // 
            this.boxGeneralButtons.BackColor = System.Drawing.Color.White;
            this.boxGeneralButtons.ColumnCount = 2;
            this.boxGeneralButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.boxGeneralButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.boxGeneralButtons.Controls.Add(this.btnAdd, 0, 0);
            this.boxGeneralButtons.Controls.Add(this.btnCopy, 0, 1);
            this.boxGeneralButtons.Controls.Add(this.btnDelete, 1, 0);
            this.boxGeneralButtons.Controls.Add(this.btnPaste, 1, 1);
            this.boxGeneralButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxGeneralButtons.Enabled = false;
            this.boxGeneralButtons.Location = new System.Drawing.Point(0, 449);
            this.boxGeneralButtons.MinimumSize = new System.Drawing.Size(50, 50);
            this.boxGeneralButtons.Name = "boxGeneralButtons";
            this.boxGeneralButtons.RowCount = 2;
            this.boxGeneralButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.boxGeneralButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.boxGeneralButtons.Size = new System.Drawing.Size(321, 72);
            this.boxGeneralButtons.TabIndex = 6;
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(154, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Object";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.AutoSize = true;
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopy.Location = new System.Drawing.Point(3, 39);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(154, 30);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy Object";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(163, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(155, 30);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Remove Object";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.AutoSize = true;
            this.btnPaste.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPaste.Location = new System.Drawing.Point(163, 39);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(155, 30);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "Paste Object";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // splitContainerTreeInfo
            // 
            this.splitContainerTreeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTreeInfo.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTreeInfo.Name = "splitContainerTreeInfo";
            // 
            // splitContainerTreeInfo.Panel1
            // 
            this.splitContainerTreeInfo.Panel1.Controls.Add(this.treeMain);
            this.splitContainerTreeInfo.Panel1.Controls.Add(this.boxGeneralButtons);
            // 
            // splitContainerTreeInfo.Panel2
            // 
            this.splitContainerTreeInfo.Panel2.Controls.Add(this.tabControl);
            this.splitContainerTreeInfo.Size = new System.Drawing.Size(1163, 521);
            this.splitContainerTreeInfo.SplitterDistance = 321;
            this.splitContainerTreeInfo.TabIndex = 7;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFiller,
            this.lblStatus,
            this.ProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 678);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip.Size = new System.Drawing.Size(1163, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblFiller
            // 
            this.lblFiller.Name = "lblFiller";
            this.lblFiller.Size = new System.Drawing.Size(1046, 17);
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
            // splitContainerLog_InfoTree
            // 
            this.splitContainerLog_InfoTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLog_InfoTree.Location = new System.Drawing.Point(0, 27);
            this.splitContainerLog_InfoTree.Name = "splitContainerLog_InfoTree";
            this.splitContainerLog_InfoTree.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLog_InfoTree.Panel1
            // 
            this.splitContainerLog_InfoTree.Panel1.Controls.Add(this.splitContainerTreeInfo);
            // 
            // splitContainerLog_InfoTree.Panel2
            // 
            this.splitContainerLog_InfoTree.Panel2.Controls.Add(this.boxLog);
            this.splitContainerLog_InfoTree.Size = new System.Drawing.Size(1163, 651);
            this.splitContainerLog_InfoTree.SplitterDistance = 521;
            this.splitContainerLog_InfoTree.TabIndex = 9;
            // 
            // boxLog
            // 
            this.boxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxLog.Location = new System.Drawing.Point(0, 0);
            this.boxLog.Name = "boxLog";
            this.boxLog.ReadOnly = true;
            this.boxLog.Size = new System.Drawing.Size(1163, 126);
            this.boxLog.TabIndex = 0;
            this.boxLog.Text = "";
            // 
            // minimizeGPKToolStripMenuItem
            // 
            this.minimizeGPKToolStripMenuItem.Name = "minimizeGPKToolStripMenuItem";
            this.minimizeGPKToolStripMenuItem.Size = new System.Drawing.Size(236, 24);
            this.minimizeGPKToolStripMenuItem.Text = "Minimize GPK";
            this.minimizeGPKToolStripMenuItem.Click += new System.EventHandler(this.minimizeGPKToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(233, 6);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(233, 6);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1163, 700);
            this.Controls.Add(this.splitContainerLog_InfoTree);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "GUI";
            this.Text = "Terahelper 0.16 - by GoneUp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeMain_DragDrop);
            this.Resize += new System.EventHandler(this.GUI_Resize);
            this.treeContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabInfo.ResumeLayout(false);
            this.tabInfo.PerformLayout();
            this.boxDataButtons.ResumeLayout(false);
            this.boxDataButtons.PerformLayout();
            this.tabPropertys.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).EndInit();
            this.boxPropertyButtons.ResumeLayout(false);
            this.boxPropertyButtons.PerformLayout();
            this.tabTexturePreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boxImagePreview)).EndInit();
            this.boxGeneralButtons.ResumeLayout(false);
            this.boxGeneralButtons.PerformLayout();
            this.splitContainerTreeInfo.Panel1.ResumeLayout(false);
            this.splitContainerTreeInfo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeInfo)).EndInit();
            this.splitContainerTreeInfo.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainerLog_InfoTree.Panel1.ResumeLayout(false);
            this.splitContainerLog_InfoTree.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLog_InfoTree)).EndInit();
            this.splitContainerLog_InfoTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox boxInfo;
        private System.Windows.Forms.Button btnImportRaw;
        private System.Windows.Forms.Button btnExportRaw;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ToolStripMenuItem replaceSaveToolStripMenuItem;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExportOgg;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.Button btnImportOgg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button btnFakeOGG;
        private System.Windows.Forms.ToolStripMenuItem miscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFilesizeToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPropertys;
        private System.Windows.Forms.DataGridView gridProps;
        private SplitContainer splitContainerTreeInfo;
        private Button btnPropClear;
        private Button btnPropSave;
        private DataGridViewTextBoxColumn name;
        private DataGridViewComboBoxColumn type;
        private DataGridViewTextBoxColumn size;
        private DataGridViewTextBoxColumn aIndex;
        private DataGridViewTextBoxColumn iType;
        private DataGridViewTextBoxColumn value;
        private Button btnPreviewOgg;
        private ToolStripMenuItem setAllPropertysToolStripMenuItem;
        private ToolStripMenuItem setAllVolumeMultipliersToolStripMenuItem;
        private ToolStripMenuItem customToolStripMenuItem;
        private ToolStripMenuItem tESTBigBytePropExportToolStripMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar ProgressBar;
        private ToolStripStatusLabel lblFiller;
        private ToolStripMenuItem bigBytePropImportToolStripMenuItem;
        private ToolStripMenuItem addNameToolStripMenuItem;
        private TabPage tabTexturePreview;
        private PictureBox boxImagePreview;
        private Button btnExportDDS;
        private Button btnImportDDS;
        private ToolStripMenuItem searchForObjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem nextToolStripMenuItem;
        private SplitContainer splitContainerLog_InfoTree;
        private TableLayoutPanel boxPropertyButtons;
        private TableLayoutPanel boxGeneralButtons;
        private TableLayoutPanel boxDataButtons;
        private ContextMenuStrip treeContextMenu;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem importRawDataToolStripMenuItem;
        private ToolStripMenuItem importOGGToolStripMenuItem;
        private ToolStripMenuItem importDDSToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem exportRawDataToolStripMenuItem;
        private ToolStripMenuItem exportOGGToolStripMenuItem;
        private ToolStripMenuItem exportDDSToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem previewOGGToolStripMenuItem;
        private ToolStripMenuItem savePaddingStripMenuItem;
        private Button btnExportProps;
        private ToolStripMenuItem dumpHeadersMenuItem;
        private ToolStripMenuItem loggingActiveMenuItem;
        private ToolStripMenuItem compositeGPKToolStripMenuItem;
        private ToolStripMenuItem decryptionToolStripMenuItem;
        private ToolStripMenuItem loadMappingToolStripMenuItem;
        private ToolStripMenuItem dumpCompositeTexturesToolStripMenuItem;
        private RichTextBox boxLog;
        private ToolStripMenuItem exportPackageToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem savePatchedCompositeStripMenuItem;
        private ToolStripMenuItem datEncryptionToolStripMenuItem;
        private ToolStripMenuItem writeMappingsToolStripMenuItem;
        private ToolStripMenuItem saveAddedCompositeToolStripMenuItem;
        private ToolStripMenuItem dumpIconsToolStripMenuItem;
        private ToolStripMenuItem insertStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem patchObjectMapperforSelectedPackageToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem minimizeGPKToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
    }
}

