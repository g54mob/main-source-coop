using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings
{
	[CreateAssetMenu(fileName = "BartenderReactSettings_Default", menuName = "RubberArms/Bartender/Bartender React Settings")]
	public class BartenderReactSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Min(0.05f)]
		public float ReactDurationSeconds { get; private set; } = 1.25f;
	}
}
