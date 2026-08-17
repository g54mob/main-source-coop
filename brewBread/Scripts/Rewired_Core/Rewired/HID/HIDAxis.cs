using Rewired.Utils.Classes.Data;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class HIDAxis : HIDControllerElement
	{
		public int rawValue;

		public double timestamp;

		public readonly int byteLength;

		public readonly int startIndex;

		public readonly bool isSigned;

		public readonly int minValue;

		public readonly int maxValue;

		public readonly int zeroValue;

		public HIDAxis(byte P_0, HIDInfo P_1, bool P_2, int P_3)
			: base(P_0, P_1)
		{
			byteLength = ((P_1.bitSize > 0) ? ((P_1.bitSize + 8 - 1) / 8) : 0);
			startIndex = P_1.dataIndex;
			isSigned = P_2;
			minValue = P_1.logicalMin;
			maxValue = P_1.logicalMax;
			zeroValue = P_3;
		}

		public override void UpdateValue(NativeBuffer inputReport, double timestamp)
		{
			if (inputReport == null || inputReport[0] != reportId)
			{
				return;
			}
			this.timestamp = timestamp;
			int num = 0;
			if (byteLength > 1)
			{
				for (int i = 0; i < byteLength; i++)
				{
					num |= inputReport[startIndex + i] << 8 * i;
				}
			}
			else
			{
				num = inputReport[startIndex];
			}
			rawValue = num;
		}
	}
}
