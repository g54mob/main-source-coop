using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Google.Apis.Util
{
	public class Repeatable<T> : IEnumerable<T>, IEnumerable
	{
		private readonly IList<T> values;

		public Repeatable(IEnumerable<T> enumeration)
		{
			values = new ReadOnlyCollection<T>(new List<T>(enumeration));
		}

		public IEnumerator<T> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static implicit operator Repeatable<T>(T elem)
		{
			if (elem == null)
			{
				return null;
			}
			return new Repeatable<T>(new T[1] { elem });
		}

		public static implicit operator Repeatable<T>(T[] elem)
		{
			if (elem.Length == 0)
			{
				return null;
			}
			return new Repeatable<T>(elem);
		}

		public static implicit operator Repeatable<T>(List<T> elem)
		{
			return new Repeatable<T>(elem);
		}
	}
}
