using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobCauldronSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobCauldronSettings")]
	public class CoinRobCauldronSettings : ScriptableObject
	{
		[field: SerializeField]
		public float MinDuration { get; private set; } = 10f;

		[field: SerializeField]
		public float MaxDuration { get; private set; } = 15f;

		[field: SerializeField]
		public float UpImpulse { get; private set; } = 22.5f;

		[field: SerializeField]
		public float ForwardImpulse { get; private set; } = 16.5f;
	}
}
