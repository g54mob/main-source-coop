using Unity.Cinemachine;

namespace EvilCore.Recording
{
	public interface IBehaviorConfigurator
	{
		DirectorCameraBehavior BehaviorType { get; }

		void Configure(CinemachineCamera vcam, DirectorCameraData data);

		void UpdateRuntime(CinemachineCamera vcam, float deltaTime);

		DirectorCameraData ExtractSettings(CinemachineCamera vcam);

		void Cleanup(CinemachineCamera vcam);

		void SetSmoothing(float smoothing)
		{
		}

		void OnPositionUpdated(CinemachineCamera vcam)
		{
		}
	}
}
