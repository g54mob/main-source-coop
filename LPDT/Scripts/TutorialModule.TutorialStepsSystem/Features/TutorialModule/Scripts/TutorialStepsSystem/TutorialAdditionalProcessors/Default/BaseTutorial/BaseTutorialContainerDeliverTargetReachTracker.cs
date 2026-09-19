using Features.GrabModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialContainerDeliverTargetReachTracker : BaseTutorialTargetReachTracker
	{
		private BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialContainerDataHolder baseTutorialContainerDataHolder)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
		}

		protected override bool VerifyTarget(Collider other)
		{
			IPointGrabable componentInParent = other.GetComponentInParent<IPointGrabable>();
			if (componentInParent == null)
			{
				return false;
			}
			return componentInParent == _baseTutorialContainerDataHolder.ContainerEntity.ContainerItemsGrabber.CartGrabbable;
		}
	}
}
