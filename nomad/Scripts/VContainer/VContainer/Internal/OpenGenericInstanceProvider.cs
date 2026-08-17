using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VContainer.Internal
{
	public class OpenGenericInstanceProvider : IInstanceProvider
	{
		private class TypeParametersEqualityComparer : IEqualityComparer<Type[]>
		{
			public bool Equals(Type[] x, Type[] y)
			{
				if (x == null || y == null)
				{
					return x == y;
				}
				if (x.Length != y.Length)
				{
					return false;
				}
				for (int i = 0; i < x.Length; i++)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
				return true;
			}

			public int GetHashCode(Type[] typeParameters)
			{
				int num = 5381;
				foreach (Type type in typeParameters)
				{
					num = ((num << 5) + num) ^ type.GetHashCode();
				}
				return num;
			}
		}

		private readonly Lifetime lifetime;

		private readonly Type implementationType;

		private readonly IReadOnlyList<IInjectParameter> customParameters;

		private readonly ConcurrentDictionary<Type[], Registration> constructedRegistrations = new ConcurrentDictionary<Type[], Registration>(new TypeParametersEqualityComparer());

		private readonly Func<Type[], Registration> createRegistrationFunc;

		public OpenGenericInstanceProvider(Type implementationType, Lifetime lifetime, List<IInjectParameter> injectParameters)
		{
			this.implementationType = implementationType;
			this.lifetime = lifetime;
			customParameters = injectParameters;
			createRegistrationFunc = CreateRegistration;
		}

		public Registration GetClosedRegistration(Type closedInterfaceType, Type[] typeParameters)
		{
			return constructedRegistrations.GetOrAdd(typeParameters, createRegistrationFunc);
		}

		private Registration CreateRegistration(Type[] typeParameters)
		{
			Type type = implementationType.MakeGenericType(typeParameters);
			InstanceProvider provider = new InstanceProvider(InjectorCache.GetOrBuild(type), customParameters);
			return new Registration(type, lifetime, new List<Type>(1) { type }, provider);
		}

		public object SpawnInstance(IObjectResolver resolver)
		{
			throw new InvalidOperationException();
		}
	}
}
