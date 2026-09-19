using System.Collections.Generic;
using Features.PlayerStatesModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings
{
	[CreateAssetMenu(fileName = "SirenEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/Siren/SirenEnemySettings")]
	public class SirenEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		public float Damage { get; private set; } = 20f;

		[field: SerializeField]
		public float ForceStrength { get; private set; }

		[field: SerializeField]
		public float HardDetectionDistance { get; private set; }

		[field: SerializeField]
		public float RaycastThreshold { get; private set; } = 1f;

		[field: SerializeField]
		public Vector3 LookOffset { get; private set; }

		[field: SerializeField]
		public List<PlayerState> IgnorePlayerStates { get; private set; } = new List<PlayerState>();
	}
}
