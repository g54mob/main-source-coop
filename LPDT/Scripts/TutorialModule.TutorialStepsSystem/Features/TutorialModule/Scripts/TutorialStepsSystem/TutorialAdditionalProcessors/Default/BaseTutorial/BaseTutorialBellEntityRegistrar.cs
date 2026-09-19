using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialBellEntityRegistrar : MonoBehaviour
	{
		[SerializeField]
		private BaseTutorialBellEntity _bellEntity;

		private BaseTutorialBellDataHolder _baseTutorialBellDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialBellDataHolder baseTutorialBellDataHolder)
		{
			_baseTutorialBellDataHolder = baseTutorialBellDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialBellDataHolder.BellEntity = _bellEntity;
		}

		private void OnDisable()
		{
			_baseTutorialBellDataHolder.BellEntity = null;
		}
	}
}
