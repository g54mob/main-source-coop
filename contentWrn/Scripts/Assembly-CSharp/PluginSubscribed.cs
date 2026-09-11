using System;
using System.Collections.Generic;
using System.Reflection;
using Steamworks;
using UnityEngine;

public class PluginSubscribed : Plugin
{
	public PublishedFileId_t publishedFileId;

	public string directory;

	public string dllPath;

	public Assembly assembly;

	public PluginInfo info;

	public SteamUGCDetails_t? steamUgcDetails;

	public string steamMetadata;

	public override PublishedFileId_t? PublishedFileId => publishedFileId;

	public override bool Publishable => false;

	public override PluginInfo? InfoFromAssembly => info;

	public override Assembly Assembly => assembly;

	public override string DisplayName => steamUgcDetails?.m_rgchTitle ?? info.name;

	public override string SteamMetadata => steamMetadata;

	public static bool Load(PublishedFileId_t publishedFileId, List<Plugin> existingPlugins, out PluginSubscribed result)
	{
		result = null;
		try
		{
			if (!Plugin.LoadDirFromFileId(publishedFileId, out var text))
			{
				return false;
			}
			if (!Plugin.FindDllInDirectory(text, out var result2, out var guid))
			{
				return false;
			}
			foreach (Plugin existingPlugin in existingPlugins)
			{
				string text2 = existingPlugin.SteamMetadata ?? existingPlugin.InfoFromAssembly?.guid;
				if (!string.IsNullOrEmpty(text2) && text2 == guid)
				{
					Debug.Log($"Already-loaded plugin {existingPlugin.DisplayName} ({existingPlugin.Assembly?.Location}) matches ContentWarningPlugin guid of subscribed plugin {publishedFileId} (guid={guid}), not loading subscribed plugin");
					return false;
				}
			}
			if (!Plugin.LoadAssemblyFromFile(result2, out var asm))
			{
				return false;
			}
			if (!Plugin.FetchAttrsFromAssembly(asm, out var pluginInfo))
			{
				return false;
			}
			result = new PluginSubscribed
			{
				publishedFileId = publishedFileId,
				directory = text,
				dllPath = result2,
				assembly = asm,
				info = pluginInfo
			};
			Debug.Log("Loaded subscribed plugin: " + result.DisplayName + " from " + result.dllPath);
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError($"Failed to load plugin from steam fileid {publishedFileId} due to the above exception");
			return false;
		}
	}
}
