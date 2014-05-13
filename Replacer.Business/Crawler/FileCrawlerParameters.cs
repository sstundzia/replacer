using System.Collections.Generic;

namespace Replacer.Business.Crawler
{
	public class FileCrawlerParameters
	{
		#region PROPERTIES

		/// <summary>
		/// Gets or sets the path info list.
		/// </summary>
		/// <value>The path info list.</value>
		public List<FileFolderPath> PathInfoList { get; set; }

		#endregion

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="FileCrawlerParameters"/> class.
		/// </summary>
		public FileCrawlerParameters()
		{
			this.PathInfoList = new List<FileFolderPath>();
		}

		#endregion
	}
}