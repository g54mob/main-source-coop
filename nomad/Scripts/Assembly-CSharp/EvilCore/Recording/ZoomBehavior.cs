using System;
using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class ZoomBehavior : IBehaviorConfigurator
	{
		private float _startFOV;

		private float _endFOV;

		private float _duration;

		private float _elapsed;

		private Transform _targetTransform;

		private float _distance;

		private float _height;

		private float _angle;

		private float _followSmoothTime;

		private float _lookAtSmoothTime;

		private float _lookAtHeightOffset;

		private bool _lookAtTarget;

		private float _shakeAmplitude;

		private float _shakeFrequency;

		private Vector3 _positionVelocity;

		private float _shakeTime;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Zoom;

		public Transform TargetTransform => _targetTransform;

		public bool HasTarget => _targetTransform != null;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			ZoomSettings zoomSettings = data.zoomSettings;
			_startFOV = zoomSettings.startFOV;
			_endFOV = zoomSettings.endFOV;
			_duration = zoomSettings.duration;
			_elapsed = 0f;
			_distance = zoomSettings.distance;
			_height = zoomSettings.height;
			_angle = zoomSettings.angle;
			_followSmoothTime = zoomSettings.followSmoothTime;
			_lookAtSmoothTime = zoomSettings.lookAtSmoothTime;
			_lookAtHeightOffset = zoomSettings.lookAtHeightOffset;
			_lookAtTarget = zoomSettings.lookAtTarget;
			_shakeAmplitude = zoomSettings.shakeAmplitude;
			_shakeFrequency = zoomSettings.shakeFrequency;
			_positionVelocity = Vector3.zero;
			_shakeTime = 0f;
			_smoothing = data.smoothing;
			_smoother.Reset();
			LensSettings lens = vcam.Lens;
			lens.FieldOfView = _startFOV;
			vcam.Lens = lens;
			if (_targetTransform != null)
			{
				vcam.transform.position = ComputeDesiredPosition();
				ApplyLookAt(vcam, instant: true);
			}
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			if (_duration > 0f)
			{
				_elapsed += deltaTime;
				float t = Mathf.Clamp01(_elapsed / _duration);
				float fieldOfView = Mathf.Lerp(_startFOV, _endFOV, t);
				LensSettings lens = vcam.Lens;
				lens.FieldOfView = fieldOfView;
				vcam.Lens = lens;
			}
			if (_targetTransform != null)
			{
				Vector3 target = ComputeDesiredPosition();
				float smoothTime = _followSmoothTime + _smoothing * 0.3f;
				vcam.transform.position = Vector3.SmoothDamp(vcam.transform.position, target, ref _positionVelocity, smoothTime, 1f / 0f, deltaTime);
				if (_lookAtTarget)
				{
					ApplyLookAt(vcam, instant: false);
				}
				if (_shakeAmplitude > 0f)
				{
					ApplyShake(vcam, deltaTime);
				}
			}
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Zoom,
				zoomSettings = new ZoomSettings
				{
					startFOV = _startFOV,
					endFOV = _endFOV,
					duration = _duration,
					distance = _distance,
					height = _height,
					angle = _angle,
					followSmoothTime = _followSmoothTime,
					lookAtSmoothTime = _lookAtSmoothTime,
					lookAtHeightOffset = _lookAtHeightOffset,
					lookAtTarget = _lookAtTarget,
					shakeAmplitude = _shakeAmplitude,
					shakeFrequency = _shakeFrequency
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

		public void SetStartFOV(float fov)
		{
			_startFOV = fov;
		}

		public void SetEndFOV(float fov)
		{
			_endFOV = fov;
		}

		public void SetDuration(float duration)
		{
			_duration = duration;
		}

		public void SetDistance(float distance)
		{
			_distance = distance;
		}

		public void SetHeight(float height)
		{
			_height = height;
		}

		public void SetAngle(float angle)
		{
			_angle = angle;
		}

		public void SetFollowSmoothTime(float time)
		{
			_followSmoothTime = time;
		}

		public void SetLookAtSmoothTime(float time)
		{
			_lookAtSmoothTime = time;
		}

		public void SetLookAtHeightOffset(float offset)
		{
			_lookAtHeightOffset = offset;
		}

		public void SetShakeAmplitude(float amplitude)
		{
			_shakeAmplitude = amplitude;
		}

		public void SetShakeFrequency(float frequency)
		{
			_shakeFrequency = frequency;
		}

		public void SetTarget(Transform target)
		{
			_targetTransform = target;
			_positionVelocity = Vector3.zero;
		}

		public void ClearTarget()
		{
			_targetTransform = null;
		}

		public void AssignTargetByRaycast(Transform cameraTransform)
		{
			if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, 500f))
			{
				_targetTransform = hitInfo.transform;
				_positionVelocity = Vector3.zero;
			}
		}

		private Vector3 ComputeDesiredPosition()
		{
			Vector3 position = _targetTransform.position;
			float f = _angle * ((float)Math.PI / 180f);
			Vector3 vector = new Vector3(Mathf.Sin(f) * _distance, _height, Mathf.Cos(f) * _distance);
			return position + vector;
		}

		private void ApplyLookAt(CinemachineCamera vcam, bool instant)
		{
			if (_targetTransform == null)
			{
				return;
			}
			Vector3 forward = _targetTransform.position + Vector3.up * _lookAtHeightOffset - vcam.transform.position;
			if (!(forward.sqrMagnitude <= 0.001f))
			{
				Quaternion quaternion = Quaternion.LookRotation(forward);
				if (instant)
				{
					vcam.transform.rotation = quaternion;
					return;
				}
				float t = 1f - Mathf.Exp(-10f / Mathf.Max(_lookAtSmoothTime, 0.01f) * Time.unscaledDeltaTime);
				vcam.transform.rotation = Quaternion.Slerp(vcam.transform.rotation, quaternion, t);
			}
		}

		private void ApplyShake(CinemachineCamera vcam, float deltaTime)
		{
			_shakeTime += deltaTime * _shakeFrequency;
			float x = (Mathf.PerlinNoise(_shakeTime * 1.1f, 0f) - 0.5f) * 2f * _shakeAmplitude * 0.02f;
			float y = (Mathf.PerlinNoise(0f, _shakeTime * 1.3f) - 0.5f) * 2f * _shakeAmplitude * 0.02f;
			float z = (Mathf.PerlinNoise(_shakeTime * 0.9f, _shakeTime * 0.7f) - 0.5f) * 2f * _shakeAmplitude * 0.01f;
			vcam.transform.position += new Vector3(x, y, z);
			float x2 = (Mathf.PerlinNoise(_shakeTime * 0.8f, 100f) - 0.5f) * _shakeAmplitude * 0.5f;
			float y2 = (Mathf.PerlinNoise(100f, _shakeTime * 0.6f) - 0.5f) * _shakeAmplitude * 0.5f;
			vcam.transform.rotation *= Quaternion.Euler(x2, y2, 0f);
		}

		private static void RemoveAllPipelineComponents(CinemachineCamera vcam)
		{
			if (!(vcam == null))
			{
				CinemachineComponentBase[] components = vcam.GetComponents<CinemachineComponentBase>();
				for (int i = 0; i < components.Length; i++)
				{
					UnityEngine.Object.Destroy(components[i]);
				}
			}
		}
	}
}
