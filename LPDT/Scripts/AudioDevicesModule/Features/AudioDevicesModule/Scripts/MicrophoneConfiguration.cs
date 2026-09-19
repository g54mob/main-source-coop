using UnityEngine;

namespace Features.AudioDevicesModule.Scripts
{
	[CreateAssetMenu(fileName = "MicrophoneConfiguration_Default", menuName = "Configurations/AudioModule/MicrophoneConfiguration")]
	public class MicrophoneConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float MaxSensitivity { get; private set; }

		[field: SerializeField]
		public float MinSensitivity { get; private set; }
	}
}
