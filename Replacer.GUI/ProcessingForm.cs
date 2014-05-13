using System;
using System.ComponentModel;
using System.Windows.Forms;

using Replacer.Business.Engine;

namespace Replacer.GUI
{
	public partial class ProcessingForm : Form, IReplacerObserver
	{
		#region FIELDS

		private Business.Engine.Replacer replacer;
		private bool finished = true;
	
		private readonly BindingList<LogMessage> messageList = new BindingList<LogMessage>();

		#endregion

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessingForm"/> class.
		/// </summary>
		public ProcessingForm()
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
			// Setup list box:
			this.listMessages.AutoGenerateColumns = false;
			this.listMessages.DataSource = this.messageList;
			
			// Mark as finished before processing starts:
			this.MarkFinished();
		}

		#endregion

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the replace parameters.
		/// </summary>
		/// <value>The replace parameters.</value>
		public ReplaceParameters ReplaceParameters { get; set; }

		#endregion

		#region ON SHOW

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Shown"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnShown(EventArgs e)
		{
			// Call base:
			base.OnShown(e);

			// Start process:
			this.StartProcess();
		}

		#endregion

		#region ON CLOSING

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			// Prevent closing window while processing:
			e.Cancel = !this.finished;
		}

		#endregion

		#region START PROCESS

		/// <summary>
		/// Starts the process.
		/// </summary>
		private void StartProcess()
		{
			// Initialize replacer:
			this.replacer = new Business.Engine.Replacer();
			this.replacer.AddObserver(this);
			this.MarkUnfinished();

			// Start process:
			ReplaceDelegate action = this.replacer.Replace;
			AsyncCallback callback = this.Callback;
			action.BeginInvoke(this.ReplaceParameters, callback, action);
		}

		/// <summary>
		/// Callbacks the specified result.
		/// </summary>
		/// <param name="result">The result.</param>
		private void Callback(IAsyncResult result)
		{
			try
			{
				// End invoke:
				ReplaceDelegate action = (ReplaceDelegate)result.AsyncState;
				action.EndInvoke(result);
			}
			catch (Exception exc)
			{
				// Log exception:
				this.AddMessage(new LogMessage
				                	{
				                		Message = "Error: " + exc.Message,
				                		Details = exc.ToString()
				                	});
			}
			finally
			{
				// Mark process as finished:
				this.MarkFinished();
			}
		}

		private delegate void ReplaceDelegate(ReplaceParameters parameters);

		#endregion

		#region ADD MESSAGE

		/// <summary>
		/// Adds the message.
		/// </summary>
		/// <param name="message">The message.</param>
		private void AddMessage(LogMessage message)
		{
			ISynchronizeInvoke sync = this;
			if (sync.InvokeRequired)
			{
				Action<LogMessage> action = this.AddMessageCore;
				sync.Invoke(action, new object[]{message});
			}
			else
			{
				this.AddMessageCore(message);
			}
		}

		/// <summary>
		/// Adds the message core.
		/// </summary>
		/// <param name="message">The message.</param>
		private void AddMessageCore(LogMessage message)
		{
			this.messageList.Add(message);
			if (this.listMessages.Rows.Count > 0 && (this.listMessages.FirstDisplayedScrollingRowIndex + this.listMessages.DisplayedRowCount(true)) >= this.listMessages.Rows.Count - 1)
			{
				this.listMessages.ClearSelection();
				this.listMessages.Rows[this.listMessages.Rows.Count - 1].Selected = true;
				this.listMessages.FirstDisplayedScrollingRowIndex = this.listMessages.Rows.Count - 1;
			}
		}

		#endregion

		#region IReplacerObserver MEMBERS

		/// <summary>
		/// Receives the log message.
		/// </summary>
		/// <param name="message">The message.</param>
		void IReplacerObserver.ReceiveLogMessage(LogMessage message)
		{
			this.AddMessage(message);
		}

		private LogMessage processingMessage;
		private DateTime fileProcessingStartTime;

		/// <summary>
		/// Files the processing.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		void IReplacerObserver.FileProcessing(string fileFullName)
		{
			this.fileProcessingStartTime = DateTime.Now;
			this.processingMessage = new LogMessage();
			this.processingMessage.FileName = fileFullName;
			this.processingMessage.Message =
				string.Format("Processing file: {0}",
				              fileFullName);
			this.AddMessage(this.processingMessage);
			
		}

		/// <summary>
		/// Files the processed.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		/// <param name="sourceText">The source text.</param>
		/// <param name="resultText">The result text.</param>
		/// <returns>Whether file should be saved</returns>
		bool IReplacerObserver.FileProcessed(string fileFullName, string sourceText, string resultText)
		{
			DateTime time = DateTime.Now;
			TimeSpan timeTaken = time - this.fileProcessingStartTime;
			this.processingMessage.TimeStamp = time;
			this.processingMessage.Message =
				string.Format("File processed in {0:hh\\:mm\\:ss\\.fff} : {1}",
				              timeTaken,
				              fileFullName);
			this.processingMessage = null;
			return !string.Equals(sourceText, resultText, StringComparison.Ordinal);
		}

		#endregion

		#region FINISHING

		/// <summary>
		/// Marks the unfinished.
		/// </summary>
		private void MarkUnfinished()
		{
			Action action = () =>
			                	{
			                		this.finished = false;
			                		this.buttonCancel.Text = "Cancel";
			                		this.progressBar1.Value = 0;
									this.progressBar1.Style = ProgressBarStyle.Marquee;
			                	};
			ISynchronizeInvoke sync = this;
			if (sync.InvokeRequired)
			{
				sync.Invoke(action, null);
			}
			else
			{
				action();
			}
		}

		/// <summary>
		/// Marks the finished.
		/// </summary>
		private void MarkFinished()
		{
			Action action = () =>
			                	{
			                		this.finished = true;
			                		this.buttonCancel.Text = "Close";
									this.progressBar1.Style = ProgressBarStyle.Blocks;
			                	};
			ISynchronizeInvoke sync = this;
			if (sync.InvokeRequired)
			{
				sync.Invoke(action, null);
			}
			else
			{
				action();
			}
		}

		#endregion

		#region EVENT HANDLERS

		/// <summary>
		/// Handles the Click event of the buttonCancel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			if (this.finished)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				this.replacer.Cancel();
			}
		}

		//
		#endregion
	}
}
