using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.RunningSessionModule.Scripts
{
	public class RunningSessionPersistenceModel : ISavable
	{
		private const string SAVE_ID = "RunningSession";

		private readonly ISavingManager _savingManager;

		private RunningSessionPersistenceDataHolder _dataHolder = new RunningSessionPersistenceDataHolder();

		public SavingGroup SavingGroup => SavingGroup.RunningSession;

		public bool HasSavedSessionCode => !string.IsNullOrEmpty(_dataHolder.SavedSessionCode);

		public string SavedSessionCode => _dataHolder.SavedSessionCode;

		public RunningSessionPersistenceModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void SetSavedSessionCode(string sessionCode)
		{
			_dataHolder.SavedSessionCode = sessionCode;
		}

		public void LoadData()
		{
			RunningSessionPersistenceDataHolder runningSessionPersistenceDataHolder = _savingManager.LoadDataForID<RunningSessionPersistenceDataHolder>("RunningSession");
			if (runningSessionPersistenceDataHolder != null)
			{
				_dataHolder = runningSessionPersistenceDataHolder;
			}
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("RunningSession", _dataHolder, SavingGroup.ToString());
		}
	}
}
