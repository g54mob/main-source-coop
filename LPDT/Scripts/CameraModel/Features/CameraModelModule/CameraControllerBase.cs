using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.CameraModelModule
{
	public class CameraControllerBase : MonoBehaviour
	{
		private const string ORBIT_SCALE_AXIS_NAME = "Orbit Scale";

		private const string LOOK_ORBIT_Y_AXIS_NAME = "Look Orbit Y";

		[SerializeField]
		private CinemachineCamera _cinemachineCamera;

		[SerializeField]
		private CinemachineBasicMultiChannelPerlinMultiple _cinemachineBasicMultiChannelPerlinMultiple;

		[SerializeField]
		private CinemachineCameraOffset _cinemachineCameraOffset;

		[SerializeField]
		private CinemachineFreeLookModifier _cinemachineFreeLookModifier;

		[SerializeField]
		private CinemachineInputAxisController _cinemachineInputAxisController;

		[SerializeField]
		private CinemachineOrbitalFollow _cinemachineOrbitalFollow;

		[SerializeField]
		private CinemachinePanTilt _cinemachinePanTilt;

		[SerializeField]
		private CinemachineDeoccluder _cinemachineDeoccluder;

		[Header("Spectator Follow Profile")]
		[Tooltip("Position damping while this rig follows a spectated remote body. The authored value is tuned for a ragdoll that stays put and leaves a walking subject far behind the camera.")]
		[SerializeField]
		private Vector3 _spectatorFollowPositionDamping = new Vector3(0.5f, 0.5f, 0.5f);

		[Tooltip("Deoccluder collision mask while this rig follows a spectated remote body. Must exclude the subject's own layers (Player / ArmEnd) — when the target is somebody else, their torso and arms cross the deoccluder cast constantly and yank the camera in.")]
		[SerializeField]
		private LayerMask _spectatorFollowOcclusionMask;

		[SerializeField]
		private float _heightOffset = 1.6f;

		[SerializeField]
		private float _cameraCollisionRadius = 0.3f;

		[SerializeField]
		private LayerMask _obstacleMask;

		[SerializeField]
		private int _horizontalSteps = 72;

		[SerializeField]
		private float _distancePadding = 0.05f;

		[SerializeField]
		private float _minAcceptableDistance = 1.2f;

		private CinemachineFreeLookModifier.IModifiableDistance _cinemachineFreeLookModifierModifiableDistance;

		private CameraModel _cameraModel;

		private bool _isSpectatorFollowProfileActive;

		private Vector3 _authoredPositionDamping;

		private LayerMask _authoredOcclusionMask;

		private CinemachineFreeLookModifier.IModifiableDistance CinemachineFreeLookModifierModifiableDistance
		{
			get
			{
				if (_cinemachineFreeLookModifier == null)
				{
					return null;
				}
				return _cinemachineFreeLookModifierModifiableDistance ?? (_cinemachineFreeLookModifierModifiableDistance = _cinemachineFreeLookModifier.GetComponent<CinemachineFreeLookModifier.IModifiableDistance>());
			}
			set
			{
				_cinemachineFreeLookModifierModifiableDistance = value;
			}
		}

		[Inject]
		public void InjectDependencies(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		public virtual float GetFOV()
		{
			if (_cinemachineCamera == null)
			{
				return 0f;
			}
			return _cinemachineCamera.Lens.FieldOfView;
		}

		public virtual void SetFOV(float value)
		{
			if (!(_cinemachineCamera == null))
			{
				float fieldOfView = value;
				if (value < 0f)
				{
					fieldOfView = 0f;
				}
				_cinemachineCamera.Lens.FieldOfView = fieldOfView;
			}
		}

		public virtual void SetNearClipPlane(float value)
		{
			if (!(_cinemachineCamera == null))
			{
				float nearClipPlane = value;
				if (value < 0f)
				{
					nearClipPlane = 0f;
				}
				_cinemachineCamera.Lens.NearClipPlane = nearClipPlane;
			}
		}

		public virtual void SetFarClipPlane(float value)
		{
			if (!(_cinemachineCamera == null))
			{
				float farClipPlane = value;
				if (value < 0f)
				{
					farClipPlane = 0f;
				}
				_cinemachineCamera.Lens.FarClipPlane = farClipPlane;
			}
		}

		public virtual void SetСameraMode(LensSettings.OverrideModes overrideMode)
		{
			if (!(_cinemachineCamera == null))
			{
				_cinemachineCamera.Lens.ModeOverride = overrideMode;
			}
		}

		public virtual void SetCameraHeight(float value)
		{
			if (!(_cinemachineCameraOffset == null))
			{
				_cinemachineCameraOffset.Offset = new Vector3(_cinemachineCameraOffset.Offset.x, value, _cinemachineCameraOffset.Offset.z);
			}
		}

		public virtual float GetCameraHeight()
		{
			if (_cinemachineCameraOffset == null)
			{
				return 0f;
			}
			return _cinemachineCameraOffset.Offset.y;
		}

		public virtual void SetCameraDistance(float value)
		{
			if (CinemachineFreeLookModifierModifiableDistance != null)
			{
				CinemachineFreeLookModifierModifiableDistance.Distance = value;
			}
		}

		public virtual float GetCameraDistance()
		{
			if (CinemachineFreeLookModifierModifiableDistance == null)
			{
				return 0f;
			}
			return CinemachineFreeLookModifierModifiableDistance.Distance;
		}

		public virtual void SetAmplitudeGain(float amplitudeGain)
		{
			if (!(_cinemachineBasicMultiChannelPerlinMultiple == null))
			{
				_cinemachineBasicMultiChannelPerlinMultiple.AmplitudeGain = amplitudeGain;
			}
		}

		public virtual void SetFrequencyGain(float frequencyGain)
		{
			if (!(_cinemachineBasicMultiChannelPerlinMultiple == null))
			{
				_cinemachineBasicMultiChannelPerlinMultiple.FrequencyGain = frequencyGain;
			}
		}

		public virtual float GetAmplitudeGain()
		{
			if (_cinemachineBasicMultiChannelPerlinMultiple == null)
			{
				return 0f;
			}
			return _cinemachineBasicMultiChannelPerlinMultiple.AmplitudeGain;
		}

		public virtual float GetFrequencyGain()
		{
			if (_cinemachineBasicMultiChannelPerlinMultiple == null)
			{
				return 0f;
			}
			return _cinemachineBasicMultiChannelPerlinMultiple.FrequencyGain;
		}

		public IEnumerator TransitionToNoise(NoiseType targetNoise, float transitionTime)
		{
			float transitionTimer = 0f;
			CinemachineBasicMultiChannelPerlinMultiple.WeightedNoiseProfile targetNoiseProfile = _cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles.FirstOrDefault((CinemachineBasicMultiChannelPerlinMultiple.WeightedNoiseProfile x) => x.NoiseType == targetNoise);
			float targetNoiseInitialWeight = targetNoiseProfile.Weight;
			List<float> noisesInitialWeights = _cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles.Select((CinemachineBasicMultiChannelPerlinMultiple.WeightedNoiseProfile x) => x.Weight).ToList();
			for (; transitionTimer < transitionTime; transitionTimer += Time.deltaTime)
			{
				for (int num = 0; num < _cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles.Length; num++)
				{
					if (_cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles[num].NoiseType != targetNoise)
					{
						_cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles[num].Weight = Mathf.Lerp(noisesInitialWeights[num], 0f, transitionTimer / transitionTime);
					}
				}
				targetNoiseProfile.Weight = Mathf.Lerp(targetNoiseInitialWeight, 1f, transitionTimer / transitionTime);
				yield return null;
			}
		}

		public void SetNoiseType(NoiseType noiseType)
		{
			CinemachineBasicMultiChannelPerlinMultiple.WeightedNoiseProfile[] noiseProfiles = _cinemachineBasicMultiChannelPerlinMultiple.NoiseProfiles;
			foreach (CinemachineBasicMultiChannelPerlinMultiple.WeightedNoiseProfile obj in noiseProfiles)
			{
				obj.Weight = ((obj.NoiseType == noiseType) ? 1f : 0f);
			}
		}

		public void SetCinemachineEnabled(bool isEnabled)
		{
			if (!(_cinemachineCamera == null))
			{
				_cinemachineCamera.enabled = isEnabled;
			}
		}

		public virtual void SetTrackingTarget(Transform currentTarget, bool forceUpdate = false)
		{
			if (!(_cinemachineCamera == null))
			{
				_cinemachineCamera.Target.TrackingTarget = currentTarget;
				if (forceUpdate)
				{
					_cinemachineCamera.PreviousStateIsValid = false;
				}
			}
		}

		public void SetSpectatorFollowProfileActive(bool isActive)
		{
			if (isActive == _isSpectatorFollowProfileActive)
			{
				return;
			}
			_isSpectatorFollowProfileActive = isActive;
			if (isActive)
			{
				if (_cinemachineOrbitalFollow != null)
				{
					_authoredPositionDamping = _cinemachineOrbitalFollow.TrackerSettings.PositionDamping;
					_cinemachineOrbitalFollow.TrackerSettings.PositionDamping = _spectatorFollowPositionDamping;
				}
				if (_cinemachineDeoccluder != null)
				{
					_authoredOcclusionMask = _cinemachineDeoccluder.CollideAgainst;
					_cinemachineDeoccluder.CollideAgainst = _spectatorFollowOcclusionMask;
				}
			}
			else
			{
				if (_cinemachineOrbitalFollow != null)
				{
					_cinemachineOrbitalFollow.TrackerSettings.PositionDamping = _authoredPositionDamping;
				}
				if (_cinemachineDeoccluder != null)
				{
					_cinemachineDeoccluder.CollideAgainst = _authoredOcclusionMask;
				}
			}
		}

		public virtual Transform GetTrackingTarget()
		{
			if (!(_cinemachineCamera == null))
			{
				return _cinemachineCamera.Target.TrackingTarget;
			}
			return null;
		}

		public virtual void SetLookAtTarget(Transform lookAtTarget)
		{
			if (!(_cinemachineCamera == null))
			{
				_cinemachineCamera.Target.LookAtTarget = lookAtTarget;
			}
		}

		public void SetXSensitivity(float sensitivity)
		{
			if (!(_cinemachineInputAxisController == null))
			{
				_cinemachineInputAxisController.Controllers[0].Input.Gain = sensitivity;
			}
		}

		public void SetYSensitivity(float sensitivity)
		{
			if (!(_cinemachineInputAxisController == null))
			{
				_cinemachineInputAxisController.Controllers[1].Input.Gain = sensitivity;
			}
		}

		public void SetInputEnabled(bool isActive)
		{
			if (!(_cinemachineInputAxisController == null))
			{
				_cinemachineInputAxisController.enabled = isActive;
			}
		}

		public void SetZoomInputEnabled(bool isEnabled)
		{
			if (_cinemachineInputAxisController == null)
			{
				return;
			}
			foreach (InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller controller in _cinemachineInputAxisController.Controllers)
			{
				if (!(controller.Name != "Orbit Scale"))
				{
					controller.Enabled = isEnabled;
					controller.InputValue = 0f;
				}
			}
		}

		public void SetVerticalLookEnabled(bool isEnabled)
		{
			if (_cinemachineInputAxisController == null)
			{
				return;
			}
			foreach (InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller controller in _cinemachineInputAxisController.Controllers)
			{
				if (!(controller.Name != "Look Orbit Y"))
				{
					controller.Enabled = isEnabled;
					controller.InputValue = 0f;
				}
			}
		}

		public void InvalidateInputValues()
		{
			if (_cinemachineInputAxisController == null)
			{
				return;
			}
			foreach (InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller controller in _cinemachineInputAxisController.Controllers)
			{
				controller.InputValue = 0f;
			}
		}

		public void SetHorizontalRotation(Vector3 forward)
		{
			Vector3 vector = Vector3.ProjectOnPlane(forward, Vector3.up);
			if (!(vector.sqrMagnitude <= Mathf.Epsilon))
			{
				SetHorizontalRotation(Vector3.SignedAngle(Vector3.forward, vector.normalized, Vector3.up));
			}
		}

		public void SetHorizontalRotation(float rotation)
		{
			if (_cinemachineOrbitalFollow != null)
			{
				_cinemachineOrbitalFollow.HorizontalAxis.Value = GetClosestEquivalentAngle(_cinemachineOrbitalFollow.HorizontalAxis.Value, rotation);
			}
			if (_cinemachinePanTilt != null)
			{
				_cinemachinePanTilt.PanAxis.Value = GetClosestEquivalentAngle(_cinemachinePanTilt.PanAxis.Value, rotation);
			}
		}

		public void AddHorizontalRotation(float deltaDegrees)
		{
			if (_cinemachineOrbitalFollow != null)
			{
				_cinemachineOrbitalFollow.HorizontalAxis.Value += deltaDegrees;
			}
			if (_cinemachinePanTilt != null)
			{
				_cinemachinePanTilt.PanAxis.Value += deltaDegrees;
			}
		}

		public float GetHorizontalRotation()
		{
			if (_cinemachineOrbitalFollow != null)
			{
				return _cinemachineOrbitalFollow.HorizontalAxis.Value;
			}
			if (_cinemachinePanTilt != null)
			{
				return _cinemachinePanTilt.PanAxis.Value;
			}
			return 0f;
		}

		public float GetVerticalRotation()
		{
			if (_cinemachineOrbitalFollow != null)
			{
				return _cinemachineOrbitalFollow.VerticalAxis.Value;
			}
			if (_cinemachinePanTilt != null)
			{
				return _cinemachinePanTilt.TiltAxis.Value;
			}
			return 0f;
		}

		public bool TryGetVerticalRange(out float min, out float max)
		{
			if (_cinemachineOrbitalFollow != null)
			{
				min = _cinemachineOrbitalFollow.VerticalAxis.Range.x;
				max = _cinemachineOrbitalFollow.VerticalAxis.Range.y;
				return true;
			}
			if (_cinemachinePanTilt != null)
			{
				min = _cinemachinePanTilt.TiltAxis.Range.x;
				max = _cinemachinePanTilt.TiltAxis.Range.y;
				return true;
			}
			min = 0f;
			max = 0f;
			return false;
		}

		public void SetVerticalRotation(Vector3 forward)
		{
			Vector3 vector = Vector3.ProjectOnPlane(forward, Vector3.up);
			if (!(vector.sqrMagnitude <= Mathf.Epsilon))
			{
				Vector3 normalized = vector.normalized;
				Vector3 normalized2 = forward.normalized;
				Vector3 axis = Vector3.Cross(Vector3.up, normalized);
				float verticalRotation = Vector3.SignedAngle(normalized, normalized2, axis);
				SetVerticalRotation(verticalRotation);
			}
		}

		public void SetVerticalRotation(float rotation)
		{
			if (_cinemachineOrbitalFollow != null)
			{
				_cinemachineOrbitalFollow.VerticalAxis.Value = Mathf.Clamp(rotation, _cinemachineOrbitalFollow.VerticalAxis.Range.x, _cinemachineOrbitalFollow.VerticalAxis.Range.y);
			}
			if (_cinemachinePanTilt != null)
			{
				_cinemachinePanTilt.TiltAxis.Value = Mathf.Clamp(rotation, _cinemachinePanTilt.TiltAxis.Range.x, _cinemachinePanTilt.TiltAxis.Range.y);
			}
		}

		public void SetBestRotation(float fallbackRotation)
		{
			float bestHorizontal;
			float bestVertical;
			float bestDistance;
			if (_cinemachineOrbitalFollow == null || _cinemachineCamera == null || _cinemachineCamera.Target.TrackingTarget == null || _cinemachineCamera.Target.TrackingTarget == null)
			{
				SetHorizontalRotation(fallbackRotation);
			}
			else if (TryFindBestDirection(out bestHorizontal, out bestVertical, out bestDistance))
			{
				SetHorizontalRotation(bestHorizontal);
				SetVerticalRotation(bestVertical);
				_cinemachineOrbitalFollow.Radius = bestDistance;
			}
			else
			{
				SetHorizontalRotation(fallbackRotation);
			}
		}

		private bool TryFindBestDirection(out float bestHorizontal, out float bestVertical, out float bestDistance)
		{
			bestHorizontal = _cinemachineOrbitalFollow.HorizontalAxis.Value;
			bestVertical = _cinemachineOrbitalFollow.VerticalAxis.Value;
			bestDistance = _minAcceptableDistance;
			Vector3 origin = _cinemachineCamera.Target.TrackingTarget.position + Vector3.up * _heightOffset;
			float radius = _cinemachineOrbitalFollow.Radius;
			float value = _cinemachineOrbitalFollow.HorizontalAxis.Value;
			float value2 = _cinemachineOrbitalFollow.VerticalAxis.Value;
			float x = _cinemachineOrbitalFollow.VerticalAxis.Range.x;
			float y = _cinemachineOrbitalFollow.VerticalAxis.Range.y;
			float[] obj = new float[13]
			{
				0f, 5f, -5f, 10f, -10f, 15f, -15f, 20f, -20f, 25f,
				-25f, 30f, -30f
			};
			float num = 360f / (float)_horizontalSteps;
			float bestScore = float.MinValue;
			bool found = false;
			float[] array = obj;
			foreach (float num2 in array)
			{
				float vertical = Mathf.Clamp(value2 + num2, x, y);
				EvaluateCandidate(origin, value, vertical, radius, ref bestScore, ref bestHorizontal, ref bestVertical, ref bestDistance, ref found);
				for (int j = 1; j <= _horizontalSteps / 2; j++)
				{
					float horizontal = NormalizeAngle(value + num * (float)j);
					EvaluateCandidate(origin, horizontal, vertical, radius, ref bestScore, ref bestHorizontal, ref bestVertical, ref bestDistance, ref found);
					float horizontal2 = NormalizeAngle(value - num * (float)j);
					EvaluateCandidate(origin, horizontal2, vertical, radius, ref bestScore, ref bestHorizontal, ref bestVertical, ref bestDistance, ref found);
				}
			}
			return found;
		}

		private void EvaluateCandidate(Vector3 origin, float horizontal, float vertical, float targetDistance, ref float bestScore, ref float bestHorizontal, ref float bestVertical, ref float bestDistance, ref bool found)
		{
			float availableDistance = GetAvailableDistance(origin, horizontal, vertical, targetDistance);
			if (!(availableDistance <= 0f) && !(availableDistance < _minAcceptableDistance))
			{
				float num = availableDistance;
				if (availableDistance >= targetDistance - 0.01f)
				{
					num += 1000f;
				}
				if (num > bestScore)
				{
					bestScore = num;
					bestHorizontal = horizontal;
					bestVertical = vertical;
					bestDistance = Mathf.Min(availableDistance, targetDistance);
					found = true;
				}
			}
		}

		private float GetAvailableDistance(Vector3 origin, float horizontalAngle, float verticalAngle, float targetDistance)
		{
			Vector3 direction = Quaternion.Euler(verticalAngle, horizontalAngle, 0f) * Vector3.back;
			if (Physics.SphereCast(origin, _cameraCollisionRadius, direction, out var hitInfo, targetDistance, _obstacleMask, QueryTriggerInteraction.Ignore))
			{
				float b = hitInfo.distance - _distancePadding;
				return Mathf.Max(0f, b);
			}
			return targetDistance;
		}

		private float NormalizeAngle(float angle)
		{
			angle %= 360f;
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (angle < -180f)
			{
				angle += 360f;
			}
			return angle;
		}

		private float GetClosestEquivalentAngle(float currentAngle, float targetAngle)
		{
			return currentAngle + Mathf.DeltaAngle(currentAngle, targetAngle);
		}

		internal async UniTask SetPerlinActive(bool isActive)
		{
			if (!(_cinemachineBasicMultiChannelPerlinMultiple == null))
			{
				if (isActive)
				{
					await WaitBlendFinished();
				}
				if (!(_cinemachineBasicMultiChannelPerlinMultiple == null))
				{
					_cinemachineBasicMultiChannelPerlinMultiple.enabled = isActive;
				}
			}
		}

		private async UniTask WaitBlendFinished()
		{
			await UniTask.WaitForEndOfFrame();
			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
			if (!(_cameraModel.CameraBrain == null))
			{
				await UniTask.WaitUntil(() => _cameraModel.CameraBrain == null || !_cameraModel.CameraBrain.IsBlending, PlayerLoopTiming.LastPostLateUpdate);
			}
		}

		internal void SetPriority(int priority)
		{
			if (!(_cinemachineCamera == null))
			{
				_cinemachineCamera.Priority = priority;
			}
		}
	}
}
