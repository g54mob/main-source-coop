using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.CollectingModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems
{
	public class CoinRobLifetimeSystem : IInitializable, IDisposable
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly CollectItemFellEvent _collectItemFellEvent;

		private readonly CoinRobSwarmsDataHolder _coinRobSwarmDataHolder;

		private readonly INavigationService _navigationService;

		private readonly CoinRobPlayerProximityService _playerProximityService;

		private readonly SemaphoreSlim _coinFallProcessing = new SemaphoreSlim(1, 1);

		public CoinRobLifetimeSystem(MultiplayerModel multiplayerModel, CoinRobSwarmEnemySettings swarmSettings, CollectItemFellEvent collectItemFellEvent, CoinRobSwarmsDataHolder coinRobSwarmDataHolder, INavigationService navigationService, CoinRobPlayerProximityService playerProximityService)
		{
			_multiplayerModel = multiplayerModel;
			_swarmSettings = swarmSettings;
			_collectItemFellEvent = collectItemFellEvent;
			_coinRobSwarmDataHolder = coinRobSwarmDataHolder;
			_navigationService = navigationService;
			_playerProximityService = playerProximityService;
		}

		public void Initialize()
		{
			_collectItemFellEvent.OnItemFell += OnItemFell;
		}

		public void Dispose()
		{
			_collectItemFellEvent.OnItemFell -= OnItemFell;
			_coinFallProcessing.Dispose();
		}

		private async void OnItemFell(IItem item)
		{
			if (!item.AvailableForEnemy || !_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			await _coinFallProcessing.WaitAsync();
			try
			{
				await ProcessCoinFallAsync(item);
			}
			finally
			{
				_coinFallProcessing.Release();
			}
		}

		private async UniTask ProcessCoinFallAsync(IItem item)
		{
			bool num = ShouldReactToCoinFall();
			bool flag = item.NetworkObject != null;
			Vector3 vector = (flag ? item.NetworkObject.transform.position : default(Vector3));
			bool flag2 = flag && IsItemOfInterest(item);
			bool flag3 = flag && _playerProximityService.IsWithinPlayerRadius(vector);
			NavMeshHit hit;
			bool flag4 = flag && _navigationService.IsPointOnNavMeshProjected(vector, out hit);
			ICoinRobSwarmHost targetSwarm;
			bool flag5 = TryGetFreeCoinRobSwarm(out targetSwarm);
			if (!num || !flag || !flag2 || !flag3 || !flag4 || !flag5)
			{
				return;
			}
			vector = item.NetworkObject.transform.position;
			targetSwarm.PrepareCoinChase(vector, item);
			if (targetSwarm.IsNearChaseTarget(vector))
			{
				targetSwarm.ChaseTarget(vector, item);
				return;
			}
			await targetSwarm.TryExtendSwarm();
			if (item.NetworkObject == null)
			{
				targetSwarm.CancelCoinChase();
			}
			else
			{
				targetSwarm.ChaseTarget(item.NetworkObject.transform.position, item);
			}
		}

		private bool IsItemOfInterest(IItem item)
		{
			if (item == null || item.NetworkObject == null)
			{
				return false;
			}
			if (item.Type == ItemType.Coin)
			{
				return true;
			}
			if (!item.NetworkObject.TryGetComponent<LevelObjectMarker>(out var component))
			{
				return false;
			}
			return _swarmSettings.IsItemOfInterest(component.Type);
		}

		private bool ShouldReactToCoinFall()
		{
			return UnityEngine.Random.value < _swarmSettings.CoinFallAggressionChance;
		}

		private bool TryGetFreeCoinRobSwarm(out ICoinRobSwarmHost coinRobSwarmHost)
		{
			coinRobSwarmHost = _coinRobSwarmDataHolder.ActiveCoinRobSwarms.FirstOrDefault((ICoinRobSwarmHost x) => x.IsInWanderingState);
			return coinRobSwarmHost != null;
		}
	}
}
