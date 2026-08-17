using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class StaticBehavior : IBehaviorConfigurator
	{
		private bool _useLookAt;

		private Vector3 _lookAtPosition;

		private bool _smoothLookAt;

		private float _smoothSpeed;

		private Quaternion _startRotation;

		private float _elapsed;

		public DirectorCameraBehavior BehaviorType => DirectorCameraBehavior.Static;

		public void Configure(CinemachineCamera vcam, DirectorCameraData data)
		{
			RemoveAllPipelineComponents(vcam);
			StaticSettings staticSettings = data.staticSettings;
			_useLookAt = staticSettings.useLookAtTarget;
			_lookAtPosition = staticSettings.lookAtPosition;
			_smoothLookAt = staticSettings.smoothLookAt;
			_smoothSpeed = staticSettings.smoothSpeed;
			_startRotation = vcam.transform.rotation;
			_elapsed = 0f;
			if (_useLookAt && !_smoothLookAt)
			{
				Vector3 forward = _lookAtPosition - vcam.transform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					vcam.transform.rotation = Quaternion.LookRotation(forward);
				}
			}
		}

		public void UpdateRuntime(CinemachineCamera vcam, float deltaTime)
		{
			if (_useLookAt && _smoothLookAt)
			{
				_elapsed += deltaTime;
				Vector3 forward = _lookAtPosition - vcam.transform.position;
				if (!(forward.sqrMagnitude <= 0.001f))
				{
					Quaternion b = Quaternion.LookRotation(forward);
					vcam.transform.rotation = Quaternion.Slerp(_startRotation, b, Mathf.Clamp01(_elapsed * _smoothSpeed));
				}
			}
		}

		public DirectorCameraData ExtractSettings(CinemachineCamera vcam)
		{
			DirectorCameraData directorCameraData = new DirectorCameraData
			{
				behaviorType = DirectorCameraBehavior.Static,
				staticSettings = new StaticSettings
				{
					useLookAtTarget = _useLookAt,
					lookAtPosition = _lookAtPosition,
					smoothLookAt = _smoothLookAt,
					smoothSpeed = _smoothSpeed
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
