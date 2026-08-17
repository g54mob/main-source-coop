using UnityEngine;

namespace Mirror.Examples.RigidbodyBenchmark
{
	[RequireComponent(typeof(Rigidbody))]
	public class AutoForce : NetworkBehaviour
	{
		public Rigidbody rigidbody3d;

		public float force = 500f;

		public float forceProbability = 0.05f;

		protected override void OnValidate()
		{
			base.OnValidate();
			rigidbody3d = GetComponent<Rigidbody>();
		}

		[ServerCallback]
		private void FixedUpdate()
		{
			if (NetworkServer.active && !rigidbody3d.isKinematic && Random.value < forceProbability * Time.deltaTime)
			{
				rigidbody3d.AddForce(Vector3.up * force);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
