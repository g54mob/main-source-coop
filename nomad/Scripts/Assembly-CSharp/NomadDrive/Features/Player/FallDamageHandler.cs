using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class FallDamageHandler : NetworkBehaviour
	{
		[SerializeField]
		private FallDamageConfig fallDamageConfig;

		[Inject]
		private IAudioManager _audioManager;

		private FirstPersonController _fpc;

		private PlayerStatsManager _stats;

		private void OnEnable()
		{
			_fpc = GetComponent<FirstPersonController>();
			if (_fpc != null)
			{
				_fpc.OnLandedImpact += HandleLandedImpact;
			}
		}

		private void OnDisable()
		{
			if (_fpc != null)
			{
				_fpc.OnLandedImpact -= HandleLandedImpact;
			}
		}

		private void HandleLandedImpact(Vector3 landingVelocity)
		{
			if (!base.isOwned || fallDamageConfig == null)
			{
				return;
			}
			PlayerState playerState = _fpc.PlayerState;
			if (playerState == PlayerState.Sit || playerState == PlayerState.Laying)
			{
				return;
			}
			float num = Mathf.Abs(landingVelocity.y);
			float fallGravityMagnitude = _fpc.FallGravityMagnitude;
			if (fallGravityMagnitude <= 0f)
			{
				return;
			}
			float fallHeight = num * num / (2f * fallGravityMagnitude);
			float num2 = fallDamageConfig.EvaluateDamage(fallHeight);
			if (!(num2 <= 0f))
			{
				if ((object)_stats == null)
				{
					_stats = GetComponent<PlayerStatsManager>();
				}
				if (!(_stats == null) && _stats.IsInitialized)
				{
					_stats.ApplyDirectDamage(num2);
					PlayImpactLocal();
					CmdBroadcastImpact();
				}
			}
		}

		[Command]
		private void CmdBroadcastImpact()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.FallDamageHandler::CmdBroadcastImpact()", -1511965895, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPlayImpact()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Player.FallDamageHandler::RpcPlayImpact()", 240114761, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void PlayImpactLocal()
		{
			if (_audioManager == null || fallDamageConfig == null)
			{
				return;
			}
			IReadOnlyList<SoundID> impactSounds = fallDamageConfig.ImpactSounds;
			if (impactSounds == null)
			{
				return;
			}
			for (int i = 0; i < impactSounds.Count; i++)
			{
				SoundID id = impactSounds[i];
				if (id.IsValid())
				{
					_audioManager.PlayOneShotAttached(id, base.gameObject);
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdBroadcastImpact()
		{
			RpcPlayImpact();
		}

		protected static void InvokeUserCode_CmdBroadcastImpact(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdBroadcastImpact called on client.");
			}
			else
			{
				((FallDamageHandler)obj).UserCode_CmdBroadcastImpact();
			}
		}

		protected void UserCode_RpcPlayImpact()
		{
			PlayImpactLocal();
		}

		protected static void InvokeUserCode_RpcPlayImpact(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayImpact called on server.");
			}
			else
			{
				((FallDamageHandler)obj).UserCode_RpcPlayImpact();
			}
		}

		static FallDamageHandler()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(FallDamageHandler), "System.Void NomadDrive.Features.Player.FallDamageHandler::CmdBroadcastImpact()", InvokeUserCode_CmdBroadcastImpact, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(FallDamageHandler), "System.Void NomadDrive.Features.Player.FallDamageHandler::RpcPlayImpact()", InvokeUserCode_RpcPlayImpact);
		}
	}
}
