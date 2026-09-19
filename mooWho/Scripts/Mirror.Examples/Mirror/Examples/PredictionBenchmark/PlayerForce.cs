using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.PredictionBenchmark
{
	public class PlayerForce : NetworkBehaviour
	{
		public float force = 50f;

		private void Update()
		{
			if (base.isLocalPlayer && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo) && PredictedRigidbody.IsPredicted(hitInfo.collider, out var predictedRigidbody))
			{
				Debug.Log("Applying force to: " + hitInfo.collider.name);
				Vector3 impulse = Random.insideUnitSphere * force;
				predictedRigidbody.predictedRigidbody.AddForce(impulse, ForceMode.Impulse);
				CmdApplyForce(predictedRigidbody.netIdentity, impulse);
			}
		}

		[Command]
		private void CmdApplyForce(NetworkIdentity cube, Vector3 impulse)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(cube);
			writer.WriteVector3(impulse);
			SendCommandInternal("System.Void Mirror.Examples.PredictionBenchmark.PlayerForce::CmdApplyForce(Mirror.NetworkIdentity,UnityEngine.Vector3)", 2109798548, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdApplyForce__NetworkIdentity__Vector3(NetworkIdentity cube, Vector3 impulse)
		{
			Debug.LogWarning($"CmdApplyForce: {force} to {cube.name}");
			cube.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
		}

		protected static void InvokeUserCode_CmdApplyForce__NetworkIdentity__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdApplyForce called on client.");
			}
			else
			{
				((PlayerForce)obj).UserCode_CmdApplyForce__NetworkIdentity__Vector3(reader.ReadNetworkIdentity(), reader.ReadVector3());
			}
		}

		static PlayerForce()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerForce), "System.Void Mirror.Examples.PredictionBenchmark.PlayerForce::CmdApplyForce(Mirror.NetworkIdentity,UnityEngine.Vector3)", InvokeUserCode_CmdApplyForce__NetworkIdentity__Vector3, requiresAuthority: true);
		}
	}
}
