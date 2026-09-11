using System.Collections.Generic;
using Zorro.Core;
using Zorro.Core.CLI;

[TypeParser(typeof(NetworkDealBase))]
public class NetworkCLIParser : CLITypeParser
{
	public override object Parse(string str)
	{
		return SingletonAsset<NetworkDealDataBase>.Instance.GetDealFromString(str);
	}

	public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
	{
		return SingletonAsset<NetworkDealDataBase>.Instance.GetAutoCompleteOptions(parameterText);
	}
}
