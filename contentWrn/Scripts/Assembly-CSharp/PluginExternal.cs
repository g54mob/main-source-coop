using System;
using System.Reflection;
using Steamworks;
using UnityEngine;

public class PluginExternal : Plugin
{
	public Assembly assembly;

	public PluginInfo info;

	public override PluginInfo? InfoFromAssembly => info;

	public override Assembly Assembly => assembly;

	public override string DisplayName => info.name;

	public override bool Publishable => false;

	public override PublishedFileId_t? PublishedFileId => null;

	public override string SteamMetadata => null;

	public static bool TryFrom(Assembly assembly, out PluginExternal result)
	{
		result = null;
		try
		{
			if (!Plugin.FetchAttrsFromAssembly(assembly, out var pluginInfo))
			{
				return false;
			}
			result = new PluginExternal
			{
				assembly = assembly,
				info = pluginInfo
			};
			Debug.Log("Found external plugin: " + result.DisplayName + " from " + assembly.Location);
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed to parse attributes from assembly " + assembly.FullName + " due to the above exception");
			return false;
		}
	}
}
