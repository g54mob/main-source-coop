using Mirror.Examples.Common.Controllers.Tank;
using UnityEngine;

namespace Mirror.Examples.Common
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[DisallowMultipleComponent]
	public class TankProjectile : MonoBehaviour
	{
		private enum CapsuleColliderDirection
		{
			XAxis = 0,
			YAxis = 1,
			ZAxis = 2
		}

		[Header("Components")]
		public Rigidbody rigidBody;

		public CapsuleCollider capsuleCollider;

		[Header("Settings")]
		public float destroyAfter = 3f;

		public float force = 1000f;

		private void OnValidate()
		{
			if (!Application.isPlaying)
			{
				Reset();
			}
		}

		private void Reset()
		{
			rigidBody = GetComponent<Rigidbody>();
			rigidBody.useGravity = false;
			rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			capsuleCollider = GetComponent<CapsuleCollider>();
			capsuleCollider.direction = 2;
			capsuleCollider.radius = 0.1f;
			capsuleCollider.height = 0.4f;
		}

		private void Start()
		{
			rigidBody.AddForce(base.transform.forward * force);
			Object.Destroy(base.gameObject, destroyAfter);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (NetworkServer.active && collision.gameObject.TryGetComponent<TankHealth>(out var component))
			{
				component.TakeDamage(1);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
