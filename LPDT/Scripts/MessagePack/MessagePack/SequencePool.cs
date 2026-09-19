using System;
using System.Buffers;
using System.Collections.Generic;
using Nerdbank.Streams;

namespace MessagePack
{
	public class SequencePool
	{
		internal struct Rental : IDisposable
		{
			private readonly SequencePool owner;

			public Sequence<byte> Value { get; }

			internal Rental(SequencePool owner, Sequence<byte> value)
			{
				this.owner = owner;
				Value = value;
			}

			public void Dispose()
			{
				owner?.Return(Value);
			}
		}

		internal static readonly SequencePool Shared = new SequencePool();

		private const int MinimumSpanLength = 32768;

		private readonly int maxSize;

		private readonly Stack<Sequence<byte>> pool = new Stack<Sequence<byte>>();

		private readonly object arrayPoolOrMemoryPool;

		public SequencePool()
			: this(checked(Environment.ProcessorCount * 2), ArrayPool<byte>.Create(81920, 100))
		{
		}

		public SequencePool(int maxSize)
			: this(maxSize, ArrayPool<byte>.Create(81920, 100))
		{
		}

		public SequencePool(int maxSize, ArrayPool<byte> arrayPool)
		{
			this.maxSize = maxSize;
			arrayPoolOrMemoryPool = arrayPool;
		}

		public SequencePool(int maxSize, MemoryPool<byte> memoryPool)
		{
			this.maxSize = maxSize;
			arrayPoolOrMemoryPool = memoryPool;
		}

		public void Clear()
		{
			lock (pool)
			{
				pool.Clear();
			}
		}

		internal Rental Rent()
		{
			lock (pool)
			{
				if (pool.Count > 0)
				{
					return new Rental(this, pool.Pop());
				}
			}
			Sequence<byte> sequence = ((arrayPoolOrMemoryPool is ArrayPool<byte> arrayPool) ? new Sequence<byte>(arrayPool) : new Sequence<byte>((MemoryPool<byte>)arrayPoolOrMemoryPool));
			sequence.MinimumSpanLength = 32768;
			return new Rental(this, sequence);
		}

		private void Return(Sequence<byte> value)
		{
			value.Reset();
			lock (pool)
			{
				if (pool.Count < maxSize)
				{
					value.MinimumSpanLength = 32768;
					pool.Push(value);
				}
			}
		}
	}
}
