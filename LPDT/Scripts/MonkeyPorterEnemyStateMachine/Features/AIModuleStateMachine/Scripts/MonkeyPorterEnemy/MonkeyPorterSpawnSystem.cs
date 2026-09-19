using System;
using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterSpawnSystem : IInitializable, IDisposable, ILevelContentSpawnProvider
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		private readonly MonkeyPorterSpawnPointsModel _monkeyPorterSpawnPointsModel;

		private readonly MonkeyPorterSpawnConfig _monkeyPorterSpawnConfig;

		private readonly MonkeyPorterRunModel _monkeyPorterRunModel;

		public MonkeyPorterSpawnSystem(MultiplayerModel multiplayerModel, ILevelContentSpawnRegistry levelContentSpawnRegistry, MonkeyPorterSpawnPointsModel monkeyPorterSpawnPointsModel, MonkeyPorterSpawnConfig monkeyPorterSpawnConfig, MonkeyPorterRunModel monkeyPorterRunModel)
		{
			_multiplayerModel = multiplayerModel;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
			_monkeyPorterSpawnPointsModel = monkeyPorterSpawnPointsModel;
			_monkeyPorterSpawnConfig = monkeyPorterSpawnConfig;
			_monkeyPorterRunModel = monkeyPorterRunModel;
		}

		public void Initialize()
		{
			_levelContentSpawnRegistry.Register(this);
		}

		public void Dispose()
		{
			_levelContentSpawnRegistry.Unregister(this);
		}

		public async UniTask SpawnLevelContentAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (!runner.IsSharedModeMasterClient || (!_monkeyPorterRunModel.IsOwned.Value && !_monkeyPorterSpawnConfig.AlwaysSpawnWithoutPurchase) || _monkeyPorterSpawnPointsModel.Points.Count == 0)
			{
				return;
			}
			MonkeyPorterSpawnPointData point = _monkeyPorterSpawnPointsModel.Points[0];
			NetworkObject monkeyObject = await runner.SpawnAsync(_monkeyPorterSpawnConfig.MonkeyPrefab, point.Position, point.Rotation, runner.LocalPlayer);
			if (!(monkeyObject == null))
			{
				_monkeyPorterRunModel.IsOwned.Value = false;
				NetworkObject networkObject = await runner.SpawnAsync(_monkeyPorterSpawnConfig.CartPrefab, point.Position, point.Rotation, runner.LocalPlayer);
				if (!(networkObject == null) && monkeyObject.TryGetComponent<MonkeyPorterEnemy>(out var component))
				{
					component.AttachCart(networkObject);
				}
			}
		}
	}
}
