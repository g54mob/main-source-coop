using Rewired.Data;
using Rewired.Interfaces;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal abstract class PlatformInitializer
	{
		public abstract object Initialize(IConfigVars_Internal configVars);

		public abstract IElementIdentifierTool CreateTool(string inputSourceString);
	}
}
