using System;

namespace Zorro.Core
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassExtendsAttribute : ClassTypeConstraintAttribute
	{
		public Type BaseType { get; private set; }

		public ClassExtendsAttribute()
		{
		}

		public ClassExtendsAttribute(Type baseType)
		{
			BaseType = baseType;
		}

		public override bool IsConstraintSatisfied(Type type)
		{
			if (base.IsConstraintSatisfied(type) && BaseType.IsAssignableFrom(type))
			{
				return type != BaseType;
			}
			return false;
		}
	}
}
