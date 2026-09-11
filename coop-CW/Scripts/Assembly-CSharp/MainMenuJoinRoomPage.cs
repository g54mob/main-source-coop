using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.UI;

public class MainMenuJoinRoomPage : MainMenuPage, INavigationPage, IHaveParentPage
{
	public TMP_Text header;

	public TMP_Text info;

	public TMP_InputField roomInput;

	public Button joinButton;

	public Button backButton;

	private void Awake()
	{
	}

	public override void OnPageEnter()
	{
		base.OnPageEnter();
		if (SetupPage())
		{
			OpenDialog();
		}
	}

	private bool SetupPage()
	{
		if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
		{
			if (!steamRuntimeManager.UsingBigPictureMode && !steamRuntimeManager.OnSteamDeck)
			{
				SetupKeyboardMouse();
				return false;
			}
			if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.KeyboardMouse)
			{
				SetupKeyboardMouse();
				return false;
			}
		}
		SetupGamepad();
		return true;
	}

	private void SetupKeyboardMouse()
	{
		header.gameObject.SetActive(value: true);
		info.gameObject.SetActive(value: true);
		roomInput.onSubmit = new TMP_InputField.SubmitEvent();
		roomInput.onSubmit.AddListener(delegate
		{
			joinButton.Select();
		});
		roomInput.gameObject.SetActive(value: true);
		joinButton.onClick = new Button.ButtonClickedEvent();
		joinButton.onClick.AddListener(OnJoinButtonClicked);
		joinButton.gameObject.SetActive(value: true);
		backButton.onClick = new Button.ButtonClickedEvent();
		backButton.onClick.AddListener(OnBackButtonClicked);
		backButton.gameObject.SetActive(value: true);
	}

	private void SetupGamepad()
	{
		header.gameObject.SetActive(value: false);
		info.gameObject.SetActive(value: false);
		roomInput.gameObject.SetActive(value: false);
		joinButton.gameObject.SetActive(value: false);
		backButton.gameObject.SetActive(value: false);
	}

	private void OnJoinButtonClicked()
	{
		JoinRoom(roomInput.text);
	}

	private void OnBackButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuMainPage>();
	}

	private void OpenDialog()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.EnterRoomCode);
		PlatformManager.Platform.OpenDialog(localizedString, DialogType.JoinRoom, delegate(string room, DialogueResult result)
		{
			switch (result)
			{
			case DialogueResult.Succeeded:
				JoinRoom(room);
				break;
			case DialogueResult.Failed:
				OpenModal(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DialogueFailed), "");
				break;
			case DialogueResult.Aborted:
				OnBackButtonClicked();
				break;
			}
		});
	}

	private void JoinRoom(string room)
	{
		if (string.IsNullOrEmpty(room) || room.Length != roomInput.characterLimit)
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.InvalidRoomCode);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.InvalidRoomCodeHint);
			localizedString2 = string.Format(localizedString2, roomInput.characterLimit);
			OpenModal(localizedString, localizedString2);
		}
		else
		{
			MainMenuHandler.Instance.JoinRoom(room);
		}
	}

	private void OpenModal(string title, string message)
	{
		ModalOption[] options = ((!(PlatformManager.Platform is SteamRuntimeManager { UsingBigPictureMode: false })) ? new ModalOption[2]
		{
			new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Retry), OpenDialog),
			new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel), OnBackButtonClicked)
		} : new ModalOption[1]
		{
			new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok), roomInput.Select)
		});
		Modal.Show(title, message, options);
	}

	public GameObject GetFirstSelectedGameObject()
	{
		if (!(PlatformManager.Platform is SteamRuntimeManager { UsingBigPictureMode: false }))
		{
			return null;
		}
		return joinButton.gameObject;
	}

	public (UIPage, PageTransistion) GetParentPage()
	{
		if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
		{
			steamRuntimeManager.CloseDialog();
		}
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			return (pageHandler.GetPage<MainMenuMainPage>(), new SetActivePageTransistion());
		}
		if (EventSystem.current.currentSelectedGameObject != roomInput.gameObject)
		{
			return (pageHandler.GetPage<MainMenuMainPage>(), new SetActivePageTransistion());
		}
		joinButton.Select();
		return (this, new SetActivePageTransistion());
	}
}
