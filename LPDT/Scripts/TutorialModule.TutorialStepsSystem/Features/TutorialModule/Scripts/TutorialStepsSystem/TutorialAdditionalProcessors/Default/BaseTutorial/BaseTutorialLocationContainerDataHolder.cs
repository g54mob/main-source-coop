using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialLocationContainerDataHolder
	{
		private IBaseTutorialLocationContainerEntity _locationContainerEntity;

		public IBaseTutorialLocationContainerEntity LocationContainerEntity
		{
			get
			{
				return _locationContainerEntity;
			}
			set
			{
				_locationContainerEntity = value;
				this.OnLocationContainerEntityChanged?.Invoke(_locationContainerEntity);
			}
		}

		public event Action<IBaseTutorialLocationContainerEntity> OnLocationContainerEntityChanged;
	}
}
