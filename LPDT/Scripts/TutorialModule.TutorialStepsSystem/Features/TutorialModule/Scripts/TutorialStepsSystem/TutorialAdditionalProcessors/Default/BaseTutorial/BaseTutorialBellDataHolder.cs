using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialBellDataHolder
	{
		private BaseTutorialBellEntity _bellEntity;

		public BaseTutorialBellEntity BellEntity
		{
			get
			{
				return _bellEntity;
			}
			set
			{
				_bellEntity = value;
				this.OnBellEntityChanged?.Invoke(_bellEntity);
			}
		}

		public event Action<BaseTutorialBellEntity> OnBellEntityChanged;
	}
}
