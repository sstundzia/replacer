using System;
using Replacer.Business.Crawler;
using Replacer.Business.Engine;

namespace Replacer.Business
{
	[Serializable]
	public class ReplacerProject
	{
		public FileFolderPath[] FileFolderPaths { get; set; }
		public ReplacePattern[] PatternList { get; set; }
	}
}