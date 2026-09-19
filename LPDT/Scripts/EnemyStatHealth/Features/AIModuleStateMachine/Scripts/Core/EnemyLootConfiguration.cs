using System.Collections.Generic;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[CreateAssetMenu(fileName = "EnemyLootConfiguration_Enemy", menuName = "Enemies/Loot/EnemyLootConfiguration")]
	public class EnemyLootConfiguration : ScriptableObject
	{
		[Tooltip("How many weighted rolls the enemy performs on death. Ignored when the processor spawns at composite points (one roll per point).")]
		[SerializeField]
		[Min(0f)]
		private int _minDropCount = 1;

		[SerializeField]
		[Min(0f)]
		private int _maxDropCount = 1;

		[SerializeField]
		private List<EnemyLootEntry> _entries = new List<EnemyLootEntry>();

		public bool HasLoot
		{
			get
			{
				for (int i = 0; i < _entries.Count; i++)
				{
					if (_entries[i].IsValid && _entries[i].Weight > 0f)
					{
						return true;
					}
				}
				return false;
			}
		}

		public int RollDropCount()
		{
			return Random.Range(_minDropCount, Mathf.Max(_minDropCount, _maxDropCount) + 1);
		}

		public bool TryRollEntry(out EnemyLootEntry rolledEntry)
		{
			rolledEntry = null;
			float num = 0f;
			for (int i = 0; i < _entries.Count; i++)
			{
				EnemyLootEntry enemyLootEntry = _entries[i];
				if (enemyLootEntry.IsValid)
				{
					num += enemyLootEntry.Weight;
				}
			}
			if (num <= 0f)
			{
				return false;
			}
			float num2 = Random.Range(0f, num);
			for (int j = 0; j < _entries.Count; j++)
			{
				EnemyLootEntry enemyLootEntry2 = _entries[j];
				if (enemyLootEntry2.IsValid)
				{
					num2 -= enemyLootEntry2.Weight;
					if (!(num2 > 0f))
					{
						rolledEntry = enemyLootEntry2;
						return true;
					}
				}
			}
			return false;
		}
	}
}
