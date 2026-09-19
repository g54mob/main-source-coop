using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CartVirtualCollider : MonoBehaviour
	{
		public const string LAYER_NAME = "CartVirtualCollider";

		private const float WALL_THICKNESS = 0.1f;

		private const float WALL_SKIN = 0.04f;

		private void Awake()
		{
			int num = LayerMask.NameToLayer("CartVirtualCollider");
			if (num < 0)
			{
				Debug.LogError("[CartVirtualCollider] layer 'CartVirtualCollider' is not defined — the shell cannot be built", this);
				return;
			}
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (Collider collider in componentsInChildren)
			{
				if (collider == null || collider.isTrigger || !collider.enabled || !collider.gameObject.activeInHierarchy)
				{
					continue;
				}
				Bounds bounds2 = collider.bounds;
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = new Vector3(((j & 1) == 0) ? bounds2.min.x : bounds2.max.x, ((j & 2) == 0) ? bounds2.min.y : bounds2.max.y, ((j & 4) == 0) ? bounds2.min.z : bounds2.max.z);
					Vector3 vector = base.transform.InverseTransformPoint(position);
					if (!flag)
					{
						bounds = new Bounds(vector, Vector3.zero);
						flag = true;
					}
					else
					{
						bounds.Encapsulate(vector);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			GameObject obj = new GameObject("CartVirtualCollider");
			obj.transform.SetParent(base.transform, worldPositionStays: false);
			obj.layer = num;
			Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			float y = extents.y * 2f;
			AddWall(obj, num, new Vector3(center.x + extents.x + 0.04f, center.y, center.z), new Vector3(0.1f, y, extents.z * 2f));
			AddWall(obj, num, new Vector3(center.x - extents.x - 0.04f, center.y, center.z), new Vector3(0.1f, y, extents.z * 2f));
			AddWall(obj, num, new Vector3(center.x, center.y, center.z + extents.z + 0.04f), new Vector3(extents.x * 2f, y, 0.1f));
			AddWall(obj, num, new Vector3(center.x, center.y, center.z - extents.z - 0.04f), new Vector3(extents.x * 2f, y, 0.1f));
			Collider[] componentsInChildren2 = obj.GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider2 in componentsInChildren)
			{
				if (!(collider2 == null) && collider2.enabled && collider2.gameObject.activeInHierarchy)
				{
					for (int l = 0; l < componentsInChildren2.Length; l++)
					{
						Physics.IgnoreCollision(componentsInChildren2[l], collider2, ignore: true);
					}
				}
			}
		}

		private static void AddWall(GameObject shell, int layer, Vector3 localCentre, Vector3 size)
		{
			GameObject obj = new GameObject("Wall");
			obj.transform.SetParent(shell.transform, worldPositionStays: false);
			obj.transform.localPosition = localCentre;
			obj.layer = layer;
			obj.AddComponent<BoxCollider>().size = size;
		}
	}
}
