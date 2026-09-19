using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanSlowedSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanSlowedSettings")]
	public class HeadmanSlowedSettings : ScriptableObject
	{
		[field: SerializeField]
		public float SlowedSpeed { get; private set; } = 1f;

		[field: SerializeField]
		public float SlowedDuration { get; private set; } = 2f;
	}
}
