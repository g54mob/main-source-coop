using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class EnumNameValueCache<TEnum> where TEnum : struct, IFormattable, IComparable
	{
		private static EnumNameValueCache<TEnum> ZdvtDIAlSFybwRVQSofHcnvOxpry;

		private readonly ADictionary<string, TEnum> tgCUUCAoSxgMHCkWUaZcGbnabjsI;

		private readonly string[] KSDfXUqAewnkJWAVFIkSbajHPmY;

		private readonly long[] kovwpNGemwJXebpStuNyMURpTaMC;

		public static EnumNameValueCache<TEnum> Default => ZdvtDIAlSFybwRVQSofHcnvOxpry ?? (ZdvtDIAlSFybwRVQSofHcnvOxpry = new EnumNameValueCache<TEnum>());

		public int Count => kovwpNGemwJXebpStuNyMURpTaMC.Length;

		public static void Free()
		{
			ZdvtDIAlSFybwRVQSofHcnvOxpry = null;
		}

		private EnumNameValueCache()
		{
			Type typeFromHandle = typeof(TEnum);
			if (!EnumTools.IsEnum(typeFromHandle))
			{
				throw new Exception("enumType is not an enum type.");
			}
			Type underlyingEnumType = ReflectionTools.GetUnderlyingEnumType(typeFromHandle);
			KSDfXUqAewnkJWAVFIkSbajHPmY = Enum.GetNames(typeFromHandle);
			TEnum[] array = (TEnum[])Enum.GetValues(typeFromHandle);
			tgCUUCAoSxgMHCkWUaZcGbnabjsI = new ADictionary<string, TEnum>();
			kovwpNGemwJXebpStuNyMURpTaMC = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				kovwpNGemwJXebpStuNyMURpTaMC[i] = MiscTools.ToLongUnchecked(Convert.ChangeType(array[i], underlyingEnumType));
				tgCUUCAoSxgMHCkWUaZcGbnabjsI.Add(KSDfXUqAewnkJWAVFIkSbajHPmY[i], array[i]);
			}
		}

		public TEnum GetValue(string name)
		{
			return tgCUUCAoSxgMHCkWUaZcGbnabjsI[name];
		}

		public bool TryGetValue(string name, out TEnum value)
		{
			return tgCUUCAoSxgMHCkWUaZcGbnabjsI.TryGetValue(name, out value);
		}

		public string GetName(long value)
		{
			int num = IndexOf(value);
			if (num < 0)
			{
				throw new Exception("The value does not exist in the enum.");
			}
			return KSDfXUqAewnkJWAVFIkSbajHPmY[num];
		}

		public bool TryGetName(long value, out string name)
		{
			int num = IndexOf(value);
			if (num < 0)
			{
				name = string.Empty;
				return false;
			}
			name = KSDfXUqAewnkJWAVFIkSbajHPmY[num];
			return true;
		}

		public TEnum GetValueAt(int index)
		{
			if ((uint)index >= (uint)kovwpNGemwJXebpStuNyMURpTaMC.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return tgCUUCAoSxgMHCkWUaZcGbnabjsI[KSDfXUqAewnkJWAVFIkSbajHPmY[index]];
		}

		public string GetNameAt(int index)
		{
			if ((uint)index >= (uint)kovwpNGemwJXebpStuNyMURpTaMC.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return KSDfXUqAewnkJWAVFIkSbajHPmY[index];
		}

		public int IndexOf(string name)
		{
			return Array.IndexOf(KSDfXUqAewnkJWAVFIkSbajHPmY, name);
		}

		public int IndexOf(long value)
		{
			return Array.IndexOf(kovwpNGemwJXebpStuNyMURpTaMC, value);
		}

		public bool Contains(string name)
		{
			return tgCUUCAoSxgMHCkWUaZcGbnabjsI.ContainsKey(name);
		}

		public bool Contains(long value)
		{
			return IndexOf(value) >= 0;
		}
	}
}
