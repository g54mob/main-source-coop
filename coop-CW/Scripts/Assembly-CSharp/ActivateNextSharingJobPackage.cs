using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class ActivateNextSharingJobPackage : CustomCommandPackage<CustomCommandType>
{
	public ClipID ClipID;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteGuid(ClipID.id);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		ClipID = new ClipID(binaryDeserializer.ReadGuid());
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.ActivateNextSharingJob;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
