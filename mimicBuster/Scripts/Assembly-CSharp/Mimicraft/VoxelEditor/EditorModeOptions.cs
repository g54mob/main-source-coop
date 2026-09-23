using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class EditorModeOptions
	{
		private static class Slot<T> where T : struct, Enum
		{
			public static ulong Withheld;

			public static readonly T[] Values;

			static Slot()
			{
				Values = (T[])Enum.GetValues(typeof(T));
				Resets.Add(delegate
				{
					Withheld = 0uL;
				});
			}
		}

		public const string AxisSpace = "Transform.AxisSpace";

		private static readonly List<Action> Resets = new List<Action>();

		private static readonly HashSet<string> withheldSwitches = new HashSet<string>();

		private static ulong Bit<T>(T value) where T : struct, Enum
		{
			int num = UnsafeUtility.EnumToInt(value);
			if (num < 0 || num >= 64)
			{
				return 0uL;
			}
			return (ulong)(1L << num);
		}

		public static void SetOffered<T>(T value, bool offered) where T : struct, Enum
		{
			ulong num = Bit(value);
			if (offered)
			{
				Slot<T>.Withheld &= ~num;
			}
			else
			{
				Slot<T>.Withheld |= num;
			}
		}

		public static bool IsOffered<T>(T value) where T : struct, Enum
		{
			return (Slot<T>.Withheld & Bit(value)) == 0;
		}

		public static T Next<T>(T current) where T : struct, Enum
		{
			T[] values = Slot<T>.Values;
			int num = Array.IndexOf(values, current);
			if (num < 0)
			{
				return current;
			}
			for (int i = 1; i < values.Length; i++)
			{
				T val = values[(num + i) % values.Length];
				if (IsOffered(val))
				{
					return val;
				}
			}
			return current;
		}

		public static T Resolve<T>(T current) where T : struct, Enum
		{
			if (IsOffered(current))
			{
				return current;
			}
			T[] values = Slot<T>.Values;
			foreach (T val in values)
			{
				if (IsOffered(val))
				{
					return val;
				}
			}
			return current;
		}

		public static void SetSwitchOffered(string id, bool offered)
		{
			if (offered)
			{
				withheldSwitches.Remove(id);
			}
			else
			{
				withheldSwitches.Add(id);
			}
		}

		public static bool IsSwitchOffered(string id)
		{
			return !withheldSwitches.Contains(id);
		}

		public static void ResetAll()
		{
			foreach (Action reset in Resets)
			{
				reset();
			}
			withheldSwitches.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			ResetAll();
		}
	}
}
