using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Fusion
{
	public static class FusionPlatform
	{
		public static readonly FusionScriptingBackend CurrentBackend;

		public static readonly bool IsWebGL;

		static FusionPlatform()
		{
			CurrentBackend = FusionScriptingBackend.Mono;
			if (GetAnyLoadedAssemblyAttribute<MarkPlatformAsIL2CPPIfEnableIL2CPPDefinedAttribute>() != null)
			{
				CurrentBackend = FusionScriptingBackend.IL2CPP;
			}
			if (GetAnyLoadedAssemblyAttribute<MarkPlatformAsWebIfUnityWebGlDefinedAttribute>() != null)
			{
				IsWebGL = true;
			}
		}

		public static IReadOnlyList<Assembly> GetLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		public static T GetAnyAssemblyAttribute<T>(IReadOnlyList<Assembly> assemblies) where T : Attribute
		{
			foreach (Assembly assembly in assemblies)
			{
				using IEnumerator<T> enumerator2 = assembly.GetCustomAttributes<T>().GetEnumerator();
				if (enumerator2.MoveNext())
				{
					return enumerator2.Current;
				}
			}
			return null;
		}

		[return: NotNull]
		public static T[] GetAssemblyAttributes<T>(IReadOnlyList<Assembly> assemblies) where T : Attribute
		{
			List<T> list = new List<T>();
			foreach (Assembly assembly in assemblies)
			{
				foreach (T customAttribute in assembly.GetCustomAttributes<T>())
				{
					list.Add(customAttribute);
				}
			}
			return list.ToArray();
		}

		public static T GetAnyLoadedAssemblyAttribute<T>() where T : Attribute
		{
			return GetAnyAssemblyAttribute<T>(GetLoadedAssemblies());
		}

		public static T[] GetLoadedAssemblyAttributes<T>() where T : Attribute
		{
			return GetAssemblyAttributes<T>(GetLoadedAssemblies());
		}

		public static string GetLoadedPath(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			return assembly.Location;
		}
	}
}
