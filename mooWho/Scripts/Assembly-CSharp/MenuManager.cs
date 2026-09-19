using System;
using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[Serializable]
	private class SettingsPanel
	{
		public Button button;

		public GameObject scrollView;
	}

	private enum Screen
	{
		None = 0,
		Main = 1,
		Play = 2,
		HowToPlay = 3,
		CreateLobby = 4,
		LobbyList = 5,
		Settings = 6
	}

	[Header("Intro — Canvas Groups")]
	public CanvasGroup FadeOutGroup;

	public CanvasGroup MicrophoneGroup;

	[Header("Intro Timings")]
	public float microphoneFadeInTime = 0.5f;

	public float microphoneVisibleTime = 1f;

	public float microphoneFadeOutTime = 0.5f;

	public float screenFadeOutTime = 1f;

	[Header("Ekranlar (root GameObject)")]
	public GameObject mainMenuScreen;

	public GameObject playScreen;

	public GameObject howToPlayScreen;

	public GameObject createLobbyScreen;

	public GameObject lobbyListScreen;

	public GameObject settingsScreen;

	[Header("Ekran Geçiş Animasyonu")]
	[Tooltip("Geçiş süresi (saniye)")]
	public float screenTransitionTime = 0.25f;

	[Tooltip("Geçişte yukarı/aşağı kayma miktarı (piksel)")]
	public float screenSlideOffset = 40f;

	[Header("Main Menu Butonları")]
	public Button playButton;

	public Button howToPlayButton;

	public Button settingsButton;

	public Button quitButton;

	[Header("Play Screen Butonları")]
	public Button hostLobbyButton;

	public Button quickMatchButton;

	public Button lobbyListButton;

	public Button findButton;

	public Button playBackButton;

	[Header("How To Play Butonları")]
	public Button howToBackButton;

	public Button howToContinueButton;

	[Header("Create Lobby Butonları")]
	public Button publicButton;

	public Button inviteOnlyButton;

	public Button plusButton;

	public Button minusButton;

	public Button createConfirmButton;

	public Button lobbyBackButton;

	[Header("Settings Butonları (varsa)")]
	public Button settingsBackButton;

	[Header("Settings — Panel Sekmeleri (Audio, ileride General vs.)")]
	[Tooltip("Her sekme butonu SADECE kendi panelini açar ve diğer tüm panelleri kapatır (toggle değil, radio-button gibi)")]
	[SerializeField]
	private SettingsPanel[] settingsPanels;

	[Header("Settings — Genel Ayarlar")]
	[SerializeField]
	private Slider mouseSensitivitySlider;

	[SerializeField]
	private TextMeshProUGUI mouseSensitivityValueText;

	[SerializeField]
	private Toggle invertYToggle;

	[SerializeField]
	private TMP_Dropdown resolutionDropdown;

	[SerializeField]
	private Toggle fullscreenToggle;

	[SerializeField]
	private TMP_Dropdown qualityDropdown;

	[SerializeField]
	private Toggle vsyncToggle;

	[Header("Settings — Ses Ayarları")]
	[SerializeField]
	private Slider masterVolumeSlider;

	[SerializeField]
	private TextMeshProUGUI masterVolumeValueText;

	[SerializeField]
	private Slider musicVolumeSlider;

	[SerializeField]
	private TextMeshProUGUI musicVolumeValueText;

	[SerializeField]
	private Slider mimicVolumeSlider;

	[SerializeField]
	private TextMeshProUGUI mimicVolumeValueText;

	[SerializeField]
	private TMP_Dropdown voiceChatModeDropdown;

	[Tooltip("Push-to-talk tuşunu yeniden bağlama butonu — SADECE Voice Mode 'Push To Talk' iken görünür (Voice Activation'da SetActive(false))")]
	[SerializeField]
	private Button pushToTalkButton;

	[Tooltip("PushToTalkButton'ın içindeki metin — normalde '[V]' gibi güncel tuşu, tuş kaydı beklenirken '[ ]' gösterir")]
	[SerializeField]
	private TextMeshProUGUI pushToTalkText;

	[SerializeField]
	private TMP_Dropdown microphoneDropdown;

	[Tooltip("AÇIK ve local oyuncu Animal rolündeyken hayvan NPC'lerin (taklit sesi çalan bot'ların) sesini susturur — Hunter rolündeyken tamamen etkisizdir, diğer OYUNCULARIN sesi (Dissonance) bundan hiç etkilenmez.")]
	[SerializeField]
	private Toggle muteNpcAnimalsToggle;

	[SerializeField]
	private Button resetSettingsButton;

	[SerializeField]
	private Button applySettingsButton;

	[Header("Settings — Dil")]
	[SerializeField]
	private TMP_Dropdown languageDropdown;

	[Header("Ana Menü — Hızlı Dil Seçici")]
	[Tooltip("Settings ekranındaki dil dropdown'undan AYRI, ana menüde duran ikinci bir dil seçici (ikisi de senkron kalır)")]
	[SerializeField]
	private TMP_Dropdown mainMenuLanguageDropdown;

	private static readonly CommActivationMode[] VoiceModeOrder = new CommActivationMode[2]
	{
		CommActivationMode.PushToTalk,
		CommActivationMode.VoiceActivation
	};

	private readonly List<string> _microphoneOptions = new List<string> { "" };

	[Header("Sahne Farkındalığı — menü dışında (lobi/oyun) sadece Settings erişilebilir")]
	[Tooltip("Menü sahnesinin adı — bu sahnede değilsek Main/Play/HowToPlay/CreateLobby/LobbyList kapalı kalır")]
	[SerializeField]
	private string menuSceneName = "Menu";

	[Tooltip("Sadece menü dışındayken aktif olan, lobiden ayrılma butonu")]
	[SerializeField]
	private Button leaveLobbyButton;

	[Tooltip("Settings > Game sekmesini açan buton — Ana Menü'de kapalı, Lobby/Game sahnesinde açık. leaveLobbyButton ile AYNI mantık/koşul.")]
	[SerializeField]
	private Button GameButton;

	[Header("Host Ayrıldı Mesajı")]
	[Tooltip("HostLeaveMessageText'in CanvasGroup'u — normalde kapalı (m_IsActive: 0), SADECE host oyundan ayrıldığı için menüye dönüldüğünde (bkz. SteamLobby.OnHostLeftLobby) anlık açılıp bir süre sonra smooth fade ile kapanır. Normal 'Leave Lobby' ile menüye dönüşte HİÇ tetiklenmez.")]
	[SerializeField]
	private CanvasGroup hostLeaveMessageGroup;

	[Tooltip("Mesajın tam opak görünür kalacağı süre (sn), fade-out başlamadan önce")]
	public float hostLeaveMessageVisibleTime = 3f;

	[Tooltip("Fade-out süresi (sn)")]
	public float hostLeaveMessageFadeOutTime = 1f;

	[Header("Lobiye Katılma Başarısız Mesajı")]
	[Tooltip("FailedToJoinMessageText'in CanvasGroup'u — normalde kapalı (m_IsActive: 0), kullanıcının KENDİ başlattığı bir katılma denemesi gerçekten başarısız olunca (bkz. SteamLobby.OnJoinFailed — kilitli/kapanmış lobi ya da Mirror bağlantı zaman aşımı) anlık açılıp bir süre sonra smooth fade ile kapanır. Quick Match'in kendi arka plan adaylarında HİÇ tetiklenmez (bkz. _awaitingQuickMatchJoinResult).")]
	[SerializeField]
	private CanvasGroup failedToJoinMessageGroup;

	[Tooltip("Mesajın tam opak görünür kalacağı süre (sn), fade-out başlamadan önce")]
	public float failedToJoinMessageVisibleTime = 3f;

	[Tooltip("Fade-out süresi (sn)")]
	public float failedToJoinMessageFadeOutTime = 1f;

	[Header("Lobiden Ayrıl — Onay Popup'ı")]
	[Tooltip("leaveLobbyButton'a basınca DİREKT ayrılmaz, önce bu popup açılır")]
	[SerializeField]
	private GameObject leaveConfirmPopup;

	[Tooltip("Popup'ı kapatır, AYRILMAZ")]
	[SerializeField]
	private Button leavePopupBackButton;

	[Tooltip("Popup'ı kapatır VE gerçekten lobiden ayrılır")]
	[SerializeField]
	private Button leavePopupLeaveButton;

	[Header("Versiyon Uyuşmazlığı — Popup")]
	[Tooltip("Katılınan lobinin sahibi farklı bir oyun versiyonundaysa (bkz. SteamLobby.OnLobbyEntered) açılır — içinde sadece OK butonu var, kapatmaktan başka bir şey yapmaz.")]
	[SerializeField]
	private GameObject versionMismatchPopup;

	[SerializeField]
	private Button versionMismatchOkButton;

	private bool _inMenuScene = true;

	[Header("Avatar (ESC > Game ayarları — PlayerItemGamePanel)")]
	[Tooltip("Rol Hunter ise kullanılan avatar.")]
	public Sprite hunterSprite;

	[Tooltip("Rol Animal ise, hayvan tipine göre kullanılan avatar listesi.")]
	public AnimalSpriteEntry[] animalProfileSprites;

	[Header("Create Lobby — Player Count")]
	public TextMeshProUGUI playerCountText;

	public int minPlayers = 2;

	public int maxPlayers = 8;

	public int defaultPlayerCount = 8;

	[Header("Create Lobby — Room Type Görsel")]
	public Image publicButtonImage;

	public TextMeshProUGUI publicButtonText;

	public Image inviteOnlyButtonImage;

	public TextMeshProUGUI inviteOnlyButtonText;

	[Header("Room Type — Renkler")]
	public Color selectedButtonColor = new Color(0.95f, 0.55f, 0.1f);

	public Color unselectedButtonColor = new Color(0.75f, 0.55f, 0.35f);

	public Color selectedTextColor = Color.white;

	public Color unselectedTextColor = new Color(0.55f, 0.4f, 0.25f);

	[Header("Create Lobby — Harita Seçimi")]
	public Button MapFarmButton;

	public Image MapFarmButtonImage;

	public TextMeshProUGUI MapFarmButtonText;

	public Button MapForestButton;

	public Image MapForestButtonImage;

	public TextMeshProUGUI MapForestButtonText;

	[Header("Harita Butonu — Renkler")]
	[Tooltip("Seçili haritanın resmi/ismi bu renkte (normal, tam ton)")]
	public Color mapSelectedColor = Color.white;

	[Tooltip("Seçili OLMAYAN haritanın hem resmi hem isim yazısı bu gri tona boyanır")]
	public Color mapUnselectedColor = new Color(0.5f, 0.5f, 0.5f);

	[Header("Matchmaking UI")]
	public GameObject matchmakingBox;

	public TextMeshProUGUI matchmakingCountdownText;

	public Button cancelMatchmakingButton;

	[Header("UI Sesleri")]
	public AudioSource uiAudioSource;

	public AudioClip clickSound;

	public AudioClip hoverSound;

	[Range(0f, 1f)]
	public float hoverVolume = 0.2f;

	[Range(0f, 1f)]
	public float clickVolume = 0.35f;

	[Header("Buton Animasyonu")]
	[Tooltip("Tüm butonlara otomatik hover/click animasyonu + ses ekle")]
	public bool autoAddButtonFX = true;

	public float hoverScale = 1.06f;

	public float clickScale = 0.94f;

	public float fxSpeed = 12f;

	private int _playerCount;

	private bool _isPublic = true;

	private MapType _selectedMap;

	private Coroutine _transition;

	private bool _pendingHostLeaveMessage;

	private Coroutine _hostLeaveMessageRoutine;

	private Coroutine _failedToJoinMessageRoutine;

	private float _matchmakingElapsed;

	private bool _matchmakingActive;

	private Screen _current;

	private bool _wasRebindingPTT;

	public static MenuManager Instance { get; private set; }

	public static bool IsSettingsOpen
	{
		get
		{
			if (Instance != null)
			{
				return Instance._current == Screen.Settings;
			}
			return false;
		}
	}

	public Sprite GetAvatarSprite(PlayerRoleData roleData)
	{
		if (roleData == null)
		{
			return null;
		}
		if (roleData.Role == PlayerRole.Hunter)
		{
			return hunterSprite;
		}
		AnimalType animalType = (roleData.RolesLocked ? roleData.AssignedAnimal : roleData.LobbyPreviewAnimal);
		if (animalProfileSprites == null)
		{
			return null;
		}
		AnimalSpriteEntry[] array = animalProfileSprites;
		foreach (AnimalSpriteEntry animalSpriteEntry in array)
		{
			if (animalSpriteEntry != null && animalSpriteEntry.type == animalType)
			{
				return animalSpriteEntry.sprite;
			}
		}
		return null;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (FadeOutGroup != null)
		{
			FadeOutGroup.alpha = 1f;
		}
		if (MicrophoneGroup != null)
		{
			MicrophoneGroup.alpha = 0f;
		}
		_playerCount = Mathf.Clamp(defaultPlayerCount, minPlayers, maxPlayers);
		if (uiAudioSource == null)
		{
			uiAudioSource = base.gameObject.AddComponent<AudioSource>();
			uiAudioSource.playOnAwake = false;
			uiAudioSource.spatialBlend = 0f;
		}
		BindButtons();
		SetupButtonFX();
		SetupSettingsPanels();
		if (cancelMatchmakingButton != null)
		{
			cancelMatchmakingButton.onClick.AddListener(OnCancelMatchmaking);
		}
		if (matchmakingBox != null)
		{
			matchmakingBox.SetActive(value: false);
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
		Localization.OnLanguageChanged += OnLanguageChanged;
		SteamLobby.OnHostLeftLobby += OnHostLeftLobbyReceived;
		SteamLobby.OnJoinFailed += OnJoinFailedReceived;
		if (hostLeaveMessageGroup != null)
		{
			hostLeaveMessageGroup.gameObject.SetActive(value: false);
		}
		if (failedToJoinMessageGroup != null)
		{
			failedToJoinMessageGroup.gameObject.SetActive(value: false);
		}
	}

	private void Start()
	{
		HideAllScreensImmediate();
		SetupGeneralSettings();
		SetupAudioSettings();
		SetupLanguageDropdown();
		if (SteamLobby.instance != null)
		{
			SteamLobby instance = SteamLobby.instance;
			instance.OnMatchmakingStarted = (Action)Delegate.Combine(instance.OnMatchmakingStarted, new Action(ShowMatchmakingBox));
			SteamLobby instance2 = SteamLobby.instance;
			instance2.OnMatchmakingEnded = (Action)Delegate.Combine(instance2.OnMatchmakingEnded, new Action(HideMatchmakingBox));
		}
		StartCoroutine(IntroSequence());
		Application.runInBackground = true;
	}

	private void OnDestroy()
	{
		if (SteamLobby.instance != null)
		{
			SteamLobby instance = SteamLobby.instance;
			instance.OnMatchmakingStarted = (Action)Delegate.Remove(instance.OnMatchmakingStarted, new Action(ShowMatchmakingBox));
			SteamLobby instance2 = SteamLobby.instance;
			instance2.OnMatchmakingEnded = (Action)Delegate.Remove(instance2.OnMatchmakingEnded, new Action(HideMatchmakingBox));
		}
		SceneManager.sceneLoaded -= OnSceneLoaded;
		Localization.OnLanguageChanged -= OnLanguageChanged;
		SteamLobby.OnHostLeftLobby -= OnHostLeftLobbyReceived;
		SteamLobby.OnJoinFailed -= OnJoinFailedReceived;
	}

	private void OnLanguageChanged()
	{
		PopulateVoiceModeDropdownOptions();
		PopulateLanguageDropdownOptions();
		PopulateQualityDropdownOptions();
		RefreshLobbyUI();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		_inMenuScene = scene.name == menuSceneName;
		if (leaveLobbyButton != null)
		{
			leaveLobbyButton.gameObject.SetActive(!_inMenuScene);
		}
		if (GameButton != null)
		{
			GameButton.gameObject.SetActive(!_inMenuScene);
		}
		if (_current == Screen.Settings)
		{
			CursorManager.Instance?.PopUI();
		}
		HideAllScreensImmediate();
		_current = Screen.None;
		if (!_inMenuScene)
		{
			return;
		}
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		LoadingScreen.Instance?.Hide();
		ShowMainMenu();
		if (_pendingHostLeaveMessage)
		{
			_pendingHostLeaveMessage = false;
			if (_hostLeaveMessageRoutine != null)
			{
				StopCoroutine(_hostLeaveMessageRoutine);
			}
			_hostLeaveMessageRoutine = StartCoroutine(ShowHostLeaveMessageRoutine());
		}
	}

	private void OnHostLeftLobbyReceived()
	{
		_pendingHostLeaveMessage = true;
	}

	private IEnumerator ShowHostLeaveMessageRoutine()
	{
		if (!(hostLeaveMessageGroup == null))
		{
			hostLeaveMessageGroup.gameObject.SetActive(value: true);
			hostLeaveMessageGroup.alpha = 1f;
			yield return new WaitForSeconds(hostLeaveMessageVisibleTime);
			yield return FadeCanvasGroup(hostLeaveMessageGroup, 1f, 0f, hostLeaveMessageFadeOutTime);
			hostLeaveMessageGroup.gameObject.SetActive(value: false);
			_hostLeaveMessageRoutine = null;
		}
	}

	private void OnJoinFailedReceived()
	{
		if (_failedToJoinMessageRoutine != null)
		{
			StopCoroutine(_failedToJoinMessageRoutine);
		}
		_failedToJoinMessageRoutine = StartCoroutine(ShowFailedToJoinMessageRoutine());
	}

	private IEnumerator ShowFailedToJoinMessageRoutine()
	{
		if (!(failedToJoinMessageGroup == null))
		{
			failedToJoinMessageGroup.gameObject.SetActive(value: true);
			failedToJoinMessageGroup.alpha = 1f;
			yield return new WaitForSeconds(failedToJoinMessageVisibleTime);
			yield return FadeCanvasGroup(failedToJoinMessageGroup, 1f, 0f, failedToJoinMessageFadeOutTime);
			failedToJoinMessageGroup.gameObject.SetActive(value: false);
			_failedToJoinMessageRoutine = null;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (leaveConfirmPopup != null && leaveConfirmPopup.activeSelf)
			{
				SetLeaveConfirmPopupVisible(visible: false);
			}
			else
			{
				ToggleSettingsViaEscape();
			}
		}
		bool flag = AudioSettingsManager.Instance != null && AudioSettingsManager.Instance.IsRebindingPushToTalkKey;
		if (flag != _wasRebindingPTT)
		{
			_wasRebindingPTT = flag;
			RefreshPushToTalkRebindLabel();
		}
		if (SteamLobby.instance != null && SteamLobby.instance.IsMatchmaking != _matchmakingActive)
		{
			if (SteamLobby.instance.IsMatchmaking)
			{
				ShowMatchmakingBox();
			}
			else
			{
				HideMatchmakingBox();
			}
		}
		if (_matchmakingActive)
		{
			_matchmakingElapsed += Time.deltaTime;
			if (matchmakingCountdownText != null)
			{
				int num = Mathf.FloorToInt(_matchmakingElapsed);
				int num2 = num / 60;
				int num3 = num % 60;
				matchmakingCountdownText.text = $"{num2:00}:{num3:00}";
			}
		}
	}

	public void PlayClickSound()
	{
		if (clickSound != null && uiAudioSource != null)
		{
			uiAudioSource.PlayOneShot(clickSound, clickVolume);
		}
	}

	public void PlayHoverSound()
	{
		if (hoverSound != null && uiAudioSource != null)
		{
			uiAudioSource.PlayOneShot(hoverSound, hoverVolume);
		}
	}

	private void BindButtons()
	{
		Bind(playButton, OnPlayClicked);
		Bind(howToPlayButton, OnHowToPlayClicked);
		Bind(settingsButton, OnSettingsClicked);
		Bind(quitButton, OnQuitClicked);
		Bind(hostLobbyButton, OnHostLobbyClicked);
		Bind(quickMatchButton, OnQuickMatchClicked);
		Bind(lobbyListButton, OnLobbyListClicked);
		Bind(findButton, OnFindClicked);
		Bind(playBackButton, OnBackToMainFromPlay);
		Bind(howToBackButton, OnBackToMainFromHowTo);
		Bind(howToContinueButton, OnBackToMainFromHowTo);
		Bind(publicButton, OnSelectPublic);
		Bind(inviteOnlyButton, OnSelectInviteOnly);
		Bind(MapFarmButton, OnSelectMapFarm);
		Bind(MapForestButton, OnSelectMapForest);
		Bind(plusButton, OnPlusPlayer);
		Bind(minusButton, OnMinusPlayer);
		Bind(createConfirmButton, OnCreateConfirmClicked);
		Bind(lobbyBackButton, OnBackToPlayFromLobby);
		Bind(settingsBackButton, OnBackToMainFromSettings);
		Bind(resetSettingsButton, OnResetSettingsClicked);
		Bind(applySettingsButton, OnApplySettingsClicked);
		Bind(leaveLobbyButton, OnLeaveLobbyClicked);
		Bind(leavePopupBackButton, OnLeaveConfirmBackClicked);
		Bind(leavePopupLeaveButton, OnLeaveConfirmLeaveClicked);
		Bind(versionMismatchOkButton, OnVersionMismatchOkClicked);
	}

	private void Bind(Button btn, UnityAction action)
	{
		if (!(btn == null))
		{
			btn.onClick.RemoveListener(action);
			btn.onClick.AddListener(action);
		}
	}

	private void SetupButtonFX()
	{
		if (!autoAddButtonFX)
		{
			return;
		}
		Button[] componentsInChildren = GetComponentsInChildren<Button>(includeInactive: true);
		foreach (Button button in componentsInChildren)
		{
			UIButtonFX uIButtonFX = button.GetComponent<UIButtonFX>();
			if (uIButtonFX == null)
			{
				uIButtonFX = button.gameObject.AddComponent<UIButtonFX>();
			}
			uIButtonFX.Configure(this, hoverScale, clickScale, fxSpeed);
		}
	}

	private IEnumerator IntroSequence()
	{
		if (MicrophoneGroup != null)
		{
			yield return FadeCanvasGroup(MicrophoneGroup, 0f, 1f, microphoneFadeInTime);
			yield return new WaitForSeconds(microphoneVisibleTime);
			yield return FadeCanvasGroup(MicrophoneGroup, 1f, 0f, microphoneFadeOutTime);
		}
		if (FadeOutGroup != null)
		{
			yield return FadeCanvasGroup(FadeOutGroup, 1f, 0f, screenFadeOutTime);
		}
		ShowMainMenu();
	}

	private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
	{
		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			group.alpha = Mathf.Lerp(from, to, timer / duration);
			yield return null;
		}
		group.alpha = to;
	}

	private void HideAllScreensImmediate()
	{
		if (mainMenuScreen != null)
		{
			mainMenuScreen.SetActive(value: false);
		}
		if (playScreen != null)
		{
			playScreen.SetActive(value: false);
		}
		if (howToPlayScreen != null)
		{
			howToPlayScreen.SetActive(value: false);
		}
		if (createLobbyScreen != null)
		{
			createLobbyScreen.SetActive(value: false);
		}
		if (lobbyListScreen != null)
		{
			lobbyListScreen.SetActive(value: false);
		}
		if (settingsScreen != null)
		{
			settingsScreen.SetActive(value: false);
		}
		if (mainMenuLanguageDropdown != null)
		{
			mainMenuLanguageDropdown.gameObject.SetActive(value: false);
		}
		SetLeaveConfirmPopupVisible(visible: false);
	}

	private GameObject GetScreenObject(Screen s)
	{
		return s switch
		{
			Screen.Main => mainMenuScreen, 
			Screen.Play => playScreen, 
			Screen.HowToPlay => howToPlayScreen, 
			Screen.CreateLobby => createLobbyScreen, 
			Screen.LobbyList => lobbyListScreen, 
			Screen.Settings => settingsScreen, 
			_ => null, 
		};
	}

	private void SwitchTo(Screen screen)
	{
		if ((_inMenuScene || screen == Screen.Settings || screen == Screen.None) && (screen != Screen.Settings || _current == Screen.Settings || CanOpenSettingsNow()))
		{
			if (SteamLobby.instance != null && SteamLobby.instance.IsMatchmaking)
			{
				SteamLobby.instance.CancelMatchmaking();
			}
			bool flag = _current == Screen.Settings;
			bool flag2 = screen == Screen.Settings;
			if (flag2 && !flag)
			{
				CursorManager.Instance?.PushUI();
				RoleAssignmentUI.CancelActiveRecordingForSettings();
			}
			else if (flag && !flag2)
			{
				CursorManager.Instance?.PopUI();
			}
			if (flag && !flag2)
			{
				AudioSettingsManager.Instance?.CancelRebindPushToTalkKey();
				SetLeaveConfirmPopupVisible(visible: false);
			}
			if (_transition != null)
			{
				StopCoroutine(_transition);
			}
			_transition = StartCoroutine(TransitionRoutine(screen));
		}
	}

	private bool CanOpenSettingsNow()
	{
		if ((!RoleAssignmentUI.IsShowing || RoleAssignmentUI.IsAnimalRecordingActive) && !AdminPanelUI.IsOpen)
		{
			return !LobbyGameSettingsUI.IsShowing;
		}
		return false;
	}

	private void ToggleSettingsViaEscape()
	{
		if (_current == Screen.Settings)
		{
			SwitchTo(_inMenuScene ? Screen.Main : Screen.None);
		}
		else
		{
			SwitchTo(Screen.Settings);
		}
	}

	private IEnumerator TransitionRoutine(Screen target)
	{
		GameObject screenObject = GetScreenObject(_current);
		GameObject toObj = GetScreenObject(target);
		if (screenObject != null && screenObject.activeSelf)
		{
			yield return AnimateScreen(screenObject, exiting: true);
		}
		HideAllScreensImmediate();
		_current = target;
		if (target == Screen.CreateLobby)
		{
			RefreshLobbyUI();
		}
		if (mainMenuLanguageDropdown != null)
		{
			mainMenuLanguageDropdown.gameObject.SetActive(target == Screen.Main);
		}
		if (toObj != null)
		{
			toObj.SetActive(value: true);
			yield return AnimateScreen(toObj, exiting: false);
		}
		_transition = null;
	}

	private IEnumerator AnimateScreen(GameObject screenObj, bool exiting)
	{
		CanvasGroup cg = screenObj.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = screenObj.AddComponent<CanvasGroup>();
		}
		RectTransform rt = screenObj.transform as RectTransform;
		Vector2 basePos = ((rt != null) ? rt.anchoredPosition : Vector2.zero);
		float from = (exiting ? 1f : 0f);
		float to = (exiting ? 0f : 1f);
		float slideFrom = (exiting ? 0f : screenSlideOffset);
		float slideTo = (exiting ? (0f - screenSlideOffset) : 0f);
		float t = 0f;
		float dur = Mathf.Max(0.01f, screenTransitionTime);
		while (t < dur)
		{
			t += Time.deltaTime;
			float t2 = Mathf.SmoothStep(0f, 1f, t / dur);
			cg.alpha = Mathf.Lerp(from, to, t2);
			if (rt != null)
			{
				rt.anchoredPosition = basePos + new Vector2(0f, Mathf.Lerp(slideFrom, slideTo, t2));
			}
			yield return null;
		}
		cg.alpha = to;
		if (rt != null)
		{
			rt.anchoredPosition = basePos;
		}
	}

	public void ShowMainMenu()
	{
		SwitchTo(Screen.Main);
	}

	private void OnPlayClicked()
	{
		SwitchTo(Screen.Play);
	}

	private void OnHowToPlayClicked()
	{
		SwitchTo(Screen.HowToPlay);
	}

	private void OnSettingsClicked()
	{
		SwitchTo(Screen.Settings);
	}

	private void OnQuitClicked()
	{
		Application.Quit();
	}

	private void OnBackToMainFromPlay()
	{
		SwitchTo(Screen.Main);
	}

	private void OnBackToMainFromHowTo()
	{
		SwitchTo(Screen.Main);
	}

	private void OnBackToPlayFromLobby()
	{
		SwitchTo(Screen.Play);
	}

	private void OnBackToMainFromSettings()
	{
		SwitchTo(_inMenuScene ? Screen.Main : Screen.None);
	}

	private void OnHostLobbyClicked()
	{
		SwitchTo(Screen.CreateLobby);
	}

	private void OnQuickMatchClicked()
	{
		if (SteamLobby.instance == null)
		{
			Debug.LogWarning("[Menu] SteamLobby yok!");
			return;
		}
		Debug.Log("[Menu] Quick Match — müsait lobby aranıyor...");
		SteamLobby.instance.FindMatch();
	}

	private void OnLobbyListClicked()
	{
		SwitchTo(Screen.LobbyList);
		SteamLobby.instance?.RefreshLobbyList();
	}

	private void OnFindClicked()
	{
		Debug.Log("[Menu] Find.");
	}

	public void ShowPlayScreenPublic()
	{
		SwitchTo(Screen.Play);
	}

	public void ShowCreateLobbyPublic()
	{
		SwitchTo(Screen.CreateLobby);
	}

	public void ShowLobbyListPublic()
	{
		SwitchTo(Screen.LobbyList);
	}

	private void OnLeaveLobbyClicked()
	{
		SetLeaveConfirmPopupVisible(visible: true);
	}

	private void OnLeaveConfirmBackClicked()
	{
		SetLeaveConfirmPopupVisible(visible: false);
	}

	private void OnLeaveConfirmLeaveClicked()
	{
		SetLeaveConfirmPopupVisible(visible: false);
		SteamLobby.instance?.LeaveLobby();
	}

	private void SetLeaveConfirmPopupVisible(bool visible)
	{
		if (leaveConfirmPopup != null)
		{
			leaveConfirmPopup.SetActive(visible);
		}
	}

	public static void ShowVersionMismatchPopup()
	{
		if (Instance != null)
		{
			Instance.SetVersionMismatchPopupVisible(visible: true);
		}
	}

	private void OnVersionMismatchOkClicked()
	{
		SetVersionMismatchPopupVisible(visible: false);
	}

	private void SetVersionMismatchPopupVisible(bool visible)
	{
		if (versionMismatchPopup != null)
		{
			versionMismatchPopup.SetActive(visible);
		}
	}

	private void ShowMatchmakingBox()
	{
		_matchmakingActive = true;
		_matchmakingElapsed = 0f;
		if (matchmakingBox != null)
		{
			matchmakingBox.SetActive(value: true);
		}
		if (matchmakingCountdownText != null)
		{
			matchmakingCountdownText.text = "00:00";
		}
	}

	private void HideMatchmakingBox()
	{
		_matchmakingActive = false;
		if (matchmakingBox != null)
		{
			matchmakingBox.SetActive(value: false);
		}
	}

	private void OnCancelMatchmaking()
	{
		if (SteamLobby.instance != null)
		{
			SteamLobby.instance.CancelMatchmaking();
		}
		else
		{
			HideMatchmakingBox();
		}
	}

	private void OnPlusPlayer()
	{
		_playerCount = Mathf.Min(maxPlayers, _playerCount + 1);
		RefreshLobbyUI();
	}

	private void OnMinusPlayer()
	{
		_playerCount = Mathf.Max(minPlayers, _playerCount - 1);
		RefreshLobbyUI();
	}

	private void OnSelectPublic()
	{
		_isPublic = true;
		UpdateRoomTypeVisual();
	}

	private void OnSelectInviteOnly()
	{
		_isPublic = false;
		UpdateRoomTypeVisual();
	}

	private void OnSelectMapFarm()
	{
		_selectedMap = MapType.Farm;
		UpdateMapVisual();
	}

	private void OnSelectMapForest()
	{
		_selectedMap = MapType.Forest;
		UpdateMapVisual();
	}

	private void UpdateMapVisual()
	{
		if (MapFarmButtonText != null)
		{
			MapFarmButtonText.text = Localization.Get("MAP_FARM");
		}
		if (MapForestButtonText != null)
		{
			MapForestButtonText.text = Localization.Get("MAP_FOREST");
		}
		ApplyMapButtonStyle(MapFarmButtonImage, MapFarmButtonText, _selectedMap == MapType.Farm);
		ApplyMapButtonStyle(MapForestButtonImage, MapForestButtonText, _selectedMap == MapType.Forest);
	}

	private void ApplyMapButtonStyle(Image img, TextMeshProUGUI txt, bool selected)
	{
		Color color = (selected ? mapSelectedColor : mapUnselectedColor);
		if (img != null)
		{
			img.color = color;
		}
		if (txt != null)
		{
			txt.color = color;
		}
	}

	private void RefreshLobbyUI()
	{
		if (playerCountText != null)
		{
			playerCountText.text = Localization.GetPlural("PLAYER_COUNT_FORMAT", _playerCount, _playerCount);
		}
		if (plusButton != null)
		{
			plusButton.interactable = _playerCount < maxPlayers;
		}
		if (minusButton != null)
		{
			minusButton.interactable = _playerCount > minPlayers;
		}
		UpdateRoomTypeVisual();
		UpdateMapVisual();
	}

	private void UpdateRoomTypeVisual()
	{
		ApplyRoomTypeStyle(publicButtonImage, publicButtonText, _isPublic);
		ApplyRoomTypeStyle(inviteOnlyButtonImage, inviteOnlyButtonText, !_isPublic);
	}

	private void ApplyRoomTypeStyle(Image img, TextMeshProUGUI txt, bool selected)
	{
		if (img != null)
		{
			img.color = (selected ? selectedButtonColor : unselectedButtonColor);
		}
		if (txt != null)
		{
			txt.color = (selected ? selectedTextColor : unselectedTextColor);
		}
	}

	private void SetupSettingsPanels()
	{
		if (settingsPanels == null)
		{
			return;
		}
		for (int i = 0; i < settingsPanels.Length; i++)
		{
			SettingsPanel settingsPanel = settingsPanels[i];
			if (settingsPanel.scrollView != null)
			{
				settingsPanel.scrollView.SetActive(i == 0);
			}
			if (!(settingsPanel.button == null))
			{
				GameObject target = settingsPanel.scrollView;
				settingsPanel.button.onClick.AddListener(delegate
				{
					ShowOnlySettingsPanel(target);
				});
			}
		}
	}

	private void ShowOnlySettingsPanel(GameObject target)
	{
		if (settingsPanels == null)
		{
			return;
		}
		SettingsPanel[] array = settingsPanels;
		foreach (SettingsPanel settingsPanel in array)
		{
			if (settingsPanel.scrollView != null)
			{
				settingsPanel.scrollView.SetActive(settingsPanel.scrollView == target);
			}
		}
	}

	private void SetupGeneralSettings()
	{
		if (mouseSensitivitySlider != null)
		{
			GeneralSettingsManager instance = GeneralSettingsManager.Instance;
			mouseSensitivitySlider.minValue = ((instance != null) ? instance.minSensitivity : 0.1f);
			mouseSensitivitySlider.maxValue = ((instance != null) ? instance.maxSensitivity : 3f);
			mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
		}
		if (invertYToggle != null)
		{
			invertYToggle.onValueChanged.AddListener(OnInvertYChanged);
		}
		if (fullscreenToggle != null)
		{
			fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
		}
		if (vsyncToggle != null)
		{
			vsyncToggle.onValueChanged.AddListener(OnVSyncChanged);
		}
		if (resolutionDropdown != null)
		{
			resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
		}
		if (qualityDropdown != null)
		{
			qualityDropdown.onValueChanged.AddListener(OnQualityDropdownChanged);
		}
		PopulateResolutionDropdownOptions();
		PopulateQualityDropdownOptions();
		RefreshGeneralSettingsUI();
	}

	private void PopulateResolutionDropdownOptions()
	{
		if (!(resolutionDropdown == null) && !(GeneralSettingsManager.Instance == null))
		{
			int value = resolutionDropdown.value;
			List<string> list = new List<string>();
			Resolution[] availableResolutions = GeneralSettingsManager.Instance.AvailableResolutions;
			for (int i = 0; i < availableResolutions.Length; i++)
			{
				Resolution resolution = availableResolutions[i];
				list.Add($"{resolution.width} x {resolution.height}");
			}
			resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions(list);
			resolutionDropdown.SetValueWithoutNotify(value);
		}
	}

	private void PopulateQualityDropdownOptions()
	{
		if (!(qualityDropdown == null))
		{
			int value = qualityDropdown.value;
			List<string> list = new List<string>();
			string[] names = QualitySettings.names;
			foreach (string rawName in names)
			{
				list.Add(LocalizeQualityLevelName(rawName));
			}
			qualityDropdown.ClearOptions();
			qualityDropdown.AddOptions(list);
			qualityDropdown.SetValueWithoutNotify(value);
		}
	}

	private string LocalizeQualityLevelName(string rawName)
	{
		return rawName switch
		{
			"High" => Localization.Get("QUALITY_HIGH"), 
			"Normal" => Localization.Get("QUALITY_NORMAL"), 
			"Low" => Localization.Get("QUALITY_LOW"), 
			_ => rawName, 
		};
	}

	private void RefreshGeneralSettingsUI()
	{
		GeneralSettingsManager instance = GeneralSettingsManager.Instance;
		if (!(instance == null))
		{
			if (mouseSensitivitySlider != null)
			{
				mouseSensitivitySlider.SetValueWithoutNotify(instance.MouseSensitivity);
			}
			if (mouseSensitivityValueText != null)
			{
				mouseSensitivityValueText.text = instance.MouseSensitivity.ToString("0.0");
			}
			if (invertYToggle != null)
			{
				invertYToggle.SetIsOnWithoutNotify(instance.InvertY);
			}
			if (fullscreenToggle != null)
			{
				fullscreenToggle.SetIsOnWithoutNotify(instance.Fullscreen);
			}
			if (vsyncToggle != null)
			{
				vsyncToggle.SetIsOnWithoutNotify(instance.VSync);
			}
			if (resolutionDropdown != null)
			{
				resolutionDropdown.SetValueWithoutNotify(instance.ResolutionIndex);
			}
			if (qualityDropdown != null)
			{
				qualityDropdown.SetValueWithoutNotify(instance.QualityLevel);
			}
		}
	}

	private void OnMouseSensitivityChanged(float value)
	{
		GeneralSettingsManager.Instance?.PreviewMouseSensitivity(value);
		if (mouseSensitivityValueText != null)
		{
			mouseSensitivityValueText.text = value.ToString("0.0");
		}
	}

	private void OnInvertYChanged(bool value)
	{
		GeneralSettingsManager.Instance?.PreviewInvertY(value);
	}

	private void OnFullscreenChanged(bool value)
	{
		GeneralSettingsManager.Instance?.PreviewFullscreen(value);
	}

	private void OnVSyncChanged(bool value)
	{
		GeneralSettingsManager.Instance?.PreviewVSync(value);
	}

	private void OnResolutionDropdownChanged(int index)
	{
		GeneralSettingsManager.Instance?.PreviewResolution(index);
	}

	private void OnQualityDropdownChanged(int index)
	{
		GeneralSettingsManager.Instance?.PreviewQualityLevel(index);
	}

	private void SetupAudioSettings()
	{
		if (masterVolumeSlider != null)
		{
			masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
		}
		if (musicVolumeSlider != null)
		{
			musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
		}
		if (mimicVolumeSlider != null)
		{
			mimicVolumeSlider.onValueChanged.AddListener(OnMimicVolumeChanged);
		}
		if (voiceChatModeDropdown != null)
		{
			voiceChatModeDropdown.onValueChanged.AddListener(OnVoiceModeDropdownChanged);
		}
		if (pushToTalkButton != null)
		{
			pushToTalkButton.onClick.AddListener(OnPushToTalkRebindClicked);
		}
		if (microphoneDropdown != null)
		{
			microphoneDropdown.onValueChanged.AddListener(OnMicrophoneDropdownChanged);
		}
		if (muteNpcAnimalsToggle != null)
		{
			muteNpcAnimalsToggle.onValueChanged.AddListener(OnMuteNpcAnimalsChanged);
		}
		PopulateVoiceModeDropdownOptions();
		PopulateMicrophoneDropdownOptions();
		RefreshAudioSettingsUI();
	}

	private void PopulateMicrophoneDropdownOptions()
	{
		if (!(microphoneDropdown == null))
		{
			int value = microphoneDropdown.value;
			List<string> collection = new List<string>(Microphone.devices);
			_microphoneOptions.Clear();
			_microphoneOptions.Add("");
			_microphoneOptions.AddRange(collection);
			List<string> list = new List<string> { Localization.Get("SETTINGS_MIC_DEFAULT") };
			list.AddRange(collection);
			microphoneDropdown.ClearOptions();
			microphoneDropdown.AddOptions(list);
			microphoneDropdown.SetValueWithoutNotify(value);
		}
	}

	private void PopulateVoiceModeDropdownOptions()
	{
		if (!(voiceChatModeDropdown == null))
		{
			int value = voiceChatModeDropdown.value;
			voiceChatModeDropdown.ClearOptions();
			voiceChatModeDropdown.AddOptions(new List<string>
			{
				Localization.Get("SETTINGS_PUSH_TO_TALK"),
				Localization.Get("SETTINGS_VOICE_ACTIVATION")
			});
			voiceChatModeDropdown.SetValueWithoutNotify(value);
		}
	}

	private void SetupLanguageDropdown()
	{
		PopulateLanguageDropdownOptions();
		if (languageDropdown != null)
		{
			languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
		}
		if (mainMenuLanguageDropdown != null)
		{
			mainMenuLanguageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
		}
	}

	private void PopulateLanguageDropdownOptions()
	{
		List<string> list = new List<string>();
		foreach (Language value in Enum.GetValues(typeof(Language)))
		{
			list.Add(Localization.GetNativeLanguageName(value));
		}
		ApplyLanguageDropdownOptions(languageDropdown, list);
		ApplyLanguageDropdownOptions(mainMenuLanguageDropdown, list);
	}

	private void ApplyLanguageDropdownOptions(TMP_Dropdown dropdown, List<string> options)
	{
		if (!(dropdown == null))
		{
			dropdown.ClearOptions();
			dropdown.AddOptions(options);
			dropdown.SetValueWithoutNotify((int)Localization.Current);
		}
	}

	private void OnLanguageDropdownChanged(int index)
	{
		if (Enum.IsDefined(typeof(Language), index))
		{
			Localization.Current = (Language)index;
		}
	}

	private void RefreshAudioSettingsUI()
	{
		AudioSettingsManager instance = AudioSettingsManager.Instance;
		float num = ((instance != null) ? instance.MasterVolume01 : 1f);
		float num2 = ((instance != null) ? instance.MusicVolume01 : 0.5f);
		float num3 = ((instance != null) ? instance.MimicVolume01 : 0.5f);
		CommActivationMode commActivationMode = ((instance != null) ? instance.VoiceMode : CommActivationMode.PushToTalk);
		if (masterVolumeSlider != null)
		{
			masterVolumeSlider.SetValueWithoutNotify(num);
		}
		SetVolumeText(masterVolumeValueText, num);
		if (musicVolumeSlider != null)
		{
			musicVolumeSlider.SetValueWithoutNotify(num2);
		}
		SetVolumeText(musicVolumeValueText, num2);
		if (mimicVolumeSlider != null)
		{
			mimicVolumeSlider.SetValueWithoutNotify(num3);
		}
		SetVolumeText(mimicVolumeValueText, num3);
		if (voiceChatModeDropdown != null)
		{
			int b = Array.IndexOf(VoiceModeOrder, commActivationMode);
			voiceChatModeDropdown.SetValueWithoutNotify(Mathf.Max(0, b));
		}
		if (microphoneDropdown != null)
		{
			string text = ((instance != null) ? instance.SelectedMicrophone : "");
			int b2 = _microphoneOptions.IndexOf(text ?? "");
			microphoneDropdown.SetValueWithoutNotify(Mathf.Max(0, b2));
		}
		if (muteNpcAnimalsToggle != null)
		{
			muteNpcAnimalsToggle.SetIsOnWithoutNotify(instance != null && instance.MuteNpcAnimals);
		}
		RefreshPushToTalkButtonVisibility(commActivationMode);
		RefreshPushToTalkRebindLabel();
	}

	private void RefreshPushToTalkButtonVisibility(CommActivationMode voiceMode)
	{
		if (pushToTalkButton != null)
		{
			pushToTalkButton.gameObject.SetActive(voiceMode == CommActivationMode.PushToTalk);
		}
	}

	private void RefreshPushToTalkRebindLabel()
	{
		if (!(pushToTalkText == null))
		{
			AudioSettingsManager instance = AudioSettingsManager.Instance;
			string text = ((instance != null && instance.IsRebindingPushToTalkKey) ? " " : ((instance != null) ? GetKeyShortLabel(instance.PushToTalkKey) : "V"));
			pushToTalkText.text = "[" + text + "]";
		}
	}

	private static string GetKeyShortLabel(KeyCode key)
	{
		if (key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6)
		{
			return "M" + (int)(key - 323 + 1);
		}
		return key.ToString();
	}

	private void OnPushToTalkRebindClicked()
	{
		AudioSettingsManager.Instance?.BeginRebindPushToTalkKey();
		RefreshPushToTalkRebindLabel();
	}

	private void OnMasterVolumeChanged(float value01)
	{
		AudioSettingsManager.Instance?.PreviewMasterVolume01(value01);
		SetVolumeText(masterVolumeValueText, value01);
	}

	private void OnMusicVolumeChanged(float value01)
	{
		AudioSettingsManager.Instance?.PreviewMusicVolume01(value01);
		SetVolumeText(musicVolumeValueText, value01);
	}

	private void OnMimicVolumeChanged(float value01)
	{
		AudioSettingsManager.Instance?.PreviewMimicVolume01(value01);
		SetVolumeText(mimicVolumeValueText, value01);
	}

	private void OnMuteNpcAnimalsChanged(bool value)
	{
		AudioSettingsManager.Instance?.PreviewMuteNpcAnimals(value);
	}

	private void OnVoiceModeDropdownChanged(int index)
	{
		if (index >= 0 && index < VoiceModeOrder.Length)
		{
			CommActivationMode commActivationMode = VoiceModeOrder[index];
			AudioSettingsManager.Instance?.PreviewVoiceMode(commActivationMode);
			RefreshPushToTalkButtonVisibility(commActivationMode);
		}
	}

	private void OnMicrophoneDropdownChanged(int index)
	{
		if (index >= 0 && index < _microphoneOptions.Count)
		{
			AudioSettingsManager.Instance?.PreviewMicrophone(_microphoneOptions[index]);
		}
	}

	private void SetVolumeText(TextMeshProUGUI text, float value01)
	{
		if (text != null)
		{
			text.text = $"{Mathf.RoundToInt(value01 * 100f)}%";
		}
	}

	private void OnApplySettingsClicked()
	{
		AudioSettingsManager.Instance?.SaveCurrent();
		GeneralSettingsManager.Instance?.SaveCurrent();
	}

	private void OnResetSettingsClicked()
	{
		AudioSettingsManager.Instance?.ResetToDefaults();
		RefreshAudioSettingsUI();
		GeneralSettingsManager.Instance?.ResetToDefaults();
		RefreshGeneralSettingsUI();
	}

	private void OnCreateConfirmClicked()
	{
		if (SteamLobby.instance != null && SteamLobby.instance.IsMatchmaking)
		{
			SteamLobby.instance.CancelMatchmaking();
		}
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (singleton != null)
		{
			singleton.maxConnections = _playerCount;
			singleton.SetSelectedMap(_selectedMap);
		}
		Debug.Log($"[Menu] Lobby kuruluyor. Max oyuncu: {_playerCount}, Public: {_isPublic}, Map: {_selectedMap}");
		if (SteamLobby.instance != null)
		{
			SteamLobby.instance.CreateLobby(_isPublic);
			return;
		}
		Debug.LogWarning("[Menu] SteamLobby yok — düz host başlatılıyor (Steam dışı test).");
		if (singleton != null && !NetworkServer.active)
		{
			LoadingScreen.Instance?.Show();
			singleton.SetMultiplayer(value: true);
			singleton.StartHost();
			LoadingScreen.Instance?.Hide();
		}
	}
}
