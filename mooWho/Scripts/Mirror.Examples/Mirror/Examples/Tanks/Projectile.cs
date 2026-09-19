using UnityEngine;

namespace Mirror.Examples.Tanks
{
	public class Projectile : NetworkBehaviour
	{
		public float destroyAfter = 2f;

		public Rigidbody rigidBody;

		public float force = 1000f;

		public override void OnStartServer()
		{
			Invoke("DestroySelf", destroyAfter);
		}

		private void Start()
		{
			rigidBody.AddForce(base.transform.forward * force);
		}

		[Server]
		private void DestroySelf()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.Examples.Tanks.Projectile::DestroySelf()' called when server was not active");
			}
			else
			{
				NetworkServer.Destroy(base.gameObject);
			}
		}

		[ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			Debug.Log("Hit: " + other.name);
			if (other.transform.parent.TryGetComponent<Tank>(out var component))
			{
				Tank tank = component;
				tank.Networkhealth = tank.health - 1;
				if (component.health == 0)
				{
					NetworkServer.RemovePlayerForConnection(component.netIdentity.connectionToClient, RemovePlayerOptions.Destroy);
				}
				DestroySelf();
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
