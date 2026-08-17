using System;
using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class OrbitBehavior : IBehaviorConfigurator
	{
		private Vector3 _targetPosition;

		private float _radius;

		private float _height;

		private float _startAngle;

		private float _endAngle;

		private float _speed;

		private bool _lookAtTarget;

		private Vector3 _targetOffset;

		private bool _autoRotate;

		private float _currentAngle;

		private float _targetRadius;

		private float _targetHeight;

		private float _targetSpeed;

		private float _parameterSmoothTime;

		private float _radiusVelocity;

		private float _heightVelocity;

		private float _speedVelocity;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Orbit;

		public float ParameterSmoothTime => _parameterSmoothTime;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			OrbitSettings orbitSettings = data.orbitSettings;
			_radius = orbitSettings.radius;
			_targetRadius = orbitSettings.radius;
			_height = orbitSettings.height;
			_targetHeight = orbitSettings.height;
			_startAngle = orbitSettings.startAngle;
			_endAngle = orbitSettings.endAngle;
			_speed = orbitSettings.speed;
			_targetSpeed = orbitSettings.speed;
			_lookAtTarget = orbitSettings.lookAtTarget;
			_targetOffset = orbitSettings.targetOffset;
			_autoRotate = orbitSettings.autoRotate;
			_currentAngle = _startAngle;
			_parameterSmoothTime = orbitSettings.parameterSmoothTime;
			_smoothing = data.smoothing;
			_smoother.Reset();
			_radiusVelocity = 0f;
			_heightVelocity = 0f;
			_speedVelocity = 0f;
			if (orbitSettings.targetPosition == Vector3.zero && vcam != null)
			{
				_targetPosition = GetLookAtPoint(vcam.transform, _radius);
			}
			else
			{
				_targetPosition = orbitSettings.targetPosition;
			}
			ApplyOrbitPosition(vcam, _currentAngle);
		}

		private static Vector3 GetLookAtPoint(Transform camTransform, float fallbackDistance)
		{
			if (Physics.Raycast(camTransform.position, camTransform.forward, out var hitInfo, 500f))
			{
				return hitInfo.point;
			}
			return camTransform.position + camTransform.forward * fallbackDistance;
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			_radius = Mathf.SmoothDamp(_radius, _targetRadius, ref _radiusVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
			_height = Mathf.SmoothDamp(_height, _targetHeight, ref _heightVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
			_speed = Mathf.SmoothDamp(_speed, _targetSpeed, ref _speedVelocity, _parameterSmoothTime, 1f / 0f, deltaTime);
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
			Vector3 vector = ComputeOrbitPosition(_currentAngle);
			Quaternion target = ComputeOrbitRotation(vector);
			vcam.transform.position = _smoother.Smooth(vcam.transform.position, vector, _smoothing, deltaTime);
			vcam.transform.rotation = _smoother.Smooth(vcam.transform.rotation, target, _smoothing, deltaTime);
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Orbit,
				orbitSettings = new OrbitSettings
				{
					targetPosition = _targetPosition,
					radius = _targetRadius,
					height = _targetHeight,
					startAngle = _startAngle,
					endAngle = _endAngle,
					speed = _targetSpeed,
					lookAtTarget = _lookAtTarget,
					targetOffset = _targetOffset,
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

		private Vector3 ComputeOrbitPosition(float angleDegrees)
		{
			float f = angleDegrees * ((float)Math.PI / 180f);
			Vector3 vector = _targetPosition + _targetOffset;
			Vector3 vector2 = new Vector3(Mathf.Sin(f) * _radius, _height, Mathf.Cos(f) * _radius);
			return vector + vector2;
		}

		private Quaternion ComputeOrbitRotation(Vector3 position)
		{
			if (!_lookAtTarget)
			{
				return Quaternion.identity;
			}
			Vector3 forward = _targetPosition + _targetOffset - position;
			if (forward.sqrMagnitude > 0.001f)
			{
				return Quaternion.LookRotation(forward);
			}
			return Quaternion.identity;
		}

		private void ApplyOrbitPosition(CinemachineCamera vcam, float angleDegrees)
		{
			vcam.transform.position = ComputeOrbitPosition(angleDegrees);
			Vector3 vector = _targetPosition + _targetOffset;
			if (_lookAtTarget)
			{
				Vector3 forward = vector - vcam.transform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					vcam.transform.rotation = Quaternion.LookRotation(forward);
				}
			}
		}

		public void SetTargetPosition(Vector3 position)
		{
			_targetPosition = position;
		}

		public void SetRadius(float radius)
		{
			_targetRadius = radius;
		}

		public void SetHeight(float height)
		{
			_targetHeight = height;
		}

		public void SetSpeed(float speed)
		{
			_targetSpeed = speed;
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
					UnityEngine.Object.Destroy(components[i]);
				}
			}
		}
	}
}
