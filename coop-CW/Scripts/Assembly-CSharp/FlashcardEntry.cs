using Zorro.Core.Serizalization;

public class FlashcardEntry : ItemDataEntry
{
	public VideoHandle videoID;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteGuid(videoID.id);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		videoID = new VideoHandle(binaryDeserializer.ReadGuid());
	}
}
