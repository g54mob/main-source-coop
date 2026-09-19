using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialCheckpointSystem
{
	public class TutorialCheckPointModel : ISavable
	{
		private const string SAVE_ID = "SavedCheckpoint";

		private ISavingManager _savingManager;

		private TutorialSaveData _tutorialSaveData;

		public TutorialStep SavedTutorialStep
		{
			get
			{
				return _tutorialSaveData.SavedTutorialStep;
			}
			set
			{
				if (_tutorialSaveData.SavedTutorialStep != value)
				{
					_tutorialSaveData.SavedTutorialStep = value;
					this.OnSavedTutorialStepTypeChanged?.Invoke(_tutorialSaveData.SavedTutorialStep);
				}
			}
		}

		public SavingGroup SavingGroup => SavingGroup.GameData;

		public event Action<TutorialStep> OnSavedTutorialStepTypeChanged;

		[Inject]
		public void InjectDependencies(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		void ISavable.SaveData()
		{
			_savingManager.SaveDataWithID("SavedCheckpoint", _tutorialSaveData, SavingGroup.ToString());
		}

		void ISavable.LoadData()
		{
			_tutorialSaveData = _savingManager.LoadDataForID<TutorialSaveData>("SavedCheckpoint") ?? new TutorialSaveData();
		}
	}
}
