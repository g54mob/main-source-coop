using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace VContainer.Internal
{
	public static class InjectorCache
	{
		private static readonly ConcurrentDictionary<Type, IInjector> Injectors = new ConcurrentDictionary<Type, IInjector>();

		public static IInjector GetOrBuild(Type type)
		{
			return Injectors.GetOrAdd(type, delegate(Type key)
			{
				Type type2 = key.Assembly.GetType(key.FullName + "GeneratedInjector", throwOnError: false);
				if (type2 != null)
				{
					return (IInjector)Activator.CreateInstance(type2);
				}
				MethodInfo method = key.GetMethod("__GetGeneratedInjector", BindingFlags.Static | BindingFlags.Public);
				return (method != null) ? ((IInjector)method.Invoke(null, null)) : ReflectionInjector.Build(key);
			});
		}
	}
}
