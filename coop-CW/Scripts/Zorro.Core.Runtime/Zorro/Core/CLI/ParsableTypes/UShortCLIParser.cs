using System.Collections.Generic;

namespace Zorro.Core.CLI.ParsableTypes
{
	[TypeParser(typeof(ushort))]
	public class UShortCLIParser : CLITypeParser
	{
		public override object Parse(string str)
		{
			return ushort.Parse(str);
		}

		public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
		{
			List<ParameterAutocomplete> list = new List<ParameterAutocomplete>();
			if (string.IsNullOrEmpty(parameterText))
			{
				list.Add(new ParameterAutocomplete(((ushort)0).ToString()));
				list.Add(new ParameterAutocomplete(ushort.MaxValue.ToString()));
			}
			return list;
		}
	}
}
