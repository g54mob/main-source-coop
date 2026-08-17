using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NomadDrive.Sandbox.Test
{
	public class TestDummyPlayer : NetworkBehaviour
	{
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isLocalPlayer)
			{
				Camera componentInChildren = GetComponentInChildren<Camera>();
				if (componentInChildren != null)
				{
					componentInChildren.gameObject.SetActive(value: false);
				}
			}
		}

		[ClientRpc]
		public void RpcTeleport(Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			SendRPCInternal("System.Void NomadDrive.Sandbox.Test.TestDummyPlayer::RpcTeleport(UnityEngine.Vector3)", 209851173, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcTeleport__Vector3(Vector3 position)
		{
		}

		protected static void InvokeUserCode_RpcTeleport__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcTeleport called on server.");
			}
			else
			{
				((TestDummyPlayer)obj).UserCode_RpcTeleport__Vector3(reader.ReadVector3());
			}
		}

		static TestDummyPlayer()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(TestDummyPlayer), "System.Void NomadDrive.Sandbox.Test.TestDummyPlayer::RpcTeleport(UnityEngine.Vector3)", InvokeUserCode_RpcTeleport__Vector3);
		}
	}
}
