using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Mimicraft.Analytics;
using Mimicraft.Dev;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Tutorial;
using Mimicraft.UI;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mimicraft.Networking
{
	public class LobbyView : MonoBehaviour
	{
		private enum NetworkKind
		{
			UnityTransport = 0,
			Steam = 1
		}

		private enum LobbyVisibility
		{
			Public = 0,
			FriendsOnly = 1,
			Private = 2
		}

		private static readonly string[] VisibilityLabels = new string[3] { "Public", "Friends Only", "Invites Only" };

		private const string GameSceneName = "Game";

		private const string MenuSceneName = "Menu";

		public const ushort DefaultPort = 7777;

		private static readonly UnityEngine.Color SelectedColor = new UnityEngine.Color(0.25f, 0.45f, 0.25f, 0.95f);

		private static readonly UnityEngine.Color UnselectedColor = new UnityEngine.Color(0.18f, 0.18f, 0.18f, 0.9f);

		[SerializeField]
		private TMP_Dropdown transportDropdown;

		[SerializeField]
		private TMP_InputField lobbyNameField;

		[SerializeField]
		private TMP_InputField prepSecondsField;

		[SerializeField]
		private TMP_InputField huntSecondsField;

		[SerializeField]
		private TMP_InputField roundEndSecondsField;

		[SerializeField]
		private TMP_InputField minHidersField;

		[SerializeField]
		private TMP_InputField minHuntersField;

		[Tooltip("Zorunlu taunt aralığı, saniye - Modelcilerin kendilerini ele verme sıklığı. Varsayılan 30, sınırlar 10 ile 120. Yazılan sayı bu aralığa kırpılır; boş bırakılırsa 30 kullanılır. Satırına LobbySettingRow ▸ TauntInterval ver.")]
		[SerializeField]
		private TMP_InputField tauntIntervalField;

		[Tooltip("Iskalama cezası çarpanı - Avcı boşa ateş edince silahın kendi cezasının kaç katını yer. x0.25 ile x2 arası, çeyrek adımlarla, varsayılan x1.\n\nSlider'ın Min/Max/Whole Numbers değerleri KODDAN kurulur (bkz. BindSelfDamageSlider); Inspector'da ne bıraktığın önemsiz. Satırına LobbySettingRow ▸ SelfDamage ver.")]
		[SerializeField]
		private Slider selfDamageSlider;

		[Tooltip("Slider'ın yanındaki çarpan yazısı - 'x1', 'x1.25'. İsteğe bağlı; boş bırakılırsa slider sayısız kalır.")]
		[SerializeField]
		private TextMeshProUGUI selfDamageLabel;

		[Tooltip("Round başında kaç kişinin Avcı olacağı - odanın yüzdesi olarak. %10 ile %50 arası, beşer adımlarla, varsayılan %30.\n\nSlider'ın Min/Max/Whole Numbers değerleri KODDAN kurulur (bkz. BindHunterShareSlider); Inspector'da ne bıraktığın önemsiz. Satırına LobbySettingRow ▸ HunterShare ver.")]
		[SerializeField]
		private Slider hunterShareSlider;

		[Tooltip("Slider'ın yanındaki oran yazısı - '%25 · 4 Avcı / 12 Modelci'. Sayılar Max Players alanına göre hesaplanır. İsteğe bağlı; boş bırakılırsa slider sayısız kalır.")]
		[SerializeField]
		private TextMeshProUGUI hunterShareLabel;

		[Tooltip("Lobiye kaç oyuncunun alınacağı. Üst sınır 16'dır ve yazılan daha büyük bir sayı ona kırpılır. Yoksa ya da boşsa 16 kullanılır.")]
		[SerializeField]
		private TMP_InputField maxPlayersField;

		[Header("Deathmatch")]
		[Tooltip("Round'un başlaması için gereken en az oyuncu sayısı. Sayı dolana kadar oyun 'Oyuncular bekleniyor' aşamasında kalır.\n\nHer modda var - satırına LobbySettingRow KOYMA, Max. oyuncu satırı gibi.")]
		[SerializeField]
		private TMP_InputField minPlayersField;

		[Tooltip("Maçı bitiren öldürme sayısı. LobbySettingRow ▸ ScoreLimit.")]
		[SerializeField]
		private TMP_InputField scoreLimitField;

		[Tooltip("Maçın süresi, saniye. LobbySettingRow ▸ TimeLimit.")]
		[SerializeField]
		private TMP_InputField timeLimitField;

		[Tooltip("Yeterli oyuncu toplanınca maça kadar sayılan süre, saniye. LobbySettingRow ▸ ExtraWarmup.")]
		[SerializeField]
		private TMP_InputField extraWarmupField;

		[Tooltip("Vurulan oyuncunun yeniden doğma süresi, saniye. LobbySettingRow ▸ RespawnTime.")]
		[SerializeField]
		private TMP_InputField respawnTimeField;

		[SerializeField]
		private TMP_InputField portField;

		[Tooltip("Lobinin şifresi. Boş bırakılırsa lobi herkese açık olur.\n\nŞifreyi HOST doğruluyor - yanlış şifreyle gelen istemci daha oyuna girmeden reddediliyor. Bilerek hatırlanmıyor: geçen seferden kalmış bir şifre, kimsenin bilmediği bir şifreyle lobi açmak demek olurdu.\n\nContent Type'ı Password yapmak istersen o tamamen görsel bir tercih.")]
		[SerializeField]
		private TMP_InputField passwordField;

		[SerializeField]
		private Button createButton;

		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[Tooltip("Optional. Left unassigned, one is built at runtime under Map Picker Parent (or under this card) - the panel that actually ships is the hand-built one in the Menu scene, which has diverged from what Create() below produces, so nothing here can rely on Create() having made it.")]
		[SerializeField]
		private MapPickerView mapPicker;

		[Tooltip("Menüdeki mod seçici. Bağlıysa, haritası olmayan bir mod seçilince harita seçici kilitlenir. Boş bırakılabilir - o zaman harita seçici her zaman açık kalır.")]
		[SerializeField]
		private GameModePickerView modePicker;

		[Tooltip("Where to build the map picker when Map Picker is empty. Left empty too, it goes directly under this card, just below the title.")]
		[SerializeField]
		private RectTransform mapPickerParent;

		[Tooltip("Steam lobby visibility: Herkese Açık / Sadece Arkadaşlar / Özel. Optional and hand-wired like Transport Dropdown - left unassigned, lobbies are public, which is what they were before this existed. Ignored on Unity Transport, which has no equivalent concept.")]
		[SerializeField]
		private TMP_Dropdown visibilityDropdown;

		[Tooltip("Sunucu tick rate'i: Varsayılan, Yüksek, Ultra. Seçenekleri bu bileşen doldurur, elle yazılmaz. Yoksa konsoldaki `tickrate` komutuyla seçilen değer geçerli olur.")]
		[SerializeField]
		private TMP_Dropdown tickRateDropdown;

		[Tooltip("Varsayılanın üzerinde bir tick rate seçilince görünen uyarı metni. Yoksa uyarı gösterilmez - seçim yine çalışır.")]
		[SerializeField]
		private TextMeshProUGUI tickRateWarningLabel;

		private NetworkKind selectedNetwork;

		private LobbySettingRow[] settingRows;

		private static readonly uint[] TickRates = new uint[3] { 30u, 64u, 128u };

		private static readonly string[] TickRateKeys = new string[3] { "Lobby.TickRate.Default", "Lobby.TickRate.High", "Lobby.TickRate.Ultra" };

		private bool creating;

		private bool hostStarted;

		public static LobbyView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Lobby");
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(340f, 500f);
			rectTransform.gameObject.AddComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(0.16f, 0.16f, 0.16f, 0.98f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(16, 16, 16, 16);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			((RectTransform)UIFactory.CreateLabel(rectTransform, "Title", "Lobi", 20).transform).sizeDelta = new Vector2(308f, 26f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "NetworkRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform2.sizeDelta = new Vector2(308f, 32f);
			float x = (308f - horizontalLayoutGroup.spacing) / 2f;
			((RectTransform)UIFactory.CreateButton(rectTransform2, "UnityTransportToggle", "Unity Transport", out var text).transform).sizeDelta = new Vector2(x, 32f);
			((RectTransform)UIFactory.CreateButton(rectTransform2, "SteamToggle", "Steam", out text).transform).sizeDelta = new Vector2(x, 32f);
			TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform, "LobbyNameField", "Lobby Name");
			tMP_InputField.text = "My Lobby";
			((RectTransform)tMP_InputField.transform).sizeDelta = new Vector2(308f, 30f);
			TMP_InputField tMP_InputField2 = UIFactory.CreateInputField(rectTransform, "PrepSecondsField", "Prep Duration (s)");
			((RectTransform)tMP_InputField2.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField2.text = "60";
			TMP_InputField tMP_InputField3 = UIFactory.CreateInputField(rectTransform, "HuntSecondsField", "Hunt Duration (s)");
			((RectTransform)tMP_InputField3.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField3.text = "300";
			TMP_InputField tMP_InputField4 = UIFactory.CreateInputField(rectTransform, "RoundEndSecondsField", "Round End Wait Duration (s)");
			((RectTransform)tMP_InputField4.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField4.text = "10";
			TMP_InputField tMP_InputField5 = UIFactory.CreateInputField(rectTransform, "MinHidersField", "Min. Modeler count");
			((RectTransform)tMP_InputField5.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField5.text = "1";
			TMP_InputField tMP_InputField6 = UIFactory.CreateInputField(rectTransform, "MinHuntersField", "Min. Hunter Count");
			((RectTransform)tMP_InputField6.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField6.text = "1";
			TMP_InputField tMP_InputField7 = UIFactory.CreateInputField(rectTransform, "PortField", "Port (Unity Transport)");
			((RectTransform)tMP_InputField7.transform).sizeDelta = new Vector2(308f, 30f);
			tMP_InputField7.text = ((ushort)7777).ToString();
			Button button = UIFactory.CreateButton(rectTransform, "CreateButton", "Lobi Oluştur", out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(308f, 36f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Status", "", 13);
			textMeshProUGUI.textWrappingMode = TextWrappingModes.Normal;
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(308f, 60f);
			LobbyView lobbyView = rectTransform.gameObject.AddComponent<LobbyView>();
			lobbyView.lobbyNameField = tMP_InputField;
			lobbyView.prepSecondsField = tMP_InputField2;
			lobbyView.huntSecondsField = tMP_InputField3;
			lobbyView.roundEndSecondsField = tMP_InputField4;
			lobbyView.minHidersField = tMP_InputField5;
			lobbyView.minHuntersField = tMP_InputField6;
			lobbyView.portField = tMP_InputField7;
			lobbyView.createButton = button;
			lobbyView.statusLabel = textMeshProUGUI;
			return lobbyView;
		}

		private void Awake()
		{
			transportDropdown.onValueChanged.RemoveAllListeners();
			transportDropdown.onValueChanged.AddListener(delegate(int index)
			{
				SelectNetwork((index != 0) ? NetworkKind.Steam : NetworkKind.UnityTransport);
			});
			EnsureMapPicker();
			BuildVisibilityDropdown();
			createButton.onClick.RemoveAllListeners();
			createButton.onClick.AddListener(CreateLobby);
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton != null)
			{
				singleton.OnClientConnectedCallback += OnClientConnected;
				singleton.OnClientDisconnectCallback += OnClientDisconnected;
			}
			UpdateNetworkToggleVisuals();
			if (modePicker != null)
			{
				modePicker.SelectionChanged += ApplyMode;
			}
			ApplyMode(CurrentMode());
		}

		private void OnEnable()
		{
			ApplyMode(CurrentMode());
		}

		private GameModeDefinition CurrentMode()
		{
			return GameModeCatalog.Find(SelectedModeId());
		}

		private void Start()
		{
			LobbyPrefs.Bind(transportDropdown, "Transport");
			LobbyPrefs.Bind(lobbyNameField, "Name");
			LobbyPrefs.Bind(prepSecondsField, "PrepSeconds");
			LobbyPrefs.Bind(huntSecondsField, "HuntSeconds");
			LobbyPrefs.Bind(roundEndSecondsField, "RoundEndSeconds");
			LobbyPrefs.Bind(minHidersField, "MinHiders");
			LobbyPrefs.Bind(minHuntersField, "MinHunters");
			LobbyPrefs.Bind(minPlayersField, "MinPlayers");
			LobbyPrefs.Bind(scoreLimitField, "ScoreLimit");
			LobbyPrefs.Bind(timeLimitField, "TimeLimit");
			LobbyPrefs.Bind(extraWarmupField, "ExtraWarmup");
			LobbyPrefs.Bind(respawnTimeField, "RespawnTime");
			LobbyPrefs.Bind(portField, "Port");
			LobbyPrefs.Bind(visibilityDropdown, "Visibility");
			LobbyPrefs.Bind(maxPlayersField, "MaxPlayers");
			LobbyPrefs.Bind(tauntIntervalField, "TauntInterval");
			BindMaxPlayersClamp();
			BindTauntIntervalClamp();
			BindSelfDamageSlider();
			BindHunterShareSlider();
			AttachTooltips();
			BuildTickRateDropdown();
			SelectNetwork((transportDropdown != null && transportDropdown.value != 0) ? NetworkKind.Steam : NetworkKind.UnityTransport);
			if (mapPicker != null)
			{
				mapPicker.TrySelectMapId(LobbyPrefs.GetString("MapId"));
				mapPicker.SelectionChanged += RememberMap;
			}
			if (modePicker != null)
			{
				modePicker.TrySelectModeId(LobbyPrefs.GetString("ModeId"));
				modePicker.SelectionChanged += RememberMode;
			}
		}

		private void RememberMap(string mapId)
		{
			LobbyPrefs.SetString("MapId", mapId);
		}

		private void RememberMode(GameModeDefinition mode)
		{
			LobbyPrefs.SetString("ModeId", (mode != null) ? mode.ModeId : "");
		}

		private void ApplySettingRows(GameModeDefinition mode)
		{
			if (settingRows == null)
			{
				settingRows = GetComponentsInChildren<LobbySettingRow>(includeInactive: true);
			}
			LobbySettingRow[] array = settingRows;
			foreach (LobbySettingRow lobbySettingRow in array)
			{
				if (lobbySettingRow != null)
				{
					lobbySettingRow.Apply(mode);
				}
			}
		}

		private void ApplyMode(GameModeDefinition mode)
		{
			ApplyMapPickerAvailability(mode);
			ApplySettingRows(mode);
			UILayout.RebuildFromDeferred(this, base.transform);
		}

		private void ApplyMapPickerAvailability(GameModeDefinition mode)
		{
			if (!(mapPicker == null))
			{
				bool active = mode == null || mode.UsesMap;
				mapPicker.gameObject.SetActive(active);
			}
		}

		private void OnDestroy()
		{
			if (modePicker != null)
			{
				modePicker.SelectionChanged -= ApplyMode;
				modePicker.SelectionChanged -= RememberMode;
			}
			if (mapPicker != null)
			{
				mapPicker.SelectionChanged -= RememberMap;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton != null)
			{
				singleton.OnClientConnectedCallback -= OnClientConnected;
				singleton.OnClientDisconnectCallback -= OnClientDisconnected;
			}
		}

		private void SelectNetwork(NetworkKind kind)
		{
			selectedNetwork = kind;
			UpdateNetworkToggleVisuals();
		}

		private void UpdateNetworkToggleVisuals()
		{
			bool flag = selectedNetwork == NetworkKind.Steam;
			if (SetShown((portField != null) ? portField.gameObject : null, !flag) | SetShown((visibilityDropdown != null) ? visibilityDropdown.gameObject : null, flag))
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
			}
		}

		private static bool SetShown(GameObject target, bool shown)
		{
			if (target == null || target.activeSelf == shown)
			{
				return false;
			}
			target.SetActive(shown);
			return true;
		}

		private LobbySettingsData CollectSettings()
		{
			return new LobbySettingsData
			{
				LobbyName = (string.IsNullOrWhiteSpace(lobbyNameField.text) ? "Lobby" : lobbyNameField.text),
				PrepSeconds = ParseIntOrDefault(prepSecondsField.text, 60),
				HuntSeconds = ParseIntOrDefault(huntSecondsField.text, 300),
				RoundEndSeconds = ParseIntOrDefault(roundEndSecondsField.text, 10),
				MinHiders = ParseIntOrDefault(minHidersField.text, 1),
				MinHunters = ParseIntOrDefault(minHuntersField.text, 1),
				TauntIntervalSeconds = TauntIntervalSetting(),
				SelfDamagePercent = SelfDamageSetting(),
				HunterSharePercent = HunterShareSetting(),
				MaxPlayers = LobbyCapacity.Clamp(ParseIntOrDefault(TextOf(maxPlayersField), 16)),
				MinPlayers = ParseIntOrDefault(TextOf(minPlayersField), 0),
				ScoreLimit = ParseIntOrDefault(TextOf(scoreLimitField), 0),
				TimeLimitSeconds = ParseIntOrDefault(TextOf(timeLimitField), 0),
				ExtraWarmupSeconds = ParseIntOrDefault(TextOf(extraWarmupField), 0),
				RespawnSeconds = ParseIntOrDefault(TextOf(respawnTimeField), 0),
				MapId = SelectedMapId(),
				ModeId = SelectedModeId(),
				HasPassword = !string.IsNullOrEmpty(Password())
			};
		}

		private static string TextOf(TMP_InputField field)
		{
			if (!(field != null))
			{
				return "";
			}
			return field.text;
		}

		private void BindMaxPlayersClamp()
		{
			if (!(maxPlayersField == null))
			{
				maxPlayersField.onEndEdit.AddListener(delegate
				{
					ClampMaxPlayersField(save: true);
				});
				ClampMaxPlayersField(save: false);
			}
		}

		private void BindSelfDamageSlider()
		{
			if (!(selfDamageSlider == null))
			{
				selfDamageSlider.wholeNumbers = true;
				selfDamageSlider.minValue = SelfDamageStep(25);
				selfDamageSlider.maxValue = SelfDamageStep(200);
				int percent = GameModeController.ResolveSelfDamagePercent(LobbyPrefs.GetInt("SelfDamage", 100));
				selfDamageSlider.SetValueWithoutNotify(SelfDamageStep(percent));
				RefreshSelfDamageLabel();
				selfDamageSlider.onValueChanged.AddListener(delegate
				{
					RefreshSelfDamageLabel();
					LobbyPrefs.SetInt("SelfDamage", SelfDamageSetting());
				});
			}
		}

		private static int SelfDamageStep(int percent)
		{
			return percent / 25;
		}

		private void BindHunterShareSlider()
		{
			if (hunterShareSlider == null)
			{
				return;
			}
			hunterShareSlider.wholeNumbers = true;
			hunterShareSlider.minValue = HunterShareStep(5);
			hunterShareSlider.maxValue = HunterShareStep(95);
			int percent = GameModeController.ResolveHunterSharePercent(LobbyPrefs.GetInt("HunterShare", 30));
			hunterShareSlider.SetValueWithoutNotify(HunterShareStep(percent));
			RefreshHunterShareLabel();
			hunterShareSlider.onValueChanged.AddListener(delegate
			{
				RefreshHunterShareLabel();
				LobbyPrefs.SetInt("HunterShare", HunterShareSetting());
			});
			if (maxPlayersField != null)
			{
				maxPlayersField.onEndEdit.AddListener(delegate
				{
					RefreshHunterShareLabel();
				});
			}
		}

		private static int HunterShareStep(int percent)
		{
			return percent / 5;
		}

		private int HunterShareSetting()
		{
			if (!(hunterShareSlider == null))
			{
				return GameModeController.ResolveHunterSharePercent(Mathf.RoundToInt(hunterShareSlider.value) * 5);
			}
			return 30;
		}

		private void RefreshHunterShareLabel()
		{
			if (hunterShareLabel != null)
			{
				hunterShareLabel.text = GameModeController.HunterShareLabel(HunterShareSetting(), LobbyCapacity.Clamp(ParseIntOrDefault(TextOf(maxPlayersField), 16)));
			}
		}

		private int SelfDamageSetting()
		{
			if (!(selfDamageSlider == null))
			{
				return GameModeController.ResolveSelfDamagePercent(Mathf.RoundToInt(selfDamageSlider.value) * 25);
			}
			return 100;
		}

		private void RefreshSelfDamageLabel()
		{
			if (selfDamageLabel != null)
			{
				selfDamageLabel.text = GameModeController.SelfDamageLabel(SelfDamageSetting());
			}
		}

		private void AttachTooltips()
		{
			UITooltipTrigger.AttachKey(transportDropdown, "Tooltip.Lobby.Transport");
			UITooltipTrigger.AttachKey(lobbyNameField, "Tooltip.Lobby.Name");
			UITooltipTrigger.AttachKey(passwordField, "Tooltip.Lobby.Password");
			UITooltipTrigger.AttachKey(prepSecondsField, "Tooltip.Lobby.Prep");
			UITooltipTrigger.AttachKey(huntSecondsField, "Tooltip.Lobby.Hunt");
			UITooltipTrigger.AttachKey(roundEndSecondsField, "Tooltip.Lobby.RoundEnd");
			UITooltipTrigger.AttachKey(minHidersField, "Tooltip.Lobby.MinModelers");
			UITooltipTrigger.AttachKey(minHuntersField, "Tooltip.Lobby.MinHunters");
			UITooltipTrigger.AttachKey(tauntIntervalField, "Tooltip.Lobby.TauntInterval");
			UITooltipTrigger.AttachKey(selfDamageSlider, "Tooltip.Lobby.SelfDamage");
			UITooltipTrigger.AttachKey(hunterShareSlider, "Tooltip.Lobby.HunterShare");
			UITooltipTrigger.AttachKey(maxPlayersField, "Tooltip.Lobby.MaxPlayers");
			UITooltipTrigger.AttachKey(minPlayersField, "Tooltip.Lobby.MinPlayers");
			UITooltipTrigger.AttachKey(scoreLimitField, "Tooltip.Lobby.ScoreLimit");
			UITooltipTrigger.AttachKey(timeLimitField, "Tooltip.Lobby.TimeLimit");
			UITooltipTrigger.AttachKey(extraWarmupField, "Tooltip.Lobby.Warmup");
			UITooltipTrigger.AttachKey(respawnTimeField, "Tooltip.Lobby.Respawn");
			UITooltipTrigger.AttachKey(portField, "Tooltip.Lobby.Port");
			UITooltipTrigger.AttachKey(visibilityDropdown, "Tooltip.Lobby.Visibility");
			UITooltipTrigger.AttachKey(tickRateDropdown, "Tooltip.Lobby.TickRate");
		}

		private void BindTauntIntervalClamp()
		{
			if (!(tauntIntervalField == null))
			{
				tauntIntervalField.onEndEdit.AddListener(delegate
				{
					WriteTauntIntervalField(RoundManager.ClampTauntInterval(TypedTauntInterval()), save: true);
				});
				WriteTauntIntervalField(RoundManager.ResolveTauntInterval(TypedTauntInterval()), save: false);
			}
		}

		private int TypedTauntInterval()
		{
			return ParseIntOrDefault(TextOf(tauntIntervalField), 0);
		}

		private int TauntIntervalSetting()
		{
			int num = TypedTauntInterval();
			if (num > 0)
			{
				return RoundManager.ClampTauntInterval(num);
			}
			return 30;
		}

		private void WriteTauntIntervalField(int seconds, bool save)
		{
			string text = tauntIntervalField.text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				string text2 = seconds.ToString(CultureInfo.InvariantCulture);
				if (text2 != text)
				{
					tauntIntervalField.SetTextWithoutNotify(text2);
				}
				if (save)
				{
					LobbyPrefs.SetString("TauntInterval", text2);
				}
			}
		}

		private void ClampMaxPlayersField(bool save)
		{
			string text = maxPlayersField.text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				string text2 = LobbyCapacity.Clamp(ParseIntOrDefault(text, 16)).ToString(CultureInfo.InvariantCulture);
				if (text2 != text)
				{
					maxPlayersField.SetTextWithoutNotify(text2);
				}
				if (save)
				{
					LobbyPrefs.SetString("MaxPlayers", text2);
				}
			}
		}

		private void BuildTickRateDropdown()
		{
			if (tickRateDropdown == null)
			{
				RefreshTickRateWarning();
				return;
			}
			List<string> list = new List<string>(TickRateKeys.Length);
			string[] tickRateKeys = TickRateKeys;
			foreach (string key in tickRateKeys)
			{
				list.Add(Loc.Get(key));
			}
			tickRateDropdown.ClearOptions();
			tickRateDropdown.AddOptions(list);
			tickRateDropdown.SetValueWithoutNotify(NearestTickRateIndex(ServerTickRate.Preferred));
			tickRateDropdown.onValueChanged.RemoveListener(OnTickRateChanged);
			tickRateDropdown.onValueChanged.AddListener(OnTickRateChanged);
			RefreshTickRateWarning();
		}

		private static int NearestTickRateIndex(uint rate)
		{
			int num = 0;
			for (int i = 0; i < TickRates.Length; i++)
			{
				if (TickRates[i] == rate)
				{
					return i;
				}
				if (Mathf.Abs((int)(TickRates[i] - rate)) < Mathf.Abs((int)(TickRates[num] - rate)))
				{
					num = i;
				}
			}
			return num;
		}

		private void OnTickRateChanged(int index)
		{
			ServerTickRate.Preferred = TickRates[Mathf.Clamp(index, 0, TickRates.Length - 1)];
			RefreshTickRateWarning();
		}

		private void RefreshTickRateWarning()
		{
			if (!(tickRateWarningLabel == null))
			{
				bool flag = ServerTickRate.Preferred > 30;
				tickRateWarningLabel.text = (flag ? Loc.Get("Lobby.TickRateWarning") : "");
				if (tickRateWarningLabel.gameObject.activeSelf != flag)
				{
					tickRateWarningLabel.gameObject.SetActive(flag);
				}
			}
		}

		private string SelectedMapId()
		{
			if (mapPicker != null && !string.IsNullOrEmpty(mapPicker.SelectedMapId))
			{
				return mapPicker.SelectedMapId;
			}
			MapScriptableObject mapScriptableObject = MapCatalog.Default;
			if (!(mapScriptableObject != null))
			{
				return "";
			}
			return mapScriptableObject.MapId;
		}

		private static string SelectedModeId()
		{
			if (!string.IsNullOrEmpty(GameModeSelection.SelectedModeId))
			{
				return GameModeSelection.SelectedModeId;
			}
			GameModeDefinition gameModeDefinition = GameModeCatalog.Default;
			if (!(gameModeDefinition != null))
			{
				return "";
			}
			return gameModeDefinition.ModeId;
		}

		private void EnsureMapPicker()
		{
			if (mapPicker != null)
			{
				return;
			}
			Transform parent = ((mapPickerParent != null) ? mapPickerParent : base.transform);
			mapPicker = MapPickerView.Create(parent);
			if (mapPickerParent != null)
			{
				return;
			}
			mapPicker.transform.SetSiblingIndex(1);
			if (!TryGetComponent<ContentSizeFitter>(out var component) || component.verticalFit == ContentSizeFitter.FitMode.Unconstrained)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				RectTransform rectTransform2 = mapPicker.transform as RectTransform;
				if (rectTransform != null && rectTransform2 != null)
				{
					VerticalLayoutGroup component2;
					float num = (TryGetComponent<VerticalLayoutGroup>(out component2) ? component2.spacing : 0f);
					rectTransform.sizeDelta += new Vector2(0f, rectTransform2.sizeDelta.y + num);
				}
			}
		}

		private string Password()
		{
			if (!(passwordField != null))
			{
				return "";
			}
			return passwordField.text;
		}

		private static int ParseIntOrDefault(string text, int fallback)
		{
			if (!int.TryParse(text, out var result))
			{
				return fallback;
			}
			return result;
		}

		public async void CreateLobby()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null)
			{
				SetStatus(Loc.Get("Status.NetworkManagerMissing"));
				return;
			}
			Telemetry.Send("menu_action", ("menu_item", TutorialLaunch.Requested ? "tutorial" : ((SelectedModeId() == "practice") ? "practice" : "host")), ("game_mode", SelectedModeId().ToString()), ("transport", selectedNetwork.ToString().ToLowerInvariant()), ("session_seconds", Telemetry.SessionSeconds));
			if (creating || hostStarted)
			{
				Debug.LogWarning("[Oturum] Lobi zaten olusturuluyor ya da oturum baslatildi - ikinci istek yok sayildi. Butonun On Click listesinde iki giris olabilir.");
				return;
			}
			creating = true;
			try
			{
				await CreateLobbyInternal(singleton);
			}
			finally
			{
				creating = false;
			}
		}

		private async Task CreateLobbyInternal(NetworkManager manager)
		{
			LobbySettingsData data = CollectSettings();
			LobbyCapacity.Host(data.MaxPlayers);
			await HostPort.ReleaseLocalServers();
			if (selectedNetwork == NetworkKind.Steam)
			{
				if (!SteamManager.IsInitialized)
				{
					SetStatus(Loc.Get("Status.SteamNotRunning"));
					return;
				}
				FacepunchTransport facepunchTransport = manager.GetComponent<FacepunchTransport>();
				if (facepunchTransport == null)
				{
					SetStatus(Loc.Get("Status.FacepunchMissing"));
					return;
				}
				SetStatus(Loc.Get("Status.CreatingSteamLobby"));
				Lobby? lobby = await SteamMatchmaking.CreateLobbyAsync(LobbyCapacity.Hosted);
				if (!lobby.HasValue)
				{
					SetStatus(Loc.Get("Status.LobbyCreateFailed"));
					return;
				}
				Lobby value = lobby.Value;
				ApplyVisibility(value);
				SteamLobbyData.Write(value, data);
				SteamManager.ActiveLobby = value;
				manager.NetworkConfig.NetworkTransport = facepunchTransport;
				SetStatus($"Lobby created - Lobby ID: {value.Id}. Share this ID with your friend.");
			}
			else
			{
				UnityTransport component = manager.GetComponent<UnityTransport>();
				if (component == null)
				{
					SetStatus(Loc.Get("Status.UnityTransportMissing"));
					return;
				}
				SteamManager.ActiveLobby = null;
				ushort result;
				ushort num = (ushort)(ushort.TryParse(portField.text, out result) ? result : 7777);
				ushort num2 = HostPort.FindFree(num);
				if (num2 == 0)
				{
					SetStatus(Loc.Format("Status.NoFreePort", num, num + 64));
					return;
				}
				if (num2 != num)
				{
					SetStatus(Loc.Format("Status.PortMoved", num, num2));
					Debug.LogWarning($"{num} portu başka bir uygulamada; sunucu {num2} portunda açıldı.");
					if (portField != null)
					{
						portField.text = num2.ToString();
					}
				}
				component.SetConnectionData("0.0.0.0", num2);
				manager.NetworkConfig.NetworkTransport = component;
				SetStatus(Loc.Get("Status.CreatingServer"));
				LanBeaconBroadcaster lanBeaconBroadcaster = UnityEngine.Object.FindFirstObjectByType<LanBeaconBroadcaster>();
				if (lanBeaconBroadcaster != null && !TutorialLaunch.Requested)
				{
					lanBeaconBroadcaster.StartBroadcasting(data, num2);
				}
			}
			DevCheats.SetSessionPolicy(selectedNetwork != NetworkKind.Steam || SelectedVisibility() != LobbyVisibility.Public);
			LobbySettingsSync.PendingSettings = data;
			Debug.Log($"[Oturum] Ayarlar devredildi: mod='{data.ModeId}' harita='{data.MapId}'.");
			LobbyPassword.Host(manager, Password());
			LoadingScreen.Show("Loading.StartingServer");
			Scene own = base.gameObject.scene;
			if (own.IsValid() && own.isLoaded && SceneManager.GetActiveScene() != own)
			{
				SceneManager.SetActiveScene(own);
			}
			await SessionScenes.ReleaseExtras(own);
			hostStarted = true;
			ServerTickRate.ApplyForHosting(manager);
			if (!manager.StartHost())
			{
				hostStarted = false;
				LoadingScreen.Hide();
				SteamManager.LeaveActiveLobby();
				LanBeaconBroadcaster lanBeaconBroadcaster2 = UnityEngine.Object.FindFirstObjectByType<LanBeaconBroadcaster>();
				if (lanBeaconBroadcaster2 != null)
				{
					lanBeaconBroadcaster2.StopBroadcasting();
				}
				SetStatus(Loc.Get("Status.HostFailed"));
				return;
			}
			if (own.IsValid() && own.isLoaded && SceneManager.GetActiveScene() != own)
			{
				SceneManager.SetActiveScene(own);
			}
			LogSceneState();
			try
			{
				manager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				LoadingScreen.Hide();
				manager.Shutdown();
				SteamManager.LeaveActiveLobby();
				LanBeaconBroadcaster lanBeaconBroadcaster3 = UnityEngine.Object.FindFirstObjectByType<LanBeaconBroadcaster>();
				if (lanBeaconBroadcaster3 != null)
				{
					lanBeaconBroadcaster3.StopBroadcasting();
				}
				hostStarted = false;
				SetStatus(Loc.Get("Status.HostFailed"));
				if (!own.IsValid() || !own.isLoaded)
				{
					SceneManager.LoadScene("Menu", LoadSceneMode.Single);
				}
			}
		}

		private static void LogSceneState()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (SceneManager.sceneCount == 1 && SceneManager.GetSceneAt(0) == activeScene)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Oturum] Sahne yuklemesi oncesi. Aktif='" + activeScene.name + "' " + $"(build {activeScene.buildIndex}, handle {activeScene.handle}). Yuklu {SceneManager.sceneCount}: ");
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				stringBuilder.Append($"'{sceneAt.name}'(build {sceneAt.buildIndex}, handle {sceneAt.handle}, " + $"loaded {sceneAt.isLoaded})");
				if (i < SceneManager.sceneCount - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			Debug.Log(stringBuilder.ToString());
		}

		private void BuildVisibilityDropdown()
		{
			if (!(visibilityDropdown == null))
			{
				visibilityDropdown.ClearOptions();
				visibilityDropdown.AddOptions(new List<string>(VisibilityLabels));
				visibilityDropdown.value = 0;
				visibilityDropdown.RefreshShownValue();
			}
		}

		private LobbyVisibility SelectedVisibility()
		{
			if (TutorialLaunch.Requested)
			{
				return LobbyVisibility.Private;
			}
			if (visibilityDropdown == null)
			{
				return LobbyVisibility.Public;
			}
			return (LobbyVisibility)Mathf.Clamp(visibilityDropdown.value, 0, VisibilityLabels.Length - 1);
		}

		private void ApplyVisibility(Lobby lobby)
		{
			switch (SelectedVisibility())
			{
			case LobbyVisibility.FriendsOnly:
				lobby.SetFriendsOnly();
				break;
			case LobbyVisibility.Private:
				lobby.SetPrivate();
				break;
			default:
				lobby.SetPublic();
				break;
			}
			lobby.SetJoinable(b: true);
		}

		private void OnClientConnected(ulong clientId)
		{
			if (NetworkManager.Singleton != null && clientId == NetworkManager.Singleton.LocalClientId)
			{
				SetStatus(Loc.Get("Status.Connected"));
			}
		}

		private void OnClientDisconnected(ulong clientId)
		{
			if (NetworkManager.Singleton != null && clientId == NetworkManager.Singleton.LocalClientId)
			{
				SetStatus(Loc.Get("Status.Disconnected"));
			}
		}

		private void SetStatus(string message)
		{
			statusLabel.text = message;
		}
	}
}
