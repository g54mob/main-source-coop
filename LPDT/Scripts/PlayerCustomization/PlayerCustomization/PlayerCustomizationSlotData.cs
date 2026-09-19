using System;
using Features.DeadPartsModule.Data;
using Features.SkinConfiguration.Scripts;
using UnityEngine;

namespace PlayerCustomization
{
	[Serializable]
	public class PlayerCustomizationSlotData
	{
		[field: SerializeField]
		public int PlayerId { get; set; }

		[field: SerializeField]
		public string Nickname { get; set; }

		[field: SerializeField]
		public PlayerCustomizationSlotDataStatus PlayerCustomizationSlotDataStatus { get; set; }

		[field: SerializeField]
		public Color PrimaryColor { get; set; }

		[field: SerializeField]
		public Color VariableColor { get; set; }

		[field: SerializeField]
		public SkinType HatPartSkinId { get; set; }

		[field: SerializeField]
		public SkinType TorsoPartSkinId { get; set; }

		[field: SerializeField]
		public SkinType BottomPartSkinId { get; set; }

		[field: SerializeField]
		public bool IsFullSkin { get; set; }

		[field: SerializeField]
		public ButtTexturePreset ButtTexturePreset { get; set; }

		[field: SerializeField]
		public ButtMeshPreset ButtMeshPreset { get; set; }
	}
}
