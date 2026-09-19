#define DEBUG
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	internal struct PooledList<T> : IDisposable
	{
		private ArrayPool<T> _pool;

		private T[] _array;

		private T _single;

		private int _count;

		public readonly int Count => _count;

		public readonly int Capacity
		{
			get
			{
				T[] array = _array;
				return (array != null) ? array.Length : ((_count > 0) ? 1 : 0);
			}
		}

		public readonly bool IsValid => _pool != null;

		public PooledList([NotNull] ArrayPool<T> pool, int capacity = 4)
		{
			_pool = pool;
			_array = ((capacity <= 1) ? null : pool.Rent(capacity));
			_single = default(T);
			_count = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(T item)
		{
			Assert.Always(IsValid, "IsValid");
			if (_array == null)
			{
				if (_count == 0)
				{
					_single = item;
					_count = 1;
					return;
				}
				Assert.Check(_count == 1, "_count == 1");
				_array = _pool.Rent(4);
				_array[0] = _single;
				_single = default(T);
			}
			else if (_count == _array.Length)
			{
				Grow();
			}
			Assert.Check(_array != null, "_array != null");
			_array[_count++] = item;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> AsSpan()
		{
			if (_array == null)
			{
				return (_count > 0) ? MemoryMarshal.CreateSpan(ref _single, 1) : Span<T>.Empty;
			}
			return new Span<T>(_array, 0, _count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<T> AsEnumerable()
		{
			if (_array == null)
			{
				return (_count > 0) ? Enumerable.Repeat(_single, 1) : Enumerable.Empty<T>();
			}
			return _array.Take(_count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Resize(int count)
		{
			Assert.Always(count <= Capacity, "count <= Capacity");
			_count = count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Span<T>(PooledList<T> list)
		{
			return list.AsSpan();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<T>(PooledList<T> list)
		{
			return list.AsSpan();
		}

		private void Grow()
		{
			int minimumLength = _array.Length * 2;
			T[] array = _pool.Rent(minimumLength);
			Array.Copy(_array, array, _count);
			_pool.Return(_array);
			_array = array;
		}

		[Obsolete("Do not call directly. Use Simulation methods, as they do trace references.")]
		public void Dispose()
		{
			if (_array != null)
			{
				Assert.Check(_pool != null, "_pool != null");
				_pool.Return(_array);
			}
			_single = default(T);
			_array = null;
			_pool = null;
			_count = 0;
		}
	}
}
