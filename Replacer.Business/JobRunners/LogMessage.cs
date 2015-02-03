using System;

namespace Replacer.Business.JobRunners
{
	public class LogMessage
	{
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the details.
		/// </summary>
		/// <value>The details.</value>
		public string Details { get; set; }

		/// <summary>
		/// Gets or sets the time stamp.
		/// </summary>
		/// <value>The time stamp.</value>
		public DateTime TimeStamp { get; set; }

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="LogMessage"/> class.
		/// </summary>
		public LogMessage()
		{
			this.TimeStamp = DateTime.Now;
		}

		#endregion

		#region TO STRING

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("[{0:HH:mm:ss}] {1}",
			                     this.TimeStamp,
			                     this.Message);
		}

		/// <summary>
		/// Gets as string.
		/// </summary>
		/// <value>
		/// As string.
		/// </value>
		public string AsString
		{
			get { return this.ToString(); }
		}

		#endregion
	}
}