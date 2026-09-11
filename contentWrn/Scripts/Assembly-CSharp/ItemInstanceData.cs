using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.Serizalization;

public class ItemInstanceData
{
	public Guid m_guid;

	public HashSet<ItemDataEntry> m_dataEntries = new HashSet<ItemDataEntry>();

	public float timeSinceGrounded;

	private bool isHeld;

	private bool isHeldByMe;

	public float timeInHand;

	public ItemInstanceData(Guid guid)
	{
		m_guid = guid;
	}

	public bool TryGetEntry<T>(out T t) where T : ItemDataEntry
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			if (dataEntry is T val)
			{
				t = val;
				return true;
			}
		}
		t = null;
		return false;
	}

	public void TryGetElseAdd<T>(ref T startValue) where T : ItemDataEntry
	{
		if (TryGetEntry<T>(out var t))
		{
			startValue = t;
		}
		else
		{
			AddDataEntry(startValue);
		}
	}

	public bool TryGetEntry(Type type, out ItemDataEntry t)
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			if (dataEntry.GetType() == type)
			{
				t = dataEntry;
				return true;
			}
		}
		t = null;
		return false;
	}

	public void AddDataEntry(ItemDataEntry entry)
	{
		m_dataEntries.Add(entry);
	}

	public void RemoveDataEntry(ItemDataEntry entry)
	{
		m_dataEntries.Remove(entry);
	}

	public void Serialize(BinarySerializer binarySerializer, bool createNewGuid)
	{
		if (!createNewGuid)
		{
			binarySerializer.WriteGuid(m_guid);
		}
		else
		{
			binarySerializer.WriteGuid(Guid.NewGuid());
		}
		binarySerializer.WriteByte((byte)m_dataEntries.Count);
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			binarySerializer.WriteByte(GetEntryIdentifier(dataEntry.GetType()));
			dataEntry.Serialize(binarySerializer);
		}
		binarySerializer.WriteHalf((half)timeSinceGrounded);
		binarySerializer.WriteHalf((half)timeInHand);
	}

	public byte[] Serialize(bool createNewGuid = false)
	{
		BinarySerializer binarySerializer = new BinarySerializer(32, Allocator.Temp);
		Serialize(binarySerializer, createNewGuid);
		byte[] result = binarySerializer.buffer.ToArray();
		binarySerializer.Dispose();
		return result;
	}

	public NativeArray<byte> SerializeAsNative()
	{
		BinarySerializer binarySerializer = new BinarySerializer(32, Allocator.Temp);
		Serialize(binarySerializer, createNewGuid: false);
		NativeArray<byte> buffer = binarySerializer.buffer;
		binarySerializer.Dispose();
		return buffer;
	}

	public void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		m_guid = binaryDeserializer.ReadGuid();
		int num = binaryDeserializer.ReadByte();
		for (int i = 0; i < num; i++)
		{
			ItemDataEntry entryType = GetEntryType(binaryDeserializer.ReadByte());
			bool flag = true;
			if (!TryGetEntry(entryType.GetType(), out var t))
			{
				t = entryType;
				flag = false;
			}
			t.Deserialize(binaryDeserializer);
			if (!flag)
			{
				m_dataEntries.Add(t);
			}
		}
		timeSinceGrounded = binaryDeserializer.ReadHalf();
		timeInHand = binaryDeserializer.ReadHalf();
	}

	public void Deserialize(byte[] bytes)
	{
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(new NativeArray<byte>(bytes, Allocator.Temp));
		Deserialize(binaryDeserializer);
		binaryDeserializer.Dispose();
	}

	public byte GetEntryIdentifier(Type type)
	{
		if (type == typeof(BatteryEntry))
		{
			return 1;
		}
		if (type == typeof(OnOffEntry))
		{
			return 2;
		}
		if (type == typeof(VideoInfoEntry))
		{
			return 3;
		}
		if (type == typeof(FlashcardEntry))
		{
			return 4;
		}
		if (type == typeof(LifeTimeEntry))
		{
			return 5;
		}
		if (type == typeof(StashAbleEntry))
		{
			return 6;
		}
		if (type == typeof(IntRangeEntry))
		{
			return 7;
		}
		if (type == typeof(TimeEntry))
		{
			return 8;
		}
		if (type == typeof(IntEntry))
		{
			return 9;
		}
		if (type == typeof(TitleCardDataEntry))
		{
			return 10;
		}
		throw new Exception($"Unknown entry type: {type}");
	}

	public ItemDataEntry GetEntryType(byte identifier)
	{
		return identifier switch
		{
			1 => new BatteryEntry(), 
			2 => new OnOffEntry(), 
			3 => new VideoInfoEntry(), 
			4 => new FlashcardEntry(), 
			5 => new LifeTimeEntry(), 
			6 => new StashAbleEntry(), 
			7 => new IntRangeEntry(), 
			8 => new TimeEntry(), 
			9 => new IntEntry(), 
			10 => new TitleCardDataEntry(), 
			_ => throw new Exception($"Unknown entry identifier: {identifier}"), 
		};
	}

	public ItemDataSyncer CreateDataSyncer(bool roomOwned)
	{
		if (roomOwned && !PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Tried to create a data syncer for a room owned item on a non master client");
			return null;
		}
		PhotonView photonView = null;
		photonView = ((!roomOwned) ? PhotonNetwork.Instantiate("ItemDataSyncer", Vector3.zero, Quaternion.identity, 0).GetComponent<PhotonView>() : PhotonNetwork.InstantiateRoomObject("ItemDataSyncer", Vector3.zero, Quaternion.identity, 0).GetComponent<PhotonView>());
		ItemDataSyncer component = photonView.GetComponent<ItemDataSyncer>();
		byte[] array = m_guid.ToByteArray();
		photonView.RPC("RPC_SetInstanceData", RpcTarget.AllBuffered, array);
		return component;
	}

	public bool IsDirty()
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			if (dataEntry.IsDirty())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsForceDirty()
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			if (dataEntry.IsForceDirty())
			{
				return true;
			}
		}
		return false;
	}

	public void ClearForceDirty()
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			dataEntry.ClearForceDirty();
		}
	}

	public void ClearDirty()
	{
		foreach (ItemDataEntry dataEntry in m_dataEntries)
		{
			dataEntry.ClearDirty();
		}
	}

	public ItemInstanceData Copy()
	{
		byte[] bytes = Serialize(createNewGuid: true);
		ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.NewGuid());
		itemInstanceData.Deserialize(bytes);
		return itemInstanceData;
	}
}
