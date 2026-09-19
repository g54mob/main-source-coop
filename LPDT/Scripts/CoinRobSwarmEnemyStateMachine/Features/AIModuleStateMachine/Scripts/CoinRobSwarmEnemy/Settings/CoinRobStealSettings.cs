using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobStealSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobStealSettings")]
	public class CoinRobStealSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AttackTime { get; private set; } = 0.65f;

		[field: SerializeField]
		public float GrabStartDelay { get; private set; } = 0.12f;

		[field: SerializeField]
		public float StealApproachDistance { get; private set; } = 1f;

		[field: SerializeField]
		public float ItemCheckRadius { get; private set; } = 1.5f;

		[field: SerializeField]
		public float StealTaskTimeout { get; private set; } = 4f;

		[field: SerializeField]
		public float StealUnreachableGiveUpTime { get; private set; } = 3f;

		[field: SerializeField]
		public float MaxStealHeightAboveUnit { get; private set; } = 0.2f;

		[field: SerializeField]
		public float CarrySettleDistance { get; private set; } = 0.35f;

		[field: SerializeField]
		public float CarrySettleTimeout { get; private set; } = 1.25f;
	}
}
