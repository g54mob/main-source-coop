using System;

namespace Zorro.Core
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassImplementsAttribute : ClassTypeConstraintAttribute
	{
		public Type InterfaceType { get; private set; }

		public ClassImplementsAttribute()
		{
		}

		public ClassImplementsAttribute(Type interfaceType)
		{
			InterfaceType = interfaceType;
		}

		public override bool IsConstraintSatisfied(Type type)
		{
			if (base.IsConstraintSatisfied(type))
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] == InterfaceType)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
