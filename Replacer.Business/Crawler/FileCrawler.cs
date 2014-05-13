using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Replacer.Business.Crawler
{
	public class FileCrawler
	{
		#region OBSERVERS

		private readonly List<IFileCrawlerObserver> observers = new List<IFileCrawlerObserver>();

		/// <summary>
		/// Adds the observer.
		/// </summary>
		/// <param name="observer">The observer.</param>
		public void AddObserver(IFileCrawlerObserver observer)
		{
			if (observer != null && !this.observers.Contains(observer))
				this.observers.Add(observer);
		}

		/// <summary>
		/// Removes the observer.
		/// </summary>
		/// <param name="observer">The observer.</param>
		public void RemoveObserver(IFileCrawlerObserver observer)
		{
			if (this.observers.Contains(observer))
				this.observers.Remove(observer);
		}

		#endregion

		#region CRAWL

		/// <summary>
		/// Crawls the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public void Crawl(FileCrawlerParameters parameters)
		{
			// Reset cancelled flag:
			this.ResetCancelled();

			// Don't crawl if no observers exist:
			if (this.observers.Count > 0)
			{
				List<FileFolderPath> paths = parameters.PathInfoList;
				int pathsCount = paths.Count;

				for (int i = 0; i < pathsCount; i++)
				{
					// Check cancelled:
					if (this.IsCancelled())
						break;

					FileFolderPath path = paths[i];

					// Notify crawling started:
					this.NotifyFileCrawlingStarted(path);

					try
					{
						// Check if the specified path is actually a file:
						if (File.Exists(path.RootPath))
						{
							this.NotifyProcessFile(path, path.RootPath);
						}
						else if (Directory.Exists(path.RootPath))
						{
							// Create regex parser if required:
							Regex regex = null;
							if (!String.IsNullOrWhiteSpace(path.Pattern))
								regex = new Regex(path.Pattern);

							// Get directory info:
							DirectoryInfo info = new DirectoryInfo(path.RootPath);
							this.ProcessDirectory(path, info, regex);
						}
					}
					catch (Exception exc)
					{
						// Notify error:
						this.NotifyErrorOccurred(path, exc);
					}
					finally
					{
						// Notify path crawling finished:
						this.NotifyFileCrawlingFinished(path);
					}
				}
			}
		}

		/// <summary>
		/// Processes the directory.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="info">The info.</param>
		/// <param name="regex">The regex.</param>
		private void ProcessDirectory(FileFolderPath path, DirectoryInfo info, Regex regex)
		{
			// Iterate files:
			FileInfo[] files = info.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				// Check cancelled:
				if (this.IsCancelled())
					break;

				try
				{
					if (regex != null)
					{
						// Resolve correct file name:
						string fileName = String.Empty;
						switch (path.PatternType)
						{
							case FileNamePatternType.FileName:
								fileName = fileInfo.Name;
								break;
							case FileNamePatternType.FileFullName:
								fileName = fileInfo.FullName;
								break;
						}

						// Check against regex:
						if (!string.IsNullOrEmpty(fileName) && !regex.Match(fileName).Success)
							continue;
					}

					// Process file:
					this.NotifyProcessFile(path, fileInfo.FullName);
				}
				catch (Exception exc)
				{
					// Notify error:
					this.NotifyErrorOccurred(path, exc);
				}
			}

			// Iterate folders:
			DirectoryInfo[] directories = info.GetDirectories();
			foreach (DirectoryInfo directory in directories)
			{
				// Check cancelled:
				if (this.IsCancelled())
					break;

				try
				{
					// Include/Exclude folders that have ".svn" or "_svn" as their name.
					if (path.IncludeSvnFolders || (String.Compare(directory.Name, ".svn", true) != 0 && String.Compare(directory.Name, "_svn", true) != 0))
					{
						if (regex != null)
						{
							// Resolve correct folder name:
							string folderName = String.Empty;
							switch (path.PatternType)
							{
								case FileNamePatternType.FolderName:
									folderName = directory.Name;
									break;
								case FileNamePatternType.FolderFullPath:
									folderName = directory.FullName;
									break;
							}

							// Check against regex:
							if (!string.IsNullOrEmpty(folderName) && !regex.Match(folderName).Success)
								continue;
						}

						// Crawl through child folder:
						this.ProcessDirectory(path, directory, regex);
					}
				}
				catch (Exception exc)
				{
					// Notify error:
					this.NotifyErrorOccurred(path, exc);
				}
			}
		}

		#endregion

		#region NOTIFY

		/// <summary>
		/// Notifies the process file.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="filePath">The file path.</param>
		private void NotifyProcessFile(FileFolderPath pathInfo, string filePath)
		{
			int count = this.observers.Count;
			for (int i = 0; i < count; i++)
			{
				this.observers[i].ProcessFile(pathInfo, filePath);
			}
		}

		/// <summary>
		/// Notifies the file crawling started.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		private void NotifyFileCrawlingStarted(FileFolderPath pathInfo)
		{
			int count = this.observers.Count;
			for (int i = 0; i < count; i++)
			{
				this.observers[i].FileCrawlingStarted(pathInfo);
			}
		}

		/// <summary>
		/// Notifies the file crawling finished.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		private void NotifyFileCrawlingFinished(FileFolderPath pathInfo)
		{
			int count = this.observers.Count;
			for (int i = 0; i < count; i++)
			{
				this.observers[i].FileCrawlingFinished(pathInfo);
			}
		}

		/// <summary>
		/// Notifies the error occurred.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="exc">The exc.</param>
		private void NotifyErrorOccurred(FileFolderPath pathInfo, Exception exc)
		{
			int count = this.observers.Count;
			for (int i = 0; i < count; i++)
			{
				this.observers[i].ErrorOccurred(pathInfo, exc);
			}
		}

		#endregion

		#region CANCEL

		private bool cancelled;
		private readonly object lockObject = new object();

		/// <summary>
		/// Cancels the crawling.
		/// </summary>
		public void CancelCrawling()
		{
			lock (lockObject)
			{
				this.cancelled = true;
			}
		}

		/// <summary>
		/// Determines whether crawling is cancelled.
		/// </summary>
		protected bool IsCancelled()
		{
			lock (lockObject)
			{
				return this.cancelled;
			}
		}

		/// <summary>
		/// Resets the cancelled.
		/// </summary>
		protected void ResetCancelled()
		{
			lock (lockObject)
			{
				this.cancelled = false;
			}
		}

		#endregion
	}
}