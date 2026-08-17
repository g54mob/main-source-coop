using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.BilliardsPredicted
{
	public class PlayerPredicted : NetworkBehaviour
	{
		private WhiteBallPredicted whiteBall;

		private void Awake()
		{
			whiteBall = Object.FindAnyObjectByType<WhiteBallPredicted>();
		}

		private void ApplyForceToWhite(Vector3 force)
		{
			whiteBall.GetComponent<PredictedRigidbody>().predictedRigidbody.AddForce(force, ForceMode.Impulse);
		}

		public void OnDraggedBall(Vector3 force)
		{
			ApplyForceToWhite(force);
			if (!base.isServer)
			{
				CmdApplyForce(force);
			}
		}

		private bool IsValidMove(Vector3 force)
		{
			return true;
		}

		[Command]
		private void CmdApplyForce(Vector3 force)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(force);
			SendCommandInternal("System.Void Mirror.Examples.BilliardsPredicted.PlayerPredicted::CmdApplyForce(UnityEngine.Vector3)", 974701952, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdApplyForce__Vector3(Vector3 force)
		{
			if (!IsValidMove(force))
			{
				Debug.Log($"Server rejected move: {force}");
			}
			else
			{
				ApplyForceToWhite(force);
			}
		}

		protected static void InvokeUserCode_CmdApplyForce__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdApplyForce called on client.");
			}
			else
			{
				((PlayerPredicted)obj).UserCode_CmdApplyForce__Vector3(reader.ReadVector3());
			}
		}

		static PlayerPredicted()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerPredicted), "System.Void Mirror.Examples.BilliardsPredicted.PlayerPredicted::CmdApplyForce(UnityEngine.Vector3)", InvokeUserCode_CmdApplyForce__Vector3, requiresAuthority: true);
		}
	}
}
