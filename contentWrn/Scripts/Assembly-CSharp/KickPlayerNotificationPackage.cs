using ExitGames.Client.Photon;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class KickPlayerNotificationPackage : CustomCommandPackage<CustomCommandType>
{
	protected override void SerializeData(BinarySerializer binarySerializer)
	{
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.KickPlayerNotification;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
