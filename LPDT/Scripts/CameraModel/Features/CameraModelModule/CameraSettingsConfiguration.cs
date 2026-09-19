using Global.SerializableDictionary;
using UnityEngine;

namespace Features.CameraModelModule
{
	[CreateAssetMenu(fileName = "CameraSettingsConfiguration_Default", menuName = "Configurations/CamerModelModule/CameraSettingsConfiguration")]
	public class CameraSettingsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<CameraType, float> CamerasSensitivityMultipliers { get; private set; }
	}
}
