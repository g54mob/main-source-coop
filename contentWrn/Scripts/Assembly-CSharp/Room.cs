using UnityEngine;

public class Room : MonoBehaviour
{
	public float weight = 1f;

	public bool startRoom;

	public GameObject doorBlocker;

	public void SnapGizmos()
	{
		Door[] componentsInChildren = GetComponentsInChildren<Door>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SnapToRoom();
		}
	}

	internal bool Verify()
	{
		RoomCollider[] componentsInChildren = GetComponentsInChildren<RoomCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Collider component = componentsInChildren[i].GetComponent<Collider>();
			if (!VerifyCollider(component))
			{
				return false;
			}
		}
		return true;
	}

	public void SpawnDoorBlockers()
	{
		if (!doorBlocker)
		{
			return;
		}
		DoorBlocker[] componentsInChildren = GetComponentsInChildren<DoorBlocker>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].gameObject);
		}
		Door[] componentsInChildren2 = GetComponentsInChildren<Door>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (!componentsInChildren2[j].occupied)
			{
				Object.Instantiate(doorBlocker, componentsInChildren2[j].transform.position, componentsInChildren2[j].transform.rotation, base.transform);
			}
		}
	}

	public bool VerifyCollider(Collider col)
	{
		if (col.GetType() == typeof(BoxCollider))
		{
			BoxCollider boxCollider = (BoxCollider)col;
			Collider[] colliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.transform.lossyScale * 0.5f, boxCollider.transform.rotation);
			if (CollidesWithSometing(col, colliders))
			{
				return false;
			}
		}
		else
		{
			Bounds totalBounds = HelperFunctions.GetTotalBounds(col.gameObject);
			Collider[] colliders2 = Physics.OverlapBox(totalBounds.center, totalBounds.size * 0.5f);
			if (CollidesWithSometing(col, colliders2))
			{
				return false;
			}
		}
		return true;
	}

	private bool CollidesWithSometing(Collider ownCol, Collider[] colliders)
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			Room componentInParent = colliders[i].GetComponentInParent<Room>();
			if ((bool)colliders[i].GetComponent<RoomCollider>() && !(componentInParent == this))
			{
				float distance = 0f;
				Vector3 direction = default(Vector3);
				Physics.ComputePenetration(ownCol, ownCol.transform.position, ownCol.transform.rotation, colliders[i], colliders[i].transform.position, colliders[i].transform.rotation, out direction, out distance);
				if (distance > 0.25f)
				{
					return true;
				}
			}
		}
		return false;
	}
}
