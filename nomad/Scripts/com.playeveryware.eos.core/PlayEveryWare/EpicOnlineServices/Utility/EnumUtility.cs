using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	internal static class EnumUtility<TEnum> where TEnum : struct, Enum
	{
		public static bool TryParse(string enumValueString, IDictionary<string, TEnum> customMappings, out TEnum result, TEnum defaultValue = default(TEnum))
		{
			result = defaultValue;
			if (string.IsNullOrEmpty(enumValueString))
			{
				return true;
			}
			if (Enum.TryParse<TEnum>(enumValueString, out result))
			{
				return true;
			}
			if (customMappings.TryGetValue(enumValueString, out result))
			{
				return true;
			}
			Debug.LogWarning("\"" + enumValueString + "\" was not recognized as a valid TEnum value, and parsing failed.");
			return false;
		}

		public static bool TryParse(IList<string> enumValuesAsStrings, IDictionary<string, TEnum> customMappings, out TEnum result, TEnum defaultValue = default(TEnum))
		{
			result = defaultValue;
			foreach (string enumValuesAsString in enumValuesAsStrings)
			{
				if (!TryParse(enumValuesAsString, customMappings, out var result2, defaultValue))
				{
					result = defaultValue;
					return false;
				}
				result = Combine(result, result2);
			}
			return true;
		}

		private static TEnum Combine(TEnum current, TEnum toAdd)
		{
			Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			if (underlyingType == typeof(int))
			{
				int num = Convert.ToInt32(current);
				int num2 = Convert.ToInt32(toAdd);
				int value = num | num2;
				return (TEnum)Enum.ToObject(typeof(TEnum), value);
			}
			if (underlyingType == typeof(uint))
			{
				uint num3 = Convert.ToUInt32(current);
				uint num4 = Convert.ToUInt32(toAdd);
				uint value2 = num3 | num4;
				return (TEnum)Enum.ToObject(typeof(TEnum), value2);
			}
			if (underlyingType == typeof(long))
			{
				long num5 = Convert.ToInt64(current);
				long num6 = Convert.ToInt64(toAdd);
				long value3 = num5 | num6;
				return (TEnum)Enum.ToObject(typeof(TEnum), value3);
			}
			if (underlyingType == typeof(ulong))
			{
				ulong num7 = Convert.ToUInt64(current);
				ulong num8 = Convert.ToUInt64(toAdd);
				ulong value4 = num7 | num8;
				return (TEnum)Enum.ToObject(typeof(TEnum), value4);
			}
			throw new ArgumentException("Unsupported enum underlying type: \"" + underlyingType.FullName + ".\"");
		}

		public static IEnumerable<TEnum> GetEnumerator(TEnum bitFlag)
		{
			IList<TEnum> list = new List<TEnum>();
			ulong num = Convert.ToUInt64(default(TEnum));
			foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
			{
				if (num != Convert.ToUInt64(value) && bitFlag.HasFlag(value))
				{
					list.Add(value);
				}
			}
			return list;
		}

		public static TEnum GetLowest()
		{
			return GetExtreme((object current, object extreme) => Comparer<object>.Default.Compare(current, extreme) < 0);
		}

		public static TEnum GetHighest()
		{
			return GetExtreme((object current, object extreme) => Comparer<object>.Default.Compare(current, extreme) > 0);
		}

		private static TEnum GetExtreme(Func<object, object, bool> comparison)
		{
			object obj = null;
			foreach (object value in Enum.GetValues(typeof(TEnum)))
			{
				if (obj == null || comparison(value, obj))
				{
					obj = value;
				}
			}
			if (obj != null)
			{
				return (TEnum)Enum.ToObject(typeof(TEnum), obj);
			}
			return default(TEnum);
		}
	}
}
