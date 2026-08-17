using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QFSW.QC.Utilities
{
	public static class ReflectionExtensions
	{
		private static readonly Dictionary<Type, string> _typeDisplayNames = new Dictionary<Type, string>
		{
			{
				typeof(int),
				"int"
			},
			{
				typeof(float),
				"float"
			},
			{
				typeof(decimal),
				"decimal"
			},
			{
				typeof(double),
				"double"
			},
			{
				typeof(string),
				"string"
			},
			{
				typeof(bool),
				"bool"
			},
			{
				typeof(byte),
				"byte"
			},
			{
				typeof(sbyte),
				"sbyte"
			},
			{
				typeof(uint),
				"uint"
			},
			{
				typeof(short),
				"short"
			},
			{
				typeof(ushort),
				"ushort"
			},
			{
				typeof(long),
				"decimal"
			},
			{
				typeof(ulong),
				"ulong"
			},
			{
				typeof(char),
				"char"
			},
			{
				typeof(object),
				"object"
			}
		};

		private static readonly Type[] _valueTupleTypes = new Type[8]
		{
			typeof(ValueTuple<>),
			typeof(ValueTuple<, >),
			typeof(ValueTuple<, , >),
			typeof(ValueTuple<, , , >),
			typeof(ValueTuple<, , , , >),
			typeof(ValueTuple<, , , , , >),
			typeof(ValueTuple<, , , , , , >),
			typeof(ValueTuple<, , , , , , , >)
		};

		private static readonly Type[][] _primitiveTypeCastHierarchy = new Type[6][]
		{
			new Type[3]
			{
				typeof(byte),
				typeof(sbyte),
				typeof(char)
			},
			new Type[2]
			{
				typeof(short),
				typeof(ushort)
			},
			new Type[2]
			{
				typeof(int),
				typeof(uint)
			},
			new Type[2]
			{
				typeof(long),
				typeof(ulong)
			},
			new Type[1] { typeof(float) },
			new Type[1] { typeof(double) }
		};

		public static bool IsDelegate(this Type type)
		{
			if (!typeof(Delegate).IsAssignableFrom(type))
			{
				return false;
			}
			return true;
		}

		public static bool IsStrongDelegate(this Type type)
		{
			if (!type.IsDelegate())
			{
				return false;
			}
			if (type.IsAbstract)
			{
				return false;
			}
			return true;
		}

		public static bool IsDelegate(this FieldInfo fieldInfo)
		{
			return fieldInfo.FieldType.IsDelegate();
		}

		public static bool IsStrongDelegate(this FieldInfo fieldInfo)
		{
			return fieldInfo.FieldType.IsStrongDelegate();
		}

		public static bool IsGenericTypeOf(this Type genericType, Type nonGenericType)
		{
			if (!genericType.IsGenericType)
			{
				return false;
			}
			return genericType.GetGenericTypeDefinition() == nonGenericType;
		}

		public static bool IsDerivedTypeOf(this Type type, Type baseType)
		{
			return baseType.IsAssignableFrom(type);
		}

		public static bool IsCastableTo(this Type from, Type to, bool implicitly = false)
		{
			if (!to.IsAssignableFrom(from))
			{
				return from.HasCastDefined(to, implicitly);
			}
			return true;
		}

		private static bool HasCastDefined(this Type from, Type to, bool implicitly)
		{
			if ((from.IsPrimitive || from.IsEnum) && (to.IsPrimitive || to.IsEnum))
			{
				if (!implicitly)
				{
					if (!(from == to))
					{
						if (from != typeof(bool))
						{
							return to != typeof(bool);
						}
						return false;
					}
					return true;
				}
				IEnumerable<Type> enumerable = Enumerable.Empty<Type>();
				Type[][] primitiveTypeCastHierarchy = _primitiveTypeCastHierarchy;
				foreach (Type[] array in primitiveTypeCastHierarchy)
				{
					if (array.Any((Type t) => t == to))
					{
						return enumerable.Any((Type t) => t == from);
					}
					enumerable = enumerable.Concat(array);
				}
				return false;
			}
			if (!IsCastDefined(to, (MethodInfo m) => m.GetParameters()[0].ParameterType, (MethodInfo _) => from, implicitly, lookInBase: false))
			{
				return IsCastDefined(from, (MethodInfo _) => to, (MethodInfo m) => m.ReturnType, implicitly, lookInBase: true);
			}
			return true;
		}

		private static bool IsCastDefined(Type type, Func<MethodInfo, Type> baseType, Func<MethodInfo, Type> derivedType, bool implicitly, bool lookInBase)
		{
			BindingFlags bindingAttr = (BindingFlags)(0x18 | (lookInBase ? 64 : 2));
			return (from m in type.GetMethods(bindingAttr)
				where m.Name == "op_Implicit" || (!implicitly && m.Name == "op_Explicit")
				select m).Any((MethodInfo m) => baseType(m).IsAssignableFrom(derivedType(m)));
		}

		public static object Cast(this Type type, object data)
		{
			if (type.IsInstanceOfType(data))
			{
				return data;
			}
			try
			{
				return Convert.ChangeType(data, type);
			}
			catch (InvalidCastException)
			{
				Type type2 = data.GetType();
				ParameterExpression parameterExpression = Expression.Parameter(type2, "data");
				return Expression.Lambda(Expression.Convert(Expression.Convert(parameterExpression, type2), type), parameterExpression).Compile().DynamicInvoke(data);
			}
		}

		public static bool IsOverride(this MethodInfo methodInfo)
		{
			return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
		}

		public static bool HasAttribute<T>(this ICustomAttributeProvider provider, bool searchInherited = true) where T : Attribute
		{
			try
			{
				return provider.IsDefined(typeof(T), searchInherited);
			}
			catch (MissingMethodException)
			{
				return false;
			}
		}

		public static string GetDisplayName(this Type type, bool includeNamespace = false)
		{
			if (type.IsGenericParameter)
			{
				return type.Name;
			}
			if (type.IsArray)
			{
				int arrayRank = type.GetArrayRank();
				return type.GetElementType().GetDisplayName(includeNamespace) + "[" + new string(',', arrayRank - 1) + "]";
			}
			if (_typeDisplayNames.ContainsKey(type))
			{
				string text = _typeDisplayNames[type];
				if (type.IsGenericType && !type.IsConstructedGenericType)
				{
					Type[] genericArguments = type.GetGenericArguments();
					return text + "<" + new string(',', genericArguments.Length - 1) + ">";
				}
				return text;
			}
			if (type.IsGenericTypeOf(typeof(Nullable<>)))
			{
				return type.GetGenericArguments()[0].GetDisplayName() + "?";
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				Type[] genericArguments2 = type.GetGenericArguments();
				if (_valueTupleTypes.Contains(genericTypeDefinition))
				{
					return type.GetTupleDisplayName(includeNamespace);
				}
				if (type.IsConstructedGenericType)
				{
					string[] array = new string[genericArguments2.Length];
					for (int i = 0; i < genericArguments2.Length; i++)
					{
						array[i] = genericArguments2[i].GetDisplayName(includeNamespace);
					}
					return genericTypeDefinition.GetDisplayName(includeNamespace).Split('<')[0] + "<" + string.Join(", ", array) + ">";
				}
				return (includeNamespace ? type.FullName : type.Name).Split('`')[0] + "<" + new string(',', genericArguments2.Length - 1) + ">";
			}
			Type declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				return declaringType.GetDisplayName(includeNamespace) + "." + type.Name;
			}
			if (!includeNamespace)
			{
				return type.Name;
			}
			return type.FullName;
		}

		private static string GetTupleDisplayName(this Type type, bool includeNamespace = false)
		{
			IEnumerable<string> values = from x in type.GetGenericArguments()
				select x.GetDisplayName(includeNamespace);
			return "(" + string.Join(", ", values) + ")";
		}

		public static bool AreMethodsEqual(MethodInfo a, MethodInfo b)
		{
			if (a.Name != b.Name)
			{
				return false;
			}
			ParameterInfo[] parameters = a.GetParameters();
			ParameterInfo[] parameters2 = b.GetParameters();
			if (parameters.Length != parameters2.Length)
			{
				return false;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				ParameterInfo parameterInfo2 = parameters2[i];
				if (parameterInfo.Name != parameterInfo2.Name)
				{
					return false;
				}
				if (parameterInfo.HasDefaultValue != parameterInfo2.HasDefaultValue)
				{
					return false;
				}
				Type parameterType = parameterInfo.ParameterType;
				Type parameterType2 = parameterInfo2.ParameterType;
				if (!parameterType.ContainsGenericParameters && !parameterType2.ContainsGenericParameters && parameterType != parameterType2)
				{
					return false;
				}
			}
			if (a.IsGenericMethod != b.IsGenericMethod)
			{
				return false;
			}
			if (a.IsGenericMethod && b.IsGenericMethod)
			{
				Type[] genericArguments = a.GetGenericArguments();
				Type[] genericArguments2 = b.GetGenericArguments();
				if (genericArguments.Length != genericArguments2.Length)
				{
					return false;
				}
				for (int j = 0; j < genericArguments.Length; j++)
				{
					Type obj = genericArguments[j];
					Type type = genericArguments2[j];
					if (obj.Name != type.Name)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static MethodInfo RebaseMethod(this MethodInfo method, Type newBase)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags = (BindingFlags)((int)bindingFlags | (method.IsStatic ? 8 : 4));
			bindingFlags = (BindingFlags)((int)bindingFlags | (method.IsPublic ? 16 : 32));
			MethodInfo[] array = (from x in newBase.GetMethods(bindingFlags)
				where AreMethodsEqual(x, method)
				select x).ToArray();
			if (array.Length == 0)
			{
				throw new ArgumentException($"Could not rebase method {method} onto type {newBase} as no matching candidates were found");
			}
			if (array.Length > 1)
			{
				throw new ArgumentException($"Could not rebase method {method} onto type {newBase} as too many matching candidates were found");
			}
			return array[0];
		}
	}
}
