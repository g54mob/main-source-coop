using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class HIDHat : HIDControllerElement
	{
		[CustomObfuscation(rename = false)]
		public enum Type
		{
			Default = 0,
			Custom = 1
		}

		public int rawValue;

		public double timestamp;

		public readonly int byteLength;

		public readonly int startIndex;

		public readonly Type type;

		private Func<int, int> yYsIzhEAunEeadgbIpStWaJjrpBe;

		public HIDHat(byte P_0, HIDInfo P_1, Type P_2)
			: base(P_0, P_1)
		{
			type = P_2;
			byteLength = ((P_1.bitSize > 0) ? ((P_1.bitSize + 8 - 1) / 8) : 0);
			startIndex = P_1.dataIndex;
		}

		public HIDHat(byte P_0, HIDInfo P_1, Func<int, int> P_2)
			: this(P_0, P_1, Type.Custom)
		{
			yYsIzhEAunEeadgbIpStWaJjrpBe = P_2;
		}

		public override void UpdateValue(NativeBuffer inputReport, double timestamp)
		{
			if (inputReport == null || inputReport[0] != reportId)
			{
				return;
			}
			this.timestamp = timestamp;
			if (byteLength == 1)
			{
				rawValue = inputReport[startIndex];
			}
			else
			{
				rawValue = 0;
				for (int i = 0; i < byteLength; i++)
				{
					rawValue |= inputReport[startIndex + i] << 8 * i;
				}
			}
			if (type == Type.Custom && yYsIzhEAunEeadgbIpStWaJjrpBe != null)
			{
				rawValue = yYsIzhEAunEeadgbIpStWaJjrpBe(rawValue);
			}
		}
	}
}
