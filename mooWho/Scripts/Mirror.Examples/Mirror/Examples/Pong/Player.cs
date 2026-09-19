using UnityEngine;

namespace Mirror.Examples.Pong
{
	public class Player : NetworkBehaviour
	{
		public float speed = 30f;

		public Rigidbody2D rigidbody2d;

		private void FixedUpdate()
		{
			if (base.isLocalPlayer)
			{
				rigidbody2d.linearVelocity = new Vector2(0f, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
