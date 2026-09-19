using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyLastChanceSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyLastChanceSettings")]
	public class MonkeyLastChanceSettings : ScriptableObject
	{
		[field: SerializeField]
		public float Duration { get; private set; } = 4f;
	}
}
