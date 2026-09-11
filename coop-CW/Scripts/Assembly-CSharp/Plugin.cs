using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Mono.Cecil;
using Steamworks;
using UnityEngine;

public abstract class Plugin
{
	public struct PluginInfo
	{
		public string guid;

		public string name;

		public string version;

		public bool vanillaCompatible;
	}

	public struct PluginHashInfo
	{
		public int version;

		public int guid;

		public int name;
	}

	public static Harmony Harmony = new Harmony("se.landfall.contentwarning");

	public abstract PluginInfo? InfoFromAssembly { get; }

	public abstract Assembly Assembly { get; }

	public abstract string DisplayName { get; }

	public abstract bool Publishable { get; }

	public abstract PublishedFileId_t? PublishedFileId { get; }

	public abstract string SteamMetadata { get; }

	public static bool HasSameIdentity(Plugin left, Plugin right)
	{
		if (left == right)
		{
			return true;
		}
		if (left.Assembly != null && left.Assembly == right.Assembly)
		{
			return true;
		}
		if (left.PublishedFileId.HasValue && left.PublishedFileId == right.PublishedFileId)
		{
			return true;
		}
		string text = left.SteamMetadata ?? left.InfoFromAssembly?.guid;
		string text2 = right.SteamMetadata ?? right.InfoFromAssembly?.guid;
		if (text != null && text == text2)
		{
			return true;
		}
		return false;
	}

	public PublishedFileId_t? DiscoverPublishedFileId(List<Plugin> allPlugins)
	{
		if (PublishedFileId.HasValue)
		{
			return PublishedFileId;
		}
		foreach (Plugin allPlugin in allPlugins)
		{
			if (allPlugin.PublishedFileId.HasValue && HasSameIdentity(this, allPlugin))
			{
				return allPlugin.PublishedFileId;
			}
		}
		return null;
	}

	public static bool LoadDirFromFileId(PublishedFileId_t fileId, out string directory)
	{
		if (!SteamUGC.GetItemInstallInfo(fileId, out var _, out directory, 2048u, out var _))
		{
			PublishedFileId_t publishedFileId_t = fileId;
			Debug.LogError("Failed to get steamworks item: " + publishedFileId_t.ToString());
			return false;
		}
		return true;
	}

	public static bool FindDllInDirectory(string directory, out string result, out string guid)
	{
		string[] files = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories);
		foreach (string text in files)
		{
			if (IsValidSteamworksPlugin(text, out guid))
			{
				result = text;
				return true;
			}
		}
		Debug.LogError("Did not find valid valid plugin DLL in directory: " + directory);
		result = null;
		guid = null;
		return false;
	}

	public static bool LoadAssemblyFromFile(string dllPath, out Assembly asm)
	{
		asm = null;
		try
		{
			asm = Assembly.LoadFrom(dllPath);
			Debug.Log("Patching " + asm.Location + " with " + Harmony.GetType().Assembly.Location);
			Harmony.PatchAll(asm);
			Type[] types = asm.GetTypes();
			foreach (Type type in types)
			{
				if (type.GetCustomAttribute<ContentWarningPlugin>() != null)
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
			}
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed to load dll due to the above: " + dllPath);
			return false;
		}
	}

	private static bool IsValidSteamworksPlugin(string dll, out string guid)
	{
		try
		{
			using AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(dll);
			foreach (TypeDefinition type in assemblyDefinition.MainModule.Types)
			{
				foreach (CustomAttribute customAttribute in type.CustomAttributes)
				{
					if (customAttribute.AttributeType.Name == "ContentWarningPlugin" && customAttribute.HasConstructorArguments && customAttribute.ConstructorArguments.Count > 0 && customAttribute.ConstructorArguments[0].Value is string text)
					{
						guid = text;
						return true;
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.Log("Skipping loading dll " + dll + " due to the above exception");
		}
		guid = null;
		return false;
	}

	public PluginHashInfo? GetPluginHashInfo()
	{
		PluginInfo? infoFromAssembly = InfoFromAssembly;
		if (infoFromAssembly.HasValue)
		{
			PluginInfo valueOrDefault = infoFromAssembly.GetValueOrDefault();
			if (!valueOrDefault.vanillaCompatible)
			{
				PluginHashInfo value = default(PluginHashInfo);
				value.version = valueOrDefault.version?.GetHashCode() ?? 0;
				value.guid = valueOrDefault.guid?.GetHashCode() ?? 0;
				value.name = valueOrDefault.name?.GetHashCode() ?? 0;
				return value;
			}
		}
		return null;
	}

	public static bool FetchAttrsFromAssembly(Assembly assembly, out PluginInfo pluginInfo)
	{
		bool num = IsMelonLoaderPlugin(assembly) || IsBepInExPlugin(assembly);
		ContentWarningPlugin contentWarningPlugin = GetContentWarningPlugin(assembly);
		if (!num && contentWarningPlugin == null)
		{
			pluginInfo = default(PluginInfo);
			return false;
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assembly.Location);
		string name = fileNameWithoutExtension;
		string version = contentWarningPlugin?.Version;
		if (IsSkippedDueToAssemblyMetadata(assembly))
		{
			Debug.Log("Found plugin " + fileNameWithoutExtension + " that would be considered a plugin, but is skipped due to having [assembly: AssemblyMetadata(\"ContentWarning.VanillaCompatible\", \"true\")]");
			pluginInfo = new PluginInfo
			{
				guid = contentWarningPlugin?.Guid,
				name = name,
				version = version,
				vanillaCompatible = true
			};
			return true;
		}
		if (contentWarningPlugin == null)
		{
			Debug.LogWarning("Found plugin with no attribute. Will be treated as non-compatible with vanilla: " + assembly.Location);
			pluginInfo = new PluginInfo
			{
				guid = null,
				name = name,
				version = null,
				vanillaCompatible = false
			};
		}
		else
		{
			Debug.Log($"Found plugin with Attribute: {fileNameWithoutExtension}, guid: {contentWarningPlugin.Guid}, version: {contentWarningPlugin.Version}, VanillaCompatible: {contentWarningPlugin.VanillaCompatible}");
			if (string.IsNullOrEmpty(contentWarningPlugin.Guid))
			{
				Debug.LogError("ContentWarningPlugin guid null/empty! This is not allowed");
			}
			pluginInfo = new PluginInfo
			{
				guid = contentWarningPlugin.Guid,
				name = name,
				version = version,
				vanillaCompatible = contentWarningPlugin.VanillaCompatible
			};
		}
		return true;
	}

	private static bool IsMelonLoaderPlugin(Assembly assembly)
	{
		foreach (CustomAttributeData customAttribute in assembly.CustomAttributes)
		{
			if (customAttribute.AttributeType.FullName == "MelonLoader.MelonInfoAttribute")
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsBepInExPlugin(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			foreach (CustomAttributeData customAttribute in types[i].CustomAttributes)
			{
				if (customAttribute.AttributeType.FullName == "BepInEx.BepInPlugin")
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool IsSkippedDueToAssemblyMetadata(Assembly assembly)
	{
		foreach (AssemblyMetadataAttribute customAttribute in assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
		{
			if (customAttribute.Key == "ContentWarning.VanillaCompatible" && customAttribute.Value == "true")
			{
				return true;
			}
		}
		return false;
	}

	private static ContentWarningPlugin GetContentWarningPlugin(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			ContentWarningPlugin customAttribute = types[i].GetCustomAttribute<ContentWarningPlugin>();
			if (customAttribute != null)
			{
				return customAttribute;
			}
		}
		return null;
	}
}
