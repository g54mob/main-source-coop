using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class AddClipToShareQueuePackage : CustomCommandPackage<CustomCommandType>
{
	public VideoHandle VideoHandle;

	public ClipID ClipID;

	public int ClipOwner;

	public bool isReRequest;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteInt(ClipOwner);
		binarySerializer.WriteGuid(VideoHandle.id);
		binarySerializer.WriteGuid(ClipID.id);
		binarySerializer.WriteBool(isReRequest);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		ClipOwner = binaryDeserializer.ReadInt();
		VideoHandle = new VideoHandle(binaryDeserializer.ReadGuid());
		ClipID = new ClipID(binaryDeserializer.ReadGuid());
		isReRequest = binaryDeserializer.ReadBool();
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.AddClipToShareQueue;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
