using System;
using System.Reflection;

namespace MCPForUnity.Runtime.Helpers
{
	public static class UnityAssembliesCompat
	{
		private static readonly string[] CurrentAssembliesAqns = new string[4] { "UnityEngine.Assemblies.CurrentAssemblies, UnityEngine.CoreModule", "UnityEngine.Assemblies.CurrentAssemblies, UnityEngine", "UnityEngine.Assemblies.CurrentAssemblies, UnityEditor.CoreModule", "UnityEngine.Assemblies.CurrentAssemblies, UnityEditor" };

		private static Func<Assembly[]> _getLoadedAssemblies;

		private static bool _probed;

		public static Assembly[] GetLoadedAssemblies()
		{
			if (!_probed)
			{
				_probed = true;
				_getLoadedAssemblies = ResolveCurrentAssembliesDelegate();
			}
			if (_getLoadedAssemblies != null)
			{
				try
				{
					return _getLoadedAssemblies();
				}
				catch
				{
				}
			}
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		private static Func<Assembly[]> ResolveCurrentAssembliesDelegate()
		{
			string[] currentAssembliesAqns = CurrentAssembliesAqns;
			foreach (string typeName in currentAssembliesAqns)
			{
				Type type;
				try
				{
					type = Type.GetType(typeName, throwOnError: false);
				}
				catch
				{
					type = null;
				}
				Func<Assembly[]> func = TryBindGetLoadedAssemblies(type);
				if (func != null)
				{
					return func;
				}
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type type2;
				try
				{
					type2 = assembly.GetType("UnityEngine.Assemblies.CurrentAssemblies", throwOnError: false);
				}
				catch
				{
					continue;
				}
				Func<Assembly[]> func2 = TryBindGetLoadedAssemblies(type2);
				if (func2 != null)
				{
					return func2;
				}
			}
			return null;
		}

		private static Func<Assembly[]> TryBindGetLoadedAssemblies(Type type)
		{
			if (type == null)
			{
				return null;
			}
			MethodInfo method = type.GetMethod("GetLoadedAssemblies", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
			if (method == null || !typeof(Assembly[]).IsAssignableFrom(method.ReturnType))
			{
				return null;
			}
			try
			{
				return (Func<Assembly[]>)Delegate.CreateDelegate(typeof(Func<Assembly[]>), method);
			}
			catch
			{
				return () => (Assembly[])method.Invoke(null, null);
			}
		}
	}
}
