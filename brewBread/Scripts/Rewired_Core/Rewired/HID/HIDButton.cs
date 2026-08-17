using Rewired.Utils.Classes.Data;

namespace Rewired.HID
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class HIDButton : HIDControllerElement
	{
		public bool rawValue;

		public double timestamp;

		public HIDButton(byte P_0, HIDInfo P_1)
			: base(P_0, P_1)
		{
		}

		public void SetValue(bool rawValue, double timestamp)
		{
			this.rawValue = rawValue;
			this.timestamp = timestamp;
		}

		public override void UpdateValue(NativeBuffer inputReport, double timestamp)
		{
			if (inputReport != null && inputReport[0] == reportId)
			{
				this.timestamp = timestamp;
			}
		}
	}
}
