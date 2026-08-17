using System.Collections;
using System.Collections.Generic;

namespace QFSW.QC.Containers
{
	public struct ArraySingle<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		private readonly T _data;

		public T this[int index] => _data;

		public int Count => 1;

		public ArraySingle(T data)
		{
			_data = data;
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return _data;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield return _data;
		}
	}
}
