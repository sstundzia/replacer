using System.Collections.Generic;

using Replacer.Business.Crawler;

namespace Replacer.Business.Engine
{
	public class ReplaceParameters
	{
		#region PROPERTIES

		/// <summary>
		/// Gets or sets the file crawler parameters.
		/// </summary>
		/// <value>The file crawler parameters.</value>
		public FileCrawlerParameters FileCrawlerParameters { get; set; }

		/// <summary>
		/// Gets or sets the replace patterns.
		/// </summary>
		/// <value>The replace patterns.</value>
		public List<ReplacePattern> ReplacePatterns { get; set; }

		#endregion

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceParameters"/> class.
		/// </summary>
		public ReplaceParameters()
		{
			this.FileCrawlerParameters = new FileCrawlerParameters();
			this.ReplacePatterns = new List<ReplacePattern>();
		}

		#endregion
	}
}
