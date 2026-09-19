using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings
{
	[CreateAssetMenu(fileName = "SharkAudioSettings_Default", menuName = "Configurations/AIModuleStateMachine/Shark/SharkAudioSettings")]
	public class SharkAudioSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Tooltip("Horizontal distance from the shark fin to a player above which the start-hunt sound is not triggered or heard.")]
		public float StartHuntSoundAudibleDistance { get; private set; } = 10f;

		[field: SerializeField]
		[field: Tooltip("After Hunt ends with OnTargetLost (e.g. player left NavMesh), suppress start-hunt sound for the same player if Hunt is re-acquired within this window.")]
		public float HuntSoundReacquireDebounce { get; private set; } = 2f;

		[field: SerializeField]
		[field: Tooltip("Minimum time between attack-start sounds, including rapid Attack state re-entries (player or mimic).")]
		public float AttackSoundReacquireDebounce { get; private set; } = 2f;

		[field: SerializeField]
		[field: Tooltip("After the first start-hunt sound, the shark must move at least this horizontal distance away from every player before the next start-hunt sound can play. 0 disables retreat gating.")]
		public float StartHuntSoundMinRetreatDistance { get; private set; } = 15f;
	}
}
