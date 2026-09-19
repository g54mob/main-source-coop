using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public class GameAnalyticsInitialization : MonoBehaviour
	{
		private const string BOOTSTRAP_SCENE_NAME = "BootstrapScene";

		[SerializeField]
		private GameAnalyticsConfiguration _gameAnalyticsConfiguration;

		private bool _isManualSessionHandlingEnabled;

		private void Start()
		{
			if (!IsGameAnalyticsDisabled())
			{
				Object.DontDestroyOnLoad(base.gameObject);
				InitializeAnalytics();
				Addressables.LoadSceneAsync("BootstrapScene");
			}
		}

		private void InitializeAnalytics()
		{
			_isManualSessionHandlingEnabled = IsApplicationRequireManualSessionHandling();
			GameAnalytics.SetEnabledManualSessionHandling(_isManualSessionHandlingEnabled);
			GameAnalytics.Initialize();
			if (_isManualSessionHandlingEnabled)
			{
				GameAnalytics.StartSession();
			}
		}

		private bool IsApplicationRequireManualSessionHandling()
		{
			return !_gameAnalyticsConfiguration.DeviceTypesWithAutoSessionHandling.Contains(Application.platform);
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
