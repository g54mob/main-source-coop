using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class EnumNameValueCache<TEnum> where TEnum : struct, IComparable, IFormattable
	{
		private static EnumNameValueCache<TEnum> WdZTcAjAdWIJKjVtqbsoPJYTZfFD;

		private readonly ADictionary<string, TEnum> dghiwidhDQBRmRHuiSOjXRRuMaHi;

		private readonly string[] hvUOaLPqpUHNuWSVBhLSdtYZLDTTA;

		private readonly long[] WHRdheAgeWNNNBWLrUQZyNuuArfiA;

		public static EnumNameValueCache<TEnum> Default => WdZTcAjAdWIJKjVtqbsoPJYTZfFD ?? (WdZTcAjAdWIJKjVtqbsoPJYTZfFD = new EnumNameValueCache<TEnum>());

		public int Count => WHRdheAgeWNNNBWLrUQZyNuuArfiA.Length;

		public static void Free()
		{
			WdZTcAjAdWIJKjVtqbsoPJYTZfFD = null;
		}

		private EnumNameValueCache()
		{
			Type typeFromHandle = typeof(TEnum);
			if (!EnumTools.IsEnum(typeFromHandle))
			{
				throw new Exception("enumType is not an enum type.");
			}
			Type underlyingEnumType = ReflectionTools.GetUnderlyingEnumType(typeFromHandle);
			hvUOaLPqpUHNuWSVBhLSdtYZLDTTA = Enum.GetNames(typeFromHandle);
			TEnum[] array = (TEnum[])Enum.GetValues(typeFromHandle);
			dghiwidhDQBRmRHuiSOjXRRuMaHi = new ADictionary<string, TEnum>();
			WHRdheAgeWNNNBWLrUQZyNuuArfiA = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				WHRdheAgeWNNNBWLrUQZyNuuArfiA[i] = MiscTools.ToLongUnchecked(Convert.ChangeType(array[i], underlyingEnumType));
				dghiwidhDQBRmRHuiSOjXRRuMaHi.Add(hvUOaLPqpUHNuWSVBhLSdtYZLDTTA[i], array[i]);
			}
		}

		public TEnum GetValue(string name)
		{
			return ((ADictionary<string, string>)(object)dghiwidhDQBRmRHuiSOjXRRuMaHi)[name];
		}

		public bool TryGetValue(string name, out TEnum value)
		{
			return dghiwidhDQBRmRHuiSOjXRRuMaHi.TryGetValue(name, out value);
		}

		public string GetName(long value)
		{
			int num = IndexOf(value);
			if (num < 0)
			{
				throw new Exception("The value does not exist in the enum.");
			}
			return hvUOaLPqpUHNuWSVBhLSdtYZLDTTA[num];
		}

		public bool TryGetName(long value, out string name)
		{
			int num = IndexOf(value);
			if (num < 0)
			{
				name = string.Empty;
				return false;
			}
			name = hvUOaLPqpUHNuWSVBhLSdtYZLDTTA[num];
			return true;
		}

		public TEnum GetValueAt(int index)
		{
			if ((uint)index >= (uint)WHRdheAgeWNNNBWLrUQZyNuuArfiA.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return ((ADictionary<string, string>)(object)dghiwidhDQBRmRHuiSOjXRRuMaHi)[hvUOaLPqpUHNuWSVBhLSdtYZLDTTA[index]];
		}

		public string GetNameAt(int index)
		{
			if ((uint)index >= (uint)WHRdheAgeWNNNBWLrUQZyNuuArfiA.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return hvUOaLPqpUHNuWSVBhLSdtYZLDTTA[index];
		}

		public int IndexOf(string name)
		{
			return Array.IndexOf(hvUOaLPqpUHNuWSVBhLSdtYZLDTTA, name);
		}

		public int IndexOf(long value)
		{
			return Array.IndexOf(WHRdheAgeWNNNBWLrUQZyNuuArfiA, value);
		}

		public bool Contains(string name)
		{
			return dghiwidhDQBRmRHuiSOjXRRuMaHi.ContainsKey(name);
		}

		public bool Contains(long value)
		{
			return IndexOf(value) >= 0;
		}
	}
}
