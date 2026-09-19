using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialLocationContainerRegistrar : MonoBehaviour
	{
		[SerializeField]
		private GameObject _locationContainerEntity;

		private BaseTutorialLocationContainerDataHolder _baseTutorialLocationContainerDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialLocationContainerDataHolder baseTutorialLocationContainerDataHolder)
		{
			_baseTutorialLocationContainerDataHolder = baseTutorialLocationContainerDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialLocationContainerDataHolder.LocationContainerEntity = _locationContainerEntity.GetComponent<IBaseTutorialLocationContainerEntity>();
		}

		private void OnDisable()
		{
			_baseTutorialLocationContainerDataHolder.LocationContainerEntity = null;
		}
	}
}
