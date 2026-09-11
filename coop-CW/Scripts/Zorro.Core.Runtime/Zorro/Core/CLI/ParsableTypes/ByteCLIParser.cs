using System.Collections.Generic;

namespace Zorro.Core.CLI.ParsableTypes
{
	[TypeParser(typeof(byte))]
	public class ByteCLIParser : CLITypeParser
	{
		public override object Parse(string str)
		{
			return byte.Parse(str);
		}

		public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
		{
			List<ParameterAutocomplete> list = new List<ParameterAutocomplete>();
			if (string.IsNullOrEmpty(parameterText))
			{
				list.Add(new ParameterAutocomplete(((byte)0).ToString()));
				list.Add(new ParameterAutocomplete(byte.MaxValue.ToString()));
			}
			return list;
		}
	}
}
