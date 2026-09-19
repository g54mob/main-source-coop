using Global.SerializableDictionary;
using UnityEngine;

namespace Features.DeadPartsModule.Data
{
	[CreateAssetMenu(fileName = "ButtMeshPresetConfiguration_Default", menuName = "Configurations/Player/ButtMeshPresetConfiguration")]
	public class ButtMeshPresetConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<ButtMeshPreset, ButtMeshPresetData> MeshPresets { get; private set; }
	}
}
