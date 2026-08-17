using EvilCore.Extensions;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Inputs;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.Downed
{
	[DefaultExecutionOrder(20000)]
	public class ThirdPersonSpectatorCamera : MonoBehaviour
	{
		[Header("Framing")]
		[SerializeField]
		private float distance = 3.5f;

		[SerializeField]
		private float heightOffset = 1.1f;

		[SerializeField]
		private float minPitch = -25f;

		[SerializeField]
		private float maxPitch = 70f;

		[SerializeField]
		private float startPitch = 25f;

		[Header("Look")]
		[Tooltip("Free-look sensitivity (deg/s per look-axis unit). Lower = less sensitive.")]
		[SerializeField]
		private float yawSpeed = 90f;

		[SerializeField]
		private float pitchSpeed = 65f;

		[Tooltip("Collision cast-distance ease-out rate (the camera pulls in instantly, eases back out at this rate).")]
		[SerializeField]
		private float followSmooth = 12f;

		[Header("Collision")]
		[SerializeField]
		private float collisionRadius = 0.25f;

		[SerializeField]
		private LayerMask collisionMask = -1;

		private Transform _target;

		private Transform _cameraToDrive;

		private FirstPersonController _fpc;

		private Transform _originalParent;

		private Vector3 _originalLocalPos;

		private Quaternion _originalLocalRot;

		private float _yaw;

		private float _pitch;

		private bool _active;

		private float _currentCastDistance;

		private bool _loggedDrive;

		private bool _lookEnabled = true;

		[Inject]
		private IGameUIManager _guiManager;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		public void Activate(Transform target, Transform cameraToDrive, FirstPersonController fpc)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			base.enabled = true;
			_loggedDrive = false;
			_cameraToDrive = cameraToDrive;
			_fpc = fpc;
			_fpc?.SuspendCameraDriving(suspended: true);
			Camera[] componentsInChildren = GetComponentsInChildren<Camera>(includeInactive: true);
			foreach (Camera camera in componentsInChildren)
			{
				if (camera != null && camera.transform != _cameraToDrive)
				{
					camera.enabled = false;
				}
			}
			if (_cameraToDrive != null)
			{
				_originalParent = _cameraToDrive.parent;
				_originalLocalPos = _cameraToDrive.localPosition;
				_originalLocalRot = _cameraToDrive.localRotation;
			}
			_yaw = ((target != null) ? target.eulerAngles.y : ((_cameraToDrive != null) ? _cameraToDrive.eulerAngles.y : 0f));
			_pitch = startPitch;
			_currentCastDistance = distance;
			_active = true;
			SetTarget(target);
			_ = _cameraToDrive == null;
		}

		public void Deactivate()
		{
			_active = false;
			RestoreCameraParent();
			_fpc?.SuspendCameraDriving(suspended: false);
			_fpc?.ResetHeadRotation();
			_target = null;
			_cameraToDrive = null;
			_fpc = null;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
			if (!(_cameraToDrive == null) && !(target == null))
			{
				_cameraToDrive.SetParent(target, worldPositionStays: false);
				ApplyLocalOrbit();
			}
		}

		public void EmergencyUnparentCamera()
		{
			RestoreCameraParent();
		}

		public void SetLookEnabled(bool enabled)
		{
			_lookEnabled = enabled;
		}

		private void RestoreCameraParent()
		{
			if (!(_cameraToDrive == null) && !(_cameraToDrive.parent == _originalParent))
			{
				_cameraToDrive.SetParent(_originalParent, worldPositionStays: false);
				_cameraToDrive.localPosition = _originalLocalPos;
				_cameraToDrive.localRotation = _originalLocalRot;
			}
		}

		private void LateUpdate()
		{
			if (!_active)
			{
				return;
			}
			if (_target == null || _cameraToDrive == null)
			{
				if (!_loggedDrive)
				{
					_loggedDrive = true;
				}
				return;
			}
			if (_lookEnabled && (_guiManager == null || !_guiManager.IsGameMenuOpen))
			{
				_yaw += BaseInputs.GetLookHorizontal() * yawSpeed * Time.deltaTime;
				_pitch -= BaseInputs.GetLookVertical() * pitchSpeed * Time.deltaTime;
				_pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
			}
			ApplyLocalOrbit();
			if (!_loggedDrive)
			{
				_loggedDrive = true;
			}
		}

		private void ApplyLocalOrbit()
		{
			if (!(_target == null) && !(_cameraToDrive == null))
			{
				Vector3 vector = Vector3.up * heightOffset;
				Vector3 vector2 = Quaternion.Euler(_pitch, _yaw, 0f) * Vector3.back;
				Vector3 origin = _target.TransformPoint(vector);
				Vector3 direction = _target.TransformDirection(vector2);
				float num = distance;
				if (Physics.SphereCast(origin, collisionRadius, direction, out var hitInfo, distance, collisionMask, QueryTriggerInteraction.Ignore))
				{
					num = hitInfo.distance;
				}
				if (num < _currentCastDistance)
				{
					_currentCastDistance = num;
				}
				else
				{
					_currentCastDistance = Mathf.Lerp(_currentCastDistance, num, 1f - Mathf.Exp((0f - followSmooth) * Time.deltaTime));
				}
				Vector3 vector3 = vector + vector2 * _currentCastDistance;
				_cameraToDrive.localPosition = vector3;
				_cameraToDrive.localRotation = Quaternion.LookRotation(vector - vector3, Vector3.up);
			}
		}
	}
}
