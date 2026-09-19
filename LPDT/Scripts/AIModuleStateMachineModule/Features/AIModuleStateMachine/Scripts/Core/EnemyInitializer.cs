using System.Collections.Generic;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	public class EnemyInitializer : MonoBehaviour
	{
		[SerializeField]
		private EnemyStatsConfiguration _enemyStatsConfiguration;

		[SerializeField]
		private EntityStatEntityMonoBase _statEntity;

		private void Awake()
		{
			InitializeEnemyStats();
		}

		private void InitializeEnemyStats()
		{
			if (_statEntity == null)
			{
				Debug.LogError("EnemyInitializer on " + base.name + " has no EntityStatEntityMonoBase reference.", this);
				return;
			}
			if (_enemyStatsConfiguration == null)
			{
				Debug.LogError("EnemyInitializer on " + base.name + " has no EnemyStatsConfiguration assigned.", this);
				return;
			}
			if (_enemyStatsConfiguration.Stats == null || _enemyStatsConfiguration.Stats.Count == 0)
			{
				Debug.LogError("EnemyInitializer on " + base.name + " has empty stats configuration.", this);
				return;
			}
			foreach (var (entityStatType2, num2) in _enemyStatsConfiguration.Stats)
			{
				if (entityStatType2 != EntityStatType.None)
				{
					IStat stat = _statEntity.GetStat(entityStatType2);
					if (stat == null)
					{
						Debug.LogError(string.Format("{0} on {1} failed to resolve stat {2}.", "EnemyInitializer", base.name, entityStatType2), this);
						continue;
					}
					stat.MaxValue = num2;
					stat.OverrideValue(num2);
				}
			}
		}
	}
}
