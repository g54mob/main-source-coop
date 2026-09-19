using Features.RagdollModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts
{
	[CreateAssetMenu(fileName = "PlayerStatesConfiguration_Default", menuName = "Configurations/PlayerStates/PlayerStatesConfiguration")]
	public class PlayerStatesConfiguration : ScriptableObject
	{
		[Tooltip("Fallback when a preset is missing from StunDurations.")]
		[field: SerializeField]
		public float StunTemporalDuration { get; private set; } = 4f;

		[field: SerializeField]
		public SerializableDictionary<StunDurationPreset, float> StunDurations { get; private set; } = new SerializableDictionary<StunDurationPreset, float>();

		[field: SerializeField]
		public float StaminaStunTemporalDuration { get; private set; } = 3f;

		[field: SerializeField]
		public float StunThrowMultiplier { get; private set; }

		[field: SerializeField]
		public PhysicsMaterial ThrowPhysicsMaterial { get; private set; }

		[field: SerializeField]
		public float ThrowPlayerMass { get; private set; }

		public float ResolveStunDuration(StunDurationPreset preset)
		{
			if (StunDurations != null && StunDurations.TryGetValue(preset, out var value))
			{
				return value;
			}
			return StunTemporalDuration;
		}
	}
}
