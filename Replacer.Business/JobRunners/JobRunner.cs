using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Replacer.Business.Crawler;
using Replacer.Business.Engine;

namespace Replacer.Business.JobRunners
{
    public class JobRunner: IFileCrawlerObserver
    {
        #region FIELDS

        private readonly ReplacerEngine replacer;
        private readonly FileCrawler crawler;
        private readonly FileCrawlerParameters fileCrawlerParameters;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="replacePatternList"></param>
        /// <param name="fileCrawlerParameters"></param>
        public JobRunner(FileCrawlerParameters fileCrawlerParameters, IEnumerable<ReplacePattern> replacePatternList)
        {
            this.fileCrawlerParameters = fileCrawlerParameters;
            this.replacer = new ReplacerEngine(replacePatternList);
            this.crawler = new FileCrawler();
            this.crawler.AddObserver(this);
        }

        #endregion

        #region RUN

        /// <summary>
        /// Runs the replace job.
        /// </summary>
        public void Run()
        {
            this.crawler.Crawl(this.fileCrawlerParameters);
        }

        #endregion

        #region CANCEL

        /// <summary>
        /// Cancels the replace job.
        /// </summary>
        public void Cancel()
        {
            this.crawler.CancelCrawling();
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

                string output = this.replacer.ProcessString(input);

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
        private readonly List<IJobRunnerObserver> observers = new List<IJobRunnerObserver>();

        /// <summary>
        /// Adds the observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void AddObserver(IJobRunnerObserver observer)
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
        public void RemoveObserver(IJobRunnerObserver observer)
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
            this.NotifyMessage(filePath, "Error: " + exc.Message, exc.ToString());
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
                foreach (IJobRunnerObserver observer in this.observers)
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
                    IJobRunnerObserver observer = this.observers[i];
                    result &= observer.FileProcessed(fileFullName, sourceText, resultText);
                }
            }
            return result;
        }

        #endregion
    }
}