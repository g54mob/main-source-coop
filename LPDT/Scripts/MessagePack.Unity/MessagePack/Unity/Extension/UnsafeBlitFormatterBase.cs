using System;
using System.Buffers;
using System.Runtime.InteropServices;
using MessagePack.Formatters;

namespace MessagePack.Unity.Extension
{
	public abstract class UnsafeBlitFormatterBase<T, TReverseEndianessHelper> : IMessagePackFormatter<T[]?>, IMessagePackFormatter where T : unmanaged where TReverseEndianessHelper : struct, IReverseEndianessHelper
	{
		protected abstract sbyte TypeCode { get; }

		protected void CopyDeserializeUnsafe(ReadOnlySpan<byte> src, Span<T> dest)
		{
			src.CopyTo(MemoryMarshal.Cast<T, byte>(dest));
		}

		public unsafe void Serialize(ref MessagePackWriter writer, T[]? value, MessagePackSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNil();
				return;
			}
			int num = value.Length * sizeof(T);
			int length = MessagePackWriter.GetEncodedLength(num) + num + 1;
			writer.WriteExtensionFormatHeader(new ExtensionHeader(TypeCode, length));
			writer.Write(num);
			writer.Write(BitConverter.IsLittleEndian);
			writer.WriteRaw(MemoryMarshal.Cast<T, byte>((Span<T>)value));
		}

		public unsafe T[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			if (reader.ReadExtensionFormatHeader().TypeCode != TypeCode)
			{
				throw new InvalidOperationException("Invalid typeCode.");
			}
			int num = reader.ReadInt32();
			bool num2 = reader.ReadBoolean();
			T[] array = new T[num / sizeof(T)];
			Span<byte> destination = MemoryMarshal.Cast<T, byte>((Span<T>)array);
			reader.ReadRaw(num).CopyTo(destination);
			if (num2 != BitConverter.IsLittleEndian && array.Length != 0)
			{
				TReverseEndianessHelper val = default(TReverseEndianessHelper);
				for (int i = 0; i < destination.Length; i += sizeof(T))
				{
					val.ReverseEndianess(destination.Slice(i, sizeof(T)));
				}
			}
			return array;
		}
	}
}
