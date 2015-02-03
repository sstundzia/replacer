using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Replacer.Business;
using Replacer.Business.Crawler;
using Replacer.Business.Engine;

namespace Replacer.GUI
{
	public partial class MainForm : Form
	{
		#region FIELDS

		private readonly BindingList<FileFolderPath> pathList = new BindingList<FileFolderPath>();
		private readonly BindingList<ReplacePattern> patternList = new BindingList<ReplacePattern>();

		#endregion

		#region INNER TYPES

	    private class ValueItem
		{
			public object Value { get; set; }
			public string DisplayText { get; set; }
		}

		#endregion

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
			this.Init();
		}

		#endregion

		#region INIT

		/// <summary>
		/// Inits this instance.
		/// </summary>
		private void Init()
		{
			this.columnPatternType.DataSource =
				new[]
					{
						new ValueItem
							{
								Value = FileNamePatternType.FileName,
								DisplayText = "File name"
							},
						new ValueItem
							{
								Value = FileNamePatternType.FileFullName,
								DisplayText = "File full name"
							},
						new ValueItem
							{
								Value = FileNamePatternType.FolderName,
								DisplayText = "Folder name"
							},
						new ValueItem
							{
								Value = FileNamePatternType.FolderFullPath,
								DisplayText = "Folder full path"
							}
					};
			// Setup value list for PatternType column:
			this.columnPatternType.ValueMember = "Value";
			this.columnPatternType.DisplayMember = "DisplayText";

			// Setup value list for ReplaceType column:
			this.columnReplaceType.DataSource =
				new[]
					{
						new ValueItem
							{
								Value = ReplaceType.Regex,
								DisplayText = "Regular Expression"
							},
						new ValueItem
							{
								Value = ReplaceType.Simple,
								DisplayText = "Simple Replace"
							}
					};
			this.columnReplaceType.ValueMember = "Value";
			this.columnReplaceType.DisplayMember = "DisplayText";

			this.gridFilePaths.DataSource = this.pathList;
			this.gridReplacePatterns.DataSource = this.patternList;
		}

		#endregion

		#region NEW PROJECT

		/// <summary>
		/// News the project.
		/// </summary>
		private void NewProject()
		{
			// Save project:
			this.SaveProject();

			// Clear project:
			this.pathList.Clear();
			this.patternList.Clear();
		}

		#endregion

		#region SAVE PROJECT

		/// <summary>
		/// Saves the project.
		/// </summary>
		private void SaveProject()
		{
			// Setup dialog:
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.AddExtension = true;
				dialog.DefaultExt = "rpproj";
				dialog.Filter = @"Replacer project (*.rpproj)|*.rpproj|All files (*.*)|*.*";
				dialog.OverwritePrompt = true;

				// Show dialog:
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					// Serialize project:
					ReplacerProject project = new ReplacerProject();
					project.FileFolderPaths = this.pathList.ToArray();
					project.PatternList = this.patternList.ToArray();
					XmlSerializer serializer = new XmlSerializer(typeof(ReplacerProject));

					Stream saveFileStream = dialog.OpenFile();
					serializer.Serialize(saveFileStream, project);
					saveFileStream.Close();
				}
			}
		}

		#endregion

		#region OPEN PROJECT

		private void OpenProject()
		{
			// Setup dialog:
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.AddExtension = true;
				dialog.DefaultExt = "rpproj";
				dialog.Filter = @"Replacer project (*.rpproj)|*.rpproj|All files (*.*)|*.*";
				
				// Show dialog:
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					Stream openFileStream = new FileStream(dialog.FileName, FileMode.Open);
					XmlSerializer serializer = new XmlSerializer(typeof (ReplacerProject));
					XmlReader reader = new XmlTextReader(openFileStream);
					if (!serializer.CanDeserialize(reader))
					{
						MessageBox.Show(this, "Invalid Project file", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						ReplacerProject project = (ReplacerProject) serializer.Deserialize(reader);
						this.pathList.Clear();
						foreach (FileFolderPath path in project.FileFolderPaths)
						{
							this.pathList.Add(path);
						}
						this.patternList.Clear();
						foreach (ReplacePattern pattern in project.PatternList)
						{
							this.patternList.Add(pattern);
						}
					}
					openFileStream.Close();
				}
			}
		}

		#endregion

		#region EVENT HANDLERS

		/// <summary>
		/// Handles the Click event of the buttonReplace control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void buttonReplace_Click(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;
			try
			{
				// Set wait cursor:
				Cursor.Current = Cursors.WaitCursor;

				// Process:
				using (ProcessingForm form = new ProcessingForm())
				{
					form.ReplacePatterns = this.patternList;
                    form.FileCrawlerParameters = new FileCrawlerParameters{PathInfoList = this.pathList.ToList()};
					form.ShowDialog(this);
				}
			}
			finally
			{
				// Restore original cursor:
				Cursor.Current = currentCursor;
			}
		}

		/// <summary>
		/// Handles the Click event of the buttonSave control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		private void buttonSave_Click(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;
			try
			{
				// Set wait cursor:
				Cursor.Current = Cursors.WaitCursor;

				// Save project:
				this.SaveProject();
			}
			finally
			{
				// Restore original cursor:
				Cursor.Current = currentCursor;
			}
		}

		/// <summary>
		/// Handles the Click event of the buttonOpen control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		private void buttonOpen_Click(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;
			try
			{
				// Set wait cursor:
				Cursor.Current = Cursors.WaitCursor;

				// Save project:
				this.OpenProject();
			}
			finally
			{
				// Restore original cursor:
				Cursor.Current = currentCursor;
			}
		}

		/// <summary>
		/// Handles the Click event of the buttonNew control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		private void buttonNew_Click(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;
			try
			{
				// Set wait cursor:
				Cursor.Current = Cursors.WaitCursor;

				// Clear project:
				this.NewProject();
			}
			finally
			{
				// Restore original cursor:
				Cursor.Current = currentCursor;
			}
		}

		#endregion

		
	}
}
