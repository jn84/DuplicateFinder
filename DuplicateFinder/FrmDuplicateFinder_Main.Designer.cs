namespace DuplicateFinder
{
	partial class FrmDuplicateFinder_Main
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lstDirectories = new System.Windows.Forms.ListBox();
			this.btnAddDirectory = new System.Windows.Forms.Button();
			this.btnFindDupes = new System.Windows.Forms.Button();
			this.ctxMenuDir = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.txtTotalFiles = new System.Windows.Forms.TextBox();
			this.lblProcessed = new System.Windows.Forms.Label();
			this.txtTotalFolders = new System.Windows.Forms.TextBox();
			this.lblDirectoriesFound = new System.Windows.Forms.Label();
			this.dgrdFileDuplicates = new System.Windows.Forms.DataGridView();
			this.gridColKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgrdFileList = new System.Windows.Forms.DataGridView();
			this.gridColFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tabCtlDuplicates = new System.Windows.Forms.TabControl();
			this.tabFiles = new System.Windows.Forms.TabPage();
			this.tabFolders = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.gridColFolderKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColFolderSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColSubFolderCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridColFolderCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.progbarFiles = new System.Windows.Forms.ProgressBar();
			this.chkSkipEmptyFiles = new System.Windows.Forms.CheckBox();
			this.ctxMenuDir.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgrdFileDuplicates)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgrdFileList)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tabCtlDuplicates.SuspendLayout();
			this.tabFiles.SuspendLayout();
			this.tabFolders.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// lstDirectories
			// 
			this.lstDirectories.FormattingEnabled = true;
			this.lstDirectories.ItemHeight = 20;
			this.lstDirectories.Location = new System.Drawing.Point(18, 18);
			this.lstDirectories.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.lstDirectories.Name = "lstDirectories";
			this.lstDirectories.Size = new System.Drawing.Size(592, 164);
			this.lstDirectories.TabIndex = 1;
			this.lstDirectories.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstDirectories_MouseDown);
			// 
			// btnAddDirectory
			// 
			this.btnAddDirectory.Location = new System.Drawing.Point(18, 234);
			this.btnAddDirectory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnAddDirectory.Name = "btnAddDirectory";
			this.btnAddDirectory.Size = new System.Drawing.Size(254, 35);
			this.btnAddDirectory.TabIndex = 2;
			this.btnAddDirectory.Text = "Add Directory";
			this.btnAddDirectory.UseVisualStyleBackColor = true;
			this.btnAddDirectory.Click += new System.EventHandler(this.btnAddDirectory_Click);
			// 
			// btnFindDupes
			// 
			this.btnFindDupes.Location = new System.Drawing.Point(358, 234);
			this.btnFindDupes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnFindDupes.Name = "btnFindDupes";
			this.btnFindDupes.Size = new System.Drawing.Size(254, 35);
			this.btnFindDupes.TabIndex = 3;
			this.btnFindDupes.Text = "Find Duplicates";
			this.btnFindDupes.UseVisualStyleBackColor = true;
			this.btnFindDupes.Click += new System.EventHandler(this.btnFindDupes_Click);
			// 
			// ctxMenuDir
			// 
			this.ctxMenuDir.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.ctxMenuDir.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDirectoryToolStripMenuItem,
            this.removeDirectoryToolStripMenuItem});
			this.ctxMenuDir.Name = "ctxMenuDir";
			this.ctxMenuDir.Size = new System.Drawing.Size(297, 64);
			// 
			// addDirectoryToolStripMenuItem
			// 
			this.addDirectoryToolStripMenuItem.Name = "addDirectoryToolStripMenuItem";
			this.addDirectoryToolStripMenuItem.Size = new System.Drawing.Size(296, 30);
			this.addDirectoryToolStripMenuItem.Text = "Add Directory";
			this.addDirectoryToolStripMenuItem.Click += new System.EventHandler(this.addDirectoryToolStripMenuItem_Click);
			// 
			// removeDirectoryToolStripMenuItem
			// 
			this.removeDirectoryToolStripMenuItem.Name = "removeDirectoryToolStripMenuItem";
			this.removeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(296, 30);
			this.removeDirectoryToolStripMenuItem.Text = "Remove Selected Directory";
			this.removeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.removeDirectoryToolStripMenuItem_Click);
			// 
			// txtTotalFiles
			// 
			this.txtTotalFiles.Location = new System.Drawing.Point(131, 880);
			this.txtTotalFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtTotalFiles.Name = "txtTotalFiles";
			this.txtTotalFiles.ReadOnly = true;
			this.txtTotalFiles.Size = new System.Drawing.Size(148, 26);
			this.txtTotalFiles.TabIndex = 4;
			// 
			// lblProcessed
			// 
			this.lblProcessed.AutoSize = true;
			this.lblProcessed.Location = new System.Drawing.Point(42, 883);
			this.lblProcessed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblProcessed.Name = "lblProcessed";
			this.lblProcessed.Size = new System.Drawing.Size(81, 20);
			this.lblProcessed.TabIndex = 5;
			this.lblProcessed.Text = "Total Files";
			// 
			// txtTotalFolders
			// 
			this.txtTotalFolders.Location = new System.Drawing.Point(131, 916);
			this.txtTotalFolders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtTotalFolders.Name = "txtTotalFolders";
			this.txtTotalFolders.ReadOnly = true;
			this.txtTotalFolders.Size = new System.Drawing.Size(148, 26);
			this.txtTotalFolders.TabIndex = 6;
			// 
			// lblDirectoriesFound
			// 
			this.lblDirectoriesFound.AutoSize = true;
			this.lblDirectoriesFound.Location = new System.Drawing.Point(22, 919);
			this.lblDirectoriesFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDirectoriesFound.Name = "lblDirectoriesFound";
			this.lblDirectoriesFound.Size = new System.Drawing.Size(101, 20);
			this.lblDirectoriesFound.TabIndex = 7;
			this.lblDirectoriesFound.Text = "Total Folders";
			// 
			// dgrdFileDuplicates
			// 
			this.dgrdFileDuplicates.AllowUserToAddRows = false;
			this.dgrdFileDuplicates.AllowUserToDeleteRows = false;
			this.dgrdFileDuplicates.AllowUserToOrderColumns = true;
			this.dgrdFileDuplicates.AllowUserToResizeColumns = false;
			this.dgrdFileDuplicates.AllowUserToResizeRows = false;
			this.dgrdFileDuplicates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgrdFileDuplicates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColKey,
            this.gridColFileName,
            this.gridColCount,
            this.gridColSize});
			this.dgrdFileDuplicates.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgrdFileDuplicates.Location = new System.Drawing.Point(0, 0);
			this.dgrdFileDuplicates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dgrdFileDuplicates.MultiSelect = false;
			this.dgrdFileDuplicates.Name = "dgrdFileDuplicates";
			this.dgrdFileDuplicates.ReadOnly = true;
			this.dgrdFileDuplicates.Size = new System.Drawing.Size(579, 545);
			this.dgrdFileDuplicates.TabIndex = 8;
			this.dgrdFileDuplicates.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrdFileDuplicates_RowEnter);
			// 
			// gridColKey
			// 
			dataGridViewCellStyle1.NullValue = null;
			this.gridColKey.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridColKey.HeaderText = "";
			this.gridColKey.Name = "gridColKey";
			this.gridColKey.ReadOnly = true;
			this.gridColKey.Visible = false;
			// 
			// gridColFileName
			// 
			this.gridColFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.gridColFileName.HeaderText = "File Name";
			this.gridColFileName.Name = "gridColFileName";
			this.gridColFileName.ReadOnly = true;
			this.gridColFileName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// gridColCount
			// 
			dataGridViewCellStyle2.NullValue = null;
			this.gridColCount.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridColCount.HeaderText = "Count";
			this.gridColCount.Name = "gridColCount";
			this.gridColCount.ReadOnly = true;
			this.gridColCount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.gridColCount.Width = 50;
			// 
			// gridColSize
			// 
			dataGridViewCellStyle3.Format = "N4";
			dataGridViewCellStyle3.NullValue = null;
			this.gridColSize.DefaultCellStyle = dataGridViewCellStyle3;
			this.gridColSize.HeaderText = "Size (KB)";
			this.gridColSize.Name = "gridColSize";
			this.gridColSize.ReadOnly = true;
			this.gridColSize.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// dgrdFileList
			// 
			this.dgrdFileList.AllowUserToAddRows = false;
			this.dgrdFileList.AllowUserToDeleteRows = false;
			this.dgrdFileList.AllowUserToResizeColumns = false;
			this.dgrdFileList.AllowUserToResizeRows = false;
			this.dgrdFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgrdFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColFullPath});
			this.dgrdFileList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgrdFileList.Location = new System.Drawing.Point(9, 258);
			this.dgrdFileList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dgrdFileList.Name = "dgrdFileList";
			this.dgrdFileList.ReadOnly = true;
			this.dgrdFileList.Size = new System.Drawing.Size(836, 606);
			this.dgrdFileList.TabIndex = 9;
			// 
			// gridColFullPath
			// 
			this.gridColFullPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.gridColFullPath.HeaderText = "Full File Path";
			this.gridColFullPath.Name = "gridColFullPath";
			this.gridColFullPath.ReadOnly = true;
			this.gridColFullPath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dgrdFileList);
			this.groupBox1.Location = new System.Drawing.Point(622, 20);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Size = new System.Drawing.Size(854, 874);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// tabCtlDuplicates
			// 
			this.tabCtlDuplicates.Controls.Add(this.tabFiles);
			this.tabCtlDuplicates.Controls.Add(this.tabFolders);
			this.tabCtlDuplicates.Location = new System.Drawing.Point(22, 280);
			this.tabCtlDuplicates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tabCtlDuplicates.Name = "tabCtlDuplicates";
			this.tabCtlDuplicates.SelectedIndex = 0;
			this.tabCtlDuplicates.Size = new System.Drawing.Size(591, 585);
			this.tabCtlDuplicates.TabIndex = 11;
			// 
			// tabFiles
			// 
			this.tabFiles.Controls.Add(this.dgrdFileDuplicates);
			this.tabFiles.Location = new System.Drawing.Point(4, 29);
			this.tabFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tabFiles.Name = "tabFiles";
			this.tabFiles.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tabFiles.Size = new System.Drawing.Size(583, 552);
			this.tabFiles.TabIndex = 0;
			this.tabFiles.Text = "File Duplicates";
			this.tabFiles.UseVisualStyleBackColor = true;
			// 
			// tabFolders
			// 
			this.tabFolders.Controls.Add(this.dataGridView1);
			this.tabFolders.Location = new System.Drawing.Point(4, 29);
			this.tabFolders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tabFolders.Name = "tabFolders";
			this.tabFolders.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.tabFolders.Size = new System.Drawing.Size(583, 552);
			this.tabFolders.TabIndex = 1;
			this.tabFolders.Text = "Folder Duplicates";
			this.tabFolders.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColFolderKey,
            this.gridColFolderSize,
            this.gridColFiles,
            this.gridColSubFolderCount,
            this.gridColFolderCount});
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(579, 545);
			this.dataGridView1.TabIndex = 0;
			// 
			// gridColFolderKey
			// 
			this.gridColFolderKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.gridColFolderKey.HeaderText = "";
			this.gridColFolderKey.Name = "gridColFolderKey";
			this.gridColFolderKey.ReadOnly = true;
			this.gridColFolderKey.Visible = false;
			this.gridColFolderKey.Width = 90;
			// 
			// gridColFolderSize
			// 
			this.gridColFolderSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.gridColFolderSize.HeaderText = "Folder Size (MB)";
			this.gridColFolderSize.Name = "gridColFolderSize";
			this.gridColFolderSize.ReadOnly = true;
			// 
			// gridColFiles
			// 
			this.gridColFiles.HeaderText = "File Count";
			this.gridColFiles.Name = "gridColFiles";
			this.gridColFiles.ReadOnly = true;
			this.gridColFiles.Width = 79;
			// 
			// gridColSubFolderCount
			// 
			this.gridColSubFolderCount.HeaderText = "Subfolder Count";
			this.gridColSubFolderCount.Name = "gridColSubFolderCount";
			this.gridColSubFolderCount.ReadOnly = true;
			this.gridColSubFolderCount.Width = 108;
			// 
			// gridColFolderCount
			// 
			this.gridColFolderCount.HeaderText = "Count";
			this.gridColFolderCount.Name = "gridColFolderCount";
			this.gridColFolderCount.ReadOnly = true;
			this.gridColFolderCount.Width = 45;
			// 
			// progbarFiles
			// 
			this.progbarFiles.Location = new System.Drawing.Point(297, 879);
			this.progbarFiles.Name = "progbarFiles";
			this.progbarFiles.Size = new System.Drawing.Size(308, 27);
			this.progbarFiles.TabIndex = 12;
			// 
			// chkSkipEmptyFiles
			// 
			this.chkSkipEmptyFiles.AutoSize = true;
			this.chkSkipEmptyFiles.Location = new System.Drawing.Point(18, 191);
			this.chkSkipEmptyFiles.Name = "chkSkipEmptyFiles";
			this.chkSkipEmptyFiles.Size = new System.Drawing.Size(167, 24);
			this.chkSkipEmptyFiles.TabIndex = 13;
			this.chkSkipEmptyFiles.Text = "Ignore Empty Files";
			this.chkSkipEmptyFiles.UseVisualStyleBackColor = true;
			// 
			// FrmDuplicateFinder_Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1488, 1062);
			this.Controls.Add(this.chkSkipEmptyFiles);
			this.Controls.Add(this.progbarFiles);
			this.Controls.Add(this.tabCtlDuplicates);
			this.Controls.Add(this.lblDirectoriesFound);
			this.Controls.Add(this.txtTotalFolders);
			this.Controls.Add(this.lblProcessed);
			this.Controls.Add(this.txtTotalFiles);
			this.Controls.Add(this.btnFindDupes);
			this.Controls.Add(this.btnAddDirectory);
			this.Controls.Add(this.lstDirectories);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FrmDuplicateFinder_Main";
			this.Text = "FrmDuplicateFinder_Main";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ctxMenuDir.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgrdFileDuplicates)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgrdFileList)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.tabCtlDuplicates.ResumeLayout(false);
			this.tabFiles.ResumeLayout(false);
			this.tabFolders.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstDirectories;
		private System.Windows.Forms.Button btnAddDirectory;
		private System.Windows.Forms.Button btnFindDupes;
		private System.Windows.Forms.ContextMenuStrip ctxMenuDir;
		private System.Windows.Forms.ToolStripMenuItem addDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeDirectoryToolStripMenuItem;
		private System.Windows.Forms.TextBox txtTotalFiles;
		private System.Windows.Forms.Label lblProcessed;
		private System.Windows.Forms.TextBox txtTotalFolders;
		private System.Windows.Forms.Label lblDirectoriesFound;
		private System.Windows.Forms.DataGridView dgrdFileDuplicates;
		private System.Windows.Forms.DataGridView dgrdFileList;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFullPath;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabControl tabCtlDuplicates;
		private System.Windows.Forms.TabPage tabFiles;
		private System.Windows.Forms.TabPage tabFolders;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFolderKey;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFolderSize;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFiles;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColSubFolderCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFolderCount;
		private System.Windows.Forms.ProgressBar progbarFiles;
		private System.Windows.Forms.CheckBox chkSkipEmptyFiles;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColKey;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColFileName;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridColSize;
	}
}

