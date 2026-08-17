using Rewired.Internal.Localization;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IControllerTemplate_Internal : IControllerTemplate
	{
		DeviceLocalizationInfo deviceLocalizationInfo { get; }
	}
}
