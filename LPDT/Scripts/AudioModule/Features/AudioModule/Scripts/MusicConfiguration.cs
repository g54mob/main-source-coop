using FMODUnity;
using UnityEngine;

namespace Features.AudioModule.Scripts
{
	[CreateAssetMenu(fileName = "MusicConfiguration_Default", menuName = "Configurations/Audio/MusicConfiguration")]
	public class MusicConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public EventReference MenuMusic { get; private set; }

		[field: SerializeField]
		public EventReference StoreAmbiance { get; private set; }

		[field: SerializeField]
		public string ParameterName { get; private set; } = "LowPass";

		[field: SerializeField]
		public float MenuFadeDuration { get; private set; } = 6f;
	}
}
