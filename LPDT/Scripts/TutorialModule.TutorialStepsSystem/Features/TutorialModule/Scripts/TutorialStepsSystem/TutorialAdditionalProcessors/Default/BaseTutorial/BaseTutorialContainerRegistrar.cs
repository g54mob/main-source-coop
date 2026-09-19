using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialContainerRegistrar : MonoBehaviour
	{
		[SerializeField]
		private TutorialContainerEntity _container;

		private BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialContainerDataHolder baseTutorialContainerDataHolder)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialContainerDataHolder.ContainerEntity = _container;
		}

		private void OnDisable()
		{
			_baseTutorialContainerDataHolder.ContainerEntity = null;
		}
	}
}
