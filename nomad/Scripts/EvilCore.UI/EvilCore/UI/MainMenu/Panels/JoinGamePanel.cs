using System.Collections.Generic;
using System.Linq;
using EvilCore.Localization;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class JoinGamePanel : MonoBehaviour
	{
		[Header("Lobby List")]
		[SerializeField]
		private RectTransform lobbyListContent;

		[SerializeField]
		private LobbyEntryUI lobbyEntryPrefab;

		[Header("Status")]
		[SerializeField]
		private TextMeshProUGUI statusText;

		[Header("Actions")]
		[SerializeField]
		private Button refreshButton;

		[SerializeField]
		private Button backButton;

		[Header("Region Filter")]
		[SerializeField]
		private Toggle allRegionsToggle;

		[SerializeField]
		private TextMeshProUGUI allRegionsLabel;

		[Header("Join by Code")]
		[SerializeField]
		private TMP_InputField joinCodeInput;

		[SerializeField]
		private Button joinByCodeButton;

		[SerializeField]
		private Button pasteCodeButton;

		[Header("Password Popup")]
		[SerializeField]
		private PasswordPopup passwordPopup;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IRegionService _regionService;

		private readonly List<LobbyEntryUI> _lobbyEntries = new List<LobbyEntryUI>();

		private List<LobbySearchResult> _lastResults = new List<LobbySearchResult>();

		private void Start()
		{
			refreshButton.onClick.AddListener(RefreshLobbies);
			backButton.onClick.AddListener(delegate
			{
				_uiManager.ShowMainPanel();
			});
			joinByCodeButton.onClick.AddListener(OnJoinByCodeClicked);
			pasteCodeButton.onClick.AddListener(OnPasteCodeClicked);
			passwordPopup.gameObject.SetActive(value: false);
			if (allRegionsToggle != null)
			{
				allRegionsToggle.SetIsOnWithoutNotify(value: false);
				allRegionsToggle.onValueChanged.AddListener(delegate
				{
					RenderLobbies();
				});
			}
			if (allRegionsLabel != null)
			{
				allRegionsLabel.text = L("@lobby.all_regions");
			}
			_regionService.OnRegionResolved += OnRegionResolved;
		}

		public void OnPanelShown()
		{
			RefreshLobbies();
		}

		private string L(string key)
		{
			return _localizationService?.Localize(key) ?? key;
		}

		private void RefreshLobbies()
		{
			ClearLobbyList();
			statusText.text = L("@lobby.searching");
			refreshButton.interactable = false;
			_lobbyManager.SearchLobbies(OnLobbiesReceived);
		}

		private void OnLobbiesReceived(List<LobbySearchResult> results)
		{
			refreshButton.interactable = true;
			_lastResults = results ?? new List<LobbySearchResult>();
			RenderLobbies();
		}

		private void RenderLobbies()
		{
			ClearLobbyList();
			if (_lastResults.Count == 0)
			{
				statusText.text = L("@lobby.no_results");
				return;
			}
			RegionCode localRegion = _regionService.LocalRegion;
			IEnumerable<LobbySearchResult> source;
			if ((!(allRegionsToggle != null) || !allRegionsToggle.isOn) && localRegion != RegionCode.Unknown)
			{
				source = _lastResults.Where((LobbySearchResult l) => l.Region == localRegion);
			}
			else
			{
				IEnumerable<LobbySearchResult> lastResults = _lastResults;
				source = lastResults;
			}
			List<LobbySearchResult> list = source.OrderBy((LobbySearchResult l) => PingSortKey(l.Region)).ToList();
			if (list.Count == 0)
			{
				statusText.text = L("@lobby.no_results_region");
				return;
			}
			statusText.text = string.Format(L("@lobby.count_found"), list.Count);
			foreach (LobbySearchResult item in list)
			{
				LobbyEntryUI lobbyEntryUI = Object.Instantiate(lobbyEntryPrefab, lobbyListContent);
				lobbyEntryUI.Setup(item, OnJoinClicked, RegionDisplayName(item.Region), _regionService.EstimatePingMs(item.Region));
				_lobbyEntries.Add(lobbyEntryUI);
			}
		}

		private int PingSortKey(RegionCode hostRegion)
		{
			int num = _regionService.EstimatePingMs(hostRegion);
			if (num >= 0)
			{
				return num;
			}
			return 2147483647;
		}

		private string RegionDisplayName(RegionCode region)
		{
			return region switch
			{
				RegionCode.NorthAmerica => L("@lobby.region_na"), 
				RegionCode.SouthAmerica => L("@lobby.region_sa"), 
				RegionCode.Europe => L("@lobby.region_eu"), 
				RegionCode.Asia => L("@lobby.region_as"), 
				RegionCode.Oceania => L("@lobby.region_oc"), 
				_ => L("@lobby.region_unknown"), 
			};
		}

		private void OnRegionResolved(RegionCode region)
		{
			if (_lastResults.Count > 0)
			{
				RenderLobbies();
			}
		}

		private bool IsLocalUserBanned(LobbySearchResult lobby)
		{
			if (string.IsNullOrEmpty(lobby.BannedMembers))
			{
				return false;
			}
			string localProductUserId = _lobbyManager.LocalProductUserId;
			if (string.IsNullOrEmpty(localProductUserId))
			{
				return false;
			}
			string[] array = lobby.BannedMembers.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == localProductUserId)
				{
					return true;
				}
			}
			return false;
		}

		private void OnJoinClicked(LobbySearchResult lobby)
		{
			if (!_lobbyManager.IsVersionCompatible(lobby))
			{
				statusText.text = L("@lobby.version_mismatch");
			}
			else if (lobby.MemberCount >= lobby.MaxMembers)
			{
				statusText.text = L("@lobby.full");
			}
			else if (IsLocalUserBanned(lobby))
			{
				statusText.text = L("@lobby.banned");
			}
			else if (lobby.HasPassword)
			{
				passwordPopup.Show(lobby, OnPasswordConfirmed);
			}
			else
			{
				JoinLobby(lobby);
			}
		}

		private void OnPasswordConfirmed(LobbySearchResult lobby, string enteredPassword)
		{
			if (_lobbyManager.ValidatePassword(lobby, enteredPassword))
			{
				JoinLobby(lobby);
			}
			else
			{
				passwordPopup.ShowError(L("@lobby.wrong_password"));
			}
		}

		private void JoinLobby(LobbySearchResult lobby)
		{
			_uiManager.ShowLoadingGame();
			_lobbyManager.JoinLobbyById(lobby.LobbyId);
		}

		private void OnPasteCodeClicked()
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			if (!string.IsNullOrEmpty(systemCopyBuffer))
			{
				joinCodeInput.text = systemCopyBuffer.Trim().ToUpperInvariant();
			}
		}

		private void OnJoinByCodeClicked()
		{
			string text = joinCodeInput.text?.Trim().ToUpperInvariant();
			if (string.IsNullOrEmpty(text) || text.Length != 5)
			{
				statusText.text = L("@lobby.invalid_code");
				return;
			}
			statusText.text = L("@lobby.searching");
			joinByCodeButton.interactable = false;
			_lobbyManager.SearchLobbyByCode(text, delegate(LobbySearchResult? result)
			{
				joinByCodeButton.interactable = true;
				if (result.HasValue)
				{
					OnJoinClicked(result.Value);
				}
				else
				{
					statusText.text = L("@lobby.not_found");
				}
			});
		}

		private void ClearLobbyList()
		{
			foreach (LobbyEntryUI lobbyEntry in _lobbyEntries)
			{
				if (lobbyEntry != null)
				{
					Object.Destroy(lobbyEntry.gameObject);
				}
			}
			_lobbyEntries.Clear();
		}

		private void OnDestroy()
		{
			refreshButton?.onClick.RemoveAllListeners();
			backButton?.onClick.RemoveAllListeners();
			joinByCodeButton?.onClick.RemoveAllListeners();
			pasteCodeButton?.onClick.RemoveAllListeners();
			if (allRegionsToggle != null)
			{
				allRegionsToggle.onValueChanged.RemoveAllListeners();
			}
			if (_regionService != null)
			{
				_regionService.OnRegionResolved -= OnRegionResolved;
			}
			ClearLobbyList();
		}
	}
}
