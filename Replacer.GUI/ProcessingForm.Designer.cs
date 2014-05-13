namespace Replacer.GUI
{
	partial class ProcessingForm
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.listMessages = new System.Windows.Forms.DataGridView();
			this.columnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.listMessages)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 432);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(683, 29);
			this.panel1.TabIndex = 0;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.Location = new System.Drawing.Point(605, 3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.progressBar1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(683, 27);
			this.panel2.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Processing. Please wait...";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(134, 0);
			this.progressBar1.MarqueeAnimationSpeed = 30;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(546, 23);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 0;
			// 
			// listMessages
			// 
			this.listMessages.AllowUserToAddRows = false;
			this.listMessages.AllowUserToDeleteRows = false;
			this.listMessages.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.listMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnMessage});
			this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listMessages.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.listMessages.Location = new System.Drawing.Point(0, 27);
			this.listMessages.Name = "listMessages";
			this.listMessages.ReadOnly = true;
			this.listMessages.RowHeadersVisible = false;
			this.listMessages.Size = new System.Drawing.Size(683, 405);
			this.listMessages.TabIndex = 2;
			// 
			// columnMessage
			// 
			this.columnMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnMessage.DataPropertyName = "AsString";
			this.columnMessage.HeaderText = "Message";
			this.columnMessage.Name = "columnMessage";
			this.columnMessage.ReadOnly = true;
			this.columnMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ProcessingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 461);
			this.Controls.Add(this.listMessages);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "ProcessingForm";
			this.Text = "Processing...";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.listMessages)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.DataGridView listMessages;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnMessage;
	}
}