using System;

namespace Replacer.Business.Engine
{
	[Serializable]
	public class ReplacePattern
	{
		#region PROPERTIES

		/// <summary>
		/// Gets or sets the find pattern.
		/// </summary>
		/// <value>The find pattern.</value>
		public string Find { get; set; }

		/// <summary>
		/// Gets or sets the replace pattern.
		/// </summary>
		/// <value>The replace pattern.</value>
		public string Replace { get; set; }

		/// <summary>
		/// Gets or sets the type of the replace.
		/// </summary>
		/// <value>The type of the replace.</value>
		public ReplaceType ReplaceType { get; set; }

		#endregion

		#region CONSTRUCTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplacePattern"/> class.
		/// </summary>
		public ReplacePattern()
		{
			this.ReplaceType = ReplaceType.Regex;
		}

		#endregion
	}
}