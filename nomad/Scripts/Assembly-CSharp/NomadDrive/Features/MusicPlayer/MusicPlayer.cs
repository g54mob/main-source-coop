using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EvilCore.Audio;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.MusicPlayer
{
	public class MusicPlayer : HeldItem
	{
		private const string VolumeParameterName = "volume";

		private const float TrackEndCheckInterval = 0.5f;

		[SerializeField]
		private AudioTrackLibrary library;

		[SerializeField]
		[Range(0f, 1f)]
		private float defaultVolume = 0.7f;

		[SyncVar(hook = "OnIsPlayingChanged")]
		private bool _isPlaying;

		[SyncVar(hook = "OnCurrentTrackIndexChanged")]
		private int _currentTrackIndex = -1;

		[SyncVar(hook = "OnVolumeChanged")]
		private float _volume = 0.7f;

		[Inject]
		private IAudioManager _audioManager;

		private AudioHandle _trackInstance;

		private bool _hasTrackInstance;

		private float _trackEndCheckTimer;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isPlaying;

		public Action<int, int> _Mirror_SyncVarHookDelegate__currentTrackIndex;

		public Action<float, float> _Mirror_SyncVarHookDelegate__volume;

		public bool IsPlaying => _isPlaying;

		public float Volume => _volume;

		public AudioTrack CurrentTrack => GetCurrentTrack();

		public AudioTrackLibrary Library => library;

		private string DebugCurrentTrack => CurrentTrack?.DisplayName ?? "Not playing";

		private string DebugIsPlaying
		{
			get
			{
				if (!_isPlaying)
				{
					return "Stopped";
				}
				return "Playing";
			}
		}

		private string DebugVolume => $"{_volume * 100f:F0}%";

		public bool Network_isPlaying
		{
			get
			{
				return _isPlaying;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isPlaying, 512uL, _Mirror_SyncVarHookDelegate__isPlaying);
			}
		}

		public int Network_currentTrackIndex
		{
			get
			{
				return _currentTrackIndex;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _currentTrackIndex, 1024uL, _Mirror_SyncVarHookDelegate__currentTrackIndex);
			}
		}

		public float Network_volume
		{
			get
			{
				return _volume;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _volume, 2048uL, _Mirror_SyncVarHookDelegate__volume);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			Network_volume = defaultVolume;
			library?.Initialize();
		}

		protected void OnDestroy()
		{
			StopCurrentTrack();
		}

		public void TurnOn()
		{
			if (!_isPlaying && library.HasUnlockedTracks())
			{
				CmdTurnOn();
			}
		}

		public void TurnOff()
		{
			if (_isPlaying)
			{
				CmdTurnOff();
			}
		}

		public void TogglePower()
		{
			if (_isPlaying)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
		}

		public void NextTrack()
		{
			if (_isPlaying)
			{
				CmdNextTrack();
			}
		}

		public void PreviousTrack()
		{
			if (_isPlaying)
			{
				CmdPreviousTrack();
			}
		}

		public void SetVolume(float volume)
		{
			volume = Mathf.Clamp01(volume);
			CmdSetVolume(volume);
		}

		public void IncreaseVolume(float amount = 0.1f)
		{
			SetVolume(_volume + amount);
		}

		public void DecreaseVolume(float amount = 0.1f)
		{
			SetVolume(_volume - amount);
		}

		public bool UnlockTrack(AudioTrack track)
		{
			if (library == null)
			{
				return false;
			}
			return library.Unlock(track);
		}

		public AudioTrack GetCurrentTrack()
		{
			if (!_isPlaying || _currentTrackIndex < 0)
			{
				return null;
			}
			return library?.GetTrackByIndex(_currentTrackIndex);
		}

		public IReadOnlyList<AudioTrack> GetUnlockedTracks()
		{
			return library?.GetUnlockedTracks();
		}

		[Command(requiresAuthority = false)]
		private void CmdTurnOn()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdTurnOn()", 206220331, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdTurnOff()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdTurnOff()", 650519925, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdNextTrack()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdNextTrack()", -381840579, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPreviousTrack()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdPreviousTrack()", -1019826627, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetVolume(float volume)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(volume);
			SendCommandInternal("System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdSetVolume(System.Single)", -1436227744, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnIsPlayingChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					PlayCurrentTrack();
				}
				else
				{
					StopCurrentTrack();
				}
			}
		}

		private void OnCurrentTrackIndexChanged(int oldValue, int newValue)
		{
			if (IsLateJoinCompleted && _isPlaying)
			{
				StopCurrentTrack();
				PlayCurrentTrack();
			}
		}

		private void OnVolumeChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				UpdateVolume();
			}
		}

		private void PlayCurrentTrack()
		{
			if (!(library == null) && _audioManager != null)
			{
				AudioTrack trackByIndex = library.GetTrackByIndex(_currentTrackIndex);
				if (!(trackByIndex == null))
				{
					StopCurrentTrack();
					_trackInstance = _audioManager.PlayEventAttached(trackByIndex.SoundId, base.gameObject);
					_hasTrackInstance = true;
					UpdateVolume();
				}
			}
		}

		private void StopCurrentTrack()
		{
			if (_hasTrackInstance && _audioManager != null)
			{
				_audioManager.StopEvent(_trackInstance);
				_hasTrackInstance = false;
			}
		}

		private void UpdateVolume()
		{
			if (_hasTrackInstance)
			{
				_audioManager.SetParameter(_trackInstance, "volume", _volume);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			library?.Initialize();
			if (_isPlaying && _currentTrackIndex >= 0)
			{
				PlayCurrentTrack();
			}
		}

		private void DebugTurnOn()
		{
			TurnOn();
		}

		private void DebugTurnOff()
		{
			TurnOff();
		}

		private void DebugNextTrack()
		{
			NextTrack();
		}

		private void DebugPreviousTrack()
		{
			PreviousTrack();
		}

		private void DebugVolumeDown()
		{
			DecreaseVolume();
		}

		private void DebugVolumeUp()
		{
			IncreaseVolume();
		}

		public MusicPlayer()
		{
			_Mirror_SyncVarHookDelegate__isPlaying = OnIsPlayingChanged;
			_Mirror_SyncVarHookDelegate__currentTrackIndex = OnCurrentTrackIndexChanged;
			_Mirror_SyncVarHookDelegate__volume = OnVolumeChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdTurnOn()
		{
			if (!_isPlaying && !(library == null) && library.HasUnlockedTracks())
			{
				Network_currentTrackIndex = 0;
				Network_isPlaying = true;
				PlayCurrentTrack();
			}
		}

		protected static void InvokeUserCode_CmdTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTurnOn called on client.");
			}
			else
			{
				((MusicPlayer)obj).UserCode_CmdTurnOn();
			}
		}

		protected void UserCode_CmdTurnOff()
		{
			if (_isPlaying)
			{
				Network_isPlaying = false;
				StopCurrentTrack();
			}
		}

		protected static void InvokeUserCode_CmdTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTurnOff called on client.");
			}
			else
			{
				((MusicPlayer)obj).UserCode_CmdTurnOff();
			}
		}

		protected void UserCode_CmdNextTrack()
		{
			if (!_isPlaying || library == null)
			{
				return;
			}
			AudioTrack trackByIndex = library.GetTrackByIndex(_currentTrackIndex);
			AudioTrack nextTrack = library.GetNextTrack(trackByIndex);
			if (nextTrack != null)
			{
				int trackIndex = library.GetTrackIndex(nextTrack);
				if (trackIndex >= 0)
				{
					Network_currentTrackIndex = trackIndex;
				}
			}
		}

		protected static void InvokeUserCode_CmdNextTrack(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdNextTrack called on client.");
			}
			else
			{
				((MusicPlayer)obj).UserCode_CmdNextTrack();
			}
		}

		protected void UserCode_CmdPreviousTrack()
		{
			if (!_isPlaying || library == null)
			{
				return;
			}
			AudioTrack trackByIndex = library.GetTrackByIndex(_currentTrackIndex);
			AudioTrack previousTrack = library.GetPreviousTrack(trackByIndex);
			if (previousTrack != null)
			{
				int trackIndex = library.GetTrackIndex(previousTrack);
				if (trackIndex >= 0)
				{
					Network_currentTrackIndex = trackIndex;
				}
			}
		}

		protected static void InvokeUserCode_CmdPreviousTrack(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPreviousTrack called on client.");
			}
			else
			{
				((MusicPlayer)obj).UserCode_CmdPreviousTrack();
			}
		}

		protected void UserCode_CmdSetVolume__Single(float volume)
		{
			Network_volume = Mathf.Clamp01(volume);
		}

		protected static void InvokeUserCode_CmdSetVolume__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetVolume called on client.");
			}
			else
			{
				((MusicPlayer)obj).UserCode_CmdSetVolume__Single(reader.ReadFloat());
			}
		}

		static MusicPlayer()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(MusicPlayer), "System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdTurnOn()", InvokeUserCode_CmdTurnOn, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(MusicPlayer), "System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdTurnOff()", InvokeUserCode_CmdTurnOff, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(MusicPlayer), "System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdNextTrack()", InvokeUserCode_CmdNextTrack, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(MusicPlayer), "System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdPreviousTrack()", InvokeUserCode_CmdPreviousTrack, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(MusicPlayer), "System.Void NomadDrive.Features.MusicPlayer.MusicPlayer::CmdSetVolume(System.Single)", InvokeUserCode_CmdSetVolume__Single, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isPlaying);
				writer.WriteVarInt(_currentTrackIndex);
				writer.WriteFloat(_volume);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_isPlaying);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteVarInt(_currentTrackIndex);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteFloat(_volume);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isPlaying, _Mirror_SyncVarHookDelegate__isPlaying, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _currentTrackIndex, _Mirror_SyncVarHookDelegate__currentTrackIndex, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref _volume, _Mirror_SyncVarHookDelegate__volume, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isPlaying, _Mirror_SyncVarHookDelegate__isPlaying, reader.ReadBool());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _currentTrackIndex, _Mirror_SyncVarHookDelegate__currentTrackIndex, reader.ReadVarInt());
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _volume, _Mirror_SyncVarHookDelegate__volume, reader.ReadFloat());
			}
		}
	}
}
