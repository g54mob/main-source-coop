using System;
using System.Collections.Generic;

namespace VContainer.Internal
{
	internal static class ListPool<T>
	{
		internal readonly struct BufferScope : IDisposable
		{
			private readonly List<T> _buffer;

			public BufferScope(List<T> buffer)
			{
				_buffer = buffer;
			}

			public void Dispose()
			{
				ListPool<T>.Release(_buffer);
			}
		}

		private const int DefaultCapacity = 32;

		private static readonly Stack<List<T>> _pool = new Stack<List<T>>(4);

		internal static List<T> Get()
		{
			lock (_pool)
			{
				if (_pool.Count == 0)
				{
					return new List<T>(32);
				}
				return _pool.Pop();
			}
		}

		internal static BufferScope Get(out List<T> buffer)
		{
			buffer = Get();
			return new BufferScope(buffer);
		}

		internal static void Release(List<T> buffer)
		{
			buffer.Clear();
			lock (_pool)
			{
				_pool.Push(buffer);
			}
		}
	}
}
