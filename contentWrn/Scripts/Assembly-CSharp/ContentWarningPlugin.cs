using System;

[AttributeUsage(AttributeTargets.Class)]
public class ContentWarningPlugin : Attribute
{
	public string Guid { get; set; }

	public string Version { get; set; }

	public bool VanillaCompatible { get; set; }

	public ContentWarningPlugin(string guid, string version, bool vanillaCompatible)
	{
		Guid = guid;
		Version = version;
		VanillaCompatible = vanillaCompatible;
	}
}
