using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class PlayerDamageAudioHandler : NetworkBehaviour
	{
		[SerializeField]
		private PlayerDamageAudioConfig damageAudioConfig;

		[Inject]
		private IAudioManager _audioManager;

		private PlayerStatsManager _stats;

		private void OnEnable()
		{
			_stats = GetComponent<PlayerStatsManager>();
			if (_stats != null)
			{
				_stats.OnHealthDamaged += HandleHealthDamaged;
			}
		}

		private void OnDisable()
		{
			if (_stats != null)
			{
				_stats.OnHealthDamaged -= HandleHealthDamaged;
			}
		}

		private void HandleHealthDamaged()
		{
			if (base.isOwned && !(damageAudioConfig == null))
			{
				PlayDamageLocal();
				CmdBroadcastDamage();
			}
		}

		[Command]
		private void CmdBroadcastDamage()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerDamageAudioHandler::CmdBroadcastDamage()", 418304638, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPlayDamage()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Player.PlayerDamageAudioHandler::RpcPlayDamage()", 802782870, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void PlayDamageLocal()
		{
			if (_audioManager == null || damageAudioConfig == null)
			{
				return;
			}
			IReadOnlyList<SoundID> damageSounds = damageAudioConfig.DamageSounds;
			if (damageSounds == null)
			{
				return;
			}
			for (int i = 0; i < damageSounds.Count; i++)
			{
				SoundID id = damageSounds[i];
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

		protected void UserCode_CmdBroadcastDamage()
		{
			RpcPlayDamage();
		}

		protected static void InvokeUserCode_CmdBroadcastDamage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdBroadcastDamage called on client.");
			}
			else
			{
				((PlayerDamageAudioHandler)obj).UserCode_CmdBroadcastDamage();
			}
		}

		protected void UserCode_RpcPlayDamage()
		{
			PlayDamageLocal();
		}

		protected static void InvokeUserCode_RpcPlayDamage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayDamage called on server.");
			}
			else
			{
				((PlayerDamageAudioHandler)obj).UserCode_RpcPlayDamage();
			}
		}

		static PlayerDamageAudioHandler()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerDamageAudioHandler), "System.Void NomadDrive.Features.Player.PlayerDamageAudioHandler::CmdBroadcastDamage()", InvokeUserCode_CmdBroadcastDamage, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerDamageAudioHandler), "System.Void NomadDrive.Features.Player.PlayerDamageAudioHandler::RpcPlayDamage()", InvokeUserCode_RpcPlayDamage);
		}
	}
}
