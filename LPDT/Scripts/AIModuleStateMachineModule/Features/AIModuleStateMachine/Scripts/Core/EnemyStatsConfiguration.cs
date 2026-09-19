using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[CreateAssetMenu(fileName = "EnemyStatsConfiguration_Default", menuName = "Configurations/AIModuleStateMachine/EnemyStatsConfiguration")]
	public class EnemyStatsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<EntityStatType, float> Stats { get; private set; } = new SerializableDictionary<EntityStatType, float>();
	}
}
