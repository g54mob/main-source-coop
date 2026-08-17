using System;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal struct HardwareControllerMapIdentifier
	{
		public readonly Guid guid;

		public readonly InputSource inputSource;

		public readonly InputPlatform actualInputPlatform;

		public readonly int variantIndex;

		public HardwareControllerMapIdentifier(Guid P_0, InputSource P_1, InputPlatform P_2, int P_3)
		{
			guid = P_0;
			inputSource = P_1;
			actualInputPlatform = P_2;
			variantIndex = P_3;
		}

		public static bool Matches(HardwareControllerMapIdentifier a, HardwareControllerMapIdentifier b)
		{
			if (a.guid == b.guid && a.inputSource == b.inputSource && a.actualInputPlatform == b.actualInputPlatform)
			{
				return a.variantIndex == b.variantIndex;
			}
			return false;
		}
	}
}
