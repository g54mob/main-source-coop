using Features.ScreenShakeModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class ScreenShakeOnNextLevelTrigger : MonoBehaviour
	{
		[SerializeField]
		private ScreenShakeData _screenShakeData;

		private QuotaCompletionModel _quotaCompletionModel;

		private IScreenShakeService _screenShakeService;

		[Inject]
		private void InjectDependencies(QuotaCompletionModel quotaCompletionModel, IScreenShakeService screenShakeService)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_screenShakeService = screenShakeService;
		}

		private void Start()
		{
			_quotaCompletionModel.OnBellActivated += TriggerScreenShake;
		}

		private void OnDestroy()
		{
			_quotaCompletionModel.OnBellActivated -= TriggerScreenShake;
		}

		private void TriggerScreenShake(bool isActivated)
		{
			if (isActivated)
			{
				_screenShakeService.TriggerLocalScreenShake(_screenShakeData);
			}
		}
	}
}
