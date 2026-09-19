using System.Collections.Generic;

namespace Features.ProgressSavingModule.Scripts.Implementation
{
	public interface ISavingService
	{
		void LoadAllData();

		void LoadDataForGroup(SavingGroup savingGroup);

		void SaveAllData();

		void SaveDataForGroup(SavingGroup savingGroup);

		void SaveDataForGroups(List<SavingGroup> savingGroup);
	}
}
