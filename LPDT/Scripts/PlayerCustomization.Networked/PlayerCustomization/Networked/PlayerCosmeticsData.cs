using System.Runtime.InteropServices;
using Features.SkinConfiguration.Scripts;
using Fusion;
using UnityEngine;

namespace PlayerCustomization.Networked
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	[NetworkStructWeaved(8)]
	public struct PlayerCosmeticsData : INetworkStruct
	{
		[FieldOffset(0)]
		public Color PrimaryColor;

		[FieldOffset(16)]
		public SkinType HatPartSkinId;

		[FieldOffset(20)]
		public SkinType TorsoPartSkinId;

		[FieldOffset(24)]
		public SkinType BottomPartSkinId;

		[FieldOffset(28)]
		public NetworkBool IsFullSkin;
	}
}
