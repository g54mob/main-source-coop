using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.GrabModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobSwarmReactiveAggroService
	{
		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		public CoinRobSwarmReactiveAggroService(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public bool TryResolveChaseAggro(CoinRobCombatSettings combatSettings, CoinRobSwarmEnemyContext context, Dictionary<int, float> playerNearCoinCarrierSince, out int playerId)
		{
			playerId = 0;
			if (!HasCoinCarrier(context))
			{
				playerNearCoinCarrierSince.Clear();
				return false;
			}
			float playerChaseAggroRadius = combatSettings.PlayerChaseAggroRadius;
			float playerChaseAggroDuration = combatSettings.PlayerChaseAggroDuration;
			HashSet<int> hashSet = new HashSet<int>();
			int num = 0;
			float num2 = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Value.NetworkObject == null)
				{
					continue;
				}
				int playerId2 = player.Key.PlayerId;
				Vector3 position = player.Value.NetworkObject.transform.position;
				if (!TryGetClosestCoinCarrierDistance(context, position, out var distance))
				{
					playerNearCoinCarrierSince.Remove(playerId2);
					continue;
				}
				if (distance > playerChaseAggroRadius)
				{
					playerNearCoinCarrierSince.Remove(playerId2);
					continue;
				}
				hashSet.Add(playerId2);
				if (!playerNearCoinCarrierSince.ContainsKey(playerId2))
				{
					playerNearCoinCarrierSince[playerId2] = Time.time;
				}
				if (!(Time.time - playerNearCoinCarrierSince[playerId2] < playerChaseAggroDuration) && distance < num2)
				{
					num2 = distance;
					num = playerId2;
				}
			}
			List<int> list = new List<int>();
			foreach (int key in playerNearCoinCarrierSince.Keys)
			{
				if (!hashSet.Contains(key))
				{
					list.Add(key);
				}
			}
			foreach (int item in list)
			{
				playerNearCoinCarrierSince.Remove(item);
			}
			if (num <= 0)
			{
				return false;
			}
			playerId = num;
			return true;
		}

		public bool TryResolveGrabAggro(CoinRobSwarmEnemyContext context, out int playerId)
		{
			playerId = 0;
			if (TryGetGrabbedPlayer(context.SwarmKing, out playerId))
			{
				return true;
			}
			foreach (CoinRobBehaviour swarmUnit in context.SwarmUnits)
			{
				if (TryGetGrabbedPlayer(swarmUnit, out playerId))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetGrabbedPlayer(CoinRobBehaviour member, out int playerId)
		{
			playerId = 0;
			if (member == null)
			{
				return false;
			}
			SimplePointGrabable componentInChildren = member.GetComponentInChildren<SimplePointGrabable>();
			if (componentInChildren == null || componentInChildren.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			playerId = componentInChildren.GrabbedByPlayers[0];
			return true;
		}

		private bool HasCoinCarrier(CoinRobSwarmEnemyContext context)
		{
			if (context.SwarmKing != null && context.SwarmKing.IsCarryingLoot)
			{
				return true;
			}
			foreach (CoinRobBehaviour swarmUnit in context.SwarmUnits)
			{
				if (swarmUnit != null && swarmUnit.IsCarryingLoot)
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetClosestCoinCarrierDistance(CoinRobSwarmEnemyContext context, Vector3 playerPosition, out float distance)
		{
			distance = float.MaxValue;
			bool flag = false;
			if (context.SwarmKing != null && context.SwarmKing.IsCarryingLoot)
			{
				distance = Vector3.Distance(playerPosition, context.SwarmKing.transform.position);
				flag = true;
			}
			foreach (CoinRobBehaviour swarmUnit in context.SwarmUnits)
			{
				if (!(swarmUnit == null) && swarmUnit.IsCarryingLoot)
				{
					float num = Vector3.Distance(playerPosition, swarmUnit.transform.position);
					if (!flag || num < distance)
					{
						distance = num;
						flag = true;
					}
				}
			}
			return flag;
		}
	}
}
