using System;
using Unity.Collections;
using Zorro.Core.Serizalization;

public class TitleCardDataEntry : ItemDataEntry
{
	public NativeArray<uint> data;

	public Action OnDataChanged;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		NativeArray<byte> value = data.Reinterpret<byte>(4);
		binarySerializer.WriteInt(value.Length);
		binarySerializer.WriteBytes(value);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		int count = binaryDeserializer.ReadInt();
		data = binaryDeserializer.ReadBytes(count, Allocator.Persistent).Reinterpret<uint>(1);
		OnDataChanged?.Invoke();
	}
}
