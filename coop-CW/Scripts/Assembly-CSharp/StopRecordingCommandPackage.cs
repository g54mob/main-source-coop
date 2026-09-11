using System;
using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class StopRecordingCommandPackage : CustomCommandPackage<CustomCommandType>
{
	public Guid CameraDataGuid;

	public bool IsValidClip;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteBool(IsValidClip);
		binarySerializer.WriteGuid(CameraDataGuid);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		IsValidClip = binaryDeserializer.ReadBool();
		CameraDataGuid = binaryDeserializer.ReadGuid();
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.StopRecording;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
