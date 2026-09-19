using Unity.Cinemachine;

namespace Features.ScreenShakeModule.Scripts
{
	public class ScreenShakeService : IScreenShakeService
	{
		private readonly CameraLocalScreenShakeModel _cameraLocalScreenShakeModel;

		public ScreenShakeService(CameraLocalScreenShakeModel cameraLocalScreenShakeModel)
		{
			_cameraLocalScreenShakeModel = cameraLocalScreenShakeModel;
		}

		public void TriggerLocalScreenShake(ScreenShakeData screenShakeData)
		{
			_cameraLocalScreenShakeModel.TriggerScreenShake(screenShakeData);
		}

		public void TriggerScreenShake(CinemachineImpulseSource impulseSource, ScreenShakeData screenShakeData)
		{
			impulseSource.ImpulseDefinition.TimeEnvelope.AttackTime = screenShakeData.InTime;
			impulseSource.ImpulseDefinition.TimeEnvelope.DecayTime = screenShakeData.OutTime;
			impulseSource.ImpulseDefinition.FrequencyGain = screenShakeData.FrequencyGain;
			impulseSource.GenerateImpulseWithForce(screenShakeData.Force * _cameraLocalScreenShakeModel.ScreenShakeIntensityNormalized);
		}
	}
}
