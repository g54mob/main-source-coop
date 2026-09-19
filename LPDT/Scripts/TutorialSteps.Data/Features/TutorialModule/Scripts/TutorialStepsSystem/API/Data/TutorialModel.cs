using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data
{
	public class TutorialModel : ISavable
	{
		private const string SAVE_ID = "TutorialSaveData";

		private readonly ISavingManager _savingManager;

		private bool _isBaseTutorialCompleted;

		public SavingGroup SavingGroup => SavingGroup.GameData;

		public bool IsBaseTutorialCompleted
		{
			get
			{
				return _isBaseTutorialCompleted;
			}
			set
			{
				_isBaseTutorialCompleted = value;
				if (_isBaseTutorialCompleted)
				{
					IsBaseTutorialSequenceInvoked = false;
					this.OnBaseTutorialCompleted?.Invoke();
				}
			}
		}

		public bool IsBaseTutorialSequenceInvoked { get; set; }

		public bool IsTutorialInProgress { get; private set; }

		public event Action OnBaseTutorialCompleted;

		public event Action OnIsTutorialInProgressChanged;

		public TutorialModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void SetIsTutorialInProgress(bool isTutorialInProgress)
		{
			IsTutorialInProgress = isTutorialInProgress;
			this.OnIsTutorialInProgressChanged?.Invoke();
		}

		void ISavable.LoadData()
		{
			TutorialSaveDataHolder tutorialSaveDataHolder = _savingManager.LoadDataForID<TutorialSaveDataHolder>("TutorialSaveData");
			if (tutorialSaveDataHolder != null)
			{
				IsBaseTutorialCompleted = tutorialSaveDataHolder.IsBaseTutorialCompleted;
			}
		}

		void ISavable.SaveData()
		{
			TutorialSaveDataHolder dataObject = new TutorialSaveDataHolder
			{
				IsBaseTutorialCompleted = IsBaseTutorialCompleted
			};
			_savingManager.SaveDataWithID("TutorialSaveData", dataObject, SavingGroup.ToString());
		}
	}
}
