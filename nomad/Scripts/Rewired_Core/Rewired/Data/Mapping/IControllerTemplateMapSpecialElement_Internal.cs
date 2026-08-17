namespace Rewired.Data.Mapping
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	internal interface IControllerTemplateMapSpecialElement_Internal
	{
		T GetMapping<T>() where T : ControllerTemplateSpecialElementMapping;
	}
}
