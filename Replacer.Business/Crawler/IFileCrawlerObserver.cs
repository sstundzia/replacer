using System;

namespace Replacer.Business.Crawler
{
	public interface IFileCrawlerObserver
	{
		/// <summary>
		/// Processes the file.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="filePath">The file path.</param>
		void ProcessFile(FileFolderPath pathInfo, string filePath);

		/// <summary>
		/// Files the crawling started.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		void FileCrawlingStarted(FileFolderPath pathInfo);

		/// <summary>
		/// Files the crawling finished.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		void FileCrawlingFinished(FileFolderPath pathInfo);

		/// <summary>
		/// Notifies that exception has occurred.
		/// </summary>
		/// <param name="pathInfo">The path info.</param>
		/// <param name="exc">The exc.</param>
		void ErrorOccurred(FileFolderPath pathInfo, Exception exc);
	}
}