using System;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PlayerWearingHatContentEvent : PlayerBaseEvent
{
	private int hatID = 1;

	private Hat hatInDatabase;

	public PlayerWearingHatContentEvent()
	{
	}

	public PlayerWearingHatContentEvent(string playerName, int actorNumber, Vector3 worldPosition, int hatId)
		: base(playerName, actorNumber, worldPosition)
	{
		hatID = hatId;
		hatInDatabase = HatDatabase.instance.GetHatFromIndex(hatID);
		if (!hatInDatabase.giveContentEvent)
		{
			Debug.LogError("hat " + hatInDatabase.name + " is not supposed to give content event");
		}
	}

	public override float GetContentValue()
	{
		return hatInDatabase.contentValue;
	}

	public override ushort GetID()
	{
		return 1044;
	}

	public override int GetUniqueID()
	{
		return hatID;
	}

	public override string GetName()
	{
		if (!(hatInDatabase != null))
		{
			return string.Empty;
		}
		return hatInDatabase.name;
	}

	public override void Serialize(BinarySerializer serializer)
	{
		base.Serialize(serializer);
		serializer.WriteInt(hatID);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		base.Deserialize(deserializer);
		hatID = deserializer.ReadInt();
		hatInDatabase = HatDatabase.instance.GetHatFromIndex(hatID);
		if (!hatInDatabase.giveContentEvent)
		{
			Debug.LogError("hat " + hatInDatabase.name + " is not supposed to give content event");
		}
	}

	public override string[] GetAllComments()
	{
		return Array.Empty<string>();
	}

	public override Comment GenerateComment()
	{
		return new Comment(hatInDatabase.comments.GetRandom(), playerName);
	}
}
