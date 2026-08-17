using System;
using EvilCore.Localization;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class PasswordPopup : MonoBehaviour
	{
		[Inject]
		private ILocalizationService _localizationService;

		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private TMP_InputField passwordInput;

		[SerializeField]
		private TextMeshProUGUI errorText;

		[SerializeField]
		private Button confirmButton;

		[SerializeField]
		private Button cancelButton;

		private LobbySearchResult _targetLobby;

		private Action<LobbySearchResult, string> _onConfirmed;

		private void Awake()
		{
			confirmButton.onClick.AddListener(OnConfirmClicked);
			cancelButton.onClick.AddListener(OnCancelClicked);
		}

		public void Show(LobbySearchResult lobby, Action<LobbySearchResult, string> onConfirmed)
		{
			_targetLobby = lobby;
			_onConfirmed = onConfirmed;
			string format = _localizationService?.Localize("@lobby.enter_password") ?? "@lobby.enter_password";
			titleText.text = string.Format(format, lobby.LobbyName);
			passwordInput.text = "";
			errorText.gameObject.SetActive(value: false);
			base.gameObject.SetActive(value: true);
		}

		public void ShowError(string message)
		{
			errorText.text = message;
			errorText.gameObject.SetActive(value: true);
		}

		private void OnConfirmClicked()
		{
			if (!string.IsNullOrEmpty(passwordInput.text))
			{
				_onConfirmed?.Invoke(_targetLobby, passwordInput.text);
			}
		}

		private void OnCancelClicked()
		{
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			confirmButton?.onClick.RemoveAllListeners();
			cancelButton?.onClick.RemoveAllListeners();
		}
	}
}
