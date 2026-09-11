using Zorro.Core.Serizalization;

public class IntEntry : ItemDataEntry
{
	public int i;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteInt(i);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		i = binaryDeserializer.ReadInt();
	}
}
