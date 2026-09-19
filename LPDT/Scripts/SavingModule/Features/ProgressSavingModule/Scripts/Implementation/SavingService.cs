using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.ProgressSavingModule.Scripts.Implementation
{
	public class SavingService : ISavingService
	{
		private readonly ISavingManager _savingManager;

		private IEnumerable<ISavable> _savables;

		private Dictionary<SavingGroup, IEnumerable<ISavable>> _savablesBySavingGroup = new Dictionary<SavingGroup, IEnumerable<ISavable>>();

		public SavingService(IEnumerable<ISavable> savables, ISavingManager savingManager)
		{
			_savingManager = savingManager;
			IEnumerable<ISavable> source = (_savables = (savables as ISavable[]) ?? savables.ToArray());
			foreach (SavingGroup savingGroup in Enum.GetValues(typeof(SavingGroup)).Cast<SavingGroup>())
			{
				_savablesBySavingGroup.Add(savingGroup, source.Where((ISavable savable) => savable.SavingGroup == savingGroup));
			}
		}

		public void LoadAllData()
		{
			foreach (ISavable savable in _savables)
			{
				savable.LoadData();
			}
		}

		public void LoadDataForGroup(SavingGroup savingGroup)
		{
			foreach (ISavable item in _savablesBySavingGroup[savingGroup])
			{
				item.LoadData();
			}
		}

		public void SaveAllData()
		{
			foreach (ISavable savable in _savables)
			{
				savable.SaveData();
			}
			foreach (KeyValuePair<SavingGroup, IEnumerable<ISavable>> item in _savablesBySavingGroup)
			{
				CommitGroup(item.Key);
			}
		}

		public void SaveDataForGroup(SavingGroup savingGroup)
		{
			foreach (ISavable item in _savablesBySavingGroup[savingGroup])
			{
				item.SaveData();
			}
			CommitGroup(savingGroup);
		}

		public void SaveDataForGroups(List<SavingGroup> savingGroup)
		{
			foreach (SavingGroup item in savingGroup.Distinct())
			{
				SaveDataForGroup(item);
			}
		}

		private void CommitGroup(SavingGroup savingGroup)
		{
			if (savingGroup != SavingGroup.None)
			{
				_savingManager.Commit(savingGroup.ToString());
			}
		}
	}
}
