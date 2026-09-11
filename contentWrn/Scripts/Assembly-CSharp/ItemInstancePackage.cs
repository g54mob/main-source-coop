using System;
using ExitGames.Client.Photon;
using Unity.Collections;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class ItemInstancePackage : CustomCommandPackage<CustomCommandType>
{
	public Guid ItemInstanceGuid;

	public ItemRPC ItemRPC;

	public NativeArray<byte> Data;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteGuid(ItemInstanceGuid);
		binarySerializer.WriteInt((int)ItemRPC);
		binarySerializer.WriteInt(Data.Length);
		binarySerializer.WriteBytes(Data);
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		ItemInstanceGuid = binaryDeserializer.ReadGuid();
		ItemRPC = (ItemRPC)binaryDeserializer.ReadInt();
		int count = binaryDeserializer.ReadInt();
		Data = binaryDeserializer.ReadBytes(count, Allocator.Persistent);
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.ItemInstance;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
