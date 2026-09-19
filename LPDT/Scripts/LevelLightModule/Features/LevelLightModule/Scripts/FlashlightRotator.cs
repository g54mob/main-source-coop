using System;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class FlashlightRotator : NetworkBehaviour
	{
		private const int RaycastHitsBufferSize = 16;

		[SerializeField]
		private float _rotationSpeed = 5f;

		[SerializeField]
		private LayerMask _targetDetectionMask;

		[SerializeField]
		private LayerMask _lightAdjustmentDetectionMask;

		[SerializeField]
		private Transform _flashlight;

		[SerializeField]
		private Light _flashlightLight;

		[SerializeField]
		private Vector2[] _lightAdjustmentRaycastScreenOffsets = new Vector2[5]
		{
			Vector2.zero,
			new Vector2(-80f, 0f),
			new Vector2(80f, 0f),
			new Vector2(0f, -80f),
			new Vector2(0f, 80f)
		};

		[SerializeField]
		private FlashlightAdjustConfiguration _adjustConfiguration;

		[SerializeField]
		[Min(0.01f)]
		private float _maxRayDistance = 50f;

		[SerializeField]
		[Min(0f)]
		private float _aimPositionChangeThreshold = 0.04f;

		[SerializeField]
		[Min(0f)]
		private float _aimAngleChangeThreshold = 0.35f;

		[SerializeField]
		[Min(0f)]
		private float _maxSampleInterval = 0.2f;

		private readonly RaycastHit[] _raycastHits = new RaycastHit[16];

		private CameraModel _cameraModel;

		private PlayerMovableModel _playerMovableModel;

		private Vector3 _localTargetForward = Vector3.zero;

		private float _localTargetSpotAngle;

		private float _localTargetIntensity;

		private float _currentBaseIntensity;

		private bool _hasLocalLightTarget;

		private Vector3 _lastSampledAimPosition;

		private Vector3 _lastSampledAimForward;

		private float _lastSampleTime;

		private bool _hasSampledAim;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedForward", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedForward;

		public float IntensityMultiplier { get; private set; } = 1f;

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3 NetworkedForward
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlashlightRotator.NetworkedForward. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlashlightRotator.NetworkedForward. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		private void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
		}

		public void SetIntensityMultiplier(float intensityMultiplier)
		{
			IntensityMultiplier = Mathf.Max(0f, intensityMultiplier);
			ApplyLightIntensity();
		}

		private void FixedUpdate()
		{
			if (!base.HasInputAuthority || _cameraModel.CameraObject == null || _playerMovableModel.RotationMode != PlayerRotationMode.HeadAndBodyRotation)
			{
				return;
			}
			Transform aimTransform = _cameraModel.AimTransform;
			Vector3 position = aimTransform.position;
			Vector3 forward = aimTransform.forward;
			if (ShouldSampleAim(position, forward))
			{
				_lastSampledAimPosition = position;
				_lastSampledAimForward = forward;
				_lastSampleTime = Time.time;
				_hasSampledAim = true;
				Ray ray = AimRay(Vector2.zero);
				_localTargetForward = forward;
				int hitsCount = Physics.RaycastNonAlloc(ray, _raycastHits, _maxRayDistance, _targetDetectionMask);
				if (TryGetRotationHit(hitsCount, out var rotationHit))
				{
					_localTargetForward = rotationHit.point - base.transform.position;
				}
				UpdateLightTarget();
				_localTargetForward.Normalize();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _localTargetForward != Vector3.zero)
			{
				NetworkedForward = _localTargetForward;
			}
		}

		public override void Render()
		{
			Vector3 vector = (base.HasInputAuthority ? _localTargetForward : NetworkedForward);
			if (!(vector == Vector3.zero))
			{
				base.transform.forward = Vector3.Lerp(base.transform.forward, vector, Time.deltaTime * _rotationSpeed);
				ApplyLightTarget();
			}
		}

		private bool ShouldSampleAim(Vector3 aimPosition, Vector3 aimForward)
		{
			if (!_hasSampledAim)
			{
				return true;
			}
			if (Time.time - _lastSampleTime >= _maxSampleInterval)
			{
				return true;
			}
			if ((aimPosition - _lastSampledAimPosition).sqrMagnitude >= _aimPositionChangeThreshold * _aimPositionChangeThreshold)
			{
				return true;
			}
			float num = Mathf.Cos(_aimAngleChangeThreshold * (MathF.PI / 180f));
			return Vector3.Dot(aimForward, _lastSampledAimForward) < num;
		}

		private bool TryGetRotationHit(int hitsCount, out RaycastHit rotationHit)
		{
			float num = float.PositiveInfinity;
			bool result = false;
			rotationHit = default(RaycastHit);
			int playerId = base.Object.StateAuthority.PlayerId;
			for (int i = 0; i < hitsCount; i++)
			{
				RaycastHit raycastHit = _raycastHits[i];
				SimplePointGrabable componentInParent = raycastHit.collider.GetComponentInParent<SimplePointGrabable>();
				if ((!(componentInParent != null) || !componentInParent.GrabbedByPlayers.Contains(playerId)) && !(raycastHit.distance >= num))
				{
					rotationHit = raycastHit;
					num = raycastHit.distance;
					result = true;
				}
			}
			return result;
		}

		private void UpdateLightTarget()
		{
			bool hasLocalLightTarget = _hasLocalLightTarget;
			float distanceForMaxValues = _adjustConfiguration.DistanceForMaxValues;
			float t = 1f;
			float t2 = 1f;
			if (TryGetLightAdjustmentHits(distanceForMaxValues, out var averageDistance, out var centerHit, out var hasCenterHit))
			{
				t = Mathf.InverseLerp(_adjustConfiguration.DistanceForMinValues, _adjustConfiguration.DistanceForMaxValues, averageDistance);
			}
			if (hasCenterHit)
			{
				t2 = Mathf.InverseLerp(_adjustConfiguration.DistanceForMinValues, _adjustConfiguration.DistanceForMaxValues, centerHit.distance);
			}
			_localTargetSpotAngle = Mathf.Lerp(_adjustConfiguration.MaxOuterCutOff, _adjustConfiguration.MinOuterCutOff, t);
			_localTargetIntensity = Mathf.Lerp(_adjustConfiguration.MinIntensity, _adjustConfiguration.MaxIntensity, t2);
			if (!hasLocalLightTarget)
			{
				_currentBaseIntensity = _localTargetIntensity;
			}
			_hasLocalLightTarget = true;
		}

		private bool TryGetLightAdjustmentHits(float maxDistance, out float averageDistance, out RaycastHit centerHit, out bool hasCenterHit)
		{
			averageDistance = 0f;
			centerHit = default(RaycastHit);
			hasCenterHit = false;
			float num = 0f;
			int num2 = 0;
			bool flag = false;
			Vector2[] lightAdjustmentRaycastScreenOffsets = _lightAdjustmentRaycastScreenOffsets;
			int num3 = ((lightAdjustmentRaycastScreenOffsets == null || lightAdjustmentRaycastScreenOffsets.Length <= 0) ? 1 : _lightAdjustmentRaycastScreenOffsets.Length);
			for (int i = 0; i < num3; i++)
			{
				lightAdjustmentRaycastScreenOffsets = _lightAdjustmentRaycastScreenOffsets;
				Vector2 screenOffset = ((lightAdjustmentRaycastScreenOffsets != null && lightAdjustmentRaycastScreenOffsets.Length > 0) ? _lightAdjustmentRaycastScreenOffsets[i] : Vector2.zero);
				bool flag2 = screenOffset.sqrMagnitude <= 0f;
				if (flag2)
				{
					flag = true;
				}
				int hitsCount = Physics.RaycastNonAlloc(AimRay(screenOffset), _raycastHits, maxDistance, _lightAdjustmentDetectionMask);
				if (TryGetNearestHit(hitsCount, out var nearestHit))
				{
					num += nearestHit.distance;
					num2++;
					if (flag2)
					{
						centerHit = nearestHit;
						hasCenterHit = true;
					}
				}
			}
			if (!hasCenterHit && !flag)
			{
				hasCenterHit = TryGetCenterLightAdjustmentHit(maxDistance, out centerHit);
			}
			if (num2 == 0)
			{
				return false;
			}
			averageDistance = num / (float)num2;
			return true;
		}

		private bool TryGetCenterLightAdjustmentHit(float maxDistance, out RaycastHit lightHit)
		{
			int hitsCount = Physics.RaycastNonAlloc(AimRay(Vector2.zero), _raycastHits, maxDistance, _lightAdjustmentDetectionMask);
			return TryGetNearestHit(hitsCount, out lightHit);
		}

		private Ray AimRay(Vector2 screenOffset)
		{
			Ray result = _cameraModel.CameraObject.ScreenPointToRay(GetScreenCenterWithOffset(screenOffset));
			Transform aimTransform = _cameraModel.AimTransform;
			if (aimTransform == _cameraModel.CameraObject.transform)
			{
				return result;
			}
			Quaternion quaternion = aimTransform.rotation * Quaternion.Inverse(_cameraModel.CameraObject.transform.rotation);
			return new Ray(aimTransform.position, quaternion * result.direction);
		}

		private bool TryGetNearestHit(int hitsCount, out RaycastHit nearestHit)
		{
			float num = float.PositiveInfinity;
			nearestHit = default(RaycastHit);
			bool result = false;
			for (int i = 0; i < hitsCount; i++)
			{
				RaycastHit raycastHit = _raycastHits[i];
				if (!(raycastHit.distance >= num))
				{
					nearestHit = raycastHit;
					num = raycastHit.distance;
					result = true;
				}
			}
			return result;
		}

		private void ApplyLightTarget()
		{
			if (base.HasInputAuthority && _hasLocalLightTarget)
			{
				float t = Time.deltaTime * _adjustConfiguration.AdjustmentLerpSpeed;
				_currentBaseIntensity = Mathf.Lerp(_currentBaseIntensity, _localTargetIntensity, t);
				ApplyLightIntensity();
				_flashlightLight.spotAngle = Mathf.Lerp(_flashlightLight.spotAngle, _localTargetSpotAngle, t);
			}
		}

		private void ApplyLightIntensity()
		{
			float num = ((base.HasInputAuthority && _hasLocalLightTarget) ? _currentBaseIntensity : _adjustConfiguration.MaxIntensity);
			_flashlightLight.intensity = num * IntensityMultiplier;
		}

		private static Vector3 GetScreenCenterWithOffset(Vector2 offset)
		{
			return new Vector3((float)Screen.width / 2f + offset.x, (float)Screen.height / 2f + offset.y, 0f);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkedForward = _NetworkedForward;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetworkedForward = NetworkedForward;
		}
	}
}
