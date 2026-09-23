using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class GameModeCatalog
	{
		private const string ResourceFolder = "GameModes";

		private static GameModeDefinition[] cached;

		public static IReadOnlyList<GameModeDefinition> All
		{
			get
			{
				if (cached != null)
				{
					return cached;
				}
				List<GameModeDefinition> list = new List<GameModeDefinition>();
				GameModeDefinition[] array = Resources.LoadAll<GameModeDefinition>("GameModes");
				foreach (GameModeDefinition gameModeDefinition in array)
				{
					if (!(gameModeDefinition == null))
					{
						if (!gameModeDefinition.IsUsable)
						{
							Debug.LogWarning("[GameModeCatalog] '" + gameModeDefinition.name + "' eksik (Mode Id ve Mode Scene Name dolu olmalı) - listeye alınmadı.");
						}
						else
						{
							list.Add(gameModeDefinition);
						}
					}
				}
				list.Sort(delegate(GameModeDefinition a, GameModeDefinition b)
				{
					int num = SortKey(a).CompareTo(SortKey(b));
					return (num == 0) ? string.CompareOrdinal(a.DisplayName, b.DisplayName) : num;
				});
				cached = list.ToArray();
				if (cached.Length == 0)
				{
					Debug.LogWarning("[GameModeCatalog] Assets/Resources/GameModes altında kullanılabilir oyun modu yok.");
				}
				return cached;
			}
		}

		public static GameModeDefinition Default
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

		private static int SortKey(GameModeDefinition mode)
		{
			return mode.SortOrder + mode.Availability.SortOffset();
		}

		public static GameModeDefinition Find(string modeId)
		{
			if (string.IsNullOrEmpty(modeId))
			{
				return null;
			}
			foreach (GameModeDefinition item in All)
			{
				if (item.ModeId == modeId)
				{
					return item;
				}
			}
			return null;
		}

		public static string DisplayName(string modeId)
		{
			GameModeDefinition gameModeDefinition = Find(modeId);
			if (gameModeDefinition != null)
			{
				return gameModeDefinition.DisplayName;
			}
			if (!string.IsNullOrEmpty(modeId))
			{
				return modeId;
			}
			return "Bilinmeyen mod";
		}

		public static void Invalidate()
		{
			cached = null;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			cached = null;
		}
	}
}
