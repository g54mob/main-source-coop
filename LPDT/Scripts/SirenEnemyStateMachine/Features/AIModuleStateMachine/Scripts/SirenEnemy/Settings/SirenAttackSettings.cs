using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings
{
	[CreateAssetMenu(fileName = "SirenAttackSettings_Default", menuName = "Configurations/AIModuleStateMachine/Siren/SirenAttackSettings")]
	public class SirenAttackSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AttackTime { get; private set; } = 5f;
	}
}
