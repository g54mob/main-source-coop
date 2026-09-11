using UnityEngine;

public class HotbarUI : MonoBehaviour
{
	public HotbarSlotUI[] slots;

	public void Set(Player player)
	{
		if (!player.TryGetInventory(out var o))
		{
			return;
		}
		for (int i = 0; i < slots.Length; i++)
		{
			if (o.TryGetSlot(i, out var slot))
			{
				slots[i].SetData(slot, i == player.data.selectedItemSlot);
			}
		}
	}
}
