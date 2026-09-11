using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Unity;
using Portningsbolaget.Platforms;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class PlayerVoiceHandler : MonoBehaviour
{
	public AnimationCurve m_boostedVolumeOverDistance = AnimationCurve.EaseInOut(0f, 1f, 20f, 0f);

	private Recorder m_Recorder;

	public AudioSource audioSource;

	public Player player;

	private GlobalPlayerData m_GlobalPlayerData;

	private PhotonView m_PhotonView;

	private float m_IgnoreTimer;

	private string m_setMicrophoneDevice;

	private VoiceSetting m_VoiceSetting;

	private VoiceChatModeSetting m_VoiceChatModeSetting;

	private bool m_currentlyTransmitting;

	private bool m_allowedToCommunicate;

	private float defaultMaxDistance;

	public static PlayerVoiceHandler Instance { get; private set; }

	public bool AllowedToCommunicate => m_allowedToCommunicate;

	public bool Initialized { get; private set; }

	private void Awake()
	{
		m_Recorder = GetComponent<Recorder>();
		m_currentlyTransmitting = m_Recorder.TransmitEnabled;
		m_PhotonView = GetComponent<PhotonView>();
		PlayerHandler instance = PlayerHandler.instance;
		instance.OnPlayerJoined = (Action<Player>)Delegate.Combine(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
		if (player.refs.view.IsMine)
		{
			Instance = this;
		}
	}

	private IEnumerator Start()
	{
		yield return 2;
		m_VoiceSetting = GameHandler.Instance.SettingsHandler.GetSetting<VoiceSetting>();
		m_VoiceChatModeSetting = GameHandler.Instance.SettingsHandler.GetSetting<VoiceChatModeSetting>();
		player.TryGetGlobalPlayerData(out m_GlobalPlayerData);
		if (player.refs.view.IsMine)
		{
			PlatformManager.Platform.CanCommunicate(delegate(bool allowed)
			{
				m_allowedToCommunicate = allowed;
				Debug.Log($"PlayerVoiceHandler: allowedToCommunicate = {m_allowedToCommunicate}");
				CheckCommunicationPrivilegesOnAllPlayers();
				Initialized = true;
				m_GlobalPlayerData.Initialized = true;
			});
		}
		else
		{
			if (m_GlobalPlayerData.Initialized)
			{
				m_allowedToCommunicate = !m_GlobalPlayerData.isMuted;
			}
			else
			{
				m_allowedToCommunicate = true;
			}
			Initialized = true;
		}
		SetUserName(player);
	}

	private void OnDestroy()
	{
		PlayerHandler instance = PlayerHandler.instance;
		instance.OnPlayerJoined = (Action<Player>)Delegate.Remove(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
		if (player.refs.view.IsMine)
		{
			Instance = null;
		}
	}

	private void OnEnable()
	{
		AudioSettings.OnAudioConfigurationChanged += AudioConfigChanged;
		PlayerHandler instance = PlayerHandler.instance;
		instance.OnPlayerJoined = (Action<Player>)Delegate.Combine(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
	}

	private void OnDisable()
	{
		AudioSettings.OnAudioConfigurationChanged -= AudioConfigChanged;
		PlayerHandler instance = PlayerHandler.instance;
		instance.OnPlayerJoined = (Action<Player>)Delegate.Remove(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
	}

	private void AudioConfigChanged(bool devicewaschanged)
	{
		Debug.Log("Audio config changed, restarting recording.");
		m_Recorder.RestartRecording();
	}

	public void Ignore()
	{
		m_IgnoreTimer = 1f;
	}

	private void Update()
	{
		PushToTalk();
		float distance;
		bool flag = SFX_Player.GetBoostValue(base.transform.position, out distance) - 1f > 0.5f;
		float num = m_boostedVolumeOverDistance.Evaluate(distance);
		if (m_IgnoreTimer > 0f)
		{
			DecrementIgnoreTimer();
			if (m_IgnoreTimer > 0f)
			{
				return;
			}
		}
		if (player.refs.view.IsMine && m_VoiceSetting != null)
		{
			if (!string.IsNullOrEmpty(m_VoiceSetting.Value.id) && m_VoiceSetting.ChoiceCount == 0)
			{
				m_VoiceSetting.Value = default(VoiceSetting.MicrophoneInfo);
			}
			string id = m_VoiceSetting.Value.id;
			if (id != m_setMicrophoneDevice && !string.IsNullOrEmpty(id))
			{
				m_setMicrophoneDevice = id;
				m_Recorder.MicrophoneDevice = new DeviceInfo(id);
				Debug.Log("Setting microphone to " + id);
			}
		}
		float rangeFactor;
		float obstructionValue = AudioObstructability.GetObstructionValue(base.transform.position, 0.8f, out rangeFactor);
		bool dead = Player.localPlayer.data.dead;
		bool dead2 = player.data.dead;
		bool flag2 = !(dead2 && dead);
		if (flag)
		{
			flag2 = false;
		}
		audioSource.spatialBlend = (flag2 ? 1 : 0);
		float num2 = 1f;
		if (!(m_GlobalPlayerData == null) || player.TryGetGlobalPlayerData(out m_GlobalPlayerData))
		{
			num2 = m_GlobalPlayerData.localVoiceVolume;
			if (flag)
			{
				num2 = num * m_GlobalPlayerData.localVoiceVolume;
			}
		}
		if (m_allowedToCommunicate)
		{
			GlobalPlayerData globalPlayerData = m_GlobalPlayerData;
			if ((object)globalPlayerData == null || !globalPlayerData.isBlocked)
			{
				goto IL_01d9;
			}
		}
		m_Recorder.TransmitEnabled = false;
		m_currentlyTransmitting = false;
		goto IL_01d9;
		IL_01d9:
		bool mute = m_GlobalPlayerData == null || m_GlobalPlayerData.isMuted || m_GlobalPlayerData.isBlocked;
		audioSource.mute = mute;
		float voiceVolumeModifier = player.data.voiceVolumeModifier;
		if (!dead)
		{
			audioSource.volume = (dead2 ? 0f : num2) * voiceVolumeModifier;
		}
		else
		{
			audioSource.volume = num2 * voiceVolumeModifier;
		}
		if (flag2)
		{
			audioSource.volume *= obstructionValue;
		}
	}

	private void PushToTalk()
	{
		if (m_VoiceChatModeSetting != null)
		{
			bool flag = m_VoiceChatModeSetting.CanTalk();
			if (flag != m_currentlyTransmitting)
			{
				m_currentlyTransmitting = flag;
				m_Recorder.TransmitEnabled = flag;
			}
		}
	}

	public void CheckCommunicationWithUser(Photon.Realtime.Player p)
	{
		if (!GlobalPlayerData.TryGetPlayerData(p, out var globalPlayerData))
		{
			Debug.LogError("Failed to get player data for " + p.NickName);
			return;
		}
		ulong.TryParse((string)p.CustomProperties["UserID"], out var _);
		if (!globalPlayerData.Initialized)
		{
			globalPlayerData.canCommunicateWith = true;
			globalPlayerData.isMuted = false;
		}
	}

	[PunRPC]
	public void RPC_SetBlockedState(bool blocked)
	{
		if (m_GlobalPlayerData == null && !player.TryGetGlobalPlayerData(out m_GlobalPlayerData))
		{
			Debug.LogError("Failed to get global player data for " + player.m_name.text);
		}
		else
		{
			m_GlobalPlayerData.isBlocked = blocked;
		}
	}

	private void SetUserName(Player PlayerIn)
	{
		PlayerIn.m_name.text = PlayerIn.photonView.Controller.NickName;
	}

	private void CheckCommunicationPrivilegesOnAllPlayers()
	{
		Photon.Realtime.Player[] playerListOthers = PhotonNetwork.PlayerListOthers;
		foreach (Photon.Realtime.Player p in playerListOthers)
		{
			CheckCommunicationWithUser(p);
		}
	}

	private void OnPlayerJoined(Player p)
	{
		if (player.refs.view.IsMine)
		{
			if (Initialized)
			{
				CheckCommunicationWithUser(p.photonView.Controller);
			}
			SetUserName(p);
		}
	}

	private void DecrementIgnoreTimer()
	{
		m_IgnoreTimer -= Time.unscaledDeltaTime;
	}
}
