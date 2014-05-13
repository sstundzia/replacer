namespace Replacer.GUI
{
	partial class MainForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gridFilePaths = new System.Windows.Forms.DataGridView();
			this.columnRootPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnPatternType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.columnIncludeSvnFolders = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridReplacePatterns = new System.Windows.Forms.DataGridView();
			this.columnFind = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnReplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnReplaceType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonReplace = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.buttonNew = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridFilePaths)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridReplacePatterns)).BeginInit();
			this.panel1.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.gridFilePaths);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.gridReplacePatterns);
			this.splitContainer1.Size = new System.Drawing.Size(835, 544);
			this.splitContainer1.SplitterDistance = 230;
			this.splitContainer1.TabIndex = 0;
			// 
			// gridFilePaths
			// 
			this.gridFilePaths.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridFilePaths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridFilePaths.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnRootPath,
            this.columnPattern,
            this.columnPatternType,
            this.columnIncludeSvnFolders});
			this.gridFilePaths.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridFilePaths.Location = new System.Drawing.Point(0, 0);
			this.gridFilePaths.Name = "gridFilePaths";
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFilePaths.RowsDefaultCellStyle = dataGridViewCellStyle1;
			this.gridFilePaths.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFilePaths.Size = new System.Drawing.Size(835, 230);
			this.gridFilePaths.TabIndex = 0;
			// 
			// columnRootPath
			// 
			this.columnRootPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.columnRootPath.DataPropertyName = "RootPath";
			this.columnRootPath.HeaderText = "Root Path";
			this.columnRootPath.Name = "columnRootPath";
			this.columnRootPath.Width = 200;
			// 
			// columnPattern
			// 
			this.columnPattern.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.columnPattern.DataPropertyName = "Pattern";
			this.columnPattern.HeaderText = "Pattern";
			this.columnPattern.Name = "columnPattern";
			this.columnPattern.Width = 200;
			// 
			// columnPatternType
			// 
			this.columnPatternType.DataPropertyName = "PatternType";
			this.columnPatternType.HeaderText = "Pattern Type";
			this.columnPatternType.MinimumWidth = 100;
			this.columnPatternType.Name = "columnPatternType";
			// 
			// columnIncludeSvnFolders
			// 
			this.columnIncludeSvnFolders.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.columnIncludeSvnFolders.DataPropertyName = "IncludeSvnFolders";
			this.columnIncludeSvnFolders.HeaderText = "Include SVN Folders";
			this.columnIncludeSvnFolders.Name = "columnIncludeSvnFolders";
			this.columnIncludeSvnFolders.Width = 80;
			// 
			// gridReplacePatterns
			// 
			this.gridReplacePatterns.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridReplacePatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridReplacePatterns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnFind,
            this.columnReplace,
            this.columnReplaceType});
			this.gridReplacePatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridReplacePatterns.Location = new System.Drawing.Point(0, 0);
			this.gridReplacePatterns.Name = "gridReplacePatterns";
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridReplacePatterns.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.gridReplacePatterns.Size = new System.Drawing.Size(835, 310);
			this.gridReplacePatterns.TabIndex = 0;
			// 
			// columnFind
			// 
			this.columnFind.DataPropertyName = "Find";
			this.columnFind.HeaderText = "Find";
			this.columnFind.Name = "columnFind";
			this.columnFind.Width = 300;
			// 
			// columnReplace
			// 
			this.columnReplace.DataPropertyName = "Replace";
			this.columnReplace.HeaderText = "Replace";
			this.columnReplace.Name = "columnReplace";
			this.columnReplace.Width = 300;
			// 
			// columnReplaceType
			// 
			this.columnReplaceType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.columnReplaceType.DataPropertyName = "ReplaceType";
			this.columnReplaceType.HeaderText = "Replace Type";
			this.columnReplaceType.MinimumWidth = 150;
			this.columnReplaceType.Name = "columnReplaceType";
			this.columnReplaceType.Width = 150;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonNew);
			this.panel1.Controls.Add(this.buttonOpen);
			this.panel1.Controls.Add(this.buttonSave);
			this.panel1.Controls.Add(this.buttonReplace);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 544);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(835, 38);
			this.panel1.TabIndex = 1;
			// 
			// buttonReplace
			// 
			this.buttonReplace.Location = new System.Drawing.Point(3, 12);
			this.buttonReplace.Name = "buttonReplace";
			this.buttonReplace.Size = new System.Drawing.Size(75, 23);
			this.buttonReplace.TabIndex = 0;
			this.buttonReplace.Text = "&Replace";
			this.buttonReplace.UseVisualStyleBackColor = true;
			this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(85, 12);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(75, 23);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "&Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// buttonOpen
			// 
			this.buttonOpen.Location = new System.Drawing.Point(166, 12);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(75, 23);
			this.buttonOpen.TabIndex = 1;
			this.buttonOpen.Text = "&Open";
			this.buttonOpen.UseVisualStyleBackColor = true;
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonNew
			// 
			this.buttonNew.Location = new System.Drawing.Point(247, 12);
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Size = new System.Drawing.Size(75, 23);
			this.buttonNew.TabIndex = 1;
			this.buttonNew.Text = "&New";
			this.buttonNew.UseVisualStyleBackColor = true;
			this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(835, 582);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.panel1);
			this.Name = "MainForm";
			this.Text = "Replacer";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridFilePaths)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridReplacePatterns)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView gridFilePaths;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnRootPath;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnPattern;
		private System.Windows.Forms.DataGridViewComboBoxColumn columnPatternType;
		private System.Windows.Forms.DataGridViewCheckBoxColumn columnIncludeSvnFolders;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonReplace;
		private System.Windows.Forms.DataGridView gridReplacePatterns;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnFind;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnReplace;
		private System.Windows.Forms.DataGridViewComboBoxColumn columnReplaceType;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonNew;
	}
}

