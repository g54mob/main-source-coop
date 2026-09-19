using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MessagePack
{
	internal static class Utilities
	{
		internal delegate void GetWriterBytesAction<TArg>(ref MessagePackWriter writer, TArg argument);

		internal struct NonGenericDictionaryEnumerable
		{
			private IDictionary dictionary;

			internal NonGenericDictionaryEnumerable(IDictionary dictionary)
			{
				this.dictionary = dictionary;
			}

			public NonGenericDictionaryEnumerator GetEnumerator()
			{
				return new NonGenericDictionaryEnumerator(dictionary);
			}
		}

		internal struct NonGenericDictionaryEnumerator : IEnumerator<DictionaryEntry>, IEnumerator, IDisposable
		{
			private IDictionaryEnumerator enumerator;

			public DictionaryEntry Current => enumerator.Entry;

			object IEnumerator.Current => enumerator.Entry;

			internal NonGenericDictionaryEnumerator(IDictionary dictionary)
			{
				enumerator = dictionary.GetEnumerator();
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}

			public void Reset()
			{
				enumerator.Reset();
			}
		}

		internal static byte[] GetWriterBytes<TArg>(TArg arg, GetWriterBytesAction<TArg> action, SequencePool pool)
		{
			using SequencePool.Rental rental = pool.Rent();
			MessagePackWriter writer = new MessagePackWriter(rental.Value);
			action(ref writer, arg);
			writer.Flush();
			return rental.Value.AsReadOnlySequence.ToArray<byte>();
		}

		internal static Memory<byte> GetMemoryCheckResult(this IBufferWriter<byte> bufferWriter, int size = 0)
		{
			Memory<byte> memory = bufferWriter.GetMemory(size);
			if (memory.IsEmpty)
			{
				ThrowInvalidOperationException("The underlying IBufferWriter<byte>.GetMemory(int) method returned an empty memory block, which is not allowed. This is a bug in " + bufferWriter.GetType().FullName);
			}
			if (memory.Length < size)
			{
				ThrowInvalidOperationException("The underlying IBufferWriter<byte>.GetMemory(int) returned a buffer that is smaller than the requested size. This is a bug in " + bufferWriter.GetType().FullName);
			}
			return memory;
			[DoesNotReturn]
			static void ThrowInvalidOperationException(string message)
			{
				throw new InvalidOperationException(message);
			}
		}

		internal static NonGenericDictionaryEnumerable GetEntryEnumerator(this IDictionary dictionary)
		{
			return new NonGenericDictionaryEnumerable(dictionary);
		}
	}
}
