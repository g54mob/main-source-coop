using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.Networking
{
	public class LobbyBrowserView : MonoBehaviour
	{
		private enum FilterKind
		{
			Steam = 0,
			Lan = 1,
			Internet = 2
		}

		private enum SteamScope
		{
			Public = 0,
			FriendsOnly = 1
		}

		private const int MaxSteamResults = 50;

		private static readonly UnityEngine.Color SelectedColor = new UnityEngine.Color(0.25f, 0.45f, 0.25f, 0.95f);

		private static readonly UnityEngine.Color UnselectedColor = new UnityEngine.Color(0.18f, 0.18f, 0.18f, 0.9f);

		[SerializeField]
		private GameObject resultTemplate;

		[SerializeField]
		private TMP_Dropdown transportDropdown;

		[SerializeField]
		private TMP_Dropdown steamScopeDropdown;

		[SerializeField]
		private TMP_InputField nameFilterField;

		[Tooltip("Oyun moduna göre süzme. İlk seçenek 'Tümü' olacak şekilde katalogdan doldurulur. Bos birakilirsa mod filtresi hic uygulanmaz.")]
		[SerializeField]
		private TMP_Dropdown modeFilterDropdown;

		[SerializeField]
		private Button scanButton;

		[SerializeField]
		private RectTransform resultsContainer;

		[SerializeField]
		private TextMeshProUGUI joinSectionLabel;

		[SerializeField]
		private TMP_InputField joinTargetField;

		[SerializeField]
		private Button joinButton;

		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[SerializeField]
		private LanBeaconListener lanListener;

		[Header("Detay ve şifre")]
		[Tooltip("Satırdaki Detaylar butonunun açacağı panel. Boş bırakılırsa satırlarda Detaylar butonu hiç görünmez - tarayıcı eskisi gibi sadece Katıl'dan ibaret olur.")]
		[SerializeField]
		private LobbyDetailsPanelView detailsPanel;

		[Tooltip("Şifreli bir lobiye katılmaya çalışınca açılacak panel. Boş bırakılırsa şifreli bir lobiye katılmak mümkün olmaz - oyuncunun şifreyi girecek yeri olmadığı için deneme yapılmadan durum satırında söylenir.")]
		private string modeFilterId = "";

		private readonly List<string> modeFilterIds = new List<string>();

		private FilterKind selectedFilter;

		private SteamScope steamScope;

		private readonly List<GameObject> resultRows = new List<GameObject>();

		private const float LanPollInterval = 0.5f;

		private float nextLanPollTime;

		private int lanResultSignature;

		private readonly HashSet<SteamId> shownLobbyIds = new HashSet<SteamId>();

		private readonly Dictionary<SteamId, string> awaitingLobbyData = new Dictionary<SteamId, string>();

		private string pendingFilter;

		private Action retryJoin;

		public static LobbyBrowserView Create(Transform parent, LanBeaconListener lanListener)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "LobbyBrowser");
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(340f, 600f);
			rectTransform.gameObject.AddComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(0.16f, 0.16f, 0.16f, 0.98f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(16, 16, 16, 16);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			((RectTransform)UIFactory.CreateLabel(rectTransform, "Title", Loc.Get("LobbyBrowser.Title"), 20).transform).sizeDelta = new Vector2(308f, 26f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "FilterRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 6f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform2.sizeDelta = new Vector2(308f, 30f);
			float x = (308f - horizontalLayoutGroup.spacing * 2f) / 3f;
			((RectTransform)UIFactory.CreateButton(rectTransform2, "SteamFilterToggle", "Steam", out var text).transform).sizeDelta = new Vector2(x, 30f);
			((RectTransform)UIFactory.CreateButton(rectTransform2, "LanFilterToggle", "LAN", out text).transform).sizeDelta = new Vector2(x, 30f);
			((RectTransform)UIFactory.CreateButton(rectTransform2, "InternetFilterToggle", "Internet", out text).transform).sizeDelta = new Vector2(x, 30f);
			BuildSteamScopeRow(rectTransform, 308f, out var _, out var _);
			TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform, "NameFilterField", Loc.Get("LobbyBrowser.SearchByName"));
			((RectTransform)tMP_InputField.transform).sizeDelta = new Vector2(308f, 30f);
			Button button = UIFactory.CreateButton(rectTransform, "ScanButton", Loc.Get("LobbyBrowser.Scan"), out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(308f, 32f);
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "Results");
			VerticalLayoutGroup verticalLayoutGroup2 = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 4f;
			verticalLayoutGroup2.childControlWidth = false;
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childForceExpandWidth = false;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup2.childAlignment = TextAnchor.UpperCenter;
			rectTransform3.sizeDelta = new Vector2(308f, 280f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "JoinHeading", "");
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(308f, 20f);
			RectTransform rectTransform4 = UIFactory.CreateRect(rectTransform, "JoinRow");
			HorizontalLayoutGroup horizontalLayoutGroup2 = rectTransform4.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.spacing = 8f;
			horizontalLayoutGroup2.childControlWidth = false;
			horizontalLayoutGroup2.childControlHeight = false;
			horizontalLayoutGroup2.childForceExpandWidth = false;
			horizontalLayoutGroup2.childForceExpandHeight = false;
			rectTransform4.sizeDelta = new Vector2(308f, 30f);
			float x2 = 215.59999f - horizontalLayoutGroup2.spacing * 0.5f;
			float x3 = 92.4f - horizontalLayoutGroup2.spacing * 0.5f;
			TMP_InputField tMP_InputField2 = UIFactory.CreateInputField(rectTransform4, "JoinTargetField", "Lobi ID / ip:port");
			((RectTransform)tMP_InputField2.transform).sizeDelta = new Vector2(x2, 30f);
			Button button2 = UIFactory.CreateButton(rectTransform4, "JoinButton", "Katıl", out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(x3, 30f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "Status", "", 13);
			textMeshProUGUI2.textWrappingMode = TextWrappingModes.Normal;
			((RectTransform)textMeshProUGUI2.transform).sizeDelta = new Vector2(308f, 40f);
			LobbyBrowserView lobbyBrowserView = rectTransform.gameObject.AddComponent<LobbyBrowserView>();
			lobbyBrowserView.nameFilterField = tMP_InputField;
			lobbyBrowserView.scanButton = button;
			lobbyBrowserView.resultsContainer = rectTransform3;
			lobbyBrowserView.joinSectionLabel = textMeshProUGUI;
			lobbyBrowserView.joinTargetField = tMP_InputField2;
			lobbyBrowserView.joinButton = button2;
			lobbyBrowserView.statusLabel = textMeshProUGUI2;
			lobbyBrowserView.lanListener = lanListener;
			return lobbyBrowserView;
		}

		public void SetLanListener(LanBeaconListener lanListener)
		{
			this.lanListener = lanListener;
		}

		public void EnsureJoinSection()
		{
			if (!(joinSectionLabel != null) || !(joinTargetField != null) || !(joinButton != null))
			{
				Transform transform = base.transform.Find("Status");
				Transform transform2 = ((transform != null) ? transform.parent : base.transform);
				int siblingIndex = ((transform != null) ? transform.GetSiblingIndex() : transform2.childCount);
				TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(transform2, "JoinHeading", "");
				((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(308f, 20f);
				textMeshProUGUI.transform.SetSiblingIndex(siblingIndex++);
				RectTransform rectTransform = UIFactory.CreateRect(transform2, "JoinRow");
				HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
				horizontalLayoutGroup.spacing = 8f;
				horizontalLayoutGroup.childControlWidth = false;
				horizontalLayoutGroup.childControlHeight = false;
				horizontalLayoutGroup.childForceExpandWidth = false;
				horizontalLayoutGroup.childForceExpandHeight = false;
				rectTransform.sizeDelta = new Vector2(308f, 30f);
				rectTransform.SetSiblingIndex(siblingIndex);
				float x = 215.59999f - horizontalLayoutGroup.spacing * 0.5f;
				float x2 = 92.4f - horizontalLayoutGroup.spacing * 0.5f;
				TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform, "JoinTargetField", "Lobi ID / ip:port");
				((RectTransform)tMP_InputField.transform).sizeDelta = new Vector2(x, 30f);
				TextMeshProUGUI text;
				Button button = UIFactory.CreateButton(rectTransform, "JoinButton", "Katıl", out text);
				((RectTransform)button.transform).sizeDelta = new Vector2(x2, 30f);
				joinSectionLabel = textMeshProUGUI;
				joinTargetField = tMP_InputField;
				joinButton = button;
				joinButton.onClick.RemoveAllListeners();
				joinButton.onClick.AddListener(JoinManual);
				UpdateFilterVisuals();
			}
		}

		private static RectTransform BuildSteamScopeRow(Transform parent, float contentWidth, out Button publicToggle, out Button friendsToggle)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "SteamScopeRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 6f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform.sizeDelta = new Vector2(contentWidth, 26f);
			float x = (contentWidth - horizontalLayoutGroup.spacing) * 0.5f;
			publicToggle = UIFactory.CreateButton(rectTransform, "SteamScopePublicToggle", Loc.Get("LobbyBrowser.Public"), out var text);
			((RectTransform)publicToggle.transform).sizeDelta = new Vector2(x, 26f);
			friendsToggle = UIFactory.CreateButton(rectTransform, "SteamScopeFriendsToggle", Loc.Get("LobbyBrowser.FriendsOnly"), out text);
			((RectTransform)friendsToggle.transform).sizeDelta = new Vector2(x, 26f);
			return rectTransform;
		}

		private void Awake()
		{
			transportDropdown.onValueChanged.RemoveAllListeners();
			transportDropdown.onValueChanged.AddListener(delegate
			{
				SelectFilter(FilterKindFor(transportDropdown));
			});
			steamScopeDropdown.onValueChanged.RemoveAllListeners();
			steamScopeDropdown.onValueChanged.AddListener(delegate(int index)
			{
				if (index == 0)
				{
					SelectSteamScope(SteamScope.Public);
				}
				else
				{
					SelectSteamScope(SteamScope.FriendsOnly);
				}
			});
			BuildModeFilterDropdown();
			scanButton.onClick.RemoveAllListeners();
			scanButton.onClick.AddListener(Scan);
			joinButton.onClick.RemoveAllListeners();
			joinButton.onClick.AddListener(JoinManual);
			UpdateFilterVisuals();
		}

		private void Start()
		{
			LobbyPrefs.Bind(transportDropdown, "Browser.Transport");
			LobbyPrefs.Bind(steamScopeDropdown, "Browser.SteamScope");
			LobbyPrefs.Bind(nameFilterField, "Browser.NameFilter");
			LobbyPrefs.Bind(joinTargetField, "Browser.JoinTarget");
			RestoreModeFilter(LobbyPrefs.GetString("Browser.ModeFilter"));
			steamScope = ((steamScopeDropdown != null && steamScopeDropdown.value != 0) ? SteamScope.FriendsOnly : SteamScope.Public);
			SelectFilter(FilterKindFor(transportDropdown));
		}

		private static FilterKind FilterKindFor(TMP_Dropdown dropdown)
		{
			if (dropdown == null || dropdown.value < 0 || dropdown.value >= dropdown.options.Count)
			{
				return FilterKind.Steam;
			}
			string text = dropdown.options[dropdown.value].text;
			if (text == "Steam")
			{
				return FilterKind.Steam;
			}
			if (!(text == "LAN"))
			{
				return FilterKind.Internet;
			}
			return FilterKind.Lan;
		}

		private string ModeFilterIdAt(int index)
		{
			if (index < 0 || index >= modeFilterIds.Count)
			{
				return "";
			}
			return modeFilterIds[index];
		}

		private void RestoreModeFilter(string modeId)
		{
			int num = modeFilterIds.IndexOf(modeId);
			if (num < 0)
			{
				num = 0;
			}
			modeFilterId = ModeFilterIdAt(num);
			if (modeFilterDropdown != null && num < modeFilterDropdown.options.Count)
			{
				modeFilterDropdown.SetValueWithoutNotify(num);
			}
		}

		private void SelectFilter(FilterKind kind)
		{
			if (selectedFilter == FilterKind.Lan && kind != FilterKind.Lan)
			{
				lanListener.StopListening();
			}
			selectedFilter = kind;
			UpdateFilterVisuals();
			ClearResults();
			steamScopeDropdown.gameObject.SetActive(kind == FilterKind.Steam);
			switch (kind)
			{
			case FilterKind.Lan:
				lanListener.StartListening();
				lanResultSignature = LanSignature();
				nextLanPollTime = 0f;
				break;
			case FilterKind.Internet:
				SetStatus("Otomatik tarama yok - aşağıdaki Doğrudan Bağlantı'dan ip:port girip yine de bağlanabilirsin.");
				break;
			default:
				SetStatus("");
				break;
			}
			Scan();
		}

		private void SelectSteamScope(SteamScope scope)
		{
			steamScope = scope;
			UpdateFilterVisuals();
			ClearResults();
			SetStatus((scope == SteamScope.FriendsOnly) ? "Sadece bu oyunda/lobide olan arkadaşların gösterilecek - Tara'ya bas." : "");
			Scan();
		}

		private void Update()
		{
			if (selectedFilter == FilterKind.Lan && !(lanListener == null) && !(Time.unscaledTime < nextLanPollTime))
			{
				nextLanPollTime = Time.unscaledTime + 0.5f;
				int num = LanSignature();
				if (num != lanResultSignature)
				{
					lanResultSignature = num;
					Scan();
				}
			}
		}

		private int LanSignature()
		{
			int num = 0;
			foreach (LanLobbyInfo lobby in lanListener.GetLobbies())
			{
				num ^= ((lobby.Ip != null) ? lobby.Ip.GetHashCode() : 0) * 31 + lobby.Port + lobby.Settings.LobbyName.GetHashCode();
			}
			return num;
		}

		private void UpdateFilterVisuals()
		{
			joinTargetField.placeholder.GetComponent<TextMeshProUGUI>().text = ((selectedFilter == FilterKind.Steam) ? "Steam Lobby ID" : $"IP  (port boşsa {(ushort)7777})");
			joinSectionLabel.text = ((selectedFilter == FilterKind.Steam) ? "Join Steam Lobby" : "Direct Connect");
		}

		private async void Scan()
		{
			if (selectedFilter == FilterKind.Internet)
			{
				SetStatus("Otomatik tarama yok - aşağıdaki Doğrudan Bağlantı'dan ip:port girip yine de bağlanabilirsin.");
				return;
			}
			ClearResults();
			string filter = nameFilterField.text;
			if (selectedFilter == FilterKind.Steam)
			{
				if (!SteamManager.IsInitialized)
				{
					SetStatus(Loc.Get("Status.SteamNotRunning"));
					return;
				}
				if (steamScope == SteamScope.FriendsOnly)
				{
					ScanFriendsOnly(filter);
					return;
				}
				SetStatus(Loc.Get("Status.ScanningSteamLobbies"));
				Lobby[] array = await SteamMatchmaking.LobbyList.WithMaxResults(50).RequestAsync();
				if (array == null)
				{
					SetStatus(Loc.Get("Status.NoLobbiesFound"));
					return;
				}
				shownLobbyIds.Clear();
				int num = 0;
				Lobby[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Lobby lobby = array2[i];
					string data = lobby.GetData("name");
					if (PassesNameFilter(data, filter) && shownLobbyIds.Add(lobby.Id))
					{
						AddSteamRow(lobby, string.IsNullOrEmpty(data) ? Loc.Get("LobbyBrowser.Unnamed") : data);
						num++;
					}
				}
				num += ScanFriends(filter);
				SetStatus((num == 0) ? "Uygun lobi bulunamadı." : $"{num} lobi bulundu.");
				return;
			}
			int num2 = 0;
			foreach (LanLobbyInfo lobby2 in lanListener.GetLobbies())
			{
				string displayName = lobby2.Settings.LobbyName.ToString();
				if (PassesNameFilter(displayName, filter))
				{
					AddLanRow(lobby2, displayName);
					num2++;
				}
			}
			SetStatus((num2 == 0) ? "Uygun lobi bulunamadı (LAN dinleniyor)." : $"{num2} lobi bulundu.");
		}

		private void ScanFriendsOnly(string filter)
		{
			SetStatus(Loc.Get("Status.ScanningFriends"));
			shownLobbyIds.Clear();
			int num = ScanFriends(filter);
			if (num != 0 || awaitingLobbyData.Count <= 0)
			{
				SetStatus((num == 0) ? "Arkadaşlarından şu an bu oyunda/lobide kimse yok." : $"{num} arkadaş lobisi bulundu.");
			}
		}

		private int ScanFriends(string filter)
		{
			int num = 0;
			pendingFilter = filter;
			foreach (Friend friend in SteamFriends.GetFriends())
			{
				Lobby? lobby = ((!friend.IsPlayingThisGame) ? ((Lobby?)null) : friend.GameInfo?.Lobby);
				if (!lobby.HasValue && SteamManager.TryParseConnectLobby(friend.GetRichPresence("connect"), out var lobbyId))
				{
					lobby = new Lobby(lobbyId);
				}
				if (!lobby.HasValue)
				{
					continue;
				}
				if (!HasLobbyData(lobby.Value))
				{
					if (!shownLobbyIds.Contains(lobby.Value.Id) && !awaitingLobbyData.ContainsKey(lobby.Value.Id) && lobby.Value.Refresh())
					{
						awaitingLobbyData[lobby.Value.Id] = friend.Name;
					}
					continue;
				}
				string data = lobby.Value.GetData("name");
				if (PassesNameFilter(data, filter) && shownLobbyIds.Add(lobby.Value.Id))
				{
					AddSteamRow(lobby.Value, string.IsNullOrEmpty(data) ? friend.Name : data);
					num++;
				}
			}
			return num;
		}

		private static bool HasLobbyData(Lobby lobby)
		{
			if (string.IsNullOrEmpty(lobby.GetData("mode")))
			{
				return !string.IsNullOrEmpty(lobby.GetData("name"));
			}
			return true;
		}

		private void OnLobbyDataArrived(Lobby lobby)
		{
			if (!awaitingLobbyData.TryGetValue(lobby.Id, out var value) || !HasLobbyData(lobby))
			{
				return;
			}
			awaitingLobbyData.Remove(lobby.Id);
			string data = lobby.GetData("name");
			if (PassesNameFilter(data, pendingFilter) && shownLobbyIds.Add(lobby.Id))
			{
				int count = resultRows.Count;
				AddSteamRow(lobby, string.IsNullOrEmpty(data) ? value : data);
				if (resultRows.Count > count)
				{
					SetStatus($"{resultRows.Count} lobi bulundu.");
				}
			}
		}

		private bool RefusedForVersion(string hostVersion)
		{
			if (GameVersion.CanJoin(hostVersion, out var refusal))
			{
				return false;
			}
			SetStatus(refusal);
			GameVersion.ShowRefusal(refusal);
			return true;
		}

		private static bool PassesNameFilter(string name, string filter)
		{
			if (!string.IsNullOrEmpty(filter))
			{
				if (!string.IsNullOrEmpty(name))
				{
					return name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
				}
				return false;
			}
			return true;
		}

		private void BuildModeFilterDropdown()
		{
			if (modeFilterDropdown == null)
			{
				return;
			}
			modeFilterIds.Clear();
			modeFilterIds.Add("");
			List<string> list = new List<string> { Loc.Get("LobbyBrowser.AllModes") };
			foreach (GameModeDefinition item in GameModeCatalog.All)
			{
				modeFilterIds.Add(item.ModeId);
				list.Add(item.DisplayName);
			}
			modeFilterDropdown.ClearOptions();
			modeFilterDropdown.AddOptions(list);
			modeFilterDropdown.SetValueWithoutNotify(0);
			modeFilterDropdown.onValueChanged.RemoveAllListeners();
			modeFilterDropdown.onValueChanged.AddListener(delegate(int index)
			{
				modeFilterId = ModeFilterIdAt(index);
				LobbyPrefs.SetString("Browser.ModeFilter", modeFilterId);
				Scan();
			});
		}

		private void AddSteamRow(Lobby lobby, string displayName)
		{
			string data = lobby.GetData("mode");
			if (!PassesModeFilter(data))
			{
				return;
			}
			LobbyListing listing = ReadSteamLobby(lobby, displayName);
			string hostVersion = lobby.GetData("version");
			bool known = HasLobbyData(lobby);
			resultRows.Add(CreateRow(in listing, delegate
			{
				if (!known || !RefusedForVersion(hostVersion))
				{
					RequestJoin(in listing, delegate
					{
						JoinSteamLobby(lobby);
					});
				}
			}));
		}

		private void AddLanRow(LanLobbyInfo info, string displayName)
		{
			string modeId = info.Settings.ModeId.ToString();
			if (!PassesModeFilter(modeId))
			{
				return;
			}
			LobbyListing listing = ReadLanLobby(info, displayName);
			resultRows.Add(CreateRow(in listing, delegate
			{
				if (!RefusedForVersion(info.Version))
				{
					RequestJoin(in listing, delegate
					{
						JoinLanLobby(info);
					});
				}
			}));
		}

		private static LobbyListing ReadSteamLobby(Lobby lobby, string displayName)
		{
			LobbySettingsData settings = new LobbySettingsData
			{
				PrepSeconds = ParseIntOrZero(lobby.GetData("prep_seconds")),
				HuntSeconds = ParseIntOrZero(lobby.GetData("hunt_seconds")),
				RoundEndSeconds = ParseIntOrZero(lobby.GetData("round_end_seconds")),
				MinHiders = ParseIntOrZero(lobby.GetData("min_hiders")),
				MinHunters = ParseIntOrZero(lobby.GetData("min_hunters")),
				HasPassword = (lobby.GetData("has_password") == "1"),
				MinPlayers = ParseIntOrZero(lobby.GetData("min_players")),
				ScoreLimit = ParseIntOrZero(lobby.GetData("score_limit")),
				TimeLimitSeconds = ParseIntOrZero(lobby.GetData("time_limit_seconds")),
				ExtraWarmupSeconds = ParseIntOrZero(lobby.GetData("extra_warmup_seconds")),
				RespawnSeconds = ParseIntOrZero(lobby.GetData("respawn_seconds"))
			};
			return new LobbyListing(displayName, lobby.GetData("mode"), lobby.GetData("map"), settings, lobby.MemberCount, lobby.MaxMembers, overSteam: true, lobby.Id.ToString(), SteamPing.EstimateMs(lobby.GetData("ping_location")), lobby.GetData("lang"), lobby.GetData("country"), lobby.GetData("phase"));
		}

		private static LobbyListing ReadLanLobby(LanLobbyInfo info, string displayName)
		{
			return new LobbyListing(displayName, info.Settings.ModeId.ToString(), info.Settings.MapId.ToString(), info.Settings, info.Players, info.MaxPlayers, overSteam: false, $"{info.Ip}:{info.Port}", -1, "", "", info.Phase);
		}

		private static int ParseIntOrZero(string text)
		{
			if (!int.TryParse(text, out var result))
			{
				return 0;
			}
			return result;
		}

		private static string MapSuffix(string mapId)
		{
			if (!string.IsNullOrEmpty(mapId))
			{
				return "  —  " + MapCatalog.DisplayName(mapId);
			}
			return "";
		}

		private static string ModeSuffix(string modeId)
		{
			if (!string.IsNullOrEmpty(modeId))
			{
				return "  —  " + GameModeCatalog.DisplayName(modeId);
			}
			return "";
		}

		private bool PassesModeFilter(string modeId)
		{
			if (!string.IsNullOrEmpty(modeFilterId) && !string.IsNullOrEmpty(modeId))
			{
				return modeId == modeFilterId;
			}
			return true;
		}

		private GameObject CreateRow(in LobbyListing listing, Action onJoin)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(resultTemplate, resultsContainer);
			gameObject.name = "ResultRow";
			if (gameObject.TryGetComponent<LobbyResultRowView>(out var component))
			{
				LobbyListing captured = listing;
				component.Bind(in listing, onJoin, (detailsPanel != null) ? ((Action)delegate
				{
					ShowDetails(in captured, onJoin);
				}) : null);
				gameObject.SetActive(value: true);
				return gameObject;
			}
			Transform transform = gameObject.transform.Find("LobbyNameText");
			if (transform != null && transform.TryGetComponent<TextMeshProUGUI>(out var component2))
			{
				component2.SetText(LegacyRowLabel(in listing));
			}
			Transform transform2 = gameObject.transform.Find("JoinButton");
			if (transform2 != null && transform2.TryGetComponent<Button>(out var component3))
			{
				component3.onClick.RemoveAllListeners();
				component3.onClick.AddListener(delegate
				{
					onJoin();
				});
			}
			gameObject.SetActive(value: true);
			return gameObject;
		}

		private static string LegacyRowLabel(in LobbyListing listing)
		{
			string text = (string.IsNullOrEmpty(listing.Name) ? Loc.Get("LobbyBrowser.Unnamed") : listing.Name);
			string text2 = (listing.HasPassword ? "  [*]" : "");
			string text3 = (listing.OverSteam ? "  [Steam]" : "  [LAN]");
			return text + text2 + ModeSuffix(listing.ModeId) + MapSuffix(listing.MapId) + text3;
		}

		private void ShowDetails(in LobbyListing listing, Action onJoin)
		{
			if (detailsPanel != null)
			{
				detailsPanel.Show(in listing, onJoin);
			}
		}

		private void RequestJoin(in LobbyListing listing, Action join)
		{
			if (!listing.HasPassword)
			{
				retryJoin = null;
				LobbyPassword.Offer(NetworkManager.Singleton, "");
				join();
				return;
			}
			Action retry = join;
			string retryLobbyName = listing.Name;
			retryJoin = delegate
			{
				RetryWithPassword(retryLobbyName, retry);
			};
			AskForPassword(listing.Name, null, join);
		}

		private void RetryWithPassword(string lobbyName, Action join)
		{
			AskForPassword(lobbyName, Loc.Get("Lobby.WrongPassword"), join);
		}

		private void AskForPassword(string lobbyName, string error, Action join)
		{
			string text = (string.IsNullOrEmpty(lobbyName) ? Loc.Get("LobbyBrowser.Unnamed") : lobbyName);
			DialogView.Show(new DialogRequest(Loc.Format("Lobby.PasswordPrompt", text), Loc.Get("Common.Join"), Loc.Get("Common.Cancel"), null, wantsInput: true, null, null, inputIsPassword: true), delegate(DialogAnswer answer, string typed)
			{
				if (answer == DialogAnswer.Confirm)
				{
					LobbyPassword.Offer(NetworkManager.Singleton, typed);
					join();
				}
			});
			if (!string.IsNullOrEmpty(error))
			{
				DialogView.ShowError(error);
			}
		}

		private void OnClientDisconnected(ulong clientId)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (!(singleton == null) && clientId == singleton.LocalClientId)
			{
				LoadingScreen.Hide();
				if (GameVersion.IsRefusal(singleton.DisconnectReason, out var hostVersion))
				{
					SetStatus(GameVersion.RefusalMessage(hostVersion));
					retryJoin = null;
				}
				else if (singleton.DisconnectReason == "Lobby.Banned")
				{
					SetStatus(Loc.Get("Lobby.Banned"));
					retryJoin = null;
				}
				else if (!(singleton.DisconnectReason != "Lobby.WrongPassword"))
				{
					SetStatus(Loc.Get("Lobby.WrongPassword"));
					Action action = retryJoin;
					retryJoin = null;
					action?.Invoke();
				}
			}
		}

		private void OnEnable()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton != null)
			{
				singleton.OnClientDisconnectCallback += OnClientDisconnected;
			}
			Loc.Changed += RelocalizeModeFilter;
			SteamMatchmaking.OnLobbyDataChanged += OnLobbyDataArrived;
		}

		private void OnDisable()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton != null)
			{
				singleton.OnClientDisconnectCallback -= OnClientDisconnected;
			}
			Loc.Changed -= RelocalizeModeFilter;
			SteamMatchmaking.OnLobbyDataChanged -= OnLobbyDataArrived;
		}

		private void RelocalizeModeFilter()
		{
			string modeId = modeFilterId;
			BuildModeFilterDropdown();
			RestoreModeFilter(modeId);
		}

		private void ClearResults()
		{
			foreach (GameObject resultRow in resultRows)
			{
				UnityEngine.Object.Destroy(resultRow);
			}
			resultRows.Clear();
			awaitingLobbyData.Clear();
		}

		private void JoinSteamLobby(Lobby lobby)
		{
			SteamLobbyJoin.JoinAsync(lobby, base.gameObject.scene, SetStatus);
		}

		private Task StageScenes()
		{
			return SteamLobbyJoin.StageScenes(base.gameObject.scene);
		}

		private async void JoinLanLobby(LanLobbyInfo info)
		{
			NetworkManager manager = NetworkManager.Singleton;
			if (manager == null)
			{
				SetStatus(Loc.Get("Status.NetworkManagerMissing"));
				return;
			}
			UnityTransport component = manager.GetComponent<UnityTransport>();
			if (component == null)
			{
				SetStatus(Loc.Get("Status.UnityTransportMissing"));
				return;
			}
			SteamManager.ActiveLobby = null;
			component.SetConnectionData(info.Ip, info.Port);
			manager.NetworkConfig.NetworkTransport = component;
			SetStatus(Loc.Get("Status.Connecting"));
			LoadingScreen.Show("Loading.Connecting");
			await StageScenes();
			ServerTickRate.ApplyForJoining(manager, info.TickRate);
			LobbyPassword.Prepare(manager);
			manager.StartClient();
			ConnectWatchdog.Watch(manager);
		}

		private async void JoinManual()
		{
			NetworkManager manager = NetworkManager.Singleton;
			if (manager == null)
			{
				SetStatus(Loc.Get("Status.NetworkManagerMissing"));
				return;
			}
			if (selectedFilter == FilterKind.Steam)
			{
				if (!SteamManager.IsInitialized)
				{
					SetStatus(Loc.Get("Status.SteamNotRunning"));
					return;
				}
				if (!ulong.TryParse(joinTargetField.text, out var result))
				{
					SetStatus(Loc.Get("Status.InvalidSteamId"));
					return;
				}
				FacepunchTransport facepunchTransport = manager.GetComponent<FacepunchTransport>();
				if (facepunchTransport == null)
				{
					SetStatus(Loc.Get("Status.FacepunchMissing"));
					return;
				}
				SetStatus(Loc.Get("Status.JoiningLobby"));
				Lobby? lobby = await SteamLobbyJoin.JoinLobbyWithTimeout(result);
				if (!lobby.HasValue)
				{
					SetStatus(Loc.Get("Status.LobbyJoinFailed"));
					return;
				}
				if (RefusedForVersion(lobby.Value.GetData("version")))
				{
					lobby.Value.Leave();
					return;
				}
				SteamManager.ActiveLobby = lobby.Value;
				manager.NetworkConfig.NetworkTransport = facepunchTransport;
				facepunchTransport.targetSteamId = lobby.Value.Owner.Id;
				string data = lobby.Value.GetData("name");
				bool locked = lobby.Value.GetData("has_password") == "1";
				string tickRate = lobby.Value.GetData("tickrate");
				Connect(data, locked, async delegate
				{
					SetStatus(Loc.Get("Status.Connecting"));
					LoadingScreen.Show("Loading.Connecting");
					await StageScenes();
					ServerTickRate.ApplyForJoining(manager, tickRate);
					LobbyPassword.Prepare(manager);
					manager.StartClient();
					ConnectWatchdog.Watch(manager);
				});
				return;
			}
			UnityTransport component = manager.GetComponent<UnityTransport>();
			if (component == null)
			{
				SetStatus(Loc.Get("Status.UnityTransportMissing"));
				return;
			}
			SteamManager.ActiveLobby = null;
			if (!TryParseEndpoint(joinTargetField.text, out var address, out var port))
			{
				SetStatus($"Geçersiz adres. \"ip\" ya da \"ip:port\" yaz - port yazmazsan {(ushort)7777} kullanılır.");
				return;
			}
			component.SetConnectionData(address, port);
			manager.NetworkConfig.NetworkTransport = component;
			Connect($"{address}:{port}", locked: false, async delegate
			{
				SetStatus(Loc.Get("Status.Connecting"));
				LoadingScreen.Show("Loading.Connecting");
				await StageScenes();
				ServerTickRate.ApplyForDirectJoin(manager);
				LobbyPassword.Prepare(manager);
				manager.StartClient();
				ConnectWatchdog.Watch(manager);
			});
		}

		private void Connect(string lobbyName, bool locked, Action connect)
		{
			retryJoin = delegate
			{
				RetryWithPassword(lobbyName, connect);
			};
			if (!locked)
			{
				LobbyPassword.Offer(NetworkManager.Singleton, "");
				connect();
			}
			else
			{
				AskForPassword(lobbyName, null, connect);
			}
		}

		private static bool TryParseEndpoint(string raw, out string address, out ushort port)
		{
			address = null;
			port = 7777;
			if (string.IsNullOrWhiteSpace(raw))
			{
				return false;
			}
			string[] array = raw.Trim().Split(':');
			if (array.Length > 2)
			{
				return false;
			}
			address = array[0].Trim();
			if (address.Length == 0)
			{
				return false;
			}
			if (array.Length < 2)
			{
				return true;
			}
			string text = array[1].Trim();
			if (text.Length == 0)
			{
				return true;
			}
			if (ushort.TryParse(text, out port))
			{
				return port != 0;
			}
			return false;
		}

		private void SetStatus(string message)
		{
			statusLabel.text = message;
		}
	}
}
