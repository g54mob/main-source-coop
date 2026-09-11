using System.Collections.Generic;
using UnityEngine;

public class ExtractVideoItemDetector : MonoBehaviour
{
	private Collider[] results = new Collider[10];

	public List<(Item item, Pickup pickup)> CheckForItems()
	{
		HashSet<Pickup> hashSet = new HashSet<Pickup>();
		List<(Item, Pickup)> list = new List<(Item, Pickup)>();
		int num = Physics.OverlapBoxNonAlloc(base.transform.position, base.transform.localScale * 0.5f, results, base.transform.rotation);
		for (int i = 0; i < num; i++)
		{
			Collider collider = results[i];
			Pickup componentInParent = collider.GetComponentInParent<Pickup>();
			if ((bool)componentInParent && componentInParent.itemInstance != null && componentInParent.itemInstance.item != null)
			{
				Item item = componentInParent.itemInstance.item;
				if (hashSet.Add(componentInParent))
				{
					list.Add((item, componentInParent));
					Debug.Log("Found pickup: " + item.name);
					Debug.DrawLine(collider.ClosestPoint(base.transform.position), base.transform.position, Color.blue);
				}
			}
		}
		return list;
	}

	private void OnDrawGizmos()
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.matrix = matrix;
	}
}
