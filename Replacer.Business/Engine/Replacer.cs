using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Replacer.Business.Crawler;

using System.Linq;

namespace Replacer.Business.Engine
{
	public class Replacer : IFileCrawlerObserver
	{
		#region FIELDS

		private List<ReplacePatternWrapper> wrappedPatterns;
		private FileCrawler crawler;

		#endregion

		#region INNER TYPES

		private class ReplacePatternWrapper
		{
			#region FIELDS

			private readonly ReplacePattern pattern;
			private readonly Regex regex;
			private readonly List<ReplacePatternSmartTag> usedTags = new List<ReplacePatternSmartTag>();
			private readonly List<ReplacePatternSmartTag> fileUniqueTags = new List<ReplacePatternSmartTag>();
			private readonly List<ReplacePatternSmartTag> matchUniqueTags = new List<ReplacePatternSmartTag>();
			private readonly string constantReplacePattern;

			#endregion

			#region CONSTRUCTOR

			/// <summary>
			/// Initializes a new instance of the <see cref="ReplacePatternWrapper"/> class.
			/// </summary>
			/// <param name="pattern">The pattern.</param>
			public ReplacePatternWrapper(ReplacePattern pattern)
			{
				this.pattern = pattern;
				this.constantReplacePattern = this.pattern.Replace;

				// Process regex type:
				if (pattern.ReplaceType == ReplaceType.Regex)
				{
					// Initialize regex object:
					this.regex = new Regex(pattern.Find, RegexOptions.Compiled);

					// Collect all tags:
					if (!string.IsNullOrWhiteSpace(pattern.Replace))
					{
						// Collect all tags:
                        MatchCollection matches = new Regex(@"\$\{\<(?<TagName>\w+)(?>\((?<Options>(?>(?<Paren>\()|(?<-Paren>\))|[^\(\)]+)*)\))?\>\}", RegexOptions.CultureInvariant).Matches(pattern.Replace);
						ReplacePatternSmartTag tag;
						foreach (Match match in matches)
						{
							if (AllAvailableSmartTags.Tags.TryGetValue(match.Groups["TagName"].Value, out tag))
							{
								if (!this.usedTags.Contains(tag))
									this.usedTags.Add(tag);
							}
						}
					}
				}
			}

			#endregion

			#region PROCESS PATTERN

			/// <summary>
			/// Processes the pattern.
			/// </summary>
			/// <param name="sourceString">The source string.</param>
			/// <returns></returns>
			public string ProcessPattern(string sourceString)
			{
				string result;
				if (this.regex != null)
				{
					string replacePattern = this.constantReplacePattern;
					if (this.usedTags.Count > 0)
					{
						result = this.regex.Replace(
							sourceString,
							m => m.Result(this.usedTags.Aggregate(replacePattern,
							                                             (current, tag) => tag.ApplyTag(current, m))));
					}
					else
						result = this.regex.Replace(sourceString, replacePattern ?? string.Empty);
				}
				else
					result = sourceString.Replace(this.pattern.Find, this.pattern.Replace ?? string.Empty);
				return result;
			}

			#endregion
		}

		#endregion

		#region REPLACE

		/// <summary>
		/// Replaces the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public void Replace(ReplaceParameters parameters)
		{
			// Validate:
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			if (parameters.FileCrawlerParameters == null)
				throw new ArgumentException("parameters.FileCrawlerParameters is null");

			if (parameters.FileCrawlerParameters.PathInfoList.Count == 0)
			{
				this.NotifyMessage(null, "Error: No files to crawl.");
				return;
			}

			if (parameters.ReplacePatterns.Count == 0)
			{
				this.NotifyMessage(null, "Error: No replace patterns specified.");
				return;
			}

            // Reset tags:
		    AllAvailableSmartTags.Reset();

			// Wrap patterns:
			this.wrappedPatterns = parameters.ReplacePatterns
				.Select(p => new ReplacePatternWrapper(p))
				.ToList();

			// Process:
			this.crawler = new FileCrawler();
			this.crawler.AddObserver(this);
			this.crawler.Crawl(parameters.FileCrawlerParameters);
			this.crawler.RemoveObserver(this);

			// Clean up:
			this.wrappedPatterns = null;
		}

		#endregion

		#region IFileCrawlerObserver

		/// <summary>
		/// Processes the file.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="filePath">The file path.</param>
		void IFileCrawlerObserver.ProcessFile(FileFolderPath pathInfo, string filePath)
		{
			try
			{
				// Notify processing file:
				this.NotifyFileProcessing(filePath);

				// Read file:
				//string input = File.ReadAllText(filePath);
				string input;
				Encoding detectedEncoding;
				using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
				{
					input = reader.ReadToEnd();
					detectedEncoding = reader.CurrentEncoding;
				}

				// Reset file tags:
				AllAvailableSmartTags.ResetFileTags();

				// Apply replacing:
				string output = input;
				int count = this.wrappedPatterns.Count;
				for (int i = 0; i < count; i++)
				{
					output = this.wrappedPatterns[i].ProcessPattern(output);
				}

				// Notify file processed:
				bool shouldSave = this.NotifyFileProcessed(filePath, input, output);

				// Save file:
				if (shouldSave)
				{
					//File.WriteAllText(filePath, output, Encoding.ASCII);
					File.WriteAllText(filePath, output, detectedEncoding);
				}
			}
			catch (Exception e)
			{
				this.NotifyMessage(filePath, String.Format("Exception in file {0} : {1}", filePath, e));
			}
		}

		/// <summary>
		/// Files the crawling started.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		void IFileCrawlerObserver.FileCrawlingStarted(FileFolderPath pathInfo)
		{
			this.NotifyMessage(pathInfo.RootPath, "Crawling started.");
		}

		/// <summary>
		/// Files the crawling finished.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		void IFileCrawlerObserver.FileCrawlingFinished(FileFolderPath pathInfo)
		{
			this.NotifyMessage(pathInfo.RootPath, "Crawling finished.");
		}

		/// <summary>
		/// Notifies that exception has occurred.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="exc">The exc.</param>
		void IFileCrawlerObserver.ErrorOccurred(FileFolderPath pathInfo, Exception exc)
		{
			this.NotifyMessage(pathInfo.RootPath, exc);
		}

		#endregion

		#region OBSERVERS

		/// <summary>
		/// List of registered observers.
		/// </summary>
		private readonly List<IReplacerObserver> observers = new List<IReplacerObserver>();

		/// <summary>
		/// Adds the observer.
		/// </summary>
		/// <param name="observer">The observer.</param>
		public void AddObserver(IReplacerObserver observer)
		{
			if (!this.observers.Contains(observer))
			{
				this.observers.Add(observer);
			}
		}

		/// <summary>
		/// Removes the observer.
		/// </summary>
		/// <param name="observer">The observer.</param>
		public void RemoveObserver(IReplacerObserver observer)
		{
			if (this.observers.Contains(observer))
			{
				this.observers.Remove(observer);
			}
		}

		#endregion

		#region NOTIFY

		/// <summary>
		/// Notifies the message.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="exc">The exc.</param>
		private void NotifyMessage(string filePath, Exception exc)
		{
			NotifyMessage(filePath, "Error: " + exc.Message, exc.ToString());
		}

		/// <summary>
		/// Notifies the message.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="message">The message.</param>
		/// <param name="details">The details.</param>
		private void NotifyMessage(string filePath, string message, string details = null)
		{
			if (this.observers.Count > 0)
			{
				foreach (IReplacerObserver observer in observers)
				{
					observer.ReceiveLogMessage(new LogMessage { FileName = filePath, Message = message, Details = details });
				}
			}
		}

		/// <summary>
		/// Notifies the file processing.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		/// <returns></returns>
		private void NotifyFileProcessing(string fileFullName)
		{
			if (this.observers.Count > 0)
			{
				for (int i = 0; i < this.observers.Count; i++)
				{
					this.observers[i].FileProcessing(fileFullName);
				}
			}
		}

		/// <summary>
		/// Notifies the file processed.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		/// <param name="sourceText">The source text.</param>
		/// <param name="resultText">The result text.</param>
		/// <returns></returns>
		private bool NotifyFileProcessed(string fileFullName, string sourceText, string resultText)
		{
			bool result = true;
			if (this.observers.Count > 0)
			{
				for (int i = 0; i < this.observers.Count; i++)
				{
					IReplacerObserver observer = this.observers[i];
					result &= observer.FileProcessed(fileFullName, sourceText, resultText);
				}
			}
			return result;
		}

		#endregion

		#region CANCEL

		/// <summary>
		/// Cancels the replacing process.
		/// </summary>
		public void Cancel()
		{
			if (this.crawler != null)
			{
				this.crawler.CancelCrawling();
				this.NotifyMessage(null, "[Processing break queued: User Cancelled]");
			}
		}

		#endregion
	}
}