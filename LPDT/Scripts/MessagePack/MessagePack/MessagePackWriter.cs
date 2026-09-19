using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MessagePack
{
	public ref struct MessagePackWriter
	{
		private BufferWriter writer;

		public CancellationToken CancellationToken { get; set; }

		public bool OldSpec { get; set; }

		public MessagePackWriter(IBufferWriter<byte> writer)
		{
			this = default(MessagePackWriter);
			this.writer = new BufferWriter(writer);
			OldSpec = false;
		}

		internal MessagePackWriter(SequencePool sequencePool, byte[] array)
		{
			this = default(MessagePackWriter);
			writer = new BufferWriter(sequencePool, array);
			OldSpec = false;
		}

		public MessagePackWriter Clone(IBufferWriter<byte> writer)
		{
			MessagePackWriter result = new MessagePackWriter(writer);
			result.OldSpec = OldSpec;
			result.CancellationToken = CancellationToken;
			return result;
		}

		public void Flush()
		{
			writer.Commit();
		}

		public void WriteNil()
		{
			AssumesTrue(MessagePackPrimitives.TryWriteNil(writer.GetSpan(1), out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteRaw(ReadOnlySpan<byte> rawMessagePackBlock)
		{
			writer.Write(rawMessagePackBlock);
		}

		public void WriteRaw(in ReadOnlySequence<byte> rawMessagePackBlock)
		{
			ReadOnlySequence<byte>.Enumerator enumerator = rawMessagePackBlock.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ReadOnlyMemory<byte> current = enumerator.Current;
				writer.Write(current.Span);
			}
		}

		public void WriteArrayHeader(int count)
		{
			WriteArrayHeader(checked((uint)count));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteArrayHeader(uint count)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteArrayHeader(writer.GetSpan(5), count, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteMapHeader(int count)
		{
			WriteMapHeader(checked((uint)count));
		}

		public void WriteMapHeader(uint count)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteMapHeader(writer.GetSpan(5), count, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(byte value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(2), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteUInt8(byte value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteUInt8(writer.GetSpan(2), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(sbyte value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(2), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteInt8(sbyte value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteInt8(writer.GetSpan(2), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(ushort value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(3), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteUInt16(ushort value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteUInt16(writer.GetSpan(3), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(short value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(3), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteInt16(short value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteInt16(writer.GetSpan(3), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(uint value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(5), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteUInt32(uint value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteUInt32(writer.GetSpan(5), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(int value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(5), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteInt32(int value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteInt32(writer.GetSpan(5), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(ulong value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(9), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteUInt64(ulong value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteUInt64(writer.GetSpan(9), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(long value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(9), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteInt64(long value)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteInt64(writer.GetSpan(9), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(bool value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(1), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(char value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(3), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(float value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(5), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(double value)
		{
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(9), value, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(DateTime dateTime)
		{
			if (OldSpec)
			{
				throw new NotSupportedException("The MsgPack spec does not define a format for DateTime in OldSpec mode. Turn off OldSpec mode or use NativeDateTimeFormatter.");
			}
			AssumesTrue(MessagePackPrimitives.TryWrite(writer.GetSpan(15), dateTime, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void Write(byte[]? src)
		{
			if (src == null)
			{
				WriteNil();
			}
			else
			{
				Write(src.AsSpan());
			}
		}

		public void Write(ReadOnlySpan<byte> src)
		{
			int length = src.Length;
			WriteBinHeader(length);
			Span<byte> span = writer.GetSpan(length);
			src.CopyTo(span);
			writer.Advance(length);
		}

		public void Write(in ReadOnlySequence<byte> src)
		{
			int num = checked((int)src.Length);
			WriteBinHeader(num);
			Span<byte> span = writer.GetSpan(num);
			src.CopyTo(span);
			writer.Advance(num);
		}

		public void WriteBinHeader(int length)
		{
			if (OldSpec)
			{
				WriteStringHeader(length);
				return;
			}
			AssumesTrue(checked(MessagePackPrimitives.TryWriteBinHeader(writer.GetSpan(length + 5), (uint)length, out var bytesWritten)));
			writer.Advance(bytesWritten);
		}

		public void WriteString(in ReadOnlySequence<byte> utf8stringBytes)
		{
			int num = checked((int)utf8stringBytes.Length);
			WriteStringHeader(num);
			Span<byte> span = writer.GetSpan(num);
			utf8stringBytes.CopyTo(span);
			writer.Advance(num);
		}

		public void WriteString(ReadOnlySpan<byte> utf8stringBytes)
		{
			int length = utf8stringBytes.Length;
			WriteStringHeader(length);
			Span<byte> span = writer.GetSpan(length);
			utf8stringBytes.CopyTo(span);
			writer.Advance(length);
		}

		public void WriteStringHeader(int byteCount)
		{
			AssumesTrue(checked(MessagePackPrimitives.TryWriteStringHeader(writer.GetSpan(byteCount + 5), (uint)byteCount, out var bytesWritten)));
			writer.Advance(bytesWritten);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write(string? value)
		{
			if (value == null)
			{
				WriteNil();
				return;
			}
			int bufferSize;
			int encodedBytesOffset;
			ref byte reference = ref WriteString_PrepareSpan(value.Length, out bufferSize, out encodedBytesOffset);
			fixed (char* chars = value)
			{
				fixed (byte* ptr = &reference)
				{
					int bytes = StringEncoding.UTF8.GetBytes(chars, value.Length, (byte*)checked(unchecked((nuint)ptr) + unchecked((nuint)encodedBytesOffset)), bufferSize);
					WriteString_PostEncoding(ptr, encodedBytesOffset, bytes);
				}
			}
		}

		public unsafe void Write(ReadOnlySpan<char> value)
		{
			int bufferSize;
			int encodedBytesOffset;
			ref byte reference = ref WriteString_PrepareSpan(value.Length, out bufferSize, out encodedBytesOffset);
			fixed (char* chars = value)
			{
				fixed (byte* ptr = &reference)
				{
					int bytes = StringEncoding.UTF8.GetBytes(chars, value.Length, (byte*)checked(unchecked((nuint)ptr) + unchecked((nuint)encodedBytesOffset)), bufferSize);
					WriteString_PostEncoding(ptr, encodedBytesOffset, bytes);
				}
			}
		}

		public void WriteExtensionFormatHeader(ExtensionHeader extensionHeader)
		{
			AssumesTrue(MessagePackPrimitives.TryWriteExtensionFormatHeader(writer.GetSpan(checked((int)(extensionHeader.Length + 6))), extensionHeader, out var bytesWritten));
			writer.Advance(bytesWritten);
		}

		public void WriteExtensionFormat(ExtensionResult extensionData)
		{
			WriteExtensionFormatHeader(extensionData.Header);
			WriteRaw(extensionData.Data);
		}

		public Span<byte> GetSpan(int length)
		{
			return writer.GetSpan(length);
		}

		public void Advance(int length)
		{
			writer.Advance(length);
		}

		internal byte[] FlushAndGetArray()
		{
			if (writer.TryGetUncommittedSpan(out var span))
			{
				return span.ToArray();
			}
			if (writer.SequenceRental.Value == null)
			{
				throw new NotSupportedException("This instance was not initialized to support this operation.");
			}
			Flush();
			byte[] result = writer.SequenceRental.Value.AsReadOnlySequence.ToArray<byte>();
			writer.SequenceRental.Dispose();
			return result;
		}

		private unsafe static void WriteBigEndian(ushort value, byte* span)
		{
			*span = (byte)(value >> 8);
			span[1] = (byte)value;
		}

		private unsafe static void WriteBigEndian(uint value, byte* span)
		{
			*span = (byte)(value >> 24);
			span[1] = (byte)(value >> 16);
			span[2] = (byte)(value >> 8);
			span[3] = (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref byte WriteString_PrepareSpan(int characterLength, out int bufferSize, out int encodedBytesOffset)
		{
			bufferSize = checked(StringEncoding.UTF8.GetMaxByteCount(characterLength) + 5);
			ref byte pointer = ref writer.GetPointer(bufferSize);
			int num = ((characterLength <= 31) ? 1 : ((characterLength <= 255 && !OldSpec) ? 2 : ((characterLength > 65535) ? 5 : 3)));
			encodedBytesOffset = num;
			return ref pointer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void WriteString_PostEncoding(byte* pBuffer, int estimatedOffset, int byteCount)
		{
			if (byteCount <= 31)
			{
				if (estimatedOffset != 1)
				{
					Buffer.MemoryCopy((void*)checked(unchecked((nuint)pBuffer) + unchecked((nuint)estimatedOffset)), (void*)checked(unchecked((nuint)pBuffer) + (nuint)1u), byteCount, byteCount);
				}
				checked
				{
					*pBuffer = (byte)(0xA0 | byteCount);
					writer.Advance(byteCount + 1);
				}
			}
			else if (byteCount <= 255 && !OldSpec)
			{
				if (estimatedOffset != 2)
				{
					Buffer.MemoryCopy((void*)checked(unchecked((nuint)pBuffer) + unchecked((nuint)estimatedOffset)), (void*)checked(unchecked((nuint)pBuffer) + (nuint)2u), byteCount, byteCount);
				}
				*pBuffer = 217;
				pBuffer[1] = (byte)byteCount;
				writer.Advance(checked(byteCount + 2));
			}
			else if (byteCount <= 65535)
			{
				if (estimatedOffset != 3)
				{
					Buffer.MemoryCopy((void*)checked(unchecked((nuint)pBuffer) + unchecked((nuint)estimatedOffset)), (void*)checked(unchecked((nuint)pBuffer) + (nuint)3u), byteCount, byteCount);
				}
				*pBuffer = 218;
				WriteBigEndian(checked((ushort)byteCount), (byte*)checked(unchecked((nuint)pBuffer) + (nuint)1u));
				writer.Advance(checked(byteCount + 3));
			}
			else
			{
				if (estimatedOffset != 5)
				{
					Buffer.MemoryCopy((void*)checked(unchecked((nuint)pBuffer) + unchecked((nuint)estimatedOffset)), (void*)checked(unchecked((nuint)pBuffer) + (nuint)5u), byteCount, byteCount);
				}
				*pBuffer = 219;
				WriteBigEndian(checked((uint)byteCount), (byte*)checked(unchecked((nuint)pBuffer) + (nuint)1u));
				writer.Advance(checked(byteCount + 5));
			}
		}

		public static int GetEncodedLength(long value)
		{
			if (value >= 0)
			{
				return (value <= 65535) ? ((value <= 127) ? 1 : ((value > 255) ? 3 : 2)) : ((value > uint.MaxValue) ? 9 : 5);
			}
			return (value >= -32768) ? ((value >= -32) ? 1 : ((value < -128) ? 3 : 2)) : ((value < int.MinValue) ? 9 : 5);
		}

		public static int GetEncodedLength(ulong value)
		{
			if (value > long.MaxValue)
			{
				return 9;
			}
			return GetEncodedLength(checked((long)value));
		}

		private static void AssumesTrue([DoesNotReturnIf(false)] bool condition)
		{
			if (!condition)
			{
				throw new Exception("Internal error.");
			}
		}
	}
}
