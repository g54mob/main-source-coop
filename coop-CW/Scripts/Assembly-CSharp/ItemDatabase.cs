using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(menuName = "Database/ItemDatabase", order = 9999, fileName = "ItemDatabase")]
public class ItemDatabase : ObjectDatabaseAsset<ItemDatabase, Item>
{
	public List<string> lastLoadedPersistentIDs = new List<string>();

	public List<Item> lastLoadedItems = new List<Item>();

	public override void AddRuntimeEntry(Item obj)
	{
		byte id = 0;
		if (Objects.Count > 0)
		{
			id = (byte)(Objects.Max((Item item) => item.id) + 1);
		}
		obj.id = id;
		base.AddRuntimeEntry(obj);
	}

	public static bool TryGetItemFromID(byte id, out Item item)
	{
		foreach (Item @object in SingletonAsset<ItemDatabase>.Instance.Objects)
		{
			if (@object.id == id)
			{
				item = @object;
				return true;
			}
		}
		item = null;
		return false;
	}

	public static bool TryGetItemFromPersistentID(Guid id, out Item item)
	{
		foreach (Item @object in SingletonAsset<ItemDatabase>.Instance.Objects)
		{
			if (@object.PersistentID == id)
			{
				item = @object;
				return true;
			}
		}
		item = null;
		return false;
	}
}
