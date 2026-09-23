using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class MapCatalog
	{
		private const string ResourceFolder = "Maps";

		private static MapScriptableObject[] cached;

		private static readonly HashSet<string> reportedDevelopmentOnly = new HashSet<string>();

		public static bool ShowsDevelopmentMaps => Debug.isDebugBuild;

		public static IReadOnlyList<MapScriptableObject> All
		{
			get
			{
				if (cached != null)
				{
					return cached;
				}
				List<MapScriptableObject> list = new List<MapScriptableObject>();
				MapScriptableObject[] array = Resources.LoadAll<MapScriptableObject>("Maps");
				foreach (MapScriptableObject mapScriptableObject in array)
				{
					if (!(mapScriptableObject == null))
					{
						if (!mapScriptableObject.IsUsable)
						{
							Debug.LogWarning("[MapCatalog] '" + mapScriptableObject.name + "' eksik (Map Id ve ardından Scene Name ya da Prefab dolu olmalı) - listeye alınmadı.");
						}
						else if (!mapScriptableObject.DevelopmentOnly || ShowsDevelopmentMaps)
						{
							list.Add(mapScriptableObject);
						}
					}
				}
				list.Sort(delegate(MapScriptableObject a, MapScriptableObject b)
				{
					int num = a.Availability.SortOffset().CompareTo(b.Availability.SortOffset());
					return (num == 0) ? string.CompareOrdinal(a.DisplayName, b.DisplayName) : num;
				});
				cached = list.ToArray();
				if (cached.Length == 0)
				{
					Debug.LogWarning("[MapCatalog] Assets/Resources/Maps altında kullanılabilir harita yok.");
				}
				return cached;
			}
		}

		public static MapScriptableObject Default
		{
			get
			{
				if (All.Count <= 0)
				{
					return null;
				}
				return All[0];
			}
		}

		public static MapScriptableObject Find(string mapId)
		{
			if (string.IsNullOrEmpty(mapId))
			{
				return null;
			}
			foreach (MapScriptableObject item in All)
			{
				if (item.MapId == mapId)
				{
					return item;
				}
			}
			WarnIfDevelopmentOnly(mapId);
			return null;
		}

		private static void WarnIfDevelopmentOnly(string mapId)
		{
			if (ShowsDevelopmentMaps)
			{
				return;
			}
			MapScriptableObject[] array = Resources.LoadAll<MapScriptableObject>("Maps");
			foreach (MapScriptableObject mapScriptableObject in array)
			{
				if (!(mapScriptableObject == null) && !(mapScriptableObject.MapId != mapId) && mapScriptableObject.DevelopmentOnly)
				{
					if (reportedDevelopmentOnly.Add(mapId))
					{
						Debug.LogWarning("[MapCatalog] '" + mapId + "' sadece geliştirme haritası - bu build'de yüklenemez. Host muhtemelen Editor'den oynuyor.");
					}
					break;
				}
			}
		}

		public static string DisplayName(string mapId)
		{
			MapScriptableObject mapScriptableObject = Find(mapId);
			if (mapScriptableObject != null)
			{
				return mapScriptableObject.DisplayName;
			}
			if (!string.IsNullOrEmpty(mapId))
			{
				return mapId;
			}
			return "Bilinmeyen harita";
		}

		public static void Invalidate()
		{
			cached = null;
			reportedDevelopmentOnly.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			Invalidate();
		}
	}
}
