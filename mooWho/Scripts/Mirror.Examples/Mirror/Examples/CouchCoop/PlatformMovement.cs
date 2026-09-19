using UnityEngine;

namespace Mirror.Examples.CouchCoop
{
	public class PlatformMovement : NetworkBehaviour
	{
		private bool onPlatform;

		private Transform platformTransform;

		private Vector3 lastPlatformPosition;

		public override void OnStartAuthority()
		{
			base.enabled = true;
		}

		private void FixedUpdate()
		{
			if (onPlatform)
			{
				Vector3 vector = platformTransform.position - lastPlatformPosition;
				base.transform.position += vector;
				lastPlatformPosition = platformTransform.position;
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Finish")
			{
				platformTransform = collision.gameObject.GetComponent<Transform>();
				lastPlatformPosition = platformTransform.position;
				onPlatform = true;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (collision.gameObject.tag == "Finish")
			{
				onPlatform = false;
				platformTransform = null;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
