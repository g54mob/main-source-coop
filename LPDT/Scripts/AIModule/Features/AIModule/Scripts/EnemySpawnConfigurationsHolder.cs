using System.Collections.Generic;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[CreateAssetMenu(fileName = "EnemySpawnConfigurationsHolder_Default", menuName = "Configurations/AIModule/EnemySpawnConfigurationsHolder", order = 0)]
	public class EnemySpawnConfigurationsHolder : ScriptableObject
	{
		[SerializeField]
		private List<EnemyConfigurationTypeMap> _maps;

		public EnemySpawnConfiguration GetEnemySpawnConfiguration(EnemySpawnConfigurationType type)
		{
			foreach (EnemyConfigurationTypeMap map in _maps)
			{
				if (map.Type == type)
				{
					return map.Configuration;
				}
			}
			return GetEnemySpawnConfiguration(EnemySpawnConfigurationType.Standard);
		}
	}
}
