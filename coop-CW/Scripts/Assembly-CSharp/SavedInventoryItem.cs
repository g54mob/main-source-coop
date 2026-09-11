using System;

[Serializable]
public class SavedInventoryItem
{
	public string persistentID;

	public Guid GetPersistentID()
	{
		if (!Guid.TryParse(persistentID, out var result))
		{
			return Guid.Empty;
		}
		return result;
	}

	public SavedInventoryItem(Item item)
	{
		persistentID = item.persistentID;
	}
}
