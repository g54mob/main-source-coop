using System;

namespace Rewired.Utils
{
	public static class EnumTools
	{
		public static string GetName<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable
		{
			try
			{
				return Enum.GetName(typeof(TEnum), value);
			}
			catch
			{
				return null;
			}
		}

		public static bool ConvertByName<TEnumFrom, TEnumTo>(TEnumFrom convertFrom, out TEnumTo value) where TEnumFrom : struct, IComparable, IFormattable where TEnumTo : struct, IComparable, IFormattable
		{
			if (!ReflectionTools.IsEnum(typeof(TEnumFrom)))
			{
				throw new ArgumentException("TEnumFrom must be an enumerated type.");
			}
			if (!ReflectionTools.IsEnum(typeof(TEnumTo)))
			{
				throw new ArgumentException("TEnumTo must be an enumerated type.");
			}
			string[] names = Enum.GetNames(typeof(TEnumTo));
			int num = Array.IndexOf(names, convertFrom.ToString());
			if (num < 0)
			{
				value = default(TEnumTo);
				return false;
			}
			value = (TEnumTo)Enum.Parse(typeof(TEnumTo), names[num]);
			return true;
		}

		public static int[] GetIntValues(Type enumType)
		{
			return ArrayTools.ConvertToIntArray(Enum.GetValues(enumType));
		}

		public static bool IsEnum(Type type)
		{
			return ReflectionTools.IsEnum(type);
		}

		public static Type GetUnderlyingType(Type type)
		{
			return ReflectionTools.GetUnderlyingEnumType(type);
		}

		public static bool IsValidUnderlyingType(Type underlyingType)
		{
			if ((object)underlyingType != typeof(int) && (object)underlyingType != typeof(uint) && (object)underlyingType != typeof(byte) && (object)underlyingType != typeof(sbyte) && (object)underlyingType != typeof(short) && (object)underlyingType != typeof(ushort) && (object)underlyingType != typeof(long) && (object)underlyingType != typeof(ulong))
			{
				return false;
			}
			return true;
		}
	}
}
