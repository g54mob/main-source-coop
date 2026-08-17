using Rewired.Utils.Classes.Data;

namespace Rewired.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	internal interface IConfigVars_Internal
	{
		KeyedGetSetValueStore<string> values { get; }
	}
}
