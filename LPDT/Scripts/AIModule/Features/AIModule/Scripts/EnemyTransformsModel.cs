using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyTransformsModel
	{
		private readonly Dictionary<EnemyType, List<Transform>> _enemyies = new Dictionary<EnemyType, List<Transform>>();

		private readonly Dictionary<EnemyType, List<NetworkObject>> _enemyNetworkObjects = new Dictionary<EnemyType, List<NetworkObject>>();

		private readonly Dictionary<EnemyType, List<IEnemyBehaviour>> _enemyBehaviours = new Dictionary<EnemyType, List<IEnemyBehaviour>>();

		private readonly Dictionary<int, List<Transform>> _enemiesNearPlayers = new Dictionary<int, List<Transform>>();

		public Dictionary<EnemyType, List<Transform>> Enemyies => _enemyies;

		public Dictionary<EnemyType, List<NetworkObject>> EnemyNetworkObjects => _enemyNetworkObjects;

		public Dictionary<int, List<Transform>> EnemiesNearPlayers => _enemiesNearPlayers;

		public event Action<EnemyType> OnEnemyRegistered;

		public void AddEnemy(EnemyType enemyType, Transform transform, NetworkObject networkObject, IEnemyBehaviour behaviour = null)
		{
			if (!_enemyies.ContainsKey(enemyType))
			{
				_enemyies.Add(enemyType, new List<Transform>());
				_enemyNetworkObjects.Add(enemyType, new List<NetworkObject>());
				_enemyBehaviours.Add(enemyType, new List<IEnemyBehaviour>());
			}
			_enemyies[enemyType].Add(transform);
			_enemyNetworkObjects[enemyType].Add(networkObject);
			if (behaviour != null)
			{
				_enemyBehaviours[enemyType].Add(behaviour);
			}
			this.OnEnemyRegistered?.Invoke(enemyType);
		}

		public void RemoveEnemy(EnemyType enemyType, Transform transform, NetworkObject networkObject, IEnemyBehaviour behaviour = null)
		{
			if (!_enemyies.ContainsKey(enemyType))
			{
				return;
			}
			_enemyies[enemyType].Remove(transform);
			_enemyNetworkObjects[enemyType].Remove(networkObject);
			if (behaviour != null)
			{
				_enemyBehaviours[enemyType].Remove(behaviour);
			}
			foreach (List<Transform> value in _enemiesNearPlayers.Values)
			{
				value.Remove(transform);
			}
		}

		public void GetBehaviours(EnemyType enemyType, List<IEnemyBehaviour> buffer)
		{
			buffer.Clear();
			if (!_enemyBehaviours.TryGetValue(enemyType, out var value))
			{
				return;
			}
			for (int num = value.Count - 1; num >= 0; num--)
			{
				IEnemyBehaviour enemyBehaviour = value[num];
				if (enemyBehaviour == null || enemyBehaviour.NetworkObject == null || !enemyBehaviour.NetworkObject.IsValid)
				{
					value.RemoveAt(num);
				}
				else
				{
					buffer.Add(enemyBehaviour);
				}
			}
		}

		public int CountAliveBehaviours(EnemyType enemyType)
		{
			if (!_enemyBehaviours.TryGetValue(enemyType, out var value))
			{
				return 0;
			}
			int num = 0;
			for (int num2 = value.Count - 1; num2 >= 0; num2--)
			{
				IEnemyBehaviour enemyBehaviour = value[num2];
				if (enemyBehaviour == null || enemyBehaviour.NetworkObject == null || !enemyBehaviour.NetworkObject.IsValid)
				{
					value.RemoveAt(num2);
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		public void RefreshEnemiesNearPlayer(int playerId, Vector3 playerPosition, float distance)
		{
			if (!_enemiesNearPlayers.TryGetValue(playerId, out var value))
			{
				value = new List<Transform>();
				_enemiesNearPlayers.Add(playerId, value);
			}
			value.Clear();
			float num = distance * distance;
			foreach (KeyValuePair<EnemyType, List<Transform>> enemyie in _enemyies)
			{
				foreach (Transform item in enemyie.Value)
				{
					if (!(item == null) && (item.position - playerPosition).sqrMagnitude <= num)
					{
						value.Add(item);
					}
				}
			}
		}

		public bool HasEnemiesNearPlayer(int playerId)
		{
			if (!_enemiesNearPlayers.TryGetValue(playerId, out var value))
			{
				return false;
			}
			PruneDestroyedEnemies(value);
			return value.Count > 0;
		}

		public void ClearEnemiesNearPlayer(int playerId)
		{
			if (_enemiesNearPlayers.TryGetValue(playerId, out var value))
			{
				value.Clear();
			}
		}

		public void ClearEnemiesNearPlayers()
		{
			_enemiesNearPlayers.Clear();
		}

		private void PruneDestroyedEnemies(List<Transform> enemies)
		{
			for (int num = enemies.Count - 1; num >= 0; num--)
			{
				if (enemies[num] == null)
				{
					enemies.RemoveAt(num);
				}
			}
		}
	}
}
