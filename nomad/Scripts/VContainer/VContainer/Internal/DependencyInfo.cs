using System;
using System.Reflection;

namespace VContainer.Internal
{
	internal readonly struct DependencyInfo
	{
		public readonly Registration Dependency;

		private readonly Registration owner;

		private readonly object method;

		private readonly ParameterInfo param;

		public Type ImplementationType => Dependency.ImplementationType;

		public IInstanceProvider Provider => Dependency.Provider;

		public DependencyInfo(Registration dependency)
		{
			Dependency = dependency;
			owner = null;
			method = null;
			param = null;
		}

		public DependencyInfo(Registration dependency, Registration owner, ConstructorInfo ctor, ParameterInfo param)
		{
			Dependency = dependency;
			this.owner = owner;
			method = ctor;
			this.param = param;
		}

		public DependencyInfo(Registration dependency, Registration owner, MethodInfo method, ParameterInfo param)
		{
			Dependency = dependency;
			this.owner = owner;
			this.method = method;
			this.param = param;
		}

		public DependencyInfo(Registration dependency, Registration owner, FieldInfo field)
		{
			Dependency = dependency;
			this.owner = owner;
			method = field;
			param = null;
		}

		public DependencyInfo(Registration dependency, Registration owner, PropertyInfo prop)
		{
			Dependency = dependency;
			this.owner = owner;
			method = prop;
			param = null;
		}

		public override string ToString()
		{
			object obj = method;
			if (!(obj is ConstructorInfo))
			{
				if (!(obj is MethodInfo methodInfo))
				{
					if (!(obj is FieldInfo fieldInfo))
					{
						if (obj is PropertyInfo propertyInfo)
						{
							return owner.ImplementationType.FullName + "." + propertyInfo.Name;
						}
						return "";
					}
					return owner.ImplementationType.FullName + "." + fieldInfo.Name;
				}
				return owner.ImplementationType.FullName + "." + methodInfo.Name + "(" + param.Name + ")";
			}
			return $"{owner.ImplementationType}..ctor({param.Name})";
		}
	}
}
