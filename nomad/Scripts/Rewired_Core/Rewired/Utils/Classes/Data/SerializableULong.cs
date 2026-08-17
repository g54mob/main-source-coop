using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	public class SerializableULong
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int ulong_32BitLow;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int ulong_32BitHigh;

		public ulong value
		{
			get
			{
				return XWjxflADWvRQkwuhJTaKhozvCdar(ulong_32BitLow, ulong_32BitHigh);
			}
			set
			{
				fkrnGIAqZJfmAFDvXomuaKSJOtVCB(value, out ulong_32BitLow, out ulong_32BitHigh);
			}
		}

		public SerializableULong()
		{
		}

		public SerializableULong(SerializableULong P_0)
		{
			ulong_32BitLow = P_0.ulong_32BitLow;
			ulong_32BitHigh = P_0.ulong_32BitHigh;
		}

		private void fkrnGIAqZJfmAFDvXomuaKSJOtVCB(ulong P_0, out int P_1, out int P_2)
		{
			P_1 = (int)P_0;
			P_2 = (int)(P_0 >> 32);
		}

		private ulong XWjxflADWvRQkwuhJTaKhozvCdar(int P_0, int P_1)
		{
			long num = P_0 & 0xFFFFFFFFu;
			ulong num2 = (ulong)((long)P_1 << 32);
			return (ulong)num | num2;
		}

		public SerializableULong Clone()
		{
			return new SerializableULong
			{
				ulong_32BitLow = ulong_32BitLow,
				ulong_32BitHigh = ulong_32BitHigh
			};
		}
	}
}
