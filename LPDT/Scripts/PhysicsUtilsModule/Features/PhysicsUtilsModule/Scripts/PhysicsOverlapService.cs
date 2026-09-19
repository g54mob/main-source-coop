using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	public class PhysicsOverlapService : IPhysicsOverlapService
	{
		public bool IsOverlapping(Collider collider, int layerMask, out Vector3 resolutionVector)
		{
			resolutionVector = Vector3.zero;
			Bounds bounds = collider.bounds;
			Collider[] array = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, layerMask);
			bool result = false;
			Collider[] array2 = array;
			foreach (Collider collider2 in array2)
			{
				if (!(collider2 == collider) && Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, collider2, collider2.transform.position, collider2.transform.rotation, out var direction, out var distance))
				{
					resolutionVector += direction * distance;
					result = true;
				}
			}
			return result;
		}

		public void ResolveOverlap(Collider collider, int layerMask = -5, int maxIterations = 5, float bias = 0.05f)
		{
			Rigidbody component;
			bool flag = collider.TryGetComponent<Rigidbody>(out component);
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < maxIterations; i++)
			{
				if (!IsOverlapping(collider, layerMask, out var resolutionVector))
				{
					break;
				}
				zero += resolutionVector;
				if (flag)
				{
					component.position += resolutionVector;
				}
				else
				{
					collider.transform.position += resolutionVector;
				}
			}
			if (zero != Vector3.zero)
			{
				Vector3 vector = zero.normalized * bias;
				if (flag)
				{
					component.position += vector;
				}
				else
				{
					collider.transform.position += vector;
				}
			}
		}

		public void ResolveGroundOverlap(Collider collider, int groundLayerMask, float bias = 0.05f)
		{
			Rigidbody component;
			bool num = collider.TryGetComponent<Rigidbody>(out component);
			Vector3 vector = (num ? component.position : collider.transform.position);
			float y = collider.bounds.extents.y;
			RaycastHit hitInfo2;
			if (Physics.Raycast(vector, Vector3.up, out var hitInfo, y * 2f, groundLayerMask))
			{
				vector = hitInfo.point + Vector3.up * (y + bias);
			}
			else if (Physics.Raycast(vector, Vector3.down, out hitInfo2, y * 2f, groundLayerMask))
			{
				vector = hitInfo2.point + Vector3.up * (y + bias);
			}
			if (num)
			{
				component.position = vector;
			}
			else
			{
				collider.transform.position = vector;
			}
		}
	}
}
