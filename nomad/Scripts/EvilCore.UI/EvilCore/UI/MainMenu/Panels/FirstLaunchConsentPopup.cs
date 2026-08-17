using System.Collections;
using EvilAnalytics.SDK.Core;
using EvilCore.EvilSave;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class FirstLaunchConsentPopup : MainMenuCanvasGroup
	{
		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private TextMeshProUGUI bodyText;

		[SerializeField]
		private TextMeshProUGUI acceptLabel;

		[SerializeField]
		private TextMeshProUGUI declineLabel;

		[SerializeField]
		private Button acceptButton;

		[SerializeField]
		private Button declineButton;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IMainMenuUIManager _uiManager;

		private const string SeenKey = "ui.firstlaunch.consent.seen";

		private const string ConsentKey = "ui.firstlaunch.consent.dataCollection";

		private Coroutine _applyConsentRoutine;

		private bool _subscribedToLocale;

		public bool ShouldShow()
		{
			return !EvilCore.EvilSave.EvilSave.Prefs.GetBool("ui.firstlaunch.consent.seen");
		}

		private void Awake()
		{
			if (acceptButton != null)
			{
				acceptButton.onClick.AddListener(OnAcceptClicked);
			}
			if (declineButton != null)
			{
				declineButton.onClick.AddListener(OnDeclineClicked);
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
			if (EvilCore.EvilSave.EvilSave.Prefs.GetBool("ui.firstlaunch.consent.seen"))
			{
				ApplyConsentToAnalytics(EvilCore.EvilSave.EvilSave.Prefs.GetBool("ui.firstlaunch.consent.dataCollection"));
			}
		}

		private void OnDestroy()
		{
			if (_subscribedToLocale && _localizationService != null)
			{
				_localizationService.OnLocaleChanged -= ApplyLocalizedText;
			}
			if (acceptButton != null)
			{
				acceptButton.onClick.RemoveListener(OnAcceptClicked);
			}
			if (declineButton != null)
			{
				declineButton.onClick.RemoveListener(OnDeclineClicked);
			}
		}

		private void OnAcceptClicked()
		{
			CompleteConsent(granted: true);
		}

		private void OnDeclineClicked()
		{
			CompleteConsent(granted: false);
		}

		private void CompleteConsent(bool granted)
		{
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("ui.firstlaunch.consent.dataCollection", granted);
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("ui.firstlaunch.consent.seen", value: true);
			ApplyConsentToAnalytics(granted);
			_uiManager?.ShowMainPanel();
		}

		private void ApplyConsentToAnalytics(bool granted)
		{
			if (Analytics.IsInitialized)
			{
				SetAnalyticsConsent(granted);
				return;
			}
			if (_applyConsentRoutine != null)
			{
				StopCoroutine(_applyConsentRoutine);
			}
			_applyConsentRoutine = StartCoroutine(ApplyConsentWhenReady(granted));
		}

		private IEnumerator ApplyConsentWhenReady(bool granted)
		{
			float timeout = 30f;
			while (!Analytics.IsInitialized && timeout > 0f)
			{
				timeout -= Time.unscaledDeltaTime;
				yield return null;
			}
			if (Analytics.IsInitialized)
			{
				SetAnalyticsConsent(granted);
			}
			_applyConsentRoutine = null;
		}

		private static void SetAnalyticsConsent(bool granted)
		{
			if (granted)
			{
				Analytics.GrantConsent();
			}
			else
			{
				Analytics.RevokeConsent();
			}
		}

		private void ApplyLocalizedText()
		{
			if (titleText != null)
			{
				titleText.text = Localize("@first_launch.title");
			}
			if (bodyText != null)
			{
				bodyText.text = Localize("@first_launch.body");
			}
			if (acceptLabel != null)
			{
				acceptLabel.text = Localize("@first_launch.accept");
			}
			if (declineLabel != null)
			{
				declineLabel.text = Localize("@first_launch.decline");
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
