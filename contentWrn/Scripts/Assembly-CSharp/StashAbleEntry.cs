using Zorro.Core.Serizalization;

public class StashAbleEntry : ItemDataEntry
{
	public bool isStashAble;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteBool(isStashAble);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		isStashAble = binaryDeserializer.ReadBool();
	}
}
