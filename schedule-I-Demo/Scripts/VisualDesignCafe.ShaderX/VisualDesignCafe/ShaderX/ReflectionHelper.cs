using System;
using System.Collections.Generic;
using System.Reflection;

namespace VisualDesignCafe.ShaderX
{
	internal class ReflectionHelper
	{
		private static readonly BindingFlags _allBindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private static readonly BindingFlags _staticBindings = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private static Dictionary<string, MethodInfo> _methodCache = new Dictionary<string, MethodInfo>();

		private static Dictionary<string, Type> _typeNameCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<Type, Type[]> _typeCache = new Dictionary<Type, Type[]>();

		public static object GetPropertyValue<T>(object obj, string propertyName)
		{
			return typeof(T).GetProperty(propertyName, _allBindings)?.GetValue(obj);
		}

		public static object GetPropertyValue<T>(string propertyName)
		{
			return typeof(T).GetProperty(propertyName, _staticBindings)?.GetValue(null);
		}

		public static void Invoke<T>(object obj, string methodName, object[] parameters)
		{
			Invoke(typeof(T), methodName, out var _, obj, parameters);
		}

		public static void Invoke<T>(string methodName, params object[] parameters)
		{
			Invoke(typeof(T), methodName, out var _, parameters);
		}

		public static bool Invoke(string typeName, string methodName, params object[] parameters)
		{
			Type type = GetType(typeName);
			object returnValue;
			if (type != null)
			{
				return Invoke(type, methodName, out returnValue, parameters);
			}
			return false;
		}

		public static bool Invoke<T>(string typeName, string methodName, out T returnValue, params object[] parameters)
		{
			returnValue = default(T);
			Type type = GetType(typeName);
			bool flag = false;
			if (type != null)
			{
				flag = Invoke(type, methodName, out var returnValue2, parameters);
				if (flag)
				{
					returnValue = (T)returnValue2;
				}
			}
			return flag;
		}

		public static Type[] GetTypesDerivedFrom<T>() where T : class
		{
			if (_typeCache.TryGetValue(typeof(T), out var value))
			{
				return value;
			}
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						try
						{
							if (typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
							{
								list.Add(type);
							}
						}
						catch (Exception)
						{
						}
					}
				}
				catch (Exception)
				{
				}
			}
			value = list.ToArray();
			_typeCache[typeof(T)] = value;
			return value;
		}

		public static Type GetType(string fullName)
		{
			if (_typeNameCache.TryGetValue(fullName, out var value))
			{
				return value;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					value = assembly.GetType(fullName, throwOnError: false, ignoreCase: true);
					if (value != null)
					{
						_typeNameCache[fullName] = value;
						return value;
					}
				}
				catch (Exception)
				{
				}
			}
			_typeNameCache[fullName] = null;
			return null;
		}

		private static bool Invoke(Type type, string methodName, out object returnValue, params object[] parameters)
		{
			returnValue = null;
			string key = type.FullName + "." + methodName;
			if (!_methodCache.TryGetValue(key, out var value))
			{
				value = (_methodCache[key] = type.GetMethod(methodName, _allBindings));
			}
			if (value == null)
			{
				return false;
			}
			if (value.IsStatic)
			{
				returnValue = value.Invoke(null, parameters);
			}
			else
			{
				returnValue = value.Invoke(parameters[0], (object[])parameters[1]);
			}
			return true;
		}
	}
}
