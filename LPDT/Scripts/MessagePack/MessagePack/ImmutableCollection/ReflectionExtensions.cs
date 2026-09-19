using System;
using System.Reflection;

namespace MessagePack.ImmutableCollection
{
	internal static class ReflectionExtensions
	{
		public static bool IsNullable(this TypeInfo type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			return false;
		}
	}
}
