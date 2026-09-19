using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.DamageableTrackModule.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.AnalyticsExtensions
{
	public static class SessionAnalyticsEventNameExtensions
	{
		public static string BuildSessionStartedEventName(bool isFirstSession, int partySize)
		{
			if (!isFirstSession)
			{
				return $"session:started:size{partySize}";
			}
			return $"session:started:isFirst:size{partySize}";
		}

		public static string BuildLevelStartedBossEventName(string levelKey, string bossType)
		{
			return "level:started:" + levelKey + ":boss:" + bossType;
		}

		public static string BuildEnemySpawnedEventName(string enemyType)
		{
			return "enemy:spawned:" + enemyType;
		}

		public static string BuildEnemyKilledEventName(string enemyType)
		{
			return "enemy:killed:" + enemyType;
		}

		public static string BuildMainMenuOpenedEventName(MainMenuOpenedAnalyticsSource source)
		{
			return $"main_menu:opened:{source}";
		}

		public static string BuildMainMenuClickEventName(MainMenuClickAnalyticsButton button)
		{
			return $"main_menu:click:{button}";
		}

		public static string BuildLobbyEnteredEventName(string source)
		{
			return "lobby:entered:" + source;
		}

		public static string BuildLobbyClickEventName(LobbyClickAnalyticsButton button)
		{
			return $"lobby:click:{button}";
		}

		public static string BuildDeathCausedEventName(DamageSource source)
		{
			string text = $"death:{source.Category}:{source.Type}";
			if (source.Category != DamageCauseCategory.Enemy)
			{
				return text;
			}
			string damageOwnerTypeName = source.Owner.DamageOwnerTypeName;
			if (!string.IsNullOrEmpty(damageOwnerTypeName))
			{
				text = text + ":" + damageOwnerTypeName;
			}
			string prefabInstanceName = source.Owner.PrefabInstanceName;
			if (!string.IsNullOrEmpty(prefabInstanceName))
			{
				text = text + ":" + prefabInstanceName;
			}
			return text;
		}

		public static string BuildSessionEndEventName(SessionEndAnalyticsReason reason, SessionEndAnalyticsPlace place)
		{
			return "session:end:" + ToReasonEventSegment(reason) + ":" + ToPlaceEventSegment(place);
		}

		private static string ToReasonEventSegment(SessionEndAnalyticsReason reason)
		{
			return reason switch
			{
				SessionEndAnalyticsReason.Normal => "Normal", 
				SessionEndAnalyticsReason.AltF4 => "AltF4", 
				SessionEndAnalyticsReason.Crash => "Crash", 
				SessionEndAnalyticsReason.Disconnect => "Disconnect", 
				_ => "Unknown", 
			};
		}

		private static string ToPlaceEventSegment(SessionEndAnalyticsPlace place)
		{
			return place.Kind switch
			{
				SessionEndAnalyticsPlaceKind.Loading => "Loading", 
				SessionEndAnalyticsPlaceKind.Beach => $"Beach_L{place.LevelNumber:D2}", 
				SessionEndAnalyticsPlaceKind.Level => $"Level_L{place.LevelNumber:D2}", 
				SessionEndAnalyticsPlaceKind.Shop => $"Shop_L{place.LevelNumber:D2}", 
				_ => "Unknown", 
			};
		}
	}
}
