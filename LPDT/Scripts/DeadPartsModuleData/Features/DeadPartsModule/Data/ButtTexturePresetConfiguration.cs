using Global.SerializableDictionary;
using UnityEngine;

namespace Features.DeadPartsModule.Data
{
	[CreateAssetMenu(fileName = "ButtTexturePresetConfiguration_Default", menuName = "Configurations/Player/ButtTexturePresetConfiguration")]
	public class ButtTexturePresetConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<ButtTexturePreset, ButtTexturePresetData> TexturePresets { get; private set; }
	}
}
