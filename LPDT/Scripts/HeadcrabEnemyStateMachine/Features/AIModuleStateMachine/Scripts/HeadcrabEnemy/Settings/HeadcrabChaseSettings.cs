using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadcrabChaseSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headcrab/HeadcrabChaseSettings")]
	public class HeadcrabChaseSettings : ScriptableObject
	{
		[field: SerializeField]
		public float ChasingTime { get; private set; } = 1.5f;
	}
}
