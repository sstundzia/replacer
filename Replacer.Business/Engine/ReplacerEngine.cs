using System;
using System.Collections.Generic;
using System.Linq;

namespace Replacer.Business.Engine
{
	public class ReplacerEngine
	{
		#region FIELDS

		private readonly List<ReplacePatternWrapper> wrappedPatterns;

		#endregion

		#region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacerEngine"/> class.
        /// </summary>
        /// <param name="replacePatternList">The replace pattern list.</param>
        /// <exception cref="System.ArgumentNullException">replacePatternList</exception>
        /// <exception cref="System.ArgumentException">No replace patterns specified.</exception>
        public ReplacerEngine(IEnumerable<ReplacePattern> replacePatternList)
		{
			// Validate:
			if (replacePatternList == null)
				throw new ArgumentNullException("replacePatternList");

			// Wrap patterns:
			this.wrappedPatterns = replacePatternList
				.Select(p => new ReplacePatternWrapper(p))
				.ToList();

            if (this.wrappedPatterns.Count == 0)
            {
                throw new ArgumentException("No replace patterns specified.");
            }

            // Reset tags:
            AllAvailableSmartTags.Reset();
		}

		#endregion

	    #region PROCESS STRING

        /// <summary>
        /// Processes the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
	    public string ProcessString(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Reset file tags:
            AllAvailableSmartTags.ResetFileTags();

            // Apply replacing:
            string output = input;
            int count = this.wrappedPatterns.Count;
            for (int i = 0; i < count; i++)
            {
                output = this.wrappedPatterns[i].ProcessPattern(output);
            }

	        return output;
	    }

	    #endregion
	}
}