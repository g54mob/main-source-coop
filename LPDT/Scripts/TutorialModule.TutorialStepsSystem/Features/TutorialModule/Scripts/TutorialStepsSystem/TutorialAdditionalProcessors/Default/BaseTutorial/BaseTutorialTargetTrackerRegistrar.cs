using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialTargetTrackerRegistrar : MonoBehaviour
	{
		[SerializeField]
		private BaseTutorialTargetReachTracker _baseTutorialTargetReachTracker;

		private BaseTutorialTargetTrackerDataHolder _baseTutorialTargetTrackerDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialTargetTrackerDataHolder baseTutorialTargetTrackerDataHolder)
		{
			_baseTutorialTargetTrackerDataHolder = baseTutorialTargetTrackerDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialTargetTrackerDataHolder.AddTargetReachTracker(_baseTutorialTargetReachTracker);
		}

		private void OnDisable()
		{
			_baseTutorialTargetTrackerDataHolder.RemoveTargetReachTracker(_baseTutorialTargetReachTracker);
		}
	}
}
