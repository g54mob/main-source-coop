using System.Collections;
using System.Collections.Generic;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.GameMenu.Panels
{
	public class OnlineGameInfoPanel : MonoBehaviour
	{
		[Header("Join Code")]
		[SerializeField]
		private TextMeshProUGUI joinCodeText;

		[SerializeField]
		private Button copyCodeButton;

		[Header("World Seed")]
		[SerializeField]
		private TextMeshProUGUI seedText;

		[Header("Player List")]
		[SerializeField]
		private RectTransform playerListContent;

		[SerializeField]
		private PlayerListEntryUI playerEntryPrefab;

		[Header("Single-player overlay")]
		[Tooltip("CanvasGroup wrapping the multiplayer info (join code + seed + player list). Dimmed in single-player.")]
		[SerializeField]
		private CanvasGroup multiplayerContentGroup;

		[Tooltip("Overlay shown over the dimmed content in single-player, holding the Activate Multiplayer button + hint.")]
		[SerializeField]
		private GameObject activateMultiplayerOverlay;

		[SerializeField]
		private Button activateMultiplayerButton;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IAuthService _authService;

		[Inject]
		private IWorldSeedProvider _worldSeedProvider;

		private readonly List<PlayerListEntryUI> _entries = new List<PlayerListEntryUI>();

		private Coroutine _pingRefreshCoroutine;

		private void Start()
		{
			if (seedText != null)
			{
				seedText.text = ((_worldSeedProvider != null && _worldSeedProvider.HasSeed) ? _worldSeedProvider.Seed.ToString() : "-----");
			}
			bool flag = _networkManager != null && _networkManager.IsSingleplayerSession;
			if (activateMultiplayerOverlay != null)
			{
				activateMultiplayerOverlay.SetActive(flag);
			}
			SetContentDimmed(flag);
			if (flag)
			{
				if (activateMultiplayerButton != null)
				{
					activateMultiplayerButton.onClick.AddListener(OnActivateMultiplayerClicked);
					activateMultiplayerButton.interactable = _authService != null && _authService.IsLoggedIn;
				}
				return;
			}
			if (joinCodeText != null)
			{
				joinCodeText.text = _lobbyManager.JoinCode ?? "-----";
			}
			copyCodeButton?.onClick.AddListener(OnCopyCodeClicked);
			RefreshPlayerList();
		}

		private void SetContentDimmed(bool dimmed)
		{
			if (!(multiplayerContentGroup == null))
			{
				multiplayerContentGroup.alpha = (dimmed ? 0.35f : 1f);
				multiplayerContentGroup.interactable = !dimmed;
				multiplayerContentGroup.blocksRaycasts = !dimmed;
			}
		}

		private void OnActivateMultiplayerClicked()
		{
			_networkManager?.ActivateMultiplayer();
		}

		private void OnEnable()
		{
			if (_networkManager != null)
			{
				_networkManager.OnConnectedPlayersChanged += RefreshPlayerList;
			}
			_pingRefreshCoroutine = StartCoroutine(PingRefreshLoop());
		}

		private void OnDisable()
		{
			if (_networkManager != null)
			{
				_networkManager.OnConnectedPlayersChanged -= RefreshPlayerList;
			}
			if (_pingRefreshCoroutine != null)
			{
				StopCoroutine(_pingRefreshCoroutine);
				_pingRefreshCoroutine = null;
			}
		}

		private IEnumerator PingRefreshLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(2f);
				RefreshPingValues();
			}
		}

		private void RefreshPingValues()
		{
			IReadOnlyList<INetworkPlayer> readOnlyList = _networkManager?.ConnectedPlayers;
			if (readOnlyList == null)
			{
				return;
			}
			for (int i = 0; i < _entries.Count && i < readOnlyList.Count; i++)
			{
				INetworkPlayer networkPlayer = readOnlyList[i];
				if (_entries[i] != null && networkPlayer != null)
				{
					_entries[i].UpdatePing(networkPlayer.PingMs);
				}
			}
		}

		private void OnCopyCodeClicked()
		{
			if (!string.IsNullOrEmpty(_lobbyManager.JoinCode))
			{
				GUIUtility.systemCopyBuffer = _lobbyManager.JoinCode;
			}
		}

		private void RefreshPlayerList()
		{
			ClearPlayerList();
			if (_networkManager != null && _networkManager.IsSingleplayerSession)
			{
				return;
			}
			IReadOnlyList<INetworkPlayer> readOnlyList = _networkManager?.ConnectedPlayers;
			if (readOnlyList == null)
			{
				return;
			}
			bool isLocalHost = _lobbyManager != null && _lobbyManager.IsOwner;
			bool flag = true;
			foreach (INetworkPlayer item in readOnlyList)
			{
				string memberId = item.EosProductUserId ?? "";
				string playerName = (string.IsNullOrEmpty(item.DisplayName) ? "Connecting..." : item.DisplayName);
				int pingMs = item.PingMs;
				bool isHost = flag;
				flag = false;
				PlayerListEntryUI playerListEntryUI = Object.Instantiate(playerEntryPrefab, playerListContent);
				playerListEntryUI.Setup(memberId, playerName, pingMs, isHost, isLocalHost, OnKickClicked, OnBanClicked);
				_entries.Add(playerListEntryUI);
			}
		}

		private void OnKickClicked(string memberId)
		{
			_lobbyManager.KickMember(memberId);
		}

		private void OnBanClicked(string memberId)
		{
			_lobbyManager.BanMember(memberId);
		}

		private void ClearPlayerList()
		{
			foreach (PlayerListEntryUI entry in _entries)
			{
				if (entry != null)
				{
					Object.Destroy(entry.gameObject);
				}
			}
			_entries.Clear();
		}

		private void OnDestroy()
		{
			copyCodeButton?.onClick.RemoveAllListeners();
			activateMultiplayerButton?.onClick.RemoveAllListeners();
			ClearPlayerList();
		}
	}
}
