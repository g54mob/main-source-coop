using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.Extensions;
using Features.GamePhasesModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public class SleeperFearHomeReassignmentSystem : IInitializable, IDisposable
	{
		private const float HomeMatchSqrDistance = 0.25f;

		private const int SpawnSettleTimeoutFrames = 600;

		private readonly GamePhasesModel _gamePhasesModel;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		private readonly EnemySpawnStatesModel _enemySpawnStatesModel;

		private readonly EnemyTransformsModel _enemyTransformsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly List<IFearHomeAssignable> _preFearHomes = new List<IFearHomeAssignable>();

		private readonly List<IEnemyBehaviour> _behaviourBuffer = new List<IEnemyBehaviour>();

		private CancellationTokenSource _reassignCts;

		public SleeperFearHomeReassignmentSystem(GamePhasesModel gamePhasesModel, EnemySpawnPointsModel enemySpawnPointsModel, EnemySpawnStatesModel enemySpawnStatesModel, EnemyTransformsModel enemyTransformsModel, MultiplayerModel multiplayerModel)
		{
			_gamePhasesModel = gamePhasesModel;
			_enemySpawnPointsModel = enemySpawnPointsModel;
			_enemySpawnStatesModel = enemySpawnStatesModel;
			_enemyTransformsModel = enemyTransformsModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_gamePhasesModel.BeforeFearAllEnemies += OnBeforeFearAllEnemies;
			_gamePhasesModel.OnGamePhaseActivated += OnGamePhaseActivated;
		}

		public void Dispose()
		{
			_gamePhasesModel.BeforeFearAllEnemies -= OnBeforeFearAllEnemies;
			_gamePhasesModel.OnGamePhaseActivated -= OnGamePhaseActivated;
			CancelReassign();
		}

		private void OnBeforeFearAllEnemies()
		{
			if (!IsMaster())
			{
				return;
			}
			CancelReassign();
			_preFearHomes.Clear();
			_enemyTransformsModel.GetBehaviours(EnemyType.Sleeper, _behaviourBuffer);
			for (int i = 0; i < _behaviourBuffer.Count; i++)
			{
				if (_behaviourBuffer[i] is IFearHomeAssignable fearHomeAssignable && IsAlive(fearHomeAssignable))
				{
					_preFearHomes.Add(fearHomeAssignable);
				}
			}
		}

		private void OnGamePhaseActivated()
		{
			if (IsMaster() && _preFearHomes.Count != 0)
			{
				CancelReassign();
				_reassignCts = new CancellationTokenSource();
				WaitAndReassignAsync(_reassignCts.Token).Forget();
			}
		}

		private async UniTaskVoid WaitAndReassignAsync(CancellationToken token)
		{
			try
			{
				await WaitForSleeperSpawnSettleAsync(token);
				if (!token.IsCancellationRequested)
				{
					ReassignHomes();
				}
			}
			finally
			{
				_preFearHomes.Clear();
			}
		}

		private async UniTask WaitForSleeperSpawnSettleAsync(CancellationToken token)
		{
			if (!_enemySpawnStatesModel.IsAttached || !_enemySpawnStatesModel.States.TryGetValue(EnemyType.Sleeper, out var value) || value.MaxSpawnedCount <= 0)
			{
				return;
			}
			int maxSpawned = value.MaxSpawnedCount;
			for (int frame = 0; frame < 600; frame++)
			{
				if (token.IsCancellationRequested)
				{
					break;
				}
				if (_enemyTransformsModel.CountAliveBehaviours(EnemyType.Sleeper) >= maxSpawned)
				{
					break;
				}
				await UniTask.Yield(PlayerLoopTiming.Update, token);
			}
		}

		private void ReassignHomes()
		{
			if (!IsMaster() || !_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(EnemyType.Sleeper, out var value) || value.Count == 0)
			{
				return;
			}
			List<IFearHomeAssignable> list = new List<IFearHomeAssignable>(_preFearHomes.Count);
			HashSet<NetworkId> hashSet = new HashSet<NetworkId>(_preFearHomes.Count);
			for (int i = 0; i < _preFearHomes.Count; i++)
			{
				IFearHomeAssignable fearHomeAssignable = _preFearHomes[i];
				if (IsAlive(fearHomeAssignable))
				{
					list.Add(fearHomeAssignable);
					hashSet.Add(fearHomeAssignable.NetworkObject.Id);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			List<EnemySpawnPointData> list2 = new List<EnemySpawnPointData>(value.Count);
			for (int j = 0; j < value.Count; j++)
			{
				EnemySpawnPointData enemySpawnPointData = value[j];
				if (!enemySpawnPointData.IsOneTimeSpawnPoint && (!enemySpawnPointData.Occupancy.IsOccupied || hashSet.Contains(enemySpawnPointData.Occupancy.OccupantId)))
				{
					list2.Add(enemySpawnPointData);
				}
			}
			if (list2.Count == 0)
			{
				return;
			}
			list2.Shuffle();
			int num = Math.Min(list.Count, list2.Count);
			for (int k = 0; k < num; k++)
			{
				list[k].ReleaseSpawnPointOccupancy();
			}
			for (int l = 0; l < num; l++)
			{
				IFearHomeAssignable fearHomeAssignable2 = list[l];
				EnemySpawnPointData enemySpawnPointData2 = list2[l];
				if (EnemySpawnPointOccupancyUtility.TryOccupy(enemySpawnPointData2, fearHomeAssignable2))
				{
					fearHomeAssignable2.ApplyFearHome(enemySpawnPointData2.Position, 0.25f);
				}
			}
		}

		private static bool IsAlive(IFearHomeAssignable home)
		{
			if (home != null && home.NetworkObject != null)
			{
				return home.NetworkObject.IsValid;
			}
			return false;
		}

		private bool IsMaster()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner != null)
			{
				return networkRunner.IsSharedModeMasterClient;
			}
			return false;
		}

		private void CancelReassign()
		{
			if (_reassignCts != null)
			{
				_reassignCts.Cancel();
				_reassignCts.Dispose();
				_reassignCts = null;
			}
		}
	}
}
