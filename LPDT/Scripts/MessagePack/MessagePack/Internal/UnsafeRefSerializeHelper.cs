using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MessagePack.Internal
{
	internal static class UnsafeRefSerializeHelper
	{
		internal static void Serialize(ref MessagePackWriter writer, ref bool input, int length)
		{
			ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(length));
			for (nuint num = 0u; num < checked((nuint)length); num = checked(num + 1))
			{
				Unsafe.Add(ref reference, num) = (byte)(Unsafe.Add(ref input, num) ? 195 : 194);
			}
			writer.Advance(length);
		}

		internal static void Serialize(ref MessagePackWriter writer, ref sbyte input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref sbyte input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref sbyte input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 1073741823)
					{
						num = 1073741823;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 2));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref short input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref short input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref short input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 715827882)
					{
						num = 715827882;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 3));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref char input, int length)
		{
			Serialize(ref writer, ref Unsafe.As<char, ushort>(ref input), length);
		}

		internal static void Serialize(ref MessagePackWriter writer, ref ushort input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref ushort input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref ushort input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 715827882)
					{
						num = 715827882;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 3));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref int input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref int input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref int input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 429496729)
					{
						num = 429496729;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 5));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref uint input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref uint input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref uint input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 429496729)
					{
						num = 429496729;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 5));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref long input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref long input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref long input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 238609294)
					{
						num = 238609294;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 9));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref ulong input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref ulong input, int length)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				writer.Write(Unsafe.Add(ref input, i));
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref ulong input, int length)
		{
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 238609294)
					{
						num = 238609294;
					}
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(num * 9));
					nuint num2 = 0u;
					for (nuint num3 = 0u; num3 < (nuint)num; num3++)
					{
						num2 += ReverseWriteUnknown(ref Unsafe.Add(ref reference, num2), Unsafe.Add(ref input, num3));
					}
					writer.Advance((int)num2);
					length -= num;
					input = ref Unsafe.Add(ref input, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref float input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref float input, int length)
		{
			ref uint source = ref Unsafe.As<float, uint>(ref input);
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 429496729)
					{
						num = 429496729;
					}
					int length2 = num * 5;
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(length2));
					nuint num2 = 0u;
					nuint num3 = 0u;
					while (num2 < (nuint)num)
					{
						Unsafe.Add(ref reference, num3) = 202;
						Unsafe.WriteUnaligned(ref Unsafe.Add(ref reference, num3 + 1), Unsafe.Add(ref source, num2));
						num2++;
						num3 += 5;
					}
					writer.Advance(length2);
					length -= num;
					source = ref Unsafe.Add(ref source, num);
				}
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref float input, int length)
		{
			ref uint source = ref Unsafe.As<float, uint>(ref input);
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 429496729)
					{
						num = 429496729;
					}
					int length2 = num * 5;
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(length2));
					nuint num2 = 0u;
					nuint num3 = 0u;
					while (num2 < (nuint)num)
					{
						Unsafe.Add(ref reference, num3) = 202;
						Unsafe.WriteUnaligned(ref Unsafe.Add(ref reference, num3 + 1), BinaryPrimitives.ReverseEndianness(Unsafe.Add(ref source, num2)));
						num2++;
						num3 += 5;
					}
					writer.Advance(length2);
					length -= num;
					source = ref Unsafe.Add(ref source, num);
				}
			}
		}

		internal static void Serialize(ref MessagePackWriter writer, ref double input, int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				LittleEndianSerialize(ref writer, ref input, length);
			}
			else
			{
				BigEndianSerialize(ref writer, ref input, length);
			}
		}

		private static void BigEndianSerialize(ref MessagePackWriter writer, ref double input, int length)
		{
			ref ulong source = ref Unsafe.As<double, ulong>(ref input);
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 238609294)
					{
						num = 238609294;
					}
					int length2 = num * 9;
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(length2));
					nuint num2 = 0u;
					nuint num3 = 0u;
					while (num2 < (nuint)num)
					{
						Unsafe.Add(ref reference, num3) = 203;
						Unsafe.WriteUnaligned(ref Unsafe.Add(ref reference, num3 + 1), Unsafe.Add(ref source, num2));
						num2++;
						num3 += 9;
					}
					writer.Advance(length2);
					length -= num;
					source = ref Unsafe.Add(ref source, num);
				}
			}
		}

		private static void LittleEndianSerialize(ref MessagePackWriter writer, ref double input, int length)
		{
			ref ulong source = ref Unsafe.As<double, ulong>(ref input);
			checked
			{
				while (length > 0)
				{
					int num = length;
					if (num > 238609294)
					{
						num = 238609294;
					}
					int length2 = num * 9;
					ref byte reference = ref MemoryMarshal.GetReference(writer.GetSpan(length2));
					nuint num2 = 0u;
					nuint num3 = 0u;
					while (num2 < (nuint)num)
					{
						Unsafe.Add(ref reference, num3) = 203;
						Unsafe.WriteUnaligned(ref Unsafe.Add(ref reference, num3 + 1), BinaryPrimitives.ReverseEndianness(Unsafe.Add(ref source, num2)));
						num2++;
						num3 += 9;
					}
					writer.Advance(length2);
					length -= num;
					source = ref Unsafe.Add(ref source, num);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, byte value)
		{
			if (value <= 127)
			{
				destination = value;
				return 1u;
			}
			destination = 204;
			Unsafe.Add(ref destination, 1) = value;
			return 2u;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, sbyte value)
		{
			if (value < -32)
			{
				destination = 208;
				Unsafe.Add(ref destination, 1) = (byte)value;
				return 2u;
			}
			destination = (byte)value;
			return 1u;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, ushort value)
		{
			if (value <= 255)
			{
				return ReverseWriteUnknown(ref destination, (byte)value);
			}
			destination = 205;
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
			return 3u;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, short value)
		{
			if (value < 0)
			{
				if (value >= -128)
				{
					return ReverseWriteUnknown(ref destination, (sbyte)value);
				}
				destination = 209;
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
				return 3u;
			}
			return ReverseWriteUnknown(ref destination, (ushort)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, uint value)
		{
			if (value <= 65535)
			{
				return ReverseWriteUnknown(ref destination, (ushort)value);
			}
			destination = 206;
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
			return 5u;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, int value)
		{
			if (value < 0)
			{
				if (value >= -32768)
				{
					return ReverseWriteUnknown(ref destination, (short)value);
				}
				destination = 210;
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
				return 5u;
			}
			return ReverseWriteUnknown(ref destination, (uint)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, ulong value)
		{
			if (value <= uint.MaxValue)
			{
				return ReverseWriteUnknown(ref destination, (uint)value);
			}
			destination = 207;
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
			return 9u;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static nuint ReverseWriteUnknown(ref byte destination, long value)
		{
			if (value < 0)
			{
				if (value >= int.MinValue)
				{
					return ReverseWriteUnknown(ref destination, (int)value);
				}
				destination = 211;
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 1), BinaryPrimitives.ReverseEndianness(value));
				return 9u;
			}
			return ReverseWriteUnknown(ref destination, (ulong)value);
		}
	}
}
