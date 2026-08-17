using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Den.Tools
{
	public static class ReflectionExtensions
	{
		public static object CallStaticMethodFrom(string assembly, string type, string method, params object[] parameters)
		{
			return Assembly.Load(assembly).GetType(type).GetMethod(method)
				.Invoke(null, parameters);
		}

		public static void GetPropertiesFrom<T1, T2>(this T1 dst, T2 src) where T1 : class where T2 : class
		{
			PropertyInfo[] properties = src.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
			PropertyInfo[] properties2 = src.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
			for (int i = 0; i < properties.Length; i++)
			{
				for (int j = 0; j < properties2.Length; j++)
				{
					if (properties[i].Name == properties2[j].Name && properties2[j].CanWrite)
					{
						properties2[j].SetValue(dst, properties[i].GetValue(src, null), null);
					}
				}
			}
		}

		public static IEnumerable<FieldInfo> UsableFields(this Type type, bool nonPublic = false, bool includeStatic = false)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			if (includeStatic)
			{
				bindingFlags |= BindingFlags.Static;
			}
			FieldInfo[] fields = type.GetFields(bindingFlags);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsLiteral && !fieldInfo.FieldType.IsPointer && !fieldInfo.IsNotSerialized)
				{
					yield return fieldInfo;
				}
			}
		}

		public static IEnumerable<PropertyInfo> UsableProperties(this Type type, bool nonPublic = false, bool skipItems = true)
		{
			BindingFlags bindingAttr = ((!nonPublic) ? (BindingFlags.Instance | BindingFlags.Public) : (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
			PropertyInfo[] properties = type.GetProperties(bindingAttr);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanWrite && (!skipItems || !(propertyInfo.Name == "Item")))
				{
					yield return propertyInfo;
				}
			}
		}

		public static IEnumerable<MemberInfo> UsableMembers(this Type type, bool nonPublic = false, bool skipItems = true)
		{
			BindingFlags flags = ((!nonPublic) ? (BindingFlags.Instance | BindingFlags.Public) : (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
			FieldInfo[] fields = type.GetFields(flags);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsLiteral && !fieldInfo.FieldType.IsPointer && !fieldInfo.IsNotSerialized)
				{
					yield return fieldInfo;
				}
			}
			PropertyInfo[] properties = type.GetProperties(flags);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanWrite && (!skipItems || !(propertyInfo.Name == "Item")))
				{
					yield return propertyInfo;
				}
			}
		}

		public static void PrintAllFields(this Type type, BindingFlags flags)
		{
			FieldInfo[] fields = type.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				Debug.Log(fields[i].Name + ", field, " + flags);
			}
			PropertyInfo[] properties = type.GetProperties(flags);
			for (int j = 0; j < properties.Length; j++)
			{
				Debug.Log(properties[j].Name + ", property, " + flags);
			}
			MethodInfo[] methods = type.GetMethods(flags);
			for (int k = 0; k < methods.Length; k++)
			{
				Debug.Log(methods[k].Name + ", method, " + flags);
			}
		}

		public static void PrintAllFields(this Type type)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			type.PrintAllFields(flags);
			flags = BindingFlags.Instance | BindingFlags.NonPublic;
			type.PrintAllFields(flags);
			flags = BindingFlags.Static | BindingFlags.Public;
			type.PrintAllFields(flags);
			flags = BindingFlags.Static | BindingFlags.NonPublic;
			type.PrintAllFields(flags);
		}

		public static Component CopyComponent(Component src, GameObject go)
		{
			Type type = src.GetType();
			Component component = go.GetComponent(src.GetType());
			if (component == null)
			{
				component = go.AddComponent(type);
			}
			foreach (FieldInfo item in type.UsableFields(nonPublic: true))
			{
				item.SetValue(component, item.GetValue(src));
			}
			foreach (PropertyInfo item2 in type.UsableProperties(nonPublic: true))
			{
				if (!(item2.Name == "name"))
				{
					try
					{
						item2.SetValue(component, item2.GetValue(src, null), null);
					}
					catch
					{
					}
				}
			}
			return component;
		}

		[Obsolete]
		public static IEnumerable<Type> SubtypesEnumerable(this Type parent)
		{
			Assembly assembly = Assembly.GetAssembly(parent);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsSubclassOf(parent) && !type.IsInterface && !type.IsAbstract)
				{
					yield return type;
				}
			}
		}

		public static Type[] Subtypes(this Type parent, bool allAssemblies = false, Predicate<Type> filter = null)
		{
			List<Type> list = new List<Type>();
			Assembly[] array = ((!allAssemblies) ? new Assembly[1] { Assembly.GetAssembly(parent) } : AppDomain.CurrentDomain.GetAssemblies());
			Assembly[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Type[] types = array2[i].GetTypes();
				foreach (Type type in types)
				{
					if (!type.IsInterface && !type.IsAbstract && (type.IsSubclassOf(parent) || parent.IsAssignableFrom(type)) && (filter == null || filter(type)))
					{
						list.Add(type);
					}
				}
			}
			return list.ToArray();
		}

		public static Type GetTerrainInspectorType()
		{
			return null;
		}

		public static object GetTerrainInspectorField(string fieldName, Type inspectorType = null)
		{
			if (inspectorType == null)
			{
				inspectorType = GetTerrainInspectorType();
			}
			object[] array = Resources.FindObjectsOfTypeAll(inspectorType);
			object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				object value = inspectorType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(array2[i], null);
				if (value != null)
				{
					return value;
				}
			}
			return null;
		}

		public static void SetTerrainInspectorField(string fieldName, object obj, Type inspectorType = null)
		{
			if (inspectorType == null)
			{
				inspectorType = GetTerrainInspectorType();
			}
			object[] array = Resources.FindObjectsOfTypeAll(inspectorType);
			object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				inspectorType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(array2[i], obj, null);
			}
		}

		public static int GetVersion<T>(this HashSet<T> hashSet)
		{
			return (int)typeof(HashSet<T>).GetField("_version", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(hashSet);
		}

		public static int GetVersion<TK, TV>(this Dictionary<TK, TV> dict)
		{
			return (int)typeof(Dictionary<TK, TV>).GetField("version", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dict);
		}

		public static T GetAddComponent<T>(this GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if (val == null)
			{
				val = go.AddComponent<T>();
			}
			return val;
		}

		public static void ReflectionReset<T>(this T obj)
		{
			Type type = obj.GetType();
			T val = (T)Activator.CreateInstance(type);
			foreach (FieldInfo item in type.UsableFields(nonPublic: true))
			{
				item.SetValue(obj, item.GetValue(val));
			}
			foreach (PropertyInfo item2 in type.UsableProperties(nonPublic: true))
			{
				item2.SetValue(obj, item2.GetValue(val, null), null);
			}
		}

		public static Dictionary<T, MethodInfo> GetAllMethodsWithAttribute<T>(Type baseTypeRef = null) where T : Attribute
		{
			Dictionary<T, MethodInfo> dictionary = new Dictionary<T, MethodInfo>();
			if (baseTypeRef == null)
			{
				baseTypeRef = typeof(ReflectionExtensions);
			}
			string fullName = baseTypeRef.Assembly.FullName;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				bool flag = false;
				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				for (int j = 0; j < referencedAssemblies.Length; j++)
				{
					if (referencedAssemblies[j].FullName == fullName)
					{
						flag = true;
						break;
					}
				}
				if (assembly.FullName == fullName)
				{
					flag = true;
				}
				if (!flag)
				{
					continue;
				}
				Type[] types = assembly.GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					MethodInfo[] methods = types[j].GetMethods(BindingFlags.Static | BindingFlags.Public);
					foreach (MethodInfo methodInfo in methods)
					{
						foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
						{
							if (customAttribute is T key)
							{
								if (dictionary.ContainsKey(key))
								{
									Debug.LogError("Editor method is defined twice. Attach to debug.");
								}
								else
								{
									dictionary.Add(key, methodInfo);
								}
							}
						}
					}
				}
			}
			return dictionary;
		}

		public static Dictionary<T, Type> GetAllTypesWithAttribute<T>(Type baseTypeRef = null) where T : Attribute
		{
			Dictionary<T, Type> dictionary = new Dictionary<T, Type>();
			if (baseTypeRef == null)
			{
				baseTypeRef = typeof(ReflectionExtensions);
			}
			string fullName = baseTypeRef.Assembly.FullName;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				bool flag = false;
				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				for (int j = 0; j < referencedAssemblies.Length; j++)
				{
					if (referencedAssemblies[j].FullName == fullName)
					{
						flag = true;
						break;
					}
				}
				if (assembly.FullName == fullName)
				{
					flag = true;
				}
				if (!flag)
				{
					continue;
				}
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					foreach (Attribute customAttribute in type.GetCustomAttributes())
					{
						if (customAttribute is T key && !dictionary.ContainsKey(key))
						{
							dictionary.Add(key, type);
						}
					}
				}
			}
			return dictionary;
		}
	}
}
