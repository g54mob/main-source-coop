using Mimicraft.Gameplay;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class CompanionRoundManager : RoundManager
	{
		public enum CompanionSide
		{
			None = 0,
			Hunters = 1,
			Hiders = 2
		}

		public override bool OffersCompanionChoice => true;

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestBecomeCompanionServerRpc(CompanionSide side, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(3812524413u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in side, default(FastBufferWriter.ForEnums));
				__endSendRpc(ref bufferWriter, 3812524413u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			ulong senderClientId = rpcParams.Receive.SenderClientId;
			if (side == CompanionSide.Hunters && base.NetworkManager.ConnectedClients.TryGetValue(senderClientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerCompanion component = value.PlayerObject.GetComponent<PlayerCompanion>();
				if (component == null)
				{
					Debug.LogWarning("[CompanionRoundManager] Oyuncu prefabinde PlayerCompanion yok " + $"(client {senderClientId}) - kopege donusum atlandi.");
				}
				else if (RevealNonParticipant(senderClientId))
				{
					component.ServerSetCompanion(value: true);
				}
			}
		}

		protected override void RevealNonParticipants()
		{
			base.RevealNonParticipants();
			if (!base.IsServer)
			{
				return;
			}
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				if (!(connectedClients.PlayerObject == null))
				{
					PlayerCompanion component = connectedClients.PlayerObject.GetComponent<PlayerCompanion>();
					if (component != null)
					{
						component.ServerSetCompanion(value: false);
					}
				}
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3812524413u, __rpc_handler_3812524413, "RequestBecomeCompanionServerRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3812524413(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out CompanionSide value, default(FastBufferWriter.ForEnums));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((CompanionRoundManager)target).RequestBecomeCompanionServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "CompanionRoundManager";
		}
	}
}
