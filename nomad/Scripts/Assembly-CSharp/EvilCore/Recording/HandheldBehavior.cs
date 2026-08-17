using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class HandheldBehavior : IBehaviorConfigurator
	{
		private float _amplitudeGain;

		private float _frequencyGain;

		private float _time;

		private Vector3 _basePosition;

		private Quaternion _baseRotation;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Handheld;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			HandheldSettings handheldSettings = data.handheldSettings;
			_amplitudeGain = handheldSettings.amplitudeGain;
			_frequencyGain = handheldSettings.frequencyGain;
			_time = 0f;
			_basePosition = vcam.transform.position;
			_baseRotation = vcam.transform.rotation;
			_smoothing = data.smoothing;
			_smoother.Reset();
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			_time += deltaTime * _frequencyGain;
			float x = (Mathf.PerlinNoise(_time * 1.1f, 0f) - 0.5f) * 2f * _amplitudeGain * 0.02f;
			float y = (Mathf.PerlinNoise(0f, _time * 1.3f) - 0.5f) * 2f * _amplitudeGain * 0.02f;
			float z = (Mathf.PerlinNoise(_time * 0.9f, _time * 0.7f) - 0.5f) * 2f * _amplitudeGain * 0.01f;
			float x2 = (Mathf.PerlinNoise(_time * 0.8f, 100f) - 0.5f) * _amplitudeGain * 0.5f;
			float y2 = (Mathf.PerlinNoise(100f, _time * 0.6f) - 0.5f) * _amplitudeGain * 0.5f;
			Vector3 target = _basePosition + new Vector3(x, y, z);
			Quaternion target2 = _baseRotation * Quaternion.Euler(x2, y2, 0f);
			vcam.transform.position = _smoother.Smooth(vcam.transform.position, target, _smoothing, deltaTime);
			vcam.transform.rotation = _smoother.Smooth(vcam.transform.rotation, target2, _smoothing, deltaTime);
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Handheld,
				position = _basePosition,
				rotation = _baseRotation,
				handheldSettings = new HandheldSettings
				{
					amplitudeGain = _amplitudeGain,
					frequencyGain = _frequencyGain
				}
			};
			if (vcam != null)
			{
				directorCameraData.position = vcam.transform.position;
				directorCameraData.rotation = vcam.transform.rotation;
				directorCameraData.fieldOfView = vcam.Lens.FieldOfView;
			}
			return directorCameraData;
		}

		public void Cleanup(CinemachineCamera vcam)
		{
			RemoveAllPipelineComponents(vcam);
			vcam.transform.position = _basePosition;
			vcam.transform.rotation = _baseRotation;
		}

		public void SetSmoothing(float smoothing)
		{
			_smoothing = smoothing;
		}

		public void SetAmplitude(float amplitude)
		{
			_amplitudeGain = amplitude;
		}

		public void SetFrequency(float frequency)
		{
			_frequencyGain = frequency;
		}

		private static void RemoveAllPipelineComponents(CinemachineCamera vcam)
		{
			if (!(vcam == null))
			{
				CinemachineComponentBase[] components = vcam.GetComponents<CinemachineComponentBase>();
				for (int i = 0; i < components.Length; i++)
				{
					Object.Destroy(components[i]);
				}
			}
		}
	}
}
