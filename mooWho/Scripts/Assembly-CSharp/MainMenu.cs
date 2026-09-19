using System.Collections.Generic;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu instance;

	public MenuState state;

	[Header("UI Panels")]
	[SerializeField]
	private GameObject invite_friend_panel;

	[Header("Party UI Elements")]
	[SerializeField]
	private GameObject btn01_ready_party;

	[SerializeField]
	private GameObject btn01_startgame_party;

	[SerializeField]
	private GameObject btn01_leaveparty_party;

	[SerializeField]
	private GameObject pnl_friends_party;

	[Header("Party Slot Buttons")]
	[SerializeField]
	private Button btn04_friends_1;

	[SerializeField]
	private Button btn04_friends_2;

	[SerializeField]
	private Button btn04_friends_3;

	[SerializeField]
	private Button btn04_friends_4;

	[Header("Home UI Elements")]
	[SerializeField]
	private GameObject btn01_singleplayer;

	[SerializeField]
	private GameObject btn01_findmatch;

	[SerializeField]
	private GameObject btn01_createparty;

	[SerializeField]
	private GameObject btn01_credits;

	[Header("Ready Button")]
	[SerializeField]
	private Image readyButton_Image;

	[SerializeField]
	private TMP_Text readyButton_Text;

	public Color readyColor;

	public Color notReadyColor;

	[Header("Default Avatar")]
	public Sprite TransparentBox;

	private Dictionary<int, PlayerSlotData> playerSlots = new Dictionary<int, PlayerSlotData>();

	private Dictionary<int, Image> slotAvatarImages = new Dictionary<int, Image>();

	private Dictionary<CSteamID, int> steamIDToSlot = new Dictionary<CSteamID, int>();

	private void Awake()
	{
		instance = this;
		InitializePartySlots();
	}

	private void InitializePartySlots()
	{
		if (btn04_friends_1 != null)
		{
			btn04_friends_1.onClick.AddListener(delegate
			{
				OnPartySlotClicked(0);
			});
			slotAvatarImages[0] = FindAvatarImage(btn04_friends_1);
		}
		if (btn04_friends_2 != null)
		{
			btn04_friends_2.onClick.AddListener(delegate
			{
				OnPartySlotClicked(1);
			});
			slotAvatarImages[1] = FindAvatarImage(btn04_friends_2);
		}
		if (btn04_friends_3 != null)
		{
			btn04_friends_3.onClick.AddListener(delegate
			{
				OnPartySlotClicked(2);
			});
			slotAvatarImages[2] = FindAvatarImage(btn04_friends_3);
		}
		if (btn04_friends_4 != null)
		{
			btn04_friends_4.onClick.AddListener(delegate
			{
				OnPartySlotClicked(3);
			});
			slotAvatarImages[3] = FindAvatarImage(btn04_friends_4);
		}
		for (int num = 0; num < 4; num++)
		{
			playerSlots[num] = new PlayerSlotData();
		}
	}

	private Image FindAvatarImage(Button button)
	{
		if (button == null)
		{
			return null;
		}
		Image[] componentsInChildren = button.GetComponentsInChildren<Image>(includeInactive: true);
		foreach (Image image in componentsInChildren)
		{
			if (image.name == "AvatarImage")
			{
				return image;
			}
		}
		Debug.LogWarning("AvatarImage not found in button: " + button.name);
		return null;
	}

	private void OnPartySlotClicked(int slotIndex)
	{
		if (!playerSlots[slotIndex].isOccupied && invite_friend_panel != null)
		{
			invite_friend_panel.SetActive(value: true);
		}
	}

	public void AddPlayerToParty(CSteamID steamID)
	{
		if (steamIDToSlot.ContainsKey(steamID))
		{
			Debug.Log($"Player {steamID} already in party");
			return;
		}
		int num = FindFirstEmptySlot();
		if (num == -1)
		{
			Debug.LogWarning("No empty slots available!");
			return;
		}
		Sprite avatar = SteamHelper.ConvertTextureToSprite(SteamHelper.GetAvatar(steamID));
		UpdatePartySlot(num, steamID, avatar);
		steamIDToSlot[steamID] = num;
		Debug.Log($"Player {steamID} added to slot {num}");
	}

	public void RemovePlayerFromParty(CSteamID steamID)
	{
		if (!steamIDToSlot.ContainsKey(steamID))
		{
			Debug.Log($"Player {steamID} not found in party");
			return;
		}
		int num = steamIDToSlot[steamID];
		ClearPartySlot(num);
		steamIDToSlot.Remove(steamID);
		Debug.Log($"Player {steamID} removed from slot {num}");
	}

	private int FindFirstEmptySlot()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!playerSlots[i].isOccupied)
			{
				return i;
			}
		}
		return -1;
	}

	public void UpdatePartySlot(int slotIndex, CSteamID steamID, Sprite avatar)
	{
		if (slotIndex < 0 || slotIndex > 3)
		{
			return;
		}
		playerSlots[slotIndex].isOccupied = true;
		playerSlots[slotIndex].steamID = steamID;
		Button slotButton = GetSlotButton(slotIndex);
		if (slotButton != null)
		{
			slotButton.interactable = false;
			if (slotAvatarImages.ContainsKey(slotIndex) && slotAvatarImages[slotIndex] != null && avatar != null)
			{
				slotAvatarImages[slotIndex].sprite = avatar;
			}
		}
	}

	public void ClearPartySlot(int slotIndex)
	{
		if (slotIndex < 0 || slotIndex > 3)
		{
			return;
		}
		playerSlots[slotIndex].isOccupied = false;
		playerSlots[slotIndex].steamID = CSteamID.Nil;
		Button slotButton = GetSlotButton(slotIndex);
		if (slotButton != null)
		{
			slotButton.interactable = true;
			if (slotAvatarImages.ContainsKey(slotIndex) && slotAvatarImages[slotIndex] != null)
			{
				slotAvatarImages[slotIndex].sprite = TransparentBox;
			}
		}
	}

	private Button GetSlotButton(int slotIndex)
	{
		return slotIndex switch
		{
			0 => btn04_friends_1, 
			1 => btn04_friends_2, 
			2 => btn04_friends_3, 
			3 => btn04_friends_4, 
			_ => null, 
		};
	}

	public void SetMenuState(MenuState state)
	{
		this.state = state;
		UpdateUIElements(state);
		if (state == MenuState.InParty)
		{
			SetLocalPlayerToFirstSlot();
		}
		else
		{
			ClearAllSlots();
		}
	}

	private void SetLocalPlayerToFirstSlot()
	{
		CSteamID steamID = SteamUser.GetSteamID();
		if (!steamIDToSlot.ContainsKey(steamID))
		{
			AddPlayerToParty(steamID);
		}
	}

	private void ClearAllSlots()
	{
		for (int i = 0; i < 4; i++)
		{
			ClearPartySlot(i);
		}
		steamIDToSlot.Clear();
	}

	private void UpdateUIElements(MenuState state)
	{
		bool flag = state == MenuState.InParty;
		if (btn01_ready_party != null)
		{
			btn01_ready_party.SetActive(flag);
		}
		if (btn01_startgame_party != null)
		{
			btn01_startgame_party.SetActive(flag);
		}
		if (btn01_leaveparty_party != null)
		{
			btn01_leaveparty_party.SetActive(flag);
		}
		if (pnl_friends_party != null)
		{
			pnl_friends_party.SetActive(flag);
		}
		if (btn01_singleplayer != null)
		{
			btn01_singleplayer.SetActive(!flag);
		}
		if (btn01_findmatch != null)
		{
			btn01_findmatch.SetActive(!flag);
		}
		if (btn01_createparty != null)
		{
			btn01_createparty.SetActive(!flag);
		}
		if (btn01_credits != null)
		{
			btn01_credits.SetActive(!flag);
		}
	}

	public void CreateParty()
	{
		PopupManager.instance.Popup_Show("Creating Party");
		((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(value: true);
		SteamLobby.instance.CreateLobby();
	}

	public void StartSinglePlayer()
	{
		LobbyController.instance.StartGameSolo();
	}

	public void LeaveParty()
	{
		if (NetworkClient.active)
		{
			if (NetworkClient.localPlayer.isServer)
			{
				NetworkManager.singleton.StopHost();
			}
			else
			{
				NetworkManager.singleton.StopClient();
			}
			SteamLobby.instance.Leave();
			ClearAllSlots();
		}
	}

	public void FindMatch()
	{
		SteamLobby.instance.FindMatch();
	}

	public void StartGame()
	{
		LobbyController.instance.StartGameWithParty();
	}

	public void StartLocalClient()
	{
		((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(value: true);
		NetworkManager.singleton.StartClient();
	}

	public void StartLocalHost()
	{
		((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(value: true);
		NetworkManager.singleton.StartHost();
	}

	public void ToggleReady()
	{
		if (NetworkClient.active)
		{
			NetworkClient.localPlayer.GetComponent<MyClient>().ToggleReady();
		}
	}

	public void UpdateReadyButton(bool value)
	{
		readyButton_Text.text = (value ? "Ready" : "Not Ready");
		readyButton_Image.color = (value ? readyColor : notReadyColor);
	}
}
