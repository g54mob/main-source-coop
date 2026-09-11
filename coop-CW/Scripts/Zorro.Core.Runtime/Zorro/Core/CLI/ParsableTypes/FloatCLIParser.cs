using System.Collections.Generic;

namespace Zorro.Core.CLI.ParsableTypes
{
	[TypeParser(typeof(float))]
	public class FloatCLIParser : CLITypeParser
	{
		public override object Parse(string str)
		{
			return float.Parse(str);
		}

		public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
		{
			return new List<ParameterAutocomplete>();
		}
	}
}
