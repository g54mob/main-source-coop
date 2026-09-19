using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[CreateAssetMenu(fileName = "EnemyResourceWarmupConfiguration_Default", menuName = "Configurations/AIModule/EnemyResourceWarmupConfiguration")]
	public class EnemyResourceWarmupConfiguration : ScriptableObject
	{
		[SerializeField]
		private List<EnemyResourceWarmupEntry> _warmupPrefabsByType = new List<EnemyResourceWarmupEntry>();

		[SerializeField]
		private List<GameObject> _extraWarmupPrefabs = new List<GameObject>();

		public IReadOnlyList<GameObject> ExtraWarmupPrefabs
		{
			get
			{
				IReadOnlyList<GameObject> extraWarmupPrefabs = _extraWarmupPrefabs;
				return extraWarmupPrefabs ?? Array.Empty<GameObject>();
			}
		}

		public bool TryGetWarmupPrefab(EnemyType enemyType, out GameObject warmupPrefab)
		{
			warmupPrefab = null;
			if (_warmupPrefabsByType == null)
			{
				return false;
			}
			for (int i = 0; i < _warmupPrefabsByType.Count; i++)
			{
				EnemyResourceWarmupEntry enemyResourceWarmupEntry = _warmupPrefabsByType[i];
				if (enemyResourceWarmupEntry != null && enemyResourceWarmupEntry.EnemyType == enemyType && !(enemyResourceWarmupEntry.WarmupPrefab == null))
				{
					warmupPrefab = enemyResourceWarmupEntry.WarmupPrefab;
					return true;
				}
			}
			return false;
		}
	}
}
