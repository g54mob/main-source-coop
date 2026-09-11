using UnityEngine;

namespace pworld.Scripts
{
	public class PSphereCollision : PCollision
	{
		public Vector3 lastPosition;

		private SphereCollider collider_gc;

		private void Awake()
		{
			collider_gc = GetComponentInChildren<SphereCollider>();
		}

		private void Update()
		{
			CastCollision();
		}

		private void LateUpdate()
		{
			lastPosition = base.transform.position;
		}

		private void CastCollision()
		{
			RaycastHit[] array = Physics.SphereCastAll(new Ray(lastPosition, base.transform.position - lastPosition), collider_gc.radius, Vector3.Distance(lastPosition, base.transform.position));
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit obj = array[i];
				if (!(obj.collider == collider_gc))
				{
					OnCollision?.Invoke(obj);
					OnCollisionLate?.Invoke(obj);
					break;
				}
			}
		}
	}
}
