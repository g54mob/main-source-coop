using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zorro.Core
{
	public static class IEnumerableExtensions
	{
		public static T RandomElement<T>(this IEnumerable<T> enumerable)
		{
			T[] source = (enumerable as T[]) ?? enumerable.ToArray();
			int index = UnityEngine.Random.Range(0, source.Count());
			return source.ElementAt(index);
		}

		public static T? MaxBy<T>(this IEnumerable<T> enumerable, Func<T, IComparable> selector)
		{
			return enumerable.OrderByDescending(selector).FirstOrDefault();
		}
	}
}
