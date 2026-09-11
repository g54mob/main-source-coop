using System;
using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PlayerEmoteContentEvent : PlayerBaseEvent
{
	public byte emoteItemID;

	public Item item;

	public PlayerEmoteContentEvent()
	{
	}

	public PlayerEmoteContentEvent(string playerName, int actorNumber, Item item, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
		this.item = item;
		emoteItemID = item.id;
	}

	public override float GetContentValue()
	{
		if (item == null)
		{
			Debug.LogError("item is null");
		}
		if (item.emoteInfo == null)
		{
			Debug.LogError("Item.emoteInfo is null");
		}
		return item.emoteInfo.emoteBaseScore;
	}

	public override ushort GetID()
	{
		return 1023;
	}

	public override string GetName()
	{
		return "PlayerEmoteing";
	}

	public override string[] GetAllComments()
	{
		return Array.Empty<string>();
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(item.emoteInfo.comments);
		return new Comment(list.GetRandom(), playerName);
	}

	public override void Serialize(BinarySerializer serializer)
	{
		base.Serialize(serializer);
		serializer.WriteByte(emoteItemID);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		base.Deserialize(deserializer);
		emoteItemID = deserializer.ReadByte();
		if (!ItemDatabase.TryGetItemFromID(emoteItemID, out item))
		{
			Debug.LogError("Item not found!");
		}
	}
}
