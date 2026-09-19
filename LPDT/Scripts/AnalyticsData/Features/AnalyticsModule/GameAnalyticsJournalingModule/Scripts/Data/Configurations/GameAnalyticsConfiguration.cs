using System.Collections.Generic;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations
{
	[CreateAssetMenu(fileName = "GameAnalyticsConfiguration_Default", menuName = "Configurations/GameAnalytics/GameAnalyticsConfiguration")]
	public class GameAnalyticsConfiguration : ScriptableObject
	{
		[SerializeField]
		private bool _isGameAnalyticsEnabled = true;

		[SerializeField]
		private bool _isDebugModeEnabled;

		[SerializeField]
		private List<RuntimePlatform> _deviceTypesWithAutoSessionHandling;

		[SerializeField]
		private GameAnalyticsGameConfiguration _debugGameConfiguration;

		public bool IsGameAnalyticsEnabled => _isGameAnalyticsEnabled;

		public bool IsDebugModeEnabled => _isDebugModeEnabled;

		public List<RuntimePlatform> DeviceTypesWithAutoSessionHandling => _deviceTypesWithAutoSessionHandling;

		public GameAnalyticsGameConfiguration DebugGameConfiguration => _debugGameConfiguration;
	}
}
