using System;
using System.Collections.Generic;
using System.Text;

namespace Mirror.BouncyCastle.Utilities.Collections
{
	public abstract class CollectionUtilities
	{
		public static V GetValueOrNull<K, V>(IDictionary<K, V> d, K k) where V : class
		{
			if (!d.TryGetValue(k, out var value))
			{
				return null;
			}
			return value;
		}

		public static T RequireNext<T>(IEnumerator<T> e)
		{
			if (!e.MoveNext())
			{
				throw new InvalidOperationException();
			}
			return e.Current;
		}

		public static string ToString<T>(IEnumerable<T> c)
		{
			IEnumerator<T> enumerator = c.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return "[]";
			}
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(enumerator.Current);
			while (enumerator.MoveNext())
			{
				stringBuilder.Append(", ");
				stringBuilder.Append(enumerator.Current);
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}
