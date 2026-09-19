using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings
{
	[CreateAssetMenu(fileName = "SirenStunSettings_Default", menuName = "Configurations/AIModuleStateMachine/Siren/SirenStunSettings")]
	public class SirenStunSettings : ScriptableObject
	{
		[field: SerializeField]
		public float StunTime { get; private set; } = 10f;
	}
}
