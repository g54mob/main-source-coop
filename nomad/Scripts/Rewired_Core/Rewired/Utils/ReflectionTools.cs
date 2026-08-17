using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	public static class ReflectionTools
	{
		[Flags]
		public enum BindingFlags
		{
			IgnoreCase = 1,
			DeclaredOnly = 2,
			Instance = 4,
			Static = 8,
			Public = 0x10,
			NonPublic = 0x20,
			FlattenHierarchy = 0x40
		}

		public static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		public static bool IsEnum(Type type)
		{
			if (type == null)
			{
				return false;
			}
			return type.IsEnum;
		}

		public static Type GetUnderlyingEnumType(Type enumType)
		{
			if (enumType == null)
			{
				return null;
			}
			if (!IsEnum(enumType))
			{
				return null;
			}
			return Enum.GetUnderlyingType(enumType);
		}

		public static bool IsClass(Type type)
		{
			return type.IsClass;
		}

		public static bool IsPrimitive(Type type)
		{
			return type.IsPrimitive;
		}

		public static bool IsArray(Type type)
		{
			return type.IsArray;
		}

		public static bool DoesTypeImplement(Type type, Type baseOrInterfaceType)
		{
			return baseOrInterfaceType.IsAssignableFrom(type);
		}

		public static bool IsGenericType(Type type)
		{
			if (type == null)
			{
				return false;
			}
			return type.IsGenericType;
		}

		public static Type[] GetGenericArguments(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetGenericArguments();
		}

		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetFields();
		}

		public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetFields((System.Reflection.BindingFlags)bindingFlags);
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetProperties();
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetProperties((System.Reflection.BindingFlags)bindingFlags);
		}

		public static IEnumerable<MethodInfo> GetMethods(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetMethods();
		}

		public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetMethods((System.Reflection.BindingFlags)bindingFlags);
		}

		public static bool IsDefined(Type type, Type attributeType, bool inherit)
		{
			if (type == null || attributeType == null)
			{
				return false;
			}
			return type.IsDefined(attributeType, inherit);
		}

		public static T GetAttribute<T>(Type type, bool inherit) where T : Attribute
		{
			if (type == null)
			{
				return null;
			}
			try
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
				if (customAttributes == null || customAttributes.Length == 0)
				{
					return null;
				}
				return customAttributes[0] as T;
			}
			catch
			{
				return null;
			}
		}

		internal static bool IsAssemblyLoaded(string assemblyName, bool useShortName, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(assemblyName))
			{
				return false;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies == null)
			{
				return false;
			}
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (ignoreCase)
				{
					if (useShortName)
					{
						if (assemblies[i].GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
					else if (assemblies[i].FullName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				else if (useShortName)
				{
					if (assemblies[i].GetName().Name.Equals(assemblyName))
					{
						return true;
					}
				}
				else if (assemblies[i].FullName.Equals(assemblyName))
				{
					return true;
				}
			}
			return false;
		}

		internal static Type GetTypeInUnityEditorAssembly(string classPath, bool ignoreCase = false)
		{
			return BSlEPUHtLVWqEkmrtxBclfPTnmScA(classPath, true, ignoreCase);
		}

		internal static Type GetTypeInUnityBuildAssembly(string classPath, bool ignoreCase = false)
		{
			return BSlEPUHtLVWqEkmrtxBclfPTnmScA(classPath, false, ignoreCase);
		}

		private static Type BSlEPUHtLVWqEkmrtxBclfPTnmScA(string P_0, bool P_1, bool P_2 = false)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type type = assemblies[i].GetType(P_0, throwOnError: false, P_2);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		internal static Type GetTypeInAssembly(string classPath, string assemblyName, bool ignoreCase = false)
		{
			return Type.GetType(classPath + ", " + assemblyName + ", Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", throwOnError: false, ignoreCase);
		}

		public static TRet GetPrivateField<T, TRet>(T obj, string name)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			return (TRet)typeof(T).GetField(name, (System.Reflection.BindingFlags)bindingAttr).GetValue(obj);
		}

		public static TRet GetPrivateProperty<T, TRet>(T obj, string name)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			return (TRet)typeof(T).GetProperty(name, (System.Reflection.BindingFlags)bindingAttr).GetValue(obj, null);
		}

		public static void SetPrivateField<T>(T obj, string name, object value)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			typeof(T).GetField(name, (System.Reflection.BindingFlags)bindingAttr).SetValue(obj, value);
		}

		public static void SetPrivateProperty<T>(T obj, string name, object value)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			typeof(T).GetProperty(name, (System.Reflection.BindingFlags)bindingAttr).SetValue(obj, value, null);
		}

		public static TRet CallPrivateMethod<T, TRet>(T obj, string name, params object[] param)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			return (TRet)typeof(T).GetMethod(name, (System.Reflection.BindingFlags)bindingAttr).Invoke(obj, param);
		}

		public static MethodInfo GetMethodInfo(Delegate @delegate)
		{
			return @delegate?.Method;
		}
	}
}
