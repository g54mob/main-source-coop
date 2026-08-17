using System.Collections.Generic;
using System.Linq;
using EvilCore.Networking;
using EvilCore.UI.MainMenu.Panels;
using EvilCore.UI.Scripts;
using EvilCore.UI.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.MainMenu
{
	public class MainMenuUIManager : MonoBehaviour, IMainMenuUIManager
	{
		[SerializeField]
		private JoinGamePanel joinGamePanel;

		[SerializeField]
		private CreateGamePanel createGamePanel;

		[SerializeField]
		private SinglePlayerPanel singlePlayerPanel;

		[SerializeField]
		private LoadingGamePanel loadingGamePanel;

		[SerializeField]
		private LoadGamePanel loadGamePanel;

		[SerializeField]
		private SettingsPanel settingsPanel;

		[SerializeField]
		private FirstLaunchConsentPopup firstLaunchPanel;

		[SerializeField]
		private SaveCompatibilityNoticePopup saveNoticePanel;

		[Header("Slide Transitions (optional)")]
		[Tooltip("Assign a UISlidePanelTransition per sub-panel. On each: sidePanel = the main/side panel, contentPanel = that sub-panel, openButton/closeButton left EMPTY (the manager and the existing Back buttons drive it). When set, opening the panel and returning via Back slides instead of an instant alpha swap.")]
		[SerializeField]
		private UISlidePanelTransition settingsTransition;

		[SerializeField]
		private UISlidePanelTransition createGameTransition;

		[SerializeField]
		private UISlidePanelTransition singlePlayerTransition;

		[SerializeField]
		private UISlidePanelTransition joinGameTransition;

		[SerializeField]
		private UISlidePanelTransition loadGameTransition;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IGameSaveService _gameSaveService;

		private List<MainMenuCanvasGroup> _panels;

		private UISlidePanelTransition _activeTransition;

		private void Awake()
		{
			_panels = GetComponentsInChildren<MainMenuCanvasGroup>().ToList();
			foreach (MainMenuCanvasGroup panel in _panels)
			{
				panel.Initialize();
			}
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			settingsPanel?.Initialize(ShowMainPanel);
			if (TryResumeMultiplayerActivation())
			{
				return;
			}
			if (firstLaunchPanel != null && firstLaunchPanel.ShouldShow())
			{
				ShowPanel(MainMenuCanvasGroupName.FirstLaunch);
				return;
			}
			if (saveNoticePanel != null && saveNoticePanel.ShouldShow())
			{
				ShowPanel(MainMenuCanvasGroupName.SaveNotice);
				return;
			}
			IEOSLobbyManager lobbyManager = _lobbyManager;
			if (lobbyManager != null && lobbyManager.WasKicked)
			{
				ShowPanel(MainMenuCanvasGroupName.MainPanel);
				ShowJoinGame();
				_lobbyManager.ConsumeKickedFlag();
			}
			else
			{
				ShowMainPanel();
			}
		}

		private bool TryResumeMultiplayerActivation()
		{
			string text = _networkManager?.ConsumePendingMultiplayerSlot();
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			ShowLoadingGame();
			_gameSaveService?.SelectSlotForContinue(text);
			_lobbyManager?.CreateGameWithLobby(BuildActivationOptions(text));
			return true;
		}

		private LobbyCreateOptions BuildActivationOptions(string slot)
		{
			string lobbyName = slot;
			SaveSlotInfo[] array = _gameSaveService?.GetSaveSlots();
			if (array != null)
			{
				SaveSlotInfo[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					SaveSlotInfo saveSlotInfo = array2[i];
					if (saveSlotInfo.SlotId == slot && !string.IsNullOrEmpty(saveSlotInfo.DisplayName))
					{
						lobbyName = saveSlotInfo.DisplayName;
						break;
					}
				}
			}
			return new LobbyCreateOptions
			{
				LobbyName = lobbyName,
				MaxPlayers = 4u,
				IsPublic = false,
				Password = null,
				Seed = 0
			};
		}

		public void ShowMainPanel()
		{
			if (_activeTransition != null)
			{
				UISlidePanelTransition activeTransition = _activeTransition;
				_activeTransition = null;
				if (activeTransition.ShowSide(force: true))
				{
					return;
				}
			}
			ShowPanel(MainMenuCanvasGroupName.MainPanel);
		}

		public void ShowJoinGame()
		{
			OpenPanel(MainMenuCanvasGroupName.JoinGame, joinGameTransition);
			if (joinGamePanel != null)
			{
				joinGamePanel.OnPanelShown();
			}
		}

		public void ShowCreateGame()
		{
			OpenPanel(MainMenuCanvasGroupName.CreateGame, createGameTransition);
			if (createGamePanel != null)
			{
				createGamePanel.OnPanelShown();
			}
		}

		public void ShowSinglePlayer()
		{
			OpenPanel(MainMenuCanvasGroupName.SinglePlayer, singlePlayerTransition);
			if (singlePlayerPanel != null)
			{
				singlePlayerPanel.OnPanelShown();
			}
		}

		public void ShowLoadGame()
		{
			OpenPanel(MainMenuCanvasGroupName.LoadGame, loadGameTransition);
			if (loadGamePanel != null)
			{
				loadGamePanel.OnPanelShown();
			}
		}

		public void ShowSettings()
		{
			OpenPanel(MainMenuCanvasGroupName.Settings, settingsTransition);
			if (settingsPanel != null)
			{
				settingsPanel.OnPanelShown();
			}
		}

		public void ShowLoadingGame()
		{
			if (_activeTransition != null)
			{
				_activeTransition.SnapToSide();
				_activeTransition = null;
			}
			ShowPanel(MainMenuCanvasGroupName.LoadingGame);
			if (loadingGamePanel != null)
			{
				loadingGamePanel.OnPanelShown();
			}
		}

		public void ShowNetworkError()
		{
			if (_activeTransition != null)
			{
				_activeTransition.SnapToSide();
				_activeTransition = null;
			}
			ShowPanel(MainMenuCanvasGroupName.NetworkError);
		}

		public void HideAll()
		{
			foreach (MainMenuCanvasGroup panel in _panels)
			{
				panel.Hide();
			}
		}

		private void OpenPanel(MainMenuCanvasGroupName panel, UISlidePanelTransition transition)
		{
			if (transition != null)
			{
				_activeTransition = transition;
				transition.ShowContent();
			}
			else
			{
				ShowPanel(panel);
			}
		}

		private void ShowPanel(MainMenuCanvasGroupName panelName)
		{
			foreach (MainMenuCanvasGroup panel in _panels)
			{
				if (panel.MainMenuCanvasGroupName == panelName)
				{
					panel.Show(interactable: true, blockRaycast: true);
				}
				else
				{
					panel.Hide();
				}
			}
		}
	}
}
