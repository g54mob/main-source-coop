using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ProgressSavingModule.Scripts.Implementation;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public class InitializeSentAnalyticsModelSystem : IInitializable
	{
		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly ISavingService _savingService;

		public InitializeSentAnalyticsModelSystem(SentAnalyticsModel sentAnalyticsModel, ISavingService savingService)
		{
			_sentAnalyticsModel = sentAnalyticsModel;
			_savingService = savingService;
		}

		public void Initialize()
		{
			DateTime lastSavedData = _sentAnalyticsModel.GetLastSavedData();
			if (!DayIsEqual(lastSavedData, DateTime.Now))
			{
				_sentAnalyticsModel.SetLastSavedData(DateTime.Now);
				_sentAnalyticsModel.ResetEventCount();
				_savingService.SaveDataForGroup(SavingGroup.Analytics);
			}
		}

		private bool DayIsEqual(DateTime a, DateTime b)
		{
			if (a.Year == b.Year && a.Month == b.Month)
			{
				return a.Day == b.Day;
			}
			return false;
		}
	}
}
