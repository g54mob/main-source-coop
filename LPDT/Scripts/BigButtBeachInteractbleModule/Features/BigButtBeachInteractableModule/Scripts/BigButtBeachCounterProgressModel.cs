using System.Collections.Generic;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachCounterProgressModel : ISavable
	{
		private const string SAVE_ID = "BigButtBeachCounterProgress";

		private readonly ISavingManager _savingManager;

		private BigButtBeachCounterProgressDataHolder _dataHolder = new BigButtBeachCounterProgressDataHolder();

		public SavingGroup SavingGroup => SavingGroup.BigButtBeachCounterProgress;

		public BigButtBeachCounterProgressModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public bool TryGetProgress(string statAPIString, out long currentValue, out long steamMaxValue, out long savedUtcTicks)
		{
			EnsureEntriesInitialized();
			for (int i = 0; i < _dataHolder.Entries.Count; i++)
			{
				BigButtBeachCounterProgressEntry bigButtBeachCounterProgressEntry = _dataHolder.Entries[i];
				if (!(bigButtBeachCounterProgressEntry.StatAPIString != statAPIString))
				{
					currentValue = bigButtBeachCounterProgressEntry.CurrentValue;
					steamMaxValue = bigButtBeachCounterProgressEntry.SteamMaxValue;
					savedUtcTicks = bigButtBeachCounterProgressEntry.SavedUtcTicks;
					return true;
				}
			}
			currentValue = 0L;
			steamMaxValue = 0L;
			savedUtcTicks = 0L;
			return false;
		}

		public void SetProgress(string statAPIString, long currentValue, long steamMaxValue, long savedUtcTicks)
		{
			EnsureEntriesInitialized();
			for (int i = 0; i < _dataHolder.Entries.Count; i++)
			{
				BigButtBeachCounterProgressEntry bigButtBeachCounterProgressEntry = _dataHolder.Entries[i];
				if (!(bigButtBeachCounterProgressEntry.StatAPIString != statAPIString))
				{
					bigButtBeachCounterProgressEntry.CurrentValue = currentValue;
					bigButtBeachCounterProgressEntry.SteamMaxValue = steamMaxValue;
					bigButtBeachCounterProgressEntry.SavedUtcTicks = savedUtcTicks;
					return;
				}
			}
			_dataHolder.Entries.Add(new BigButtBeachCounterProgressEntry
			{
				StatAPIString = statAPIString,
				CurrentValue = currentValue,
				SteamMaxValue = steamMaxValue,
				SavedUtcTicks = savedUtcTicks
			});
		}

		public void LoadData()
		{
			BigButtBeachCounterProgressDataHolder bigButtBeachCounterProgressDataHolder = _savingManager.LoadDataForID<BigButtBeachCounterProgressDataHolder>("BigButtBeachCounterProgress");
			if (bigButtBeachCounterProgressDataHolder != null)
			{
				_dataHolder = bigButtBeachCounterProgressDataHolder;
			}
			EnsureEntriesInitialized();
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("BigButtBeachCounterProgress", _dataHolder, SavingGroup.ToString());
		}

		private void EnsureEntriesInitialized()
		{
			BigButtBeachCounterProgressDataHolder dataHolder = _dataHolder;
			if (dataHolder.Entries == null)
			{
				dataHolder.Entries = new List<BigButtBeachCounterProgressEntry>();
			}
		}
	}
}
