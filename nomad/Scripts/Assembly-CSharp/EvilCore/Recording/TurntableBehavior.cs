using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class TurntableBehavior : IBehaviorConfigurator
	{
		private Vector3 _pivotPosition;

		private float _speed;

		private float _pitch;

		private Vector3 _offset;

		private float _startAngle;

		private float _endAngle;

		private bool _autoRotate;

		private float _currentAngle;

		private float _targetSpeed;

		private float _targetPitch;

		private Vector3 _targetOffset;

		private float _parameterSmoothTime;

		private float _speedVelocity;

		private float _pitchVelocity;

		private Vector3 _offsetVelocity;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Turntable;

		public float ParameterSmoothTime => _parameterSmoothTime;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			TurntableSettings turntableSettings = data.turntableSettings;
			_speed = turntableSettings.speed;
			_targetSpeed = turntableSettings.speed;
			_pitch = turntableSettings.pitch;
			_targetPitch = turntableSettings.pitch;
			_offset = turntableSettings.offset;
			_targetOffset = turntableSettings.offset;
			_startAngle = turntableSettings.startAngle;
			_endAngle = turntableSettings.endAngle;
			_autoRotate = turntableSettings.autoRotate;
			_currentAngle = _startAngle;
			_parameterSmoothTime = turntableSettings.parameterSmoothTime;
			_smoothing = data.smoothing;
			_smoother.Reset();
			_speedVelocity = 0f;
			_pitchVelocity = 0f;
			_offsetVelocity = Vector3.zero;
			if (vcam != null)
			{
				_pivotPosition = vcam.transform.position;
			}
			ApplyTurntableTransform(vcam, _currentAngle);
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			_speed = Mathf.SmoothDamp(_speed, _targetSpeed, ref _speedVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
			_pitch = Mathf.SmoothDamp(_pitch, _targetPitch, ref _pitchVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
			_offset = Vector3.SmoothDamp(_offset, _targetOffset, ref _offsetVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
			if (!_autoRotate)
			{
				return;
			}
			_currentAngle += _speed * deltaTime;
			if (_endAngle > _startAngle)
			{
				if (_currentAngle > _endAngle)
				{
					_currentAngle = _startAngle + (_currentAngle - _endAngle);
				}
			}
			else if (_currentAngle < _endAngle)
			{
				_currentAngle = _startAngle + (_currentAngle - _endAngle);
			}
			Vector3 target = ComputeTurntablePosition(_currentAngle);
			Quaternion target2 = ComputeTurntableRotation(_currentAngle);
			vcam.transform.position = _smoother.Smooth(vcam.transform.position, target, _smoothing, deltaTime);
			vcam.transform.rotation = _smoother.Smooth(vcam.transform.rotation, target2, _smoothing, deltaTime);
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Turntable,
				turntableSettings = new TurntableSettings
				{
					speed = _targetSpeed,
					pitch = _targetPitch,
					offset = _targetOffset,
					startAngle = _startAngle,
					endAngle = _endAngle,
					autoRotate = _autoRotate,
					parameterSmoothTime = _parameterSmoothTime
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

		public void OnPositionUpdated(CinemachineCamera vcam)
		{
			if (vcam != null)
			{
				_pivotPosition = vcam.transform.position;
			}
		}

		private Vector3 ComputeTurntablePosition(float angleDegrees)
		{
			if (_offset.sqrMagnitude < 0.0001f)
			{
				return _pivotPosition;
			}
			Quaternion quaternion = Quaternion.Euler(0f, angleDegrees, 0f);
			return _pivotPosition + quaternion * _offset;
		}

		private Quaternion ComputeTurntableRotation(float angleDegrees)
		{
			return Quaternion.Euler(_pitch, angleDegrees, 0f);
		}

		private void ApplyTurntableTransform(CinemachineCamera vcam, float angleDegrees)
		{
			if (!(vcam == null))
			{
				vcam.transform.position = ComputeTurntablePosition(angleDegrees);
				vcam.transform.rotation = ComputeTurntableRotation(angleDegrees);
			}
		}

		public void SetSpeed(float speed)
		{
			_targetSpeed = speed;
		}

		public void SetPitch(float pitch)
		{
			_targetPitch = pitch;
		}

		public void SetOffset(Vector3 offset)
		{
			_targetOffset = offset;
		}

		public void SetParameterSmoothTime(float time)
		{
			_parameterSmoothTime = time;
		}

		public void SetAngleRange(float start, float end)
		{
			_startAngle = start;
			_endAngle = end;
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
