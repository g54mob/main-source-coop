using System;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core.Serizalization
{
	public class BinaryDeserializer : IDisposable
	{
		public NativeArray<byte> buffer;

		public int position;

		public BinaryDeserializer(NativeArray<byte> buffer)
		{
			this.buffer = buffer;
			position = 0;
		}

		public BinaryDeserializer(byte[] buffer, Allocator allocator)
		{
			NativeArray<byte> dst = new NativeArray<byte>(buffer, allocator);
			ByteArrayConvertion.MoveFromByteArray(ref buffer, ref dst);
			this.buffer = dst;
			position = 0;
		}

		public BinaryDeserializer(BinarySerializer serializer)
		{
			buffer = serializer.buffer;
			position = 0;
		}

		public void Dispose()
		{
			buffer.Dispose();
		}

		public byte ReadByte()
		{
			byte result = buffer[position];
			position++;
			return result;
		}

		public float ReadFloat()
		{
			float result = new NativeSlice<byte>(buffer, position, 4).SliceConvert<float>()[0];
			position += 4;
			return result;
		}

		public float2 ReadFloat2()
		{
			NativeSlice<float> nativeSlice = new NativeSlice<byte>(buffer, position, 8).SliceConvert<float>();
			position += 8;
			return new float2(nativeSlice[0], nativeSlice[1]);
		}

		public float3 ReadFloat3()
		{
			NativeSlice<float> nativeSlice = new NativeSlice<byte>(buffer, position, 12).SliceConvert<float>();
			position += 12;
			return new float3(nativeSlice[0], nativeSlice[1], nativeSlice[2]);
		}

		public float4 ReadFloat4()
		{
			NativeSlice<float> nativeSlice = new NativeSlice<byte>(buffer, position, 16).SliceConvert<float>();
			position += 16;
			return new float4(nativeSlice[0], nativeSlice[1], nativeSlice[2], nativeSlice[3]);
		}

		public half ReadHalf()
		{
			half result = new NativeSlice<byte>(buffer, position, 2).SliceConvert<half>()[0];
			position += 2;
			return result;
		}

		public int ReadInt()
		{
			int result = new NativeSlice<byte>(buffer, position, 4).SliceConvert<int>()[0];
			position += 4;
			return result;
		}

		public int2 ReadInt2()
		{
			NativeSlice<int> nativeSlice = new NativeSlice<byte>(buffer, position, 8).SliceConvert<int>();
			position += 8;
			return new int2(nativeSlice[0], nativeSlice[1]);
		}

		public int3 ReadInt3()
		{
			NativeSlice<int> nativeSlice = new NativeSlice<byte>(buffer, position, 12).SliceConvert<int>();
			position += 12;
			return new int3(nativeSlice[0], nativeSlice[1], nativeSlice[2]);
		}

		public int4 ReadInt4()
		{
			NativeSlice<int> nativeSlice = new NativeSlice<byte>(buffer, position, 16).SliceConvert<int>();
			position += 16;
			return new int4(nativeSlice[0], nativeSlice[1], nativeSlice[2], nativeSlice[3]);
		}

		public uint ReadUInt()
		{
			uint result = new NativeSlice<byte>(buffer, position, 4).SliceConvert<uint>()[0];
			position += 4;
			return result;
		}

		public uint2 ReadUInt2()
		{
			NativeSlice<uint> nativeSlice = new NativeSlice<byte>(buffer, position, 8).SliceConvert<uint>();
			position += 8;
			return new uint2(nativeSlice[0], nativeSlice[1]);
		}

		public uint3 ReadUInt3()
		{
			NativeSlice<uint> nativeSlice = new NativeSlice<byte>(buffer, position, 12).SliceConvert<uint>();
			position += 12;
			return new uint3(nativeSlice[0], nativeSlice[1], nativeSlice[2]);
		}

		public uint4 ReadUInt4()
		{
			NativeSlice<uint> nativeSlice = new NativeSlice<byte>(buffer, position, 16).SliceConvert<uint>();
			position += 16;
			return new uint4(nativeSlice[0], nativeSlice[1], nativeSlice[2], nativeSlice[3]);
		}

		public bool ReadBool()
		{
			bool result = buffer[position] > 0;
			position++;
			return result;
		}

		public ulong ReadUlong()
		{
			ulong result = new NativeSlice<byte>(buffer, position, 8).SliceConvert<ulong>()[0];
			position += 8;
			return result;
		}

		public long ReadLong()
		{
			long result = new NativeSlice<byte>(buffer, position, 8).SliceConvert<long>()[0];
			position += 8;
			return result;
		}

		public ushort ReadUShort()
		{
			ushort result = new NativeSlice<byte>(buffer, position, 2).SliceConvert<ushort>()[0];
			position += 2;
			return result;
		}

		public short ReadShort()
		{
			short result = new NativeSlice<byte>(buffer, position, 2).SliceConvert<short>()[0];
			position += 2;
			return result;
		}

		public NativeArray<byte> ReadBytes(int count, Allocator allocator)
		{
			NativeSlice<byte> nativeSlice = new NativeSlice<byte>(buffer, position, count);
			position += count;
			NativeArray<byte> nativeArray = new NativeArray<byte>(count, allocator);
			nativeSlice.CopyTo(nativeArray);
			return nativeArray;
		}

		public byte[] ReadBytes(int count)
		{
			NativeArray<byte> nativeArray = ReadBytes(count, Allocator.Temp);
			byte[] result = nativeArray.ToByteArray();
			nativeArray.Dispose();
			return result;
		}

		public Guid ReadGuid()
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = ReadByte();
			}
			return new Guid(array);
		}

		public string ReadString(Encoding encoding)
		{
			int num = ReadInt();
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadByte();
			}
			return encoding.GetString(array);
		}

		public Color ReadColor()
		{
			return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
		}

		public Quaternion ReadQuaternion()
		{
			return Quaternion.Euler(ReadFloat3());
		}
	}
}
