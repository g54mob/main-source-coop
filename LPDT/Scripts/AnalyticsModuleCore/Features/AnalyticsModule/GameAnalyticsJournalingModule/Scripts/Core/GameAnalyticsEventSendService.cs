using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.AnalyticsExtensions;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;
using Features.ExtendedLogger.Scripts;
using Features.GameJournalingModule.Scripts.Core;
using Features.ProgressSavingModule.Scripts.Implementation;
using GameAnalyticsSDK;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core
{
	public class GameAnalyticsEventSendService : IAnalyticsEventSendService
	{
		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly GameAnalyticsEventsConfiguration _gameAnalyticsEventsConfiguration;

		private readonly ISavingService _savingService;

		private readonly AnalyticsEventQuota _eventQuota;

		private readonly MenuOpenedAnalyticsSourceModel _menuOpenedAnalyticsSourceModel;

		public GameAnalyticsEventSendService(SentAnalyticsModel sentAnalyticsModel, GameAnalyticsEventsConfiguration gameAnalyticsEventsConfiguration, ISavingService savingService, AnalyticsEventQuota eventQuota, MenuOpenedAnalyticsSourceModel menuOpenedAnalyticsSourceModel)
		{
			_sentAnalyticsModel = sentAnalyticsModel;
			_gameAnalyticsEventsConfiguration = gameAnalyticsEventsConfiguration;
			_savingService = savingService;
			_eventQuota = eventQuota;
			_menuOpenedAnalyticsSourceModel = menuOpenedAnalyticsSourceModel;
		}

		public void TrackMainMenuOpened()
		{
			TrackMainMenuOpened(_menuOpenedAnalyticsSourceModel.Consume());
		}

		public void TrackMainMenuOpened(MainMenuOpenedAnalyticsSource source)
		{
			NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildMainMenuOpenedEventName(source), AnalyticsEventQuotaGroup.General);
		}

		public void TrackMainMenuClick(MainMenuClickAnalyticsButton button)
		{
			NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildMainMenuClickEventName(button), AnalyticsEventQuotaGroup.General);
		}

		public void TrackLobbyEntered(string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildLobbyEnteredEventName(source), AnalyticsEventQuotaGroup.General);
			}
		}

		public void TrackLobbyClick(LobbyClickAnalyticsButton button)
		{
			NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildLobbyClickEventName(button), AnalyticsEventQuotaGroup.General);
		}

		public void TrackSessionStart(bool isFirstSession, int partySize)
		{
			NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildSessionStartedEventName(isFirstSession, partySize), AnalyticsEventQuotaGroup.General);
		}

		public void TrackSessionEnd(SessionEndAnalyticsReason reason, SessionEndAnalyticsPlace place)
		{
			NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildSessionEndEventName(reason, place), AnalyticsEventQuotaGroup.General);
		}

		public void TrackLocationEntered()
		{
			NewDesignEvent("location:entered", AnalyticsEventQuotaGroup.General);
		}

		public void TrackItemPickedUp()
		{
			NewDesignEvent("item:picked_up", AnalyticsEventQuotaGroup.General);
		}

		public void TrackEnemyEncountered()
		{
			NewDesignEvent("enemy:encountered", AnalyticsEventQuotaGroup.General);
		}

		public void TrackEnemySpawned(string enemyType, float spawnCount)
		{
			if (!(spawnCount <= 0f) && !string.IsNullOrEmpty(enemyType))
			{
				NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildEnemySpawnedEventName(enemyType), spawnCount, AnalyticsEventQuotaGroup.General);
			}
		}

		public void TrackEnemyKilled(string enemyType, float killCount)
		{
			if (!(killCount <= 0f) && !string.IsNullOrEmpty(enemyType))
			{
				NewDesignEvent(SessionAnalyticsEventNameExtensions.BuildEnemyKilledEventName(enemyType), killCount, AnalyticsEventQuotaGroup.General);
			}
		}

		public void TrackItemLoaded()
		{
			NewDesignEvent("item:loaded", AnalyticsEventQuotaGroup.General);
		}

		public void TrackPopupShown(string popupName)
		{
			NewDesignEvent(popupName + ":Shown", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingSearchStarted()
		{
			NewDesignEvent("matchmaking:search:started", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingSearchFound()
		{
			NewDesignEvent("matchmaking:search:found", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingSearchNotFound()
		{
			NewDesignEvent("matchmaking:search:not_found", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingPreviewJoin()
		{
			NewDesignEvent("matchmaking:preview:join", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingPreviewSkip()
		{
			NewDesignEvent("matchmaking:preview:skip", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingPreviewClosed()
		{
			NewDesignEvent("matchmaking:preview:closed", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingJoinSuccess()
		{
			NewDesignEvent("matchmaking:join:success", AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingJoinFailed(string reason)
		{
			NewDesignEvent("matchmaking:join:failed:" + reason, AnalyticsEventQuotaGroup.General);
		}

		public void TrackMatchmakingSessionStarted(int randomsCount, bool isPublic)
		{
			NewDesignEvent(string.Format("matchmaking:session_started:{0}_randoms:{1}", randomsCount, isPublic ? "public" : "private"), AnalyticsEventQuotaGroup.General);
		}

		public bool CanSendBatch(AnalyticsEventQuotaGroup quotaGroup, int count)
		{
			if (count <= 0)
			{
				return true;
			}
			if (HasDailyBudget(count))
			{
				return _eventQuota.CanSendBatch(quotaGroup, count);
			}
			return false;
		}

		public void TrackTutorialStarted()
		{
			TrackTutorialStepFinished("started");
		}

		public void TrackTutorialStepFinished(string stepName)
		{
			NewDesignEvent("tutorial:" + stepName, AnalyticsEventQuotaGroup.General);
		}

		public void NewDesignEvent(string eventName)
		{
			NewDesignEvent(eventName, AnalyticsEventQuotaGroup.General);
		}

		public void NewDesignEvent(string eventName, float value)
		{
			NewDesignEvent(eventName, value, AnalyticsEventQuotaGroup.General);
		}

		public void NewDesignEvent(string eventName, AnalyticsEventQuotaGroup quotaGroup)
		{
			if (CanSend(quotaGroup))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.Analytics, $"eventName : {eventName}, quotaGroup : {quotaGroup}");
				GameAnalytics.NewDesignEvent(eventName);
				RecordSentEvent(quotaGroup);
			}
		}

		public void NewDesignEvent(string eventName, float value, AnalyticsEventQuotaGroup quotaGroup)
		{
			if (CanSend(quotaGroup))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.Analytics, $"eventName : {eventName}, eventValue : {value}, quotaGroup : {quotaGroup}");
				GameAnalytics.NewDesignEvent(eventName, value);
				RecordSentEvent(quotaGroup);
			}
		}

		private bool CanSend(AnalyticsEventQuotaGroup quotaGroup)
		{
			if (HasDailyBudget())
			{
				return _eventQuota.HasRemaining(quotaGroup);
			}
			return false;
		}

		private bool HasDailyBudget(int count = 1)
		{
			return _sentAnalyticsModel.EventCount + count <= _gameAnalyticsEventsConfiguration.MaxEventsCountPerDay - _gameAnalyticsEventsConfiguration.ReservedEventsCount;
		}

		private void RecordSentEvent(AnalyticsEventQuotaGroup quotaGroup)
		{
			_sentAnalyticsModel.IncrementEventCount();
			_eventQuota.RecordSent(quotaGroup);
			if (_sentAnalyticsModel.EventCount % _gameAnalyticsEventsConfiguration.SaveEveryNEvents == 0)
			{
				_savingService.SaveDataForGroup(SavingGroup.Analytics);
			}
		}
	}
}
