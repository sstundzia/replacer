using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Replacer.Business;
using Replacer.Business.Engine;

namespace Replacer.Console
{
	public class CommandLineRunner : IReplacerObserver
	{
		#region FIELDS

		private DateTime fileProcessingStartTime;
		private TextWriter logFile;
		private bool logToConsole;

		#endregion

		#region RUN

		/// <summary>
		/// Runs the specified args.
		/// </summary>
		/// <param name="args">The args.</param>
		public void Run(string[] args)
		{
			// Parse command line args:
			CommandLineArgsParser parser = new CommandLineArgsParser();
			string errorMessage;
			if (!parser.ParseArguments(args, out errorMessage))
			{
				if (!string.IsNullOrWhiteSpace(errorMessage))
				{
					System.Console.WriteLine(@"ERROR: " + errorMessage);
					System.Console.WriteLine();
				}
				parser.PrintHelp();
				return;
			}

			try
			{
				// Read Project File:
				Stream openFileStream = new FileStream(parser.ProjectPath, FileMode.Open);
				XmlSerializer serializer = new XmlSerializer(typeof (ReplacerProject));
				XmlReader reader = new XmlTextReader(openFileStream);
				if (!serializer.CanDeserialize(reader))
				{
					System.Console.WriteLine(@"Invalid Project file");
					openFileStream.Close();
					return;
				}

				ReplacerProject project = (ReplacerProject) serializer.Deserialize(reader);
				openFileStream.Close();

				// Setup logging:
				this.logToConsole = parser.LogToConsole;
				if (!string.IsNullOrWhiteSpace(parser.LogFilePath))
				{
					this.logFile = new StreamWriter(parser.LogFilePath, true, Encoding.UTF8);
				}

				// Process:
				// Create replace parameters:
				ReplaceParameters parameters = new ReplaceParameters
				{
					FileCrawlerParameters = {PathInfoList = project.FileFolderPaths.ToList()},
					ReplacePatterns = project.PatternList.ToList()
				};

				// Initialize replacer:
				Business.Engine.Replacer replacer = new Business.Engine.Replacer();
				replacer.AddObserver(this);

				// Start process:
				replacer.Replace(parameters);
			}
			catch (Exception exc)
			{
				if (!this.logToConsole)
					System.Console.WriteLine(exc);
				this.Log(new LogMessage
				{
					TimeStamp = DateTime.Now,
					Message = exc.ToString()
				});
			}
			finally
			{
				if (this.logFile != null)
					this.logFile.Close();
			}
		}

		#endregion

		#region REPLACER OBSERVER

		/// <summary>
		/// Receives the log message.
		/// </summary>
		/// <param name="message">The message.</param>
		void IReplacerObserver.ReceiveLogMessage(LogMessage message)
		{
			this.Log(message);
		}

		/// <summary>
		/// Files the processed.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		/// <param name="sourceText">The source text.</param>
		/// <param name="resultText">The result text.</param>
		/// <returns>
		/// Whether file should be saved
		/// </returns>
		bool IReplacerObserver.FileProcessed(string fileFullName, string sourceText, string resultText)
		{
			DateTime time = DateTime.Now;
			TimeSpan timeTaken = time - this.fileProcessingStartTime;
			LogMessage processingMessage = new LogMessage
			{
				TimeStamp = time,
				Message = string.Format("File processed in {0:hh\\:mm\\:ss\\.fff} : {1}",
					timeTaken,
					fileFullName)
			};
			this.Log(processingMessage);
			return !string.Equals(sourceText, resultText, StringComparison.Ordinal);
		}

		/// <summary>
		/// Files the processing.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		void IReplacerObserver.FileProcessing(string fileFullName)
		{
			this.fileProcessingStartTime = DateTime.Now;
		}

		#endregion

		#region LOG

		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		private void Log(LogMessage message)
		{
			if (this.logToConsole)
				System.Console.WriteLine(message);
			if (this.logFile != null)
			{
				this.logFile.WriteLine(message.ToString());
				this.logFile.Flush();
			}
		}

		#endregion


	}
}