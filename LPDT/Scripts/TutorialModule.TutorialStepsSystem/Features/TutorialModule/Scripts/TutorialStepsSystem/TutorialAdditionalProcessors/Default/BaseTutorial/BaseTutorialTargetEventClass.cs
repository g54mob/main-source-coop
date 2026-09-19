using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialTargetEventClass
	{
		public event Action<BaseTutorialTargetType> OnTargetReached;

		public void Invoke(BaseTutorialTargetType baseTutorialTargetType)
		{
			this.OnTargetReached?.Invoke(baseTutorialTargetType);
		}
	}
}
