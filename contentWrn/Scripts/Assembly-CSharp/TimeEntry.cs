using Zorro.Core.Serizalization;

public class TimeEntry : ItemDataEntry
{
	public float currentTime;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteFloat(currentTime);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		currentTime = binaryDeserializer.ReadFloat();
	}
}
