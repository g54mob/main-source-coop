using System;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class AllPlayersDownedPopup : GameCanvasGroup, IAllDownedPopup
	{
		[SerializeField]
		private TextMeshProUGUI messageText;

		[SerializeField]
		private Button confirmButton;

		[Tooltip("Optional: the Confirm button's text label (localized).")]
		[SerializeField]
		private TextMeshProUGUI confirmButtonLabel;

		[Inject]
		private ILocalizationService _localizationService;

		public event Action OnConfirmed;

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
			}
		}

		private void OnDestroy()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= ApplyLocalizedText;
			}
			if (confirmButton != null)
			{
				confirmButton.onClick.RemoveListener(OnConfirmClicked);
			}
		}

		public void Show()
		{
			ApplyLocalizedText();
			Show(interactable: true, blockRaycast: true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		private void OnConfirmClicked()
		{
			Hide();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			this.OnConfirmed?.Invoke();
		}

		private void ApplyLocalizedText()
		{
			if (_localizationService != null)
			{
				if (messageText != null)
				{
					messageText.text = _localizationService.Localize("@player.all_downed");
				}
				if (confirmButtonLabel != null)
				{
					confirmButtonLabel.text = _localizationService.Localize("@player.all_downed_confirm");
				}
			}
		}
	}
}
