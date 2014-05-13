using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Replacer.Business.Engine
{
	public static class AllAvailableSmartTags
	{
		/// <summary>
		/// Gets or sets the tags.
		/// </summary>
		/// <value>
		/// The tags.
		/// </value>
		public static Dictionary<string, ReplacePatternSmartTag> Tags { get; set; }

		static AllAvailableSmartTags()
		{
			List<ReplacePatternSmartTag> smartTags = new List<ReplacePatternSmartTag>
				{
					new ReplacePatternSmartTag(
						"NewGUID", "Generates New GUID value for each match.", SmartTagType.MatchUnique,
						() => Guid.NewGuid().ToString()),
					new ReplacePatternSmartTag(
						"NewFileGUID",
						"Generates new GUID value for each file (reusing same value for each match in the same file).",
						SmartTagType.FileUnique,
						() => Guid.NewGuid().ToString()),
					new ReplacePatternSmartTag(
						"NewConstGUID", "Generates new GUID value for the whole processing of the pattern..",
						SmartTagType.Constant,
						() => Guid.NewGuid().ToString()),
					new ReplacePatternSmartTag(
						"DateShort", "Replaces with a current date in short format.", SmartTagType.Constant,
						() => DateTime.Today.ToShortDateString()),
					new ReplacePatternSmartTag(
						"DateLong", "Replaces with a current date in long format.", SmartTagType.Constant,
						() => DateTime.Today.ToLongDateString()),
					new ReplacePatternSmartTag(
						"TimeShort", "Replaces with a current time in short format.", SmartTagType.Constant,
						() => DateTime.Now.ToShortTimeString()),
					new ReplacePatternSmartTag(
						"TimeLong", "Replaces with a current time in long format.", SmartTagType.Constant,
						() => DateTime.Now.ToShortTimeString()),
					new ReplacePatternSmartTag(
						"DateTimeFormat", "Replaces with the current date and time in given format.", SmartTagType.Constant,
						format => DateTime.Now.ToString(format)),
					new ReplacePatternSmartTag(
						"ToUpper", "Converts value of specified match group to ALL CAPS.", SmartTagType.MatchUnique,
						(context, options) => context.Groups[options].Value.ToUpper()),
					new ReplacePatternSmartTag(
						"ToLower", "Converts value of specified match group to \"all lowercase\".", SmartTagType.MatchUnique,
						(context, options) => context.Groups[options].Value.ToLower()),
					new ReplacePatternSmartTag(
						"ToUpperChar", "Converts specified chars from value of specified match group to UPPERCASE.",
						SmartTagType.MatchUnique,
						(context, options) => ToUpperChar(context, options, Char.ToUpper)),
					new ReplacePatternSmartTag(
						"ToLowerChar", "Converts specified chars from value of specified match group to lowercase.",
						SmartTagType.MatchUnique,
						(context, options) => ToUpperChar(context, options, Char.ToLower)),


				};

			Tags = smartTags.ToDictionary(t => t.Tag);
		}

		/// <summary>
		/// Resets all tags.
		/// </summary>
		public static void Reset()
		{
			foreach (KeyValuePair<string, ReplacePatternSmartTag> pair in Tags)
			{
				pair.Value.Reset();
			}
		}

		/// <summary>
		/// Resets all FileUnique type tags.
		/// </summary>
		public static void ResetFileTags()
		{
			foreach (ReplacePatternSmartTag tag in Tags.Values.Where(t => t.Type == SmartTagType.FileUnique))
			{
				tag.Reset();
			}
		}

		private static readonly Regex charConversionOptionsParser = new Regex(@"(?<GroupName>\w+)\s*(?>\[(?>(?<Index>[^,\]]+),?)+\])?");
		private static string ToUpperChar(Match context, string options, Func<char, char> processingFunction)
		{
			string result = string.Empty;
			Match match = charConversionOptionsParser.Match(context.Result(options));
			if (match.Success)
			{
				// Convert to char array:
				char[] charArray = context.Groups[match.Groups["GroupName"].Value].Value.ToCharArray();

				// Check for specified indices:
				Group indexGroup = match.Groups["Index"];
				if (indexGroup.Success)
				{
					// Process indices:
					foreach (Capture capture in indexGroup.Captures)
					{
						if (capture.Length > 0)
						{
							// Split options into range:
							string[] range = capture.Value.Split('-');

							// Parse "from" index:
							int from;
							if (!Int32.TryParse(range[0], out from))
								from = 0;
							int to = from;

							// Parse "to" index:
							if (range.Length > 1)
							{
								if (!Int32.TryParse(range[1], out to))
									to = charArray.Length - 1;
							}

							// Ensure valid range:
							if (from < 0)
								from = 0;
							if (to >= charArray.Length)
								to = charArray.Length - 1;

							// Apply processing:
							for (int i = from; i <= to; i++)
								charArray[i] = processingFunction(charArray[i]);
						}
					}
				}
				// No indices specified. Fallback to default - process the first char (if possible):
				else if (charArray.Length > 0)
				{
					charArray[0] = processingFunction(charArray[0]);
				}

				result = new string(charArray);
			}
			return result;
		}
	}
}