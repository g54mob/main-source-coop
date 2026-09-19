using System.Runtime.InteropServices;
using Features.SkinConfiguration.Scripts;
using Fusion;

namespace PlayerCustomization.Networked
{
	[StructLayout(LayoutKind.Explicit, Size = 272)]
	[NetworkStructWeaved(68)]
	public struct RewardSlot : INetworkStruct
	{
		[FieldOffset(0)]
		public NetworkString<_64> PersistentId;

		[FieldOffset(260)]
		public CosmeticRewardPart Part;

		[FieldOffset(264)]
		public SkinType SkinId;

		[FieldOffset(268)]
		public NetworkBool Occupied;
	}
}
