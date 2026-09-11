using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class ReRequestClipPackage : CustomCommandPackage<CustomCommandType>
{
	public VideoHandle VideoHandle;

	public ClipID ClipID;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteGuid(VideoHandle.id);
		binarySerializer.WriteGuid(ClipID.id);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		VideoHandle = new VideoHandle(binaryDeserializer.ReadGuid());
		ClipID = new ClipID(binaryDeserializer.ReadGuid());
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.ReRequestClip;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
