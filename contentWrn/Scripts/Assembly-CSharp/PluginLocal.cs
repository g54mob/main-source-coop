using System;
using System.IO;
using System.Reflection;
using Steamworks;
using UnityEngine;

public class PluginLocal : Plugin
{
	public string directory;

	public string dllPath;

	public Assembly assembly;

	public PluginInfo info;

	public override PluginInfo? InfoFromAssembly => info;

	public override Assembly Assembly => assembly;

	public override string DisplayName => info.name;

	public override bool Publishable => !string.IsNullOrEmpty(info.guid);

	public override string SteamMetadata => null;

	public override PublishedFileId_t? PublishedFileId => null;

	public static bool Load(string directory, out PluginLocal result)
	{
		result = null;
		try
		{
			if (!Plugin.FindDllInDirectory(directory, out var result2, out var _))
			{
				return false;
			}
			if (!Plugin.LoadAssemblyFromFile(result2, out var asm))
			{
				return false;
			}
			if (!Plugin.FetchAttrsFromAssembly(asm, out var pluginInfo))
			{
				return false;
			}
			result = new PluginLocal
			{
				directory = directory,
				dllPath = result2,
				assembly = asm,
				info = pluginInfo
			};
			Debug.Log("Loaded local plugin: " + result.DisplayName + " from " + result.dllPath);
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed to load plugin from " + directory + " due to the above exception");
			return false;
		}
	}

	public string GetPreviewImagePath()
	{
		string text = Path.Join(directory, "preview.png");
		if (File.Exists(text))
		{
			return text;
		}
		string text2 = Path.Join(directory, "preview.jpg");
		if (File.Exists(text2))
		{
			return text2;
		}
		return null;
	}
}
