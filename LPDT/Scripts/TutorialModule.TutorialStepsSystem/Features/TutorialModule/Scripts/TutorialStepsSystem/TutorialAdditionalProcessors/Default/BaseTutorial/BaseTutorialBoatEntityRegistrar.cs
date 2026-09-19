using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialBoatEntityRegistrar : MonoBehaviour
	{
		[SerializeField]
		private BaseTutorialBoatEntity _boatEntity;

		private BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialContainerDataHolder baseTutorialContainerDataHolder)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialContainerDataHolder.BoatEntity = _boatEntity;
		}

		private void OnDisable()
		{
			_baseTutorialContainerDataHolder.BoatEntity = null;
		}
	}
}
