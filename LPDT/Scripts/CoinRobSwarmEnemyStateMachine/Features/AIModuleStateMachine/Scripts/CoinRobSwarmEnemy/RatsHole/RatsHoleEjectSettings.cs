using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[CreateAssetMenu(fileName = "RatsHoleEjectSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/RatsHoleEjectSettings")]
	public class RatsHoleEjectSettings : ScriptableObject
	{
		[field: SerializeField]
		public int ItemsPerGrab { get; private set; } = 1;

		[field: SerializeField]
		public float GrabCooldown { get; private set; } = 0.35f;

		[field: SerializeField]
		public RatsHoleEjectSelectionMode SelectionMode { get; private set; }

		[field: SerializeField]
		public float ImpulsePerMass { get; private set; } = 5f;

		[field: SerializeField]
		public float UpwardBias { get; private set; } = 0.2f;

		[field: SerializeField]
		public Vector3 LocalEjectDirection { get; private set; } = Vector3.forward;

		[field: SerializeField]
		public ForceMode ForceMode { get; private set; } = ForceMode.Impulse;

		[field: SerializeField]
		public float SpawnSpreadRadius { get; private set; } = 0.12f;

		[field: SerializeField]
		public float SpawnOffset { get; private set; } = 0.35f;
	}
}
