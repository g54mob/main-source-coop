using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;
using Features.GameJournalingModule.Scripts.AnalyticsMarkService;
using Features.GameJournalingModule.Scripts.Core;
using GameAnalyticsSDK;
using UnityEngine;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public class GameAnalyticsJournalingSystem : IConcreteJournalingSystem, IInitializable, IDisposable
	{
		private readonly GameAnalyticsConfiguration _gameAnalyticsConfiguration;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly GameAnalyticsMarkUserService _gameAnalyticsMarkUserService;

		private readonly ISessionEndAnalyticsService _sessionEndAnalyticsService;

		private bool _isManualSessionHandlingEnabled = true;

		private bool _isGameAnalyticsEnabled;

		public bool IsEnabled => _isGameAnalyticsEnabled;

		public IAnalyticsEventSendService AnalyticsEventSendService => _gameAnalyticsEventSendService;

		public IAnalyticsMarkUserService AnalyticsMarkUserService => _gameAnalyticsMarkUserService;

		public GameAnalyticsJournalingSystem(GameAnalyticsConfiguration gameAnalyticsConfiguration, GameAnalyticsEventSendService gameAnalyticsEventSendService, GameAnalyticsMarkUserService gameAnalyticsMarkUserService, ISessionEndAnalyticsService sessionEndAnalyticsService)
		{
			_gameAnalyticsConfiguration = gameAnalyticsConfiguration;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_gameAnalyticsMarkUserService = gameAnalyticsMarkUserService;
			_sessionEndAnalyticsService = sessionEndAnalyticsService;
		}

		public void Initialize()
		{
			Application.quitting += HandleApplicationQuit;
			StartSession();
		}

		public void Dispose()
		{
			Application.quitting -= HandleApplicationQuit;
		}

		public void HandleApplicationQuit()
		{
			_sessionEndAnalyticsService.PrepareForApplicationQuit();
			_sessionEndAnalyticsService.TrySendSessionEnd();
			EndSession();
		}

		private void StartSession()
		{
			if (IsGameAnalyticsDisabled())
			{
				Debug.LogError("GameAnalytics is disabled");
				return;
			}
			_isGameAnalyticsEnabled = true;
			_isManualSessionHandlingEnabled = IsApplicationRequireManualSessionHandling();
			if (Application.isEditor || Debug.isDebugBuild)
			{
				_gameAnalyticsMarkUserService.MarkAsDebugBuild();
				_gameAnalyticsMarkUserService.MarkAsDeveloper();
			}
			else
			{
				_gameAnalyticsMarkUserService.MarkAsReleaseBuild();
			}
		}

		private bool IsApplicationRequireManualSessionHandling()
		{
			return !_gameAnalyticsConfiguration.DeviceTypesWithAutoSessionHandling.Contains(Application.platform);
		}

		private void EndSession()
		{
			if (!IsGameAnalyticsDisabled() && _isManualSessionHandlingEnabled)
			{
				GameAnalytics.EndSession();
			}
		}

		private bool IsGameAnalyticsDisabled()
		{
			if (!_gameAnalyticsConfiguration.IsGameAnalyticsEnabled)
			{
				return true;
			}
			if (!GameAnalytics.SettingsGA.Platforms.Contains(Application.platform) && !Application.isEditor)
			{
				return true;
			}
			if (Debug.isDebugBuild)
			{
				return !_gameAnalyticsConfiguration.IsDebugModeEnabled;
			}
			return false;
		}
	}
}
