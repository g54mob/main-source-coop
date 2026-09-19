using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.PredictionBenchmark
{
	public class RandomForce : NetworkBehaviour
	{
		public float force = 10f;

		public float interval = 3f;

		private PredictedRigidbody prediction;

		private Rigidbody rb => prediction.predictedRigidbody;

		private void Awake()
		{
			prediction = GetComponent<PredictedRigidbody>();
		}

		public override void OnStartClient()
		{
			float time = Random.Range(0f, interval);
			InvokeRepeating("ApplyForce", time, interval);
		}

		[ClientCallback]
		private void ApplyForce()
		{
			if (NetworkClient.active)
			{
				Vector2 insideUnitCircle = Random.insideUnitCircle;
				Vector3 impulse = new Vector3(insideUnitCircle.x, 1f, insideUnitCircle.y) * force;
				rb.AddForce(impulse, ForceMode.Impulse);
				CmdApplyForce(impulse);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdApplyForce(Vector3 impulse)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(impulse);
			SendCommandInternal("System.Void Mirror.Examples.PredictionBenchmark.RandomForce::CmdApplyForce(UnityEngine.Vector3)", 561404971, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdApplyForce__Vector3(Vector3 impulse)
		{
			rb.AddForce(impulse, ForceMode.Impulse);
		}

		protected static void InvokeUserCode_CmdApplyForce__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdApplyForce called on client.");
			}
			else
			{
				((RandomForce)obj).UserCode_CmdApplyForce__Vector3(reader.ReadVector3());
			}
		}

		static RandomForce()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(RandomForce), "System.Void Mirror.Examples.PredictionBenchmark.RandomForce::CmdApplyForce(UnityEngine.Vector3)", InvokeUserCode_CmdApplyForce__Vector3, requiresAuthority: false);
		}
	}
}
