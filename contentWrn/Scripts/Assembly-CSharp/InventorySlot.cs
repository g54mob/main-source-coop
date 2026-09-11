public class InventorySlot
{
	private PlayerInventory m_inventory;

	public int SlotID { get; private set; }

	public ItemDescriptor ItemInSlot { get; private set; }

	public InventorySlot(int slotID, PlayerInventory playerInventory)
	{
		SlotID = slotID;
		m_inventory = playerInventory;
	}

	public void Clear()
	{
		if (ItemInSlot.item != null)
		{
			m_inventory.SyncClearSlot(SlotID);
		}
	}

	public void ClearLocal()
	{
		if (!(ItemInSlot.item == null))
		{
			ItemInstanceDataHandler.RemoveInstanceData(ItemInSlot.data.m_guid);
			ItemInSlot = ItemDescriptor.Empty;
		}
	}

	public void Add(ItemDescriptor item)
	{
		if (ItemInSlot.item == null)
		{
			m_inventory.SyncAddToSlot(SlotID, item);
		}
	}

	public void AddLocal(ItemDescriptor item)
	{
		if (ItemInSlot.item == null)
		{
			ItemInSlot = item;
			ItemInstanceDataHandler.AddInstanceData(item.data);
		}
	}
}
