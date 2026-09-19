using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings
{
	[CreateAssetMenu(fileName = "SirenIdleSettings_Default", menuName = "Configurations/AIModuleStateMachine/Siren/SirenIdleSettings")]
	public class SirenIdleSettings : ScriptableObject
	{
		[field: SerializeField]
		public float MinLookTime { get; private set; } = 1f;
	}
}
