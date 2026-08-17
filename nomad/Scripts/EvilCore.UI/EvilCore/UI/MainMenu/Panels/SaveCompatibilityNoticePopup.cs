using EvilCore.EvilSave;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class SaveCompatibilityNoticePopup : MainMenuCanvasGroup
	{
		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private TextMeshProUGUI bodyText;

		[SerializeField]
		private TextMeshProUGUI okLabel;

		[SerializeField]
		private Button okButton;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private IGameSaveService _gameSaveService;

		private const string SeenKey = "ui.savenotice.seen8";

		private bool _subscribedToLocale;

		public bool ShouldShow()
		{
			if (!EvilCore.EvilSave.EvilSave.Prefs.GetBool("ui.savenotice.seen8"))
			{
				return HasAnySave();
			}
			return false;
		}

		private bool HasAnySave()
		{
			if (_gameSaveService != null)
			{
				return _gameSaveService.GetSaveSlots().Length != 0;
			}
			return false;
		}

		private void Awake()
		{
			if (okButton != null)
			{
				okButton.onClick.AddListener(OnDismissClicked);
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
		}

		private void OnDestroy()
		{
			if (_subscribedToLocale && _localizationService != null)
			{
				_localizationService.OnLocaleChanged -= ApplyLocalizedText;
			}
			if (okButton != null)
			{
				okButton.onClick.RemoveListener(OnDismissClicked);
			}
		}

		private void OnDismissClicked()
		{
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("ui.savenotice.seen8", value: true);
			_uiManager?.ShowMainPanel();
		}

		private void ApplyLocalizedText()
		{
			if (titleText != null)
			{
				titleText.text = Localize("@save_notice.title");
			}
			if (bodyText != null)
			{
				bodyText.text = Localize("@save_notice.body");
			}
			if (okLabel != null)
			{
				okLabel.text = Localize("@save_notice.ok");
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
	}
}
