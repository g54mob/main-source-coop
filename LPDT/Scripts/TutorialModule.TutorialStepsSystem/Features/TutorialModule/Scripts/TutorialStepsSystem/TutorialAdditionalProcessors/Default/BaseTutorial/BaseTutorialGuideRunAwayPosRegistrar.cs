using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialGuideRunAwayPosRegistrar : MonoBehaviour
	{
		[SerializeField]
		private Transform _runAwayDestination;

		private BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder)
		{
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialEnemiesDataHolder.GuideRunAwayPosition = _runAwayDestination.position;
		}

		private void OnDisable()
		{
			_baseTutorialEnemiesDataHolder.GuideRunAwayPosition = null;
		}
	}
}
