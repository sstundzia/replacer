namespace Replacer.Business.Engine
{
	public interface IReplacerObserver
	{
		/// <summary>
		/// Receives the log message.
		/// </summary>
		/// <param name="message">The message.</param>
		void ReceiveLogMessage(LogMessage message);

		/// <summary>
		/// Files the processed.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		/// <param name="sourceText">The source text.</param>
		/// <param name="resultText">The result text.</param>
		/// <returns>Whether file should be saved</returns>
		bool FileProcessed(string fileFullName, string sourceText, string resultText);

		/// <summary>
		/// Files the processing.
		/// </summary>
		/// <param name="fileFullName">Full name of the file.</param>
		void FileProcessing(string fileFullName);
	}
}