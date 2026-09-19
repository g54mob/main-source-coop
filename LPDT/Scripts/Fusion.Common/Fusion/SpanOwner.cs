using System;
using System.Buffers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Fusion
{
	[StructLayout(LayoutKind.Auto)]
	internal ref struct SpanOwner<T>
	{
		[CanBeNull]
		private readonly object owner;

		private readonly Span<T> memory;

		public readonly Span<T> Span => memory;

		public readonly bool IsEmpty => memory.IsEmpty;

		public readonly int Length => memory.Length;

		public readonly ref T this[int index] => ref memory[index];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanOwner(Span<T> span)
		{
			owner = null;
			memory = span;
		}

		public SpanOwner(Span<T> span, int length)
			: this(span.Slice(0, length))
		{
		}

		public SpanOwner(int minBufferSize, bool exactSize = true)
		{
			T[] array = ArrayPool<T>.Shared.Rent(minBufferSize);
			memory = (exactSize ? new Span<T>(array, 0, minBufferSize) : new Span<T>(array));
			owner = array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator SpanOwner<T>(Span<T> span)
		{
			return new SpanOwner<T>(span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public readonly ref T GetPinnableReference()
		{
			return ref memory.GetPinnableReference();
		}

		public override readonly string ToString()
		{
			return memory.ToString();
		}

		public void Dispose()
		{
			if (owner is T[] array)
			{
				ArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
			}
			this = default(SpanOwner<T>);
		}
	}
}
