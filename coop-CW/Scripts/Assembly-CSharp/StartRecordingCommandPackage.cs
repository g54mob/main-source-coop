using System;
using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class StartRecordingCommandPackage : CustomCommandPackage<CustomCommandType>
{
	public Guid CameraDataGuid;

	public VideoHandle VideoID;

	public ClipID ClipID;

	public int CameraOwner;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteGuid(CameraDataGuid);
		binarySerializer.WriteGuid(VideoID.id);
		binarySerializer.WriteGuid(ClipID.id);
		binarySerializer.WriteInt(CameraOwner);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		CameraDataGuid = binaryDeserializer.ReadGuid();
		VideoID = new VideoHandle(binaryDeserializer.ReadGuid());
		ClipID = new ClipID(binaryDeserializer.ReadGuid());
		CameraOwner = binaryDeserializer.ReadInt();
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.StartRecording;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
