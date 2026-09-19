using UnityEngine;

namespace Mirror.Examples.Billiards
{
	public class RedBall : NetworkBehaviour
	{
		[ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (NetworkServer.active)
			{
				NetworkServer.Destroy(base.gameObject);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
