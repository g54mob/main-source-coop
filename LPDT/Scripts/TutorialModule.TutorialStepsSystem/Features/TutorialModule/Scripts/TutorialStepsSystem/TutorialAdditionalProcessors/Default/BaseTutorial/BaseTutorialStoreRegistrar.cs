using Features.TutorialModule.Scripts.GuideModule;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialStoreRegistrar : MonoBehaviour
	{
		[SerializeField]
		private TipType _tipType;

		[SerializeField]
		private Transform _tipHolder;

		private BaseTutorialStoreDataHolder _baseTutorialStoreDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialStoreDataHolder baseTutorialStoreDataHolder)
		{
			_baseTutorialStoreDataHolder = baseTutorialStoreDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialStoreDataHolder.AddTipHolder(_tipType, _tipHolder);
		}

		private void OnDisable()
		{
			_baseTutorialStoreDataHolder.RemoveTipHolder(_tipType);
		}
	}
}
