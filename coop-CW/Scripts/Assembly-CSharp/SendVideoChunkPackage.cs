using ExitGames.Client.Photon;
using Unity.Collections;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class SendVideoChunkPackage : CustomCommandPackage<CustomCommandType>
{
	public ushort ChunkCount;

	public ushort ChunkIndex;

	public VideoHandle VideoHandle;

	public ClipID ClipID;

	public byte[] VideoChunkData;

	public NativeArray<byte> ContentEventData;

	protected override void SerializeData(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteUshort(ChunkCount);
		binarySerializer.WriteUshort(ChunkIndex);
		binarySerializer.WriteGuid(VideoHandle.id);
		binarySerializer.WriteGuid(ClipID.id);
		NativeArray<byte> dst = new NativeArray<byte>(VideoChunkData.Length, Allocator.Temp);
		ByteArrayConvertion.MoveFromByteArray(ref VideoChunkData, ref dst);
		binarySerializer.WriteInt(VideoChunkData.Length);
		binarySerializer.WriteBytes(dst);
		if (ChunkIndex == 0)
		{
			binarySerializer.WriteInt(ContentEventData.Length);
			binarySerializer.WriteBytes(ContentEventData);
		}
		dst.Dispose();
		ContentEventData.Dispose();
	}

	public override void DeserializeData(BinaryDeserializer binaryDeserializer)
	{
		ChunkCount = binaryDeserializer.ReadUShort();
		ChunkIndex = binaryDeserializer.ReadUShort();
		VideoHandle = new VideoHandle(binaryDeserializer.ReadGuid());
		ClipID = new ClipID(binaryDeserializer.ReadGuid());
		int num = binaryDeserializer.ReadInt();
		NativeArray<byte> src = binaryDeserializer.ReadBytes(num, Allocator.Temp);
		VideoChunkData = new byte[num];
		ByteArrayConvertion.MoveToByteArray(ref src, ref VideoChunkData);
		if (ChunkIndex == 0)
		{
			int count = binaryDeserializer.ReadInt();
			ContentEventData = binaryDeserializer.ReadBytes(count, Allocator.Persistent);
		}
		src.Dispose();
	}

	public override CustomCommandType GetCommandType()
	{
		return CustomCommandType.SendVideoChunk;
	}

	public override SendOptions GetSendOptions()
	{
		return SendOptions.SendReliable;
	}
}
