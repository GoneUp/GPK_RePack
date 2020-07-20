using GPK_RePack.Forms.Helper;

namespace GPK_RePack.Forms
{
    partial class FormMapperView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeMapperView = new GPK_RePack.Forms.Helper.CompositeTreeView();
            this.btnDeleteMapping = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.boxSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeMapperView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnDeleteMapping);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.boxSearch);
            this.splitContainer1.Size = new System.Drawing.Size(584, 480);
            this.splitContainer1.SplitterDistance = 431;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeMapperView
            // 
            this.treeMapperView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMapperView.Location = new System.Drawing.Point(0, 0);
            this.treeMapperView.Name = "treeMapperView";
            this.treeMapperView.Size = new System.Drawing.Size(584, 431);
            this.treeMapperView.TabIndex = 0;
            this.treeMapperView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMapperView_AfterSelect);
            // 
            // btnDeleteMapping
            // 
            this.btnDeleteMapping.Location = new System.Drawing.Point(478, 22);
            this.btnDeleteMapping.Name = "btnDeleteMapping";
            this.btnDeleteMapping.Size = new System.Drawing.Size(106, 23);
            this.btnDeleteMapping.TabIndex = 2;
            this.btnDeleteMapping.Text = "Delete Mapping";
            this.btnDeleteMapping.UseVisualStyleBackColor = true;
            this.btnDeleteMapping.Click += new System.EventHandler(this.btnDeleteMapping_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search here. Caseinsensitive. Length > 3.";
            // 
            // boxSearch
            // 
            this.boxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxSearch.Location = new System.Drawing.Point(0, 0);
            this.boxSearch.Name = "boxSearch";
            this.boxSearch.Size = new System.Drawing.Size(584, 20);
            this.boxSearch.TabIndex = 0;
            this.boxSearch.TextChanged += new System.EventHandler(this.boxSearch_TextChanged);
            // 
            // FormMapperView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 480);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormMapperView";
            this.Text = "MapperView";
            this.Load += new System.EventHandler(this.formMapperView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox boxSearch;
        private CompositeTreeView treeMapperView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteMapping;
    }
}