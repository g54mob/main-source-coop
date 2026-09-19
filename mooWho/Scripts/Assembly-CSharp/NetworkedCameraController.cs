using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class NetworkedCameraController : NetworkBehaviour
{
	public enum CameraMode
	{
		ThirdPerson = 0,
		FirstPerson = 1
	}

	[Header("Rig References")]
	public Transform cameraRig;

	public Camera playerCamera;

	[Header("Orbit Settings")]
	public float mouseSensitivity = 2f;

	public float pitchMin = -30f;

	public float pitchMax = 60f;

	[Header("First Person")]
	[Tooltip("Normal FPS kamera offset")]
	public Vector3 fpsEyeOffset = new Vector3(0f, 0.7f, 0.1f);

	[Tooltip("Aim (nişan) FPS kamera offset")]
	public Vector3 aimFpsEyeOffset = new Vector3(0.12f, 0.63f, 0.34f);

	[Tooltip("Aim geçiş hızı")]
	public float aimTransitionSpeed = 10f;

	private bool _isAiming;

	private Vector3 _currentEyeOffset;

	[Tooltip("FPS'te dikey bakış sınırı")]
	public float fpsPitchMin = -70f;

	public float fpsPitchMax = 80f;

	[Header("Zoom / Distance (TPS)")]
	[Tooltip("Engel yokken kamera bu mesafede durur — artık scroll ile değiştirilemiyor, tamamen otomatik: engelsizken burada, bir engele çarpınca yakınlaşır.")]
	public float defaultDistance = 5f;

	[Tooltip("Bir engelin İÇİNE girilirse (SphereCast'in kaçırdığı köşe durumları dahil) kamera en fazla bu kadar yakınlaşabilir")]
	public float minDistance = 1.5f;

	public LayerMask cameraObstacleMask;

	public float cameraRadius = 0.2f;

	[Tooltip("Bir engele çarpınca yakınlaşma hızı (birim/sn) — duvara gömülmeyi önlemek için ani/keskin olsun diye zoomOutSpeed'den yüksek tutulmalı")]
	public float zoomInSpeed = 25f;

	[Tooltip("Engelden uzaklaşıp varsayılan mesafeye dönüş hızı (birim/sn) — klasik third-person oyunlardaki gibi daha yumuşak/yavaş")]
	public float zoomOutSpeed = 6f;

	[Header("Free Look (Alt)")]
	[Tooltip("Basılıyken serbest bakış — karakter yönü sabit, kamera döner")]
	public KeyCode freeLookKey = KeyCode.LeftAlt;

	private bool _freeLooking;

	private float _freeLookYaw;

	private float _freeLookPitch;

	[Header("Follow Smoothing")]
	public float followSmoothing = 0.1f;

	[Header("Kamera Sallanması — Hunter (FPS)")]
	public bool enableCameraShake = true;

	[Tooltip("Koşarken sürekli hafif sallanma — pozisyon genliği")]
	public float runShakeAmplitudePos = 0.015f;

	[Tooltip("Koşarken sürekli hafif sallanma — rotasyon (derece)")]
	public float runShakeAmplitudeRot = 0.4f;

	public float runShakeFrequency = 9f;

	[Tooltip("Ateş edince ani sarsıntı — pozisyon genliği")]
	public float fireShakeAmplitudePos = 0.05f;

	[Tooltip("Ateş edince ani sarsıntı — rotasyon (derece)")]
	public float fireShakeAmplitudeRot = 2.5f;

	[Tooltip("Ateş sarsıntısının sönümlenme hızı (sn^-1)")]
	public float fireShakeDecay = 6f;

	private float _fireShakeStrength;

	private float _shakeSeed;

	private PlayerController _pc;

	private Health _health;

	private float _yaw;

	private float _pitch;

	private float _smoothedObstacleDistance = -1f;

	private Vector3 _rigVelocity;

	[SerializeField]
	private Vector3 pivotOffset = new Vector3(0f, 1.5f, 0f);

	[Header("First Person — Head Referansı (opsiyonel)")]
	[Tooltip("Atanırsa FPS kamera pozisyonu bu bone'un (kafa) world pozisyonuna göre hesaplanır — yürüme/animasyon sallanması sırasında kamera kafayla birlikte hareket eder. Boşsa eskisi gibi root transform kullanılır. Not: rotasyon (pitch/yaw) her zaman mouse ile kontrollü kalır, kafa bone'unun kendi animasyon rotasyonundan ETKİLENMEZ — aksi halde kafa animasyonu görüşü istenmeden döndürürdü. Atarsan fpsEyeOffset/aimFpsEyeOffset'i muhtemelen küçültmen gerekir (artık baz nokta root değil, kafa).")]
	[SerializeField]
	private Transform headBone;

	[Header("Hunter Aim IK (Spine_01 network sync)")]
	[Tooltip("Animation Rigging AimIK (MultiAimConstraint) Source Object'i — prefabda 'AimTarget'")]
	[SerializeField]
	private Transform aimTarget;

	[Tooltip("AimTarget'ın göz pozisyonundan ne kadar ileride durduğu")]
	[SerializeField]
	private float aimTargetDistance = 20f;

	[Tooltip("Uzak client'larda pitch yumuşatma hızı")]
	[SerializeField]
	private float aimSmoothSpeed = 14f;

	[SyncVar]
	private float _syncedPitch;

	private PlayerRoleData _role;

	private float _displayPitch;

	private bool _hasDisplayPitch;

	public bool IsFreeLooking => _freeLooking;

	public Transform CameraTransform
	{
		get
		{
			if (!(playerCamera != null))
			{
				return null;
			}
			return playerCamera.transform;
		}
	}

	public CameraMode Mode { get; private set; }

	public bool IsFirstPerson => Mode == CameraMode.FirstPerson;

	public float Yaw => _yaw;

	public float Pitch => _pitch;

	public Vector3 MovementForward => Quaternion.Euler(0f, _yaw, 0f) * Vector3.forward;

	public Vector3 MovementRight => Quaternion.Euler(0f, _yaw, 0f) * Vector3.right;

	public float Network_syncedPitch
	{
		get
		{
			return _syncedPitch;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _syncedPitch, 1uL, null);
		}
	}

	public override void OnStartLocalPlayer()
	{
		if (playerCamera != null)
		{
			playerCamera.gameObject.SetActive(value: true);
			if (SceneManager.GetActiveScene().name != "Lobby")
			{
				playerCamera.enabled = true;
			}
		}
		_yaw = base.transform.eulerAngles.y;
		_pitch = 15f;
		_currentEyeOffset = fpsEyeOffset;
	}

	private void Awake()
	{
		if (playerCamera != null)
		{
			playerCamera.gameObject.SetActive(value: false);
		}
		_pc = GetComponent<PlayerController>();
		_role = GetComponent<PlayerRoleData>();
		_health = GetComponent<Health>();
		_shakeSeed = Random.Range(0f, 1000f);
		syncDirection = SyncDirection.ClientToServer;
	}

	public void SetAiming(bool aiming)
	{
		_isAiming = aiming;
	}

	public void TriggerFireShake()
	{
		_fireShakeStrength = 1f;
	}

	private Vector3 ComputeShake(out float shakeRotZ)
	{
		shakeRotZ = 0f;
		Vector3 zero = Vector3.zero;
		if (!enableCameraShake)
		{
			return zero;
		}
		if (_pc != null && _pc.IsRunning)
		{
			float y = Time.time * runShakeFrequency;
			float num = (Mathf.PerlinNoise(_shakeSeed, y) - 0.5f) * 2f;
			float f = (Mathf.PerlinNoise(_shakeSeed + 10f, y) - 0.5f) * 2f;
			zero += new Vector3(num, Mathf.Abs(f), 0f) * runShakeAmplitudePos;
			shakeRotZ += num * runShakeAmplitudeRot;
		}
		if (_fireShakeStrength > 0.001f)
		{
			float y2 = Time.time * 45f;
			float num2 = (Mathf.PerlinNoise(_shakeSeed + 20f, y2) - 0.5f) * 2f;
			float y3 = (Mathf.PerlinNoise(_shakeSeed + 30f, y2) - 0.5f) * 2f;
			zero += new Vector3(num2, y3, 0f) * fireShakeAmplitudePos * _fireShakeStrength;
			shakeRotZ += num2 * fireShakeAmplitudeRot * _fireShakeStrength;
			_fireShakeStrength = Mathf.Max(0f, _fireShakeStrength - Time.deltaTime * fireShakeDecay);
		}
		return zero;
	}

	public void SetMode(CameraMode mode)
	{
		Mode = mode;
		if (mode == CameraMode.FirstPerson)
		{
			_pitch = Mathf.Clamp(_pitch, fpsPitchMin, fpsPitchMax);
		}
		else
		{
			_pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
		}
	}

	private void LateUpdate()
	{
		if (base.isLocalPlayer && _health != null && _health.IsDead)
		{
			return;
		}
		if (base.isLocalPlayer)
		{
			HandleInput();
			if (Mode == CameraMode.FirstPerson)
			{
				ApplyFirstPerson();
			}
			else
			{
				ApplyThirdPerson();
			}
			Network_syncedPitch = _pitch;
		}
		UpdateAimTarget();
	}

	private void UpdateAimTarget()
	{
		if (aimTarget == null)
		{
			return;
		}
		bool flag = _role != null && _role.Role == PlayerRole.Hunter;
		if (aimTarget.gameObject.activeSelf != flag)
		{
			aimTarget.gameObject.SetActive(flag);
		}
		if (flag)
		{
			float num = (base.isLocalPlayer ? _pitch : _syncedPitch);
			if (!_hasDisplayPitch)
			{
				_displayPitch = num;
				_hasDisplayPitch = true;
			}
			else if (base.isLocalPlayer)
			{
				_displayPitch = num;
			}
			else
			{
				_displayPitch = Mathf.Lerp(_displayPitch, num, Time.deltaTime * aimSmoothSpeed);
			}
			Vector3 vector = ((headBone != null) ? headBone.position : base.transform.position) + base.transform.rotation * fpsEyeOffset;
			Quaternion quaternion = base.transform.rotation * Quaternion.Euler(_displayPitch, 0f, 0f);
			aimTarget.position = vector + quaternion * Vector3.forward * aimTargetDistance;
		}
	}

	private void HandleInput()
	{
		if (CursorManager.Instance != null && CursorManager.Instance.AnyUIOpen)
		{
			return;
		}
		float num = ((GeneralSettingsManager.Instance != null) ? GeneralSettingsManager.Instance.MouseSensitivity : 1f);
		float num2 = Input.GetAxis("Mouse X") * mouseSensitivity * num;
		float num3 = Input.GetAxis("Mouse Y") * mouseSensitivity * num;
		if (GeneralSettingsManager.Instance != null && GeneralSettingsManager.Instance.InvertY)
		{
			num3 = 0f - num3;
		}
		if (Mode == CameraMode.ThirdPerson && Input.GetKey(freeLookKey))
		{
			if (!_freeLooking)
			{
				_freeLooking = true;
				_freeLookYaw = _yaw;
				_freeLookPitch = _pitch;
			}
			_freeLookYaw += num2;
			_freeLookPitch -= num3;
			_freeLookPitch = Mathf.Clamp(_freeLookPitch, pitchMin, pitchMax);
		}
		else
		{
			if (_freeLooking)
			{
				_freeLooking = false;
			}
			_yaw += num2;
			_pitch -= num3;
			if (Mode == CameraMode.FirstPerson)
			{
				_pitch = Mathf.Clamp(_pitch, fpsPitchMin, fpsPitchMax);
			}
			else
			{
				_pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
			}
		}
	}

	private void ApplyThirdPerson()
	{
		if (!(cameraRig == null) && !(playerCamera == null))
		{
			Vector3 vector = base.transform.position + pivotOffset;
			if (Physics.CheckSphere(vector, cameraRadius, cameraObstacleMask, QueryTriggerInteraction.Ignore))
			{
				vector = base.transform.position;
			}
			cameraRig.position = Vector3.SmoothDamp(cameraRig.position, vector, ref _rigVelocity, followSmoothing);
			float y = (_freeLooking ? _freeLookYaw : _yaw);
			float x = (_freeLooking ? _freeLookPitch : _pitch);
			if (!_freeLooking)
			{
				_freeLookYaw = Mathf.LerpAngle(_freeLookYaw, _yaw, Time.deltaTime * 10f);
			}
			cameraRig.rotation = Quaternion.Euler(x, y, 0f);
			float num = defaultDistance;
			Vector3 vector2 = cameraRig.rotation * Vector3.back;
			Vector3 position = cameraRig.position;
			if (Physics.SphereCast(position, cameraRadius, vector2, out var hitInfo, num, cameraObstacleMask, QueryTriggerInteraction.Ignore))
			{
				num = Mathf.Clamp(hitInfo.distance, minDistance, num);
			}
			Vector3 position2 = position + vector2 * num;
			int num2 = 0;
			while (num > 0.05f && num2 < 24 && Physics.CheckSphere(position2, cameraRadius, cameraObstacleMask, QueryTriggerInteraction.Ignore))
			{
				num = Mathf.Max(0.05f, num - 0.25f);
				position2 = position + vector2 * num;
				num2++;
			}
			if (_smoothedObstacleDistance < 0f)
			{
				_smoothedObstacleDistance = num;
			}
			else
			{
				float num3 = ((num < _smoothedObstacleDistance) ? zoomInSpeed : zoomOutSpeed);
				_smoothedObstacleDistance = Mathf.MoveTowards(_smoothedObstacleDistance, num, Time.deltaTime * num3);
			}
			playerCamera.transform.localPosition = new Vector3(0f, 0f, 0f - _smoothedObstacleDistance);
			playerCamera.transform.localRotation = Quaternion.identity;
		}
	}

	private void ApplyFirstPerson()
	{
		if (!(cameraRig == null) && !(playerCamera == null))
		{
			Vector3 b = (_isAiming ? aimFpsEyeOffset : fpsEyeOffset);
			_currentEyeOffset = Vector3.Lerp(_currentEyeOffset, b, Time.deltaTime * aimTransitionSpeed);
			Vector3 position = ((headBone != null) ? headBone.position : base.transform.position) + base.transform.rotation * _currentEyeOffset;
			cameraRig.position = position;
			cameraRig.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
			float shakeRotZ;
			Vector3 localPosition = ComputeShake(out shakeRotZ);
			playerCamera.transform.localPosition = localPosition;
			playerCamera.transform.localRotation = Quaternion.Euler(0f, 0f, shakeRotZ);
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (base.isLocalPlayer && hasFocus && CursorManager.Instance != null && !CursorManager.Instance.AnyUIOpen)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public override bool Weaved()
	{
		return true;
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_syncedPitch);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteFloat(_syncedPitch);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _syncedPitch, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _syncedPitch, null, reader.ReadFloat());
		}
	}
}
