using EvilCore.Localization;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class NetworkErrorPopup : MainMenuCanvasGroup
	{
		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private TextMeshProUGUI messageText;

		[SerializeField]
		private Button confirmButton;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private INetworkErrorService _errorService;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		private NetworkErrorType _currentErrorType;

		private bool _subscribedToLocale;

		private void Awake()
		{
			if (confirmButton != null)
			{
				confirmButton.onClick.AddListener(OnConfirmClicked);
			}
		}

		private void Start()
		{
			ApplyLocalizedText();
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += ApplyLocalizedText;
				_subscribedToLocale = true;
			}
			if (_errorService != null)
			{
				_errorService.OnNetworkError += Show;
				NetworkErrorInfo? networkErrorInfo = _errorService.ConsumePending();
				if (networkErrorInfo.HasValue)
				{
					Show(networkErrorInfo.Value);
				}
			}
		}

		private void OnDestroy()
		{
			if (_subscribedToLocale && _localizationService != null)
			{
				_localizationService.OnLocaleChanged -= ApplyLocalizedText;
			}
			if (_errorService != null)
			{
				_errorService.OnNetworkError -= Show;
			}
			if (confirmButton != null)
			{
				confirmButton.onClick.RemoveListener(OnConfirmClicked);
			}
		}

		private void Show(NetworkErrorInfo info)
		{
			_currentErrorType = info.Type;
			ApplyLocalizedText();
			if (_lobbyManager != null && _lobbyManager.IsInLobby)
			{
				_lobbyManager.LeaveLobby();
			}
			_uiManager?.ShowNetworkError();
		}

		private void OnConfirmClicked()
		{
			_uiManager?.ShowMainPanel();
		}

		private void ApplyLocalizedText()
		{
			if (titleText != null)
			{
				titleText.text = Localize("@network.error_title");
			}
			if (messageText != null)
			{
				messageText.text = Localize(MapKey(_currentErrorType));
			}
		}

		private string Localize(string key)
		{
			if (_localizationService == null)
			{
				return key;
			}
			return _localizationService.Localize(key);
		}

		private static string MapKey(NetworkErrorType type)
		{
			return type switch
			{
				NetworkErrorType.ConnectionLost => "@network.connection_lost", 
				NetworkErrorType.ConnectionTimeout => "@network.connection_timeout", 
				NetworkErrorType.ConnectionFailed => "@network.connection_failed", 
				NetworkErrorType.ServerFull => "@lobby.full", 
				NetworkErrorType.LobbyNotFound => "@lobby.not_found", 
				NetworkErrorType.LobbyCreateFailed => "@network.lobby_create_failed", 
				NetworkErrorType.LobbyJoinFailed => "@network.lobby_join_failed", 
				NetworkErrorType.AuthenticationFailed => "@network.authentication_failed", 
				_ => "@network.unknown", 
			};
		}
	}
}
