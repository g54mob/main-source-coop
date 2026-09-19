using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MessagePack
{
	internal class SipHash
	{
		private readonly ulong initialState0;

		private readonly ulong initialState1;

		public SipHash()
		{
			using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			Span<byte> span = stackalloc byte[16];
			randomNumberGenerator.GetBytes(span);
			initialState0 = 0x736F6D6570736575L ^ BinaryPrimitives.ReadUInt64LittleEndian(span);
			initialState1 = 0x646F72616E646F6DL ^ BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8));
		}

		public SipHash(ReadOnlySpan<byte> key)
		{
			if (key.Length != 16)
			{
				throw new ArgumentException("SipHash key must be exactly 128-bit long (16 bytes).", "key");
			}
			initialState0 = 0x736F6D6570736575L ^ BinaryPrimitives.ReadUInt64LittleEndian(key);
			initialState1 = 0x646F72616E646F6DL ^ BinaryPrimitives.ReadUInt64LittleEndian(key.Slice(8));
		}

		public void GetKey(Span<byte> key)
		{
			if (key.Length != 16)
			{
				throw new ArgumentException("SipHash key must be exactly 128-bit long (16 bytes).", "key");
			}
			BinaryPrimitives.WriteUInt64LittleEndian(key, initialState0 ^ 0x736F6D6570736575L);
			BinaryPrimitives.WriteUInt64LittleEndian(key.Slice(8), initialState1 ^ 0x646F72616E646F6DL);
		}

		public long Compute(ReadOnlySpan<byte> data)
		{
			ulong num = initialState0;
			ulong num2 = initialState1;
			ulong num3 = 0x1F160A001E161714L ^ initialState0;
			ulong num4 = 0x100A160317100A1EL ^ initialState1;
			int num5 = data.Length & -8;
			ulong num6;
			for (int i = 0; i < num5; i += 8)
			{
				num6 = MemoryMarshal.Read<ulong>(data.Slice(i));
				num4 ^= num6;
				num += num2;
				num3 += num4;
				num2 = (num2 << 13) | (num2 >> 51);
				num4 = (num4 << 16) | (num4 >> 48);
				num2 ^= num;
				num4 ^= num3;
				num = (num << 32) | (num >> 32);
				num3 += num2;
				num += num4;
				num2 = (num2 << 17) | (num2 >> 47);
				num4 = (num4 << 21) | (num4 >> 43);
				num2 ^= num3;
				num4 ^= num;
				num3 = (num3 << 32) | (num3 >> 32);
				num += num2;
				num3 += num4;
				num2 = (num2 << 13) | (num2 >> 51);
				num4 = (num4 << 16) | (num4 >> 48);
				num2 ^= num;
				num4 ^= num3;
				num = (num << 32) | (num >> 32);
				num3 += num2;
				num += num4;
				num2 = (num2 << 17) | (num2 >> 47);
				num4 = (num4 << 21) | (num4 >> 43);
				num2 ^= num3;
				num4 ^= num;
				num3 = (num3 << 32) | (num3 >> 32);
				num ^= num6;
			}
			num6 = (ulong)((long)data.Length << 56);
			switch (data.Length & 7)
			{
			case 7:
				num6 |= MemoryMarshal.Read<uint>(data.Slice(num5)) | ((ulong)MemoryMarshal.Read<ushort>(data.Slice(num5 + 4)) << 32) | ((ulong)data[num5 + 6] << 48);
				break;
			case 6:
				num6 |= MemoryMarshal.Read<uint>(data.Slice(num5)) | ((ulong)MemoryMarshal.Read<ushort>(data.Slice(num5 + 4)) << 32);
				break;
			case 5:
				num6 |= MemoryMarshal.Read<uint>(data.Slice(num5)) | ((ulong)data[num5 + 4] << 32);
				break;
			case 4:
				num6 |= MemoryMarshal.Read<uint>(data.Slice(num5));
				break;
			case 3:
				num6 |= MemoryMarshal.Read<ushort>(data.Slice(num5)) | ((ulong)data[num5 + 2] << 16);
				break;
			case 2:
				num6 |= MemoryMarshal.Read<ushort>(data.Slice(num5));
				break;
			case 1:
				num6 |= data[num5];
				break;
			}
			num4 ^= num6;
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			num ^= num6;
			num3 ^= 0xFF;
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			num += num2;
			num3 += num4;
			num2 = (num2 << 13) | (num2 >> 51);
			num4 = (num4 << 16) | (num4 >> 48);
			num2 ^= num;
			num4 ^= num3;
			num = (num << 32) | (num >> 32);
			num3 += num2;
			num += num4;
			num2 = (num2 << 17) | (num2 >> 47);
			num4 = (num4 << 21) | (num4 >> 43);
			num2 ^= num3;
			num4 ^= num;
			num3 = (num3 << 32) | (num3 >> 32);
			return (long)(num ^ num2 ^ (num3 ^ num4));
		}
	}
}
