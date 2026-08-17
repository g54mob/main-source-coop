using System;
using System.Linq;

namespace EvilCore.Extensions
{
	public static class EquatableExtensions
	{
		public static bool EqualsToAll<T>(this T value, params T[] values) where T : IEquatable<T>
		{
			return values.All((T o) => o.Equals(value));
		}

		public static bool EqualsToAny<T>(this T value, params T[] values) where T : IEquatable<T>
		{
			return values.Any((T o) => o.Equals(value));
		}
	}
}
