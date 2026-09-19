using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Fusion
{
	[Obsolete("No longer used or maintained")]
	public static class ReflectionUtils
	{
		[return: NotNull]
		public static T GetCustomAttributeOrThrow<T>(this MemberInfo member, bool inherit) where T : Attribute
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(T), inherit);
			if (customAttributes.Length == 0)
			{
				throw new ArgumentOutOfRangeException("T", $"{member} has no attribute {typeof(T)}");
			}
			if (customAttributes.Length > 1)
			{
				throw new InvalidOperationException($"{member} has more than one attribute {typeof(T)}");
			}
			return (T)customAttributes[0];
		}
	}
}
