using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	[CreateAssetMenu(fileName = "PlayerStatsConfiguration_Default", menuName = "Configurations/PlayerStats/PlayerStatsConfiguration")]
	public class PlayerStatsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<EntityStatType, float> PlayerStats { get; private set; }
	}
}
