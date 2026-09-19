using System.Collections.Generic;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyTargetPositionsModel : ILevelCleanup
	{
		private readonly Dictionary<EnemyType, EnemyTargetsData> _targetPositions = new Dictionary<EnemyType, EnemyTargetsData>();

		private readonly Dictionary<EnemyType, EnemyTargetsData> _areaPosition = new Dictionary<EnemyType, EnemyTargetsData>();

		public IReadOnlyDictionary<EnemyType, EnemyTargetsData> TargetPositions => _targetPositions;

		public IReadOnlyDictionary<EnemyType, EnemyTargetsData> AreaPosition => _areaPosition;

		public void UpdateEnemyTargetPosition(EnemyType enemyType, int enemyInstance, Vector3 targetPosition)
		{
			if (!_targetPositions.TryGetValue(enemyType, out var value))
			{
				value = new EnemyTargetsData();
				_targetPositions.Add(enemyType, value);
			}
			value.Positions[enemyInstance] = targetPosition;
		}

		public bool RemoveEnemyTargetPosition(EnemyType enemyType, int enemyInstance, bool removeTypeIfEmpty = true)
		{
			if (!_targetPositions.TryGetValue(enemyType, out var value))
			{
				return false;
			}
			bool num = value.Positions.Remove(enemyInstance);
			if (num && removeTypeIfEmpty && value.Positions.Count == 0)
			{
				_targetPositions.Remove(enemyType);
			}
			return num;
		}

		public void UpdateEnemyAreaPosition(EnemyType enemyType, int enemyInstance, Vector3 targetPosition)
		{
			if (!_areaPosition.TryGetValue(enemyType, out var value))
			{
				value = new EnemyTargetsData();
				_areaPosition.Add(enemyType, value);
			}
			value.Positions[enemyInstance] = targetPosition;
		}

		public bool RemoveEnemyAreaPosition(EnemyType enemyType, int enemyInstance, bool removeTypeIfEmpty = true)
		{
			if (!_areaPosition.TryGetValue(enemyType, out var value))
			{
				return false;
			}
			bool num = value.Positions.Remove(enemyInstance);
			if (num && removeTypeIfEmpty && value.Positions.Count == 0)
			{
				_areaPosition.Remove(enemyType);
			}
			return num;
		}

		public void Cleanup()
		{
			_targetPositions.Clear();
			_areaPosition.Clear();
		}
	}
}
