using Mirror;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
public class FreeCamera : MonoBehaviour
{
	[Header("Hareket")]
	public float moveSpeed = 6f;

	public float sprintMultiplier = 3f;

	[Tooltip("Yumuşak hızlanma/yavaşlama süresi — büyük değer daha 'ağır'/sinematik his verir")]
	public float moveSmoothTime = 0.2f;

	[Header("Bakış")]
	public float lookSensitivity = 2f;

	[Tooltip("Bakış yumuşatma süresi — büyük değer daha yumuşak ama gecikmeli döner")]
	public float lookSmoothTime = 0.08f;

	[Header("Scroll ile Hız Ayarı")]
	public float scrollSpeedStep = 2f;

	public float minSpeed = 1f;

	public float maxSpeed = 50f;

	[Header("Kontrol")]
	[Tooltip("Admin panelini tekrar açmadan direkt karaktere dönmek için")]
	public KeyCode exitKey = KeyCode.F2;

	private Camera _cam;

	private AudioListener _listener;

	private AudioListener _playerListener;

	private Vector3 _currentVelocity;

	private Vector3 _dampVelocityRef;

	private float _yaw;

	private float _pitch;

	private float _yawVel;

	private float _pitchVel;

	private float _targetYaw;

	private float _targetPitch;

	private float _currentSpeed;

	private PlayerController _playerController;

	private CharacterController _characterController;

	private NetworkedCameraController _networkedCam;

	private PlayerVoiceMonitor _voiceMonitor;

	private PlayerModelController _modelController;

	private AudioListener _spectatorListener;

	public static FreeCamera Instance { get; private set; }

	public bool IsActive { get; private set; }

	private void Awake()
	{
		Instance = this;
		_cam = GetComponent<Camera>();
		_cam.enabled = false;
		_listener = GetComponent<AudioListener>();
		_listener.enabled = false;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void Toggle()
	{
		if (IsActive)
		{
			Deactivate();
		}
		else
		{
			Activate();
		}
	}

	public void Activate()
	{
		if (IsActive)
		{
			return;
		}
		IsActive = true;
		_currentSpeed = moveSpeed;
		_currentVelocity = Vector3.zero;
		_dampVelocityRef = Vector3.zero;
		_yawVel = (_pitchVel = 0f);
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (localPlayer != null)
		{
			_playerController = localPlayer.GetComponent<PlayerController>();
			_characterController = localPlayer.GetComponent<CharacterController>();
			_networkedCam = localPlayer.GetComponent<NetworkedCameraController>();
			_voiceMonitor = localPlayer.GetComponent<PlayerVoiceMonitor>();
			_modelController = localPlayer.GetComponent<PlayerModelController>();
			Transform transform = ((_networkedCam != null) ? _networkedCam.CameraTransform : null);
			if (transform != null)
			{
				base.transform.SetPositionAndRotation(transform.position, transform.rotation);
			}
			else
			{
				base.transform.SetPositionAndRotation(localPlayer.transform.position + Vector3.up * 1.5f, localPlayer.transform.rotation);
			}
			if (_playerController != null)
			{
				_playerController.enabled = false;
			}
			if (_characterController != null)
			{
				_characterController.enabled = false;
			}
			if (_voiceMonitor != null)
			{
				_voiceMonitor.SetSuspended(suspended: true);
			}
			if (_modelController != null)
			{
				_modelController.SetModelVisible(visible: false);
			}
			if (_networkedCam != null)
			{
				if (_networkedCam.playerCamera != null)
				{
					_networkedCam.playerCamera.enabled = false;
					_playerListener = _networkedCam.playerCamera.GetComponent<AudioListener>();
					if (_playerListener != null)
					{
						_playerListener.enabled = false;
					}
				}
				_networkedCam.enabled = false;
			}
		}
		if (SpectatorController.IsSpectating && SpectatorController.Instance != null && SpectatorController.Instance.spectatorCamera != null)
		{
			Camera spectatorCamera = SpectatorController.Instance.spectatorCamera;
			spectatorCamera.enabled = false;
			_spectatorListener = spectatorCamera.GetComponent<AudioListener>();
			if (_spectatorListener != null)
			{
				_spectatorListener.enabled = false;
			}
		}
		Vector3 eulerAngles = base.transform.eulerAngles;
		_pitch = (_targetPitch = NormalizePitch(eulerAngles.x));
		_yaw = (_targetYaw = eulerAngles.y);
		_cam.enabled = true;
		_listener.enabled = true;
		PlayerHUD.Instance?.SetHudVisible(visible: false);
		PlayerOutline.SetSuppressAll(suppress: true);
		NameTagManager.Instance?.SetSuppressAll(suppress: true);
	}

	public void Deactivate()
	{
		if (!IsActive)
		{
			return;
		}
		IsActive = false;
		_cam.enabled = false;
		_listener.enabled = false;
		if (_playerController != null)
		{
			_playerController.enabled = true;
		}
		if (_characterController != null)
		{
			_characterController.enabled = true;
		}
		if (_voiceMonitor != null)
		{
			_voiceMonitor.SetSuspended(suspended: false);
		}
		if (_modelController != null)
		{
			_modelController.SetModelVisible(visible: true);
		}
		if (_networkedCam != null)
		{
			_networkedCam.enabled = true;
			if (_networkedCam.playerCamera != null)
			{
				_networkedCam.playerCamera.enabled = true;
			}
		}
		if (_playerListener != null)
		{
			_playerListener.enabled = true;
		}
		if (SpectatorController.Instance != null && SpectatorController.Instance.spectatorCamera != null)
		{
			SpectatorController.Instance.spectatorCamera.enabled = true;
		}
		if (_spectatorListener != null)
		{
			_spectatorListener.enabled = true;
		}
		_playerController = null;
		_characterController = null;
		_networkedCam = null;
		_voiceMonitor = null;
		_modelController = null;
		_playerListener = null;
		_spectatorListener = null;
		PlayerHUD.Instance?.SetHudVisible(visible: true);
		PlayerOutline.SetSuppressAll(suppress: false);
		NameTagManager.Instance?.SetSuppressAll(suppress: false);
	}

	private static float NormalizePitch(float x)
	{
		if (!(x > 180f))
		{
			return x;
		}
		return x - 360f;
	}

	private void Update()
	{
		if (IsActive)
		{
			if (Input.GetKeyDown(exitKey))
			{
				Deactivate();
				return;
			}
			HandleLook();
			HandleMove();
		}
	}

	private void HandleLook()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			float num = Input.GetAxis("Mouse X") * lookSensitivity;
			float num2 = Input.GetAxis("Mouse Y") * lookSensitivity;
			_targetYaw += num;
			_targetPitch -= num2;
			_targetPitch = Mathf.Clamp(_targetPitch, -89f, 89f);
			_yaw = Mathf.SmoothDampAngle(_yaw, _targetYaw, ref _yawVel, lookSmoothTime);
			_pitch = Mathf.SmoothDampAngle(_pitch, _targetPitch, ref _pitchVel, lookSmoothTime);
			base.transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
		}
	}

	private void HandleMove()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (Mathf.Abs(axis) > 0.0001f)
		{
			_currentSpeed = Mathf.Clamp(_currentSpeed + axis * scrollSpeedStep * 10f, minSpeed, maxSpeed);
		}
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		float num = 0f;
		if (Input.GetKey(KeyCode.Space))
		{
			num += 1f;
		}
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
		{
			num -= 1f;
		}
		Vector3 vector = Vector3.ClampMagnitude(base.transform.right * axisRaw + base.transform.forward * axisRaw2 + Vector3.up * num, 1f);
		bool key = Input.GetKey(KeyCode.LeftShift);
		float num2 = _currentSpeed * (key ? sprintMultiplier : 1f);
		Vector3 target = vector * num2;
		Vector3 currentVelocity = Vector3.SmoothDamp(_currentVelocity, target, ref _dampVelocityRef, moveSmoothTime);
		_currentVelocity = currentVelocity;
		base.transform.position += _currentVelocity * Time.deltaTime;
	}
}
