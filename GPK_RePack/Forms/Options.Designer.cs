namespace GPK_RePack.Forms
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnProperties = new System.Windows.Forms.RadioButton();
            this.btnData = new System.Windows.Forms.RadioButton();
            this.btnDataProps = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLogTrace = new System.Windows.Forms.RadioButton();
            this.btnLogInfo = new System.Windows.Forms.RadioButton();
            this.btnLogDebug = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnViewPack = new System.Windows.Forms.RadioButton();
            this.btnViewNormal = new System.Windows.Forms.RadioButton();
            this.btnViewClass = new System.Windows.Forms.RadioButton();
            this.boxDebug = new System.Windows.Forms.CheckBox();
            this.boxImports = new System.Windows.Forms.CheckBox();
            this.boxPatchmode = new System.Windows.Forms.CheckBox();
            this.boxUseUID = new System.Windows.Forms.CheckBox();
            this.boxJitData = new System.Windows.Forms.CheckBox();
            this.boxGenerateMipmaps = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.boxScaleFactor = new System.Windows.Forms.TextBox();
            this.btnOpenSettingsFolder = new System.Windows.Forms.Button();
            this.boxEnableTexture2D = new System.Windows.Forms.CheckBox();
            this.boxColorPreview = new System.Windows.Forms.PictureBox();
            this.btnSelectColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.boxSavefilePostfix = new System.Windows.Forms.TextBox();
            this.boxCompression = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxColorPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnProperties);
            this.groupBox1.Controls.Add(this.btnData);
            this.groupBox1.Controls.Add(this.btnDataProps);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy Mode";
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(6, 65);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(107, 17);
            this.btnProperties.TabIndex = 2;
            this.btnProperties.TabStop = true;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.CheckedChanged += new System.EventHandler(this.btnProperties_CheckedChanged);
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(6, 42);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(107, 17);
            this.btnData.TabIndex = 1;
            this.btnData.TabStop = true;
            this.btnData.Text = "Data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.CheckedChanged += new System.EventHandler(this.btnData_CheckedChanged);
            // 
            // btnDataProps
            // 
            this.btnDataProps.AutoSize = true;
            this.btnDataProps.Location = new System.Drawing.Point(6, 19);
            this.btnDataProps.Name = "btnDataProps";
            this.btnDataProps.Size = new System.Drawing.Size(107, 17);
            this.btnDataProps.TabIndex = 0;
            this.btnDataProps.TabStop = true;
            this.btnDataProps.Text = "Data + Properties";
            this.btnDataProps.UseVisualStyleBackColor = true;
            this.btnDataProps.CheckedChanged += new System.EventHandler(this.btnDataProps_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLogTrace);
            this.groupBox2.Controls.Add(this.btnLogInfo);
            this.groupBox2.Controls.Add(this.btnLogDebug);
            this.groupBox2.Location = new System.Drawing.Point(12, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 98);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log Level";
            // 
            // btnLogTrace
            // 
            this.btnLogTrace.Location = new System.Drawing.Point(6, 65);
            this.btnLogTrace.Name = "btnLogTrace";
            this.btnLogTrace.Size = new System.Drawing.Size(107, 17);
            this.btnLogTrace.TabIndex = 5;
            this.btnLogTrace.TabStop = true;
            this.btnLogTrace.Text = "Trace";
            this.btnLogTrace.UseVisualStyleBackColor = true;
            this.btnLogTrace.CheckedChanged += new System.EventHandler(this.btnLogTrace_CheckedChanged);
            // 
            // btnLogInfo
            // 
            this.btnLogInfo.AutoSize = true;
            this.btnLogInfo.Location = new System.Drawing.Point(6, 19);
            this.btnLogInfo.Name = "btnLogInfo";
            this.btnLogInfo.Size = new System.Drawing.Size(43, 17);
            this.btnLogInfo.TabIndex = 3;
            this.btnLogInfo.TabStop = true;
            this.btnLogInfo.Text = "Info";
            this.btnLogInfo.UseVisualStyleBackColor = true;
            this.btnLogInfo.CheckedChanged += new System.EventHandler(this.btnLogInfo_CheckedChanged);
            // 
            // btnLogDebug
            // 
            this.btnLogDebug.Location = new System.Drawing.Point(6, 42);
            this.btnLogDebug.Name = "btnLogDebug";
            this.btnLogDebug.Size = new System.Drawing.Size(107, 17);
            this.btnLogDebug.TabIndex = 4;
            this.btnLogDebug.TabStop = true;
            this.btnLogDebug.Text = "Debug";
            this.btnLogDebug.UseVisualStyleBackColor = true;
            this.btnLogDebug.CheckedChanged += new System.EventHandler(this.btnLogDebug_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 285);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The settings are saved automatically.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnViewPack);
            this.groupBox3.Controls.Add(this.btnViewNormal);
            this.groupBox3.Controls.Add(this.btnViewClass);
            this.groupBox3.Location = new System.Drawing.Point(171, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(176, 98);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "View Mode";
            // 
            // btnViewPack
            // 
            this.btnViewPack.Location = new System.Drawing.Point(6, 65);
            this.btnViewPack.Name = "btnViewPack";
            this.btnViewPack.Size = new System.Drawing.Size(107, 17);
            this.btnViewPack.TabIndex = 5;
            this.btnViewPack.TabStop = true;
            this.btnViewPack.Text = "Per Package";
            this.btnViewPack.UseVisualStyleBackColor = true;
            this.btnViewPack.CheckedChanged += new System.EventHandler(this.btnViewPack_CheckedChanged);
            // 
            // btnViewNormal
            // 
            this.btnViewNormal.AutoSize = true;
            this.btnViewNormal.Location = new System.Drawing.Point(6, 19);
            this.btnViewNormal.Name = "btnViewNormal";
            this.btnViewNormal.Size = new System.Drawing.Size(141, 17);
            this.btnViewNormal.TabIndex = 3;
            this.btnViewNormal.TabStop = true;
            this.btnViewNormal.Text = "Normal (Imports/Exports)";
            this.btnViewNormal.UseVisualStyleBackColor = true;
            this.btnViewNormal.CheckedChanged += new System.EventHandler(this.btnViewNormal_CheckedChanged);
            // 
            // btnViewClass
            // 
            this.btnViewClass.Location = new System.Drawing.Point(6, 42);
            this.btnViewClass.Name = "btnViewClass";
            this.btnViewClass.Size = new System.Drawing.Size(107, 17);
            this.btnViewClass.TabIndex = 4;
            this.btnViewClass.TabStop = true;
            this.btnViewClass.Text = "Per Class";
            this.btnViewClass.UseVisualStyleBackColor = true;
            this.btnViewClass.CheckedChanged += new System.EventHandler(this.btnViewClass_CheckedChanged);
            // 
            // boxDebug
            // 
            this.boxDebug.AutoSize = true;
            this.boxDebug.Location = new System.Drawing.Point(171, 147);
            this.boxDebug.Name = "boxDebug";
            this.boxDebug.Size = new System.Drawing.Size(88, 17);
            this.boxDebug.TabIndex = 7;
            this.boxDebug.Text = "Debug Mode";
            this.boxDebug.UseVisualStyleBackColor = true;
            this.boxDebug.CheckedChanged += new System.EventHandler(this.boxDebug_CheckedChanged);
            // 
            // boxImports
            // 
            this.boxImports.AutoSize = true;
            this.boxImports.Location = new System.Drawing.Point(171, 116);
            this.boxImports.Name = "boxImports";
            this.boxImports.Size = new System.Drawing.Size(90, 17);
            this.boxImports.TabIndex = 8;
            this.boxImports.Text = "Show Imports";
            this.boxImports.UseVisualStyleBackColor = true;
            this.boxImports.CheckedChanged += new System.EventHandler(this.boxImports_CheckedChanged);
            // 
            // boxPatchmode
            // 
            this.boxPatchmode.AutoSize = true;
            this.boxPatchmode.Location = new System.Drawing.Point(171, 131);
            this.boxPatchmode.Name = "boxPatchmode";
            this.boxPatchmode.Size = new System.Drawing.Size(81, 17);
            this.boxPatchmode.TabIndex = 9;
            this.boxPatchmode.Text = "PatchMode";
            this.boxPatchmode.UseVisualStyleBackColor = true;
            this.boxPatchmode.CheckedChanged += new System.EventHandler(this.boxPatchmode_CheckedChanged);
            // 
            // boxUseUID
            // 
            this.boxUseUID.AutoSize = true;
            this.boxUseUID.Location = new System.Drawing.Point(258, 116);
            this.boxUseUID.Name = "boxUseUID";
            this.boxUseUID.Size = new System.Drawing.Size(70, 17);
            this.boxUseUID.TabIndex = 10;
            this.boxUseUID.Text = "UID Tree";
            this.boxUseUID.UseVisualStyleBackColor = true;
            this.boxUseUID.CheckedChanged += new System.EventHandler(this.boxUseUID_CheckedChanged);
            // 
            // boxJitData
            // 
            this.boxJitData.AutoSize = true;
            this.boxJitData.Location = new System.Drawing.Point(258, 131);
            this.boxJitData.Name = "boxJitData";
            this.boxJitData.Size = new System.Drawing.Size(67, 17);
            this.boxJitData.TabIndex = 11;
            this.boxJitData.Text = "JIT Data";
            this.boxJitData.UseVisualStyleBackColor = true;
            this.boxJitData.CheckedChanged += new System.EventHandler(this.boxJitData_CheckedChanged);
            // 
            // boxGenerateMipmaps
            // 
            this.boxGenerateMipmaps.Location = new System.Drawing.Point(171, 163);
            this.boxGenerateMipmaps.Name = "boxGenerateMipmaps";
            this.boxGenerateMipmaps.Size = new System.Drawing.Size(134, 17);
            this.boxGenerateMipmaps.TabIndex = 12;
            this.boxGenerateMipmaps.Text = "Generate MipMaps";
            this.boxGenerateMipmaps.UseVisualStyleBackColor = true;
            this.boxGenerateMipmaps.CheckedChanged += new System.EventHandler(this.boxGenerateMipmaps_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Font Scaling Factor (x,xx): ";
            // 
            // boxScaleFactor
            // 
            this.boxScaleFactor.Location = new System.Drawing.Point(302, 178);
            this.boxScaleFactor.Name = "boxScaleFactor";
            this.boxScaleFactor.Size = new System.Drawing.Size(36, 20);
            this.boxScaleFactor.TabIndex = 14;
            this.boxScaleFactor.Text = "1,0";
            this.boxScaleFactor.TextChanged += new System.EventHandler(this.boxScaleFactor_TextChanged);
            // 
            // btnOpenSettingsFolder
            // 
            this.btnOpenSettingsFolder.Location = new System.Drawing.Point(12, 261);
            this.btnOpenSettingsFolder.Name = "btnOpenSettingsFolder";
            this.btnOpenSettingsFolder.Size = new System.Drawing.Size(147, 20);
            this.btnOpenSettingsFolder.TabIndex = 15;
            this.btnOpenSettingsFolder.Text = "Open Settings Folder";
            this.btnOpenSettingsFolder.UseVisualStyleBackColor = true;
            this.btnOpenSettingsFolder.Click += new System.EventHandler(this.btnOpenSettingsFolder_Click);
            // 
            // boxEnableTexture2D
            // 
            this.boxEnableTexture2D.AutoSize = true;
            this.boxEnableTexture2D.Location = new System.Drawing.Point(258, 147);
            this.boxEnableTexture2D.Name = "boxEnableTexture2D";
            this.boxEnableTexture2D.Size = new System.Drawing.Size(102, 17);
            this.boxEnableTexture2D.TabIndex = 16;
            this.boxEnableTexture2D.Text = "Texture Support";
            this.boxEnableTexture2D.UseVisualStyleBackColor = true;
            this.boxEnableTexture2D.CheckedChanged += new System.EventHandler(this.boxEnableTexture2D_CheckedChanged);
            // 
            // boxColorPreview
            // 
            this.boxColorPreview.Location = new System.Drawing.Point(305, 261);
            this.boxColorPreview.Name = "boxColorPreview";
            this.boxColorPreview.Size = new System.Drawing.Size(36, 20);
            this.boxColorPreview.TabIndex = 17;
            this.boxColorPreview.TabStop = false;
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.Location = new System.Drawing.Point(165, 261);
            this.btnSelectColor.Name = "btnSelectColor";
            this.btnSelectColor.Size = new System.Drawing.Size(132, 20);
            this.btnSelectColor.TabIndex = 18;
            this.btnSelectColor.Text = "Select Image BG Color";
            this.btnSelectColor.UseVisualStyleBackColor = true;
            this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 199);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Savefile Suffix";
            // 
            // boxSavefilePostfix
            // 
            this.boxSavefilePostfix.Location = new System.Drawing.Point(258, 197);
            this.boxSavefilePostfix.Name = "boxSavefilePostfix";
            this.boxSavefilePostfix.Size = new System.Drawing.Size(80, 20);
            this.boxSavefilePostfix.TabIndex = 20;
            this.boxSavefilePostfix.TextChanged += new System.EventHandler(this.boxSavefilePostfix_TextChanged);
            // 
            // boxCompression
            // 
            this.boxCompression.Location = new System.Drawing.Point(12, 220);
            this.boxCompression.Name = "boxCompression";
            this.boxCompression.Size = new System.Drawing.Size(153, 17);
            this.boxCompression.TabIndex = 21;
            this.boxCompression.Text = "Enable Savecompression";
            this.boxCompression.UseVisualStyleBackColor = true;
            this.boxCompression.CheckedChanged += new System.EventHandler(this.boxCompression_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 309);
            this.Controls.Add(this.boxCompression);
            this.Controls.Add(this.boxSavefilePostfix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSelectColor);
            this.Controls.Add(this.boxColorPreview);
            this.Controls.Add(this.boxEnableTexture2D);
            this.Controls.Add(this.btnOpenSettingsFolder);
            this.Controls.Add(this.boxScaleFactor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.boxGenerateMipmaps);
            this.Controls.Add(this.boxJitData);
            this.Controls.Add(this.boxUseUID);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.boxPatchmode);
            this.Controls.Add(this.boxImports);
            this.Controls.Add(this.boxDebug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Options_FormClosed);
            this.Load += new System.EventHandler(this.Options_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxColorPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton btnData;
        private System.Windows.Forms.RadioButton btnDataProps;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton btnProperties;
        private System.Windows.Forms.RadioButton btnLogTrace;
        private System.Windows.Forms.RadioButton btnLogInfo;
        private System.Windows.Forms.RadioButton btnLogDebug;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton btnViewNormal;
        private System.Windows.Forms.RadioButton btnViewClass;
        private System.Windows.Forms.CheckBox boxDebug;
        private System.Windows.Forms.CheckBox boxImports;
        private System.Windows.Forms.CheckBox boxPatchmode;
        private System.Windows.Forms.CheckBox boxUseUID;
        private System.Windows.Forms.CheckBox boxJitData;
        private System.Windows.Forms.CheckBox boxGenerateMipmaps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox boxScaleFactor;
        private System.Windows.Forms.RadioButton btnViewPack;
        private System.Windows.Forms.Button btnOpenSettingsFolder;
        private System.Windows.Forms.CheckBox boxEnableTexture2D;
        private System.Windows.Forms.PictureBox boxColorPreview;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox boxSavefilePostfix;
        private System.Windows.Forms.CheckBox boxCompression;
    }
}