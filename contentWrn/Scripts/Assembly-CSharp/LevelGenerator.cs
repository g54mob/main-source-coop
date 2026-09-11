using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public Room[] rooms;

	public int nrOfRooms = 10;

	public void Go()
	{
		int num = 10;
		while (num > 0)
		{
			num--;
			Clear();
			if (Generate())
			{
				break;
			}
		}
		CheckAdditionalDoorOccupation();
		CloseDoors();
	}

	private void CheckAdditionalDoorOccupation()
	{
		Door[] componentsInChildren = GetComponentsInChildren<Door>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (i != j)
				{
					Transform transform = componentsInChildren[i].transform;
					Transform transform2 = componentsInChildren[j].transform;
					if (!(Mathf.Abs(transform.position.x - transform2.position.x) > 0.1f) && !(Mathf.Abs(transform.position.y - transform2.position.y) > 0.1f) && !(Mathf.Abs(transform.position.z - transform2.position.z) > 0.1f))
					{
						componentsInChildren[i].occupied = true;
						componentsInChildren[j].occupied = true;
					}
				}
			}
		}
	}

	private void CloseDoors()
	{
		Room[] componentsInChildren = GetComponentsInChildren<Room>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SpawnDoorBlockers();
		}
	}

	public void Clear()
	{
		Transform objectRoot = GetObjectRoot();
		for (int num = objectRoot.childCount - 1; num >= 0; num--)
		{
			Room component = objectRoot.GetChild(num).GetComponent<Room>();
			if (!component || !component.startRoom)
			{
				Object.DestroyImmediate(objectRoot.GetChild(num).gameObject);
			}
		}
		for (int i = 0; i < objectRoot.childCount; i++)
		{
			Door[] componentsInChildren = objectRoot.GetChild(i).GetComponentsInChildren<Door>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].occupied = false;
			}
		}
	}

	private bool Generate()
	{
		for (int i = 0; i < nrOfRooms; i++)
		{
			if (!SpawnRoom())
			{
				return false;
			}
		}
		return true;
	}

	private bool SpawnRoom()
	{
		int num = 500;
		while (num > 0)
		{
			num--;
			Door randomDoor = GetRandomDoor();
			Room roomToSpawn = GetRoomToSpawn();
			if (SpawnRoom(roomToSpawn, randomDoor))
			{
				break;
			}
		}
		return true;
	}

	private bool SpawnRoom(Room room, Door door)
	{
		GameObject gameObject = null;
		if (!Application.isEditor)
		{
			gameObject = Object.Instantiate(room.gameObject);
		}
		gameObject.transform.localScale *= base.transform.localScale.x;
		Room component = gameObject.GetComponent<Room>();
		Door enterenceDoor = GetEnterenceDoor(component);
		gameObject.transform.SetParent(GetObjectRoot());
		gameObject.transform.rotation = Quaternion.LookRotation(door.transform.forward);
		gameObject.transform.position = GetRoomConnectionPosition(component, door);
		Physics.SyncTransforms();
		if (VerifyRoomSpawn(component))
		{
			enterenceDoor.occupied = true;
			door.occupied = true;
			return true;
		}
		Object.DestroyImmediate(gameObject);
		return false;
	}

	private bool VerifyRoomSpawn(Room room)
	{
		return room.Verify();
	}

	private Vector3 GetRoomConnectionPosition(Room room, Door door)
	{
		Door opposingDoor = GetOpposingDoor(room, door);
		return room.transform.position + (door.transform.position - opposingDoor.transform.position);
	}

	private Door GetEnterenceDoor(Room spawnedRoom)
	{
		Door[] componentsInChildren = spawnedRoom.GetComponentsInChildren<Door>();
		float num = 0f;
		Door result = null;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			float num2 = Vector3.Angle(componentsInChildren[i].transform.forward, spawnedRoom.transform.forward);
			if (num2 > num)
			{
				num = num2;
				result = componentsInChildren[i];
			}
		}
		return result;
	}

	private Door GetOpposingDoor(Room room, Door door)
	{
		Door[] componentsInChildren = room.GetComponentsInChildren<Door>();
		float num = 0f;
		Door result = null;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			float num2 = Vector3.Angle(componentsInChildren[i].transform.forward, door.transform.forward);
			if (num2 > num)
			{
				num = num2;
				result = componentsInChildren[i];
			}
		}
		return result;
	}

	private Room GetRoomToSpawn()
	{
		return rooms[Random.Range(0, rooms.Length)];
	}

	private Door GetRandomDoor()
	{
		List<Door> unoccupiedDoors = GetUnoccupiedDoors();
		return unoccupiedDoors[Random.Range(0, unoccupiedDoors.Count)];
	}

	private List<Door> GetUnoccupiedDoors()
	{
		Door[] componentsInChildren = base.transform.GetComponentsInChildren<Door>();
		List<Door> list = new List<Door>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].occupied)
			{
				list.Add(componentsInChildren[i]);
			}
		}
		return list;
	}

	private Transform GetObjectRoot()
	{
		return base.transform.Find("ObjectRoot");
	}
}
