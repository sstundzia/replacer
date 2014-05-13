using System;

namespace Replacer.Business.Crawler
{
	[Serializable]
	public class FileFolderPath
	{
		/// <summary>
		/// Gets or sets the root directory/file path.
		/// </summary>
		/// <value>The file path.</value>
		public string RootPath { get; set; }

		/// <summary>
		/// Gets or sets the pattern.
		/// </summary>
		/// <value>The pattern.</value>
		public string Pattern { get; set; }

		/// <summary>
		/// Gets or sets the type of the pattern.
		/// </summary>
		/// <value>The type of the pattern.</value>
		public FileNamePatternType PatternType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether include SVN folders.
		/// </summary>
		/// <value><c>true</c> if include SVN folders; otherwise, <c>false</c>.</value>
		public bool IncludeSvnFolders { get; set; }

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="FileFolderPath"/> class.
		/// </summary>
		public FileFolderPath()
		{
			this.PatternType = FileNamePatternType.FileName;
		}

		#endregion
	}
}