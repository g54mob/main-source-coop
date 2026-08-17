using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class DollyBehavior : IBehaviorConfigurator
	{
		private DollyDirection _direction;

		private float _speed;

		private bool _lookAtTarget;

		private Vector3 _lookAtPosition;

		private Vector3 _startPosition;

		private Vector3 _moveDirection;

		private float _smoothing;

		private PositionSmoother _smoother;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Dolly;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			DollySettings dollySettings = data.dollySettings;
			_direction = dollySettings.direction;
			_speed = dollySettings.speed;
			_lookAtTarget = dollySettings.lookAtTarget;
			_lookAtPosition = dollySettings.lookAtPosition;
			_smoothing = data.smoothing;
			_smoother.Reset();
			_startPosition = vcam.transform.position;
			_moveDirection = ComputeDirection(vcam.transform, _direction);
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			Vector3 target = vcam.transform.position + _moveDirection * (_speed * deltaTime);
			vcam.transform.position = _smoother.Smooth(vcam.transform.position, target, _smoothing, deltaTime);
			if (_lookAtTarget)
			{
				Vector3 forward = _lookAtPosition - vcam.transform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					Quaternion target2 = Quaternion.LookRotation(forward);
					vcam.transform.rotation = _smoother.Smooth(vcam.transform.rotation, target2, _smoothing, deltaTime);
				}
			}
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Dolly,
				dollySettings = new DollySettings
				{
					direction = _direction,
					speed = _speed,
					lookAtTarget = _lookAtTarget,
					lookAtPosition = _lookAtPosition
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

		public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		public void OnPositionUpdated(CinemachineCamera vcam)
		{
			_startPosition = vcam.transform.position;
			_moveDirection = ComputeDirection(vcam.transform, _direction);
		}

		public void SetDirection(DollyDirection direction, Transform camTransform)
		{
			_direction = direction;
			_moveDirection = ComputeDirection(camTransform, direction);
		}

		public void SetLookAtTarget(Vector3 position)
		{
			_lookAtTarget = true;
			_lookAtPosition = position;
		}

		public void ClearLookAtTarget()
		{
			_lookAtTarget = false;
		}

		private static Vector3 ComputeDirection(Transform camTransform, DollyDirection direction)
		{
			return direction switch
			{
				DollyDirection.Forward => camTransform.forward, 
				DollyDirection.Backward => -camTransform.forward, 
				DollyDirection.Left => -camTransform.right, 
				DollyDirection.Right => camTransform.right, 
				DollyDirection.Up => Vector3.up, 
				DollyDirection.Down => Vector3.down, 
				_ => camTransform.forward, 
			};
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
