using System;
using System.Runtime.InteropServices;
using Features.DeadPartsModule.Data;
using Features.SkinConfiguration.Scripts;
using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	[NetworkStructWeaved(7)]
	public struct DeadPartCustomizationData : INetworkStruct
	{
		[field: FieldOffset(0)]
		[field: SerializeField]
		public Color ButtColor { get; set; }

		[field: FieldOffset(16)]
		[field: SerializeField]
		public SkinType ButtSkinId { get; set; }

		[field: FieldOffset(20)]
		[field: SerializeField]
		public ButtTexturePreset ButtTexturePreset { get; set; }

		[field: FieldOffset(24)]
		[field: SerializeField]
		public ButtMeshPreset ButtMeshPreset { get; set; }
	}
}
