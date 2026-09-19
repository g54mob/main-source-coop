using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MessagePack
{
	public ref struct MessagePackReader
	{
		private SequenceReader<byte> reader;

		public CancellationToken CancellationToken { get; set; }

		public int Depth { get; set; }

		public ReadOnlySequence<byte> Sequence => reader.Sequence;

		public SequencePosition Position => reader.Position;

		public long Consumed => reader.Consumed;

		public bool End => reader.End;

		public bool IsNil => NextCode == 192;

		public MessagePackType NextMessagePackType => MessagePackCode.ToMessagePackType(NextCode);

		public byte NextCode
		{
			get
			{
				ThrowInsufficientBufferUnless(reader.TryPeek(out var value));
				return value;
			}
		}

		public MessagePackReader(ReadOnlyMemory<byte> memory)
		{
			this = default(MessagePackReader);
			reader = new SequenceReader<byte>(memory);
			Depth = 0;
		}

		public MessagePackReader([System.Runtime.CompilerServices.ScopedRef] in ReadOnlySequence<byte> readOnlySequence)
		{
			this = default(MessagePackReader);
			reader = new SequenceReader<byte>(in readOnlySequence);
			Depth = 0;
		}

		public MessagePackReader Clone([System.Runtime.CompilerServices.ScopedRef] in ReadOnlySequence<byte> readOnlySequence)
		{
			MessagePackReader result = new MessagePackReader(in readOnlySequence);
			result.CancellationToken = CancellationToken;
			result.Depth = Depth;
			return result;
		}

		public MessagePackReader CreatePeekReader()
		{
			return this;
		}

		public void Skip()
		{
			ThrowInsufficientBufferUnless(TrySkip());
		}

		internal bool TrySkip()
		{
			if (reader.Remaining == 0L)
			{
				return false;
			}
			byte nextCode = NextCode;
			byte b = nextCode;
			if (!MessagePackCode.IsPositiveFixInt(b) && !MessagePackCode.IsNegativeFixInt(b))
			{
				switch (b)
				{
				case 192:
				case 194:
				case 195:
					break;
				case 204:
				case 208:
					return reader.TryAdvance(2L);
				case 205:
				case 209:
					return reader.TryAdvance(3L);
				case 202:
				case 206:
				case 210:
					return reader.TryAdvance(5L);
				case 203:
				case 207:
				case 211:
					return reader.TryAdvance(9L);
				default:
				{
					if (MessagePackCode.IsFixMap(b) || (uint)(b - 222) <= 1u)
					{
						return TrySkipNextMap();
					}
					if (MessagePackCode.IsFixArray(b) || (uint)(b - 220) <= 1u)
					{
						return TrySkipNextArray();
					}
					uint length;
					if (!MessagePackCode.IsFixStr(b))
					{
						switch (b)
						{
						case 217:
						case 218:
						case 219:
							break;
						case 196:
						case 197:
						case 198:
							if (TryGetBytesLength(out length))
							{
								return reader.TryAdvance(length);
							}
							return false;
						case 199:
						case 200:
						case 201:
						case 212:
						case 213:
						case 214:
						case 215:
						case 216:
						{
							if (TryReadExtensionFormatHeader(out var extensionHeader))
							{
								return reader.TryAdvance(extensionHeader.Length);
							}
							return false;
						}
						default:
							throw ThrowInvalidCode(nextCode);
						}
					}
					if (TryGetStringLengthInBytes(out length))
					{
						return reader.TryAdvance(length);
					}
					return false;
				}
				}
			}
			return reader.TryAdvance(1L);
		}

		public Nil ReadNil()
		{
			ThrowInsufficientBufferUnless(reader.TryRead(out var value));
			if (value != 192)
			{
				throw ThrowInvalidCode(value);
			}
			return Nil.Default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryReadNil()
		{
			if (NextCode == 192)
			{
				reader.Advance(1L);
				return true;
			}
			return false;
		}

		public ReadOnlySequence<byte> ReadRaw(long length)
		{
			try
			{
				ReadOnlySequence<byte> result = reader.Sequence.Slice(reader.Position, length);
				reader.Advance(length);
				return result;
			}
			catch (ArgumentOutOfRangeException innerException)
			{
				throw ThrowNotEnoughBytesException(innerException);
			}
		}

		public ReadOnlySequence<byte> ReadRaw()
		{
			SequencePosition position = Position;
			Skip();
			return Sequence.Slice(position, Position);
		}

		public int ReadArrayHeader()
		{
			ThrowInsufficientBufferUnless(TryReadArrayHeader(out var count));
			ThrowInsufficientBufferUnless(reader.Remaining >= count);
			return count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryReadArrayHeader(out int count)
		{
			uint count2;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadArrayHeader(reader.UnreadSpan, out count2, out tokenSize);
			count = checked((int)count2);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return true;
			}
			return SlowPath(ref this, decodeResult, ref count, ref tokenSize);
			static bool SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ref int reference2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return true;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadArrayHeader(span, out var count3, out reference);
						reference2 = checked((int)count3);
						return SlowPath(ref self, readResult, ref reference2, ref reference);
					}
					reference2 = 0;
					return false;
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public int ReadMapHeader()
		{
			ThrowInsufficientBufferUnless(TryReadMapHeader(out var count));
			ThrowInsufficientBufferUnless(reader.Remaining >= checked(count * 2));
			return count;
		}

		public bool TryReadMapHeader(out int count)
		{
			uint count2;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadMapHeader(reader.UnreadSpan, out count2, out tokenSize);
			count = checked((int)count2);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return true;
			}
			return SlowPath(ref this, decodeResult, ref count, ref tokenSize);
			static bool SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ref int reference2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return true;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadMapHeader(span, out var count3, out reference);
						reference2 = checked((int)count3);
						return SlowPath(ref self, readResult, ref reference2, ref reference);
					}
					reference2 = 0;
					return false;
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public bool ReadBoolean()
		{
			ThrowInsufficientBufferUnless(reader.TryRead(out var value));
			return value switch
			{
				195 => true, 
				194 => false, 
				_ => throw ThrowInvalidCode(value), 
			};
		}

		public char ReadChar()
		{
			return (char)ReadUInt16();
		}

		public float ReadSingle()
		{
			float value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadSingle(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static float SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, float value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadSingle(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public double ReadDouble()
		{
			double value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadDouble(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static double SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, double value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadDouble(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public DateTime ReadDateTime()
		{
			DateTime value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadDateTime(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static DateTime SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, DateTime value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadDateTime(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public DateTime ReadDateTime(ExtensionHeader header)
		{
			DateTime value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadDateTime(reader.UnreadSpan, header, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, header, decodeResult, value, ref tokenSize);
			static DateTime SlowPath(ref MessagePackReader self, ExtensionHeader header2, MessagePackPrimitives.DecodeResult readResult, DateTime value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadDateTime(span, header2, out value2, out reference);
						return SlowPath(ref self, header2, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public ReadOnlySequence<byte>? ReadBytes()
		{
			if (TryReadNil())
			{
				return null;
			}
			uint bytesLength = GetBytesLength();
			ThrowInsufficientBufferUnless(reader.Remaining >= bytesLength);
			ReadOnlySequence<byte> value = reader.Sequence.Slice(reader.Position, bytesLength);
			reader.Advance(bytesLength);
			return value;
		}

		public ReadOnlySequence<byte>? ReadStringSequence()
		{
			if (TryReadNil())
			{
				return null;
			}
			uint stringLengthInBytes = GetStringLengthInBytes();
			ThrowInsufficientBufferUnless(reader.Remaining >= stringLengthInBytes);
			ReadOnlySequence<byte> value = reader.Sequence.Slice(reader.Position, stringLengthInBytes);
			reader.Advance(stringLengthInBytes);
			return value;
		}

		public bool TryReadStringSpan(out ReadOnlySpan<byte> span)
		{
			if (IsNil)
			{
				span = default(ReadOnlySpan<byte>);
				return false;
			}
			long consumed = reader.Consumed;
			checked
			{
				int num = (int)GetStringLengthInBytes();
				ThrowInsufficientBufferUnless(reader.Remaining >= num);
				int num2 = reader.CurrentSpanIndex + num;
				ReadOnlySpan<byte> currentSpan = reader.CurrentSpan;
				if (num2 <= currentSpan.Length)
				{
					currentSpan = reader.CurrentSpan;
					span = currentSpan.Slice(reader.CurrentSpanIndex, num);
					reader.Advance(num);
					return true;
				}
				reader.Rewind(reader.Consumed - consumed);
				span = default(ReadOnlySpan<byte>);
				return false;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? ReadString()
		{
			if (TryReadNil())
			{
				return null;
			}
			uint stringLengthInBytes = GetStringLengthInBytes();
			ReadOnlySpan<byte> unreadSpan = reader.UnreadSpan;
			if (unreadSpan.Length >= stringLengthInBytes)
			{
				string result = StringEncoding.UTF8.GetString(unreadSpan.Slice(0, checked((int)stringLengthInBytes)));
				reader.Advance(stringLengthInBytes);
				return result;
			}
			return ReadStringSlow(stringLengthInBytes);
		}

		public ExtensionHeader ReadExtensionFormatHeader()
		{
			ThrowInsufficientBufferUnless(TryReadExtensionFormatHeader(out var extensionHeader));
			ThrowInsufficientBufferUnless(reader.Remaining >= extensionHeader.Length);
			return extensionHeader;
		}

		public bool TryReadExtensionFormatHeader(out ExtensionHeader extensionHeader)
		{
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadExtensionHeader(reader.UnreadSpan, out extensionHeader, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return true;
			}
			return SlowPath(ref this, decodeResult, ref extensionHeader, ref tokenSize);
			static bool SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ref ExtensionHeader reference2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return true;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadExtensionHeader(span, out reference2, out reference);
						return SlowPath(ref self, readResult, ref reference2, ref reference);
					}
					reference2 = default(ExtensionHeader);
					return false;
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public ExtensionResult ReadExtensionFormat()
		{
			ExtensionHeader extensionHeader = ReadExtensionFormatHeader();
			try
			{
				ReadOnlySequence<byte> data = reader.Sequence.Slice(reader.Position, extensionHeader.Length);
				reader.Advance(extensionHeader.Length);
				return new ExtensionResult(extensionHeader.TypeCode, data);
			}
			catch (ArgumentOutOfRangeException innerException)
			{
				throw ThrowNotEnoughBytesException(innerException);
			}
		}

		private static EndOfStreamException ThrowNotEnoughBytesException()
		{
			throw new EndOfStreamException();
		}

		private static EndOfStreamException ThrowNotEnoughBytesException(Exception innerException)
		{
			throw new EndOfStreamException(new EndOfStreamException().Message, innerException);
		}

		[DoesNotReturn]
		private static Exception ThrowInvalidCode(byte code)
		{
			throw new MessagePackSerializationException($"Unexpected msgpack code {code} ({MessagePackCode.ToFormatName(code)}) encountered.");
		}

		private static void ThrowInsufficientBufferUnless(bool condition)
		{
			if (!condition)
			{
				ThrowNotEnoughBytesException();
			}
		}

		[DoesNotReturn]
		private static Exception ThrowUnreachable()
		{
			throw new Exception("Presumed unreachable point in code reached.");
		}

		private uint GetBytesLength()
		{
			ThrowInsufficientBufferUnless(TryGetBytesLength(out var length));
			return length;
		}

		private bool TryGetBytesLength(out uint length)
		{
			bool usingBinaryHeader = true;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadBinHeader(reader.UnreadSpan, out length, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return true;
			}
			return SlowPath(ref this, decodeResult, usingBinaryHeader, ref length, ref tokenSize);
			static bool SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, bool flag, ref uint reference2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return true;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					if (flag)
					{
						flag = false;
						readResult = MessagePackPrimitives.TryReadStringHeader(self.reader.UnreadSpan, out reference2, out reference);
						return SlowPath(ref self, readResult, flag, ref reference2, ref reference);
					}
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = (flag ? MessagePackPrimitives.TryReadBinHeader(span, out reference2, out reference) : MessagePackPrimitives.TryReadStringHeader(span, out reference2, out reference));
						return SlowPath(ref self, readResult, flag, ref reference2, ref reference);
					}
					reference2 = 0u;
					return false;
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryGetStringLengthInBytes(out uint length)
		{
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadStringHeader(reader.UnreadSpan, out length, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return true;
			}
			return SlowPath(ref this, decodeResult, ref length, ref tokenSize);
			static bool SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ref uint reference2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return true;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadStringHeader(span, out reference2, out reference);
						return SlowPath(ref self, readResult, ref reference2, ref reference);
					}
					reference2 = 0u;
					return false;
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		private uint GetStringLengthInBytes()
		{
			ThrowInsufficientBufferUnless(TryGetStringLengthInBytes(out var length));
			return length;
		}

		private unsafe string ReadStringSlow(uint byteLength)
		{
			ThrowInsufficientBufferUnless(reader.Remaining >= byteLength);
			checked
			{
				int num = (int)byteLength;
				int maxCharCount = StringEncoding.UTF8.GetMaxCharCount(num);
				char[] array = ArrayPool<char>.Shared.Rent(maxCharCount);
				Decoder decoder = StringEncoding.UTF8.GetDecoder();
				int num2 = 0;
				while (num > 0)
				{
					int val = num;
					ReadOnlySpan<byte> unreadSpan = reader.UnreadSpan;
					int num3 = Math.Min(val, unreadSpan.Length);
					num -= num3;
					bool flush = num == 0;
					unreadSpan = reader.UnreadSpan;
					fixed (byte* bytes = unreadSpan)
					{
						fixed (char* chars = &array[num2])
						{
							num2 += decoder.GetChars(bytes, num3, chars, array.Length - num2, flush);
						}
					}
					reader.Advance(num3);
				}
				string result = new string(array, 0, num2);
				ArrayPool<char>.Shared.Return(array);
				return result;
			}
		}

		private bool TrySkipNextArray()
		{
			if (TryReadArrayHeader(out var count))
			{
				return TrySkip(count);
			}
			return false;
		}

		private bool TrySkipNextMap()
		{
			if (TryReadMapHeader(out var count))
			{
				return TrySkip(checked(count * 2));
			}
			return false;
		}

		private bool TrySkip(int count)
		{
			for (int i = 0; i < count; i = checked(i + 1))
			{
				if (!TrySkip())
				{
					return false;
				}
			}
			return true;
		}

		public byte ReadByte()
		{
			byte value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadByte(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static byte SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, byte value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadByte(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public ushort ReadUInt16()
		{
			ushort value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadUInt16(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static ushort SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ushort value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadUInt16(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public uint ReadUInt32()
		{
			uint value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadUInt32(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static uint SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, uint value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadUInt32(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public ulong ReadUInt64()
		{
			ulong value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadUInt64(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static ulong SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, ulong value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadUInt64(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public sbyte ReadSByte()
		{
			sbyte value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadSByte(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static sbyte SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, sbyte value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadSByte(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public short ReadInt16()
		{
			short value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadInt16(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static short SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, short value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadInt16(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public int ReadInt32()
		{
			int value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadInt32(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static int SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, int value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadInt32(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}

		public long ReadInt64()
		{
			long value;
			int tokenSize;
			MessagePackPrimitives.DecodeResult decodeResult = MessagePackPrimitives.TryReadInt64(reader.UnreadSpan, out value, out tokenSize);
			if (decodeResult == MessagePackPrimitives.DecodeResult.Success)
			{
				reader.Advance(tokenSize);
				return value;
			}
			return SlowPath(ref this, decodeResult, value, ref tokenSize);
			static long SlowPath(ref MessagePackReader self, MessagePackPrimitives.DecodeResult readResult, long value2, ref int reference)
			{
				switch (readResult)
				{
				case MessagePackPrimitives.DecodeResult.Success:
					self.reader.Advance(reference);
					return value2;
				case MessagePackPrimitives.DecodeResult.TokenMismatch:
					throw ThrowInvalidCode(self.reader.UnreadSpan[0]);
				case MessagePackPrimitives.DecodeResult.EmptyBuffer:
				case MessagePackPrimitives.DecodeResult.InsufficientBuffer:
				{
					Span<byte> span = stackalloc byte[reference];
					if (self.reader.TryCopyTo(span))
					{
						readResult = MessagePackPrimitives.TryReadInt64(span, out value2, out reference);
						return SlowPath(ref self, readResult, value2, ref reference);
					}
					throw ThrowNotEnoughBytesException();
				}
				default:
					throw ThrowUnreachable();
				}
			}
		}
	}
}
