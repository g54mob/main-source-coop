using UnityEngine;

namespace Mirror.Examples.RigidbodyPhysics
{
	[RequireComponent(typeof(Rigidbody))]
	public class AddForce : NetworkBehaviour
	{
		public Rigidbody rigidbody3d;

		public float force = 500f;

		protected override void OnValidate()
		{
			base.OnValidate();
			rigidbody3d = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (!rigidbody3d.isKinematic && Input.GetKeyDown(KeyCode.Space))
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
