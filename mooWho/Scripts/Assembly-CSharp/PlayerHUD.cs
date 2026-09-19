using System;
using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	private class TopPlayerListItemRefs
	{
		public readonly GameObject root;

		public readonly TMP_Text nameText;

		public readonly TMP_Text roleNameText;

		public readonly Image avatarImage;

		public readonly TMP_Text numberText;

		public TopPlayerListItemRefs(Transform t)
		{
			root = t.gameObject;
			nameText = t.Find("PlayerNameText")?.GetComponent<TMP_Text>();
			roleNameText = t.Find("RoleNameText")?.GetComponent<TMP_Text>();
			avatarImage = t.Find("RoleAvatar")?.GetComponent<Image>();
			Transform transform = t.Find("NumberIcon");
			numberText = ((!(transform != null)) ? null : transform.Find("NumberText")?.GetComponent<TMP_Text>());
		}
	}

	private struct RankedEntry
	{
		public string name;

		public string roleName;

		public Sprite avatar;

		public int score;

		public uint netId;
	}

	private const float HudSlideFadeSpeed = 10f;

	[Header("Voice Göstergesi")]
	[Tooltip("Konuşurken (push-to-talk basılı + ses var) açılır")]
	public GameObject voiceIndicatorImage;

	[Tooltip("Bu amplitude üstü 'ses var' sayılır")]
	public float voiceThreshold = 0.02f;

	[Header("Role-Specific GUI")]
	public GameObject[] hunterOnlyObjects;

	public GameObject[] animalOnlyObjects;

	[Header("Oyun Countdown")]
	public TextMeshProUGUI gameCountdownText;

	public GameObject TopCountdownGroup;

	private const float TopCountdownHiddenY = 665f;

	private const float TopCountdownVisibleY = 486f;

	private const float TopCountdownSlideSpeed = 8f;

	private RectTransform _topCountdownRect;

	private float _topCountdownTargetY;

	private CanvasGroup _topCountdownCanvasGroup;

	private DissonanceComms _comms;

	private VoicePlayerState _localState;

	private PlayerRole _role = PlayerRole.Animal;

	private bool _roleKnown;

	[Header("Hayvan — Enerji Barı")]
	[Tooltip("Hunter'da kapanır, Animal'da açılır — AnimalEnergyController bunu SetEnergyBarVisible/SetEnergyFill ile buradan (PlayerHUD üzerinden) kullanır.")]
	public GameObject AnimalEnergyBar;

	[Tooltip("Enerji göstergesi — Image.fillAmount 0..1")]
	public Image AnimalEnergyBarFill;

	[Tooltip("Normal (yeterli enerji) dolum rengi")]
	public Color energyBarDefaultColor = new Color32(byte.MaxValue, 163, 55, byte.MaxValue);

	[Tooltip("Enerji yetersizken koşmaya çalışınca (Shift + hareket) bu renge doğru pulse atar")]
	public Color energyBarInsufficientColor = new Color32(207, 56, 56, byte.MaxValue);

	public float energyBarPulseSpeed = 6f;

	[Tooltip("Enerji TAMAMEN biterken (0) AnimalEnergyBar'ın kendisi ne kadar büyüyüp küçülerek pulse atar (ör. 0.04 = ±%4) — kasıtlı olarak çok küçük tutulmalı, enerji tekrar TAM dolana (1) kadar sürer.")]
	public float energyBarScalePulseAmount = 0.04f;

	public float energyBarScalePulseSpeed = 4f;

	private RectTransform _energyBarRect;

	private bool _energyDepleted;

	private float _energyBarPulseT;

	[Header("Hunter — Shotgun")]
	public GameObject shotgunGroup;

	[Tooltip("ShotgunAmmoBox içindeki tek ShellIcon objesi — şablon olarak kullanılır (kendisi de ilk ikon olur), MaxAmmo kadar ikon her zaman görünür, mermi hakkına göre renkleri değişir")]
	public GameObject shellIconTemplate;

	[Tooltip("Dolu mermi rengi")]
	public Color shellLoadedColor = Color.white;

	[Tooltip("Boş (harcanmış) mermi rengi")]
	public Color shellDepletedColor = new Color(0.4f, 0.4f, 0.4f);

	[Tooltip("Mermi tamamen bitince (0) bu renge doğru yavaşça pulse atar — kulübeye gitme uyarısı")]
	public Color shellEmptyPulseColor = new Color(0.9f, 0.15f, 0.1f);

	public float shellEmptyPulseSpeed = 2f;

	private readonly List<GameObject> _shellIcons = new List<GameObject>();

	private readonly List<Image> _shellIconImages = new List<Image>();

	private int _lastShownAmmo = -1;

	private int _lastShownMaxAmmo = -1;

	[Header("Hunter — Mermi Bitti Bildirimi")]
	public GameObject outOfAmmoNotification;

	private const float OutOfAmmoVisibleY = 300f;

	private const float OutOfAmmoHiddenY = 660f;

	private const float OutOfAmmoSlideSpeed = 8f;

	private RectTransform _outOfAmmoRect;

	private float _outOfAmmoTargetY;

	private CanvasGroup _outOfAmmoCanvasGroup;

	[Header("Profile")]
	public TMP_Text RoleText;

	public TMP_Text UsernameText;

	public Image RoleImage;

	public Sprite hunterSprite;

	[Header("Yanlış Vuruş Ceza Bildirimi")]
	[Tooltip("Avcı yanlış (bot) hedef vurup süreden ceza düşünce HERKESTE görünür — kendi rolüne göre renk (avcıda kırmızı, hayvanda yeşil)")]
	public TMP_Text WrongShotTimeAddText;

	public Color wrongShotHunterColor = Color.red;

	public Color wrongShotAnimalColor = Color.green;

	public float wrongShotPopDuration = 0.25f;

	[Tooltip("Pop bitince text'in kalıcı olarak kalacağı ölçek — normal boyuttan (1x) biraz büyük, daha belirgin olsun diye (abartmadan)")]
	public float wrongShotPopTargetScale = 1.15f;

	public float wrongShotHoldDuration = 2f;

	public float wrongShotFadeOutDuration = 0.3f;

	private Coroutine _wrongShotRoutine;

	[Header("Hunter Serbest Bırakma Geri Sayımı")]
	[Tooltip("Paneller kapandıktan sonra hunter kapısı açılana kadar '10 9 8 7...' gösterir — GameManager.UpdateHunterReleaseCountdown her frame çağırır, süre bitince metin boşalır")]
	public TMP_Text HunterReleaseCountdownText;

	[Tooltip("Her sayı düşüşünde oynatılan pop (büyüden normale) animasyonunun süresi")]
	public float hunterReleasePopDuration = 0.25f;

	[Tooltip("Pop'un başladığı ölçek — buradan 1x'e küçülerek oturur")]
	public float hunterReleasePopStartScale = 1.4f;

	private Coroutine _hunterReleasePopRoutine;

	private int _lastHunterReleaseSecond = -1;

	[Tooltip("Local oyuncu KENDİ panelinden çıktıktan sonra görünür — hunter release süre sekansına (yukarıdaki '10 9 8 7...' geri sayımı) girilince kayar/kaybolur. TopCountdown/OutOfAmmo ile AYNI kayma deseni (RectTransform.anchoredPosition.y).")]
	public GameObject WaitingForRecordingText;

	private const float WaitingForRecordingVisibleY = 0f;

	private const float WaitingForRecordingHiddenY = 155f;

	private const float WaitingForRecordingSlideSpeed = 8f;

	private RectTransform _waitingForRecordingRect;

	private float _waitingForRecordingTargetY;

	private bool _waitingForRecordingVisible;

	private CanvasGroup _waitingForRecordingCanvasGroup;

	private TMP_Text _waitingForRecordingLabel;

	[Tooltip("Görünürken çok hafif bir nabız (scale pulse) atar — VoiceNotificationUI'deki sinek bildirimi nabzıyla AYNI desen. Ne kadar büyüyüp küçüleceği (ör. 0.03 = ±%3) — kasıtlı olarak çok küçük tutulmalı.")]
	public float waitingForRecordingPulseAmount = 0.03f;

	public float waitingForRecordingPulseSpeed = 2.5f;

	private float _waitingForRecordingPulseT;

	[Tooltip("İSTEK: sabit sayı yerine basit bir 'yükleniyor' efekti — metnin sonunda nokta sayısı 0'dan 3'e kadar döngüsel artıp sıfırlanır (\"bekleniyor\" → \"bekleniyor.\" → \"bekleniyor..\" → \"bekleniyor...\" → başa döner). Saniyede kaç nokta eklenip çıkarılacağını belirler.")]
	public float waitingForRecordingDotsPerSecond = 2f;

	private float _waitingForRecordingDotsT;

	private HunterShotgun _localShotgun;

	private bool _roleApplied;

	public GameObject _gameEndPanel;

	private Transform _panel;

	private TMP_Text _rolesWinText;

	private LocalizedText _rolesWinTextLocalized;

	private TMP_Text _winCaptionText;

	private LocalizedText _winCaptionTextLocalized;

	private TMP_Text _newGameCountdownText;

	private GameObject _hostReturnLobbyText;

	private Transform _animalsListParent;

	private Transform _huntersListParent;

	private GameObject _animalsItemTemplate;

	private GameObject _huntersItemTemplate;

	private readonly List<TopPlayerListItemRefs> _animalsListItems = new List<TopPlayerListItemRefs>();

	private readonly List<TopPlayerListItemRefs> _huntersListItems = new List<TopPlayerListItemRefs>();

	private const int MaxRankListItems = 5;

	[Header("Game End — TopBox (yukardan düşme animasyonu)")]
	private const float TopBoxHiddenY = 360f;

	private const float TopBoxVisibleY = 0f;

	private const float TopBoxSlideSpeed = 8f;

	private RectTransform _topBoxRect;

	private float _topBoxTargetY;

	[Header("Game End — Sıralama Animasyonu")]
	[Tooltip("Panel açılışından sonraki tekil pop-in süresi (sn)")]
	public float gameEndPanelDuration = 0.35f;

	[Tooltip("Her sıralama item'ının kendi pop-in süresi (sn)")]
	public float rankItemPopDuration = 0.25f;

	[Tooltip("Sıralama item'ları arasındaki gecikme (sn) — sırayla gelsinler")]
	public float rankItemStagger = 0.3f;

	private Coroutine _gameEndAnimRoutine;

	private readonly List<Coroutine> _rankItemPopRoutines = new List<Coroutine>();

	[Header("Loading (round geçişlerinde — ışınlanma gizlensin diye)")]
	[Tooltip("Üzerinde CanvasGroup olan LoadingPanel objesi")]
	public GameObject loadingPanel;

	public float loadingFadeDuration = 0.25f;

	private Coroutine _loadingFadeRoutine;

	[Header("Free Camera (tüm HUD'u gizler)")]
	[Tooltip("Tüm oyuncu HUD'unu saran kök objenin CanvasGroup'u — free camera sırasında alpha 0 yapılır")]
	public CanvasGroup playerHudGroup;

	public float hudFadeDuration = 0.2f;

	private Coroutine _hudFadeRoutine;

	[Header("Countdown Pulse (opsiyonel)")]
	[Tooltip("gameCountdownText ile aynı objede — süre her düştüğünde küçük bir scale-up darbesi oynatır")]
	public PunchScaleText gameCountdownPunch;

	private bool _shotgunGroupVisible = true;

	public static PlayerHUD Instance { get; private set; }

	private static CanvasGroup GetOrAddCanvasGroup(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = go.AddComponent<CanvasGroup>();
		}
		return canvasGroup;
	}

	public void SetHudVisible(bool visible)
	{
		if (!(playerHudGroup == null))
		{
			if (_hudFadeRoutine != null)
			{
				StopCoroutine(_hudFadeRoutine);
			}
			_hudFadeRoutine = StartCoroutine(FadeHud(visible));
		}
	}

	private IEnumerator FadeHud(bool visible)
	{
		float target = (visible ? 1f : 0f);
		float start = playerHudGroup.alpha;
		float t = 0f;
		if (!visible)
		{
			playerHudGroup.interactable = false;
			playerHudGroup.blocksRaycasts = false;
		}
		while (t < hudFadeDuration)
		{
			t += Time.deltaTime;
			playerHudGroup.alpha = Mathf.Lerp(start, target, Mathf.Clamp01(t / hudFadeDuration));
			yield return null;
		}
		playerHudGroup.alpha = target;
		if (visible)
		{
			playerHudGroup.interactable = true;
			playerHudGroup.blocksRaycasts = true;
		}
		_hudFadeRoutine = null;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (voiceIndicatorImage != null)
		{
			voiceIndicatorImage.SetActive(value: false);
		}
		if (HunterReleaseCountdownText != null)
		{
			HunterReleaseCountdownText.gameObject.SetActive(value: false);
		}
		if (AnimalEnergyBar != null)
		{
			AnimalEnergyBar.SetActive(value: false);
			_energyBarRect = AnimalEnergyBar.GetComponent<RectTransform>();
		}
		if (AnimalEnergyBarFill != null)
		{
			AnimalEnergyBarFill.color = energyBarDefaultColor;
		}
		ResolveGameEndReferences();
		if (loadingPanel != null)
		{
			loadingPanel.SetActive(value: false);
		}
		if (TopCountdownGroup != null)
		{
			_topCountdownRect = TopCountdownGroup.GetComponent<RectTransform>();
			_topCountdownTargetY = 665f;
			if (_topCountdownRect != null)
			{
				Vector2 anchoredPosition = _topCountdownRect.anchoredPosition;
				anchoredPosition.y = 665f;
				_topCountdownRect.anchoredPosition = anchoredPosition;
			}
			_topCountdownCanvasGroup = GetOrAddCanvasGroup(TopCountdownGroup);
			if (_topCountdownCanvasGroup != null)
			{
				_topCountdownCanvasGroup.alpha = 0f;
			}
			TopCountdownGroup.SetActive(value: false);
		}
		if (outOfAmmoNotification != null)
		{
			_outOfAmmoRect = outOfAmmoNotification.GetComponent<RectTransform>();
			_outOfAmmoTargetY = 660f;
			if (_outOfAmmoRect != null)
			{
				Vector2 anchoredPosition2 = _outOfAmmoRect.anchoredPosition;
				anchoredPosition2.y = 660f;
				_outOfAmmoRect.anchoredPosition = anchoredPosition2;
			}
			_outOfAmmoCanvasGroup = GetOrAddCanvasGroup(outOfAmmoNotification);
			if (_outOfAmmoCanvasGroup != null)
			{
				_outOfAmmoCanvasGroup.alpha = 0f;
			}
			outOfAmmoNotification.SetActive(value: false);
		}
		if (WaitingForRecordingText != null)
		{
			_waitingForRecordingRect = WaitingForRecordingText.GetComponent<RectTransform>();
			_waitingForRecordingTargetY = 155f;
			if (_waitingForRecordingRect != null)
			{
				Vector2 anchoredPosition3 = _waitingForRecordingRect.anchoredPosition;
				anchoredPosition3.y = 155f;
				_waitingForRecordingRect.anchoredPosition = anchoredPosition3;
			}
			_waitingForRecordingLabel = WaitingForRecordingText.GetComponentInChildren<TMP_Text>(includeInactive: true);
			_waitingForRecordingCanvasGroup = GetOrAddCanvasGroup(WaitingForRecordingText);
			if (_waitingForRecordingCanvasGroup != null)
			{
				_waitingForRecordingCanvasGroup.alpha = 0f;
			}
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		PlayerRoleData.LocalRoleChanged -= HandleLocalRoleChanged;
	}

	private void OnEnable()
	{
		PlayerRoleData.LocalRoleChanged += HandleLocalRoleChanged;
	}

	private void HandleLocalRoleChanged(PlayerRole role)
	{
		ApplyRole(role);
	}

	public void SetEnergyBarVisible(bool visible)
	{
		if (AnimalEnergyBar != null && AnimalEnergyBar.activeSelf != visible)
		{
			AnimalEnergyBar.SetActive(visible);
		}
	}

	public void SetEnergyFill(float amount01, bool pulseInsufficient)
	{
		if (AnimalEnergyBarFill != null)
		{
			AnimalEnergyBarFill.fillAmount = amount01;
			if (pulseInsufficient)
			{
				float t = (Mathf.Sin(Time.time * energyBarPulseSpeed) + 1f) * 0.5f;
				AnimalEnergyBarFill.color = Color.Lerp(energyBarDefaultColor, energyBarInsufficientColor, t);
			}
			else
			{
				AnimalEnergyBarFill.color = energyBarDefaultColor;
			}
		}
		if (amount01 <= 0f)
		{
			_energyDepleted = true;
		}
		else if (amount01 >= 1f)
		{
			_energyDepleted = false;
		}
		if (_energyBarRect != null)
		{
			if (_energyDepleted)
			{
				_energyBarPulseT += Time.deltaTime * energyBarScalePulseSpeed;
				float num = 1f + Mathf.Sin(_energyBarPulseT) * energyBarScalePulseAmount;
				_energyBarRect.localScale = Vector3.one * num;
			}
			else if (_energyBarRect.localScale != Vector3.one)
			{
				_energyBarPulseT = 0f;
				_energyBarRect.localScale = Vector3.one;
			}
		}
	}

	public void SetTopCountdownVisible(bool visible)
	{
		_topCountdownTargetY = (visible ? 486f : 665f);
		if (visible && TopCountdownGroup != null && !TopCountdownGroup.activeSelf)
		{
			TopCountdownGroup.SetActive(value: true);
		}
	}

	public void SetWaitingForRecordingVisible(bool visible)
	{
		_waitingForRecordingTargetY = (visible ? 0f : 155f);
		_waitingForRecordingVisible = visible;
	}

	private void ResolveGameEndReferences()
	{
		if (_gameEndPanel == null)
		{
			return;
		}
		_panel = _gameEndPanel.transform.Find("Panel");
		if (_panel != null)
		{
			Transform transform = _panel.Find("TopBox");
			_topBoxRect = ((transform != null) ? transform.GetComponent<RectTransform>() : null);
			_rolesWinText = _panel.Find("TopBox/RolesWinText")?.GetComponent<TMP_Text>();
			_rolesWinTextLocalized = ((_rolesWinText != null) ? _rolesWinText.GetComponent<LocalizedText>() : null);
			_winCaptionText = _panel.Find("TopBox/WinCaptionText")?.GetComponent<TMP_Text>();
			_winCaptionTextLocalized = ((_winCaptionText != null) ? _winCaptionText.GetComponent<LocalizedText>() : null);
			Transform transform2 = _panel.Find("Countdown/NewGameCountdownText");
			_newGameCountdownText = ((transform2 != null) ? transform2.GetComponent<TMP_Text>() : null);
			Transform transform3 = _panel.Find("HostReturnToLobbyBox/HostReturnLobbyText");
			_hostReturnLobbyText = ((transform3 != null) ? transform3.gameObject : null);
			_animalsListParent = _panel.Find("AnimalsCard/AnimalsList");
			if (_animalsListParent != null && _animalsListParent.childCount > 0)
			{
				_animalsItemTemplate = _animalsListParent.GetChild(0).gameObject;
				_animalsListItems.Add(new TopPlayerListItemRefs(_animalsItemTemplate.transform));
			}
			_huntersListParent = _panel.Find("HuntersCard/HuntersList");
			if (_huntersListParent != null && _huntersListParent.childCount > 0)
			{
				_huntersItemTemplate = _huntersListParent.GetChild(0).gameObject;
				_huntersListItems.Add(new TopPlayerListItemRefs(_huntersItemTemplate.transform));
			}
		}
		if (_hostReturnLobbyText != null)
		{
			_hostReturnLobbyText.SetActive(value: false);
		}
		_gameEndPanel.SetActive(value: false);
		if (_topBoxRect != null)
		{
			_topBoxTargetY = 360f;
			Vector2 anchoredPosition = _topBoxRect.anchoredPosition;
			anchoredPosition.y = 360f;
			_topBoxRect.anchoredPosition = anchoredPosition;
		}
	}

	public void ShowGameEnd(bool huntersWon, float newGameRemaining)
	{
		if (_gameEndPanel != null && !_gameEndPanel.activeSelf)
		{
			_gameEndPanel.SetActive(value: true);
			ApplyGameEndContent(huntersWon);
			_topBoxTargetY = 0f;
			StopAllGameEndAnimations();
			_gameEndAnimRoutine = StartCoroutine(AnimateGameEndIn());
		}
		if (_newGameCountdownText != null)
		{
			int num = Mathf.Max(0, Mathf.CeilToInt(newGameRemaining));
			_newGameCountdownText.text = Localization.GetFormatted("TIMER_SECONDS_INT", num);
		}
	}

	public void HideGameEnd()
	{
		StopAllGameEndAnimations();
		if (_gameEndPanel != null && _gameEndPanel.activeSelf)
		{
			_gameEndPanel.SetActive(value: false);
		}
		if (_topBoxRect != null)
		{
			_topBoxTargetY = 360f;
			Vector2 anchoredPosition = _topBoxRect.anchoredPosition;
			anchoredPosition.y = 360f;
			_topBoxRect.anchoredPosition = anchoredPosition;
		}
	}

	private void ApplyGameEndContent(bool huntersWon)
	{
		string key = (huntersWon ? "RESULT_HUNTERS_WIN" : "RESULT_ANIMALS_WIN");
		string key2 = (huntersWon ? "RESULT_HUNTERS_WIN_CAPTION" : "RESULT_ANIMALS_WIN_CAPTION");
		if (_rolesWinTextLocalized != null)
		{
			_rolesWinTextLocalized.Key = key;
		}
		else if (_rolesWinText != null)
		{
			_rolesWinText.text = Localization.Get(key);
		}
		if (_winCaptionTextLocalized != null)
		{
			_winCaptionTextLocalized.Key = key2;
		}
		else if (_winCaptionText != null)
		{
			_winCaptionText.text = Localization.Get(key2);
		}
		PopulateTopPlayerLists();
	}

	private void PopulateTopPlayerLists()
	{
		List<RankedEntry> list = new List<RankedEntry>();
		List<RankedEntry> list2 = new List<RankedEntry>();
		foreach (KeyValuePair<uint, NetworkIdentity> item in NetworkClient.spawned)
		{
			NetworkIdentity value = item.Value;
			if (value == null)
			{
				continue;
			}
			PlayerRoleData component = value.GetComponent<PlayerRoleData>();
			if (!(component == null) && component.RolesLocked)
			{
				MyClient component2 = value.GetComponent<MyClient>();
				string text = ((component2 != null) ? component2.playerInfo.username : "?");
				if (component.Role == PlayerRole.Hunter)
				{
					HunterShotgun component3 = value.GetComponent<HunterShotgun>();
					list2.Add(new RankedEntry
					{
						name = text,
						roleName = Localization.Get("ROLE_HUNTER"),
						avatar = hunterSprite,
						score = ((component3 != null) ? component3.KillCount : 0),
						netId = value.netId
					});
				}
				else
				{
					PlayerVoiceMonitor component4 = value.GetComponent<PlayerVoiceMonitor>();
					list.Add(new RankedEntry
					{
						name = text,
						roleName = Localization.GetAnimalName(component.AssignedAnimal),
						avatar = ((RoleAssignmentUI.Instance != null) ? RoleAssignmentUI.Instance.GetAnimalProfileSprite(component.AssignedAnimal) : null),
						score = ((component4 != null) ? component4.SoundsMadeCount : 0),
						netId = value.netId
					});
				}
			}
		}
		PopulateRankList(_animalsListItems, _animalsItemTemplate, _animalsListParent, SortRanked(list));
		PopulateRankList(_huntersListItems, _huntersItemTemplate, _huntersListParent, SortRanked(list2));
		static RankedEntry[] SortRanked(List<RankedEntry> list3)
		{
			RankedEntry[] array = list3.ToArray();
			Array.Sort(array, delegate(RankedEntry a, RankedEntry b)
			{
				int num = b.score.CompareTo(a.score);
				return (num == 0) ? a.netId.CompareTo(b.netId) : num;
			});
			return array;
		}
	}

	private void PopulateRankList(List<TopPlayerListItemRefs> pooledItems, GameObject template, Transform listParent, RankedEntry[] entries)
	{
		if (template == null || listParent == null)
		{
			return;
		}
		int num = Mathf.Min(entries.Length, 5);
		while (pooledItems.Count < num)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(template, listParent);
			pooledItems.Add(new TopPlayerListItemRefs(gameObject.transform));
		}
		for (int i = 0; i < pooledItems.Count; i++)
		{
			TopPlayerListItemRefs topPlayerListItemRefs = pooledItems[i];
			if (topPlayerListItemRefs == null || topPlayerListItemRefs.root == null)
			{
				continue;
			}
			bool flag = i < num;
			topPlayerListItemRefs.root.SetActive(flag);
			if (flag)
			{
				RankedEntry rankedEntry = entries[i];
				if (topPlayerListItemRefs.nameText != null)
				{
					topPlayerListItemRefs.nameText.text = rankedEntry.name;
				}
				if (topPlayerListItemRefs.roleNameText != null)
				{
					topPlayerListItemRefs.roleNameText.text = rankedEntry.roleName;
				}
				if (topPlayerListItemRefs.avatarImage != null && rankedEntry.avatar != null)
				{
					topPlayerListItemRefs.avatarImage.sprite = rankedEntry.avatar;
				}
				if (topPlayerListItemRefs.numberText != null)
				{
					topPlayerListItemRefs.numberText.text = (i + 1).ToString();
				}
			}
		}
	}

	public void SetHostReturnPromptVisible(bool visible)
	{
		if (_hostReturnLobbyText != null && _hostReturnLobbyText.activeSelf != visible)
		{
			_hostReturnLobbyText.SetActive(visible);
		}
	}

	public void ShowLoading()
	{
		if (!(loadingPanel == null))
		{
			if (_loadingFadeRoutine != null)
			{
				StopCoroutine(_loadingFadeRoutine);
			}
			if (!loadingPanel.activeSelf)
			{
				loadingPanel.SetActive(value: true);
			}
			_loadingFadeRoutine = StartCoroutine(FadeLoadingPanel(1f, disableOnComplete: false));
		}
	}

	public void HideLoading()
	{
		if (!(loadingPanel == null) && loadingPanel.activeSelf)
		{
			if (_loadingFadeRoutine != null)
			{
				StopCoroutine(_loadingFadeRoutine);
			}
			_loadingFadeRoutine = StartCoroutine(FadeLoadingPanel(0f, disableOnComplete: true));
		}
	}

	private IEnumerator FadeLoadingPanel(float targetAlpha, bool disableOnComplete)
	{
		CanvasGroup group = loadingPanel.GetComponent<CanvasGroup>();
		if (group == null)
		{
			if (disableOnComplete)
			{
				loadingPanel.SetActive(value: false);
			}
			yield break;
		}
		float start = group.alpha;
		float t = 0f;
		while (t < loadingFadeDuration)
		{
			t += Time.deltaTime;
			group.alpha = Mathf.Lerp(start, targetAlpha, Mathf.Clamp01(t / loadingFadeDuration));
			yield return null;
		}
		group.alpha = targetAlpha;
		if (disableOnComplete)
		{
			loadingPanel.SetActive(value: false);
		}
		_loadingFadeRoutine = null;
	}

	private IEnumerator AnimateGameEndIn()
	{
		CanvasGroup canvasGroup = _gameEndPanel.GetComponent<CanvasGroup>();
		RectTransform rect = _gameEndPanel.GetComponent<RectTransform>();
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 0f;
		}
		if (rect != null)
		{
			rect.localScale = Vector3.one * 0.85f;
		}
		PrepareTopPlayerListForPop(_animalsListItems);
		PrepareTopPlayerListForPop(_huntersListItems);
		float t = 0f;
		while (t < gameEndPanelDuration)
		{
			t += Time.deltaTime;
			float num = Mathf.Clamp01(t / gameEndPanelDuration);
			float num2 = 1f - Mathf.Pow(1f - num, 3f);
			if (canvasGroup != null)
			{
				canvasGroup.alpha = num2;
			}
			if (rect != null)
			{
				rect.localScale = Vector3.one * Mathf.Lerp(0.85f, 1f, num2);
			}
			yield return null;
		}
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 1f;
		}
		if (rect != null)
		{
			rect.localScale = Vector3.one;
		}
		int maxCount = Mathf.Max(_animalsListItems.Count, _huntersListItems.Count);
		for (int i = 0; i < maxCount; i++)
		{
			if (i < _animalsListItems.Count)
			{
				TryStartPopItem(_animalsListItems[i]);
			}
			if (i < _huntersListItems.Count)
			{
				TryStartPopItem(_huntersListItems[i]);
			}
			yield return new WaitForSeconds(rankItemStagger);
		}
		_gameEndAnimRoutine = null;
	}

	private void TryStartPopItem(TopPlayerListItemRefs item)
	{
		if (item != null && !(item.root == null) && item.root.activeSelf)
		{
			_rankItemPopRoutines.Add(StartCoroutine(PopItem(item)));
		}
	}

	private void PrepareTopPlayerListForPop(List<TopPlayerListItemRefs> items)
	{
		if (items == null)
		{
			return;
		}
		foreach (TopPlayerListItemRefs item in items)
		{
			if (!(item?.root == null) && item.root.activeSelf)
			{
				CanvasGroup canvasGroup = item.root.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = item.root.AddComponent<CanvasGroup>();
				}
				canvasGroup.alpha = 0f;
				RectTransform component = item.root.GetComponent<RectTransform>();
				if (component != null)
				{
					component.localScale = Vector3.one * 0.7f;
				}
			}
		}
	}

	private IEnumerator PopItem(TopPlayerListItemRefs item)
	{
		CanvasGroup cg = item.root.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = item.root.AddComponent<CanvasGroup>();
		}
		RectTransform rt = item.root.GetComponent<RectTransform>();
		float t = 0f;
		while (t < rankItemPopDuration)
		{
			t += Time.deltaTime;
			float num = Mathf.Clamp01(t / rankItemPopDuration);
			float t2 = (cg.alpha = 1f - Mathf.Pow(1f - num, 3f));
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.Lerp(0.7f, 1f, t2);
			}
			yield return null;
		}
		cg.alpha = 1f;
		if (rt != null)
		{
			rt.localScale = Vector3.one;
		}
	}

	private void StopAllGameEndAnimations()
	{
		if (_gameEndAnimRoutine != null)
		{
			StopCoroutine(_gameEndAnimRoutine);
			_gameEndAnimRoutine = null;
		}
		foreach (Coroutine rankItemPopRoutine in _rankItemPopRoutines)
		{
			if (rankItemPopRoutine != null)
			{
				StopCoroutine(rankItemPopRoutine);
			}
		}
		_rankItemPopRoutines.Clear();
	}

	private void Start()
	{
		_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
	}

	public void ApplyRole(PlayerRole role)
	{
		_role = role;
		_roleKnown = true;
		bool flag = role == PlayerRole.Hunter;
		if (shotgunGroup != null)
		{
			shotgunGroup.SetActive(flag);
		}
		if (hunterOnlyObjects != null)
		{
			GameObject[] array = hunterOnlyObjects;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(flag);
				}
			}
		}
		if (animalOnlyObjects != null)
		{
			GameObject[] array = animalOnlyObjects;
			foreach (GameObject gameObject2 in array)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(!flag);
				}
			}
		}
		if (shotgunGroup != null)
		{
			shotgunGroup.SetActive(flag);
		}
		if (flag && NetworkClient.localPlayer != null)
		{
			_localShotgun = NetworkClient.localPlayer.GetComponent<HunterShotgun>();
		}
		if (shotgunGroup != null)
		{
			shotgunGroup.SetActive(flag);
		}
		_shotgunGroupVisible = flag;
	}

	public void ShowWrongShotTimePenalty(float seconds)
	{
		if (!(WrongShotTimeAddText == null))
		{
			WrongShotTimeAddText.text = $"-{Mathf.RoundToInt(seconds)}";
			WrongShotTimeAddText.color = ((_role == PlayerRole.Hunter) ? wrongShotHunterColor : wrongShotAnimalColor);
			if (_wrongShotRoutine != null)
			{
				StopCoroutine(_wrongShotRoutine);
			}
			_wrongShotRoutine = StartCoroutine(PlayWrongShotPopFade());
		}
	}

	public void ShowCorrectShotTimeBonus(float seconds)
	{
		if (!(WrongShotTimeAddText == null))
		{
			WrongShotTimeAddText.text = $"+{Mathf.RoundToInt(seconds)}";
			WrongShotTimeAddText.color = ((_role == PlayerRole.Hunter) ? wrongShotAnimalColor : wrongShotHunterColor);
			if (_wrongShotRoutine != null)
			{
				StopCoroutine(_wrongShotRoutine);
			}
			_wrongShotRoutine = StartCoroutine(PlayWrongShotPopFade());
		}
	}

	private IEnumerator PlayWrongShotPopFade()
	{
		GameObject go = WrongShotTimeAddText.gameObject;
		go.SetActive(value: true);
		CanvasGroup cg = go.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = go.AddComponent<CanvasGroup>();
		}
		RectTransform rt = go.GetComponent<RectTransform>();
		cg.alpha = 0f;
		if (rt != null)
		{
			rt.localScale = Vector3.one * 0.5f;
		}
		float t = 0f;
		while (t < wrongShotPopDuration)
		{
			t += Time.deltaTime;
			float t2 = (cg.alpha = Mathf.Clamp01(t / wrongShotPopDuration));
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.LerpUnclamped(0.5f, wrongShotPopTargetScale, EaseOutBack(t2));
			}
			yield return null;
		}
		cg.alpha = 1f;
		if (rt != null)
		{
			rt.localScale = Vector3.one * wrongShotPopTargetScale;
		}
		yield return new WaitForSeconds(wrongShotHoldDuration);
		t = 0f;
		while (t < wrongShotFadeOutDuration)
		{
			t += Time.deltaTime;
			cg.alpha = 1f - Mathf.Clamp01(t / wrongShotFadeOutDuration);
			yield return null;
		}
		cg.alpha = 0f;
		go.SetActive(value: false);
		_wrongShotRoutine = null;
	}

	public void UpdateHunterReleaseCountdown(float remainingSeconds)
	{
		if (HunterReleaseCountdownText == null)
		{
			return;
		}
		if (remainingSeconds <= 0f)
		{
			if (_lastHunterReleaseSecond != -1)
			{
				GameAudioManager.Instance?.PlayCountdownSnap();
			}
			if (HunterReleaseCountdownText.text.Length > 0)
			{
				HunterReleaseCountdownText.text = "";
			}
			_lastHunterReleaseSecond = -1;
			if (HunterReleaseCountdownText.gameObject.activeSelf)
			{
				HunterReleaseCountdownText.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!HunterReleaseCountdownText.gameObject.activeSelf)
		{
			HunterReleaseCountdownText.gameObject.SetActive(value: true);
		}
		int num = Mathf.CeilToInt(remainingSeconds);
		if (num != _lastHunterReleaseSecond)
		{
			_lastHunterReleaseSecond = num;
			HunterReleaseCountdownText.text = num.ToString();
			GameAudioManager.Instance?.PlayCountdownClock();
			if (_hunterReleasePopRoutine != null)
			{
				StopCoroutine(_hunterReleasePopRoutine);
			}
			_hunterReleasePopRoutine = StartCoroutine(PlayHunterReleasePop());
		}
	}

	private IEnumerator PlayHunterReleasePop()
	{
		RectTransform rt = HunterReleaseCountdownText.GetComponent<RectTransform>();
		if (!(rt == null))
		{
			float t = 0f;
			while (t < hunterReleasePopDuration)
			{
				t += Time.deltaTime;
				float t2 = Mathf.Clamp01(t / hunterReleasePopDuration);
				rt.localScale = Vector3.one * Mathf.LerpUnclamped(hunterReleasePopStartScale, 1f, EaseOutBack(t2));
				yield return null;
			}
			rt.localScale = Vector3.one;
			_hunterReleasePopRoutine = null;
		}
	}

	private static float EaseOutBack(float t)
	{
		float num = t - 1f;
		return 1f + 2.70158f * num * num * num + 1.70158f * num * num;
	}

	private void Update()
	{
		if (!_roleApplied)
		{
			TryApplyRoleFromLocalPlayer();
		}
		UpdateVoiceIndicator();
		UpdateShotgunUI();
		if (_topCountdownRect != null)
		{
			Vector2 anchoredPosition = _topCountdownRect.anchoredPosition;
			anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, _topCountdownTargetY, Time.deltaTime * 8f);
			_topCountdownRect.anchoredPosition = anchoredPosition;
			if (_topCountdownCanvasGroup != null)
			{
				bool num = _topCountdownTargetY == 486f;
				float target = (num ? 1f : 0f);
				_topCountdownCanvasGroup.alpha = Mathf.MoveTowards(_topCountdownCanvasGroup.alpha, target, Time.deltaTime * 10f);
				if (!num && _topCountdownCanvasGroup.alpha <= 0.01f && TopCountdownGroup.activeSelf)
				{
					TopCountdownGroup.SetActive(value: false);
				}
			}
		}
		if (_outOfAmmoRect != null)
		{
			Vector2 anchoredPosition2 = _outOfAmmoRect.anchoredPosition;
			anchoredPosition2.y = Mathf.Lerp(anchoredPosition2.y, _outOfAmmoTargetY, Time.deltaTime * 8f);
			_outOfAmmoRect.anchoredPosition = anchoredPosition2;
			if (_outOfAmmoCanvasGroup != null)
			{
				bool num2 = _outOfAmmoTargetY == 300f;
				float target2 = (num2 ? 1f : 0f);
				_outOfAmmoCanvasGroup.alpha = Mathf.MoveTowards(_outOfAmmoCanvasGroup.alpha, target2, Time.deltaTime * 10f);
				if (!num2 && _outOfAmmoCanvasGroup.alpha <= 0.01f && outOfAmmoNotification.activeSelf)
				{
					outOfAmmoNotification.SetActive(value: false);
				}
			}
		}
		if (_waitingForRecordingRect != null)
		{
			Vector2 anchoredPosition3 = _waitingForRecordingRect.anchoredPosition;
			anchoredPosition3.y = Mathf.Lerp(anchoredPosition3.y, _waitingForRecordingTargetY, Time.deltaTime * 8f);
			_waitingForRecordingRect.anchoredPosition = anchoredPosition3;
			if (_waitingForRecordingCanvasGroup != null)
			{
				float target3 = ((_waitingForRecordingTargetY == 0f) ? 1f : 0f);
				_waitingForRecordingCanvasGroup.alpha = Mathf.MoveTowards(_waitingForRecordingCanvasGroup.alpha, target3, Time.deltaTime * 10f);
			}
			if (_waitingForRecordingVisible)
			{
				_waitingForRecordingPulseT += Time.deltaTime * waitingForRecordingPulseSpeed;
				float num3 = 1f + Mathf.Sin(_waitingForRecordingPulseT) * waitingForRecordingPulseAmount;
				_waitingForRecordingRect.localScale = Vector3.one * num3;
				if (_waitingForRecordingLabel != null)
				{
					_waitingForRecordingDotsT += Time.deltaTime * waitingForRecordingDotsPerSecond;
					int count = Mathf.FloorToInt(_waitingForRecordingDotsT) % 4;
					_waitingForRecordingLabel.text = Localization.Get("WAITING_FOR_RECORDING") + new string('.', count);
				}
			}
			else if (_waitingForRecordingRect.localScale != Vector3.one)
			{
				_waitingForRecordingPulseT = 0f;
				_waitingForRecordingDotsT = 0f;
				_waitingForRecordingRect.localScale = Vector3.one;
			}
		}
		if (_topBoxRect != null)
		{
			Vector2 anchoredPosition4 = _topBoxRect.anchoredPosition;
			anchoredPosition4.y = Mathf.Lerp(anchoredPosition4.y, _topBoxTargetY, Time.deltaTime * 8f);
			_topBoxRect.anchoredPosition = anchoredPosition4;
		}
	}

	private void TryApplyRoleFromLocalPlayer()
	{
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (localPlayer == null)
		{
			return;
		}
		PlayerRoleData component = localPlayer.GetComponent<PlayerRoleData>();
		if (!(component == null) && component.RolesLocked)
		{
			ApplyRole(component.Role);
			MyClient component2 = localPlayer.GetComponent<MyClient>();
			if (component2 != null && !string.IsNullOrEmpty(component2.playerInfo.username))
			{
				UpdateProfile(component.Role, component.AssignedAnimal, component2.playerInfo.username);
				_roleApplied = true;
			}
		}
	}

	private void UpdateShotgunUI()
	{
		if (!_roleKnown || _role != PlayerRole.Hunter)
		{
			_outOfAmmoTargetY = 660f;
			return;
		}
		if (_localShotgun == null && NetworkClient.localPlayer != null)
		{
			_localShotgun = NetworkClient.localPlayer.GetComponent<HunterShotgun>();
		}
		if (_localShotgun == null)
		{
			_outOfAmmoTargetY = 660f;
			return;
		}
		bool flag = _localShotgun.Ammo <= 0;
		_outOfAmmoTargetY = (flag ? 300f : 660f);
		if (flag && outOfAmmoNotification != null && !outOfAmmoNotification.activeSelf)
		{
			outOfAmmoNotification.SetActive(value: true);
		}
		bool flag2 = AmmoShack.LocalHunterInShack && _localShotgun.Ammo < _localShotgun.MaxAmmo;
		SetShotgunGroupVisible(_localShotgun.IsAiming || flag2);
		if (shotgunGroup != null && shotgunGroup.activeSelf)
		{
			UpdateShellIcons(_localShotgun.Ammo, _localShotgun.MaxAmmo);
			UpdateShellPulse(_localShotgun.Ammo);
		}
	}

	private void UpdateShellIcons(int ammo, int maxAmmo)
	{
		if (shellIconTemplate == null || (ammo == _lastShownAmmo && maxAmmo == _lastShownMaxAmmo))
		{
			return;
		}
		if (_shellIcons.Count == 0)
		{
			_shellIcons.Add(shellIconTemplate);
			_shellIconImages.Add(shellIconTemplate.GetComponent<Image>());
		}
		while (_shellIcons.Count < maxAmmo)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(shellIconTemplate, shellIconTemplate.transform.parent);
			_shellIcons.Add(gameObject);
			_shellIconImages.Add(gameObject.GetComponent<Image>());
		}
		for (int i = 0; i < _shellIcons.Count; i++)
		{
			GameObject gameObject2 = _shellIcons[i];
			if (!(gameObject2 == null))
			{
				bool flag = i < maxAmmo;
				if (gameObject2.activeSelf != flag)
				{
					gameObject2.SetActive(flag);
				}
				Image image = _shellIconImages[i];
				if (image != null && flag)
				{
					image.color = ((i < ammo) ? shellLoadedColor : shellDepletedColor);
				}
			}
		}
		_lastShownAmmo = ammo;
		_lastShownMaxAmmo = maxAmmo;
	}

	private void UpdateShellPulse(int ammo)
	{
		if (ammo > 0 || AmmoShack.LocalHunterInShack)
		{
			return;
		}
		float t = (Mathf.Sin(Time.time * shellEmptyPulseSpeed) + 1f) * 0.5f;
		Color color = Color.Lerp(shellDepletedColor, shellEmptyPulseColor, t);
		for (int i = 0; i < _shellIconImages.Count; i++)
		{
			Image image = _shellIconImages[i];
			if (image != null)
			{
				image.color = color;
			}
		}
	}

	private void UpdateVoiceIndicator()
	{
		if (!(voiceIndicatorImage == null) && !(_comms == null))
		{
			if (_localState == null && !string.IsNullOrEmpty(_comms.LocalPlayerName))
			{
				_localState = _comms.FindPlayer(_comms.LocalPlayerName);
			}
			float num = ((_localState != null) ? _localState.Amplitude : 0f);
			AudioSettingsManager instance = AudioSettingsManager.Instance;
			bool flag = (instance == null || instance.VoiceMode != CommActivationMode.PushToTalk || Input.GetKey(instance.PushToTalkKey)) && num >= voiceThreshold;
			if (voiceIndicatorImage.activeSelf != flag)
			{
				voiceIndicatorImage.SetActive(flag);
			}
		}
	}

	public void UpdateGameCountdown(float remaining)
	{
		if (gameCountdownText == null)
		{
			return;
		}
		string text = ((remaining < 0f) ? "" : $"{Mathf.CeilToInt(remaining) / 60:00}:{Mathf.CeilToInt(remaining) % 60:00}");
		if (!(text == gameCountdownText.text))
		{
			gameCountdownText.text = text;
			if (!string.IsNullOrEmpty(text))
			{
				gameCountdownPunch?.Play();
			}
		}
	}

	public void UpdateProfile(PlayerRole role, AnimalType animal, string username)
	{
		if (RoleText != null)
		{
			RoleText.text = ((role == PlayerRole.Hunter) ? Localization.Get("ROLE_HUNTER") : Localization.GetAnimalName(animal));
		}
		if (UsernameText != null)
		{
			UsernameText.text = username;
		}
		if (RoleImage != null)
		{
			RoleImage.sprite = ((role == PlayerRole.Hunter) ? hunterSprite : RoleAssignmentUI.Instance.GetAnimalProfileSprite(animal));
		}
	}

	public void SetShotgunGroupVisible(bool visible)
	{
		if (_role == PlayerRole.Hunter && _shotgunGroupVisible != visible)
		{
			_shotgunGroupVisible = visible;
			if (shotgunGroup != null)
			{
				shotgunGroup.SetActive(visible);
			}
		}
	}
}
