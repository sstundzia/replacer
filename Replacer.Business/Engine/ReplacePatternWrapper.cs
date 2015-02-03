using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Replacer.Business.Engine
{
    internal class ReplacePatternWrapper
    {
        #region FIELDS

        private readonly ReplacePattern pattern;
        private readonly Regex regex;
        private readonly List<ReplacePatternSmartTag> usedTags = new List<ReplacePatternSmartTag>();
        private readonly List<ReplacePatternSmartTag> fileUniqueTags = new List<ReplacePatternSmartTag>();
        private readonly List<ReplacePatternSmartTag> matchUniqueTags = new List<ReplacePatternSmartTag>();
        private readonly string constantReplacePattern;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacePatternWrapper"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public ReplacePatternWrapper(ReplacePattern pattern)
        {
            this.pattern = pattern;
            this.constantReplacePattern = this.pattern.Replace;

            // Process regex type:
            if (pattern.ReplaceType == ReplaceType.Regex)
            {
                // Initialize regex object:
                this.regex = new Regex(pattern.Find, RegexOptions.Compiled);

                // Collect all tags:
                if (!String.IsNullOrWhiteSpace(pattern.Replace))
                {
                    // Collect all tags:
                    MatchCollection matches = new Regex(@"\$\{\<(?<TagName>\w+)(?>\((?<Options>(?>(?<Paren>\()|(?<-Paren>\))|[^\(\)]+)*)\))?\>\}", RegexOptions.CultureInvariant).Matches(pattern.Replace);
                    ReplacePatternSmartTag tag;
                    foreach (Match match in matches)
                    {
                        if (AllAvailableSmartTags.Tags.TryGetValue(match.Groups["TagName"].Value, out tag))
                        {
                            if (!this.usedTags.Contains(tag))
                                this.usedTags.Add(tag);
                        }
                    }
                }
            }
        }

        #endregion

        #region PROCESS PATTERN

        /// <summary>
        /// Processes the pattern.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public string ProcessPattern(string sourceString)
        {
            string result;
            if (this.regex != null)
            {
                string replacePattern = this.constantReplacePattern;
                if (this.usedTags.Count > 0)
                {
                    result = this.regex.Replace(
                        sourceString,
                        m => m.Result(this.usedTags.Aggregate(replacePattern,
                            (current, tag) => tag.ApplyTag(current, m))));
                }
                else
                    result = this.regex.Replace(sourceString, replacePattern ?? String.Empty);
            }
            else
                result = sourceString.Replace(this.pattern.Find, this.pattern.Replace ?? String.Empty);
            return result;
        }

        #endregion
    }
}