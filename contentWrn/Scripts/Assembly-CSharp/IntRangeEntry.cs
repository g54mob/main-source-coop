using Zorro.Core.Serizalization;

public class IntRangeEntry : ItemDataEntry
{
	public int selectedValue;

	public int maxValue;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteInt(selectedValue);
		binarySerializer.WriteInt(maxValue);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		selectedValue = binaryDeserializer.ReadInt();
		maxValue = binaryDeserializer.ReadInt();
	}
}
