using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;
using Zorro.Core.Serizalization;

public class PlayerInventory : MonoBehaviour
{
	public Item[] itemsToSpawnWith;

	public const int ARTIFACT_SLOT = 3;

	public InventorySlot[] slots = new InventorySlot[4];

	public Item emote;

	private PhotonView m_photonView;

	private bool m_syncedInitialState;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i] = new InventorySlot(i, this);
		}
	}

	public bool TryGetSlotWithItem(Item item, out InventorySlot slot)
	{
		if (item == null)
		{
			Debug.LogError("Failed to get slot with item: item is null");
			slot = null;
			return false;
		}
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].ItemInSlot.item == item)
			{
				slot = slots[i];
				return true;
			}
		}
		slot = null;
		return false;
	}

	public bool TryGetSlotWithItemType(Item.ItemType itemType, out InventorySlot slot)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].ItemInSlot.item != null && slots[i].ItemInSlot.item.itemType == itemType)
			{
				slot = slots[i];
				return true;
			}
		}
		slot = null;
		return false;
	}

	public bool TryGetFeeSlot(out InventorySlot slot)
	{
		for (int i = 0; i < 3; i++)
		{
			if (slots[i].ItemInSlot.item == null)
			{
				slot = slots[i];
				return true;
			}
		}
		slot = null;
		return false;
	}

	public bool TryGetSlot(int slotID, out InventorySlot slot)
	{
		if (slotID >= 0 && slotID < slots.Length)
		{
			slot = slots[slotID];
			return true;
		}
		slot = null;
		return false;
	}

	public bool TryAddItem(ItemDescriptor item)
	{
		InventorySlot slot;
		return TryAddItem(item, out slot);
	}

	public bool TryAddItem(ItemDescriptor item, out InventorySlot slot)
	{
		Item.ItemType itemType = item.item.itemType;
		if (itemType == Item.ItemType.Artifact || itemType == Item.ItemType.Disc)
		{
			slot = slots[3];
			if (slot.ItemInSlot.item != null)
			{
				return false;
			}
			slots[3].Add(item);
			return true;
		}
		if (TryGetFeeSlot(out slot) && slot.SlotID < 3)
		{
			slot.Add(item);
			return true;
		}
		slot = null;
		return false;
	}

	public bool TryRemoveItemFromSlot(int slotID, out ItemDescriptor item)
	{
		if (TryGetSlot(slotID, out var slot) && slot.ItemInSlot.item != null)
		{
			item = slot.ItemInSlot;
			slot.Clear();
			return true;
		}
		item = ItemDescriptor.Empty;
		return false;
	}

	public bool TryRemoveItemOfType(Item item, out InventorySlot slot)
	{
		if (TryGetSlotWithItem(item, out slot))
		{
			slot.Clear();
			return true;
		}
		slot = null;
		return false;
	}

	public bool TryGetItemInSlot(int slotID, out ItemDescriptor item)
	{
		if (TryGetSlot(slotID, out var slot) && slot.ItemInSlot.item != null)
		{
			item = slot.ItemInSlot;
			return true;
		}
		item = ItemDescriptor.Empty;
		return false;
	}

	public void SyncAddToSlot(int slotID, ItemDescriptor itemDescriptor)
	{
		byte[] array = itemDescriptor.data.Serialize();
		m_photonView.RPC("RPC_AddToSlot", RpcTarget.All, slotID, itemDescriptor.item.id, array);
	}

	public void SyncClearSlot(int slotID)
	{
		m_photonView.RPC("RPC_ClearSlot", RpcTarget.All, slotID);
	}

	[PunRPC]
	public void RPC_AddToSlot(int slotID, byte itemID, byte[] data)
	{
		ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.Empty);
		itemInstanceData.Deserialize(data);
		if (ItemDatabase.TryGetItemFromID(itemID, out var item))
		{
			if (TryGetSlot(slotID, out var slot))
			{
				slot.AddLocal(new ItemDescriptor(item, itemInstanceData));
				VerboseDebug.Log($"adding item:{itemID} to slot: {slotID}");
			}
			else
			{
				Debug.LogError($"Failed to get slot {slotID}");
			}
		}
	}

	[PunRPC]
	public void RPC_ClearSlot(int slotID)
	{
		if (TryGetSlot(slotID, out var slot))
		{
			slot.ClearLocal();
			VerboseDebug.Log($"clearing slot: {slotID}");
		}
		else
		{
			Debug.LogError($"Failed to get slot {slotID}");
		}
	}

	private byte[] SerializeInventory()
	{
		BinarySerializer binarySerializer = new BinarySerializer(96, Allocator.Temp);
		for (int i = 0; i < slots.Length; i++)
		{
			bool flag = slots[i].ItemInSlot.item != null;
			binarySerializer.WriteByte((!flag) ? byte.MaxValue : slots[i].ItemInSlot.item.id);
			if (flag)
			{
				slots[i].ItemInSlot.data.Serialize(binarySerializer, createNewGuid: false);
			}
		}
		byte[] array = binarySerializer.buffer.ToArray();
		binarySerializer.Dispose();
		Debug.Log("Serializing inventory: " + array.Length);
		return array;
	}

	private void UpdateInventory(byte[] inventory)
	{
		VerboseDebug.Log("Updating inventory: " + inventory.Length);
		if (m_syncedInitialState)
		{
			return;
		}
		m_syncedInitialState = true;
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(new NativeArray<byte>(inventory, Allocator.Temp));
		ItemDescriptor[] array = new ItemDescriptor[slots.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = default(ItemDescriptor);
			byte b = binaryDeserializer.ReadByte();
			if (b < byte.MaxValue)
			{
				ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.Empty);
				itemInstanceData.Deserialize(binaryDeserializer);
				if (ItemDatabase.TryGetItemFromID(b, out var item))
				{
					array[i] = new ItemDescriptor(item, itemInstanceData);
				}
				else
				{
					Debug.LogError($"Failed to get item from id {b}");
				}
			}
			else
			{
				array[i] = ItemDescriptor.Empty;
			}
		}
		binaryDeserializer.Dispose();
		for (int j = 0; j < array.Length; j++)
		{
			ItemDescriptor item2 = array[j];
			if (item2.item != null)
			{
				slots[j].AddLocal(item2);
			}
			else
			{
				slots[j].ClearLocal();
			}
		}
	}

	public void SyncInventoryToOthers()
	{
		byte[] array = SerializeInventory();
		m_photonView.RPC("RPC_SyncInventoryToOthers", RpcTarget.Others, array);
	}

	[PunRPC]
	public void RPC_SyncInventoryToOthers(byte[] inventory)
	{
		UpdateInventory(inventory);
	}

	private void LateUpdate()
	{
	}

	public List<ItemDescriptor> GetItems()
	{
		List<ItemDescriptor> list = new List<ItemDescriptor>();
		InventorySlot[] array = slots;
		foreach (InventorySlot inventorySlot in array)
		{
			if (inventorySlot.ItemInSlot.item != null)
			{
				list.Add(inventorySlot.ItemInSlot);
			}
		}
		return list;
	}

	public void Clear()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].Clear();
		}
	}
}
