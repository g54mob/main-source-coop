using System;
using System.Runtime.InteropServices;
using Dissonance;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class PlayerVoiceMonitor : NetworkBehaviour
{
	[Header("Buzzing Zamanlaması")]
	[Tooltip("Fallback default — GameManager.Instance mevcutsa Lobby Game Settings'teki ConfiguredBuzzingInterval kullanılır (EffectiveBuzzingInterval). 0 = buzzing kapalı.")]
	public float buzzingInterval = 30f;

	public float buzzingGraceWindow = 5f;

	[Tooltip("Yerken sessizlik sayacının (dolayısıyla sinek barının boşalma hızının) çarpanı — 1'den küçük = yerken daha YAVAŞ azalır.")]
	public float eatingSilenceTimerMultiplier = 0.3f;

	private float _speakHeldTime;

	[Header("Referans")]
	public PlayerRoleData roleData;

	[Header("Sinek (tüm client'larda)")]
	[Tooltip("Sinek evresinde açılacak obje")]
	public GameObject flyGameObject;

	[Tooltip("Sinek 3D sesi")]
	public AudioSource flyAudioSource;

	[SyncVar(hook = "OnFlyPhaseChanged")]
	private bool _flyPhaseSync;

	[SyncVar]
	private int _soundsMadeCount;

	private DissonanceComms _comms;

	private VoicePlayerState _localState;

	private float _silenceTimer;

	private bool _buzzingActive;

	private float _graceTimer;

	private bool _inFlyPhase;

	private bool _active;

	private bool _isInGameScene;

	private bool _suspended;

	[SyncVar]
	private int _buzzWarningSeconds = -1;

	private int _lastSentBuzzWarning = -1;

	[SyncVar]
	private float _fillSync;

	private float _lastSentFill = -1f;

	[Header("Voice Chat Bağı")]
	[Tooltip("Ses sadece voice chat'e YAYINLANIYORSA sayılır (mic açmadan kurtulma engeli)")]
	public bool requireTransmitting = true;

	private VoiceBroadcastTrigger _broadcast;

	private AnimalEatController _eatController;

	private PlayerModelController _modelController;

	private bool _wasValidSpeak;

	private const float MaxTickDeltaTime = 0.1f;

	public const int BuzzWarningHidden = -1;

	public const int BuzzWarningActive = -2;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__flyPhaseSync;

	private float EffectiveBuzzingInterval
	{
		get
		{
			if (!(GameManager.Instance != null))
			{
				return buzzingInterval;
			}
			return GameManager.Instance.ConfiguredBuzzingInterval;
		}
	}

	private float EffectiveSpeakThreshold
	{
		get
		{
			if (!(VoiceNotificationUI.Instance != null))
			{
				return 0.025f;
			}
			return VoiceNotificationUI.Instance.speakThreshold;
		}
	}

	private float EffectiveMinSpeakDuration
	{
		get
		{
			if (!(VoiceNotificationUI.Instance != null))
			{
				return 0.05f;
			}
			return VoiceNotificationUI.Instance.minSpeakDuration;
		}
	}

	private float SilenceTimerRate
	{
		get
		{
			if (!(_eatController != null) || !_eatController.IsEating)
			{
				return 1f;
			}
			return eatingSilenceTimerMultiplier;
		}
	}

	public int SoundsMadeCount => _soundsMadeCount;

	public int BuzzWarningSeconds => _buzzWarningSeconds;

	public bool IsInFlyPhase => _flyPhaseSync;

	public float FillAmount => _fillSync;

	public bool Network_flyPhaseSync
	{
		get
		{
			return _flyPhaseSync;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _flyPhaseSync, 1uL, _Mirror_SyncVarHookDelegate__flyPhaseSync);
		}
	}

	public int Network_soundsMadeCount
	{
		get
		{
			return _soundsMadeCount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _soundsMadeCount, 2uL, null);
		}
	}

	public int Network_buzzWarningSeconds
	{
		get
		{
			return _buzzWarningSeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _buzzWarningSeconds, 4uL, null);
		}
	}

	public float Network_fillSync
	{
		get
		{
			return _fillSync;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _fillSync, 8uL, null);
		}
	}

	private void Awake()
	{
		if (roleData == null)
		{
			roleData = GetComponent<PlayerRoleData>();
		}
		_eatController = GetComponent<AnimalEatController>();
		_modelController = GetComponent<PlayerModelController>();
		if (flyGameObject != null)
		{
			flyGameObject.SetActive(value: false);
		}
		if (flyAudioSource != null)
		{
			flyAudioSource.playOnAwake = false;
			flyAudioSource.loop = true;
			flyAudioSource.spatialBlend = 1f;
			flyAudioSource.maxDistance = 25f;
			flyAudioSource.rolloffMode = AudioRolloffMode.Linear;
		}
	}

	public override void OnStartLocalPlayer()
	{
		_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		if (_comms == null)
		{
			Debug.LogWarning("[VoiceMonitor] DissonanceComms bulunamadı!");
		}
	}

	public void SetSuspended(bool suspended)
	{
		_suspended = suspended;
		if (suspended && _active)
		{
			Deactivate();
		}
	}

	private void Update()
	{
		if (!base.isLocalPlayer || _suspended || MyNetworkManager.Singleton == null || !MyNetworkManager.Singleton.IsInGameScene || GameManager.Instance == null || (!RoleAssignmentUI.LocalPanelClosedForRound && !GameManager.Instance._gameStarted))
		{
			return;
		}
		if (!(roleData != null) || roleData.Role != PlayerRole.Animal)
		{
			if (_active)
			{
				Deactivate();
			}
			return;
		}
		float num = ReadLocalAmplitude();
		VoiceNotificationUI.Instance?.UpdateMicLevel(num);
		Health component = GetComponent<Health>();
		if (component != null && component.IsDead)
		{
			if (_active)
			{
				DeactivateOnDeath();
			}
		}
		else if (GameManager.Instance.IsHunterDoorOpen)
		{
			if (!_active)
			{
				Activate();
			}
			if (EffectiveBuzzingInterval > 0f)
			{
				TickBuzzing(num);
			}
		}
	}

	private void DeactivateOnDeath()
	{
		_active = false;
		_buzzingActive = false;
		_inFlyPhase = false;
		_silenceTimer = 0f;
		_graceTimer = 0f;
		_speakHeldTime = 0f;
		VoiceNotificationUI.Instance?.HideBuzzing();
		VoiceNotificationUI.Instance?.SetFlyCountdown(-1f, EffectiveBuzzingInterval);
		SetBuzzWarningActive(active: false);
		if (_flyPhaseSync)
		{
			CmdSetFlyPhase(on: false);
		}
	}

	private void Activate()
	{
		_active = true;
		_silenceTimer = 0f;
		_graceTimer = 0f;
		_buzzingActive = false;
		_inFlyPhase = false;
		VoiceNotificationUI.Instance?.SetFlyCountdown(EffectiveBuzzingInterval, EffectiveBuzzingInterval);
	}

	private void Deactivate()
	{
		_active = false;
		_inFlyPhase = false;
		VoiceNotificationUI.Instance?.HideBuzzing();
		VoiceNotificationUI.Instance?.SetFlyCountdown(-1f, EffectiveBuzzingInterval);
		SetBuzzWarningActive(active: false);
		if (_flyPhaseSync)
		{
			CmdSetFlyPhase(on: false);
		}
	}

	private float ReadLocalAmplitude()
	{
		if (_comms == null)
		{
			_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		}
		if (_comms == null)
		{
			return 0f;
		}
		if (requireTransmitting)
		{
			if (_broadcast == null)
			{
				_broadcast = UnityEngine.Object.FindObjectOfType<VoiceBroadcastTrigger>();
			}
			if (_broadcast == null || !_broadcast.IsTransmitting)
			{
				return 0f;
			}
		}
		if (_localState == null && !string.IsNullOrEmpty(_comms.LocalPlayerName))
		{
			_localState = _comms.FindPlayer(_comms.LocalPlayerName);
		}
		if (_localState == null)
		{
			return 0f;
		}
		return _localState.Amplitude;
	}

	private void TickBuzzing(float amp)
	{
		float num = Mathf.Min(Time.deltaTime, 0.1f);
		if (amp >= EffectiveSpeakThreshold)
		{
			_speakHeldTime += num;
		}
		else
		{
			_speakHeldTime = 0f;
		}
		bool flag = _speakHeldTime >= EffectiveMinSpeakDuration;
		if (flag && !_wasValidSpeak)
		{
			CmdPlayTalkVfx();
		}
		_wasValidSpeak = flag;
		if (_inFlyPhase)
		{
			VoiceNotificationUI.Instance?.SetFlyCountdown(-1f, EffectiveBuzzingInterval);
			SendFill(1f);
			if (flag)
			{
				_inFlyPhase = false;
				_silenceTimer = 0f;
				CmdSetFlyPhase(on: false);
				SetBuzzWarningActive(active: false);
				VoiceNotificationUI.Instance?.HideBuzzing();
				VoiceNotificationUI.Instance?.SetFlyCountdown(EffectiveBuzzingInterval, EffectiveBuzzingInterval);
				GameAudioManager.Instance?.PlayBuzzingCleared();
				CmdIncrementSoundsMade();
				AchievementManager.Instance?.Unlock("ACH_BABY_STEPS");
			}
			return;
		}
		if (!_buzzingActive)
		{
			_silenceTimer += num * SilenceTimerRate;
			float num2 = EffectiveBuzzingInterval - _silenceTimer;
			VoiceNotificationUI.Instance?.SetFlyCountdown(num2, EffectiveBuzzingInterval);
			VoiceNotificationUI.Instance?.SetFlyCountdownEating(_eatController != null && _eatController.IsEating);
			UpdateBuzzWarningCountdown(num2);
			SendFill((EffectiveBuzzingInterval > 0f) ? Mathf.Clamp01(1f - num2 / EffectiveBuzzingInterval) : 1f);
			if (_silenceTimer >= EffectiveBuzzingInterval)
			{
				_buzzingActive = true;
				_graceTimer = buzzingGraceWindow;
				VoiceNotificationUI.Instance?.ShowBuzzing();
				SetBuzzWarningActive(active: true);
				GameAudioManager.Instance?.PlayBuzzingNotification();
			}
			return;
		}
		VoiceNotificationUI.Instance?.SetFlyCountdown(-1f, EffectiveBuzzingInterval);
		SendFill(1f);
		if (flag)
		{
			_buzzingActive = false;
			_silenceTimer = 0f;
			_graceTimer = 0f;
			VoiceNotificationUI.Instance?.HideBuzzing();
			VoiceNotificationUI.Instance?.SetFlyCountdown(EffectiveBuzzingInterval, EffectiveBuzzingInterval);
			SetBuzzWarningActive(active: false);
			GameAudioManager.Instance?.PlayBuzzingCleared();
			CmdIncrementSoundsMade();
			return;
		}
		_graceTimer -= num;
		VoiceNotificationUI.Instance?.SetBuzzingCountdown(_graceTimer);
		if (_graceTimer <= 0f)
		{
			_buzzingActive = false;
			_inFlyPhase = true;
			VoiceNotificationUI.Instance?.SetFlyPulsing(pulsing: true);
			Debug.Log("[VoiceMonitor] SİNEK EVRESİ — yer belli oluyor!");
			CmdSetFlyPhase(on: true);
		}
	}

	private void UpdateBuzzWarningCountdown(float remainingSeconds)
	{
		float num = EffectiveBuzzingInterval / 3f;
		int value = ((remainingSeconds >= 0f && remainingSeconds <= num) ? Mathf.CeilToInt(remainingSeconds) : (-1));
		SendBuzzWarning(value);
	}

	private void SetBuzzWarningActive(bool active)
	{
		SendBuzzWarning(active ? (-2) : (-1));
	}

	private void SendBuzzWarning(int value)
	{
		if (value != _lastSentBuzzWarning)
		{
			_lastSentBuzzWarning = value;
			CmdSetBuzzWarning(value);
		}
	}

	[Command]
	private void CmdSetBuzzWarning(int value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVarInt(value);
		SendCommandInternal("System.Void PlayerVoiceMonitor::CmdSetBuzzWarning(System.Int32)", 102318374, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void SendFill(float value01)
	{
		float num = Mathf.Round(value01 * 50f) / 50f;
		if (!Mathf.Approximately(num, _lastSentFill))
		{
			_lastSentFill = num;
			CmdSetFill(num);
		}
	}

	[Command]
	private void CmdSetFill(float value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(value);
		SendCommandInternal("System.Void PlayerVoiceMonitor::CmdSetFill(System.Single)", 692031468, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdSetFlyPhase(bool on)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(on);
		SendCommandInternal("System.Void PlayerVoiceMonitor::CmdSetFlyPhase(System.Boolean)", -1080149929, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdIncrementSoundsMade()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void PlayerVoiceMonitor::CmdIncrementSoundsMade()", 1039098158, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdPlayTalkVfx()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void PlayerVoiceMonitor::CmdPlayTalkVfx()", -790274442, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcPlayTalkVfx()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void PlayerVoiceMonitor::RpcPlayTalkVfx()", 1865365415, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void ServerResetSoundsMade()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerVoiceMonitor::ServerResetSoundsMade()' called when server was not active");
		}
		else
		{
			Network_soundsMadeCount = 0;
		}
	}

	[Server]
	public void ServerResetBuzzingState()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerVoiceMonitor::ServerResetBuzzingState()' called when server was not active");
			return;
		}
		Network_flyPhaseSync = false;
		Network_buzzWarningSeconds = -1;
		Network_fillSync = 0f;
		TargetResetBuzzingState(base.connectionToClient);
	}

	[TargetRpc]
	private void TargetResetBuzzingState(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void PlayerVoiceMonitor::TargetResetBuzzingState(Mirror.NetworkConnectionToClient)", -334126413, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void OnFlyPhaseChanged(bool _, bool on)
	{
		if (flyGameObject != null)
		{
			flyGameObject.SetActive(on);
		}
		if (flyAudioSource != null)
		{
			if (on)
			{
				if (!flyAudioSource.isPlaying)
				{
					flyAudioSource.Play();
				}
			}
			else
			{
				flyAudioSource.Stop();
			}
		}
		GetComponent<PlayerOutline>()?.SetFlyHighlight(on);
	}

	public void NotifyKicked()
	{
		if (base.isLocalPlayer && !(roleData == null) && roleData.Role == PlayerRole.Animal)
		{
			VoiceNotificationUI.Instance?.ShowMakeSound();
		}
	}

	public PlayerVoiceMonitor()
	{
		_Mirror_SyncVarHookDelegate__flyPhaseSync = OnFlyPhaseChanged;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdSetBuzzWarning__Int32(int value)
	{
		Network_buzzWarningSeconds = value;
	}

	protected static void InvokeUserCode_CmdSetBuzzWarning__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetBuzzWarning called on client.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_CmdSetBuzzWarning__Int32(reader.ReadVarInt());
		}
	}

	protected void UserCode_CmdSetFill__Single(float value)
	{
		Network_fillSync = value;
	}

	protected static void InvokeUserCode_CmdSetFill__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetFill called on client.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_CmdSetFill__Single(reader.ReadFloat());
		}
	}

	protected void UserCode_CmdSetFlyPhase__Boolean(bool on)
	{
		Network_flyPhaseSync = on;
	}

	protected static void InvokeUserCode_CmdSetFlyPhase__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetFlyPhase called on client.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_CmdSetFlyPhase__Boolean(reader.ReadBool());
		}
	}

	protected void UserCode_CmdIncrementSoundsMade()
	{
		Network_soundsMadeCount = _soundsMadeCount + 1;
	}

	protected static void InvokeUserCode_CmdIncrementSoundsMade(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIncrementSoundsMade called on client.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_CmdIncrementSoundsMade();
		}
	}

	protected void UserCode_CmdPlayTalkVfx()
	{
		RpcPlayTalkVfx();
	}

	protected static void InvokeUserCode_CmdPlayTalkVfx(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPlayTalkVfx called on client.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_CmdPlayTalkVfx();
		}
	}

	protected void UserCode_RpcPlayTalkVfx()
	{
		_modelController?.PlayTalkVfx();
	}

	protected static void InvokeUserCode_RpcPlayTalkVfx(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayTalkVfx called on server.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_RpcPlayTalkVfx();
		}
	}

	protected void UserCode_TargetResetBuzzingState__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		_active = false;
		_buzzingActive = false;
		_inFlyPhase = false;
		_silenceTimer = 0f;
		_graceTimer = 0f;
		_speakHeldTime = 0f;
		_lastSentBuzzWarning = -1;
		_lastSentFill = -1f;
		VoiceNotificationUI.Instance?.HideBuzzing();
		VoiceNotificationUI.Instance?.SetFlyCountdown(-1f, EffectiveBuzzingInterval);
		VoiceNotificationUI.Instance?.ResetMicLevel();
	}

	protected static void InvokeUserCode_TargetResetBuzzingState__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetResetBuzzingState called on server.");
		}
		else
		{
			((PlayerVoiceMonitor)obj).UserCode_TargetResetBuzzingState__NetworkConnectionToClient(null);
		}
	}

	static PlayerVoiceMonitor()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::CmdSetBuzzWarning(System.Int32)", InvokeUserCode_CmdSetBuzzWarning__Int32, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::CmdSetFill(System.Single)", InvokeUserCode_CmdSetFill__Single, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::CmdSetFlyPhase(System.Boolean)", InvokeUserCode_CmdSetFlyPhase__Boolean, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::CmdIncrementSoundsMade()", InvokeUserCode_CmdIncrementSoundsMade, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::CmdPlayTalkVfx()", InvokeUserCode_CmdPlayTalkVfx, requiresAuthority: true);
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::RpcPlayTalkVfx()", InvokeUserCode_RpcPlayTalkVfx);
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerVoiceMonitor), "System.Void PlayerVoiceMonitor::TargetResetBuzzingState(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetResetBuzzingState__NetworkConnectionToClient);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_flyPhaseSync);
			writer.WriteVarInt(_soundsMadeCount);
			writer.WriteVarInt(_buzzWarningSeconds);
			writer.WriteFloat(_fillSync);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBool(_flyPhaseSync);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteVarInt(_soundsMadeCount);
		}
		if ((syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteVarInt(_buzzWarningSeconds);
		}
		if ((syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteFloat(_fillSync);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _flyPhaseSync, _Mirror_SyncVarHookDelegate__flyPhaseSync, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _soundsMadeCount, null, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref _buzzWarningSeconds, null, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref _fillSync, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _flyPhaseSync, _Mirror_SyncVarHookDelegate__flyPhaseSync, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _soundsMadeCount, null, reader.ReadVarInt());
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _buzzWarningSeconds, null, reader.ReadVarInt());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _fillSync, null, reader.ReadFloat());
		}
	}
}
