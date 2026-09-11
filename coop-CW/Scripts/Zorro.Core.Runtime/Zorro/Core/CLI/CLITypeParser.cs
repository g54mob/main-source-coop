using System.Collections.Generic;

namespace Zorro.Core.CLI
{
	public abstract class CLITypeParser
	{
		public abstract object Parse(string str);

		public abstract List<ParameterAutocomplete> FindAutocomplete(string parameterText);
	}
}
