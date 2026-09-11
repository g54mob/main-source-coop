using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Zorro.Core
{
	public static class ReflectionUtility
	{
		public const BindingFlags FULL_BINDING = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static Type[] GetClassesThatDeriveFrom(Type baseType)
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = assemblies[i].GetTypes();
				foreach (Type type in types)
				{
					if (type.IsSubclassOf(baseType))
					{
						list.Add(type);
					}
				}
			}
			return list.ToArray();
		}

		public static (Type, T)[] GetClassesWithAttribute<T>() where T : Attribute
		{
			List<(Type, T)> list = new List<(Type, T)>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			int num = 0;
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						T val = type.TryGetCustomAttribute<T>();
						if (val != null)
						{
							list.Add((type, val));
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
				num++;
			}
			return list.ToArray();
		}

		public static (Type, T)[] GetClassesWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
		{
			List<(Type, T)> list = new List<(Type, T)>();
			foreach (Type type in types)
			{
				T val = type.TryGetCustomAttribute<T>();
				if (val != null)
				{
					list.Add((type, val));
				}
			}
			return list.ToArray();
		}

		public static FieldInfo[] GetFieldsWithAttribute<T>(Type type) where T : Attribute
		{
			List<FieldInfo> list = new List<FieldInfo>();
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.TryGetCustomAttribute<T>() != null)
				{
					list.Add(fieldInfo);
				}
			}
			return list.ToArray();
		}

		public static (MethodInfo, Attribute)[] GetMethodsWithAttribute<T>() where T : Attribute
		{
			List<(MethodInfo, Attribute)> list = new List<(MethodInfo, Attribute)>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			int num = 0;
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					for (int j = 0; j < types.Length; j++)
					{
						MethodInfo[] methods = types[j].GetMethods();
						foreach (MethodInfo methodInfo in methods)
						{
							T val = methodInfo.TryGetCustomAttribute<T>();
							if (val != null)
							{
								list.Add((methodInfo, val));
							}
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
				num++;
			}
			return list.ToArray();
		}

		public static PropertyInfo FindProperty(this Type type, string propertyName, bool throwNotFound = true)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null && throwNotFound)
			{
				throw new MissingMemberException(type.FullName, propertyName);
			}
			return property;
		}

		public static void SetInternalProperty(this object obj, string propertyName, object value)
		{
			obj.GetType().FindProperty(propertyName).SetValue(obj, value, null);
		}

		public static T TryGetCustomAttribute<T>(this MemberInfo element) where T : Attribute
		{
			try
			{
				return element.GetCustomAttribute<T>();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError($"Failed to get custom attribute of type {typeof(T)} from {element}, see previous message");
				return null;
			}
		}
	}
}
