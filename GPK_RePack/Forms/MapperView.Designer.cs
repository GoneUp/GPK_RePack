﻿using GPK_RePack.Forms.Helper;

namespace GPK_RePack.Forms
{
    partial class formMapperView
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
            this.boxSearch = new System.Windows.Forms.TextBox();
            this.treeMapperView = new GPK_RePack.Forms.Helper.CompositeTreeView();
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
            this.splitContainer1.Panel2.Controls.Add(this.boxSearch);
            this.splitContainer1.Size = new System.Drawing.Size(386, 569);
            this.splitContainer1.SplitterDistance = 540;
            this.splitContainer1.TabIndex = 0;
            // 
            // boxSearch
            // 
            this.boxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxSearch.Location = new System.Drawing.Point(0, 0);
            this.boxSearch.Name = "boxSearch";
            this.boxSearch.Size = new System.Drawing.Size(386, 20);
            this.boxSearch.TabIndex = 0;
            this.boxSearch.TextChanged += new System.EventHandler(this.boxSearch_TextChanged);
            // 
            // treeMapperView
            // 
            this.treeMapperView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMapperView.Location = new System.Drawing.Point(0, 0);
            this.treeMapperView.Name = "treeMapperView";
            this.treeMapperView.Size = new System.Drawing.Size(386, 540);
            this.treeMapperView.TabIndex = 0;
            this.treeMapperView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMapperView_AfterSelect);
            // 
            // formMapperView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 569);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "formMapperView";
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
    }
}