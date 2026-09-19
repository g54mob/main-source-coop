using UnityEngine;

namespace Features.PostProcessingModule.Scripts
{
	[CreateAssetMenu(fileName = "CameraWaterLensConfiguration_Default", menuName = "Configurations/PostProcessing/CameraWaterLensConfiguration")]
	public class CameraWaterLensConfiguration : ScriptableObject
	{
		[Tooltip("Seconds for full wetness (1) to decay to 0 after a sober splash.")]
		[field: SerializeField]
		[field: Min(0.1f)]
		public float WetnessFullDecaySeconds { get; private set; } = 3.5f;
	}
}
