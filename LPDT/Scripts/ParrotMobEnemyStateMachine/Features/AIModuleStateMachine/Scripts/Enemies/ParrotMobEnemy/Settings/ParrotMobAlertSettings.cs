using FMODUnity;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings
{
	[CreateAssetMenu(fileName = "ParrotMobAlertSettings_Default", menuName = "Configurations/AIModuleStateMachine/ParrotMob/ParrotMobAlertSettings")]
	public class ParrotMobAlertSettings : ScriptableObject
	{
		[Tooltip("How long the parrot stares at the player before screaming.")]
		[field: SerializeField]
		public float AlertDuration { get; private set; } = 1.25f;

		[field: SerializeField]
		public float LookRotationSpeed { get; private set; } = 6f;

		[Tooltip("One-shot FMOD event played once when the parrot enters Alert.")]
		[field: Header("Audio")]
		[field: SerializeField]
		public EventReference AlertSound { get; private set; }
	}
}
