using System.Collections.Generic;
using UnityEngine;

namespace Zorro.Core
{
	public static class ListExtensions
	{
		public static T GetRandom<T>(this List<T> array)
		{
			int index = Random.Range(0, array.Count);
			return array[index];
		}
	}
}
