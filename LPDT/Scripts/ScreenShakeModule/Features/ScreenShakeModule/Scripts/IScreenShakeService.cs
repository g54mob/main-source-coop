using Unity.Cinemachine;

namespace Features.ScreenShakeModule.Scripts
{
	public interface IScreenShakeService
	{
		void TriggerLocalScreenShake(ScreenShakeData screenShakeData);

		void TriggerScreenShake(CinemachineImpulseSource impulseSource, ScreenShakeData screenShakeData);
	}
}
