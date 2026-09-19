using System;
using System.Linq;

namespace Features.CommandLineArguments.Scripts
{
	public class CommandLineArgumentsService : ICommandLineArgumentsService
	{
		public bool HasArgument(string argumentName)
		{
			return Environment.GetCommandLineArgs().Contains(argumentName);
		}

		public string GetString(string argumentName, string defaultValue = null)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == argumentName && commandLineArgs.Length > i + 1)
				{
					return commandLineArgs[i + 1];
				}
			}
			return null;
		}

		public int GetInt(string argumentName, int defaultValue = 0)
		{
			if (int.TryParse(GetString(argumentName), out var result))
			{
				return result;
			}
			return 0;
		}

		public float GetFloat(string argumentName, float defaultValue = float.NaN)
		{
			if (float.TryParse(GetString(argumentName), out var result))
			{
				return result;
			}
			return float.NaN;
		}
	}
}
