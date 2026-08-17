using System;
using System.Runtime.InteropServices;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal struct LowLevelInputEvent
	{
		private const int kHzXsFTuTHlpJRMehacWmLQXepzFA = 4;

		private const int EagtpasPfEmZfnkJjeBqJsyFcCBO = 8;

		private const int QBPkSmQLFRGExslAbDLMAkITVsnLA = 12;

		public const int buttonsPerPage = 32;

		public const int bytesPerButtonPage = 4;

		private const int sdzGGMNVwvABTjRpAzpGOopslygDA = 4;

		private const int sUHNwEuzxPocQIzIntjrHEMBkcds = 4;

		public const int byteIndex_id = 0;

		public const int byteIndex_timestamp = 4;

		public const int byteIndex_elementsStart = 12;

		public IntPtr _buffer;

		private int GpCEzgUyAMSIESspUMNbWeBUcSbAA;

		private int KomCWRdIyxDWChxnnpYJdxqUDQnY;

		private int uepwvXyOdsVRrbicQxSxHTyQLsEx;

		private int MSNUiyqXBkUrLMOiPgEbblnYBqopA;

		private int QHoIBgfTYGkzkHBqLeNanPzADRPcA;

		private int dxiKYlAGQbetwODjgxVSRKznZPbY;

		public bool isValid => _buffer != IntPtr.Zero;

		public int buttonCount => KomCWRdIyxDWChxnnpYJdxqUDQnY;

		public int axisCount => uepwvXyOdsVRrbicQxSxHTyQLsEx;

		public int byteIndex_axesStart => MSNUiyqXBkUrLMOiPgEbblnYBqopA;

		public int byteIndex_buttonsStart => QHoIBgfTYGkzkHBqLeNanPzADRPcA;

		public int byteIndex_hatsStart => dxiKYlAGQbetwODjgxVSRKznZPbY;

		public LowLevelInputEvent(IntPtr P_0, int P_1, int P_2, int P_3)
		{
			if (P_1 == 0 && P_2 == 0)
			{
				throw new ArgumentOutOfRangeException("No elements defined in event.");
			}
			_buffer = P_0;
			KomCWRdIyxDWChxnnpYJdxqUDQnY = P_1;
			uepwvXyOdsVRrbicQxSxHTyQLsEx = P_2;
			QHoIBgfTYGkzkHBqLeNanPzADRPcA = 12;
			MSNUiyqXBkUrLMOiPgEbblnYBqopA = QHoIBgfTYGkzkHBqLeNanPzADRPcA + ((P_1 > 0) ? (((P_1 - 1) / 32 + 1) * 4) : 0);
			dxiKYlAGQbetwODjgxVSRKznZPbY = MSNUiyqXBkUrLMOiPgEbblnYBqopA + P_2 * 4;
			GpCEzgUyAMSIESspUMNbWeBUcSbAA = GetReportSize(P_1, P_2, P_3);
		}

		public void SetButtonsBitMask(int bitMask, int startButtonIndex)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA > 0)
			{
				if (startButtonIndex % 32 != 0)
				{
					throw new Exception("startIndex must be divisible by 32.");
				}
				Marshal.WriteInt32(_buffer, QHoIBgfTYGkzkHBqLeNanPzADRPcA + startButtonIndex / 4, bitMask);
			}
		}

		public void SetAxisValue(int index, float value)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA > 0)
			{
				Marshal.WriteInt32(_buffer, MSNUiyqXBkUrLMOiPgEbblnYBqopA + index * 4, new eZPRuEYHSdKzxpOIPYwKrgFSfSJe(value).PBLXOQoRhflJpgYLYUieWNvLAFyP);
			}
		}

		public void SetId(uint id)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA > 0)
			{
				Marshal.WriteInt32(_buffer, 0, (int)id);
			}
		}

		public void SetTimestamp(double value)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA > 0)
			{
				Marshal.WriteInt64(_buffer, 4, new fsBaanevhcEZHeKpiHKQuYbEkuyhB(value).BSNWLedmGYtFflskBftpiZNHlXMAA);
			}
		}

		public bool GetButtonValue(int index)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA <= 0)
			{
				return false;
			}
			if (buttonCount == 0)
			{
				return false;
			}
			int num = index / 32;
			int num2 = (index - num * 32) / 8;
			int num3 = index % 8;
			return (Marshal.ReadByte(_buffer, QHoIBgfTYGkzkHBqLeNanPzADRPcA + num * 4 + num2) & (1 << num3)) != 0;
		}

		public int GetButtonsBitMask(int startButtonIndex)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA <= 0)
			{
				return 0;
			}
			if (startButtonIndex % 32 != 0)
			{
				throw new Exception("startIndex must be divisible by 32.");
			}
			return Marshal.ReadInt32(_buffer, QHoIBgfTYGkzkHBqLeNanPzADRPcA + startButtonIndex / 4);
		}

		public float GetAxisValue(int index)
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA <= 0)
			{
				return 0f;
			}
			return new eZPRuEYHSdKzxpOIPYwKrgFSfSJe(Marshal.ReadInt32(_buffer, MSNUiyqXBkUrLMOiPgEbblnYBqopA + index * 4)).nmZvzWjziPhRkzpXIMlOnltAnufg;
		}

		public uint GetId()
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA <= 0)
			{
				return 0u;
			}
			return (uint)Marshal.ReadInt32(_buffer, 0);
		}

		public double GetTimestamp()
		{
			if (GpCEzgUyAMSIESspUMNbWeBUcSbAA <= 0)
			{
				return 0.0;
			}
			return new fsBaanevhcEZHeKpiHKQuYbEkuyhB(Marshal.ReadInt64(_buffer, 4)).SykWHcNMqalPQuWyOPOeQpGizRfB;
		}

		public static int GetReportSize(int buttonCount, int axisCount, int hatCount)
		{
			return 12 + ((buttonCount > 0) ? (((buttonCount - 1) / 32 + 1) * 4) : 0) + axisCount * 4 + hatCount * 4;
		}
	}
}
