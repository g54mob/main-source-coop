using UnityEngine;

public class Door : MonoBehaviour
{
	public bool occupied;

	private void Start()
	{
		base.gameObject.SetActive(value: false);
	}

	public void SnapToRoom()
	{
		Collider[] componentsInChildren = GetComponentInParent<Room>().GetComponentsInChildren<Collider>();
		Vector3 position = base.transform.position;
		float num = 1E+12f;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].isTrigger)
			{
				Vector3 vector = componentsInChildren[i].ClosestPoint(base.transform.position);
				float num2 = Vector3.Distance(vector, base.transform.position);
				if (num2 < num)
				{
					num = num2;
					position = vector;
				}
			}
		}
		base.transform.position = position;
	}
}
