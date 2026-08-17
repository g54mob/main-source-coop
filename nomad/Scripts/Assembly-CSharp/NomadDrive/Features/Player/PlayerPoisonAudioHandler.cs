using Ami.BroAudio;
using EvilCore.Audio;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class PlayerPoisonAudioHandler : NetworkBehaviour
	{
		[SerializeField]
		private SoundID poisonSound;

		[Inject]
		private IAudioManager _audioManager;

		private PlayerStatsManager _stats;

		private void OnEnable()
		{
			_stats = GetComponent<PlayerStatsManager>();
			if (_stats != null)
			{
				_stats.OnPoisoned += HandlePoisoned;
			}
		}

		private void OnDisable()
		{
			if (_stats != null)
			{
				_stats.OnPoisoned -= HandlePoisoned;
			}
		}

		private void HandlePoisoned()
		{
			if (base.isOwned && poisonSound.IsValid())
			{
				PlayPoisonLocal();
				CmdBroadcastPoison();
			}
		}

		[Command]
		private void CmdBroadcastPoison()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerPoisonAudioHandler::CmdBroadcastPoison()", -1718544598, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPlayPoison()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Player.PlayerPoisonAudioHandler::RpcPlayPoison()", 24001036, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void PlayPoisonLocal()
		{
			if (_audioManager != null && poisonSound.IsValid())
			{
				_audioManager.PlayOneShotAttached(poisonSound, base.gameObject);
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdBroadcastPoison()
		{
			RpcPlayPoison();
		}

		protected static void InvokeUserCode_CmdBroadcastPoison(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdBroadcastPoison called on client.");
			}
			else
			{
				((PlayerPoisonAudioHandler)obj).UserCode_CmdBroadcastPoison();
			}
		}

		protected void UserCode_RpcPlayPoison()
		{
			PlayPoisonLocal();
		}

		protected static void InvokeUserCode_RpcPlayPoison(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayPoison called on server.");
			}
			else
			{
				((PlayerPoisonAudioHandler)obj).UserCode_RpcPlayPoison();
			}
		}

		static PlayerPoisonAudioHandler()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerPoisonAudioHandler), "System.Void NomadDrive.Features.Player.PlayerPoisonAudioHandler::CmdBroadcastPoison()", InvokeUserCode_CmdBroadcastPoison, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerPoisonAudioHandler), "System.Void NomadDrive.Features.Player.PlayerPoisonAudioHandler::RpcPlayPoison()", InvokeUserCode_RpcPlayPoison);
		}
	}
}
