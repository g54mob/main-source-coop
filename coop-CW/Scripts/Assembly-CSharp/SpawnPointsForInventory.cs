using System;
using UnityEngine;

public class SpawnPointsForInventory : MonoBehaviour
{
	private Transform[] m_Spawns;

	private void Awake()
	{
		int childCount = base.transform.childCount;
		m_Spawns = new Transform[childCount];
		for (int i = 0; i < childCount; i++)
		{
			m_Spawns[i] = base.transform.GetChild(i);
		}
	}

	public void SpawnItems(SavedInventoryItem[] savedInventoryItems)
	{
		int num = 0;
		foreach (SavedInventoryItem savedInventoryItem in savedInventoryItems)
		{
			if (!ItemDatabase.TryGetItemFromPersistentID(savedInventoryItem.GetPersistentID(), out var item))
			{
				Debug.Log("Item not found in database: " + savedInventoryItem.persistentID);
			}
			else if (item.itemType != Item.ItemType.Camera)
			{
				PickupHandler.CreatePickup(item.id, new ItemInstanceData(Guid.NewGuid()), m_Spawns[num++].position, Quaternion.identity);
				if (num >= m_Spawns.Length)
				{
					num = 0;
				}
			}
		}
	}
}
