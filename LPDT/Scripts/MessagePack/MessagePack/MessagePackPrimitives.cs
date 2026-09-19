using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MessagePack.Internal;

namespace MessagePack
{
	public static class MessagePackPrimitives
	{
		public enum DecodeResult
		{
			Success = 0,
			TokenMismatch = 1,
			EmptyBuffer = 2,
			InsufficientBuffer = 3
		}

		private static class Decoders
		{
			internal interface IReadInt64
			{
				DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize);
			}

			internal interface IReadUInt64
			{
				DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize);
			}

			private class ReadInt64Invalid : IReadInt64
			{
				internal static readonly ReadInt64Invalid Instance = new ReadInt64Invalid();

				private ReadInt64Invalid()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					value = 0L;
					tokenSize = 1;
					return DecodeResult.TokenMismatch;
				}
			}

			private class ReadInt64FixInt : IReadInt64
			{
				internal static readonly ReadInt64FixInt Instance = new ReadInt64FixInt();

				private ReadInt64FixInt()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 1;
					value = source[0];
					return DecodeResult.Success;
				}
			}

			private class ReadInt64NegativeFixInt : IReadInt64
			{
				internal static readonly ReadInt64NegativeFixInt Instance = new ReadInt64NegativeFixInt();

				private ReadInt64NegativeFixInt()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 1;
					value = (sbyte)source[0];
					return DecodeResult.Success;
				}
			}

			private class ReadInt64UInt8 : IReadInt64
			{
				internal static readonly ReadInt64UInt8 Instance = new ReadInt64UInt8();

				private ReadInt64UInt8()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 2;
					if (source.Length < tokenSize)
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = source[1];
					return DecodeResult.Success;
				}
			}

			private class ReadInt64UInt16 : IReadInt64
			{
				internal static readonly ReadInt64UInt16 Instance = new ReadInt64UInt16();

				private ReadInt64UInt16()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 3;
					if (!TryReadBigEndian(source.Slice(1), out ushort value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadInt64UInt32 : IReadInt64
			{
				internal static readonly ReadInt64UInt32 Instance = new ReadInt64UInt32();

				private ReadInt64UInt32()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 5;
					if (!TryReadBigEndian(source.Slice(1), out uint value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadInt64UInt64 : IReadInt64
			{
				internal static readonly ReadInt64UInt64 Instance = new ReadInt64UInt64();

				private ReadInt64UInt64()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 9;
					if (!TryReadBigEndian(source.Slice(1), out ulong value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = checked((long)value2);
					return DecodeResult.Success;
				}
			}

			private class ReadInt64Int8 : IReadInt64
			{
				internal static readonly ReadInt64Int8 Instance = new ReadInt64Int8();

				private ReadInt64Int8()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 2;
					if (source.Length < tokenSize)
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = (sbyte)source[1];
					return DecodeResult.Success;
				}
			}

			private class ReadInt64Int16 : IReadInt64
			{
				internal static readonly ReadInt64Int16 Instance = new ReadInt64Int16();

				private ReadInt64Int16()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 3;
					if (!TryReadBigEndian(source.Slice(1), out short value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadInt64Int32 : IReadInt64
			{
				internal static readonly ReadInt64Int32 Instance = new ReadInt64Int32();

				private ReadInt64Int32()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 5;
					if (!TryReadBigEndian(source.Slice(1), out int value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadInt64Int64 : IReadInt64
			{
				internal static readonly ReadInt64Int64 Instance = new ReadInt64Int64();

				private ReadInt64Int64()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out long value, out int tokenSize)
				{
					tokenSize = 9;
					if (!TryReadBigEndian(source.Slice(1), out long value2))
					{
						value = 0L;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64Invalid : IReadUInt64
			{
				internal static readonly ReadUInt64Invalid Instance = new ReadUInt64Invalid();

				private ReadUInt64Invalid()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					value = 0uL;
					tokenSize = 1;
					return DecodeResult.TokenMismatch;
				}
			}

			private class ReadUInt64FixInt : IReadUInt64
			{
				internal static readonly ReadUInt64FixInt Instance = new ReadUInt64FixInt();

				private ReadUInt64FixInt()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 1;
					value = source[0];
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64NegativeFixInt : IReadUInt64
			{
				internal static readonly ReadUInt64NegativeFixInt Instance = new ReadUInt64NegativeFixInt();

				private ReadUInt64NegativeFixInt()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 1;
					checked
					{
						value = (ulong)unchecked((sbyte)source[0]);
						return DecodeResult.Success;
					}
				}
			}

			private class ReadUInt64UInt8 : IReadUInt64
			{
				internal static readonly ReadUInt64UInt8 Instance = new ReadUInt64UInt8();

				private ReadUInt64UInt8()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 2;
					if (source.Length < tokenSize)
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = source[1];
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64UInt16 : IReadUInt64
			{
				internal static readonly ReadUInt64UInt16 Instance = new ReadUInt64UInt16();

				private ReadUInt64UInt16()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 3;
					if (!TryReadBigEndian(source.Slice(1), out ushort value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64UInt32 : IReadUInt64
			{
				internal static readonly ReadUInt64UInt32 Instance = new ReadUInt64UInt32();

				private ReadUInt64UInt32()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 5;
					if (!TryReadBigEndian(source.Slice(1), out uint value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64UInt64 : IReadUInt64
			{
				internal static readonly ReadUInt64UInt64 Instance = new ReadUInt64UInt64();

				private ReadUInt64UInt64()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 9;
					if (!TryReadBigEndian(source.Slice(1), out ulong value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = value2;
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64Int8 : IReadUInt64
			{
				internal static readonly ReadUInt64Int8 Instance = new ReadUInt64Int8();

				private ReadUInt64Int8()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 2;
					if (source.Length < tokenSize)
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					checked
					{
						value = (ulong)unchecked((sbyte)source[1]);
						return DecodeResult.Success;
					}
				}
			}

			private class ReadUInt64Int16 : IReadUInt64
			{
				internal static readonly ReadUInt64Int16 Instance = new ReadUInt64Int16();

				private ReadUInt64Int16()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 3;
					if (!TryReadBigEndian(source.Slice(1), out short value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = checked((ulong)value2);
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64Int32 : IReadUInt64
			{
				internal static readonly ReadUInt64Int32 Instance = new ReadUInt64Int32();

				private ReadUInt64Int32()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 5;
					if (!TryReadBigEndian(source.Slice(1), out int value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = checked((ulong)value2);
					return DecodeResult.Success;
				}
			}

			private class ReadUInt64Int64 : IReadUInt64
			{
				internal static readonly ReadUInt64Int64 Instance = new ReadUInt64Int64();

				private ReadUInt64Int64()
				{
				}

				public DecodeResult Read(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
				{
					tokenSize = 9;
					if (!TryReadBigEndian(source.Slice(1), out long value2))
					{
						value = 0uL;
						return DecodeResult.InsufficientBuffer;
					}
					value = checked((ulong)value2);
					return DecodeResult.Success;
				}
			}

			internal static readonly IReadInt64[] Int64JumpTable;

			internal static readonly IReadUInt64[] UInt64JumpTable;

			static Decoders()
			{
				Int64JumpTable = new IReadInt64[256];
				Int64JumpTable.AsSpan().Fill(ReadInt64Invalid.Instance);
				Int64JumpTable[204] = ReadInt64UInt8.Instance;
				Int64JumpTable[205] = ReadInt64UInt16.Instance;
				Int64JumpTable[206] = ReadInt64UInt32.Instance;
				Int64JumpTable[207] = ReadInt64UInt64.Instance;
				Int64JumpTable[208] = ReadInt64Int8.Instance;
				Int64JumpTable[209] = ReadInt64Int16.Instance;
				Int64JumpTable[210] = ReadInt64Int32.Instance;
				Int64JumpTable[211] = ReadInt64Int64.Instance;
				checked
				{
					for (int i = 224; i <= 255; i++)
					{
						Int64JumpTable[i] = ReadInt64NegativeFixInt.Instance;
					}
					for (int j = 0; j <= 127; j++)
					{
						Int64JumpTable[j] = ReadInt64FixInt.Instance;
					}
					UInt64JumpTable = new IReadUInt64[256];
					UInt64JumpTable.AsSpan().Fill(ReadUInt64Invalid.Instance);
					UInt64JumpTable[204] = ReadUInt64UInt8.Instance;
					UInt64JumpTable[205] = ReadUInt64UInt16.Instance;
					UInt64JumpTable[206] = ReadUInt64UInt32.Instance;
					UInt64JumpTable[207] = ReadUInt64UInt64.Instance;
					UInt64JumpTable[208] = ReadUInt64Int8.Instance;
					UInt64JumpTable[209] = ReadUInt64Int16.Instance;
					UInt64JumpTable[210] = ReadUInt64Int32.Instance;
					UInt64JumpTable[211] = ReadUInt64Int64.Instance;
					for (int k = 224; k <= 255; k++)
					{
						UInt64JumpTable[k] = ReadUInt64NegativeFixInt.Instance;
					}
					for (int l = 0; l <= 127; l++)
					{
						UInt64JumpTable[l] = ReadUInt64FixInt.Instance;
					}
				}
			}
		}

		public static DecodeResult TryReadNil(ReadOnlySpan<byte> source, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length == 0)
			{
				return DecodeResult.EmptyBuffer;
			}
			if (source[0] == 192)
			{
				return DecodeResult.Success;
			}
			return DecodeResult.TokenMismatch;
		}

		public static DecodeResult TryReadArrayHeader(ReadOnlySpan<byte> source, out uint count, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length == 0)
			{
				count = 0u;
				return DecodeResult.EmptyBuffer;
			}
			switch (source[0])
			{
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
				count = checked((byte)(source[0] & 0xF));
				return DecodeResult.Success;
			case 220:
			{
				tokenSize = 3;
				if (TryReadBigEndian(source.Slice(1), out ushort value2))
				{
					count = value2;
					return DecodeResult.Success;
				}
				count = 0u;
				return DecodeResult.InsufficientBuffer;
			}
			case 221:
			{
				tokenSize = 5;
				if (TryReadBigEndian(source.Slice(1), out uint value))
				{
					count = value;
					return DecodeResult.Success;
				}
				count = 0u;
				return DecodeResult.InsufficientBuffer;
			}
			default:
				count = 0u;
				return DecodeResult.TokenMismatch;
			}
		}

		public static DecodeResult TryReadMapHeader(ReadOnlySpan<byte> source, out uint count, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length == 0)
			{
				count = 0u;
				return DecodeResult.EmptyBuffer;
			}
			switch (source[0])
			{
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
				count = checked((byte)(source[0] & 0xF));
				return DecodeResult.Success;
			case 222:
			{
				tokenSize = 3;
				if (TryReadBigEndian(source.Slice(1), out ushort value2))
				{
					count = value2;
					return DecodeResult.Success;
				}
				count = 0u;
				return DecodeResult.InsufficientBuffer;
			}
			case 223:
			{
				tokenSize = 5;
				if (TryReadBigEndian(source.Slice(1), out uint value))
				{
					count = value;
					return DecodeResult.Success;
				}
				count = 0u;
				return DecodeResult.InsufficientBuffer;
			}
			default:
				count = 0u;
				return DecodeResult.TokenMismatch;
			}
		}

		public static DecodeResult TryReadBool(ReadOnlySpan<byte> source, out bool value, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length == 0)
			{
				value = false;
				return DecodeResult.EmptyBuffer;
			}
			switch (source[0])
			{
			case 195:
				value = true;
				return DecodeResult.Success;
			case 194:
				value = false;
				return DecodeResult.Success;
			default:
				value = false;
				return DecodeResult.TokenMismatch;
			}
		}

		public static DecodeResult TryReadChar(ReadOnlySpan<byte> source, out char value, out int tokenSize)
		{
			ushort value2;
			DecodeResult num = TryReadUInt16(source, out value2, out tokenSize);
			if (num == DecodeResult.Success)
			{
				value = (char)value2;
				return num;
			}
			value = '\0';
			return num;
		}

		public static DecodeResult TryReadDateTime(ReadOnlySpan<byte> source, out DateTime value, out int tokenSize)
		{
			DecodeResult decodeResult = TryReadExtensionHeader(source, out var extensionHeader, out tokenSize);
			if (decodeResult != DecodeResult.Success)
			{
				value = default(DateTime);
				return decodeResult;
			}
			decodeResult = TryReadDateTime(source.Slice(tokenSize), extensionHeader, out value, out var tokenSize2);
			checked
			{
				tokenSize += tokenSize2;
				return decodeResult;
			}
		}

		public static DecodeResult TryReadDateTime(ReadOnlySpan<byte> source, ExtensionHeader header, out DateTime value, out int tokenSize)
		{
			tokenSize = checked((int)header.Length);
			if (header.TypeCode != -1)
			{
				value = default(DateTime);
				return DecodeResult.TokenMismatch;
			}
			if (source.Length < tokenSize)
			{
				value = default(DateTime);
				return DecodeResult.InsufficientBuffer;
			}
			uint value2;
			switch (header.Length)
			{
			case 4u:
				AssumesTrue(TryReadBigEndian(source, out value2));
				value = DateTimeConstants.UnixEpoch.AddSeconds(value2);
				return DecodeResult.Success;
			case 8u:
			{
				AssumesTrue(TryReadBigEndian(source, out ulong value4));
				long num = checked((long)(value4 >> 34));
				ulong num2 = value4 & 0x3FFFFFFFFL;
				value = DateTimeConstants.UnixEpoch.AddSeconds(num2).AddTicks(num / 100);
				return DecodeResult.Success;
			}
			case 12u:
			{
				AssumesTrue(TryReadBigEndian(source, out value2));
				long num = value2;
				AssumesTrue(TryReadBigEndian(source.Slice(4), out long value3));
				value = DateTimeConstants.UnixEpoch.AddSeconds(value3).AddTicks(num / 100);
				return DecodeResult.Success;
			}
			default:
				value = default(DateTime);
				return DecodeResult.TokenMismatch;
			}
		}

		public static DecodeResult TryReadExtensionHeader(ReadOnlySpan<byte> source, out ExtensionHeader extensionHeader, out int tokenSize)
		{
			tokenSize = 2;
			if (source.Length < tokenSize)
			{
				extensionHeader = default(ExtensionHeader);
				if (source.Length != 0)
				{
					return DecodeResult.InsufficientBuffer;
				}
				return DecodeResult.EmptyBuffer;
			}
			uint num = 0u;
			switch (source[0])
			{
			case 212:
				num = 1u;
				break;
			case 213:
				num = 2u;
				break;
			case 214:
				num = 4u;
				break;
			case 215:
				num = 8u;
				break;
			case 216:
				num = 16u;
				break;
			case 199:
				tokenSize = 3;
				if (source.Length < tokenSize)
				{
					extensionHeader = default(ExtensionHeader);
					return DecodeResult.InsufficientBuffer;
				}
				num = source[1];
				break;
			case 200:
			{
				tokenSize = 4;
				if (source.Length < tokenSize)
				{
					extensionHeader = default(ExtensionHeader);
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out ushort value2));
				num = value2;
				break;
			}
			case 201:
			{
				tokenSize = 6;
				if (source.Length < tokenSize)
				{
					extensionHeader = default(ExtensionHeader);
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out uint value));
				num = value;
				break;
			}
			default:
				extensionHeader = default(ExtensionHeader);
				return DecodeResult.TokenMismatch;
			}
			sbyte typeCode = (sbyte)source[tokenSize - 1];
			extensionHeader = new ExtensionHeader(typeCode, num);
			return DecodeResult.Success;
		}

		public static DecodeResult TryReadBinHeader(ReadOnlySpan<byte> source, out uint length, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length < tokenSize)
			{
				length = 0u;
				return DecodeResult.EmptyBuffer;
			}
			switch (source[0])
			{
			case 196:
				tokenSize = 2;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				length = source[1];
				return DecodeResult.Success;
			case 197:
			{
				tokenSize = 3;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out ushort value2));
				length = value2;
				return DecodeResult.Success;
			}
			case 198:
			{
				tokenSize = 5;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out uint value));
				length = value;
				return DecodeResult.Success;
			}
			default:
				length = 0u;
				return DecodeResult.TokenMismatch;
			}
		}

		public static DecodeResult TryReadStringHeader(ReadOnlySpan<byte> source, out uint length, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length < tokenSize)
			{
				length = 0u;
				return DecodeResult.EmptyBuffer;
			}
			switch (source[0])
			{
			case 217:
				tokenSize = 2;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				length = source[1];
				return DecodeResult.Success;
			case 218:
			{
				tokenSize = 3;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out ushort value2));
				length = value2;
				return DecodeResult.Success;
			}
			case 219:
			{
				tokenSize = 5;
				if (source.Length < tokenSize)
				{
					length = 0u;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out uint value));
				length = value;
				return DecodeResult.Success;
			}
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
			case 177:
			case 178:
			case 179:
			case 180:
			case 181:
			case 182:
			case 183:
			case 184:
			case 185:
			case 186:
			case 187:
			case 188:
			case 189:
			case 190:
			case 191:
				length = checked((byte)(source[0] & 0x1F));
				return DecodeResult.Success;
			default:
				length = 0u;
				return DecodeResult.TokenMismatch;
			}
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out ushort value)
		{
			if (source.Length < 2)
			{
				value = 0;
				return false;
			}
			value = Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(source));
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return true;
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out short value)
		{
			if (TryReadBigEndian(source, out ushort value2))
			{
				value = (short)value2;
				return true;
			}
			value = 0;
			return false;
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out uint value)
		{
			if (source.Length < 4)
			{
				value = 0u;
				return false;
			}
			value = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(source));
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return true;
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out int value)
		{
			if (TryReadBigEndian(source, out uint value2))
			{
				value = (int)value2;
				return true;
			}
			value = 0;
			return false;
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out ulong value)
		{
			if (source.Length < 8)
			{
				value = 0uL;
				return false;
			}
			value = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(source));
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return true;
		}

		private static bool TryReadBigEndian(ReadOnlySpan<byte> source, out long value)
		{
			if (TryReadBigEndian(source, out ulong value2))
			{
				value = (long)value2;
				return true;
			}
			value = 0L;
			return false;
		}

		private static void AssumesTrue([DoesNotReturnIf(false)] bool condition)
		{
			if (!condition)
			{
				throw new Exception("Internal error.");
			}
		}

		public static DecodeResult TryReadByte(ReadOnlySpan<byte> source, out byte value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				ulong value2;
				DecodeResult result = Decoders.UInt64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((byte)value2);
				return result;
			}
			tokenSize = 1;
			value = 0;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadUInt16(ReadOnlySpan<byte> source, out ushort value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				ulong value2;
				DecodeResult result = Decoders.UInt64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((ushort)value2);
				return result;
			}
			tokenSize = 1;
			value = 0;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadUInt32(ReadOnlySpan<byte> source, out uint value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				ulong value2;
				DecodeResult result = Decoders.UInt64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((uint)value2);
				return result;
			}
			tokenSize = 1;
			value = 0u;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadUInt64(ReadOnlySpan<byte> source, out ulong value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				ulong value2;
				DecodeResult result = Decoders.UInt64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = value2;
				return result;
			}
			tokenSize = 1;
			value = 0uL;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadSByte(ReadOnlySpan<byte> source, out sbyte value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				long value2;
				DecodeResult result = Decoders.Int64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((sbyte)value2);
				return result;
			}
			tokenSize = 1;
			value = 0;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadInt16(ReadOnlySpan<byte> source, out short value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				long value2;
				DecodeResult result = Decoders.Int64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((short)value2);
				return result;
			}
			tokenSize = 1;
			value = 0;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadInt32(ReadOnlySpan<byte> source, out int value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				long value2;
				DecodeResult result = Decoders.Int64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = checked((int)value2);
				return result;
			}
			tokenSize = 1;
			value = 0;
			return DecodeResult.EmptyBuffer;
		}

		public static DecodeResult TryReadInt64(ReadOnlySpan<byte> source, out long value, out int tokenSize)
		{
			if (source.Length > 0)
			{
				long value2;
				DecodeResult result = Decoders.Int64JumpTable[source[0]].Read(source, out value2, out tokenSize);
				value = value2;
				return result;
			}
			tokenSize = 1;
			value = 0L;
			return DecodeResult.EmptyBuffer;
		}

		public unsafe static DecodeResult TryReadSingle(ReadOnlySpan<byte> source, out float value, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length < 1)
			{
				value = 0f;
				return DecodeResult.EmptyBuffer;
			}
			byte b = source[0];
			if (b >= 224)
			{
				goto IL_00d4;
			}
			if (b > 127)
			{
				switch (b)
				{
				case 202:
					break;
				case 203:
					goto IL_009f;
				case 208:
				case 209:
				case 210:
				case 211:
					goto IL_00d4;
				case 204:
				case 205:
				case 206:
				case 207:
					goto IL_00e2;
				default:
					value = 0f;
					return DecodeResult.TokenMismatch;
				}
				tokenSize = 5;
				if (source.Length < tokenSize)
				{
					value = 0f;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out uint value2));
				value = *(float*)(&value2);
				return DecodeResult.Success;
			}
			goto IL_00e2;
			IL_00e2:
			ulong value3;
			DecodeResult result = TryReadUInt64(source, out value3, out tokenSize);
			value = value3;
			return result;
			IL_00d4:
			long value4;
			DecodeResult result2 = TryReadInt64(source, out value4, out tokenSize);
			value = value4;
			return result2;
			IL_009f:
			tokenSize = 9;
			if (source.Length < tokenSize)
			{
				value = 0f;
				return DecodeResult.InsufficientBuffer;
			}
			AssumesTrue(TryReadBigEndian(source.Slice(1), out value3));
			value = (float)(*(double*)(&value3));
			return DecodeResult.Success;
		}

		public unsafe static DecodeResult TryReadDouble(ReadOnlySpan<byte> source, out double value, out int tokenSize)
		{
			tokenSize = 1;
			if (source.Length < 1)
			{
				value = 0.0;
				return DecodeResult.EmptyBuffer;
			}
			byte b = source[0];
			if (b >= 224)
			{
				goto IL_00e1;
			}
			if (b > 127)
			{
				switch (b)
				{
				case 202:
					break;
				case 203:
					goto IL_00a8;
				case 208:
				case 209:
				case 210:
				case 211:
					goto IL_00e1;
				case 204:
				case 205:
				case 206:
				case 207:
					goto IL_00ef;
				default:
					value = 0.0;
					return DecodeResult.TokenMismatch;
				}
				tokenSize = 5;
				if (source.Length < tokenSize)
				{
					value = 0.0;
					return DecodeResult.InsufficientBuffer;
				}
				AssumesTrue(TryReadBigEndian(source.Slice(1), out uint value2));
				value = *(float*)(&value2);
				return DecodeResult.Success;
			}
			goto IL_00ef;
			IL_00ef:
			ulong value3;
			DecodeResult result = TryReadUInt64(source, out value3, out tokenSize);
			value = value3;
			return result;
			IL_00e1:
			long value4;
			DecodeResult result2 = TryReadInt64(source, out value4, out tokenSize);
			value = value4;
			return result2;
			IL_00a8:
			tokenSize = 9;
			if (source.Length < tokenSize)
			{
				value = 0.0;
				return DecodeResult.InsufficientBuffer;
			}
			AssumesTrue(TryReadBigEndian(source.Slice(1), out value3));
			value = *(double*)(&value3);
			return DecodeResult.Success;
		}

		public static bool TryWriteNil(Span<byte> destination, out int bytesWritten)
		{
			bytesWritten = 1;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 192;
			return true;
		}

		public static bool TryWriteArrayHeader(Span<byte> destination, uint count, out int bytesWritten)
		{
			checked
			{
				if (count > 15)
				{
					if (count <= 65535)
					{
						bytesWritten = 3;
						if (destination.Length < bytesWritten)
						{
							return false;
						}
						destination[0] = 220;
						WriteBigEndian(destination.Slice(1), (ushort)count);
						return true;
					}
					bytesWritten = 5;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = 221;
					WriteBigEndian(destination.Slice(1), count);
					return true;
				}
				bytesWritten = 1;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = (byte)(0x90 | count);
				return true;
			}
		}

		public static bool TryWriteMapHeader(Span<byte> destination, uint count, out int bytesWritten)
		{
			checked
			{
				if (count > 15)
				{
					if (count <= 65535)
					{
						bytesWritten = 3;
						if (destination.Length < bytesWritten)
						{
							return false;
						}
						destination[0] = 222;
						WriteBigEndian(destination.Slice(1), (ushort)count);
						return true;
					}
					bytesWritten = 5;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = 223;
					WriteBigEndian(destination.Slice(1), count);
					return true;
				}
				bytesWritten = 1;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = (byte)(0x80 | count);
				return true;
			}
		}

		public static bool TryWrite(Span<byte> destination, sbyte value, out int bytesWritten)
		{
			if (value >= 0)
			{
				return TryWrite(destination, (byte)value, out bytesWritten);
			}
			if (value >= -32)
			{
				return TryWriteNegativeFixIntUnsafe(destination, value, out bytesWritten);
			}
			return TryWriteInt8(destination, value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, short value, out int bytesWritten)
		{
			if (value >= 0)
			{
				return TryWrite(destination, (ushort)value, out bytesWritten);
			}
			if (value < -32)
			{
				if (value >= -128)
				{
					return TryWriteInt8(destination, (sbyte)value, out bytesWritten);
				}
				return TryWriteInt16(destination, value, out bytesWritten);
			}
			return TryWriteNegativeFixIntUnsafe(destination, (sbyte)value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, int value, out int bytesWritten)
		{
			if (value >= 0)
			{
				return TryWrite(destination, (uint)value, out bytesWritten);
			}
			if (value >= -128)
			{
				if (value >= -32)
				{
					return TryWriteNegativeFixIntUnsafe(destination, (sbyte)value, out bytesWritten);
				}
				return TryWriteInt8(destination, (sbyte)value, out bytesWritten);
			}
			if (value >= -32768)
			{
				return TryWriteInt16(destination, (short)value, out bytesWritten);
			}
			return TryWriteInt32(destination, value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, long value, out int bytesWritten)
		{
			if (value >= 0)
			{
				return TryWrite(destination, (ulong)value, out bytesWritten);
			}
			return SlowPath(destination, value, out bytesWritten);
			static bool SlowPath(Span<byte> destination2, long num, out int bytesWritten2)
			{
				checked
				{
					if (num >= -32768)
					{
						if (num >= -32)
						{
							if (num >= 0)
							{
								return TryWrite(destination2, (ulong)num, out bytesWritten2);
							}
							return TryWriteNegativeFixIntUnsafe(destination2, unchecked((sbyte)num), out bytesWritten2);
						}
						if (num >= -128)
						{
							return TryWriteInt8(destination2, (sbyte)num, out bytesWritten2);
						}
						return TryWriteInt16(destination2, (short)num, out bytesWritten2);
					}
					if (num >= int.MinValue)
					{
						return TryWriteInt32(destination2, (int)num, out bytesWritten2);
					}
					return TryWriteInt64(destination2, num, out bytesWritten2);
				}
			}
		}

		public static bool TryWriteInt8(Span<byte> destination, sbyte value, out int bytesWritten)
		{
			bytesWritten = 2;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 208;
			destination[1] = (byte)value;
			return true;
		}

		public static bool TryWriteInt16(Span<byte> destination, short value, out int bytesWritten)
		{
			bytesWritten = 3;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 209;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public static bool TryWriteInt32(Span<byte> destination, int value, out int bytesWritten)
		{
			bytesWritten = 5;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 210;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public static bool TryWriteInt64(Span<byte> destination, long value, out int bytesWritten)
		{
			bytesWritten = 9;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 211;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public static bool TryWrite(Span<byte> destination, byte value, out int bytesWritten)
		{
			if (value <= 127)
			{
				return TryWriteFixIntUnsafe(destination, value, out bytesWritten);
			}
			return TryWriteUInt8(destination, value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, ushort value, out int bytesWritten)
		{
			if (value > 127)
			{
				if (value <= 255)
				{
					return TryWriteUInt8(destination, (byte)value, out bytesWritten);
				}
				return TryWriteUInt16(destination, value, out bytesWritten);
			}
			return TryWriteFixIntUnsafe(destination, (byte)value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, uint value, out int bytesWritten)
		{
			if (value <= 255)
			{
				if (value <= 127)
				{
					return TryWriteFixIntUnsafe(destination, (byte)value, out bytesWritten);
				}
				return TryWriteUInt8(destination, (byte)value, out bytesWritten);
			}
			if (value <= 65535)
			{
				return TryWriteUInt16(destination, (ushort)value, out bytesWritten);
			}
			return TryWriteUInt32(destination, value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, ulong value, out int bytesWritten)
		{
			if (value <= 127)
			{
				return TryWriteFixIntUnsafe(destination, (byte)value, out bytesWritten);
			}
			return SlowPath(destination, value, out bytesWritten);
			static bool SlowPath(Span<byte> destination2, ulong num, out int bytesWritten2)
			{
				if (num <= 65535)
				{
					if (num <= 255)
					{
						return TryWriteUInt8(destination2, (byte)num, out bytesWritten2);
					}
					return TryWriteUInt16(destination2, (ushort)num, out bytesWritten2);
				}
				if (num <= uint.MaxValue)
				{
					return TryWriteUInt32(destination2, (uint)num, out bytesWritten2);
				}
				return TryWriteUInt64(destination2, num, out bytesWritten2);
			}
		}

		public static bool TryWriteUInt8(Span<byte> destination, byte value, out int bytesWritten)
		{
			bytesWritten = 2;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 204;
			destination[1] = value;
			return true;
		}

		public static bool TryWriteUInt16(Span<byte> destination, ushort value, out int bytesWritten)
		{
			bytesWritten = 3;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 205;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public static bool TryWriteUInt32(Span<byte> destination, uint value, out int bytesWritten)
		{
			bytesWritten = 5;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 206;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public static bool TryWriteUInt64(Span<byte> destination, ulong value, out int bytesWritten)
		{
			bytesWritten = 9;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 207;
			WriteBigEndian(destination.Slice(1), value);
			return true;
		}

		public unsafe static bool TryWrite(Span<byte> destination, float value, out int bytesWritten)
		{
			bytesWritten = 5;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 202;
			WriteBigEndian(destination.Slice(1), *(int*)(&value));
			return true;
		}

		public unsafe static bool TryWrite(Span<byte> destination, double value, out int bytesWritten)
		{
			bytesWritten = 9;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 203;
			WriteBigEndian(destination.Slice(1), *(long*)(&value));
			return true;
		}

		public static bool TryWrite(Span<byte> destination, bool value, out int bytesWritten)
		{
			bytesWritten = 1;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = (byte)(value ? 195 : 194);
			return true;
		}

		public static bool TryWrite(Span<byte> destination, char value, out int bytesWritten)
		{
			return TryWrite(destination, (ushort)value, out bytesWritten);
		}

		public static bool TryWrite(Span<byte> destination, DateTime value, out int bytesWritten)
		{
			if (value.Kind == DateTimeKind.Local)
			{
				value = value.ToUniversalTime();
			}
			checked
			{
				long num = unchecked(value.Ticks / 10000000) - 62135596800L;
				long num2 = unchecked(value.Ticks % 10000000) * 100;
				if (num >> 34 == 0L)
				{
					ulong num3 = unchecked((ulong)((num2 << 34) | num));
					if ((num3 & 0xFFFFFFFF00000000uL) == 0L)
					{
						bytesWritten = 6;
						if (destination.Length < bytesWritten)
						{
							return false;
						}
						uint value2 = (uint)num3;
						destination[0] = 214;
						destination[1] = byte.MaxValue;
						WriteBigEndian(destination.Slice(2), value2);
					}
					else
					{
						bytesWritten = 10;
						if (destination.Length < bytesWritten)
						{
							return false;
						}
						destination[0] = 215;
						destination[1] = byte.MaxValue;
						WriteBigEndian(destination.Slice(2), num3);
					}
				}
				else
				{
					bytesWritten = 15;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = 199;
					destination[1] = 12;
					destination[2] = byte.MaxValue;
					WriteBigEndian(destination.Slice(3), (uint)num2);
					WriteBigEndian(destination.Slice(7), num);
				}
				return true;
			}
		}

		public static bool TryWriteBinHeader(Span<byte> destination, uint length, out int bytesWritten)
		{
			checked
			{
				if (length > 255)
				{
					if (length <= 65535)
					{
						bytesWritten = 3;
						if (destination.Length < bytesWritten)
						{
							return false;
						}
						destination[0] = 197;
						WriteBigEndian(destination.Slice(1), (ushort)length);
						return true;
					}
					bytesWritten = 5;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = 198;
					WriteBigEndian(destination.Slice(1), length);
					return true;
				}
				bytesWritten = 2;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = 196;
				destination[1] = (byte)length;
				return true;
			}
		}

		public static bool TryWriteStringHeader(Span<byte> destination, uint byteCount, out int bytesWritten)
		{
			if (byteCount <= 255)
			{
				if (byteCount <= 31)
				{
					bytesWritten = 1;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = checked((byte)(0xA0 | byteCount));
					return true;
				}
				bytesWritten = 2;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = 217;
				destination[1] = (byte)byteCount;
				return true;
			}
			if (byteCount <= 65535)
			{
				bytesWritten = 3;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = 218;
				WriteBigEndian(destination.Slice(1), checked((ushort)byteCount));
				return true;
			}
			bytesWritten = 5;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 219;
			WriteBigEndian(destination.Slice(1), byteCount);
			return true;
		}

		public static bool TryWriteExtensionFormatHeader(Span<byte> destination, ExtensionHeader extensionHeader, out int bytesWritten)
		{
			int num = checked((int)extensionHeader.Length);
			byte b = (byte)extensionHeader.TypeCode;
			if (num <= 255)
			{
				switch (num)
				{
				case 1:
				case 2:
				case 4:
				case 8:
				case 16:
				{
					bytesWritten = 2;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					byte b2 = num switch
					{
						1 => 212, 
						2 => 213, 
						4 => 214, 
						8 => 215, 
						16 => 216, 
						_ => throw ThrowUnreachable(), 
					};
					destination[0] = b2;
					destination[1] = b;
					return true;
				}
				default:
					bytesWritten = 3;
					if (destination.Length < bytesWritten)
					{
						return false;
					}
					destination[0] = 199;
					destination[1] = (byte)num;
					destination[2] = b;
					return true;
				}
			}
			if (num <= 65535)
			{
				bytesWritten = 4;
				if (destination.Length < bytesWritten)
				{
					return false;
				}
				destination[0] = 200;
				WriteBigEndian(destination.Slice(1), checked((ushort)num));
				destination[3] = b;
				return true;
			}
			bytesWritten = 6;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = 201;
			WriteBigEndian(destination.Slice(1), num);
			destination[5] = b;
			return true;
		}

		private static bool TryWriteFixIntUnsafe(Span<byte> destination, byte value, out int bytesWritten)
		{
			bytesWritten = 1;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = value;
			return true;
		}

		private static bool TryWriteNegativeFixIntUnsafe(Span<byte> destination, sbyte value, out int bytesWritten)
		{
			bytesWritten = 1;
			if (destination.Length < bytesWritten)
			{
				return false;
			}
			destination[0] = (byte)value;
			return true;
		}

		[DoesNotReturn]
		private static Exception ThrowUnreachable()
		{
			throw new Exception("Presumed unreachable point in code reached.");
		}

		private static void WriteBigEndian(Span<byte> destination, ushort value)
		{
			destination[1] = (byte)value;
			destination[0] = (byte)(value >> 8);
		}

		private static void WriteBigEndian(Span<byte> destination, uint value)
		{
			destination[3] = (byte)value;
			destination[2] = (byte)(value >> 8);
			destination[1] = (byte)(value >> 16);
			destination[0] = (byte)(value >> 24);
		}

		private static void WriteBigEndian(Span<byte> destination, ulong value)
		{
			destination[7] = (byte)value;
			destination[6] = (byte)(value >> 8);
			destination[5] = (byte)(value >> 16);
			destination[4] = (byte)(value >> 24);
			destination[3] = (byte)(value >> 32);
			destination[2] = (byte)(value >> 40);
			destination[1] = (byte)(value >> 48);
			destination[0] = (byte)(value >> 56);
		}

		private static void WriteBigEndian(Span<byte> destination, short value)
		{
			WriteBigEndian(destination, (ushort)value);
		}

		private static void WriteBigEndian(Span<byte> destination, int value)
		{
			WriteBigEndian(destination, (uint)value);
		}

		private static void WriteBigEndian(Span<byte> destination, long value)
		{
			WriteBigEndian(destination, (ulong)value);
		}
	}
}
