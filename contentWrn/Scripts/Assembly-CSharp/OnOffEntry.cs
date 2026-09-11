using Zorro.Core.Serizalization;

public class OnOffEntry : ItemDataEntry
{
	public bool on;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteBool(on);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		on = binaryDeserializer.ReadBool();
	}
}
