using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

public static class PluginHandler
{
	private static Callback<ItemInstalled_t> itemInstalledCallback;

	public static bool needsHashRefresh;

	public static List<PluginLocal> LocalPlugins = new List<PluginLocal>();

	public static List<PluginSubscribed> SubscribedPlugins = new List<PluginSubscribed>();

	public static List<PluginPublished> PublishedPlugins = new List<PluginPublished>();

	public static List<PluginExternal> ExternalPlugins = new List<PluginExternal>();

	public static List<Plugin> AllPlugins = new List<Plugin>();

	public static PublishedFileId_t[] SubscribedItems
	{
		get
		{
			try
			{
				uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
				PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
				SteamUGC.GetSubscribedItems(array, numSubscribedItems);
				return array;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Failed to load subscribed items due to the above exception");
				return Array.Empty<PublishedFileId_t>();
			}
		}
	}

	public static async Task DoPluginThings()
	{
		AllPlugins = new List<Plugin>();
		LocalPlugins = LoadLocalPlugins();
		AllPlugins.AddRange(LocalPlugins);
		SubscribedPlugins = LoadSubscribedSteamworksPlugins(AllPlugins);
		AllPlugins.AddRange(SubscribedPlugins);
		itemInstalledCallback = Callback<ItemInstalled_t>.Create(OnItemInstalled);
		ExternalPlugins = ScanLoadedAssemblies(AllPlugins);
		AllPlugins.AddRange(ExternalPlugins);
		await FillInUgcDetailsForSubscribedPlugins(SubscribedPlugins);
		PublishedPlugins = await LoadAuthoredSteamworksPlugins();
		AllPlugins.AddRange(PublishedPlugins);
		SortPlugins(AllPlugins);
	}

	private static List<PluginLocal> LoadLocalPlugins()
	{
		try
		{
			string directoryName = Path.GetDirectoryName(Application.dataPath);
			if (directoryName == null)
			{
				return new List<PluginLocal>();
			}
			string path = Path.Join(directoryName, "Plugins");
			if (!Directory.Exists(path))
			{
				return new List<PluginLocal>();
			}
			List<PluginLocal> list = new List<PluginLocal>();
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				if (PluginLocal.Load(directories[i], out var result))
				{
					list.Add(result);
					needsHashRefresh = true;
				}
			}
			return list;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed loading Plugins folder due to the above");
			return new List<PluginLocal>();
		}
	}

	private static List<PluginSubscribed> LoadSubscribedSteamworksPlugins(List<Plugin> existingPlugins)
	{
		List<PluginSubscribed> list = new List<PluginSubscribed>();
		PublishedFileId_t[] subscribedItems = SubscribedItems;
		for (int i = 0; i < subscribedItems.Length; i++)
		{
			if (PluginSubscribed.Load(subscribedItems[i], existingPlugins, out var result))
			{
				list.Add(result);
				needsHashRefresh = true;
			}
		}
		return list;
	}

	private static async void OnItemInstalled(ItemInstalled_t param)
	{
		AppId_t appID = SteamUtils.GetAppID();
		bool flag = param.m_unAppID == appID;
		Debug.Log(string.Format("Plugin callback OnItemInstalled: appid:{0} (we are {1}), fileid:{2}{3}", param.m_unAppID, appID, param.m_nPublishedFileId, flag ? "" : " (NOT OUR APPID, ignoring!)"));
		if (flag && PluginSubscribed.Load(param.m_nPublishedFileId, AllPlugins, out var result))
		{
			SubscribedPlugins.Add(result);
			AllPlugins.Add(result);
			await FillInUgcDetailsForSubscribedPlugins(SubscribedPlugins);
			SortPlugins(AllPlugins);
		}
	}

	private static async Task<List<PluginPublished>> LoadAuthoredSteamworksPlugins()
	{
		UGCQueryHandle_t query = SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_All, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1u);
		if (!SteamUGC.SetReturnMetadata(query, bReturnMetadata: true))
		{
			Debug.LogError("Unable to SteamUGC.SetReturnMetadata(true)");
		}
		if (!SteamUGC.SetReturnLongDescription(query, bReturnLongDescription: true))
		{
			Debug.LogError("Unable to SteamUGC.SetReturnLongDescription(true)");
		}
		SteamUGCQueryCompleted_t steamUGCQueryCompleted_t = await SteamUGC.SendQueryUGCRequest(query).ToAsync<SteamUGCQueryCompleted_t>();
		List<PluginPublished> list = new List<PluginPublished>();
		for (uint num = 0u; num < steamUGCQueryCompleted_t.m_unNumResultsReturned; num++)
		{
			if (SteamUGC.GetQueryUGCResult(steamUGCQueryCompleted_t.m_handle, num, out var pDetails))
			{
				if (!SteamUGC.GetQueryUGCMetadata(steamUGCQueryCompleted_t.m_handle, num, out var pchMetadata, 5000u))
				{
					Debug.LogError("Unable to SteamUGC.GetQueryUGCMetadata");
				}
				PluginPublished item = new PluginPublished(pDetails, pchMetadata);
				list.Add(item);
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(query);
		return list;
	}

	private static async Task FillInUgcDetailsForSubscribedPlugins(List<PluginSubscribed> plugins)
	{
		foreach (var (value, steamMetadata) in await FetchUgcDetailsFromFileId((from p in plugins
			where !p.steamUgcDetails.HasValue
			select p.publishedFileId).ToArray()))
		{
			foreach (PluginSubscribed plugin in plugins)
			{
				if (plugin.publishedFileId == value.m_nPublishedFileId)
				{
					plugin.steamUgcDetails = value;
					plugin.steamMetadata = steamMetadata;
					break;
				}
			}
		}
	}

	private static async Task<List<(SteamUGCDetails_t details, string metadata)>> FetchUgcDetailsFromFileId(PublishedFileId_t[] fileIds)
	{
		if (fileIds.Length == 0)
		{
			return new List<(SteamUGCDetails_t, string)>();
		}
		UGCQueryHandle_t query = SteamUGC.CreateQueryUGCDetailsRequest(fileIds, (uint)fileIds.Length);
		if (!SteamUGC.SetReturnMetadata(query, bReturnMetadata: true))
		{
			Debug.LogError("Unable to SteamUGC.SetReturnMetadata(true)");
		}
		SteamUGCQueryCompleted_t steamUGCQueryCompleted_t = await SteamUGC.SendQueryUGCRequest(query).ToAsync<SteamUGCQueryCompleted_t>();
		if (steamUGCQueryCompleted_t.m_eResult != EResult.k_EResultOK)
		{
			Modal.ShowError("Error", $"Could not fetch Steam Workshop item details: {steamUGCQueryCompleted_t.m_eResult}");
			return new List<(SteamUGCDetails_t, string)>();
		}
		List<(SteamUGCDetails_t, string)> list = new List<(SteamUGCDetails_t, string)>();
		for (uint num = 0u; num < steamUGCQueryCompleted_t.m_unNumResultsReturned; num++)
		{
			if (SteamUGC.GetQueryUGCResult(steamUGCQueryCompleted_t.m_handle, num, out var pDetails))
			{
				if (!SteamUGC.GetQueryUGCMetadata(steamUGCQueryCompleted_t.m_handle, num, out var pchMetadata, 5000u))
				{
					pchMetadata = null;
				}
				list.Add((pDetails, pchMetadata));
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(query);
		return list;
	}

	private static List<PluginExternal> ScanLoadedAssemblies(List<Plugin> alreadyLoadedPlugins)
	{
		List<PluginExternal> list = new List<PluginExternal>();
		try
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (!AlreadyLoaded(assembly) && PluginExternal.TryFrom(assembly, out var result))
				{
					list.Add(result);
					needsHashRefresh = true;
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed to check external assemblies due to the above exception");
		}
		return list;
		bool AlreadyLoaded(Assembly assembly2)
		{
			foreach (Plugin alreadyLoadedPlugin in alreadyLoadedPlugins)
			{
				if (alreadyLoadedPlugin.Assembly == assembly2)
				{
					return true;
				}
			}
			return false;
		}
	}

	private static void SortPlugins(List<Plugin> allPlugins)
	{
		allPlugins.Sort(delegate(Plugin l, Plugin r)
		{
			string displayName = l.DisplayName;
			return displayName?.CompareTo(r.DisplayName) ?? ((l != r) ? (-1) : 0);
		});
	}

	public static Hash128? CheckPlugins()
	{
		needsHashRefresh = false;
		List<Plugin.PluginHashInfo> list = new List<Plugin.PluginHashInfo>();
		foreach (Plugin allPlugin in AllPlugins)
		{
			Plugin.PluginHashInfo? pluginHashInfo = allPlugin.GetPluginHashInfo();
			if (pluginHashInfo.HasValue)
			{
				Plugin.PluginHashInfo valueOrDefault = pluginHashInfo.GetValueOrDefault();
				list.Add(valueOrDefault);
			}
		}
		Hash128? hash;
		if (list.Count > 0)
		{
			hash = Hash128.Compute(list);
			Hash128? hash2 = hash;
			Debug.Log("Plugin hash: " + hash2.ToString());
		}
		else
		{
			hash = null;
			Debug.Log("No plugins found that require hash");
		}
		return hash;
	}

	public static async Task UpdatePublishedPlugins()
	{
		foreach (PluginPublished publishedPlugin in PublishedPlugins)
		{
			AllPlugins.Remove(publishedPlugin);
		}
		PublishedPlugins = await LoadAuthoredSteamworksPlugins();
		AllPlugins.AddRange(PublishedPlugins);
		SortPlugins(AllPlugins);
	}
}
