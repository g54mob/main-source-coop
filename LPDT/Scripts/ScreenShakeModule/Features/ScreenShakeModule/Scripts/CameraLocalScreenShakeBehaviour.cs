using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.ScreenShakeModule.Scripts
{
	public class CameraLocalScreenShakeBehaviour : MonoBehaviour
	{
		[SerializeField]
		private CinemachineImpulseSource _cinemachineImpulseSource;

		private CameraLocalScreenShakeModel _cameraLocalScreenShakeModel;

		private IScreenShakeService _screenShakeService;

		[Inject]
		public void InjectDependencies(CameraLocalScreenShakeModel cameraLocalScreenShakeModel, IScreenShakeService screenShakeService)
		{
			_cameraLocalScreenShakeModel = cameraLocalScreenShakeModel;
			_screenShakeService = screenShakeService;
		}

		private void OnEnable()
		{
			_cameraLocalScreenShakeModel.OnScreenShakeTriggered += Impulse;
		}

		private void OnDisable()
		{
			_cameraLocalScreenShakeModel.OnScreenShakeTriggered -= Impulse;
		}

		private void Impulse(ScreenShakeData screenShakeData)
		{
			_screenShakeService.TriggerScreenShake(_cinemachineImpulseSource, screenShakeData);
		}
	}
}
