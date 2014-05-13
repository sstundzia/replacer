using System.Diagnostics;
using System.IO;

namespace Replacer.Console
{
	public class CommandLineArgsParser
	{
		#region CONSTANTS

		private const string PARAM_PROJECT = "-p";
		private const string PARAM_LOG_TO_FILE = "-lf";
		private const string PARAM_LOG_TO_CONSOLE = "-lc";
		private const string PARAM_HELP = "-?";

		#endregion

		#region PROPERTIES

		/// <summary>
		/// Gets the project path.
		/// </summary>
		public string ProjectPath { get; private set; }

		/// <summary>
		/// Gets the log file path.
		/// </summary>
		public string LogFilePath { get; private set; }

		/// <summary>
		/// Gets a value indicating whether logging to console is allowed.
		/// </summary>
		/// <value>
		///   <c>true</c> if log to console; otherwise, <c>false</c>.
		/// </value>
		public bool LogToConsole { get; private set; }

		#endregion

		#region PARSE ARGS

		/// <summary>
		/// Parses command line arguments.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="errorMessage"></param>
		/// <returns><c>true</c>, if the job can be run.</returns>
		public bool ParseArguments(string[] args, out string errorMessage)
		{
			errorMessage = null;
			int i = 0;
			while (i < args.Length)
			{
				switch (args[i])
				{
					case PARAM_PROJECT:
						i++;
						if (i < args.Length)
						{
							this.ProjectPath = args[i];
							if (!File.Exists(this.ProjectPath))
							{
								errorMessage = string.Format("Project file \"{0}\" does not exist.", this.ProjectPath);
								return false;
							}
						}
						else
						{
							errorMessage = string.Format("{0} option must be followed by a valid project file path.", PARAM_PROJECT);
							return false;
						}
						break;
					case PARAM_LOG_TO_FILE:
						i++;
						if (i < args.Length)
						{
							this.LogFilePath = args[i];
						}
						else
						{
							errorMessage = string.Format("{0} option must be followed by a log file path.", PARAM_LOG_TO_FILE);
							return false;
						}
						break;
					case PARAM_LOG_TO_CONSOLE:
						this.LogToConsole = true;
						break;
					case PARAM_HELP:
						errorMessage = null;
						return false;
					default:
						errorMessage = string.Format("Invalid argument: {0}", args[i]);
						break;
				}
				i++;
			}

			return !string.IsNullOrWhiteSpace(this.ProjectPath);
		}

		#endregion

		#region HELP

		/// <summary>
		/// Prints usage instructions.
		/// </summary>
		public void PrintHelp()
		{
			System.Console.WriteLine(@"Usage: {0} {1} ProjectPath {2} LogFilePath {3}", Process.GetCurrentProcess().ProcessName, PARAM_PROJECT, PARAM_LOG_TO_FILE, PARAM_LOG_TO_CONSOLE);
			System.Console.WriteLine();
			System.Console.WriteLine(@"{0} ProjectPath - Required. A path to a replacer project file.", PARAM_PROJECT);
			System.Console.WriteLine(@"{0} LogFilePath - Optional. If specified, replacer log will be outputed to specified file.", PARAM_LOG_TO_FILE);
			System.Console.WriteLine(@"{0} - Optional. If specified, replacer log will be outputed to console.", PARAM_LOG_TO_CONSOLE);
		}

		#endregion


	}
}