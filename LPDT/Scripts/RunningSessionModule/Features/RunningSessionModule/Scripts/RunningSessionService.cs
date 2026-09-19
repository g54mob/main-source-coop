using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;

namespace Features.RunningSessionModule.Scripts
{
	public class RunningSessionService : IRunningSessionService
	{
		private readonly RunningSessionModel _runningSessionModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly RunningSessionPersistenceModel _persistenceModel;

		private readonly ISavingService _savingService;

		public RunningSessionService(RunningSessionModel runningSessionModel, MultiplayerModel multiplayerModel, RunningSessionPersistenceModel persistenceModel, ISavingService savingService)
		{
			_runningSessionModel = runningSessionModel;
			_multiplayerModel = multiplayerModel;
			_persistenceModel = persistenceModel;
			_savingService = savingService;
		}

		public void InitializeSessionCode(string sessionCode)
		{
			_runningSessionModel.SessionCode = sessionCode;
		}

		public void SetSessionCodeSaved(string sessionCode)
		{
			_runningSessionModel.SessionCode = sessionCode;
			_persistenceModel.SetSavedSessionCode(sessionCode);
			_savingService.SaveDataForGroup(SavingGroup.RunningSession);
		}

		public bool TryGetSavedSessionCode(out string sessionCode)
		{
			if (!_persistenceModel.HasSavedSessionCode)
			{
				sessionCode = null;
				return false;
			}
			sessionCode = _persistenceModel.SavedSessionCode;
			return true;
		}

		public void IncreaseCurrentGameIndex()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_runningSessionModel.CurrentGameIndex++;
				_runningSessionModel.Synchronize();
			}
		}
	}
}
