using UnityEngine;

public struct PersistentObjectInfo
{
	public Item Item { get; private set; }

	public Vector3 Position { get; private set; }

	public Quaternion Rotation { get; private set; }

	public ItemInstanceData InstanceData { get; private set; }

	public Pickup Pickup { get; private set; }

	public PersistentObjectInfo(Item item, Vector3 pos, Quaternion rot, ItemInstanceData data, Pickup p)
	{
		Item = item;
		Position = pos;
		Rotation = rot;
		InstanceData = data;
		Pickup = p;
	}
}
