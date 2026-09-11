using System;
using UnityEngine;

[Serializable]
public class SavedSurfaceItem
{
	public string persistentID;

	public float posX;

	public float posY;

	public float posZ;

	public float rotX;

	public float rotY;

	public float rotZ;

	public float rotW;

	public Guid GetPersistentID()
	{
		if (!Guid.TryParse(persistentID, out var result))
		{
			return Guid.Empty;
		}
		return result;
	}

	public SavedSurfaceItem(PersistentObjectInfo persistentObject)
	{
		persistentID = persistentObject.Item.persistentID;
		posX = persistentObject.Position.x;
		posY = persistentObject.Position.y;
		posZ = persistentObject.Position.z;
		Quaternion rotation = persistentObject.Rotation;
		rotX = rotation.x;
		rotY = rotation.y;
		rotZ = rotation.z;
		rotW = rotation.w;
	}
}
