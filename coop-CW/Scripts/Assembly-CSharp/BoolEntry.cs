using Zorro.Core.Serizalization;

public class BoolEntry : ItemDataEntry
{
	public bool state;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteBool(state);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		state = binaryDeserializer.ReadBool();
	}
}
