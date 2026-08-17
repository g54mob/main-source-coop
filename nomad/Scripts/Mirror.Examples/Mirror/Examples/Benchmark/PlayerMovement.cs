using UnityEngine;

namespace Mirror.Examples.Benchmark
{
	public class PlayerMovement : NetworkBehaviour
	{
		public float speed = 5f;

		public override void OnStartClient()
		{
			base.name = string.Format("Player[{0}|{1}]", base.netId, base.isLocalPlayer ? "local" : "remote");
		}

		public override void OnStartServer()
		{
			base.name = $"Player[{base.netId}|server]";
		}

		private void Update()
		{
			if (base.isLocalPlayer)
			{
				float axis = Input.GetAxis("Horizontal");
				float axis2 = Input.GetAxis("Vertical");
				Vector3 vector = new Vector3(axis, 0f, axis2);
				base.transform.position += vector.normalized * (Time.deltaTime * speed);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
