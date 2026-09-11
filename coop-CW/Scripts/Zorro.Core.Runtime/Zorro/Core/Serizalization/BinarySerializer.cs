using System;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core.Serizalization
{
	public class BinarySerializer : IDisposable
	{
		private NativeList<byte> m_buffer;

		public int Position => m_buffer.Length;

		public NativeArray<byte> buffer => m_buffer.AsArray();

		public BinarySerializer()
		{
			m_buffer = new NativeList<byte>(1, Allocator.Persistent);
		}

		public BinarySerializer(int defaultCapacity, Allocator allocator)
		{
			m_buffer = new NativeList<byte>(defaultCapacity, allocator);
		}

		public void Shorten()
		{
			NativeList<byte> nativeList = new NativeList<byte>(m_buffer.Length, Allocator.Persistent);
			nativeList.AddRangeNoResize(m_buffer);
			m_buffer.Dispose();
			m_buffer = nativeList;
		}

		public void Dispose()
		{
			m_buffer.Dispose();
		}

		public void WriteByte(byte value)
		{
			m_buffer.Add(in value);
		}

		public void WriteBytes(NativeArray<byte> value)
		{
			m_buffer.AddRange(value);
		}

		public void WriteBytes(byte[] value)
		{
			NativeArray<byte> array = value.ToNativeArray(Allocator.Temp);
			m_buffer.AddRange(array);
			array.Dispose();
		}

		public void WriteFloat(float value)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<float>()));
			nativeArray.Dispose();
		}

		public void WriteFloat2(float2 value)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(2, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<float>()));
			nativeArray.Dispose();
		}

		public void WriteFloat3(float3 value)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(3, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<float>()));
			nativeArray.Dispose();
		}

		public void WriteFloat4(float4 value)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(4, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			nativeArray[3] = value.w;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<float>()));
			nativeArray.Dispose();
		}

		public void WriteHalf(half value)
		{
			NativeArray<half> nativeArray = new NativeArray<half>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<half>()));
			nativeArray.Dispose();
		}

		public void WriteInt(int value)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<int>()));
			nativeArray.Dispose();
		}

		public void WriteInt2(int2 value)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(2, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<int>()));
			nativeArray.Dispose();
		}

		public void WriteInt3(int3 value)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(3, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<int>()));
			nativeArray.Dispose();
		}

		public void WriteInt4(int4 value)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(4, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			nativeArray[3] = value.w;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<int>()));
			nativeArray.Dispose();
		}

		public void WriteUInt(uint value)
		{
			NativeArray<uint> nativeArray = new NativeArray<uint>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<uint>()));
			nativeArray.Dispose();
		}

		public void WriteUInt2(uint2 value)
		{
			NativeArray<uint> nativeArray = new NativeArray<uint>(2, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<uint>()));
			nativeArray.Dispose();
		}

		public void WriteUInt3(uint3 value)
		{
			NativeArray<uint> nativeArray = new NativeArray<uint>(3, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<uint>()));
			nativeArray.Dispose();
		}

		public void WriteUInt4(uint4 value)
		{
			NativeArray<uint> nativeArray = new NativeArray<uint>(4, Allocator.Temp);
			nativeArray[0] = value.x;
			nativeArray[1] = value.y;
			nativeArray[2] = value.z;
			nativeArray[3] = value.w;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<uint>()));
			nativeArray.Dispose();
		}

		public void WriteBool(bool value)
		{
			NativeArray<bool> nativeArray = new NativeArray<bool>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<bool>()));
			nativeArray.Dispose();
		}

		public void WriteUlong(ulong value)
		{
			NativeArray<ulong> nativeArray = new NativeArray<ulong>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<ulong>()));
			nativeArray.Dispose();
		}

		public void WriteLong(long value)
		{
			NativeArray<long> nativeArray = new NativeArray<long>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<long>()));
			nativeArray.Dispose();
		}

		public void WriteShort(short value)
		{
			NativeArray<short> nativeArray = new NativeArray<short>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<short>()));
			nativeArray.Dispose();
		}

		public void WriteUshort(ushort value)
		{
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(1, Allocator.Temp);
			nativeArray[0] = value;
			WriteBytes(nativeArray.Reinterpret<byte>(UnsafeUtility.SizeOf<ushort>()));
			nativeArray.Dispose();
		}

		public void WriteOptionableLong(Optionable<long> value)
		{
			WriteBool(value.IsSome);
			if (value.IsSome)
			{
				WriteLong(value.Value);
			}
		}

		public void WriteGuid(Guid guid)
		{
			byte[] array = guid.ToByteArray();
			foreach (byte value in array)
			{
				WriteByte(value);
			}
		}

		public void WriteString(string comment, Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(comment);
			WriteInt(bytes.Length);
			NativeArray<byte> value = new NativeArray<byte>(bytes, Allocator.Temp);
			WriteBytes(value);
			value.Dispose();
		}

		public void WriteColor32(Color32 c)
		{
			WriteByte(c.r);
			WriteByte(c.g);
			WriteByte(c.b);
			WriteByte(c.a);
		}

		public void WriteQuaternion(Quaternion quaternion)
		{
			WriteFloat3(quaternion.eulerAngles);
		}
	}
}
