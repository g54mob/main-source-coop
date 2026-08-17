using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Audio;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class NetworkedFootstepPlayer : NetworkBehaviour
	{
		[Header("Timing")]
		[SerializeField]
		private float footstepInterval = 0.4f;

		[SerializeField]
		private float sprintFootstepInterval = 0.25f;

		[SerializeField]
		private float crouchFootstepInterval = 0.6f;

		[Header("Surface Footstep Config")]
		[Tooltip("Per-surface SoundID mapping. BroAudio AudioEntities handle shuffle internally.")]
		[SerializeField]
		private SurfaceFootstepConfig surfaceFootstepConfig;

		[Header("Footstep Events (fallback when no config is assigned)")]
		[Tooltip("Default footstep when no surface mapping matches.")]
		[SerializeField]
		private SoundID defaultFootstep;

		[SerializeField]
		private SoundID walkFootstep;

		[SerializeField]
		private SoundID sprintFootstep;

		[SerializeField]
		private SoundID crouchFootstep;

		[SyncVar]
		private PlayerState _syncedPlayerState;

		[SyncVar]
		private SurfaceType _syncedSurfaceType;

		[Inject]
		private IAudioManager _audioManager;

		private float _footstepTimer;

		public PlayerState Network_syncedPlayerState
		{
			get
			{
				return _syncedPlayerState;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedPlayerState, 1uL, null);
			}
		}

		public SurfaceType Network_syncedSurfaceType
		{
			get
			{
				return _syncedSurfaceType;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedSurfaceType, 2uL, null);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		private void Update()
		{
			if (base.isOwned)
			{
				UpdateSyncedState();
				UpdateFootsteps(GetLocalState(), GetLocalSurface());
			}
			else
			{
				UpdateFootsteps(_syncedPlayerState, _syncedSurfaceType);
			}
		}

		private PlayerState GetLocalState()
		{
			FirstPersonController component = GetComponent<FirstPersonController>();
			if (!(component != null))
			{
				return _syncedPlayerState;
			}
			return component.PlayerState;
		}

		private SurfaceType GetLocalSurface()
		{
			FirstPersonController component = GetComponent<FirstPersonController>();
			if (!(component != null))
			{
				return _syncedSurfaceType;
			}
			return component.CurrentSurfaceType;
		}

		private void UpdateSyncedState()
		{
			FirstPersonController component = GetComponent<FirstPersonController>();
			if (!(component == null))
			{
				if (_syncedPlayerState != component.PlayerState)
				{
					CmdSetPlayerState(component.PlayerState);
				}
				if (_syncedSurfaceType != component.CurrentSurfaceType)
				{
					CmdSetSurfaceType(component.CurrentSurfaceType);
				}
			}
		}

		[Command]
		private void CmdSetPlayerState(PlayerState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdSetPlayerState(NomadDrive.Features.Player.PlayerStateMachine.PlayerState)", -1032084852, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdSetSurfaceType(SurfaceType surfaceType)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, surfaceType);
			SendCommandInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdSetSurfaceType(NomadDrive.Features.Player.SurfaceType)", -2140286867, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public void SetSurfaceType(SurfaceType surfaceType)
		{
			if (base.isOwned && _syncedSurfaceType != surfaceType)
			{
				CmdSetSurfaceType(surfaceType);
			}
		}

		private void UpdateFootsteps(PlayerState state, SurfaceType surface)
		{
			if (!IsMovementState(state))
			{
				_footstepTimer = 0f;
				return;
			}
			float footstepTimer = GetFootstepInterval(state);
			_footstepTimer -= Time.deltaTime;
			if (_footstepTimer <= 0f)
			{
				PlayFootstep(state, surface);
				_footstepTimer = footstepTimer;
			}
		}

		private void PlayFootstep(PlayerState state, SurfaceType surface)
		{
			if (_audioManager != null)
			{
				SoundID id = ResolveFootstepKey(state, surface);
				if (id.IsValid())
				{
					_audioManager.PlayOneShotAttached(id, base.gameObject);
				}
			}
		}

		private SoundID ResolveFootstepKey(PlayerState state, SurfaceType surface)
		{
			if (surfaceFootstepConfig != null)
			{
				SoundID soundID = surfaceFootstepConfig.Resolve(surface, state);
				if (soundID.IsValid())
				{
					return soundID;
				}
			}
			switch (state)
			{
			case PlayerState.Sprint:
			case PlayerState.CrouchedSprint:
				return sprintFootstep.IsValid() ? sprintFootstep : defaultFootstep;
			case PlayerState.CrouchedWalk:
				return crouchFootstep.IsValid() ? crouchFootstep : defaultFootstep;
			case PlayerState.Walk:
				return walkFootstep.IsValid() ? walkFootstep : defaultFootstep;
			default:
				return defaultFootstep;
			}
		}

		public void PlayJump()
		{
			if (base.isOwned)
			{
				SurfaceType localSurface = GetLocalSurface();
				PlayJumpLocal(localSurface);
				CmdBroadcastJump(localSurface);
			}
		}

		public void PlayLand()
		{
			if (base.isOwned)
			{
				SurfaceType localSurface = GetLocalSurface();
				PlayLandLocal(localSurface);
				CmdBroadcastLand(localSurface);
			}
		}

		[Command]
		private void CmdBroadcastJump(SurfaceType surface)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, surface);
			SendCommandInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdBroadcastJump(NomadDrive.Features.Player.SurfaceType)", -301238221, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdBroadcastLand(SurfaceType surface)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, surface);
			SendCommandInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdBroadcastLand(NomadDrive.Features.Player.SurfaceType)", 397588270, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPlayJump(SurfaceType surface)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, surface);
			SendRPCInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::RpcPlayJump(NomadDrive.Features.Player.SurfaceType)", -145772821, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPlayLand(SurfaceType surface)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, surface);
			SendRPCInternal("System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::RpcPlayLand(NomadDrive.Features.Player.SurfaceType)", 453996774, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void PlayJumpLocal(SurfaceType surface)
		{
			if (_audioManager != null && !(surfaceFootstepConfig == null))
			{
				SoundID id = surfaceFootstepConfig.ResolveJump(surface);
				if (id.IsValid())
				{
					_audioManager.PlayOneShotAttached(id, base.gameObject);
				}
			}
		}

		private void PlayLandLocal(SurfaceType surface)
		{
			if (_audioManager != null && !(surfaceFootstepConfig == null))
			{
				SoundID id = surfaceFootstepConfig.ResolveLand(surface);
				if (id.IsValid())
				{
					_audioManager.PlayOneShotAttached(id, base.gameObject);
				}
			}
		}

		private static bool IsMovementState(PlayerState state)
		{
			if (state != PlayerState.Walk && state != PlayerState.Sprint && state != PlayerState.CrouchedWalk)
			{
				return state == PlayerState.CrouchedSprint;
			}
			return true;
		}

		private float GetFootstepInterval(PlayerState state)
		{
			return state switch
			{
				PlayerState.Sprint => sprintFootstepInterval, 
				PlayerState.CrouchedSprint => sprintFootstepInterval, 
				PlayerState.CrouchedWalk => crouchFootstepInterval, 
				_ => footstepInterval, 
			};
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetPlayerState__PlayerState(PlayerState state)
		{
			Network_syncedPlayerState = state;
		}

		protected static void InvokeUserCode_CmdSetPlayerState__PlayerState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetPlayerState called on client.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_CmdSetPlayerState__PlayerState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(reader));
			}
		}

		protected void UserCode_CmdSetSurfaceType__SurfaceType(SurfaceType surfaceType)
		{
			Network_syncedSurfaceType = surfaceType;
		}

		protected static void InvokeUserCode_CmdSetSurfaceType__SurfaceType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSurfaceType called on client.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_CmdSetSurfaceType__SurfaceType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}

		protected void UserCode_CmdBroadcastJump__SurfaceType(SurfaceType surface)
		{
			RpcPlayJump(surface);
		}

		protected static void InvokeUserCode_CmdBroadcastJump__SurfaceType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdBroadcastJump called on client.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_CmdBroadcastJump__SurfaceType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}

		protected void UserCode_CmdBroadcastLand__SurfaceType(SurfaceType surface)
		{
			RpcPlayLand(surface);
		}

		protected static void InvokeUserCode_CmdBroadcastLand__SurfaceType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdBroadcastLand called on client.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_CmdBroadcastLand__SurfaceType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}

		protected void UserCode_RpcPlayJump__SurfaceType(SurfaceType surface)
		{
			PlayJumpLocal(surface);
		}

		protected static void InvokeUserCode_RpcPlayJump__SurfaceType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayJump called on server.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_RpcPlayJump__SurfaceType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}

		protected void UserCode_RpcPlayLand__SurfaceType(SurfaceType surface)
		{
			PlayLandLocal(surface);
		}

		protected static void InvokeUserCode_RpcPlayLand__SurfaceType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayLand called on server.");
			}
			else
			{
				((NetworkedFootstepPlayer)obj).UserCode_RpcPlayLand__SurfaceType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}

		static NetworkedFootstepPlayer()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdSetPlayerState(NomadDrive.Features.Player.PlayerStateMachine.PlayerState)", InvokeUserCode_CmdSetPlayerState__PlayerState, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdSetSurfaceType(NomadDrive.Features.Player.SurfaceType)", InvokeUserCode_CmdSetSurfaceType__SurfaceType, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdBroadcastJump(NomadDrive.Features.Player.SurfaceType)", InvokeUserCode_CmdBroadcastJump__SurfaceType, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::CmdBroadcastLand(NomadDrive.Features.Player.SurfaceType)", InvokeUserCode_CmdBroadcastLand__SurfaceType, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::RpcPlayJump(NomadDrive.Features.Player.SurfaceType)", InvokeUserCode_RpcPlayJump__SurfaceType);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedFootstepPlayer), "System.Void NomadDrive.Features.Player.NetworkedFootstepPlayer::RpcPlayLand(NomadDrive.Features.Player.SurfaceType)", InvokeUserCode_RpcPlayLand__SurfaceType);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(writer, _syncedPlayerState);
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, _syncedSurfaceType);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(writer, _syncedPlayerState);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(writer, _syncedSurfaceType);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedPlayerState, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(reader));
				GeneratedSyncVarDeserialize(ref _syncedSurfaceType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedPlayerState, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(reader));
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedSurfaceType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(reader));
			}
		}
	}
}
