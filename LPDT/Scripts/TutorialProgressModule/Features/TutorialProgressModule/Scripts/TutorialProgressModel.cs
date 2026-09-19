using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.TutorialProgressModule.Scripts
{
	public class TutorialProgressModel : ISavable
	{
		private const string SAVE_ID = "TutorialProgress";

		private readonly ISavingManager _savingManager;

		private TutorialProgressDataHolder _dataHolder = new TutorialProgressDataHolder();

		public SavingGroup SavingGroup => SavingGroup.TutorialProgress;

		public bool QuotaContainerTokenShown => _dataHolder.QuotaContainerTokenShown;

		public bool CardTableBellTokenShown => _dataHolder.CardTableBellTokenShown;

		public TutorialProgressModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void MarkQuotaContainerTokenShown()
		{
			_dataHolder.QuotaContainerTokenShown = true;
		}

		public void MarkCardTableBellTokenShown()
		{
			_dataHolder.CardTableBellTokenShown = true;
		}

		public void LoadData()
		{
			TutorialProgressDataHolder tutorialProgressDataHolder = _savingManager.LoadDataForID<TutorialProgressDataHolder>("TutorialProgress");
			if (tutorialProgressDataHolder != null)
			{
				_dataHolder = tutorialProgressDataHolder;
			}
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("TutorialProgress", _dataHolder, SavingGroup.ToString());
		}
	}
}
