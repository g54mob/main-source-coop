using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialUIRegistrar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _keysTipHolder;

		[SerializeField]
		private RectTransform _ctrlTipHolder;

		[SerializeField]
		private RectTransform _mouseUpTipHolder;

		[SerializeField]
		private RectTransform _turnAroundTipHolder;

		private BaseTutorialUIDataHolder _baseTutorialUIDataHolder;

		[Inject]
		public void InjectDependencies(BaseTutorialUIDataHolder baseTutorialUIDataHolder)
		{
			_baseTutorialUIDataHolder = baseTutorialUIDataHolder;
		}

		private void OnEnable()
		{
			_baseTutorialUIDataHolder.KeysTipHolder = _keysTipHolder;
			_baseTutorialUIDataHolder.CtrlTipHolder = _ctrlTipHolder;
			_baseTutorialUIDataHolder.MouseUpTipHolder = _mouseUpTipHolder;
			_baseTutorialUIDataHolder.TurnAroundTipHolder = _turnAroundTipHolder;
		}

		private void OnDisable()
		{
			_baseTutorialUIDataHolder.KeysTipHolder = null;
			_baseTutorialUIDataHolder.CtrlTipHolder = null;
			_baseTutorialUIDataHolder.MouseUpTipHolder = null;
			_baseTutorialUIDataHolder.TurnAroundTipHolder = null;
		}
	}
}
