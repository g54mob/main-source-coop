using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace Synty.AnimationBaseLocomotion.Samples
{
	public class SampleCameraController : MonoBehaviour
	{
		private const int _LAG_DELTA_TIME_ADJUSTMENT = 20;

		[Tooltip("The character game object")]
		[SerializeField]
		private GameObject _syntyCharacter;

		[Tooltip("Main camera used for player perspective")]
		[SerializeField]
		private Camera _mainCamera;

		[SerializeField]
		private Transform _playerTarget;

		[SerializeField]
		private Transform _lockOnTarget;

		[SerializeField]
		private bool _invertCamera;

		[SerializeField]
		private bool _hideCursor;

		[SerializeField]
		private bool _isLockedOn;

		[SerializeField]
		private float _mouseSensitivity = 5f;

		[SerializeField]
		private float _cameraDistance = 5f;

		[SerializeField]
		private float _cameraHeightOffset;

		[SerializeField]
		private float _cameraHorizontalOffset;

		[SerializeField]
		private float _cameraTiltOffset;

		[SerializeField]
		private Vector2 _cameraTiltBounds = new Vector2(-10f, 45f);

		[SerializeField]
		private float _positionalCameraLag = 1f;

		[SerializeField]
		private float _rotationalCameraLag = 1f;

		private float _cameraInversion;

		private InputReader _inputReader;

		private float _lastAngleX;

		private float _lastAngleY;

		private Vector3 _lastPosition;

		private float _newAngleX;

		private float _newAngleY;

		private Vector3 _newPosition;

		private float _rotationX;

		private float _rotationY;

		private Transform _syntyCamera;

		private void Start()
		{
			_syntyCamera = base.gameObject.transform.GetChild(0);
			_inputReader = _syntyCharacter.GetComponent<InputReader>();
			_playerTarget = _syntyCharacter.transform.Find("SyntyPlayer_LookAt");
			_lockOnTarget = _syntyCharacter.transform.Find("TargetLockOnPos");
			if (_hideCursor)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			_cameraInversion = (_invertCamera ? 1 : (-1));
			base.transform.position = _playerTarget.position;
			base.transform.rotation = _playerTarget.rotation;
			_lastPosition = base.transform.position;
			_syntyCamera.localPosition = new Vector3(_cameraHorizontalOffset, _cameraHeightOffset, _cameraDistance * -1f);
			_syntyCamera.localEulerAngles = new Vector3(_cameraTiltOffset, 0f, 0f);
		}

		private void Update()
		{
			float num = 1f / (_positionalCameraLag / 20f);
			float num2 = 1f / (_rotationalCameraLag / 20f);
			_rotationX = _inputReader._mouseDelta.y * _cameraInversion * _mouseSensitivity;
			_rotationY = _inputReader._mouseDelta.x * _mouseSensitivity;
			_newAngleX += _rotationX;
			_newAngleX = Mathf.Clamp(_newAngleX, _cameraTiltBounds.x, _cameraTiltBounds.y);
			_newAngleX = Mathf.Lerp(_lastAngleX, _newAngleX, num2 * Time.deltaTime);
			if (_isLockedOn)
			{
				Quaternion b = Quaternion.LookRotation(_lockOnTarget.position - _playerTarget.position);
				_newAngleY = Quaternion.Lerp(base.transform.rotation, b, num2 * Time.deltaTime).eulerAngles.y;
			}
			else
			{
				_newAngleY += _rotationY;
				_newAngleY = Mathf.Lerp(_lastAngleY, _newAngleY, num2 * Time.deltaTime);
			}
			_newPosition = _playerTarget.position;
			_newPosition = Vector3.Lerp(_lastPosition, _newPosition, num * Time.deltaTime);
			base.transform.position = _newPosition;
			base.transform.eulerAngles = new Vector3(_newAngleX, _newAngleY, 0f);
			_syntyCamera.localPosition = new Vector3(_cameraHorizontalOffset, _cameraHeightOffset, _cameraDistance * -1f);
			_syntyCamera.localEulerAngles = new Vector3(_cameraTiltOffset, 0f, 0f);
			_lastPosition = _newPosition;
			_lastAngleX = _newAngleX;
			_lastAngleY = _newAngleY;
		}

		public void LockOn(bool enable, Transform newLockOnTarget)
		{
			_isLockedOn = enable;
			if (newLockOnTarget != null)
			{
				_lockOnTarget = newLockOnTarget;
			}
		}

		public Vector3 GetCameraPosition()
		{
			return _mainCamera.transform.position;
		}

		public Vector3 GetCameraForward()
		{
			return _mainCamera.transform.forward;
		}

		public Vector3 GetCameraForwardZeroedY()
		{
			return new Vector3(_mainCamera.transform.forward.x, 0f, _mainCamera.transform.forward.z);
		}

		public Vector3 GetCameraForwardZeroedYNormalised()
		{
			return GetCameraForwardZeroedY().normalized;
		}

		public Vector3 GetCameraRightZeroedY()
		{
			return new Vector3(_mainCamera.transform.right.x, 0f, _mainCamera.transform.right.z);
		}

		public Vector3 GetCameraRightZeroedYNormalised()
		{
			return GetCameraRightZeroedY().normalized;
		}

		public float GetCameraTiltX()
		{
			return _mainCamera.transform.eulerAngles.x;
		}
	}
}
