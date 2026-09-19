namespace Features.ProgressSavingModule.Scripts.Implementation
{
	public interface ISavable
	{
		SavingGroup SavingGroup { get; }

		void LoadData();

		void SaveData();
	}
}
