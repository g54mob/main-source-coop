using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace QFSW.QC
{
	public class InjectionLoader<T>
	{
		private Type[] _injectableTypes;

		public Type[] GetInjectableTypes(bool forceReload = false)
		{
			if (_injectableTypes == null || forceReload)
			{
				_injectableTypes = (from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly assembly) => GetTypesSafe(assembly))
					where typeof(T).IsAssignableFrom(type)
					where !type.IsAbstract
					where !type.IsDefined(typeof(NoInjectAttribute), inherit: false)
					select type).ToArray();
			}
			return _injectableTypes;
		}

		public IEnumerable<T> GetInjectedInstances(bool forceReload = false)
		{
			IEnumerable<Type> injectableTypes = GetInjectableTypes(forceReload);
			return GetInjectedInstances(injectableTypes);
		}

		public IEnumerable<T> GetInjectedInstances(IEnumerable<Type> injectableTypes)
		{
			foreach (Type injectableType in injectableTypes)
			{
				T val = default(T);
				bool flag = false;
				try
				{
					val = (T)Activator.CreateInstance(injectableType);
					flag = true;
				}
				catch (MissingMethodException)
				{
					Debug.LogError($"Could not load {typeof(T)} {injectableType} as it is missing a public parameterless constructor.");
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				if (flag)
				{
					yield return val;
				}
			}
		}

		private Type[] GetTypesSafe(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException)
			{
				return Array.Empty<Type>();
			}
		}
	}
}
