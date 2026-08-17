using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class MainMenuPanel : MonoBehaviour
	{
		[SerializeField]
		private Button singlePlayerButton;

		[SerializeField]
		private Button createGameButton;

		[SerializeField]
		private Button loadGameButton;

		[SerializeField]
		private Button joinButton;

		[SerializeField]
		private Button settingsButton;

		[SerializeField]
		private Button quitButton;

		[Tooltip("Optional. Shown when offline (not logged in) — e.g. a localized \"Offline\" label (@main_menu.offline). Create/Join are greyed out alongside it.")]
		[SerializeField]
		private GameObject offlineIndicator;

		[Tooltip("Label colour for Multiplayer / Join when offline (Button.interactable only tints the background image, not the separate TMP label).")]
		[SerializeField]
		private Color disabledLabelColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private IAuthService _authService;

		private TMP_Text _createGameLabel;

		private TMP_Text _joinLabel;

		private Color _createGameDefaultColor;

		private Color _joinDefaultColor;

		private bool _labelsCached;

		private void Start()
		{
			singlePlayerButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowSinglePlayer();
			});
			createGameButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowCreateGame();
			});
			loadGameButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowLoadGame();
			});
			joinButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowJoinGame();
			});
			settingsButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowSettings();
			});
			quitButton?.onClick.AddListener(OnQuitClicked);
			if (_authService != null)
			{
				_authService.OnLoginSuccess += RefreshOnlineAvailability;
				_authService.OnLoginFailed += OnLoginFailed;
			}
			RefreshOnlineAvailability();
		}

		private void OnDestroy()
		{
			singlePlayerButton?.onClick.RemoveAllListeners();
			createGameButton?.onClick.RemoveAllListeners();
			loadGameButton?.onClick.RemoveAllListeners();
			joinButton?.onClick.RemoveAllListeners();
			settingsButton?.onClick.RemoveAllListeners();
			quitButton?.onClick.RemoveAllListeners();
			if (_authService != null)
			{
				_authService.OnLoginSuccess -= RefreshOnlineAvailability;
				_authService.OnLoginFailed -= OnLoginFailed;
			}
		}

		private void RefreshOnlineAvailability()
		{
			CacheButtonLabels();
			bool flag = _authService != null && _authService.IsLoggedIn;
			if (createGameButton != null)
			{
				createGameButton.interactable = flag;
			}
			if (joinButton != null)
			{
				joinButton.interactable = flag;
			}
			if (_createGameLabel != null)
			{
				_createGameLabel.color = (flag ? _createGameDefaultColor : disabledLabelColor);
			}
			if (_joinLabel != null)
			{
				_joinLabel.color = (flag ? _joinDefaultColor : disabledLabelColor);
			}
			if (offlineIndicator != null)
			{
				offlineIndicator.SetActive(!flag);
			}
		}

		private void CacheButtonLabels()
		{
			if (_labelsCached)
			{
				return;
			}
			_labelsCached = true;
			if (createGameButton != null)
			{
				_createGameLabel = createGameButton.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (_createGameLabel != null)
				{
					_createGameDefaultColor = _createGameLabel.color;
				}
			}
			if (joinButton != null)
			{
				_joinLabel = joinButton.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (_joinLabel != null)
				{
					_joinDefaultColor = _joinLabel.color;
				}
			}
		}

		private void OnLoginFailed(string _)
		{
			RefreshOnlineAvailability();
		}

		private static void OnQuitClicked()
		{
			Application.Quit();
		}
	}
}
