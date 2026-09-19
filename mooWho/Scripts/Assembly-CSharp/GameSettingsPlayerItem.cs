using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsPlayerItem : MonoBehaviour
{
	private Image _avatar;

	private TextMeshProUGUI _usernameText;

	private Button _playRecordButton;

	private Button _kickButton;

	private MyClient _client;

	private PlayerRoleData _roleData;

	private AudioSource _previewSource;

	private void Awake()
	{
		_avatar = base.transform.Find("Avatar")?.GetComponent<Image>();
		_usernameText = base.transform.Find("UsernameText")?.GetComponent<TextMeshProUGUI>();
		_playRecordButton = base.transform.Find("PlayRecordButton")?.GetComponent<Button>();
		_kickButton = base.transform.Find("KickButton")?.GetComponent<Button>();
		if (_playRecordButton != null)
		{
			_playRecordButton.onClick.AddListener(OnPlayRecordClicked);
		}
		if (_kickButton != null)
		{
			_kickButton.onClick.AddListener(OnKickClicked);
		}
		_previewSource = base.gameObject.AddComponent<AudioSource>();
		_previewSource.playOnAwake = false;
		_previewSource.spatialBlend = 0f;
	}

	public void Setup(MyClient client, PlayerRoleData roleData, Sprite avatarSprite)
	{
		_client = client;
		_roleData = roleData;
		if (_avatar != null)
		{
			_avatar.sprite = avatarSprite;
		}
		if (_usernameText != null)
		{
			_usernameText.text = ((client != null) ? client.playerInfo.username : "?");
		}
		bool flag = client != null && client.isLocalPlayer;
		bool active = NetworkServer.active;
		bool flag2 = roleData != null && roleData.Role == PlayerRole.Animal && VoiceClipStore.Instance != null && VoiceClipStore.Instance.HasClip(roleData.AssignedAnimal);
		if (_playRecordButton != null)
		{
			_playRecordButton.gameObject.SetActive(active && !flag && flag2 && GameSettingsPanelUI.HostCanPreviewSoundRecords);
		}
		if (_kickButton != null)
		{
			_kickButton.gameObject.SetActive(active && !flag);
		}
	}

	private void OnPlayRecordClicked()
	{
		if (!(_roleData == null) && !(VoiceClipStore.Instance == null))
		{
			AudioClip clip = VoiceClipStore.Instance.GetClip(_roleData.AssignedAnimal);
			if (clip != null)
			{
				_previewSource.PlayOneShot(clip);
			}
		}
	}

	private void OnKickClicked()
	{
		_client?.ServerKick();
	}
}
