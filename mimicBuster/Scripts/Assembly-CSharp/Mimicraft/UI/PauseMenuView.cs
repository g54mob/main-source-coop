using System;
using System.Collections;
using System.Collections.Generic;
using Mimicraft.Analytics;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Tutorial;
using Mimicraft.VoxelEditor;
using Steamworks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PauseMenuView : MonoBehaviour
	{
		private const string MenuSceneName = "Menu";

		private const float CardWidth = 300f;

		private const float CardPadding = 18f;

		private const float ContentWidth = 264f;

		private static readonly Color SelectedColor = new Color(0.25f, 0.45f, 0.25f, 0.95f);

		private static readonly Color UnselectedColor = new Color(0.18f, 0.18f, 0.18f, 0.9f);

		[SerializeField]
		private bool IsMultiplayer = true;

		[SerializeField]
		private GameObject panelRoot;

		[Tooltip("Rol tercihi bloğunun tamamı (başlık + üç buton + ipucu). Sadece rol dağıtan oyun modlarında görünür. Boş bırakılırsa butonlar ve ipucu tek tek gizlenir, başlık kalır.")]
		[SerializeField]
		private GameObject rolePreferenceGroup;

		[SerializeField]
		private Button hunterPreferenceButton;

		[SerializeField]
		private Button hiderPreferenceButton;

		[SerializeField]
		private Button noPreferenceButton;

		[SerializeField]
		private TextMeshProUGUI preferenceHintLabel;

		[SerializeField]
		private Button resumeButton;

		[Tooltip("Sadece host'ta görünür. Yeterli oyuncu olmasa bile bir round başlatır.")]
		[SerializeField]
		private Button startRoundButton;

		[Tooltip("Sadece host'ta görünür. Süren round'u atıp yeni bir Hazırlık başlatır.")]
		[SerializeField]
		private Button restartRoundButton;

		[SerializeField]
		private Button inviteFriendButton;

		[Tooltip("Steam lobi kimliği. Sadece oturum gerçekten Steam üzerinden koşarken görünür - LAN/doğrudan bağlantıda gösterilecek bir numara yok. Boş bırakılabilir.")]
		[SerializeField]
		private TextMeshProUGUI lobbyIdLabel;

		[Tooltip("Lobi ID satırının biçimi. BOŞ bırak - o zaman çeviri tablosundaki metin kullanılır ve dil değiştiğinde bu satır da değişir. Sadece bu ekranda başka bir şey yazsın istiyorsan doldur; doldurduğun metin çevrilmez.")]
		[SerializeField]
		private string lobbyIdFormat = "";

		private const string LegacyLobbyIdFormat = "Lobi ID: {0}";

		[Tooltip("Lobi kimliğini panoya kopyalayan buton. Kimlik etiketiyle birlikte görünüp gizlenir. OnClick'ine PauseMenuView.CopyLobbyId bağlanmalı. Boş bırakılabilir.")]
		[SerializeField]
		private Button copyLobbyIdButton;

		[Tooltip("Kimlik etiketi ile kopyala butonunu içeren panel. Bağlanırsa ikisi yerine bu gizlenir - panelin kendi arka planı da gitmiş olur. Boş bırakılabilir.")]
		[SerializeField]
		private GameObject lobbyIdGroup;

		[SerializeField]
		private Button leaveButton;

		[SerializeField]
		private Button quitButton;

		[Tooltip("Ders listesini açan buton. İsteğe bağlı; yalnızca sahnede bir TutorialDirector varken (Practice) görünür, diğer modlarda kendini gizler.")]
		[SerializeField]
		private Button lessonsButton;

		private RoundManager roundManager;

		private PlayerRole appliedPreference = (PlayerRole)(-1);

		private bool? lastInviteVisible;

		private bool? lastHostControlsVisible;

		private bool? lastRolePreferenceVisible;

		private NetworkManager watchedManager;

		private bool leavingAfterDisconnect;

		private string LobbyIdFormat
		{
			get
			{
				if (!string.IsNullOrEmpty(lobbyIdFormat) && !(lobbyIdFormat == "Lobi ID: {0}"))
				{
					return lobbyIdFormat;
				}
				return Loc.Get("Pause.LobbyId");
			}
		}

		public bool IsOpen => panelRoot.activeSelf;

		public static PauseMenuView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "PauseMenu");
			Stretch(rectTransform);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Panel");
			Stretch(rectTransform2);
			Stretch((RectTransform)UIFactory.CreatePanel(rectTransform2, "Scrim", new Color(0f, 0f, 0f, 0.65f)).transform);
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform2, "Card");
			rectTransform3.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.sizeDelta = new Vector2(300f, 370f);
			rectTransform3.gameObject.AddComponent<Image>().color = new Color(0.16f, 0.16f, 0.16f, 0.98f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(18, 18, 18, 18);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			((RectTransform)UIFactory.CreateLabel(rectTransform3, "Title", Loc.Get("Pause.Title"), 22).transform).sizeDelta = new Vector2(264f, 28f);
			RectTransform rectTransform4 = UIFactory.CreateRect(rectTransform3, "RolePreferenceGroup");
			VerticalLayoutGroup verticalLayoutGroup2 = rectTransform4.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 8f;
			verticalLayoutGroup2.childControlWidth = false;
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childForceExpandWidth = false;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup2.childAlignment = TextAnchor.UpperCenter;
			rectTransform4.sizeDelta = new Vector2(264f, 102f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform4, "PreferenceHeading", Loc.Get("Pause.RolePreference"));
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.color = new Color(1f, 1f, 1f, 0.65f);
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(264f, 20f);
			RectTransform rectTransform5 = UIFactory.CreateRect(rectTransform4, "PreferenceRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform5.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 6f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform5.sizeDelta = new Vector2(264f, 32f);
			float x = (264f - horizontalLayoutGroup.spacing * 2f) / 3f;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform5, "HunterPreferenceButton", Loc.Get("Role.Hunter"), out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(x, 32f);
			Button button2 = UIFactory.CreateButton(rectTransform5, "HiderPreferenceButton", Loc.Get("Role.Modeler"), out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(x, 32f);
			Button button3 = UIFactory.CreateButton(rectTransform5, "NoPreferenceButton", Loc.Get("Pause.NoPreference"), out text);
			((RectTransform)button3.transform).sizeDelta = new Vector2(x, 32f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform4, "PreferenceHint", "", 12);
			textMeshProUGUI2.textWrappingMode = TextWrappingModes.Normal;
			textMeshProUGUI2.color = new Color(1f, 1f, 1f, 0.55f);
			((RectTransform)textMeshProUGUI2.transform).sizeDelta = new Vector2(264f, 34f);
			Button button4 = UIFactory.CreateButton(rectTransform3, "ResumeButton", Loc.Get("Pause.Resume"), out text);
			((RectTransform)button4.transform).sizeDelta = new Vector2(264f, 36f);
			Button button5 = UIFactory.CreateButton(rectTransform3, "StartRoundButton", Loc.Get("Pause.StartRound"), out text);
			((RectTransform)button5.transform).sizeDelta = new Vector2(264f, 36f);
			button5.gameObject.SetActive(value: false);
			Button button6 = UIFactory.CreateButton(rectTransform3, "RestartRoundButton", Loc.Get("Pause.RestartRound"), out text);
			((RectTransform)button6.transform).sizeDelta = new Vector2(264f, 36f);
			button6.gameObject.SetActive(value: false);
			Button button7 = UIFactory.CreateButton(rectTransform3, "InviteFriendButton", Loc.Get("Pause.InviteFriend"), out text);
			((RectTransform)button7.transform).sizeDelta = new Vector2(264f, 36f);
			button7.gameObject.SetActive(value: false);
			RectTransform rectTransform6 = UIFactory.CreateRect(rectTransform3, "LobbyIdGroup");
			HorizontalLayoutGroup horizontalLayoutGroup2 = rectTransform6.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.spacing = 6f;
			horizontalLayoutGroup2.childControlWidth = false;
			horizontalLayoutGroup2.childControlHeight = false;
			horizontalLayoutGroup2.childForceExpandWidth = false;
			horizontalLayoutGroup2.childForceExpandHeight = false;
			horizontalLayoutGroup2.childAlignment = TextAnchor.MiddleLeft;
			rectTransform6.sizeDelta = new Vector2(264f, 26f);
			TextMeshProUGUI textMeshProUGUI3 = UIFactory.CreateLabel(rectTransform6, "LobbyIdLabel", "", 12);
			textMeshProUGUI3.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI3.color = new Color(1f, 1f, 1f, 0.55f);
			((RectTransform)textMeshProUGUI3.transform).sizeDelta = new Vector2(174f - horizontalLayoutGroup2.spacing, 26f);
			Button button8 = UIFactory.CreateButton(rectTransform6, "CopyLobbyIdButton", Loc.Get("Pause.Copy"), out text);
			((RectTransform)button8.transform).sizeDelta = new Vector2(90f, 26f);
			rectTransform6.gameObject.SetActive(value: false);
			Button button9 = UIFactory.CreateButton(rectTransform3, "LeaveButton", Loc.Get("Pause.LeaveLobby"), out text);
			((RectTransform)button9.transform).sizeDelta = new Vector2(264f, 36f);
			Button button10 = UIFactory.CreateButton(rectTransform3, "QuitButton", Loc.Get("Pause.QuitGame"), out text);
			((RectTransform)button10.transform).sizeDelta = new Vector2(264f, 36f);
			rectTransform2.gameObject.SetActive(value: false);
			PauseMenuView pauseMenuView = rectTransform.gameObject.AddComponent<PauseMenuView>();
			pauseMenuView.panelRoot = rectTransform2.gameObject;
			pauseMenuView.rolePreferenceGroup = rectTransform4.gameObject;
			pauseMenuView.hunterPreferenceButton = button;
			pauseMenuView.hiderPreferenceButton = button2;
			pauseMenuView.noPreferenceButton = button3;
			pauseMenuView.preferenceHintLabel = textMeshProUGUI2;
			pauseMenuView.resumeButton = button4;
			pauseMenuView.startRoundButton = button5;
			pauseMenuView.restartRoundButton = button6;
			pauseMenuView.inviteFriendButton = button7;
			pauseMenuView.lobbyIdLabel = textMeshProUGUI3;
			pauseMenuView.copyLobbyIdButton = button8;
			pauseMenuView.lobbyIdGroup = rectTransform6.gameObject;
			pauseMenuView.leaveButton = button9;
			pauseMenuView.quitButton = button10;
			button8.onClick.AddListener(pauseMenuView.CopyLobbyId);
			return pauseMenuView;
		}

		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		[Obsolete("DialogView is found automatically; this no longer does anything.")]
		public void SetConfirmDialog(UnityEngine.Object unused)
		{
		}

		private void Awake()
		{
			if (IsMultiplayer && NetworkManager.Singleton != null)
			{
				watchedManager = NetworkManager.Singleton;
				watchedManager.OnClientDisconnectCallback += OnClientDisconnected;
			}
			if ((bool)hunterPreferenceButton)
			{
				hunterPreferenceButton.onClick.RemoveAllListeners();
				hunterPreferenceButton.onClick.AddListener(delegate
				{
					RequestPreference(PlayerRole.Hunter);
				});
			}
			if ((bool)hiderPreferenceButton)
			{
				hiderPreferenceButton.onClick.RemoveAllListeners();
				hiderPreferenceButton.onClick.AddListener(delegate
				{
					RequestPreference(PlayerRole.Hider);
				});
			}
			if ((bool)noPreferenceButton)
			{
				noPreferenceButton.onClick.RemoveAllListeners();
				noPreferenceButton.onClick.AddListener(delegate
				{
					RequestPreference(PlayerRole.None);
				});
			}
			if ((bool)resumeButton)
			{
				resumeButton.onClick.RemoveAllListeners();
				resumeButton.onClick.AddListener(delegate
				{
					SetOpen(open: false);
				});
			}
			if ((bool)startRoundButton)
			{
				startRoundButton.onClick.RemoveAllListeners();
				startRoundButton.onClick.AddListener(RequestStartRound);
			}
			if ((bool)restartRoundButton)
			{
				restartRoundButton.onClick.RemoveAllListeners();
				restartRoundButton.onClick.AddListener(RequestRestartRound);
			}
			if ((bool)inviteFriendButton)
			{
				inviteFriendButton.onClick.RemoveAllListeners();
				inviteFriendButton.onClick.AddListener(InviteFriend);
			}
			if ((bool)leaveButton)
			{
				leaveButton.onClick.RemoveAllListeners();
				leaveButton.onClick.AddListener(delegate
				{
					Confirm(Loc.Get((NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost) ? "Pause.ConfirmCloseLobby" : "Pause.ConfirmLeaveLobby"), LeaveLobby);
				});
			}
			if ((bool)quitButton)
			{
				quitButton.onClick.RemoveAllListeners();
				quitButton.onClick.AddListener(delegate
				{
					Confirm(Loc.Get("Pause.ConfirmQuit"), QuitGame);
				});
			}
			if ((bool)lessonsButton)
			{
				lessonsButton.onClick.RemoveAllListeners();
				lessonsButton.onClick.AddListener(delegate
				{
					SetOpen(open: false);
					TutorialLessonMenu.Open();
				});
			}
			LocalizeCaptions();
			if (panelRoot != null)
			{
				SetOpen(open: false);
			}
		}

		private void LocalizeCaptions()
		{
			Localize(hunterPreferenceButton, "Role.Hunter");
			Localize(hiderPreferenceButton, "Role.Modeler");
			Localize(noPreferenceButton, "Pause.NoPreference");
			Localize(resumeButton, "Pause.Resume");
			Localize(startRoundButton, "Pause.StartRound");
			Localize(restartRoundButton, "Pause.RestartRound");
			Localize(inviteFriendButton, "Pause.InviteFriend");
			Localize(copyLobbyIdButton, "Pause.Copy");
			Localize(leaveButton, "Pause.LeaveLobby");
			Localize(quitButton, "Pause.QuitGame");
		}

		private static void Localize(Button button, string key)
		{
			if (button != null)
			{
				LocalizedText.AttachIfUnset(button.GetComponentInChildren<TMP_Text>(includeInactive: true), key);
			}
		}

		private void OnDisable()
		{
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void OnDestroy()
		{
			if (watchedManager != null)
			{
				watchedManager.OnClientDisconnectCallback -= OnClientDisconnected;
			}
		}

		private void OnClientDisconnected(ulong clientId)
		{
			NetworkManager networkManager = watchedManager;
			if (!(networkManager == null) && !networkManager.IsServer && clientId == networkManager.LocalClientId && !leavingAfterDisconnect)
			{
				leavingAfterDisconnect = true;
				string text = networkManager.DisconnectReason ?? "";
				DisconnectNotice.Post((text == "Lobby.Banned") ? "Lobby.Banned" : ((text == "HostClosed") ? "Status.HostClosed" : "Status.ConnectionLost"));
				Telemetry.Send("disconnect", ("reason", (text == "Lobby.Banned") ? "vote_kicked" : ((text == "HostClosed") ? "host_closed" : "lost")), ("session_seconds", Telemetry.SessionSeconds));
				StartCoroutine(LeaveNextFrame());
			}
		}

		private IEnumerator LeaveNextFrame()
		{
			yield return null;
			LeaveLobby();
		}

		private void Update()
		{
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && !GameMenuState.EscapeConsumedThisFrame && !IsEditorGestureActive() && !IsConfirming() && !SettingsMenuView.IsAnyOpen)
			{
				GameMenuState.RequestEscape(30, delegate
				{
					SetOpen(!IsOpen);
				});
			}
			if (IsOpen)
			{
				RefreshRolePreferenceVisibility();
				RefreshPreferenceVisuals();
				RefreshInviteButtonVisibility();
				RefreshHostControls();
			}
		}

		private static bool IsEditorGestureActive()
		{
			return VoxelFocusManager.IsAnyGestureActive();
		}

		private bool IsConfirming()
		{
			return DialogView.IsShowing;
		}

		public void SetOpen(bool open)
		{
			GameMenuState.SetMenuOpen(this, open);
			if (IsOpen == open)
			{
				return;
			}
			panelRoot.SetActive(open);
			if ((bool)lessonsButton)
			{
				lessonsButton.gameObject.SetActive(TutorialDirector.Instance != null && GameModeController.Current is PracticeRoundManager);
			}
			if (open)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				appliedPreference = (PlayerRole)(-1);
				RefreshRolePreferenceVisibility();
				RefreshPreferenceVisuals();
			}
			else
			{
				PlayerEditSession playerEditSession = LocalEditSession();
				if (playerEditSession != null)
				{
					playerEditSession.RefreshCursorState();
				}
			}
		}

		private static PlayerEditSession LocalEditSession()
		{
			NetworkObject networkObject = ((NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClient != null) ? NetworkManager.Singleton.LocalClient.PlayerObject : null);
			if (!(networkObject != null))
			{
				return null;
			}
			return networkObject.GetComponent<PlayerEditSession>();
		}

		private RoundManager ResolveRoundManager()
		{
			if (!(roundManager != null))
			{
				return roundManager = GameModeController.Current as RoundManager;
			}
			return roundManager;
		}

		private void RequestPreference(PlayerRole preferred)
		{
			RoundManager roundManager = ResolveRoundManager();
			if (roundManager == null || !roundManager.IsSpawned)
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Pause.RoleSendFailed"));
				}
			}
			else
			{
				roundManager.RequestRolePreferenceServerRpc(preferred);
			}
		}

		private void RequestStartRound()
		{
			if (TryResolveSpawnedMode(out var mode))
			{
				mode.RequestStartRound();
			}
		}

		private void RequestRestartRound()
		{
			if (TryResolveSpawnedMode(out var mode))
			{
				mode.RequestRestartRound();
			}
		}

		private bool TryResolveSpawnedMode(out GameModeController mode)
		{
			mode = GameModeController.Current;
			if (mode != null && mode.IsSpawned)
			{
				return true;
			}
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(Loc.Get("Pause.CommandSendFailed"));
			}
			return false;
		}

		private void RefreshHostControls()
		{
			if (startRoundButton == null && restartRoundButton == null)
			{
				return;
			}
			GameModeController current = GameModeController.Current;
			bool flag = IsMultiplayer && NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer && current != null && current.SupportsHostRoundControls;
			if (flag != lastHostControlsVisible)
			{
				lastHostControlsVisible = flag;
				if (startRoundButton != null)
				{
					startRoundButton.gameObject.SetActive(flag);
				}
				if (restartRoundButton != null)
				{
					restartRoundButton.gameObject.SetActive(flag);
				}
			}
			if (flag)
			{
				bool isRoundRunning = current.IsRoundRunning;
				if (startRoundButton != null)
				{
					startRoundButton.interactable = !isRoundRunning;
				}
				if (restartRoundButton != null)
				{
					restartRoundButton.interactable = isRoundRunning;
				}
			}
		}

		private void RefreshRolePreferenceVisibility()
		{
			GameModeController current = GameModeController.Current;
			bool flag = IsMultiplayer && current != null && current.UsesRolePreference;
			if (flag == lastRolePreferenceVisible)
			{
				return;
			}
			lastRolePreferenceVisible = flag;
			appliedPreference = (PlayerRole)(-1);
			if (rolePreferenceGroup != null)
			{
				rolePreferenceGroup.SetActive(flag);
				return;
			}
			if (hunterPreferenceButton != null && hunterPreferenceButton.transform.parent != null)
			{
				hunterPreferenceButton.transform.parent.gameObject.SetActive(flag);
			}
			if (preferenceHintLabel != null)
			{
				preferenceHintLabel.gameObject.SetActive(flag);
			}
		}

		private void RefreshPreferenceVisuals()
		{
			if (IsMultiplayer && lastRolePreferenceVisible == true)
			{
				RoundManager roundManager = ResolveRoundManager();
				PlayerRole playerRole = ((roundManager != null) ? roundManager.LocalRolePreference : PlayerRole.None);
				if (playerRole != appliedPreference)
				{
					appliedPreference = playerRole;
					hunterPreferenceButton.image.color = ((playerRole == PlayerRole.Hunter) ? SelectedColor : UnselectedColor);
					hiderPreferenceButton.image.color = ((playerRole == PlayerRole.Hider) ? SelectedColor : UnselectedColor);
					noPreferenceButton.image.color = ((playerRole == PlayerRole.None) ? SelectedColor : UnselectedColor);
					preferenceHintLabel.text = Loc.Get((playerRole == PlayerRole.None) ? "Pause.RoleRandom" : "Pause.RoleNextPreparation");
				}
			}
		}

		private void RefreshInviteButtonVisibility()
		{
			if (!IsMultiplayer)
			{
				return;
			}
			bool hasValue = SteamManager.ActiveLobby.HasValue;
			if (hasValue == lastInviteVisible)
			{
				return;
			}
			lastInviteVisible = hasValue;
			inviteFriendButton.gameObject.SetActive(hasValue);
			if (lobbyIdGroup != null)
			{
				lobbyIdGroup.SetActive(hasValue);
			}
			else
			{
				if (lobbyIdLabel != null)
				{
					lobbyIdLabel.gameObject.SetActive(hasValue);
				}
				if (copyLobbyIdButton != null)
				{
					copyLobbyIdButton.gameObject.SetActive(hasValue);
				}
			}
			if (hasValue && lobbyIdLabel != null)
			{
				lobbyIdLabel.text = string.Format(LobbyIdFormat, SteamManager.ActiveLobby.Value.Id.Value);
			}
		}

		public void CopyLobbyId()
		{
			if (!SteamManager.ActiveLobby.HasValue)
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Pause.NoLobbyId"));
				}
				return;
			}
			string text = (GUIUtility.systemCopyBuffer = SteamManager.ActiveLobby.Value.Id.Value.ToString());
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(Loc.Format("Pause.LobbyIdCopied", text));
			}
		}

		private void InviteFriend()
		{
			if (SteamManager.ActiveLobby.HasValue)
			{
				SteamFriends.OpenGameInviteOverlay(SteamManager.ActiveLobby.Value.Id);
			}
		}

		private void Confirm(string message, Action onConfirm)
		{
			DialogView.Confirm(message, onConfirm);
		}

		public static void LeaveToMenu()
		{
			PauseMenuView pauseMenuView = UnityEngine.Object.FindFirstObjectByType<PauseMenuView>(FindObjectsInactive.Include);
			if (pauseMenuView != null)
			{
				pauseMenuView.LeaveLobby();
			}
			else
			{
				SceneManager.LoadScene("Menu", LoadSceneMode.Single);
			}
		}

		private void LeaveLobby()
		{
			if (IsMultiplayer)
			{
				SteamManager.LeaveActiveLobby();
				NetworkManager singleton = NetworkManager.Singleton;
				if (singleton != null)
				{
					LobbyPassword.Clear(singleton);
					if (singleton.IsServer)
					{
						List<ulong> list = new List<ulong>();
						foreach (NetworkClient connectedClients in singleton.ConnectedClientsList)
						{
							if (connectedClients.ClientId != singleton.LocalClientId)
							{
								list.Add(connectedClients.ClientId);
							}
						}
						foreach (ulong item in list)
						{
							singleton.DisconnectClient(item, "HostClosed");
						}
					}
					singleton.Shutdown();
					UnityEngine.Object.Destroy(singleton.gameObject);
				}
				LanBeaconBroadcaster lanBeaconBroadcaster = UnityEngine.Object.FindFirstObjectByType<LanBeaconBroadcaster>();
				if (lanBeaconBroadcaster != null)
				{
					lanBeaconBroadcaster.StopBroadcasting();
					UnityEngine.Object.Destroy(lanBeaconBroadcaster.gameObject);
				}
				LanBeaconListener lanBeaconListener = UnityEngine.Object.FindFirstObjectByType<LanBeaconListener>();
				if (lanBeaconListener != null)
				{
					lanBeaconListener.StopListening();
					UnityEngine.Object.Destroy(lanBeaconListener.gameObject);
				}
			}
			GameMenuState.ClearMenuOwners();
			VoxelEditorSettings.IsMovementMode = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene("Menu", LoadSceneMode.Single);
		}

		private static void QuitGame()
		{
			Application.Quit();
		}
	}
}
