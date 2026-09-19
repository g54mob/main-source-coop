using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	[ExecuteInEditMode]
	public class PenetrationVisualizer : MonoBehaviour
	{
		private float radius = 3f;

		private int maxNeighbours = 16;

		private Collider[] neighbours;

		private Collider thisCollider;

		private void OnEnable()
		{
			neighbours = new Collider[maxNeighbours];
			thisCollider = GetComponent<Collider>();
			if (!thisCollider)
			{
				Debug.LogWarning("PenetrationVisualizer requires a Collider component.", this);
			}
		}

		private void OnDrawGizmos()
		{
			if (thisCollider == null)
			{
				return;
			}
			int num = Physics.OverlapSphereNonAlloc(base.transform.position, radius, neighbours);
			for (int i = 0; i < num; i++)
			{
				Collider collider = neighbours[i];
				if ((bool)collider && !(collider == thisCollider) && Physics.ComputePenetration(thisCollider, base.transform.position, base.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawRay(collider.transform.position, direction * distance);
				}
			}
		}

		private void OnValidate()
		{
			radius = Mathf.Max(0f, radius);
			maxNeighbours = Mathf.Clamp(maxNeighbours, 1, 256);
		}
	}
}
