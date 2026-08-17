using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class PanTiltBehavior : IBehaviorConfigurator
	{
		private float _startPan;

		private float _endPan;

		private float _startTilt;

		private float _endTilt;

		private float _speed;

		private float _progress;

		private Quaternion _baseRotation;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.PanTilt;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			PanTiltSettings panTiltSettings = data.panTiltSettings;
			_startPan = panTiltSettings.startPan;
			_endPan = panTiltSettings.endPan;
			_startTilt = panTiltSettings.startTilt;
			_endTilt = panTiltSettings.endTilt;
			_speed = panTiltSettings.speed;
			_progress = 0f;
			_baseRotation = vcam.transform.rotation;
			_smoothing = data.smoothing;
			_smoother.Reset();
			ApplyPanTilt(vcam, 0f);
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			_progress += _speed * deltaTime;
			if (_progress > 1f)
			{
				_progress = 1f;
			}
			float y = Mathf.Lerp(_startPan, _endPan, _progress);
			float x = Mathf.Lerp(_startTilt, _endTilt, _progress);
			Quaternion target = _baseRotation * Quaternion.Euler(x, y, 0f);
			vcam.transform.rotation = _smoother.Smooth(vcam.transform.rotation, target, _smoothing, deltaTime);
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.PanTilt,
				rotation = _baseRotation,
				panTiltSettings = new PanTiltSettings
				{
					startPan = _startPan,
					endPan = _endPan,
					startTilt = _startTilt,
					endTilt = _endTilt,
					speed = _speed
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
		}

		public void SetSmoothing(float smoothing)
		{
			_smoothing = smoothing;
		}

		private void ApplyPanTilt(CinemachineCamera vcam, float t)
		{
			float y = Mathf.Lerp(_startPan, _endPan, t);
			float x = Mathf.Lerp(_startTilt, _endTilt, t);
			vcam.transform.rotation = _baseRotation * Quaternion.Euler(x, y, 0f);
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
