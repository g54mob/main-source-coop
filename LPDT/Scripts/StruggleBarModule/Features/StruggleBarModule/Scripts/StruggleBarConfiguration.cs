using UnityEngine;

namespace Features.StruggleBarModule.Scripts
{
	[CreateAssetMenu(fileName = "StruggleBarConfiguration_Default", menuName = "Configurations/StruggleBar/StruggleBarConfiguration")]
	public class StruggleBarConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float DrainPerSecond { get; private set; } = 0.25f;

		[field: SerializeField]
		public float BoostPerPress { get; private set; } = 0.08f;

		[Range(0f, 1f)]
		[field: SerializeField]
		public float StartNormalized { get; private set; } = 0.35f;
	}
}
