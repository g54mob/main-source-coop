using System.Collections.Generic;
using Dissonance;
using Mirror;
using TMPro;
using UnityEngine;

public class SpectatorController : MonoBehaviour
{
	private static bool _isSpectating;

	private const string AliveRoomName = "Global";

	private const string DeadChatRoomName = "DeadChat";

	private RoomMembership? _deadChatMembership;

	[Header("Kamera")]
	public Camera spectatorCamera;

	public Vector3 followOffset = new Vector3(0f, 3f, -5f);

	public float followSmoothing = 8f;

	[Tooltip("Kamera bakış pivotu izlenen oyuncunun kaç birim üstünde olsun — eskiden 1.5f'ti, kafa/omuz hizasına göre çok yukarıda kalıyordu, 1.2f aşağı çekildi")]
	public float pivotHeight = 0.3f;

	[Header("Mouse Bakış")]
	public float mouseSensitivity = 3f;

	public float minPitch = -30f;

	public float maxPitch = 75f;

	[Header("Zoom")]
	public string zoomAxis = "Mouse ScrollWheel";

	public float zoomSpeed = 8f;

	public float minZoom = 2f;

	public float maxZoom = 14f;

	[Header("UI")]
	public GameObject spectatorCanvas;

	public TextMeshProUGUI spectatingPlayerNameText;

	[Tooltip("İzlenen oyuncu Animal ise sinek/buzz geri sayımını gösterir; Hunter ise ya da hedef yoksa boş kalır")]
	public TextMeshProUGUI spectatingBuzzText;

	private float _yaw;

	private float _pitch;

	private float _zoomDistance;

	[Header("Otomatik Geçiş")]
	[Tooltip("İzlenen oyuncu ölünce sıradakine geçmeden önce bekleme")]
	public float switchDelayOnDeath = 1.5f;

	private List<Transform> _targets = new List<Transform>();

	private int _index;

	private Transform _currentTarget;

	private float _deathSwitchTimer = -1f;

	private const float SelfCorrectGraceSeconds = 1.5f;

	private float _spectateStartTime;

	public static SpectatorController Instance { get; private set; }

	public static bool IsSpectating
	{
		get
		{
			return _isSpectating;
		}
		private set
		{
			_isSpectating = value;
		}
	}

	private void Awake()
	{
		Instance = this;
		IsSpectating = false;
		if (spectatorCamera != null)
		{
			spectatorCamera.gameObject.SetActive(value: false);
		}
		if (spectatorCanvas != null)
		{
			spectatorCanvas.SetActive(value: false);
		}
	}

	[ContextMenu("EnterSpectator")]
	public void ContextEnterSpectator()
	{
		EnterSpectator();
	}

	public static void EnterSpectator()
	{
		if (Instance == null)
		{
			Debug.LogWarning("[Spectator] Instance yok! Sahnede SpectatorController olmalı.");
		}
		else
		{
			Instance.BeginSpectate();
		}
	}

	public static void ExitSpectate()
	{
		if (!(Instance == null) && IsSpectating)
		{
			Instance.EndSpectate();
		}
	}

	private void EndSpectate()
	{
		IsSpectating = false;
		_currentTarget = null;
		_deathSwitchTimer = -1f;
		if (spectatorCamera != null)
		{
			spectatorCamera.gameObject.SetActive(value: false);
		}
		if (spectatorCanvas != null)
		{
			spectatorCanvas.SetActive(value: false);
		}
		NameTagManager.Instance?.SetSuppressAll(suppress: false);
		if (spectatingBuzzText != null)
		{
			spectatingBuzzText.text = "";
		}
		SetDeadChatMode(dead: false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (NetworkClient.localPlayer != null)
		{
			NetworkedCameraController component = NetworkClient.localPlayer.GetComponent<NetworkedCameraController>();
			if (component != null && component.playerCamera != null)
			{
				component.playerCamera.gameObject.SetActive(value: true);
			}
		}
	}

	private void BeginSpectate()
	{
		IsSpectating = true;
		_spectateStartTime = Time.time;
		if (NetworkClient.localPlayer != null)
		{
			NetworkedCameraController component = NetworkClient.localPlayer.GetComponent<NetworkedCameraController>();
			if (component != null && component.playerCamera != null)
			{
				component.playerCamera.gameObject.SetActive(value: false);
			}
		}
		if (spectatorCamera != null)
		{
			spectatorCamera.gameObject.SetActive(value: true);
		}
		if (spectatorCanvas != null)
		{
			spectatorCanvas.SetActive(value: true);
		}
		NameTagManager.Instance?.SetSuppressAll(suppress: true);
		SetDeadChatMode(dead: true);
		Vector3 vector = ((followOffset.sqrMagnitude > 0.0001f) ? followOffset.normalized : new Vector3(0f, 0.3f, -1f).normalized);
		_zoomDistance = ((followOffset.magnitude > 0.1f) ? followOffset.magnitude : 5f);
		_pitch = Mathf.Asin(Mathf.Clamp(vector.y, -1f, 1f)) * 57.29578f;
		_yaw = Mathf.Atan2(vector.x, 0f - vector.z) * 57.29578f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		RefreshTargets();
		_index = 0;
		_deathSwitchTimer = -1f;
		UpdateCurrentTarget();
	}

	private void SetDeadChatMode(bool dead)
	{
		VoiceBroadcastTrigger voiceBroadcastTrigger = Object.FindObjectOfType<VoiceBroadcastTrigger>();
		if (voiceBroadcastTrigger != null)
		{
			voiceBroadcastTrigger.RoomName = (dead ? "DeadChat" : "Global");
		}
		VoiceReceiptTrigger voiceReceiptTrigger = Object.FindObjectOfType<VoiceReceiptTrigger>();
		if (voiceReceiptTrigger != null)
		{
			voiceReceiptTrigger.RoomName = "Global";
		}
		DissonanceComms dissonanceComms = Object.FindObjectOfType<DissonanceComms>();
		if (dissonanceComms == null)
		{
			return;
		}
		if (dead)
		{
			if (!_deadChatMembership.HasValue)
			{
				_deadChatMembership = dissonanceComms.Rooms.Join(new RoomName("DeadChat"));
			}
		}
		else if (_deadChatMembership.HasValue)
		{
			dissonanceComms.Rooms.Leave(_deadChatMembership.Value);
			_deadChatMembership = null;
		}
	}

	private void RefreshTargets()
	{
		_targets.Clear();
		PlayerRoleData[] array = Object.FindObjectsOfType<PlayerRoleData>();
		foreach (PlayerRoleData playerRoleData in array)
		{
			Health component = playerRoleData.GetComponent<Health>();
			if ((!(component != null) || !component.IsDead) && !(playerRoleData.transform == NetworkClient.localPlayer?.transform))
			{
				_targets.Add(playerRoleData.transform);
			}
		}
	}

	private void Update()
	{
		if (!IsSpectating)
		{
			return;
		}
		if (Time.time - _spectateStartTime > 1.5f)
		{
			Health health = ((NetworkClient.localPlayer != null) ? NetworkClient.localPlayer.GetComponent<Health>() : null);
			if (health != null && !health.IsDead)
			{
				EndSpectate();
				return;
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			NextTarget(1);
		}
		if (Input.GetMouseButtonDown(1))
		{
			NextTarget(-1);
		}
		float num = Input.GetAxis("Mouse X") * mouseSensitivity;
		float num2 = Input.GetAxis("Mouse Y") * mouseSensitivity;
		_yaw += num;
		_pitch = Mathf.Clamp(_pitch - num2, minPitch, maxPitch);
		float axis = Input.GetAxis(zoomAxis);
		if (Mathf.Abs(axis) > 0.0001f)
		{
			_zoomDistance = Mathf.Clamp(_zoomDistance - axis * zoomSpeed, minZoom, maxZoom);
		}
		if ((_currentTarget == null || IsTargetDead(_currentTarget)) && _deathSwitchTimer < 0f)
		{
			_deathSwitchTimer = switchDelayOnDeath;
		}
		if (_deathSwitchTimer >= 0f)
		{
			_deathSwitchTimer -= Time.deltaTime;
			if (_deathSwitchTimer <= 0f)
			{
				_deathSwitchTimer = -1f;
				SwitchToNextAlive();
			}
		}
		UpdateSpectatingBuzzText();
		UpdateSpectatingFlyBar();
	}

	private void UpdateSpectatingBuzzText()
	{
		if (spectatingBuzzText == null)
		{
			return;
		}
		PlayerVoiceMonitor playerVoiceMonitor = null;
		if (_currentTarget != null)
		{
			PlayerRoleData component = _currentTarget.GetComponent<PlayerRoleData>();
			if (component != null && component.Role == PlayerRole.Animal)
			{
				playerVoiceMonitor = _currentTarget.GetComponent<PlayerVoiceMonitor>();
			}
		}
		if (playerVoiceMonitor == null)
		{
			if (spectatingBuzzText.text.Length > 0)
			{
				spectatingBuzzText.text = "";
			}
			return;
		}
		int buzzWarningSeconds = playerVoiceMonitor.BuzzWarningSeconds;
		string text = buzzWarningSeconds switch
		{
			-2 => "!", 
			-1 => "", 
			_ => buzzWarningSeconds.ToString(), 
		};
		if (text != spectatingBuzzText.text)
		{
			spectatingBuzzText.text = text;
		}
	}

	private void UpdateSpectatingFlyBar()
	{
		if (VoiceNotificationUI.Instance == null)
		{
			return;
		}
		PlayerVoiceMonitor playerVoiceMonitor = null;
		if (_currentTarget != null)
		{
			PlayerRoleData component = _currentTarget.GetComponent<PlayerRoleData>();
			if (component != null && component.Role == PlayerRole.Animal)
			{
				playerVoiceMonitor = _currentTarget.GetComponent<PlayerVoiceMonitor>();
			}
		}
		bool flag = playerVoiceMonitor != null;
		float fill = ((!flag) ? 0f : (playerVoiceMonitor.IsInFlyPhase ? 1f : playerVoiceMonitor.FillAmount));
		VoiceNotificationUI.Instance.SetFlyFillExternal(flag, fill);
	}

	private bool IsTargetDead(Transform t)
	{
		if (t == null)
		{
			return true;
		}
		Health component = t.GetComponent<Health>();
		if (component != null)
		{
			return component.IsDead;
		}
		return false;
	}

	private void LateUpdate()
	{
		if (IsSpectating && !(spectatorCamera == null) && !(_currentTarget == null))
		{
			Quaternion quaternion = Quaternion.Euler(_pitch, _yaw, 0f);
			Vector3 vector = _currentTarget.position + Vector3.up * pivotHeight;
			Vector3 b = vector + quaternion * (Vector3.back * _zoomDistance);
			spectatorCamera.transform.position = Vector3.Lerp(spectatorCamera.transform.position, b, Time.deltaTime * followSmoothing);
			spectatorCamera.transform.LookAt(vector);
		}
	}

	private void NextTarget(int dir)
	{
		RefreshTargets();
		if (_targets.Count == 0)
		{
			SetNoTarget();
			return;
		}
		int num = _targets.IndexOf(_currentTarget);
		if (num < 0)
		{
			num = 0;
		}
		_index = (num + dir + _targets.Count) % _targets.Count;
		_deathSwitchTimer = -1f;
		UpdateCurrentTarget();
	}

	private void SwitchToNextAlive()
	{
		RefreshTargets();
		if (_targets.Count == 0)
		{
			SetNoTarget();
			return;
		}
		_index = Mathf.Clamp(_index, 0, _targets.Count - 1);
		UpdateCurrentTarget();
	}

	private void UpdateCurrentTarget()
	{
		if (_targets.Count == 0)
		{
			SetNoTarget();
			return;
		}
		_index = Mathf.Clamp(_index, 0, _targets.Count - 1);
		_currentTarget = _targets[_index];
		UpdateNameUI(_currentTarget);
	}

	private void UpdateNameUI(Transform target)
	{
		if (spectatingPlayerNameText == null)
		{
			return;
		}
		string text = "?";
		if (target != null)
		{
			MyClient component = target.GetComponent<MyClient>();
			if (component != null && !string.IsNullOrEmpty(component.playerInfo.username))
			{
				text = component.playerInfo.username;
			}
		}
		spectatingPlayerNameText.text = text;
	}

	private void SetNoTarget()
	{
		_currentTarget = null;
		if (spectatingPlayerNameText != null)
		{
			spectatingPlayerNameText.text = "—";
		}
	}
}
