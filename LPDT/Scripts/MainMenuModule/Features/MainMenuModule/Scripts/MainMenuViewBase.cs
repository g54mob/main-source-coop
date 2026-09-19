using System;
using System.Collections.Generic;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public abstract class MainMenuViewBase : ViewBehaviour, IFocusableElement
	{
		[SerializeField]
		private LocalizationKey _createRoomLocalizationKey;

		[SerializeField]
		private LocalizationKey _joinRoomLocalizationKey;

		[SerializeField]
		private TMP_InputField _playerName;

		[SerializeField]
		private TMP_Text _errorMessage;

		[SerializeField]
		private TMP_Text _shutdownReason;

		[SerializeField]
		private TMP_Text _actionText;

		[SerializeField]
		private Button _joinSessionButton;

		[SerializeField]
		private Button _hostSessionButton;

		[SerializeField]
		private Button _creditsButton;

		[SerializeField]
		private Button _searchTeammatesButton;

		[SerializeField]
		private LocalizationKey _searchTeammatesLocalizationKey;

		[SerializeField]
		private TMP_Text _playerNameText;

		[SerializeField]
		private GameObject _errorPopup;

		[Header("China servers notice")]
		[SerializeField]
		private GameObject _chinaServersNoticeRoot;

		[SerializeField]
		private GameObject _chinaBannersRoot;

		[SerializeField]
		private TMP_Text _chinaServersNoticeText;

		public Button ErrorPopupCross;

		public Action OnJoinSessionButtonClick;

		public Action OnHostSessionButtonClick;

		public Action OnSearchTeammatesButtonClick;

		public Action OnCreditsButtonClick;

		public Action<string> OnPlayerNameChanged;

		[field: SerializeField]
		public List<Selectable> FirstButtonsToSelect { get; private set; }

		[field: SerializeField]
		public Selectable FirstErrorButtonToSelect { get; private set; }

		[field: SerializeField]
		public LocalizationKey ChinaServersNoticeLocalizationKey { get; private set; }

		[field: SerializeField]
		public Button ReconnectButton { get; private set; }

		[field: SerializeField]
		public Button StartBaseTutorialFirstTimeButton { get; private set; }

		[field: SerializeField]
		public Button StartBaseTutorialRegularButton { get; private set; }

		public LocalizationKey CreateRoomLocalizationKey => _createRoomLocalizationKey;

		public LocalizationKey JoinRoomLocalizationKey => _joinRoomLocalizationKey;

		public LocalizationKey SearchTeammatesLocalizationKey => _searchTeammatesLocalizationKey;

		public bool IsFocused { get; private set; }

		public bool IsFocusable { get; private set; }

		public event Action OnFocused;

		public event Action OnUnFocused;

		protected override void OnEnable()
		{
			base.OnEnable();
			_joinSessionButton.onClick.AddListener(InvokeOnJoinSessionButtonClick);
			_hostSessionButton.onClick.AddListener(InvokeOnHostSessionButtonClick);
			if (_searchTeammatesButton != null)
			{
				_searchTeammatesButton.onClick.AddListener(InvokeOnSearchTeammatesButtonClick);
			}
			if (_creditsButton != null)
			{
				_creditsButton.onClick.AddListener(InvokeOnCreditsButtonClick);
			}
			_playerName.onValueChanged.AddListener(InvokeOnPlayerNameChanged);
			EventSystem.current.SetSelectedGameObject(_playerName.gameObject);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_joinSessionButton.onClick.RemoveListener(InvokeOnJoinSessionButtonClick);
			_hostSessionButton.onClick.RemoveListener(InvokeOnHostSessionButtonClick);
			if (_searchTeammatesButton != null)
			{
				_searchTeammatesButton.onClick.RemoveListener(InvokeOnSearchTeammatesButtonClick);
			}
			if (_creditsButton != null)
			{
				_creditsButton.onClick.RemoveListener(InvokeOnCreditsButtonClick);
			}
			_playerName.onValueChanged.RemoveListener(InvokeOnPlayerNameChanged);
		}

		public Selectable GetFirstButtonToSelect()
		{
			if (FirstButtonsToSelect == null)
			{
				return null;
			}
			foreach (Selectable item in FirstButtonsToSelect)
			{
				if (item != null && item.IsInteractable() && item.gameObject.activeInHierarchy)
				{
					return item;
				}
			}
			return null;
		}

		public void SetPlayerName(string playerName)
		{
			if (_playerName != null)
			{
				_playerName.SetTextWithoutNotify(playerName);
			}
		}

		public void SetPlayerNameColor(Color color)
		{
			if (_playerNameText != null)
			{
				_playerNameText.color = color;
			}
		}

		public void SetJoinInteractable(bool interactable)
		{
			if (_joinSessionButton != null)
			{
				_joinSessionButton.interactable = interactable;
			}
		}

		public void SetHostInteractable(bool interactable)
		{
			if (_hostSessionButton != null)
			{
				_hostSessionButton.interactable = interactable;
			}
		}

		public void SetSearchTeammatesInteractable(bool interactable)
		{
			if (_searchTeammatesButton != null)
			{
				_searchTeammatesButton.interactable = interactable;
			}
		}

		public void SetNicknameInteractable(bool interactable)
		{
			if (_playerName != null)
			{
				_playerName.interactable = interactable;
			}
		}

		public void SetActionText(string text)
		{
			_actionText.text = text;
		}

		public void SetChinaServersNoticeVisible(bool isVisible)
		{
			_chinaServersNoticeRoot.SetActive(isVisible);
			_chinaBannersRoot.SetActive(isVisible);
		}

		public void SetChinaServersNoticeText(string text)
		{
			if (_chinaServersNoticeText != null)
			{
				_chinaServersNoticeText.text = text;
			}
		}

		private void InvokeOnJoinSessionButtonClick()
		{
			OnJoinSessionButtonClick?.Invoke();
		}

		private void InvokeOnHostSessionButtonClick()
		{
			OnHostSessionButtonClick?.Invoke();
		}

		private void InvokeOnCreditsButtonClick()
		{
			OnCreditsButtonClick?.Invoke();
		}

		private void InvokeOnSearchTeammatesButtonClick()
		{
			OnSearchTeammatesButtonClick?.Invoke();
		}

		private void InvokeOnPlayerNameChanged(string currentValue)
		{
			OnPlayerNameChanged?.Invoke(currentValue);
		}

		public void SetPopupActive(bool isActive)
		{
			if (!(_errorPopup == null))
			{
				_errorPopup.SetActive(isActive);
			}
		}

		public void SetShutdownReason(string shutdownReason)
		{
			_shutdownReason.text = shutdownReason;
		}

		public void SetErrorMessage(string errorMessage)
		{
			_errorMessage.text = errorMessage;
		}

		public void Focus(Action onInteract)
		{
			IsFocused = true;
		}

		public void UnFocus()
		{
			IsFocused = false;
		}

		public void MakeFocusable()
		{
			IsFocusable = true;
			this.OnFocused?.Invoke();
		}

		public void MakeUnFocusable()
		{
			IsFocusable = false;
			this.OnUnFocused?.Invoke();
		}
	}
}
