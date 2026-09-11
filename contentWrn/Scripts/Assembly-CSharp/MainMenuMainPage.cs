using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;

public class MainMenuMainPage : MainMenuPage, INavigationPage
{
	[SerializeField]
	private GameObject mainMenuHolder;

	[SerializeField]
	private bool CalibrationScreenEnabled;

	public Button hostButton;

	public Button joinButton;

	public Button joinRoomButton;

	public Button settingsButton;

	public Button creditsButton;

	public Button modManagerButton;

	public Button quitButton;

	public TextMeshProUGUI versionText;

	private void Awake()
	{
		settingsButton.onClick.AddListener(OnSettingsButtonClicked);
		hostButton.onClick.AddListener(OnHostButtonClicked);
		joinButton.onClick.AddListener(OnJoinButtonClicked);
		joinRoomButton.onClick.AddListener(OnJoinRoomButtonClicked);
		creditsButton.onClick.AddListener(OnCreditsButtonClicked);
		modManagerButton.onClick.AddListener(OnModManagerButtonClicked);
		quitButton.onClick.AddListener(OnQuitButtonClicked);
		quitButton.gameObject.SetActive(value: true);
		versionText.text = Application.version;
		if (PlayerPrefs.GetInt("FirstTime10", 0) == 0)
		{
			mainMenuHolder.SetActive(value: false);
			CalibrationScreenEnabled = true;
			CalibrationScreen.ShowVoiceChatScreen(delegate
			{
				PlayerPrefs.SetInt("FirstTime10", 1);
				RetrievableSingleton<CheckVersionHandler>.Instance.CheckVersion();
			});
			RetrievableResourceSingleton<CalibrationScreen>.Instance.mainPage = this;
		}
		else
		{
			RetrievableSingleton<CheckVersionHandler>.Instance.CheckVersion();
		}
	}

	public override void OnPageEnter()
	{
		modManagerButton.gameObject.SetActive(PluginHandler.AllPlugins.Count > 0);
		base.OnPageEnter();
	}

	public void EnableMain()
	{
		CalibrationScreenEnabled = false;
		mainMenuHolder.SetActive(value: true);
	}

	private void OnCreditsButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuCreditsPage>();
	}

	private void OnQuitButtonClicked()
	{
		string title = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CloseGame) + "?";
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_AreYouSure);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
		ModalOption[] options = new ModalOption[2]
		{
			new ModalOption(localizedString2)
			{
				OnClick = Application.Quit
			},
			new ModalOption(localizedString3)
		};
		Modal.Show(title, localizedString, options);
	}

	private void OnJoinButtonClicked()
	{
		MainMenuHandler.Instance.CanPlayMultiplayer(showModal: false, delegate(bool result)
		{
			if (result)
			{
				MainMenuHandler.Instance.JoinRandom();
			}
		});
	}

	private void OnJoinRoomButtonClicked()
	{
		MainMenuHandler.Instance.CanPlayMultiplayer(showModal: false, delegate(bool result)
		{
			if (result)
			{
				pageHandler.TransistionToPage<MainMenuJoinRoomPage>();
			}
		});
	}

	private void OnHostButtonClicked()
	{
		MainMenuHandler.Instance.CanPlayMultiplayer(showModal: true, delegate
		{
			pageHandler.TransistionToPage<MainMenuHostPage>();
		});
	}

	private void OnSettingsButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuSettingsPage>();
	}

	private void OnModManagerButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuModManagerPage>();
	}

	public GameObject GetFirstSelectedGameObject()
	{
		if (CalibrationScreenEnabled)
		{
			return RetrievableResourceSingleton<CalibrationScreen>.Instance.GetFirstSelectedGameObject();
		}
		return hostButton.gameObject;
	}
}
