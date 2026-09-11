using System;
using System.Collections.Generic;

namespace MonoMod.Utils
{
	public class GenericTypeInstantiationComparer : IEqualityComparer<Type>
	{
		private static Type CannonicalFillType = GenericMethodInstantiationComparer.CannonicalFillType;

		public bool Equals(Type x, Type y)
		{
			if ((object)x == null && (object)y == null)
			{
				return true;
			}
			if ((object)x == null || (object)y == null)
			{
				return false;
			}
			bool isGenericType = x.IsGenericType;
			bool isGenericType2 = y.IsGenericType;
			if (isGenericType != isGenericType2)
			{
				return false;
			}
			if (!isGenericType)
			{
				return x.Equals(y);
			}
			Type genericTypeDefinition = x.GetGenericTypeDefinition();
			Type genericTypeDefinition2 = y.GetGenericTypeDefinition();
			if (!genericTypeDefinition.Equals(genericTypeDefinition2))
			{
				return false;
			}
			Type[] genericArguments = x.GetGenericArguments();
			Type[] genericArguments2 = y.GetGenericArguments();
			if (genericArguments.Length != genericArguments2.Length)
			{
				return false;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				Type type = genericArguments[i];
				Type type2 = genericArguments2[i];
				if (!type.IsValueType)
				{
					type = CannonicalFillType ?? typeof(object);
				}
				if (!type2.IsValueType)
				{
					type2 = CannonicalFillType ?? typeof(object);
				}
				if (!Equals(type, type2))
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(Type type)
		{
			if (!type.IsGenericType)
			{
				return type.GetHashCode();
			}
			int num = -559038737;
			num ^= (num << 16) | (num >> 16);
			num ^= type.Assembly.GetHashCode();
			if (type.Namespace != null)
			{
				num ^= MultiTargetShims.GetHashCode(type.Namespace, StringComparison.Ordinal);
			}
			num ^= MultiTargetShims.GetHashCode(type.Name, StringComparison.Ordinal);
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				int num2 = i % 8 * 4;
				Type type2 = genericArguments[i];
				int num3 = (type2.IsValueType ? GetHashCode(type2) : (CannonicalFillType?.GetHashCode() ?? (-1717986919)));
				num ^= (num3 << num2) | (num3 >> 32 - num2);
			}
			return num;
		}
	}
}
