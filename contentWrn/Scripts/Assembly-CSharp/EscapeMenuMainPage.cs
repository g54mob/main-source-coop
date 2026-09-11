using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;

public class EscapeMenuMainPage : EscapeMenuPage, INavigationPage
{
	public Button resumeButton;

	public Button settingsButton;

	public Button inviteButton;

	public Button exitButton;

	public TMP_InputField roomCode;

	private GameObject roomCodeParent;

	private void Awake()
	{
		settingsButton.onClick.AddListener(OnSettingsButtonClicked);
		resumeButton.onClick.AddListener(OnResumeButtonClicked);
		inviteButton.onClick.AddListener(OnInviteButtonClicked);
		exitButton.onClick.AddListener(OnExitButtonClicked);
		roomCodeParent = roomCode.transform.parent.gameObject;
	}

	private void Start()
	{
		roomCode.text = PhotonNetwork.CurrentRoom.Name;
	}

	private void Update()
	{
		bool active = (bool)SurfaceNetworkHandler.Instance && !SurfaceNetworkHandler.HasStarted && !PhotonNetwork.OfflineMode;
		if (MainMenuHandler.SteamLobbyHandler == null)
		{
			active = false;
		}
		inviteButton.gameObject.SetActive(active);
		roomCodeParent.SetActive(active);
	}

	private void OnExitButtonClicked()
	{
		string title = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Quit) + "?";
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_AreYouSure);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
		ModalOption[] options = new ModalOption[2]
		{
			new ModalOption(localizedString2)
			{
				OnClick = RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect
			},
			new ModalOption(localizedString3)
		};
		Modal.Show(title, localizedString, options);
	}

	private void OnInviteButtonClicked()
	{
		if (MainMenuHandler.SteamLobbyHandler != null)
		{
			MainMenuHandler.SteamLobbyHandler.InviteScreen();
			if (PhotonNetwork.InRoom)
			{
				Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
				Debug.Log("In Current Room: " + currentRoom.Name);
			}
			else
			{
				Debug.LogError("User is not in a room");
			}
		}
	}

	private void OnResumeButtonClicked()
	{
		Singleton<EscapeMenu>.Instance.Toggle();
	}

	private void OnSettingsButtonClicked()
	{
		pageHandler.TransistionToPage<EscapeMenuSettingsPage>();
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return resumeButton.gameObject;
	}
}
