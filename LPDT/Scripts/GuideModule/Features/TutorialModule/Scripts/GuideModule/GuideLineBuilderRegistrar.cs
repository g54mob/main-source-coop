using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLineBuilderRegistrar : MonoBehaviour
	{
		[SerializeField]
		private GuideLineBuilder _guideLineBuilder;

		private GuideLineBuilderDataHolder _guideLineBuilderDataHolder;

		[Inject]
		public void InjectDependencies(GuideLineBuilderDataHolder guideLineBuilderDataHolder)
		{
			_guideLineBuilderDataHolder = guideLineBuilderDataHolder;
		}

		private void OnEnable()
		{
			_guideLineBuilderDataHolder.GuideLineBuilder = _guideLineBuilder;
		}

		private void OnDisable()
		{
			_guideLineBuilderDataHolder.GuideLineBuilder = null;
		}
	}
}
