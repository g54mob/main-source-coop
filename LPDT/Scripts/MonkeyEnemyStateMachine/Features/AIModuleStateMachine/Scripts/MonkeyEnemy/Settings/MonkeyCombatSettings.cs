using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyCombatSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyCombatSettings")]
	public class MonkeyCombatSettings : ScriptableObject
	{
		[field: SerializeField]
		public float ChasingSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public float CatchUpDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float DistanceToAttack { get; private set; } = 2.5f;

		[field: SerializeField]
		public float AttackTime { get; private set; } = 2f;

		[field: SerializeField]
		public float AttackAngle { get; private set; } = 90f;

		[field: SerializeField]
		public float ChaseTimeoutDuration { get; private set; } = 15f;

		[field: SerializeField]
		public float ChaseCooldownDuration { get; private set; } = 10f;
	}
}
