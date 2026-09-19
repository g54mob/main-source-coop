using System;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings
{
	[CreateAssetMenu(fileName = "RatsHoleEnemyMovementSettings_Default", menuName = "Configurations/AIModuleStateMachine/RatsHoleEnemy/RatsHoleEnemyMovementSettings")]
	public class RatsHoleEnemyMovementSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AggroRange { get; private set; } = 10f;

		[field: SerializeField]
		public float DespawnPlayerCheckRadius { get; private set; } = 15f;

		[field: SerializeField]
		public float CompletePointMinDistance { get; private set; } = 1.5f;

		[field: SerializeField]
		public float RunVelocityThreshold { get; private set; } = 0.1f;

		[field: SerializeField]
		public float PatrolDuration { get; private set; } = 8f;

		[field: SerializeField]
		public float PatrolRadius { get; private set; } = 15f;

		[field: SerializeField]
		public float DefaultMoveSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public SerializableDictionary<RatsHoleEnemyStateId, float> MoveSpeeds { get; private set; } = new SerializableDictionary<RatsHoleEnemyStateId, float>();

		public void OnValidate()
		{
			ValidateMoveSpeeds();
		}

		private void ValidateMoveSpeeds()
		{
			foreach (object value in Enum.GetValues(typeof(RatsHoleEnemyStateId)))
			{
				if (!MoveSpeeds.ContainsKey((RatsHoleEnemyStateId)value))
				{
					MoveSpeeds.Add((RatsHoleEnemyStateId)value, DefaultMoveSpeed);
				}
			}
		}
	}
}
