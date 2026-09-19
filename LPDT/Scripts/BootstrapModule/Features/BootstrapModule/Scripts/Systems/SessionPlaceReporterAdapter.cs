using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionPlaceReporterAdapter : ISessionPlaceReporter
	{
		private readonly SessionEndAnalyticsContextModel _sessionEndAnalyticsContextModel;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		public SessionPlaceReporterAdapter(SessionEndAnalyticsContextModel sessionEndAnalyticsContextModel, Features.LevelModule.Scripts.LevelModel levelModel)
		{
			_sessionEndAnalyticsContextModel = sessionEndAnalyticsContextModel;
			_levelModel = levelModel;
		}

		public void ReportLevelReached()
		{
			_sessionEndAnalyticsContextModel.UnregisterPlace(SessionEndAnalyticsPlaceKind.Loading);
			_sessionEndAnalyticsContextModel.RegisterPlace(SessionEndAnalyticsPlaceKind.Beach, _levelModel.CurrentSequenceLevelNumber);
			_sessionEndAnalyticsContextModel.UnregisterPlace(SessionEndAnalyticsPlaceKind.Level, _levelModel.CurrentSequenceLevelNumber);
		}
	}
}
