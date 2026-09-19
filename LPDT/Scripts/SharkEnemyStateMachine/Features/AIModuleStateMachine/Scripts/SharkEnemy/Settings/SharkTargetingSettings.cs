using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings
{
	[CreateAssetMenu(fileName = "SharkTargetingSettings_Default", menuName = "Configurations/AIModuleStateMachine/Shark/SharkTargetingSettings")]
	public class SharkTargetingSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Tooltip("Horizontal max distance from the shark to acquire or keep screening a player as a target.")]
		public float MaxHuntRadius { get; private set; } = 80f;

		[field: SerializeField]
		[field: Tooltip("If true, the player pivot must be inside HuntBounds on SharkEnemyContext.")]
		public bool RequireHuntVolume { get; private set; }

		[field: SerializeField]
		[field: Tooltip("When no valid player is in hunt range, acquire a Mimic from EnemyTransformsModel (same radius / volume / NavMesh rules as players).")]
		public bool HuntMimicWhenNoPlayerInRange { get; private set; } = true;

		[field: SerializeField]
		[field: Tooltip("Mimic must be within this horizontal distance of at least one alive, attackable player to be hunted or bitten.")]
		public float MimicAttackNearPlayerRadius { get; private set; } = 10f;

		[field: Header("Line of Sight")]
		[field: SerializeField]
		public bool UseLineOfSight { get; private set; }

		[field: SerializeField]
		[field: Tooltip("Seconds the player can stay behind obstacles before Shark loses Hunt target.")]
		public float LoseAggroAfterLosLostTime { get; private set; } = 1f;

		[field: SerializeField]
		public LayerMask SafeStandLayers { get; private set; }

		[field: SerializeField]
		public LayerMask CoverLayerMask { get; private set; }

		[field: SerializeField]
		public LayerMask IgnoreJumpProtectionLayers { get; private set; }

		[field: SerializeField]
		public float MinHighGroundDelta { get; private set; } = 1.2f;

		[field: SerializeField]
		[field: Tooltip("Upward velocity (m/s) treated as jump while still grounded. Used with CharacterMovableBase.IsGrounded to bite elevated airborne targets.")]
		public float JumpAirborneVelocityThreshold { get; private set; } = 0.5f;
	}
}
