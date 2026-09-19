using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings
{
	[CreateAssetMenu(fileName = "ParrotMobIdleSettings_Default", menuName = "Configurations/AIModuleStateMachine/ParrotMob/ParrotMobIdleSettings")]
	public class ParrotMobIdleSettings : ScriptableObject
	{
		[Tooltip("When false, parrot stays on perch. When true, micro-wanders in WanderRadius.")]
		[field: SerializeField]
		public bool CanWander { get; private set; }

		[field: SerializeField]
		public float WanderRadius { get; private set; } = 2f;
	}
}
