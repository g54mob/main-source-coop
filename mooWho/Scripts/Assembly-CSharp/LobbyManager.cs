using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dissonance;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
	[Header("Geri Sayım")]
	public float countdownSeconds = 5f;

	[SyncVar(hook = "OnCountdownChanged")]
	public float CountdownRemaining = -1f;

	[SyncVar]
	public bool CountdownActive;

	[Header("Countdown UI")]
	public GameObject countdownBox;

	private RectTransform countdownBoxRect;

	public TMP_Text countdownText;

	[Header("Countdown Animasyon")]
	[Tooltip("Aktifken Y pozisyonu")]
	public float activePosY = -22f;

	[Tooltip("Pasifken Y pozisyonu (gizli — ekran dışı/yukarı)")]
	public float hiddenPosY = 60f;

	public float slideSpeed = 8f;

	private float _targetPosY;

	private bool _countdownVisible;

	[Header("En Az Oyuncu Uyarısı")]
	[Tooltip("Hazırlanma alanına 2'den az oyuncuyla girilirse gösterilen, normalde kapalı duran yazı.")]
	public TMP_Text waitingText;

	public float waitingTextPopDuration = 0.18f;

	private bool _waitingTextVisible;

	private Coroutine _waitingTextRoutine;

	[Header("Hunter/Ready Alan Bildirimleri")]
	[Tooltip("Local oyuncu Hunter Volunteer alanına girince görünür, çıkınca kaybolur — waitingText ile AYNI pop in/out deseni. İSTEK: diğer bazı bildirimlerin (ör. makeSoundNotification) aksine bir SÜREYE bağlı DEĞİL, SADECE alan içinde/dışında olma durumuna bağlı — oyuncu alandan çıkmadan kendiliğinden kaybolmaz.")]
	public GameObject hunterVolunteerNotification;

	[Tooltip("Local oyuncu Ready alanına girince görünür, çıkınca kaybolur — AYNI desen.")]
	public GameObject readyNotification;

	public float zoneNotificationPopDuration = 0.18f;

	private bool _hunterVolunteerNotificationVisible;

	private Coroutine _hunterVolunteerNotificationRoutine;

	private bool _readyNotificationVisible;

	private Coroutine _readyNotificationRoutine;

	[Header("Lobi Kodu")]
	[Tooltip("Varsayılan olarak yıldızlı (LOBBY CODE: ******) gösterir, basılı tutulunca gerçek kodu açar.")]
	public TMP_Text lobbyCodeText;

	public Button showLobbyCodeButton;

	[Tooltip("Tıklayınca lobi kodunu panoya kopyalar — kod ekranda AÇILMAZ, maskeli kalır")]
	public Button copyLobbyCodeButton;

	private const string LobbyCodeMask = "******";

	private bool _lobbyCodeRevealed;

	[Header("Mikrofon Seviye Barı")]
	[Tooltip("RoleAssignmentUI.MicLevelBarFill ile AYNI seviye/kazanç/renk mantığı — lobide de local oyuncunun konuşma seviyesini gösterir.")]
	public Image micLevelBarFill;

	public float levelGain = 30f;

	public float levelSmoothing = 12f;

	public Color colorLow = new Color(0.9f, 0.2f, 0.1f);

	public Color colorMid = new Color(0.95f, 0.7f, 0.1f);

	public Color colorHigh = new Color(0.4f, 0.85f, 0.2f);

	private float _micBarValue;

	private DissonanceComms _comms;

	private VoiceBroadcastTrigger _broadcast;

	private VoicePlayerState _localVoiceState;

	public Action<float, float> _Mirror_SyncVarHookDelegate_CountdownRemaining;

	public static LobbyManager Instance { get; private set; }

	public float NetworkCountdownRemaining
	{
		get
		{
			return CountdownRemaining;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref CountdownRemaining, 1uL, _Mirror_SyncVarHookDelegate_CountdownRemaining);
		}
	}

	public bool NetworkCountdownActive
	{
		get
		{
			return CountdownActive;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref CountdownActive, 2uL, null);
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		if (countdownBoxRect == null && countdownBox != null)
		{
			countdownBoxRect = countdownBox.GetComponent<RectTransform>();
		}
		_targetPosY = hiddenPosY;
		if (countdownBoxRect != null)
		{
			Vector2 anchoredPosition = countdownBoxRect.anchoredPosition;
			anchoredPosition.y = hiddenPosY;
			countdownBoxRect.anchoredPosition = anchoredPosition;
		}
		if (countdownBox != null)
		{
			countdownBox.SetActive(value: false);
		}
		if (waitingText != null)
		{
			waitingText.gameObject.SetActive(value: false);
		}
		if (hunterVolunteerNotification != null)
		{
			hunterVolunteerNotification.SetActive(value: false);
		}
		if (readyNotification != null)
		{
			readyNotification.SetActive(value: false);
		}
		SetupLobbyCodeButton();
		RefreshLobbyCodeText();
		LoadingScreen.Instance?.Hide();
	}

	private void OnEnable()
	{
		Localization.OnLanguageChanged += RefreshLobbyCodeText;
	}

	private void OnDisable()
	{
		Localization.OnLanguageChanged -= RefreshLobbyCodeText;
	}

	private void SetupLobbyCodeButton()
	{
		if (showLobbyCodeButton != null)
		{
			PointerHoldRelay pointerHoldRelay = showLobbyCodeButton.gameObject.AddComponent<PointerHoldRelay>();
			pointerHoldRelay.OnDown = delegate
			{
				SetLobbyCodeRevealed(revealed: true);
			};
			pointerHoldRelay.OnUp = delegate
			{
				SetLobbyCodeRevealed(revealed: false);
			};
		}
		if (copyLobbyCodeButton != null)
		{
			copyLobbyCodeButton.onClick.AddListener(CopyLobbyCode);
		}
	}

	private void SetLobbyCodeRevealed(bool revealed)
	{
		_lobbyCodeRevealed = revealed;
		RefreshLobbyCodeText();
	}

	private void CopyLobbyCode()
	{
		string text = ((SteamLobby.instance != null) ? SteamLobby.instance.CurrentJoinCode : "");
		if (!string.IsNullOrEmpty(text))
		{
			GUIUtility.systemCopyBuffer = text;
		}
	}

	private void RefreshLobbyCodeText()
	{
		if (!(lobbyCodeText == null))
		{
			string text = ((SteamLobby.instance != null) ? SteamLobby.instance.CurrentJoinCode : "");
			if (string.IsNullOrEmpty(text))
			{
				text = "******";
			}
			string text2 = (_lobbyCodeRevealed ? text : "******");
			lobbyCodeText.text = Localization.GetFormatted("LOBBY_CODE_LABEL", text2);
		}
	}

	public override void OnStartServer()
	{
		NetworkCountdownActive = false;
		NetworkCountdownRemaining = -1f;
	}

	[Server]
	public void OnPlayerReadyChanged()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyManager::OnPlayerReadyChanged()' called when server was not active");
		}
		else if (CountdownActive)
		{
			if (!ShouldStartCountdown())
			{
				CancelCountdown();
			}
		}
		else if (ShouldStartCountdown())
		{
			StartCountdown();
		}
	}

	[Server]
	private bool ShouldStartCountdown()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean LobbyManager::ShouldStartCountdown()' called when server was not active");
			return default(bool);
		}
		List<MyClient> allClients = GetAllClients();
		int num = 0;
		bool flag = false;
		foreach (MyClient item in allClients)
		{
			if (item.InReadyZone)
			{
				num++;
				if (item.IsHostPlayer)
				{
					flag = true;
				}
			}
		}
		bool flag2 = num >= Mathf.CeilToInt((float)allClients.Count / 2f);
		return flag || flag2;
	}

	[Server]
	private List<MyClient> GetAllClients()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.Generic.List`1<MyClient> LobbyManager::GetAllClients()' called when server was not active");
			return null;
		}
		List<MyClient> list = new List<MyClient>();
		foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
		{
			if (value != null && value.identity != null)
			{
				MyClient component = value.identity.GetComponent<MyClient>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list;
	}

	[Server]
	private void StartCountdown()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyManager::StartCountdown()' called when server was not active");
			return;
		}
		NetworkCountdownActive = true;
		NetworkCountdownRemaining = countdownSeconds;
		Debug.Log("[Lobby] Geri sayım başladı.");
	}

	[Server]
	private void CancelCountdown()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyManager::CancelCountdown()' called when server was not active");
			return;
		}
		NetworkCountdownActive = false;
		NetworkCountdownRemaining = -1f;
		Debug.Log("[Lobby] Geri sayım iptal (koşul bozuldu).");
	}

	public void RefreshWaitingText(bool localPlayerInReadyZone)
	{
		SetWaitingTextVisible(visible: false);
	}

	private void SetWaitingTextVisible(bool visible)
	{
		if (visible == _waitingTextVisible)
		{
			return;
		}
		_waitingTextVisible = visible;
		if (!(waitingText == null))
		{
			if (_waitingTextRoutine != null)
			{
				StopCoroutine(_waitingTextRoutine);
			}
			if (visible)
			{
				waitingText.text = Localization.Get("LOBBY_MIN_PLAYERS_WARNING");
				waitingText.gameObject.SetActive(value: true);
			}
			_waitingTextRoutine = StartCoroutine(PopWaitingText(visible));
		}
	}

	private IEnumerator PopWaitingText(bool show)
	{
		CanvasGroup cg = waitingText.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = waitingText.gameObject.AddComponent<CanvasGroup>();
		}
		RectTransform rt = waitingText.GetComponent<RectTransform>();
		float fromAlpha = (show ? 0f : 1f);
		float toAlpha = (show ? 1f : 0f);
		float fromScale = (show ? 0.8f : 1f);
		float toScale = (show ? 1f : 0.8f);
		cg.alpha = fromAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * fromScale;
		}
		float t = 0f;
		while (t < waitingTextPopDuration)
		{
			t += Time.deltaTime;
			float t2 = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / waitingTextPopDuration), 3f);
			cg.alpha = Mathf.Lerp(fromAlpha, toAlpha, t2);
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.Lerp(fromScale, toScale, t2);
			}
			yield return null;
		}
		cg.alpha = toAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * toScale;
		}
		if (!show)
		{
			waitingText.gameObject.SetActive(value: false);
		}
		_waitingTextRoutine = null;
	}

	public void SetHunterVolunteerNotificationVisible(bool visible)
	{
		if (visible == _hunterVolunteerNotificationVisible)
		{
			return;
		}
		_hunterVolunteerNotificationVisible = visible;
		if (!(hunterVolunteerNotification == null))
		{
			if (_hunterVolunteerNotificationRoutine != null)
			{
				StopCoroutine(_hunterVolunteerNotificationRoutine);
			}
			if (visible)
			{
				hunterVolunteerNotification.SetActive(value: true);
			}
			_hunterVolunteerNotificationRoutine = StartCoroutine(PopHunterVolunteerNotification(visible));
		}
	}

	private IEnumerator PopHunterVolunteerNotification(bool show)
	{
		CanvasGroup cg = hunterVolunteerNotification.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = hunterVolunteerNotification.AddComponent<CanvasGroup>();
		}
		RectTransform rt = hunterVolunteerNotification.GetComponent<RectTransform>();
		float fromAlpha = (show ? 0f : 1f);
		float toAlpha = (show ? 1f : 0f);
		float fromScale = (show ? 0.8f : 1f);
		float toScale = (show ? 1f : 0.8f);
		cg.alpha = fromAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * fromScale;
		}
		float t = 0f;
		while (t < zoneNotificationPopDuration)
		{
			t += Time.deltaTime;
			float t2 = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / zoneNotificationPopDuration), 3f);
			cg.alpha = Mathf.Lerp(fromAlpha, toAlpha, t2);
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.Lerp(fromScale, toScale, t2);
			}
			yield return null;
		}
		cg.alpha = toAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * toScale;
		}
		if (!show)
		{
			hunterVolunteerNotification.SetActive(value: false);
		}
		_hunterVolunteerNotificationRoutine = null;
	}

	public void SetReadyNotificationVisible(bool visible)
	{
		if (visible == _readyNotificationVisible)
		{
			return;
		}
		_readyNotificationVisible = visible;
		if (!(readyNotification == null))
		{
			if (_readyNotificationRoutine != null)
			{
				StopCoroutine(_readyNotificationRoutine);
			}
			if (visible)
			{
				readyNotification.SetActive(value: true);
			}
			_readyNotificationRoutine = StartCoroutine(PopReadyNotification(visible));
		}
	}

	private IEnumerator PopReadyNotification(bool show)
	{
		CanvasGroup cg = readyNotification.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = readyNotification.AddComponent<CanvasGroup>();
		}
		RectTransform rt = readyNotification.GetComponent<RectTransform>();
		float fromAlpha = (show ? 0f : 1f);
		float toAlpha = (show ? 1f : 0f);
		float fromScale = (show ? 0.8f : 1f);
		float toScale = (show ? 1f : 0.8f);
		cg.alpha = fromAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * fromScale;
		}
		float t = 0f;
		while (t < zoneNotificationPopDuration)
		{
			t += Time.deltaTime;
			float t2 = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / zoneNotificationPopDuration), 3f);
			cg.alpha = Mathf.Lerp(fromAlpha, toAlpha, t2);
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.Lerp(fromScale, toScale, t2);
			}
			yield return null;
		}
		cg.alpha = toAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * toScale;
		}
		if (!show)
		{
			readyNotification.SetActive(value: false);
		}
		_readyNotificationRoutine = null;
	}

	private void Update()
	{
		UpdateCountdownUI();
		UpdateMicLevelBar();
		if (base.isServer && CountdownActive)
		{
			NetworkCountdownRemaining = CountdownRemaining - Time.deltaTime;
			if (CountdownRemaining <= 0f)
			{
				NetworkCountdownActive = false;
				NetworkCountdownRemaining = -1f;
				Debug.Log("[Lobby] Geri sayım bitti, oyun başlıyor!");
				LobbyGameSettings.Instance?.CommitToNetworkManager();
				MyNetworkManager.Singleton.StartGameScene();
			}
		}
	}

	private void UpdateCountdownUI()
	{
		bool flag = CountdownActive && CountdownRemaining > 0f;
		if (flag && !_countdownVisible)
		{
			_countdownVisible = true;
			if (countdownBox != null)
			{
				countdownBox.SetActive(value: true);
			}
			_targetPosY = activePosY;
		}
		else if (!flag && _countdownVisible)
		{
			_countdownVisible = false;
			_targetPosY = hiddenPosY;
		}
		if (flag && countdownText != null)
		{
			countdownText.text = Mathf.CeilToInt(CountdownRemaining).ToString();
		}
		if (countdownBoxRect != null)
		{
			Vector2 anchoredPosition = countdownBoxRect.anchoredPosition;
			anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, _targetPosY, Time.deltaTime * slideSpeed);
			countdownBoxRect.anchoredPosition = anchoredPosition;
			if (!_countdownVisible && Mathf.Abs(anchoredPosition.y - hiddenPosY) < 0.5f && countdownBox != null && countdownBox.activeSelf)
			{
				countdownBox.SetActive(value: false);
			}
		}
	}

	private void UpdateMicLevelBar()
	{
		if (!(micLevelBarFill == null))
		{
			float b = Mathf.Clamp01(ReadLocalMicAmplitude() * levelGain);
			_micBarValue = Mathf.Lerp(_micBarValue, b, Time.deltaTime * levelSmoothing);
			micLevelBarFill.fillAmount = _micBarValue;
			Color color = ((!(_micBarValue < 0.5f)) ? Color.Lerp(colorMid, colorHigh, (_micBarValue - 0.5f) / 0.5f) : Color.Lerp(colorLow, colorMid, _micBarValue / 0.5f));
			micLevelBarFill.color = color;
		}
	}

	private float ReadLocalMicAmplitude()
	{
		if (_comms == null)
		{
			_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		}
		if (_comms == null)
		{
			return 0f;
		}
		if (_broadcast == null)
		{
			_broadcast = UnityEngine.Object.FindObjectOfType<VoiceBroadcastTrigger>();
		}
		if (_broadcast == null || !_broadcast.IsTransmitting)
		{
			return 0f;
		}
		if (_localVoiceState == null && !string.IsNullOrEmpty(_comms.LocalPlayerName))
		{
			_localVoiceState = _comms.FindPlayer(_comms.LocalPlayerName);
		}
		if (_localVoiceState == null)
		{
			return 0f;
		}
		return _localVoiceState.Amplitude;
	}

	private void OnCountdownChanged(float _, float newVal)
	{
	}

	public LobbyManager()
	{
		_Mirror_SyncVarHookDelegate_CountdownRemaining = OnCountdownChanged;
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
			writer.WriteFloat(CountdownRemaining);
			writer.WriteBool(CountdownActive);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteFloat(CountdownRemaining);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteBool(CountdownActive);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref CountdownRemaining, _Mirror_SyncVarHookDelegate_CountdownRemaining, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref CountdownActive, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref CountdownRemaining, _Mirror_SyncVarHookDelegate_CountdownRemaining, reader.ReadFloat());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref CountdownActive, null, reader.ReadBool());
		}
	}
}
