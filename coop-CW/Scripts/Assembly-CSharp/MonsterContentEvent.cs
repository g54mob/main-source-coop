using UnityEngine;
using Zorro.Core.Serizalization;

public abstract class MonsterContentEvent : ContentEvent
{
	public int viewID;

	public Vector3 worldPosition;

	public override int GetUniqueID()
	{
		return viewID;
	}

	public override void Serialize(BinarySerializer serializer)
	{
		serializer.WriteInt(viewID);
		serializer.WriteFloat3(worldPosition);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		viewID = deserializer.ReadInt();
		worldPosition = deserializer.ReadFloat3();
	}
}
