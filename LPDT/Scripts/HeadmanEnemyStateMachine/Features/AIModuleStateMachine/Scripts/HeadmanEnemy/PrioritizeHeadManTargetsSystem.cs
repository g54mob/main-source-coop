using System;
using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public class PrioritizeHeadManTargetsSystem : MonoBehaviour
	{
		private readonly struct TargetPriority : IComparable<TargetPriority>
		{
			public readonly PlayerRef Player;

			public readonly float Distance;

			public readonly bool IsOnNavMesh;

			public TargetPriority(PlayerRef player, float distance, bool isOnNavMesh)
			{
				Player = player;
				Distance = distance;
				IsOnNavMesh = isOnNavMesh;
			}

			public int CompareTo(TargetPriority other)
			{
				int num = other.IsOnNavMesh.CompareTo(IsOnNavMesh);
				if (num != 0)
				{
					return num;
				}
				return Distance.CompareTo(other.Distance);
			}
		}

		[SerializeField]
		private HeadManTargetsModel _headManTargetsModel;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private INavigationService _navigationService;

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
		}

		private void Update()
		{
			PrioritizeTargets();
		}

		private void PrioritizeTargets()
		{
			IReadOnlyList<PlayerRef> headManTargetsList = _headManTargetsModel.HeadManTargetsList;
			if (headManTargetsList.Count == 0 && _headManTargetsModel.PrioritizedTargets.Count != 0)
			{
				_headManTargetsModel.SetPrioritize(Array.Empty<PlayerRef>(), 0);
				return;
			}
			PlayerRef[] prioritizedTargets = GetPrioritizedTargets(headManTargetsList);
			if (_headManTargetsModel.PrioritizedTargets.Count == 0 && prioritizedTargets.Length != 0)
			{
				_headManTargetsModel.SetPrioritize(prioritizedTargets, prioritizedTargets.Length);
			}
			else if (prioritizedTargets.Length != 0 && _headManTargetsModel.PrioritizedTargets.Count > 0 && prioritizedTargets[0] != _headManTargetsModel.PrioritizedTargets[0])
			{
				_headManTargetsModel.SetPrioritize(prioritizedTargets, prioritizedTargets.Length);
			}
		}

		private PlayerRef[] GetPrioritizedTargets(IReadOnlyList<PlayerRef> targetList)
		{
			Vector3 position = base.transform.position;
			List<TargetPriority> list = new List<TargetPriority>();
			foreach (PlayerRef target in targetList)
			{
				if (IsTargetValid(target, out var _) && _navigationService.TryGetPlayerTrackingPosition(target, out var position2))
				{
					float distance = Vector3.Distance(position, position2);
					NavMeshHit hit;
					bool isOnNavMesh = _navigationService.IsPointOnNavMeshProjected(position2, _navMeshAgent, out hit);
					list.Add(new TargetPriority(target, distance, isOnNavMesh));
				}
			}
			list.Sort();
			return list.Select((TargetPriority target) => target.Player).ToArray();
		}

		private bool IsTargetValid(PlayerRef player, out PlayerDataHolder playerData)
		{
			return _spawnedPlayersModel.Players.TryGetValue(player, out playerData);
		}
	}
}
