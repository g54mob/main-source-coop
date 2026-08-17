namespace Rewired.Data.Mapping
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	[CustomObfuscation(rename = false)]
	internal interface IControllerTemplateMapMapping_Internal
	{
		IControllerElementTarget[] GetTargets();
	}
}
