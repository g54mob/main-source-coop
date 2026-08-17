using System;
using System.Runtime.InteropServices;

namespace Rewired
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct LowLevelInputEvent
	{
		private const int lIXiBwxhBQQuhglOTWaiJEOejmXY = 4;

		private const int LdIQsGNxHnQQHkHwEbesdqfWcOCcb = 8;

		private const int GJwkYNrZUFkeSesnHujJyonvDqPfA = 12;

		public const int buttonsPerPage = 32;

		public const int bytesPerButtonPage = 4;

		private const int xqxrowwQGTdaomNVEvDzuCVqntsP = 4;

		private const int SfMHIShnMKIurLvWNVYBQfmeFAxdA = 4;

		public const int byteIndex_id = 0;

		public const int byteIndex_timestamp = 4;

		public const int byteIndex_elementsStart = 12;

		public IntPtr _buffer;

		private int vBBeaTUvJUIvpDaCXXUHdVstNNwzA;

		private int QJpgCvdDxIIaukCWckDkuElRUIGE;

		private int gsGmLwrXoBgMTRxYcaAlQzdPHlrW;

		private int bGDWKEWhHXXMvRCBcbmrKfRRtjpO;

		private int VNGwInkoDPikrpOnlAHNBURMPewAb;

		private int DqXoPCEqFmhlzBldBwUodOGOQVcL;

		public bool isValid => _buffer != IntPtr.Zero;

		public int buttonCount => QJpgCvdDxIIaukCWckDkuElRUIGE;

		public int axisCount => gsGmLwrXoBgMTRxYcaAlQzdPHlrW;

		public int byteIndex_axesStart => bGDWKEWhHXXMvRCBcbmrKfRRtjpO;

		public int byteIndex_buttonsStart => VNGwInkoDPikrpOnlAHNBURMPewAb;

		public int byteIndex_hatsStart => DqXoPCEqFmhlzBldBwUodOGOQVcL;

		public LowLevelInputEvent(IntPtr P_0, int P_1, int P_2, int P_3)
		{
			if (P_1 == 0 && P_2 == 0)
			{
				throw new ArgumentOutOfRangeException("No elements defined in event.");
			}
			_buffer = P_0;
			QJpgCvdDxIIaukCWckDkuElRUIGE = P_1;
			gsGmLwrXoBgMTRxYcaAlQzdPHlrW = P_2;
			VNGwInkoDPikrpOnlAHNBURMPewAb = 12;
			bGDWKEWhHXXMvRCBcbmrKfRRtjpO = VNGwInkoDPikrpOnlAHNBURMPewAb + ((P_1 > 0) ? (((P_1 - 1) / 32 + 1) * 4) : 0);
			DqXoPCEqFmhlzBldBwUodOGOQVcL = bGDWKEWhHXXMvRCBcbmrKfRRtjpO + P_2 * 4;
			vBBeaTUvJUIvpDaCXXUHdVstNNwzA = GetReportSize(P_1, P_2, P_3);
		}

		public void SetButtonsBitMask(int bitMask, int startButtonIndex)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA > 0)
			{
				if (startButtonIndex % 32 != 0)
				{
					throw new Exception("startIndex must be divisible by 32.");
				}
				Marshal.WriteInt32(_buffer, VNGwInkoDPikrpOnlAHNBURMPewAb + startButtonIndex / 4, bitMask);
			}
		}

		public void SetAxisValue(int index, float value)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA > 0)
			{
				Marshal.WriteInt32(_buffer, bGDWKEWhHXXMvRCBcbmrKfRRtjpO + index * 4, new QRflgAsoeZazuRxfpfYOMkdeuzQU(value).fknjsNXVBTcmqlWQnFOYRUUPTiIK);
			}
		}

		public void SetId(uint id)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA > 0)
			{
				Marshal.WriteInt32(_buffer, 0, (int)id);
			}
		}

		public void SetTimestamp(double value)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA > 0)
			{
				Marshal.WriteInt64(_buffer, 4, new PnhTxULUALcQsQUbFYKghiQWazVA(value).DJgGjhlXlaaeoESChGSmTrvoSkhS);
			}
		}

		public bool GetButtonValue(int index)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA <= 0)
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
			return (Marshal.ReadByte(_buffer, VNGwInkoDPikrpOnlAHNBURMPewAb + num * 4 + num2) & (1 << num3)) != 0;
		}

		public int GetButtonsBitMask(int startButtonIndex)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA <= 0)
			{
				return 0;
			}
			if (startButtonIndex % 32 != 0)
			{
				throw new Exception("startIndex must be divisible by 32.");
			}
			return Marshal.ReadInt32(_buffer, VNGwInkoDPikrpOnlAHNBURMPewAb + startButtonIndex / 4);
		}

		public float GetAxisValue(int index)
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA <= 0)
			{
				return 0f;
			}
			return new QRflgAsoeZazuRxfpfYOMkdeuzQU(Marshal.ReadInt32(_buffer, bGDWKEWhHXXMvRCBcbmrKfRRtjpO + index * 4)).oNeCGljHzMXevHpKFnsxFRCFJTFPB;
		}

		public uint GetId()
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA <= 0)
			{
				return 0u;
			}
			return (uint)Marshal.ReadInt32(_buffer, 0);
		}

		public double GetTimestamp()
		{
			if (vBBeaTUvJUIvpDaCXXUHdVstNNwzA <= 0)
			{
				return 0.0;
			}
			return new PnhTxULUALcQsQUbFYKghiQWazVA(Marshal.ReadInt64(_buffer, 4)).dzZfJAcoFfqRinHLcVNrwNZTaMiGb;
		}

		public static int GetReportSize(int buttonCount, int axisCount, int hatCount)
		{
			return 12 + ((buttonCount > 0) ? (((buttonCount - 1) / 32 + 1) * 4) : 0) + axisCount * 4 + hatCount * 4;
		}
	}
}
