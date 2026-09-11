using System.Collections.Generic;
using Zorro.Core;
using Zorro.Core.CLI;

[TypeParser(typeof(Item))]
public class ItemCLIParser : CLITypeParser
{
	public override object Parse(string str)
	{
		return ObjectDatabaseAsset<ItemDatabase, Item>.GetObjectFromString(str);
	}

	public override List<ParameterAutocomplete> FindAutocomplete(string parameterText)
	{
		return SingletonAsset<ItemDatabase>.Instance.GetAutocompleteOptions(parameterText);
	}
}
