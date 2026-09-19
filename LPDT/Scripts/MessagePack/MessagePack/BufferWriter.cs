using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MessagePack
{
	internal ref struct BufferWriter
	{
		private IBufferWriter<byte> _output;

		private Span<byte> _span;

		private ArraySegment<byte> _segment;

		private int _buffered;

		private long _bytesCommitted;

		private SequencePool _sequencePool;

		private SequencePool.Rental _rental;

		public Span<byte> Span => _span;

		public long BytesCommitted => _bytesCommitted;

		internal IBufferWriter<byte> UnderlyingWriter => _output;

		internal SequencePool.Rental SequenceRental => _rental;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BufferWriter(IBufferWriter<byte> output)
		{
			_buffered = 0;
			_bytesCommitted = 0L;
			_output = output ?? throw new ArgumentNullException("output");
			_sequencePool = null;
			_rental = default(SequencePool.Rental);
			Memory<byte> memoryCheckResult = _output.GetMemoryCheckResult();
			MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)memoryCheckResult, out _segment);
			_span = memoryCheckResult.Span;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal BufferWriter(SequencePool sequencePool, byte[] array)
		{
			_buffered = 0;
			_bytesCommitted = 0L;
			_sequencePool = sequencePool ?? throw new ArgumentNullException("sequencePool");
			_rental = default(SequencePool.Rental);
			_output = null;
			_segment = new ArraySegment<byte>(array);
			_span = _segment.AsSpan();
		}

		public Span<byte> GetSpan(int sizeHint = 0)
		{
			Ensure(sizeHint);
			return Span;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref byte GetPointer(int sizeHint = 0)
		{
			Ensure(sizeHint);
			if (_segment.Array != null)
			{
				return ref _segment.Array[checked(_segment.Offset + _buffered)];
			}
			return ref _span.GetPinnableReference();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Commit()
		{
			int buffered = _buffered;
			checked
			{
				if (buffered > 0)
				{
					MigrateToSequence();
					_bytesCommitted += buffered;
					_buffered = 0;
					_output.Advance(buffered);
					_span = default(Span<byte>);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Advance(int count)
		{
			checked
			{
				_buffered += count;
				_span = _span.Slice(count);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(ReadOnlySpan<byte> source)
		{
			if (_span.Length >= source.Length)
			{
				source.CopyTo(_span);
				Advance(source.Length);
			}
			else
			{
				WriteMultiBuffer(source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Ensure(int count = 0)
		{
			if (_span.Length < count)
			{
				EnsureMore(count);
			}
		}

		internal bool TryGetUncommittedSpan(out ReadOnlySpan<byte> span)
		{
			if (_sequencePool != null)
			{
				span = _segment.AsSpan(0, _buffered);
				return true;
			}
			span = default(ReadOnlySpan<byte>);
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void EnsureMore(int count = 0)
		{
			if (_buffered > 0)
			{
				Commit();
			}
			else
			{
				MigrateToSequence();
			}
			Memory<byte> memoryCheckResult = _output.GetMemoryCheckResult(count);
			MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)memoryCheckResult, out _segment);
			_span = memoryCheckResult.Span;
		}

		private void WriteMultiBuffer(ReadOnlySpan<byte> source)
		{
			int num = 0;
			int num2 = source.Length;
			checked
			{
				while (num2 > 0)
				{
					if (_span.Length == 0)
					{
						EnsureMore();
					}
					int num3 = Math.Min(num2, _span.Length);
					source.Slice(num, num3).CopyTo(_span);
					num += num3;
					num2 -= num3;
					Advance(num3);
				}
			}
		}

		private void MigrateToSequence()
		{
			if (_sequencePool != null)
			{
				_rental = _sequencePool.Rent();
				_output = _rental.Value;
				Span<byte> span = _output.GetSpan(_buffered);
				_segment.AsSpan(0, _buffered).CopyTo(span);
				_sequencePool = null;
			}
		}
	}
}
