using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyStunSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyStunSettings")]
	public class MonkeyStunSettings : ScriptableObject
	{
		[field: SerializeField]
		public float StunDuration { get; private set; } = 1.5f;
	}
}
