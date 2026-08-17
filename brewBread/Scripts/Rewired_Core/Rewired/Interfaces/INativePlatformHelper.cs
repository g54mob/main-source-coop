namespace Rewired.Interfaces
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	internal interface INativePlatformHelper
	{
		bool isApplicationFocused { get; }
	}
}
