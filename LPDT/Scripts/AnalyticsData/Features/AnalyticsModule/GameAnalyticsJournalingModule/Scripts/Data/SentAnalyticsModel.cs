using System;
using System.Collections.Generic;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	public class SentAnalyticsModel : ISavable
	{
		private const string SAVE_ID = "SentAnalytics";

		private readonly ISavingManager _savingManager;

		private SentAnalyticsDataHolder _dataHolder = new SentAnalyticsDataHolder();

		public SavingGroup SavingGroup => SavingGroup.Analytics;

		public int EventCount => _dataHolder.EventCount;

		public int SessionStartedCount => _dataHolder.SessionStartedCount;

		public SentAnalyticsModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public int GetQuotaCount(AnalyticsEventQuotaGroup quotaGroup)
		{
			EnsureQuotaCountsInitialized();
			for (int i = 0; i < _dataHolder.QuotaCounts.Count; i++)
			{
				if (_dataHolder.QuotaCounts[i].Group == (int)quotaGroup)
				{
					return _dataHolder.QuotaCounts[i].Count;
				}
			}
			return 0;
		}

		public DateTime GetLastSavedData()
		{
			if (_dataHolder.DateTimeTicks != 0L)
			{
				return new DateTime(_dataHolder.DateTimeTicks);
			}
			return DateTime.MinValue;
		}

		public void SetLastSavedData(DateTime dateTime)
		{
			_dataHolder.DateTimeTicks = dateTime.Ticks;
		}

		public void IncrementEventCount(int count = 1)
		{
			_dataHolder.EventCount += count;
		}

		public void ResetEventCount()
		{
			_dataHolder.EventCount = 0;
			_dataHolder.QuotaCounts.Clear();
		}

		public void IncrementQuotaCount(AnalyticsEventQuotaGroup quotaGroup, int count = 1)
		{
			if (count <= 0)
			{
				return;
			}
			EnsureQuotaCountsInitialized();
			for (int i = 0; i < _dataHolder.QuotaCounts.Count; i++)
			{
				if (_dataHolder.QuotaCounts[i].Group == (int)quotaGroup)
				{
					_dataHolder.QuotaCounts[i].Count += count;
					return;
				}
			}
			_dataHolder.QuotaCounts.Add(new AnalyticsQuotaCountEntry
			{
				Group = (int)quotaGroup,
				Count = count
			});
		}

		public int IncrementSessionStartedCount()
		{
			return ++_dataHolder.SessionStartedCount;
		}

		public void LoadData()
		{
			SentAnalyticsDataHolder sentAnalyticsDataHolder = _savingManager.LoadDataForID<SentAnalyticsDataHolder>("SentAnalytics");
			if (sentAnalyticsDataHolder != null)
			{
				_dataHolder = sentAnalyticsDataHolder;
				EnsureQuotaCountsInitialized();
			}
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("SentAnalytics", _dataHolder, SavingGroup.ToString());
		}

		private void EnsureQuotaCountsInitialized()
		{
			SentAnalyticsDataHolder dataHolder = _dataHolder;
			if (dataHolder.QuotaCounts == null)
			{
				dataHolder.QuotaCounts = new List<AnalyticsQuotaCountEntry>();
			}
		}
	}
}
