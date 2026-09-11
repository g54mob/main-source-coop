using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zorro.PhotonUtility;

public class EscapePlayerCellUI : Selectable
{
	public Image playerIcon;

	public TextMeshProUGUI playerName;

	public Slider playerVoiceVolume;

	public Button muteButton;

	public Button kickButton;

	public Button blockButton;

	public Button reportButton;

	public Toggle muteToggle;

	private GlobalPlayerData globalPlayerData;

	private Photon.Realtime.Player m_player;

	private bool loadedVolume;

	private bool loadedSprite;

	private bool loadingSprite;

	private bool m_selected;

	private Player.PlayerInput m_input;

	private TMP_Text muteButtonText;

	public void Setup(EscapePlayerHandler handler, Photon.Realtime.Player player)
	{
		m_player = player;
		playerName.text = player.NickName;
		m_input = Player.localPlayer?.input;
		kickButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
		kickButton.onClick.AddListener(KickClicked);
		muteButton.onClick.AddListener(OnMuteToggled);
		muteButtonText = muteButton.GetComponentInChildren<TMP_Text>();
		muteToggle.interactable = false;
		blockButton.transform.parent.gameObject.SetActive(value: false);
		if (SteamAvatarHandler.TryGetAvatarForPlayer(player, out var icon))
		{
			loadedSprite = true;
			playerIcon.sprite = icon;
			FlipSpriteImage();
		}
		LocalizationKeys.OnLanguageChanged += UpdateButtonLabels;
		UpdateButtonLabels();
	}

	private IEnumerator SetupPlatformFeatures()
	{
		yield return null;
	}

	private void UpdateButtonLabels()
	{
		if (muteButtonText != null)
		{
			LocalizationKeys.Keys key = (muteToggle.isOn ? LocalizationKeys.Keys.PlayerCell_MuteButton_Unmute : LocalizationKeys.Keys.PlayerCell_MuteButton_Mute);
			muteButtonText.text = LocalizationKeys.GetLocalizedString(key);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		m_selected = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void UpdateCell()
	{
		if (!loadedSprite && !loadingSprite)
		{
			LoadPlayerSprite();
		}
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (globalPlayerData == null && !GlobalPlayerData.TryGetPlayerData(m_player, out globalPlayerData))
		{
			Debug.LogError("Failed to get player data for player " + m_player?.NickName);
			return;
		}
		if (!loadedVolume)
		{
			playerVoiceVolume.value = globalPlayerData.localVoiceVolume;
			loadedVolume = true;
		}
		if (m_input == null)
		{
			m_input = Player.localPlayer?.input;
		}
		if (m_selected && m_input != null)
		{
			if (m_input.jumpAction.action.WasPressedThisFrame())
			{
				KickClicked();
			}
			if (m_input.interactAction.action.WasPressedThisFrame())
			{
				OnMuteToggled();
			}
		}
		globalPlayerData.localVoiceVolume = playerVoiceVolume.value;
		playerVoiceVolume.interactable = globalPlayerData.canCommunicateWith;
		muteButton.gameObject.SetActive(globalPlayerData.canCommunicateWith);
		UpdateButtonLabels();
		muteToggle.isOn = globalPlayerData.isMuted;
	}

	private void KickClicked()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_KickPlayer);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_AreYouSure);
			string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
			string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
			Modal.Show(localizedString.Replace("{Name}", m_player.NickName), localizedString2, new ModalOption[2]
			{
				new ModalOption(localizedString3, KickPlayer),
				new ModalOption(localizedString4)
			});
		}
	}

	private void KickPlayer()
	{
		Debug.Log("Sending kick event to server plugin for " + m_player);
		CustomCommands<CustomCommandType>.SendPackage(new KickPlayerNotificationPackage(), m_player);
		PhotonNetwork.RaiseEvent(20, m_player.ActorNumber, RaiseEventOptions.Default, SendOptions.SendReliable);
	}

	private void OnMuteToggled()
	{
		bool flag = !globalPlayerData.isMuted;
		if (!GlobalPlayerData.TryGetPlayerData(m_player, out globalPlayerData))
		{
			Debug.LogError("Failed to get player data for " + m_player.NickName);
			return;
		}
		if (!globalPlayerData.canCommunicateWith)
		{
			Debug.Log("Can't communicate with player: " + m_player.NickName);
			return;
		}
		Debug.Log((flag ? "Muting" : "Unmuting") + " player: " + m_player.NickName);
		globalPlayerData.isMuted = flag;
	}

	private void LoadPlayerSprite()
	{
		if (SteamAvatarHandler.TryGetAvatarForPlayer(m_player, out var icon))
		{
			loadedSprite = true;
			playerIcon.sprite = icon;
			FlipSpriteImage();
		}
	}

	private void FlipSpriteImage()
	{
		RectTransform rectTransform = (RectTransform)playerIcon.transform;
		Vector3 localScale = rectTransform.localScale;
		Vector3 localPosition = rectTransform.localPosition;
		localScale.y = 0f - localScale.y;
		rectTransform.localScale = localScale;
		localPosition.y -= rectTransform.sizeDelta.y;
		rectTransform.localPosition = localPosition;
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		m_selected = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		m_selected = false;
	}

	public override void OnMove(AxisEventData eventData)
	{
		switch (eventData.moveDir)
		{
		case MoveDirection.Right:
			if (playerVoiceVolume.interactable)
			{
				playerVoiceVolume.value += 0.1f;
			}
			break;
		case MoveDirection.Left:
			if (playerVoiceVolume.interactable)
			{
				playerVoiceVolume.value -= 0.1f;
			}
			break;
		default:
			base.OnMove(eventData);
			break;
		}
	}
}
