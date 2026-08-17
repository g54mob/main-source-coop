namespace Rewired.Interfaces
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IControllerTemplateElementIdentifier_Editor : IControllerTemplateElementIdentifier, IControllerElementIdentifierCommon_Internal
	{
		string scriptingName { get; }

		string alternateScriptingName { get; }
	}
}
