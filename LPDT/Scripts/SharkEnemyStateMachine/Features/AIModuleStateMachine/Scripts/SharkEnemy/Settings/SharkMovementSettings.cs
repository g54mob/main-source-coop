using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings
{
	[CreateAssetMenu(fileName = "SharkMovementSettings_Default", menuName = "Configurations/AIModuleStateMachine/Shark/SharkMovementSettings")]
	public class SharkMovementSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Tooltip("NavMeshAgent.speed while chasing the player (Hunt), backing off after an attack, and on first agent init. Idle uses IdleMovementSpeed instead.")]
		public float ActiveMovementSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public float HuntBoundsPadding { get; private set; } = 0.5f;

		[field: SerializeField]
		[field: Tooltip("Must match baked water NavMesh on the level (areas + Humanoid/water agent type in Navigation window).")]
		public float NavMeshStoppingDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float NavMeshAcceleration { get; private set; } = 24f;

		[field: SerializeField]
		public float NavMeshAngularSpeed { get; private set; } = 360f;

		[field: SerializeField]
		[field: Tooltip("Radius passed to INavigationService.HasAvailablePointInRange when chasing a player.")]
		public float NearestNavigationPointRange { get; private set; } = 5f;

		[field: SerializeField]
		[field: Tooltip("While hunting / acquiring: require a complete NavMesh path from the shark agent to the player (projected on mesh). If path length exceeds this value (meters), treat as unreachable. Use 0 to disable the length cap (still requires PathComplete).")]
		public float MaxHuntNavMeshPathLength { get; private set; } = 200f;

		[field: SerializeField]
		public int BackoffNavRandomAttempts { get; private set; } = 16;

		[field: SerializeField]
		public float BackoffNavSearchRadius { get; private set; } = 8f;

		[field: Header("Wander (Idle)")]
		[field: SerializeField]
		[field: Tooltip("NavMeshAgent.speed only in Idle (wandering patrol). Hunt / Backoff use ActiveMovementSpeed — slower patrol vs faster combat movement.")]
		public float IdleMovementSpeed { get; private set; } = 1.5f;

		[field: SerializeField]
		[field: Tooltip("Radius around HuntBounds center used to sample wander destinations.")]
		public float WanderRadius { get; private set; } = 12f;

		[field: SerializeField]
		public float MinWanderUpdateTime { get; private set; } = 2f;

		[field: SerializeField]
		public float MaxWanderUpdateTime { get; private set; } = 5f;

		[field: SerializeField]
		public float IdleChangeAreaUpdateFrequency { get; private set; } = 45f;

		[field: SerializeField]
		public int WanderNavRandomAttempts { get; private set; } = 8;

		[field: SerializeField]
		[field: Tooltip("NavMesh.SamplePosition radius when warping the agent onto the mesh at spawn.")]
		public float NavMeshSpawnSampleRadius { get; private set; } = 16f;
	}
}
