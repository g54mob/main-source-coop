namespace Rewired.Internal.Localization
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal interface ITryGetLocalizedName
	{
		bool TryGetLocalizedName(out string value);
	}
}
