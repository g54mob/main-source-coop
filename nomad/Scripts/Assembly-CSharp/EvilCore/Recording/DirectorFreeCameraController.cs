using EvilCore.Managers;
using Unity.Cinemachine;
using UnityEngine;

namespace EvilCore.Recording
{
	public class DirectorFreeCameraController : MonoBehaviour
	{
		private const int FreeCamPriority = 30;

		private float _moveSpeed = 10f;

		private float _fastMoveSpeed = 30f;

		private float _shiftGrowthRate = 1.4f;

		private float _mouseSensitivity = 3f;

		private float _scrollSensitivity = 5f;

		private float _accelerationRate = 20f;

		private float _decelerationRate = 40f;

		private float _currentSpeed;

		private float _shiftHoldTime;

		private float _pitch;

		private float _yaw;

		private bool _isControlling;

		private CinemachineCamera _vcam;

		public float MoveSpeed
		{
			get
			{
				return _moveSpeed;
			}
			set
			{
				_moveSpeed = value;
			}
		}

		public float FastMoveSpeed
		{
			get
			{
				return _fastMoveSpeed;
			}
			set
			{
				_fastMoveSpeed = value;
			}
		}

		public float MouseSensitivity
		{
			get
			{
				return _mouseSensitivity;
			}
			set
			{
				_mouseSensitivity = value;
			}
		}

		public bool IsControlling => _isControlling;

		private void Awake()
		{
			GameObject gameObject = new GameObject("VCam_FreeCam");
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			_vcam = gameObject.AddComponent<CinemachineCamera>();
			_vcam.Priority = 0;
		}

		public void StartControlling()
		{
			_isControlling = true;
			_currentSpeed = _moveSpeed;
			_shiftHoldTime = 0f;
			Vector3 eulerAngles = base.transform.eulerAngles;
			_yaw = eulerAngles.y;
			_pitch = eulerAngles.x;
			if (_pitch > 180f)
			{
				_pitch -= 360f;
			}
			if (_vcam != null)
			{
				_vcam.Priority = 30;
			}
			GameCursor.Lock();
		}

		public void StopControlling()
		{
			_isControlling = false;
			_shiftHoldTime = 0f;
			if (_vcam != null)
			{
				_vcam.Priority = 0;
			}
			GameCursor.Unlock();
		}

		private void Update()
		{
			if (_isControlling)
			{
				HandleMouseLook();
				HandleMovement();
				HandleSpeedScroll();
				if (Input.GetMouseButtonDown(1))
				{
					StopControlling();
				}
			}
		}

		private void HandleMouseLook()
		{
			float num = Input.GetAxis("Mouse X") * _mouseSensitivity;
			float num2 = Input.GetAxis("Mouse Y") * _mouseSensitivity;
			_yaw += num;
			_pitch -= num2;
			_pitch = Mathf.Clamp(_pitch, -89f, 89f);
			base.transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
		}

		private void HandleMovement()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			bool key = Input.GetKey(KeyCode.LeftShift);
			if (key)
			{
				_shiftHoldTime += unscaledDeltaTime;
			}
			else
			{
				_shiftHoldTime = 0f;
			}
			float num2;
			if (key)
			{
				float num = 1f + _shiftHoldTime * _shiftGrowthRate + _shiftHoldTime * _shiftHoldTime * _shiftGrowthRate * 0.5f;
				num2 = _fastMoveSpeed * num;
			}
			else
			{
				num2 = _moveSpeed;
			}
			float num3 = ((_currentSpeed < num2) ? _accelerationRate : _decelerationRate);
			_currentSpeed = Mathf.MoveTowards(_currentSpeed, num2, num3 * unscaledDeltaTime);
			Vector3 zero = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				zero += base.transform.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				zero -= base.transform.forward;
			}
			if (Input.GetKey(KeyCode.D))
			{
				zero += base.transform.right;
			}
			if (Input.GetKey(KeyCode.A))
			{
				zero -= base.transform.right;
			}
			if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
			{
				zero += Vector3.up;
			}
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.C))
			{
				zero -= Vector3.up;
			}
			if (zero.sqrMagnitude > 0f)
			{
				base.transform.position += zero.normalized * (_currentSpeed * unscaledDeltaTime);
			}
		}

		private void HandleSpeedScroll()
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (Mathf.Abs(axis) > 0.001f)
			{
				_moveSpeed = Mathf.Clamp(_moveSpeed + axis * _scrollSensitivity, 0.5f, 100f);
				_fastMoveSpeed = _moveSpeed * 3f;
			}
		}

		public Vector3 GetCurrentPosition()
		{
			return base.transform.position;
		}

		public Quaternion GetCurrentRotation()
		{
			return base.transform.rotation;
		}

		private void OnDestroy()
		{
			if (_vcam != null)
			{
				Object.Destroy(_vcam.gameObject);
			}
		}
	}
}
