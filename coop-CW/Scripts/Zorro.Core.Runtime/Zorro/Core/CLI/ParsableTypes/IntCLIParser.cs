using System.Collections.Generic;

namespace Zorro.Core.CLI.ParsableTypes
{
	[TypeParser(typeof(int))]
	public class IntCLIParser : CLITypeParser
	{
		public override object Parse(string str)
		{
			return int.Parse(str);
		}

		public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
		{
			List<ParameterAutocomplete> list = new List<ParameterAutocomplete>();
			if (string.IsNullOrEmpty(parameterText))
			{
				list.Add(new ParameterAutocomplete(int.MinValue.ToString()));
				list.Add(new ParameterAutocomplete(int.MaxValue.ToString()));
			}
			return list;
		}
	}
}
