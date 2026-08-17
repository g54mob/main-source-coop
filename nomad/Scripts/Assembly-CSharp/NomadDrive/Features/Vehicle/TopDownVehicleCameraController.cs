using System;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.FloatingOrigin;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public class TopDownVehicleCameraController : MonoBehaviour, IFloatingOriginShiftable
	{
		[Header("References")]
		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private Transform target;

		[SerializeField]
		private TopDownVehicleCameraConfig config;

		private float _currentAngle;

		private float _currentHeight;

		private float _currentDistance;

		private float _targetDistance;

		private bool _isControllerActive;

		private bool _inputEnabled = true;

		private float _smoothedMouseX;

		private float _smoothedMouseY;

		private Vector3 _positionVelocity;

		private bool _runtimeLimitOrbitAngle;

		private float _runtimeMinOrbitAngle;

		private float _runtimeMaxOrbitAngle;

		private bool _originalCameraState;

		public Action OnControllerActivated;

		public Action OnControllerDeactivated;

		private void OnEnable()
		{
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDisable()
		{
			FloatingOriginManager.UnregisterShiftable(this);
		}

		public void OnOriginShift(Vector3 delta)
		{
			_positionVelocity = Vector3.zero;
		}

		private void Start()
		{
			InitializeCamera();
			if (config == null)
			{
				EvilLogger.LogError("[TopDownCamera] Config asset is not assigned!", "Start", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\TopDownVehicleCamera\\TopDownVehicleCameraController.cs", 66);
				base.enabled = false;
				return;
			}
			_currentHeight = config.height;
			_currentDistance = config.distance;
			_targetDistance = config.distance;
			_runtimeLimitOrbitAngle = config.limitOrbitAngle;
			_runtimeMinOrbitAngle = config.minOrbitAngle;
			_runtimeMaxOrbitAngle = config.maxOrbitAngle;
			if (target == null)
			{
				VehicleManager vehicleManager = UnityEngine.Object.FindFirstObjectByType<VehicleManager>();
				if (vehicleManager != null)
				{
					target = vehicleManager.transform;
				}
			}
			if (target != null)
			{
				if (_runtimeLimitOrbitAngle)
				{
					_currentAngle = 0f;
				}
				else
				{
					Vector3 vector = base.transform.position - target.position;
					vector.y = 0f;
					_currentAngle = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				}
			}
			DeactivateController();
		}

		private void InitializeCamera()
		{
			if (targetCamera == null)
			{
				targetCamera = GetComponent<Camera>();
			}
			if (targetCamera == null)
			{
				targetCamera = GetComponentInChildren<Camera>();
			}
			if (targetCamera == null)
			{
				targetCamera = Camera.main;
			}
			if (!(targetCamera == null))
			{
				_originalCameraState = targetCamera.enabled;
			}
		}

		private void Update()
		{
			if (!(target == null) && !(targetCamera == null) && _isControllerActive && _inputEnabled)
			{
				HandleInput();
			}
		}

		private void LateUpdate()
		{
			if (!(target == null) && !(targetCamera == null) && _isControllerActive && _inputEnabled)
			{
				UpdateCameraPosition();
			}
		}

		private void HandleInput()
		{
			float mouseSmoothFactor = config.mouseSmoothFactor;
			if (config.useMouseForOrbit)
			{
				float axis = Input.GetAxis("Mouse X");
				_smoothedMouseX = Mathf.Lerp(axis, _smoothedMouseX, mouseSmoothFactor);
				_currentAngle += _smoothedMouseX * config.mouseOrbitSensitivity;
			}
			if (config.useMouseForHeight)
			{
				float axis2 = Input.GetAxis("Mouse Y");
				_smoothedMouseY = Mathf.Lerp(axis2, _smoothedMouseY, mouseSmoothFactor);
				_currentHeight -= _smoothedMouseY * config.mouseHeightSensitivity;
			}
			float axis3 = Input.GetAxis("Mouse ScrollWheel");
			if (axis3 != 0f)
			{
				_targetDistance -= axis3 * 5f;
				_targetDistance = Mathf.Clamp(_targetDistance, config.minDistance, config.maxDistance);
			}
			_currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, config.scrollSmoothSpeed * Time.deltaTime);
			_currentHeight = Mathf.Clamp(_currentHeight, config.minHeight, config.maxHeight);
			_currentDistance = Mathf.Clamp(_currentDistance, config.minDistance, config.maxDistance);
			if (_runtimeLimitOrbitAngle)
			{
				_currentAngle = Mathf.Clamp(_currentAngle, _runtimeMinOrbitAngle, _runtimeMaxOrbitAngle);
				return;
			}
			if (_currentAngle >= 360f)
			{
				_currentAngle -= 360f;
			}
			if (_currentAngle < 0f)
			{
				_currentAngle += 360f;
			}
		}

		private void UpdateCameraPosition()
		{
			float f = (target.eulerAngles.y + 180f + _currentAngle) * ((float)Math.PI / 180f);
			Vector3 vector = new Vector3(Mathf.Sin(f) * _currentDistance, _currentHeight, Mathf.Cos(f) * _currentDistance);
			Vector3 vector2 = target.position + vector;
			targetCamera.transform.position = Vector3.SmoothDamp(targetCamera.transform.position, vector2, ref _positionVelocity, config.followSmoothTime);
			if (config.lookAtTarget)
			{
				Vector3 vector3 = target.position - targetCamera.transform.position;
				if (vector3 != Vector3.zero)
				{
					Quaternion b = Quaternion.LookRotation(vector3);
					float t = 1f - Mathf.Exp((0f - config.rotationSmoothSpeed) * Time.deltaTime);
					targetCamera.transform.rotation = Quaternion.Slerp(targetCamera.transform.rotation, b, t);
				}
			}
		}

		public void SetTarget(Transform newTarget)
		{
			target = newTarget;
		}

		public void SetDistance(float newDistance)
		{
			_targetDistance = Mathf.Clamp(newDistance, config.minDistance, config.maxDistance);
			_currentDistance = _targetDistance;
		}

		public void SetHeight(float newHeight)
		{
			_currentHeight = Mathf.Clamp(newHeight, config.minHeight, config.maxHeight);
		}

		public void RotateToAngle(float angle)
		{
			if (_runtimeLimitOrbitAngle)
			{
				_currentAngle = Mathf.Clamp(angle, _runtimeMinOrbitAngle, _runtimeMaxOrbitAngle);
			}
			else
			{
				_currentAngle = angle;
			}
		}

		public void SetOrbitLimits(float minAngle, float maxAngle)
		{
			_runtimeMinOrbitAngle = minAngle;
			_runtimeMaxOrbitAngle = maxAngle;
		}

		public void SetOrbitLimitEnabled(bool enabled)
		{
			_runtimeLimitOrbitAngle = enabled;
		}

		public void ResetToRearView()
		{
			_currentAngle = 0f;
		}

		public void SetCameraActive(bool active)
		{
			if (targetCamera != null)
			{
				targetCamera.enabled = active;
			}
		}

		public Vector3 GetCurrentOffset()
		{
			if (target != null && targetCamera != null)
			{
				return targetCamera.transform.position - target.position;
			}
			return Vector3.zero;
		}

		public float GetCurrentAngle()
		{
			return _currentAngle;
		}

		public float GetCurrentDistance()
		{
			return _currentDistance;
		}

		public float GetCurrentHeight()
		{
			return _currentHeight;
		}

		public void ActivateController()
		{
			_isControllerActive = true;
			_inputEnabled = true;
			_positionVelocity = Vector3.zero;
			_smoothedMouseX = 0f;
			_smoothedMouseY = 0f;
			_currentAngle = config.defaultOrbitAngle;
			_currentHeight = config.height;
			_currentDistance = config.distance;
			_targetDistance = config.distance;
			if (targetCamera != null)
			{
				targetCamera.enabled = true;
			}
			OnControllerActivated?.Invoke();
		}

		public void DeactivateController()
		{
			_isControllerActive = false;
			_inputEnabled = false;
			if (targetCamera != null)
			{
				targetCamera.enabled = false;
			}
			OnControllerDeactivated?.Invoke();
		}

		public void ToggleController()
		{
			if (_isControllerActive)
			{
				DeactivateController();
			}
			else
			{
				ActivateController();
			}
		}

		public bool IsControllerActive()
		{
			return _isControllerActive;
		}

		public bool IsInputEnabled()
		{
			return _inputEnabled;
		}

		public void SetInputEnabled(bool enabled)
		{
			_inputEnabled = enabled;
		}
	}
}
