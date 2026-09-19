using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Features.CameraModelModule
{
	public class CameraGrabFocusService : ICameraGrabFocusService, ICameraLateResolveStep
	{
		private const string FOCUS_CAMERA_NAME = "CameraGrabFocusCamera";

		private const int FOCUS_PRIORITY = 999;

		private const float AIM_CONE_HALF_ANGLE_DEG = 75f;

		private const float ENTRY_BLEND_SECONDS = 0.3f;

		private const float SWIPE_PITCH_HEADROOM_DEG = 20f;

		private readonly CameraModel _cameraModel;

		private Func<Vector3> _focusPoint;

		private CinemachineCamera _focusCamera;

		private Transform _virtualAim;

		private Vector3 _anchorOffset;

		private Vector3 _staticAnchor;

		private bool _pinAim;

		private float _pinYaw;

		private float _pinPitch;

		private Vector2 _pendingLookDelta;

		private Vector3 _lookDirection = Vector3.forward;

		private Quaternion _entryRotation = Quaternion.identity;

		private float _activatedAt;

		public bool IsActive { get; private set; }

		public Vector3 LookDirection => _lookDirection;

		public CameraGrabFocusService(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		public void Activate(Func<Vector3> focusPoint, bool pinAim = false)
		{
			if (!(_cameraModel.CameraObject == null) && focusPoint != null)
			{
				EnsureRig();
				_focusPoint = focusPoint;
				_pinAim = pinAim;
				_pendingLookDelta = Vector2.zero;
				Transform transform = _cameraModel.CameraObject.transform;
				CameraControllerBase cameraControllerBase = ActiveController();
				Transform transform2 = cameraControllerBase?.GetTrackingTarget();
				_anchorOffset = ((transform2 != null) ? (transform.position - transform2.position) : Vector3.zero);
				_staticAnchor = transform.position;
				_lookDirection = transform.forward;
				_entryRotation = transform.rotation;
				_activatedAt = Time.time;
				_pinYaw = ((cameraControllerBase != null) ? cameraControllerBase.GetHorizontalRotation() : 0f);
				_pinPitch = ((cameraControllerBase != null) ? cameraControllerBase.GetVerticalRotation() : 0f);
				_focusCamera.Lens = LensSettings.FromCamera(_cameraModel.CameraObject);
				_focusCamera.transform.SetPositionAndRotation(transform.position, transform.rotation);
				_virtualAim.SetPositionAndRotation(transform.position, transform.rotation);
				_cameraModel.AimTransformOverride = _virtualAim;
				_focusCamera.gameObject.SetActive(value: true);
				IsActive = true;
			}
		}

		public void Deactivate()
		{
			bool isActive = IsActive;
			IsActive = false;
			_focusPoint = null;
			_pendingLookDelta = Vector2.zero;
			_cameraModel.AimTransformOverride = null;
			if (_focusCamera != null)
			{
				_focusCamera.gameObject.SetActive(value: false);
			}
			if (isActive)
			{
				CameraControllerBase cameraControllerBase = ActiveController();
				if (cameraControllerBase != null && _lookDirection.sqrMagnitude > Mathf.Epsilon)
				{
					cameraControllerBase.SetHorizontalRotation(_lookDirection);
					cameraControllerBase.SetVerticalRotation(_lookDirection);
				}
			}
		}

		public Vector2 ConsumeLookDelta()
		{
			Vector2 pendingLookDelta = _pendingLookDelta;
			_pendingLookDelta = Vector2.zero;
			return pendingLookDelta;
		}

		public void ResolveLate()
		{
			if (!IsActive)
			{
				return;
			}
			if (_cameraModel.CameraObject == null || _focusCamera == null || _virtualAim == null)
			{
				Deactivate();
				return;
			}
			CameraControllerBase cameraControllerBase = ActiveController();
			Transform transform = cameraControllerBase?.GetTrackingTarget();
			Vector3 vector = ((transform != null) ? (transform.position + _anchorOffset) : _staticAnchor);
			Vector3 forward = _focusPoint() - vector;
			Vector3 vector2 = _lookDirection;
			if (forward.sqrMagnitude > 0.0001f)
			{
				_lookDirection = forward.normalized;
				vector2 = _lookDirection;
				Quaternion quaternion = Quaternion.LookRotation(forward);
				float num = Mathf.Clamp01((Time.time - _activatedAt) / 0.3f);
				if (num < 1f)
				{
					quaternion = Quaternion.Slerp(_entryRotation, quaternion, Mathf.SmoothStep(0f, 1f, num));
					vector2 = quaternion * Vector3.forward;
				}
				_focusCamera.transform.SetPositionAndRotation(vector, quaternion);
			}
			else
			{
				_focusCamera.transform.position = vector;
			}
			if (cameraControllerBase != null)
			{
				if (_pinAim)
				{
					BankLookDeltaAndRepin(cameraControllerBase, vector2);
				}
				else
				{
					ClampAimToCone(cameraControllerBase, vector2);
				}
			}
			Quaternion rotation = ((cameraControllerBase != null) ? Quaternion.Euler(cameraControllerBase.GetVerticalRotation(), cameraControllerBase.GetHorizontalRotation(), 0f) : _virtualAim.rotation);
			_virtualAim.SetPositionAndRotation(_cameraModel.CameraObject.transform.position, rotation);
		}

		private void BankLookDeltaAndRepin(CameraControllerBase controller, Vector3 pinDirection)
		{
			float horizontalRotation = controller.GetHorizontalRotation();
			float verticalRotation = controller.GetVerticalRotation();
			_pendingLookDelta.x += Mathf.DeltaAngle(_pinYaw, horizontalRotation);
			_pendingLookDelta.y += verticalRotation - _pinPitch;
			controller.SetHorizontalRotation(pinDirection);
			controller.SetVerticalRotation(pinDirection);
			if (controller.TryGetVerticalRange(out var min, out var max))
			{
				float num = min + 20f;
				float num2 = max - 20f;
				controller.SetVerticalRotation((num <= num2) ? Mathf.Clamp(controller.GetVerticalRotation(), num, num2) : ((min + max) * 0.5f));
			}
			_pinYaw = controller.GetHorizontalRotation();
			_pinPitch = controller.GetVerticalRotation();
		}

		private void ClampAimToCone(CameraControllerBase controller, Vector3 coneCenter)
		{
			Vector3 vector = Vector3.ProjectOnPlane(coneCenter, Vector3.up);
			if (!(vector.sqrMagnitude <= Mathf.Epsilon))
			{
				Vector3 normalized = vector.normalized;
				float num = Vector3.SignedAngle(Vector3.forward, normalized, Vector3.up);
				float horizontalRotation = controller.GetHorizontalRotation();
				float f = Mathf.DeltaAngle(num, horizontalRotation);
				if (Mathf.Abs(f) > 75f)
				{
					controller.SetHorizontalRotation(num + Mathf.Sign(f) * 75f);
				}
				Vector3 axis = Vector3.Cross(Vector3.up, normalized);
				float num2 = Vector3.SignedAngle(normalized, coneCenter, axis);
				float f2 = controller.GetVerticalRotation() - num2;
				if (Mathf.Abs(f2) > 75f)
				{
					controller.SetVerticalRotation(num2 + Mathf.Sign(f2) * 75f);
				}
			}
		}

		private CameraControllerBase ActiveController()
		{
			if (!_cameraModel.Cameras.TryGetValue(_cameraModel.ActiveCameraType, out var value))
			{
				return null;
			}
			return value;
		}

		private void EnsureRig()
		{
			if (_focusCamera == null)
			{
				GameObject gameObject = new GameObject("CameraGrabFocusCamera");
				_focusCamera = gameObject.AddComponent<CinemachineCamera>();
				_focusCamera.Priority = 999;
				gameObject.SetActive(value: false);
			}
			if (_virtualAim == null)
			{
				_virtualAim = new GameObject("CameraGrabFocusVirtualAim").transform;
			}
		}
	}
}
