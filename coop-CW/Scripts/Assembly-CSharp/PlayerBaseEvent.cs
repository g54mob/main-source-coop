using System.Text;
using UnityEngine;
using Zorro.Core.Serizalization;

public abstract class PlayerBaseEvent : ContentEvent
{
	public string playerName;

	public int actorNumber;

	public Vector3 worldPosition;

	public PlayerBaseEvent()
	{
	}

	public PlayerBaseEvent(string playerName, int actorNumber, Vector3 worldPosition)
	{
		this.playerName = playerName;
		this.actorNumber = actorNumber;
		this.worldPosition = worldPosition;
	}

	public override int GetUniqueID()
	{
		return actorNumber;
	}

	public string FixPlayerName(string comment)
	{
		return comment.Replace("<playername>", playerName);
	}

	public override void Serialize(BinarySerializer serializer)
	{
		serializer.WriteInt(actorNumber);
		serializer.WriteString(playerName, Encoding.UTF8);
		serializer.WriteFloat3(worldPosition);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		actorNumber = deserializer.ReadInt();
		playerName = deserializer.ReadString(Encoding.UTF8);
		worldPosition = deserializer.ReadFloat3();
	}
}
