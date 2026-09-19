using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.Billiards
{
	public class WhiteBall : NetworkBehaviour
	{
		public LineRenderer dragIndicator;

		public Rigidbody rigidBody;

		public float forceMultiplier = 2f;

		public float maxForce = 40f;

		private Vector3 startPosition;

		private bool MouseToWorld(out Vector3 position)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (new Plane(Vector3.up, base.transform.position).Raycast(ray, out var enter))
			{
				position = ray.GetPoint(enter);
				return true;
			}
			position = default(Vector3);
			return false;
		}

		private void Awake()
		{
			startPosition = base.transform.position;
		}

		[ClientCallback]
		private void OnMouseDown()
		{
			if (NetworkClient.active)
			{
				dragIndicator.SetPosition(0, base.transform.position);
				dragIndicator.SetPosition(1, base.transform.position);
				dragIndicator.gameObject.SetActive(value: true);
			}
		}

		[ClientCallback]
		private void OnMouseDrag()
		{
			if (NetworkClient.active && MouseToWorld(out var position))
			{
				dragIndicator.SetPosition(0, base.transform.position);
				dragIndicator.SetPosition(1, position);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdApplyForce(Vector3 force)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(force);
			SendCommandInternal("System.Void Mirror.Examples.Billiards.WhiteBall::CmdApplyForce(UnityEngine.Vector3)", 1388127707, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientCallback]
		private void OnMouseUp()
		{
			if (NetworkClient.active && MouseToWorld(out var position))
			{
				Vector3 position2 = base.transform.position;
				Debug.DrawLine(position2, position, Color.white, 2f);
				Vector3 vector = (position2 - position) * forceMultiplier;
				vector = Vector3.ClampMagnitude(vector, maxForce);
				CmdApplyForce(vector);
				dragIndicator.gameObject.SetActive(value: false);
			}
		}

		[ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (NetworkServer.active)
			{
				rigidBody.position = startPosition;
				rigidBody.Sleep();
				GetComponent<NetworkRigidbodyUnreliable>().RpcTeleport(startPosition);
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdApplyForce__Vector3(Vector3 force)
		{
			rigidBody.AddForce(force, ForceMode.Impulse);
		}

		protected static void InvokeUserCode_CmdApplyForce__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdApplyForce called on client.");
			}
			else
			{
				((WhiteBall)obj).UserCode_CmdApplyForce__Vector3(reader.ReadVector3());
			}
		}

		static WhiteBall()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(WhiteBall), "System.Void Mirror.Examples.Billiards.WhiteBall::CmdApplyForce(UnityEngine.Vector3)", InvokeUserCode_CmdApplyForce__Vector3, requiresAuthority: false);
		}
	}
}
