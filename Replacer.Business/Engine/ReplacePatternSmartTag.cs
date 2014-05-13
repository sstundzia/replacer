using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Replacer.Business.Engine
{
	public class ReplacePatternSmartTag
	{
		#region PROPERTIES

		public string Tag { get; private set; }
		public string Description { get; private set; }
		public SmartTagType Type { get; private set; }
		public Func<Match, string, string> TagResult { get; private set; }

		// Value cache:
		private readonly Dictionary<string, string> valueCache = new Dictionary<string, string>();

		private readonly Regex tagApplicationRegex;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplacePatternSmartTag" /> class.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="description">The description.</param>
		/// <param name="type">The type.</param>
		/// <param name="tagResult">The tag result.</param>
		public ReplacePatternSmartTag(string tag, string description, SmartTagType type, Func<string> tagResult)
			: this(tag, description, type, (m, p) => tagResult())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplacePatternSmartTag" /> class.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="description">The description.</param>
		/// <param name="type">The type.</param>
		/// <param name="tagResult">The tag result.</param>
		public ReplacePatternSmartTag(string tag, string description, SmartTagType type, Func<string, string> tagResult)
			: this(tag, description, type, (m, p) => tagResult(p))
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplacePatternSmartTag" /> class.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="description">The description.</param>
		/// <param name="type">The type.</param>
		/// <param name="tagResult">The tag result.</param>
		public ReplacePatternSmartTag(string tag, string description, SmartTagType type, Func<Match, string, string> tagResult)
		{
			this.Description = description;
			this.Type = type;
			this.Tag = tag;
			this.TagResult = tagResult;

			this.tagApplicationRegex = new Regex(@"(?x)\$\{\<" + this.Tag + @"(?>\((?<Options>(?>(?<Paren>\()|(?<-Paren>\))|[^\(\)]+)*)\))?\>\}", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		#endregion

		#region APPLY TAG

		/// <summary>
		/// Applies the tag.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="contextMatch"></param>
		/// <returns></returns>
		public string ApplyTag(string input, Match contextMatch)
		{
			// Execute replacement:
			return this.tagApplicationRegex.Replace(input, match =>
			{
				string options = match.Groups["Options"].Value;
				string result;
				switch (this.Type)
			    {
				    case SmartTagType.Constant:
					case SmartTagType.FileUnique:
					    if (!this.valueCache.TryGetValue(options, out result))
						    this.valueCache[options] = result = this.TagResult(contextMatch, options);
					    break;
		            default:
					    result = this.TagResult(contextMatch, options);
					    break;
			    }
				return result;
			});
		}

		#endregion

		#region RESET

		/// <summary>
		/// Resets the tag state.
		/// </summary>
		public void Reset()
		{
			// Clear caches:
			this.valueCache.Clear();
		}

		#endregion
	}

	public enum SmartTagType
	{
		Constant,
		FileUnique,
		MatchUnique
	}
}