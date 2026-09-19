using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleAssignmentUI : MonoBehaviour
{
	[Header("Genel")]
	public GameObject panel;

	[Tooltip("Toplam panel süresi")]
	public float totalPanelSeconds = 45f;

	public float hunterWaitSeconds = 60f;

	public TextMeshProUGUI readyPlayerCountText;

	public TextMeshProUGUI waitingText;

	[Header("Paneller")]
	[Tooltip("Hayvan paneli (rol + ses kaydı)")]
	public GameObject animalPanel;

	[Tooltip("Avcı paneli (sadece start)")]
	public GameObject hunterPanel;

	[Header("Hayvan — Sol Rol")]
	public Image roleImage;

	public AnimalSpriteEntry[] animalSprites;

	public AnimalSpriteEntry[] animalProfileSprites;

	public TextMeshProUGUI animalNameText;

	public TextMeshProUGUI timerText;

	[Header("Hayvan — Sağ Ses Kaydı")]
	public Image micAvatar;

	public Image micLevelBarFill;

	public Button recordButton;

	public TextMeshProUGUI recordButtonLabel;

	public TextMeshProUGUI recordTimerText;

	public Button listenButton;

	public Button okButton;

	public TextMeshProUGUI okButtonLabel;

	[Header("Avcı — Erken Çıkış")]
	[Tooltip("Basılırsa panel hemen kapanır, hunter hareket edebilir hale gelir (kulübe kapısı zaten kapalı — fiziksel sınır o sağlıyor). Basmazsa hunterWaitSeconds sonunda panel otomatik kapanır.")]
	public Button hunterReadyButton;

	public TextMeshProUGUI hunterReadyButtonLabel;

	[Header("Mic Seviye Renkleri")]
	public float levelGain = 30f;

	public float levelSmoothing = 12f;

	[Tooltip("Kayıt boyunca gözlemlenen TEPE seviye (MicRecorder.GetCurrentLevel ham RMS) bu değerin altındaysa 'too quiet' sayılır.")]
	public float minValidAmplitude = 0.012f;

	public Color colorLow = new Color(0.9f, 0.2f, 0.1f);

	public Color colorMid = new Color(0.95f, 0.7f, 0.1f);

	public Color colorHigh = new Color(0.4f, 0.85f, 0.2f);

	[Header("Kayıt Ayarları")]
	[Tooltip("GameManager sahnede varsa (Lobby'de ayarlanan Max Record Length SyncVar'ı — bkz. GameManager.ConfiguredMaxRecordSeconds) bu değeri EZER; sadece GameManager yoksa (ör. test sahnesi) fallback olarak kullanılır.")]
	public float maxRecordSeconds = 3f;

	[Tooltip("Bu süreden kısa kayıtlar geçersiz sayılır (yanlışlıkla anlık tıklamayla boş/çok kısa kayıt oluşmasını engeller).")]
	public float minRecordSeconds = 0.3f;

	[Header("Referanslar")]
	public MicRecorder micRecorder;

	private Coroutine _globalTimer;

	private Coroutine _recordRoutine;

	private Coroutine _hunterWaitRoutine;

	private bool _isShowing;

	private bool _closed;

	private PlayerRole _role;

	private AnimalType _animal;

	private bool _isRecording;

	private bool _hasValidRecording;

	private byte[] _pcm;

	private int _sampleRate;

	private int _channels;

	private AudioClip _previewClip;

	public AudioSource _previewSource;

	private float _barValue;

	private float _recordingPeakAmplitude;

	private float _effectiveMaxRecordSeconds;

	public static RoleAssignmentUI Instance { get; private set; }

	public static bool IsShowing
	{
		get
		{
			if (Instance != null)
			{
				return Instance._isShowing;
			}
			return false;
		}
	}

	public static bool LocalPanelClosedForRound { get; private set; }

	public static bool IsAnimalRecordingActive { get; private set; }

	private void Awake()
	{
		Instance = this;
		if (panel != null)
		{
			panel.SetActive(value: false);
		}
		if (recordButton != null)
		{
			recordButton.onClick.AddListener(OnRecordClicked);
		}
		if (listenButton != null)
		{
			listenButton.onClick.AddListener(OnListenClicked);
		}
		if (okButton != null)
		{
			okButton.onClick.AddListener(OnOKClicked);
		}
		if (hunterReadyButton != null)
		{
			hunterReadyButton.onClick.AddListener(OnOKClicked);
		}
		AttachButtonFX(recordButton);
		AttachButtonFX(listenButton);
		AttachButtonFX(okButton);
		AttachButtonFX(hunterReadyButton);
		if (micRecorder == null)
		{
			micRecorder = Object.FindObjectOfType<MicRecorder>();
		}
		_previewSource.playOnAwake = false;
		_previewSource.spatialBlend = 0f;
		if (micLevelBarFill != null)
		{
			micLevelBarFill.fillOrigin = 0;
		}
	}

	private void AttachButtonFX(Button btn)
	{
		if (!(btn == null))
		{
			UIButtonFX uIButtonFX = btn.GetComponent<UIButtonFX>();
			if (uIButtonFX == null)
			{
				uIButtonFX = btn.gameObject.AddComponent<UIButtonFX>();
			}
			MenuManager instance = MenuManager.Instance;
			if (instance != null)
			{
				uIButtonFX.Configure(instance, instance.hoverScale, instance.clickScale, instance.fxSpeed);
			}
		}
	}

	public void ShowRole(PlayerRole role, AnimalType animal)
	{
		_role = role;
		_animal = animal;
		_closed = false;
		LocalPanelClosedForRound = false;
		if (_isRecording && micRecorder != null)
		{
			micRecorder.CancelRecording();
		}
		_isRecording = false;
		_hasValidRecording = false;
		_pcm = null;
		_previewClip = null;
		if (panel != null)
		{
			panel.SetActive(value: true);
		}
		if (!_isShowing)
		{
			_isShowing = true;
			CursorManager.Instance?.PushUI();
		}
		SetLocalPlayerFrozen(frozen: true);
		if (waitingText != null)
		{
			waitingText.text = "";
		}
		bool flag = (IsAnimalRecordingActive = role == PlayerRole.Animal);
		if (_globalTimer != null)
		{
			StopCoroutine(_globalTimer);
			_globalTimer = null;
		}
		if (_hunterWaitRoutine != null)
		{
			StopCoroutine(_hunterWaitRoutine);
			_hunterWaitRoutine = null;
		}
		if (animalPanel != null)
		{
			animalPanel.SetActive(flag);
		}
		if (hunterPanel != null)
		{
			hunterPanel.SetActive(!flag);
		}
		if (flag)
		{
			SetupAnimalRole();
			SetupAnimalFlow();
		}
		else
		{
			SetupHunterFlow();
		}
	}

	private void SetupAnimalRole()
	{
		string animalName = Localization.GetAnimalName(_animal);
		if (animalNameText != null)
		{
			animalNameText.text = animalName.ToUpper();
		}
		if (roleImage != null)
		{
			roleImage.sprite = GetAnimalSprite(_animal);
		}
		if (micAvatar != null)
		{
			micAvatar.sprite = GetAnimalProfileSprite(_animal);
		}
	}

	private void SetupAnimalFlow()
	{
		if (recordButton != null)
		{
			recordButton.interactable = true;
		}
		if (recordButtonLabel != null)
		{
			recordButtonLabel.text = Localization.Get("BTN_RECORD");
		}
		if (listenButton != null)
		{
			listenButton.interactable = false;
		}
		_barValue = 0f;
		if (micLevelBarFill != null)
		{
			micLevelBarFill.fillAmount = 0f;
			micLevelBarFill.color = colorLow;
		}
		if (recordTimerText != null)
		{
			recordTimerText.text = Localization.GetFormatted("TIMER_SECONDS_SHORT", 0f);
		}
		if (okButtonLabel != null)
		{
			okButtonLabel.text = Localization.Get("BTN_LETS_GO");
		}
		if (okButton != null)
		{
			okButton.interactable = false;
		}
		if (_globalTimer != null)
		{
			StopCoroutine(_globalTimer);
		}
		_globalTimer = StartCoroutine(GlobalCountdown());
	}

	private IEnumerator GlobalCountdown()
	{
		float t = totalPanelSeconds;
		while (t > 0f)
		{
			if (timerText != null)
			{
				timerText.text = Mathf.CeilToInt(t).ToString();
			}
			t -= Time.deltaTime;
			yield return null;
		}
		Debug.Log("[RoleUI] Süre doldu, panel kapanıyor.");
		FinalizeAndClose();
	}

	private void SetupHunterFlow()
	{
		if (hunterReadyButton != null)
		{
			hunterReadyButton.interactable = true;
		}
		if (hunterReadyButtonLabel != null)
		{
			hunterReadyButtonLabel.text = Localization.Get("BTN_LETS_GO");
		}
		if (_hunterWaitRoutine != null)
		{
			StopCoroutine(_hunterWaitRoutine);
		}
		_hunterWaitRoutine = StartCoroutine(HunterWaitRoutine());
	}

	private IEnumerator HunterWaitRoutine()
	{
		yield return new WaitForSeconds(hunterWaitSeconds);
		FinalizeAndClose();
	}

	public void SetReadyCount(int ready, int total)
	{
		if (readyPlayerCountText != null)
		{
			readyPlayerCountText.text = Localization.GetFormatted("LOBBY_PLAYERS_READY", ready, total);
		}
	}

	private void OnRecordClicked()
	{
		if (!_closed)
		{
			if (_isRecording)
			{
				StopRecording();
			}
			else
			{
				StartRecording();
			}
		}
	}

	private void StartRecording()
	{
		if (micRecorder == null)
		{
			Debug.LogError("[RoleUI] MicRecorder yok!");
			return;
		}
		_effectiveMaxRecordSeconds = ((GameManager.Instance != null) ? GameManager.Instance.ConfiguredMaxRecordSeconds : maxRecordSeconds);
		micRecorder.maxSeconds = Mathf.CeilToInt(_effectiveMaxRecordSeconds);
		if (!micRecorder.StartRecording())
		{
			Debug.LogWarning("[RoleUI] Mikrofon bulunamadı!");
			return;
		}
		_isRecording = true;
		_recordingPeakAmplitude = 0f;
		if (recordButtonLabel != null)
		{
			recordButtonLabel.text = Localization.Get("BTN_STOP");
		}
		if (listenButton != null)
		{
			listenButton.interactable = false;
		}
		_recordRoutine = StartCoroutine(RecordRoutine());
	}

	private IEnumerator RecordRoutine()
	{
		float t = _effectiveMaxRecordSeconds;
		float elapsed = 0f;
		while (_isRecording && t > 0f)
		{
			t -= Time.deltaTime;
			elapsed += Time.deltaTime;
			if (recordTimerText != null)
			{
				recordTimerText.text = Localization.GetFormatted("TIMER_SECONDS_SHORT", elapsed);
			}
			UpdateLevelBar();
			yield return null;
		}
		if (_isRecording)
		{
			StopRecording();
		}
	}

	public static void CancelActiveRecordingForSettings()
	{
		if (!(Instance == null) && Instance._isRecording)
		{
			if (Instance._recordRoutine != null)
			{
				Instance.StopCoroutine(Instance._recordRoutine);
			}
			if (Instance.micRecorder != null)
			{
				Instance.micRecorder.CancelRecording();
			}
			Instance._isRecording = false;
			if (Instance.recordButtonLabel != null)
			{
				Instance.recordButtonLabel.text = Localization.Get(Instance._hasValidRecording ? "BTN_RERECORD" : "BTN_RECORD");
			}
			if (Instance.listenButton != null)
			{
				Instance.listenButton.interactable = Instance._hasValidRecording;
			}
			Instance._barValue = 0f;
			if (Instance.micLevelBarFill != null)
			{
				Instance.micLevelBarFill.fillAmount = 0f;
				Instance.micLevelBarFill.color = Instance.colorLow;
			}
		}
	}

	private void StopRecording()
	{
		if (!_isRecording)
		{
			return;
		}
		_isRecording = false;
		if (_recordRoutine != null)
		{
			StopCoroutine(_recordRoutine);
		}
		if (recordButtonLabel != null)
		{
			recordButtonLabel.text = Localization.Get("BTN_RERECORD");
		}
		_barValue = 0f;
		if (micLevelBarFill != null)
		{
			micLevelBarFill.fillAmount = 0f;
			micLevelBarFill.color = colorLow;
		}
		bool flag = micRecorder.StopRecording(out _pcm, out _sampleRate, out _channels) && GetRecordedDurationSeconds() >= minRecordSeconds;
		bool flag2 = _recordingPeakAmplitude >= minValidAmplitude;
		if (flag && flag2)
		{
			_hasValidRecording = true;
			_previewClip = MicRecorder.PcmToAudioClip(_pcm, _sampleRate, _channels, "Preview");
			if (listenButton != null)
			{
				listenButton.interactable = true;
			}
			if (recordTimerText != null && _previewClip != null)
			{
				recordTimerText.text = Localization.GetFormatted("LABEL_RECORDING_DURATION", _previewClip.length);
			}
		}
		else
		{
			_hasValidRecording = false;
			if (listenButton != null)
			{
				listenButton.interactable = false;
			}
			if (recordTimerText != null)
			{
				recordTimerText.text = Localization.Get((!flag) ? "WARNING_TOO_SHORT" : "WARNING_TOO_QUIET");
			}
		}
		if (okButton != null && !_closed)
		{
			okButton.interactable = _hasValidRecording;
		}
	}

	private float GetRecordedDurationSeconds()
	{
		if (_pcm == null || _channels <= 0 || _sampleRate <= 0)
		{
			return 0f;
		}
		return (float)(_pcm.Length / 2) / (float)_channels / (float)_sampleRate;
	}

	private void OnListenClicked()
	{
		if (_previewClip != null)
		{
			_previewSource.PlayOneShot(_previewClip);
		}
	}

	private void UpdateLevelBar()
	{
		if (!(micLevelBarFill == null) && !(micRecorder == null))
		{
			float currentLevel = micRecorder.GetCurrentLevel();
			_recordingPeakAmplitude = Mathf.Max(_recordingPeakAmplitude, currentLevel);
			float b = Mathf.Clamp01(currentLevel * levelGain);
			_barValue = Mathf.Lerp(_barValue, b, Time.deltaTime * levelSmoothing);
			micLevelBarFill.fillAmount = _barValue;
			Color color = ((!(_barValue < 0.5f)) ? Color.Lerp(colorMid, colorHigh, (_barValue - 0.5f) / 0.5f) : Color.Lerp(colorLow, colorMid, _barValue / 0.5f));
			micLevelBarFill.color = color;
		}
	}

	private void OnOKClicked()
	{
		if (!_closed && (_role != PlayerRole.Animal || _hasValidRecording))
		{
			FinalizeAndClose();
		}
	}

	public void ForceCloseHunterPanel()
	{
		if (_role == PlayerRole.Hunter)
		{
			FinalizeAndClose();
		}
	}

	private void FinalizeAndClose()
	{
		if (_closed)
		{
			return;
		}
		_closed = true;
		if (_role == PlayerRole.Animal && _hasValidRecording)
		{
			if (VoiceNetwork.Instance != null)
			{
				VoiceNetwork.Instance.UploadVoice(_animal, _pcm, _sampleRate, _channels);
				Debug.Log($"[RoleUI] {_animal} kaydı network'e yükleniyor.");
			}
			else
			{
				VoiceClipStore.Instance?.SetClipFromPcm(_animal, _pcm, _sampleRate, _channels);
				Debug.Log($"[RoleUI] {_animal} local depolandı (network yok).");
			}
		}
		CloseInternal();
	}

	private void CloseInternal()
	{
		LocalPanelClosedForRound = true;
		IsAnimalRecordingActive = false;
		if (_isRecording)
		{
			micRecorder.CancelRecording();
		}
		if (_globalTimer != null)
		{
			StopCoroutine(_globalTimer);
			_globalTimer = null;
		}
		if (_hunterWaitRoutine != null)
		{
			StopCoroutine(_hunterWaitRoutine);
			_hunterWaitRoutine = null;
		}
		_isRecording = false;
		if (okButton != null)
		{
			okButton.interactable = false;
		}
		if (recordButton != null)
		{
			recordButton.interactable = false;
		}
		if (hunterReadyButton != null)
		{
			hunterReadyButton.interactable = false;
		}
		if (waitingText != null)
		{
			waitingText.text = Localization.Get("WAITING_FOR_PLAYERS");
		}
		if (animalPanel != null)
		{
			animalPanel.SetActive(value: false);
		}
		if (hunterPanel != null)
		{
			hunterPanel.SetActive(value: false);
		}
		if (panel != null)
		{
			panel.SetActive(value: false);
		}
		if (_isShowing)
		{
			_isShowing = false;
			CursorManager.Instance?.PopUI();
		}
		SetLocalPlayerFrozen(frozen: false);
		GameManager.Instance?.CmdPlayerReady();
	}

	private void SetLocalPlayerFrozen(bool frozen)
	{
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (!(localPlayer == null))
		{
			PlayerController component = localPlayer.GetComponent<PlayerController>();
			if (component != null)
			{
				component.enabled = !frozen;
			}
			HunterShotgun component2 = localPlayer.GetComponent<HunterShotgun>();
			if (component2 != null)
			{
				component2.enabled = !frozen;
			}
			AnimalEatController component3 = localPlayer.GetComponent<AnimalEatController>();
			if (component3 != null)
			{
				component3.enabled = !frozen;
			}
		}
	}

	public Sprite GetAnimalSprite(AnimalType type)
	{
		if (animalSprites == null)
		{
			return null;
		}
		AnimalSpriteEntry[] array = animalSprites;
		foreach (AnimalSpriteEntry animalSpriteEntry in array)
		{
			if (animalSpriteEntry != null && animalSpriteEntry.type == type)
			{
				return animalSpriteEntry.sprite;
			}
		}
		return null;
	}

	public Sprite GetAnimalProfileSprite(AnimalType type)
	{
		if (animalProfileSprites == null)
		{
			return null;
		}
		AnimalSpriteEntry[] array = animalProfileSprites;
		foreach (AnimalSpriteEntry animalSpriteEntry in array)
		{
			if (animalSpriteEntry != null && animalSpriteEntry.type == type)
			{
				return animalSpriteEntry.sprite;
			}
		}
		return null;
	}
}
