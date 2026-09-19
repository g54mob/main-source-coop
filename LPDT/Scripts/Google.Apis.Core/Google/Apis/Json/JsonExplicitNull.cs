using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.Apis.Json
{
	public static class JsonExplicitNull
	{
		[JsonExplicitNull]
		private sealed class ExplicitNullList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
		{
			public static ExplicitNullList<T> Instance = new ExplicitNullList<T>();

			public T this[int index]
			{
				get
				{
					throw new NotSupportedException();
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			public int Count
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				throw new NotSupportedException();
			}

			public bool Contains(T item)
			{
				throw new NotSupportedException();
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				throw new NotSupportedException();
			}

			public IEnumerator<T> GetEnumerator()
			{
				throw new NotSupportedException();
			}

			public int IndexOf(T item)
			{
				throw new NotSupportedException();
			}

			public void Insert(int index, T item)
			{
				throw new NotSupportedException();
			}

			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			public void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException();
			}
		}

		public static IList<T> ForIList<T>()
		{
			return ExplicitNullList<T>.Instance;
		}
	}
}
