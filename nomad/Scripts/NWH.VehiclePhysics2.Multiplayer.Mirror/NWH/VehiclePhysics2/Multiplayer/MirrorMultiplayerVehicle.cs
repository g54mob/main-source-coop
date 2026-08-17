using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NWH.VehiclePhysics2.Multiplayer
{
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(VehicleController))]
	public class MirrorMultiplayerVehicle : NetworkBehaviour
	{
		private NetworkIdentity _networkIdentity;

		private VehicleController _vehicleController;

		private bool _vehicleInitialized;

		private void Awake()
		{
			_networkIdentity = GetComponent<NetworkIdentity>();
			_vehicleController = GetComponent<VehicleController>();
			_vehicleController.onVehicleInitialized.AddListener(delegate
			{
				_vehicleInitialized = true;
				_vehicleController.MultiplayerIsRemote = !_networkIdentity.isOwned;
			});
		}

		private void Update()
		{
			if (_vehicleInitialized)
			{
				if (_networkIdentity.isServerOnly)
				{
					RpcMultiplayerState(_vehicleController.GetMultiplayerState());
				}
				else if (_networkIdentity.isOwned)
				{
					CmdMultiplayerState(_vehicleController.GetMultiplayerState());
				}
			}
		}

		[Command]
		private void CmdMultiplayerState(VehicleController.MultiplayerState value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(writer, value);
			SendCommandInternal("System.Void NWH.VehiclePhysics2.Multiplayer.MirrorMultiplayerVehicle::CmdMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", -314744535, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcMultiplayerState(VehicleController.MultiplayerState value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(writer, value);
			SendRPCInternal("System.Void NWH.VehiclePhysics2.Multiplayer.MirrorMultiplayerVehicle::RpcMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", 960988946, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdMultiplayerState__MultiplayerState(VehicleController.MultiplayerState value)
		{
			if (_vehicleInitialized)
			{
				RpcMultiplayerState(value);
			}
		}

		protected static void InvokeUserCode_CmdMultiplayerState__MultiplayerState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdMultiplayerState called on client.");
			}
			else
			{
				((MirrorMultiplayerVehicle)obj).UserCode_CmdMultiplayerState__MultiplayerState(GeneratedNetworkCode._Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(reader));
			}
		}

		protected void UserCode_RpcMultiplayerState__MultiplayerState(VehicleController.MultiplayerState value)
		{
			if (_vehicleInitialized && !_networkIdentity.isOwned)
			{
				_vehicleController.SetMultiplayerState(value);
			}
		}

		protected static void InvokeUserCode_RpcMultiplayerState__MultiplayerState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcMultiplayerState called on server.");
			}
			else
			{
				((MirrorMultiplayerVehicle)obj).UserCode_RpcMultiplayerState__MultiplayerState(GeneratedNetworkCode._Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(reader));
			}
		}

		static MirrorMultiplayerVehicle()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(MirrorMultiplayerVehicle), "System.Void NWH.VehiclePhysics2.Multiplayer.MirrorMultiplayerVehicle::CmdMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", InvokeUserCode_CmdMultiplayerState__MultiplayerState, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(MirrorMultiplayerVehicle), "System.Void NWH.VehiclePhysics2.Multiplayer.MirrorMultiplayerVehicle::RpcMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", InvokeUserCode_RpcMultiplayerState__MultiplayerState);
		}
	}
}
