using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class MiscTools
	{
		private static uint XZlzXqLyvxgaufeaWuWImsKyCQMJ = 0u;

		private static uint CPFpEXDiCzKnVdTPkYWaohANgmed = 1u;

		private static uint sbMjeMbrBQXUIlGTrpNeWkGucHbtA = uint.MaxValue;

		private static int AdXBJgPfcOyGikQevpXsRcULbtWg = 0;

		private static int QITAqHdWCZXpFQIjQrkxBmmrdpFw = int.MaxValue;

		private static int uFxfBTgRidXjPEgqgoEMQiGwieV = -1;

		public static object Clone(object obj)
		{
			if (!(obj is ICloneable))
			{
				return null;
			}
			return (obj as ICloneable).Clone();
		}

		public static T Clone<T>(T obj) where T : class, ICloneable
		{
			if (obj == null)
			{
				return null;
			}
			return obj.Clone() as T;
		}

		public static T DeepClone<T>(T obj) where T : class, IDeepCloneable
		{
			if (obj == null)
			{
				return null;
			}
			return obj.DeepClone() as T;
		}

		public static T DeepClone<T>(T obj, bool createIfNull) where T : class, IDeepCloneable, new()
		{
			if (obj == null)
			{
				return new T();
			}
			return obj.DeepClone() as T;
		}

		public static T[] DeepClone<T>(T[] obj) where T : class, IDeepCloneable
		{
			if (obj == null)
			{
				return null;
			}
			T[] array = new T[obj.Length];
			for (int i = 0; i < obj.Length; i++)
			{
				array[i] = DeepClone(obj[i]);
			}
			return array;
		}

		public static List<T> DeepClone<T>(List<T> obj) where T : class, IDeepCloneable
		{
			if (obj == null)
			{
				return null;
			}
			List<T> list = new List<T>(obj.Count);
			for (int i = 0; i < obj.Count; i++)
			{
				list.Add(DeepClone(obj[i]));
			}
			return list;
		}

		public static Dictionary<TKey, TValue> DeepClone<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TValue : class, IDeepCloneable
		{
			if (dictionary == null)
			{
				return null;
			}
			Dictionary<TKey, TValue> dictionary2 = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				dictionary2.Add(item.Key, DeepClone(item.Value));
			}
			return dictionary2;
		}

		public static Guid CreateGuidHashSHA256(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Guid.Empty;
			}
			byte[] sourceArray = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(text));
			byte[] array = new byte[16];
			Array.Copy(sourceArray, array, 16);
			return new Guid(array);
		}

		public static Guid CreateGuidHashSHA1(string text)
		{
			using SHA1 sHA = SHA1.Create();
			byte[] sourceArray = sHA.ComputeHash(Encoding.UTF8.GetBytes(text));
			byte[] array = new byte[16];
			Array.Copy(sourceArray, array, 16);
			return new Guid(array);
		}

		public static Guid CreateHIDProductGuid(int vendorId, int productId)
		{
			return new Guid(((ushort)productId).ToString("x4") + ((ushort)vendorId).ToString("x4") + "-0000-0000-0000-504944564944");
		}

		public static uint Tick(uint counter)
		{
			counter = ((counter != sbMjeMbrBQXUIlGTrpNeWkGucHbtA) ? (counter + 1) : CPFpEXDiCzKnVdTPkYWaohANgmed);
			return counter;
		}

		public static int Tick(int counter)
		{
			counter = ((counter != QITAqHdWCZXpFQIjQrkxBmmrdpFw) ? (counter + 1) : AdXBJgPfcOyGikQevpXsRcULbtWg);
			return counter;
		}

		public static uint TickPrev(uint counter)
		{
			counter = ((counter != CPFpEXDiCzKnVdTPkYWaohANgmed) ? (counter - 1) : sbMjeMbrBQXUIlGTrpNeWkGucHbtA);
			return counter;
		}

		public static int TickPrev(int counter)
		{
			if (counter <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return QITAqHdWCZXpFQIjQrkxBmmrdpFw;
			}
			counter = ((counter != AdXBJgPfcOyGikQevpXsRcULbtWg) ? (counter - 1) : QITAqHdWCZXpFQIjQrkxBmmrdpFw);
			return counter;
		}

		public static bool IsTickValid(uint tick)
		{
			return tick != XZlzXqLyvxgaufeaWuWImsKyCQMJ;
		}

		public static bool IsTickValid(int tick)
		{
			return tick > uFxfBTgRidXjPEgqgoEMQiGwieV;
		}

		public static bool IsTickNewer(uint tick1, uint tick2)
		{
			if (tick1 == tick2)
			{
				return false;
			}
			if (tick1 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return false;
			}
			if (tick2 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return true;
			}
			if (tick1 < tick2)
			{
				if (tick2 - tick1 < 2147483648u)
				{
					return false;
				}
			}
			else if (tick1 > tick2 && tick1 - tick2 > 2147483648u)
			{
				return false;
			}
			return true;
		}

		public static bool IsTickNewer(int tick1, int tick2)
		{
			if (tick1 == tick2)
			{
				return false;
			}
			if (tick1 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return false;
			}
			if (tick2 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return true;
			}
			if (tick1 < tick2)
			{
				if (tick2 - tick1 < 1073741823)
				{
					return false;
				}
			}
			else if (tick1 > tick2 && tick1 - tick2 > 1073741823)
			{
				return false;
			}
			return true;
		}

		public static bool IsTickNewerOrEqualTo(uint tick1, uint tick2)
		{
			if (tick1 == tick2)
			{
				return true;
			}
			if (tick1 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return false;
			}
			if (tick2 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return true;
			}
			if (tick1 < tick2)
			{
				if (tick2 - tick1 < 2147483648u)
				{
					return false;
				}
			}
			else if (tick1 > tick2 && tick1 - tick2 > 2147483648u)
			{
				return false;
			}
			return true;
		}

		public static bool IsTickNewerOrEqualTo(int tick1, int tick2)
		{
			if (tick1 == tick2)
			{
				return true;
			}
			if (tick1 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return false;
			}
			if (tick2 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return true;
			}
			if (tick1 < tick2)
			{
				if (tick2 - tick1 < 1073741823)
				{
					return false;
				}
			}
			else if (tick1 > tick2 && tick1 - tick2 > 1073741823)
			{
				return false;
			}
			return true;
		}

		public static long TickDifference(uint tick1, uint tick2)
		{
			if (tick1 == tick2)
			{
				return 0L;
			}
			if (tick1 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return 0L;
			}
			if (tick2 == XZlzXqLyvxgaufeaWuWImsKyCQMJ)
			{
				return 0L;
			}
			uint num;
			uint num2;
			if (tick1 < tick2)
			{
				num = tick2;
				num2 = tick1;
			}
			else
			{
				num = tick1;
				num2 = tick2;
			}
			if (num - num2 < 2147483648u)
			{
				return (long)tick1 - (long)tick2;
			}
			uint num3 = sbMjeMbrBQXUIlGTrpNeWkGucHbtA - num + num2;
			uint cPFpEXDiCzKnVdTPkYWaohANgmed = CPFpEXDiCzKnVdTPkYWaohANgmed;
			uint num4 = num3 - cPFpEXDiCzKnVdTPkYWaohANgmed;
			if (tick1 >= tick2)
			{
				return 0L - (long)num4;
			}
			return num4;
		}

		public static int TickDifference(int tick1, int tick2)
		{
			if (tick1 == tick2)
			{
				return 0;
			}
			if (tick1 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return 0;
			}
			if (tick2 <= uFxfBTgRidXjPEgqgoEMQiGwieV)
			{
				return 0;
			}
			int num;
			int num2;
			if (tick1 < tick2)
			{
				num = tick2;
				num2 = tick1;
			}
			else
			{
				num = tick1;
				num2 = tick2;
			}
			if (num - num2 < 1073741823)
			{
				return tick1 - tick2;
			}
			int num3 = QITAqHdWCZXpFQIjQrkxBmmrdpFw - num + num2;
			int adXBJgPfcOyGikQevpXsRcULbtWg = AdXBJgPfcOyGikQevpXsRcULbtWg;
			int num4 = num3 - adXBJgPfcOyGikQevpXsRcULbtWg;
			if (tick1 >= tick2)
			{
				return -num4;
			}
			return num4;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T val = a;
			a = b;
			b = val;
		}

		public static long ToLongUnchecked(object value)
		{
			if (value is int)
			{
				return (int)value;
			}
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is byte)
			{
				return (byte)value;
			}
			if (value is sbyte)
			{
				return (sbyte)value;
			}
			if (value is short)
			{
				return (short)value;
			}
			if (value is ushort)
			{
				return (ushort)value;
			}
			if (value is long)
			{
				return (long)value;
			}
			if (value is ulong)
			{
				return (long)(ulong)value;
			}
			if (value is float)
			{
				return (long)(float)value;
			}
			if (value is double)
			{
				return (long)(double)value;
			}
			if (value is decimal)
			{
				return (long)(decimal)value;
			}
			throw new ArgumentException("value must be an integral type (excluding char).");
		}

		public static bool IsValidGuid(string guid)
		{
			try
			{
				new Guid(guid);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
