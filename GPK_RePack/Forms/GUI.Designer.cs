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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.boxGeneralButtons = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.boxDataButtons = new System.Windows.Forms.GroupBox();
            this.btnFakeOGG = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.btnImportOgg = new System.Windows.Forms.Button();
            this.btnExtractOGG = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.boxInfo = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridProps = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrayIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.boxLog = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.boxGeneralButtons.SuspendLayout();
            this.boxDataButtons.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(317, 507);
            this.treeMain.TabIndex = 1;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.treeMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.miscToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
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
            this.setFilesizeToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // setFilesizeToolStripMenuItem
            // 
            this.setFilesizeToolStripMenuItem.Name = "setFilesizeToolStripMenuItem";
            this.setFilesizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.setFilesizeToolStripMenuItem.Text = "Set Filesize";
            this.setFilesizeToolStripMenuItem.Click += new System.EventHandler(this.setFilesizeToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(317, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(657, 507);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.boxInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(649, 481);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.boxGeneralButtons);
            this.panel2.Controls.Add(this.boxDataButtons);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(643, 100);
            this.panel2.TabIndex = 6;
            // 
            // boxGeneralButtons
            // 
            this.boxGeneralButtons.Controls.Add(this.btnPaste);
            this.boxGeneralButtons.Controls.Add(this.btnCopy);
            this.boxGeneralButtons.Controls.Add(this.btnAdd);
            this.boxGeneralButtons.Controls.Add(this.btnDelete);
            this.boxGeneralButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxGeneralButtons.Enabled = false;
            this.boxGeneralButtons.Location = new System.Drawing.Point(0, 0);
            this.boxGeneralButtons.Name = "boxGeneralButtons";
            this.boxGeneralButtons.Size = new System.Drawing.Size(188, 100);
            this.boxGeneralButtons.TabIndex = 5;
            this.boxGeneralButtons.TabStop = false;
            this.boxGeneralButtons.Text = "General";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(87, 58);
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
            this.btnCopy.Size = new System.Drawing.Size(75, 24);
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
            this.btnAdd.Size = new System.Drawing.Size(75, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Object";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(87, 28);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(95, 24);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Remove Object";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // boxDataButtons
            // 
            this.boxDataButtons.Controls.Add(this.btnFakeOGG);
            this.boxDataButtons.Controls.Add(this.btnDeleteData);
            this.boxDataButtons.Controls.Add(this.btnImportOgg);
            this.boxDataButtons.Controls.Add(this.btnExtractOGG);
            this.boxDataButtons.Controls.Add(this.btnReplace);
            this.boxDataButtons.Controls.Add(this.btnExport);
            this.boxDataButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.boxDataButtons.Enabled = false;
            this.boxDataButtons.Location = new System.Drawing.Point(188, 0);
            this.boxDataButtons.Name = "boxDataButtons";
            this.boxDataButtons.Size = new System.Drawing.Size(455, 100);
            this.boxDataButtons.TabIndex = 4;
            this.boxDataButtons.TabStop = false;
            this.boxDataButtons.Text = "Data Commands";
            // 
            // btnFakeOGG
            // 
            this.btnFakeOGG.Location = new System.Drawing.Point(233, 58);
            this.btnFakeOGG.Name = "btnFakeOGG";
            this.btnFakeOGG.Size = new System.Drawing.Size(104, 24);
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
            this.btnImportOgg.Location = new System.Drawing.Point(114, 58);
            this.btnImportOgg.Name = "btnImportOgg";
            this.btnImportOgg.Size = new System.Drawing.Size(113, 24);
            this.btnImportOgg.TabIndex = 6;
            this.btnImportOgg.Text = "Import OGG";
            this.btnImportOgg.UseVisualStyleBackColor = true;
            this.btnImportOgg.Click += new System.EventHandler(this.btnImportOgg_Click);
            // 
            // btnExtractOGG
            // 
            this.btnExtractOGG.Location = new System.Drawing.Point(6, 58);
            this.btnExtractOGG.Name = "btnExtractOGG";
            this.btnExtractOGG.Size = new System.Drawing.Size(102, 24);
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
            // boxInfo
            // 
            this.boxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxInfo.Location = new System.Drawing.Point(3, 3);
            this.boxInfo.Multiline = true;
            this.boxInfo.Name = "boxInfo";
            this.boxInfo.ReadOnly = true;
            this.boxInfo.Size = new System.Drawing.Size(643, 475);
            this.boxInfo.TabIndex = 5;
            this.boxInfo.Text = resources.GetString("boxInfo.Text");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridProps);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(649, 481);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Property Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridProps
            // 
            this.gridProps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridProps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colType,
            this.colSize,
            this.colArrayIndex,
            this.colInner,
            this.colValue});
            this.gridProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProps.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridProps.Enabled = false;
            this.gridProps.Location = new System.Drawing.Point(3, 3);
            this.gridProps.MultiSelect = false;
            this.gridProps.Name = "gridProps";
            this.gridProps.Size = new System.Drawing.Size(643, 475);
            this.gridProps.TabIndex = 0;
            this.gridProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridProps_CellEndEdit);
            this.gridProps.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridProps_CellValueChanged);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Items.AddRange(new object[] {
            "ArrayProperty",
            "BoolProperty",
            "ByteProperty",
            "FloatProperty",
            "IntProperty",
            "NameProperty",
            "ObjectProperty",
            "StrProperty",
            "StructProperty"});
            this.colType.Name = "colType";
            // 
            // colSize
            // 
            this.colSize.HeaderText = "Size";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // colArrayIndex
            // 
            this.colArrayIndex.HeaderText = "ArrayIndex";
            this.colArrayIndex.Name = "colArrayIndex";
            // 
            // colInner
            // 
            this.colInner.HeaderText = "InnerType";
            this.colInner.Name = "colInner";
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.treeMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 507);
            this.panel1.TabIndex = 4;
            // 
            // boxLog
            // 
            this.boxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.boxLog.Location = new System.Drawing.Point(0, 531);
            this.boxLog.Multiline = true;
            this.boxLog.Name = "boxLog";
            this.boxLog.ReadOnly = true;
            this.boxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.boxLog.Size = new System.Drawing.Size(974, 145);
            this.boxLog.TabIndex = 5;
            this.boxLog.TextChanged += new System.EventHandler(this.boxLog_TextChanged);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 676);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.boxLog);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GUI";
            this.Text = "Terahelper 0.6 - by GoneUp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUI_FormClosed);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.boxGeneralButtons.ResumeLayout(false);
            this.boxDataButtons.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProps)).EndInit();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox boxLog;
        private System.Windows.Forms.TextBox boxInfo;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ToolStripMenuItem replaceSaveToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrayIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInner;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
    }
}

