using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobSwarmFormationService
	{
		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly INavigationService _navigationService;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		private readonly CoinRobSwarmAnimatorPresenter _presenter;

		private readonly RatsHoleRegistry _ratsHoleRegistry;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly List<Vector3> _alivePlayerPositions = new List<Vector3>();

		private readonly List<CoinRobBehaviour> _patrolUnitsBuffer = new List<CoinRobBehaviour>();

		private readonly List<Vector3> _assignedPatrolPoints = new List<Vector3>();

		private readonly Dictionary<CoinRobBehaviour, float> _wanderingUnitStuckTimers = new Dictionary<CoinRobBehaviour, float>();

		public float UnitStuckRepathTime => Mathf.Max(0.05f, _swarmSettings.WanderingUnitStuckRepathTime);

		public CoinRobSwarmFormationService(CoinRobSwarmEnemyContext context, CoinRobSwarmEnemySettings swarmSettings, CoinRobChaseSettings chaseSettings, INavigationService navigationService, EnemySpawnPointsModel enemySpawnPointsModel, CoinRobSwarmAnimatorPresenter presenter, RatsHoleRegistry ratsHoleRegistry, SpawnedPlayersModel spawnedPlayersModel, IPlayerStateService playerStateService)
		{
			_context = context;
			_swarmSettings = swarmSettings;
			_chaseSettings = chaseSettings;
			_navigationService = navigationService;
			_enemySpawnPointsModel = enemySpawnPointsModel;
			_presenter = presenter;
			_ratsHoleRegistry = ratsHoleRegistry;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerStateService = playerStateService;
		}

		public Vector3 GetRandomNavmeshPosition(Vector3 center, float radius)
		{
			if (!_navigationService.TryGetRandomNavmeshPosition(center, radius, out var position))
			{
				return _context.transform.position;
			}
			return position;
		}

		public void SetSwarmDestination(Vector3 target, bool applySwarmLocomotionAnimation = true)
		{
			if (_context.SwarmKing == null)
			{
				return;
			}
			Vector3 swarmCenter = _context.SwarmCenter;
			_context.SwarmKing.SetDestination(_navigationService.ValidatePointOnNavmesh(target, _swarmSettings.SwarmRadius));
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				Vector3 destination = _navigationService.ValidatePointOnNavmesh(target + GetOffsetInSwarm(swarmCenter, swarmUnit), _swarmSettings.SwarmRadius);
				swarmUnit.SetDestination(destination);
			}
			if (applySwarmLocomotionAnimation)
			{
				_presenter.SetUnitsRunning();
			}
		}

		public void KeepUnitsActiveAroundChaseTarget(Vector3 chasePosition, float deltaTime)
		{
			RefreshUnitsPatrolAroundPointOnArrival(chasePosition);
			RepathStuckUnitsAroundKingToward(chasePosition, deltaTime);
			SyncPatrolLocomotionAnimations();
		}

		public void TeleportSwarm(Vector3 position)
		{
			_context.transform.position = position;
			if (_context.SwarmKing != null)
			{
				_context.SwarmKing.Teleport(_navigationService.ValidatePointOnNavmesh(position, _swarmSettings.SwarmRadius));
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				swarmUnit.Teleport(GetRandomNavmeshPosition(position, _swarmSettings.SwarmRadius));
			}
		}

		public void MoveSwarmToPatrolAroundKing()
		{
			if (!(_context.SwarmKing == null))
			{
				if (TryGetRandomWanderingDestinationAroundPlayer(out var destination))
				{
					_context.SwarmKing.SetDestination(destination);
					AssignUnitPatrolDestinationsAround(destination);
					_presenter.SetUnitsRunning();
				}
				else
				{
					_context.SwarmKing.StopMovement();
					AssignUnitPatrolDestinationsAround(_context.SwarmKing.transform.position);
				}
			}
		}

		public void MoveSwarmToLocalPatrolPoint()
		{
			MoveSwarmToPatrolAroundKing();
		}

		private bool TryGetRandomWanderingDestinationAroundPlayer(out Vector3 destination)
		{
			destination = Vector3.zero;
			if (!TryPickRandomAlivePlayerPosition(out var playerPosition))
			{
				return false;
			}
			float num = Mathf.Min(_swarmSettings.WanderingMinDistanceFromPlayer, _swarmSettings.WanderingMaxDistanceFromPlayer);
			float num2 = Mathf.Max(_swarmSettings.WanderingMinDistanceFromPlayer, _swarmSettings.WanderingMaxDistanceFromPlayer);
			if (num2 <= 0.0001f)
			{
				return false;
			}
			float swarmRadius = _swarmSettings.SwarmRadius;
			int num3 = Mathf.Max(1, _swarmSettings.WanderingPlayerPatrolAttempts);
			for (int i = 0; i < num3; i++)
			{
				float f = UnityEngine.Random.Range(0f, MathF.PI * 2f);
				float num4 = UnityEngine.Random.Range(num, num2);
				Vector3 point = playerPosition + new Vector3(Mathf.Cos(f) * num4, 0f, Mathf.Sin(f) * num4);
				Vector3 vector = _navigationService.ValidatePointOnNavmesh(point, swarmRadius);
				Vector3 vector2 = vector - playerPosition;
				vector2.y = 0f;
				float magnitude = vector2.magnitude;
				if (!(magnitude < num) && !(magnitude > num2))
				{
					destination = vector;
					return true;
				}
			}
			float f2 = UnityEngine.Random.Range(0f, MathF.PI * 2f);
			float num5 = (num + num2) * 0.5f;
			Vector3 point2 = playerPosition + new Vector3(Mathf.Cos(f2) * num5, 0f, Mathf.Sin(f2) * num5);
			destination = _navigationService.ValidatePointOnNavmesh(point2, swarmRadius);
			return true;
		}

		private bool TryPickRandomAlivePlayerPosition(out Vector3 playerPosition)
		{
			playerPosition = Vector3.zero;
			_alivePlayerPositions.Clear();
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (value != null && !(value.NetworkObject == null) && value.NetworkObject.IsValid && _playerStateService.IsPlayerAlive(player.Key.PlayerId))
				{
					_alivePlayerPositions.Add(value.NetworkObject.transform.position);
				}
			}
			if (_alivePlayerPositions.Count == 0)
			{
				return false;
			}
			playerPosition = _alivePlayerPositions[UnityEngine.Random.Range(0, _alivePlayerPositions.Count)];
			return true;
		}

		public void SetRunAwayDestination(Vector3 spawnTarget)
		{
			if (!(_context.SwarmKing == null))
			{
				float runAwayNavMeshSampleRadius = _chaseSettings.RunAwayNavMeshSampleRadius;
				Vector3 vector = _navigationService.ValidatePointOnNavmesh(spawnTarget, runAwayNavMeshSampleRadius);
				Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole;
				bool flag = TryGetReachableReadyHole(vector, out hole);
				Vector3 vector2 = ResolveDepositCenter(vector, hole, flag, runAwayNavMeshSampleRadius);
				_context.RunAwayDepositHole = (flag ? hole : null);
				if (flag)
				{
					_context.RunAwayPosition = vector2;
				}
				float radius = (flag ? hole.AbsorbPointRadius : _chaseSettings.KingRunAwaySpawnOrbitRadius);
				_context.SwarmKing.SetDestination(GetRandomNavmeshPosition(vector2, radius));
				AssignCarrierAndEmptyRunAwayDestinations(vector, hole, flag);
				_presenter.SetUnitsRunning();
			}
		}

		public void RepathCarriersToRunAwayDestination(Vector3 spawnTarget)
		{
			float runAwayNavMeshSampleRadius = _chaseSettings.RunAwayNavMeshSampleRadius;
			Vector3 vector = _navigationService.ValidatePointOnNavmesh(spawnTarget, runAwayNavMeshSampleRadius);
			Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole;
			bool flag = TryGetReachableReadyHole(vector, out hole);
			_context.RunAwayDepositHole = (flag ? hole : null);
			AssignCarrierAndEmptyRunAwayDestinations(vector, hole, flag);
		}

		public bool TrySetFearHoleDestination(out Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole, out Vector3 holePosition, out float absorbPointRadius, Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole preferredHole = null, Dictionary<CoinRobBehaviour, Vector3> memberDestinations = null)
		{
			hole = null;
			holePosition = default(Vector3);
			absorbPointRadius = 0f;
			memberDestinations?.Clear();
			if (_context.SwarmKing == null)
			{
				return false;
			}
			Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole2 = (IsReadyHoleReachable(preferredHole) ? preferredHole : null);
			if (hole2 == null && !TryGetReachableReadyHole(_context.SwarmCenter, out hole2))
			{
				return false;
			}
			hole = hole2;
			float runAwayNavMeshSampleRadius = _chaseSettings.RunAwayNavMeshSampleRadius;
			holePosition = _navigationService.ValidatePointOnNavmesh(hole2.AbsorbWorldPosition, runAwayNavMeshSampleRadius);
			absorbPointRadius = Mathf.Max(0f, hole2.AbsorbPointRadius);
			_context.RunAwayPosition = holePosition;
			_context.SwarmKing.SetDestination(holePosition);
			if (memberDestinations != null)
			{
				memberDestinations[_context.SwarmKing] = holePosition;
			}
			float radius = Mathf.Max(0.01f, hole2.AbsorbPointRadius);
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit == null)
				{
					continue;
				}
				if (swarmUnit.HasAttachedItem)
				{
					swarmUnit.SetDestination(holePosition);
					if (memberDestinations != null)
					{
						memberDestinations[swarmUnit] = holePosition;
					}
					continue;
				}
				Vector3 randomNavmeshPosition = GetRandomNavmeshPosition(holePosition, radius);
				swarmUnit.SetDestination(randomNavmeshPosition);
				if (memberDestinations != null)
				{
					memberDestinations[swarmUnit] = randomNavmeshPosition;
				}
			}
			_presenter.SetUnitsRunning();
			return true;
		}

		public bool TryGetRunAwayDepositHole(out Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole)
		{
			hole = null;
			if (_context.RunAwayDepositHole != null && _context.RunAwayDepositHole.IsReady)
			{
				hole = _context.RunAwayDepositHole;
				return true;
			}
			if (_context.RunAwayPosition.sqrMagnitude <= 0.0001f)
			{
				return false;
			}
			if (!TryGetReachableReadyHole(_context.RunAwayPosition, out hole))
			{
				return false;
			}
			_context.RunAwayDepositHole = hole;
			return true;
		}

		private bool TryGetReachableReadyHole(Vector3 nearPosition, out Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole)
		{
			hole = null;
			if (_context.SwarmKing == null)
			{
				return false;
			}
			NavMeshAgent navMeshAgent = _context.SwarmKing.NavMeshAgent;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			for (int i = 0; i < _ratsHoleRegistry.Holes.Count; i++)
			{
				Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole ratsHole = _ratsHoleRegistry.Holes[i];
				if (!(ratsHole == null) && ratsHole.IsReady && IsReadyHoleReachable(ratsHole, navMeshAgent, ratsHole.AbsorbPointRadius, out var path))
				{
					float length = path.GetLength();
					float sqrMagnitude = (ratsHole.AbsorbWorldPosition - nearPosition).sqrMagnitude;
					if (!(length > num) && (!Mathf.Approximately(length, num) || !(sqrMagnitude >= num2)))
					{
						num = length;
						num2 = sqrMagnitude;
						hole = ratsHole;
					}
				}
			}
			return hole != null;
		}

		private bool IsReadyHoleReachable(Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole)
		{
			if (hole == null || !hole.IsReady || _context.SwarmKing == null)
			{
				return false;
			}
			NavMeshPath path;
			return IsReadyHoleReachable(hole, _context.SwarmKing.NavMeshAgent, hole.AbsorbPointRadius, out path);
		}

		private bool IsReadyHoleReachable(Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole, NavMeshAgent agent, float sampleRadius, out NavMeshPath path)
		{
			path = null;
			if (hole != null && hole.IsReady)
			{
				return _navigationService.TryGetCompletePath(agent, hole.AbsorbWorldPosition, sampleRadius, out path);
			}
			return false;
		}

		private Vector3 ResolveDepositCenter(Vector3 spawnOnMesh, Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole, bool hasHole, float navSampleRadius)
		{
			if (!hasHole)
			{
				return spawnOnMesh;
			}
			return _navigationService.ValidatePointOnNavmesh(hole.AbsorbWorldPosition, navSampleRadius);
		}

		private void AssignCarrierAndEmptyRunAwayDestinations(Vector3 spawnOnMesh, Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole nearestHole, bool hasHole)
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit == null)
				{
					continue;
				}
				if (swarmUnit.HasAttachedItem)
				{
					if (hasHole)
					{
						Vector3 randomNavmeshPosition = GetRandomNavmeshPosition(nearestHole.AbsorbWorldPosition, nearestHole.AbsorbPointRadius);
						swarmUnit.SetDestination(randomNavmeshPosition);
					}
					else
					{
						swarmUnit.SetDestination(spawnOnMesh);
					}
				}
				else
				{
					AssignEmptyUnitAroundKing(swarmUnit);
				}
			}
		}

		public void RefreshEmptyUnitsAroundKing()
		{
			RefreshUnitsPatrolAroundKing(includeCarriers: false);
		}

		public void RefreshStealingIdleUnitsAroundKing()
		{
			RefreshUnitsPatrolAroundKing(includeCarriers: true);
		}

		private void RefreshUnitsPatrolAroundKing(bool includeCarriers)
		{
			if (_context.SwarmKing == null)
			{
				return;
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && (includeCarriers || !swarmUnit.HasAttachedItem) && !_context.ActiveStealTasks.ContainsKey(swarmUnit) && !(swarmUnit.RemainingDistance > _chaseSettings.TargetReachError))
				{
					AssignEmptyUnitAroundKing(swarmUnit);
					_presenter.SetMemberLocomotion(swarmUnit, ShouldMemberRun(swarmUnit));
				}
			}
		}

		public void BeginEmptyUnitsPatrolAroundKing()
		{
			if (_context.SwarmKing == null)
			{
				return;
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && !swarmUnit.HasAttachedItem && !_context.ActiveStealTasks.ContainsKey(swarmUnit))
				{
					AssignEmptyUnitAroundKing(swarmUnit);
					_presenter.SetMemberLocomotion(swarmUnit, isRunning: true);
				}
			}
		}

		public void BeginUnitPatrolAroundKing(CoinRobBehaviour swarmUnit)
		{
			if (!(_context.SwarmKing == null) && !(swarmUnit == null))
			{
				AssignEmptyUnitAroundKing(swarmUnit);
				_presenter.SetMemberLocomotion(swarmUnit, isRunning: true);
			}
		}

		private void AssignEmptyUnitAroundKing(CoinRobBehaviour swarmUnit)
		{
			float swarmRadius = _swarmSettings.SwarmRadius;
			Vector3 randomNavmeshPosition = GetRandomNavmeshPosition(_context.SwarmKing.transform.position, swarmRadius);
			swarmUnit.SetDestination(_navigationService.ValidatePointOnNavmesh(randomNavmeshPosition, swarmRadius));
		}

		public void BeginPlayerAttackFormation(Vector3 playerPosition)
		{
			float swarmRadius = _swarmSettings.SwarmRadius;
			if (!(_context.SwarmKing == null))
			{
				Vector3 destination = _navigationService.ValidatePointOnNavmesh(playerPosition, swarmRadius);
				_context.SwarmKing.SetDestination(destination);
				RefreshUnitsFormationAroundKing();
			}
		}

		public void RefreshUnitsFormationAroundKing()
		{
			if (_context.SwarmKing == null)
			{
				return;
			}
			AssignUnitPatrolDestinationsAround(_context.SwarmKing.transform.position);
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				_presenter.SetMemberLocomotion(swarmUnit, ShouldMemberRun(swarmUnit));
			}
		}

		public void SetUnitsFormationAroundPlayer(Vector3 playerPosition)
		{
			float radius = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			Vector3 center = _navigationService.ValidatePointOnNavmesh(playerPosition, radius);
			CollectActivePatrolUnits(_patrolUnitsBuffer);
			AssignUniquePatrolDestinations(_patrolUnitsBuffer, center, radius);
			foreach (CoinRobBehaviour item in _patrolUnitsBuffer)
			{
				_presenter.SetMemberLocomotion(item, ShouldMemberRun(item));
			}
		}

		public void SetSwarmIdleLocomotion()
		{
			_context.SwarmKing?.StopMovement();
			_presenter.SetUnitsIdle();
		}

		public Vector3 ResolveRunAwayDestination(Vector3 fromPosition)
		{
			float runAwayNavMeshSampleRadius = _chaseSettings.RunAwayNavMeshSampleRadius;
			if (TryGetNearestCoinRobSpawnPosition(fromPosition, out var spawnPosition))
			{
				return _navigationService.ValidatePointOnNavmesh(spawnPosition, runAwayNavMeshSampleRadius);
			}
			Vector3 vector = _navigationService.ValidatePointOnNavmesh(_context.GetPatrolCenter(), runAwayNavMeshSampleRadius);
			if (IsRunAwayDestinationFarEnough(fromPosition, vector))
			{
				return vector;
			}
			if (_navigationService.TryGetRandomNavmeshPosition(fromPosition, _swarmSettings.SwarmRadius * 4f, out var position) && IsRunAwayDestinationFarEnough(fromPosition, position))
			{
				return position;
			}
			Vector3 vector2 = fromPosition - _context.GetPatrolCenter();
			vector2.y = 0f;
			if (vector2.sqrMagnitude <= 0.0001f)
			{
				vector2 = Vector3.forward;
			}
			return fromPosition + vector2.normalized * (_swarmSettings.SwarmRadius * 4f);
		}

		private bool IsRunAwayDestinationFarEnough(Vector3 fromPosition, Vector3 destination)
		{
			return (destination - fromPosition).sqrMagnitude > _swarmSettings.SwarmRadius * _swarmSettings.SwarmRadius;
		}

		private void AssignUnitPatrolDestinationsAround(Vector3 center)
		{
			float radius = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			CollectActivePatrolUnits(_patrolUnitsBuffer);
			if (_patrolUnitsBuffer.Count != 0)
			{
				AssignUniquePatrolDestinations(_patrolUnitsBuffer, center, radius);
			}
		}

		private void CollectActivePatrolUnits(List<CoinRobBehaviour> buffer)
		{
			buffer.Clear();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit != null)
				{
					buffer.Add(swarmUnit);
				}
			}
		}

		private void AssignUniquePatrolDestinations(List<CoinRobBehaviour> units, Vector3 center, float radius)
		{
			_assignedPatrolPoints.Clear();
			int count = units.Count;
			float baseAngle = UnityEngine.Random.Range(0f, MathF.PI * 2f);
			float num = Mathf.Max(0.75f, MathF.PI * 2f * radius / (float)Mathf.Max(count, 1) * 0.45f);
			float minSeparationSqr = num * num;
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = ResolveUniquePatrolPoint(center, radius, baseAngle, i, count, minSeparationSqr);
				_assignedPatrolPoints.Add(vector);
				units[i].SetDestination(vector);
			}
		}

		private Vector3 ResolveUniquePatrolPoint(Vector3 center, float radius, float baseAngle, int unitIndex, int unitCount, float minSeparationSqr)
		{
			float num = MathF.PI * 2f / (float)unitCount;
			for (int i = 0; i < 10; i++)
			{
				float f = baseAngle + (float)unitIndex * num + (float)i * (num / 10f);
				float num2 = radius * UnityEngine.Random.Range(0.55f, 1f);
				Vector3 point = center + new Vector3(Mathf.Cos(f) * num2, 0f, Mathf.Sin(f) * num2);
				Vector3 vector = _navigationService.ValidatePointOnNavmesh(point, radius);
				if (IsFarEnoughFromAssigned(vector, minSeparationSqr))
				{
					return vector;
				}
			}
			float f2 = baseAngle + (float)unitIndex * num;
			Vector3 point2 = center + new Vector3(Mathf.Cos(f2) * radius, 0f, Mathf.Sin(f2) * radius);
			return _navigationService.ValidatePointOnNavmesh(point2, radius);
		}

		private bool IsFarEnoughFromAssigned(Vector3 candidate, float minSeparationSqr)
		{
			for (int i = 0; i < _assignedPatrolPoints.Count; i++)
			{
				Vector3 vector = candidate - _assignedPatrolPoints[i];
				vector.y = 0f;
				if (vector.sqrMagnitude < minSeparationSqr)
				{
					return false;
				}
			}
			return true;
		}

		private bool TryGetNearestCoinRobSpawnPosition(Vector3 fromPosition, out Vector3 spawnPosition)
		{
			spawnPosition = Vector3.zero;
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(EnemyType.CoinRobSwarm, out var value) || value.Count == 0)
			{
				return false;
			}
			EnemySpawnPointData enemySpawnPointData = null;
			float num = float.MaxValue;
			for (int i = 0; i < value.Count; i++)
			{
				EnemySpawnPointData enemySpawnPointData2 = value[i];
				if (!enemySpawnPointData2.IsOneTimeSpawnPoint)
				{
					float sqrMagnitude = (enemySpawnPointData2.Position - fromPosition).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						enemySpawnPointData = enemySpawnPointData2;
					}
				}
			}
			if (enemySpawnPointData == null)
			{
				return false;
			}
			spawnPosition = enemySpawnPointData.Position;
			return true;
		}

		public void RefreshUnitPatrolAroundKing()
		{
			if (!(_context.SwarmKing == null))
			{
				AssignUnitPatrolDestinationsAround(_context.SwarmKing.transform.position);
			}
		}

		public void RefreshUnitsPatrolAroundKingOnArrival()
		{
			if (!(_context.SwarmKing == null))
			{
				RefreshUnitsPatrolAroundPointOnArrival(_context.SwarmKing.transform.position);
			}
		}

		public void RefreshUnitsPatrolAroundPointOnArrival(Vector3 center)
		{
			float radius = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			float targetReachError = _chaseSettings.TargetReachError;
			_patrolUnitsBuffer.Clear();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && !swarmUnit.PathPending && (!swarmUnit.HasPath || swarmUnit.RemainingDistance <= targetReachError))
				{
					_wanderingUnitStuckTimers.Remove(swarmUnit);
					_patrolUnitsBuffer.Add(swarmUnit);
				}
			}
			if (_patrolUnitsBuffer.Count != 0)
			{
				AssignUniquePatrolDestinations(_patrolUnitsBuffer, center, radius);
			}
		}

		public void RepathStuckWanderingUnits(float deltaTime)
		{
			if (!(_context.SwarmKing == null))
			{
				RepathStuckUnitsAroundPoint(_context.SwarmKing.transform.position, deltaTime);
			}
		}

		public void RepathStuckUnitsAroundKingToward(Vector3 towardTarget, float deltaTime)
		{
			if (_context.SwarmKing == null)
			{
				return;
			}
			PruneWanderingStuckTimers();
			float num = Mathf.Max(0.05f, _swarmSettings.WanderingUnitStuckRepathTime);
			float targetReachError = _chaseSettings.TargetReachError;
			_patrolUnitsBuffer.Clear();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit == null)
				{
					continue;
				}
				if (!IsUnitStuckOnPatrol(swarmUnit, targetReachError))
				{
					_wanderingUnitStuckTimers.Remove(swarmUnit);
					continue;
				}
				float value;
				float num2 = (_wanderingUnitStuckTimers.TryGetValue(swarmUnit, out value) ? (value + deltaTime) : deltaTime);
				_wanderingUnitStuckTimers[swarmUnit] = num2;
				if (!(num2 < num))
				{
					_wanderingUnitStuckTimers[swarmUnit] = 0f;
					_patrolUnitsBuffer.Add(swarmUnit);
				}
			}
			for (int i = 0; i < _patrolUnitsBuffer.Count; i++)
			{
				CoinRobBehaviour coinRobBehaviour = _patrolUnitsBuffer[i];
				RepathUnitAroundKingToward(coinRobBehaviour, towardTarget, i, _patrolUnitsBuffer.Count);
				_presenter.SetMemberLocomotion(coinRobBehaviour, isRunning: true);
			}
		}

		public void RepathUnitAroundKingToward(CoinRobBehaviour unit, Vector3 towardTarget, int slotIndex = 0, int slotCount = 1)
		{
			if (!(unit == null) && !(_context.SwarmKing == null))
			{
				unit.SetDestination(GetKingDetourPointToward(unit.transform.position, towardTarget, slotIndex, slotCount));
			}
		}

		public void SetUnitDestinationNear(CoinRobBehaviour unit, Vector3 target)
		{
			if (!(unit == null))
			{
				unit.SetDestination(_navigationService.ValidatePointOnNavmesh(target, _swarmSettings.SwarmRadius));
			}
		}

		private Vector3 GetKingDetourPointToward(Vector3 unitPosition, Vector3 towardTarget, int slotIndex, int slotCount)
		{
			Vector3 position = _context.SwarmKing.transform.position;
			float num = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			Vector3 vector = towardTarget - position;
			vector.y = 0f;
			Vector3 vector2 = ((vector.sqrMagnitude > 0.0001f) ? vector.normalized : Vector3.forward);
			Vector3 vector3 = Vector3.Cross(Vector3.up, vector2);
			if (vector3.sqrMagnitude <= 0.0001f)
			{
				vector3 = Vector3.right;
			}
			vector3.Normalize();
			Vector3 lhs = position - unitPosition;
			lhs.y = 0f;
			float num2 = ((Vector3.Dot(lhs, vector3) >= 0f) ? (-1f) : 1f);
			float num3 = ((slotCount <= 1) ? num2 : ((slotIndex % 2 == 0) ? num2 : (0f - num2)));
			float sideDistance = num * (1.15f + 0.35f * (float)(slotIndex / 2));
			float forwardDistance = num * 0.9f;
			Vector3 vector4 = BuildKingDetourCandidate(position, vector3, vector2, num3, sideDistance, forwardDistance, num);
			Vector3 vector5 = BuildKingDetourCandidate(position, vector3, vector2, 0f - num3, sideDistance, forwardDistance, num);
			float num4 = HorizontalDistance(vector4, position);
			float num5 = HorizontalDistance(vector5, position);
			float num6 = num * 0.85f;
			if (num4 >= num6)
			{
				return vector4;
			}
			if (num5 > num4)
			{
				return vector5;
			}
			return vector4;
		}

		private Vector3 BuildKingDetourCandidate(Vector3 kingPosition, Vector3 side, Vector3 forward, float sideSign, float sideDistance, float forwardDistance, float sampleRadius)
		{
			Vector3 point = kingPosition + side * (sideSign * sideDistance) + forward * forwardDistance;
			return _navigationService.ValidatePointOnNavmesh(point, sampleRadius);
		}

		private float HorizontalDistance(Vector3 a, Vector3 b)
		{
			Vector3 vector = a - b;
			vector.y = 0f;
			return vector.magnitude;
		}

		public void RepathStuckUnitsAroundPoint(Vector3 center, float deltaTime)
		{
			PruneWanderingStuckTimers();
			float num = Mathf.Max(0.05f, _swarmSettings.WanderingUnitStuckRepathTime);
			float targetReachError = _chaseSettings.TargetReachError;
			float radius = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			_patrolUnitsBuffer.Clear();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit == null)
				{
					continue;
				}
				if (!IsUnitStuckOnPatrol(swarmUnit, targetReachError))
				{
					_wanderingUnitStuckTimers.Remove(swarmUnit);
					continue;
				}
				float value;
				float num2 = (_wanderingUnitStuckTimers.TryGetValue(swarmUnit, out value) ? (value + deltaTime) : deltaTime);
				_wanderingUnitStuckTimers[swarmUnit] = num2;
				if (!(num2 < num))
				{
					_wanderingUnitStuckTimers[swarmUnit] = 0f;
					_patrolUnitsBuffer.Add(swarmUnit);
				}
			}
			if (_patrolUnitsBuffer.Count == 0)
			{
				return;
			}
			AssignUniquePatrolDestinations(_patrolUnitsBuffer, center, radius);
			foreach (CoinRobBehaviour item in _patrolUnitsBuffer)
			{
				_presenter.SetMemberLocomotion(item, isRunning: true);
			}
		}

		private bool IsUnitStuckOnPatrol(CoinRobBehaviour unit, float reachDistance)
		{
			if (unit.PathPending)
			{
				return false;
			}
			if (unit.PathStatus == NavMeshPathStatus.PathInvalid || unit.PathStatus == NavMeshPathStatus.PathPartial)
			{
				return true;
			}
			if (!unit.HasPath)
			{
				return false;
			}
			if (unit.RemainingDistance <= reachDistance)
			{
				return false;
			}
			return unit.Velocity.sqrMagnitude <= 0.01f;
		}

		private void PruneWanderingStuckTimers()
		{
			if (_wanderingUnitStuckTimers.Count == 0)
			{
				return;
			}
			_patrolUnitsBuffer.Clear();
			foreach (CoinRobBehaviour key in _wanderingUnitStuckTimers.Keys)
			{
				if (key == null || !IsActiveSwarmUnit(key))
				{
					_patrolUnitsBuffer.Add(key);
				}
			}
			for (int i = 0; i < _patrolUnitsBuffer.Count; i++)
			{
				_wanderingUnitStuckTimers.Remove(_patrolUnitsBuffer[i]);
			}
		}

		private bool IsActiveSwarmUnit(CoinRobBehaviour unit)
		{
			IReadOnlyList<CoinRobBehaviour> swarmUnits = _context.SwarmUnits;
			for (int i = 0; i < swarmUnits.Count; i++)
			{
				if ((object)swarmUnits[i] == unit)
				{
					return true;
				}
			}
			return false;
		}

		public void KeepUnitsFollowingKingWhileMoving()
		{
			if (_context.SwarmKing == null || _context.SwarmKing.PathPending || !_context.SwarmKing.HasPath)
			{
				return;
			}
			Vector3 position = _context.SwarmKing.transform.position;
			float num = Mathf.Max(0.01f, _swarmSettings.SwarmRadius);
			float targetReachError = _chaseSettings.TargetReachError;
			float num2 = num * 2f;
			_patrolUnitsBuffer.Clear();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null))
				{
					float num3 = Vector3.Distance(swarmUnit.transform.position, position);
					bool num4 = !swarmUnit.PathPending && (!swarmUnit.HasPath || swarmUnit.RemainingDistance <= targetReachError);
					bool flag = num3 > num2;
					if (num4 || flag)
					{
						_patrolUnitsBuffer.Add(swarmUnit);
					}
				}
			}
			if (_patrolUnitsBuffer.Count != 0)
			{
				AssignUniquePatrolDestinations(_patrolUnitsBuffer, position, num);
			}
		}

		public void SyncPatrolLocomotionAnimations(float velocityThreshold = 0.1f)
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				SyncMemberPatrolLocomotion(swarmUnit, velocityThreshold);
			}
		}

		private void SyncMemberPatrolLocomotion(CoinRobBehaviour member, float velocityThreshold)
		{
			if (!(member == null))
			{
				_presenter.SetMemberLocomotion(member, ShouldMemberRun(member, velocityThreshold));
			}
		}

		public bool ShouldMemberRun(CoinRobBehaviour member, float velocityThreshold = 0.1f)
		{
			if (member == null)
			{
				return false;
			}
			if (member.PathPending)
			{
				return true;
			}
			if (member.Velocity.sqrMagnitude > velocityThreshold * velocityThreshold)
			{
				return true;
			}
			if (!member.HasPath)
			{
				return false;
			}
			float num = member.StoppingDistance + 0.15f;
			return member.RemainingDistance > num;
		}

		public bool IsSwarmKingNearDestination(float reachError)
		{
			if (_context.SwarmKing != null)
			{
				return _context.SwarmKing.RemainingDistance <= reachError;
			}
			return false;
		}

		public bool IsKingReadyForNextPatrol(float velocityThreshold = 0.1f)
		{
			if (_context.SwarmKing == null)
			{
				return false;
			}
			if (_context.SwarmKing.PathPending)
			{
				return false;
			}
			if (_context.SwarmKing.HasPath)
			{
				return HasSwarmKingReachedPatrolDestination(velocityThreshold);
			}
			return _context.SwarmKing.Velocity.sqrMagnitude <= velocityThreshold * velocityThreshold;
		}

		public bool HasSwarmKingReachedPatrolDestination(float velocityThreshold = 0.1f)
		{
			if (_context.SwarmKing == null || _context.SwarmKing.PathPending || !_context.SwarmKing.HasPath)
			{
				return false;
			}
			if (_context.SwarmKing.Velocity.sqrMagnitude > velocityThreshold * velocityThreshold)
			{
				return false;
			}
			float num = _context.SwarmKing.StoppingDistance + 0.15f;
			return _context.SwarmKing.RemainingDistance <= num;
		}

		public void PlayInitialAnimation(CoinRobBehaviour coinRobBehaviour)
		{
			AnimatorStateInfo currentAnimatorStateInfo = coinRobBehaviour.NetworkedAnimator.Animator.GetCurrentAnimatorStateInfo(coinRobBehaviour.AnimationsLayer);
			coinRobBehaviour.NetworkedAnimator.Play(currentAnimatorStateInfo.fullPathHash, -1, GetRandomStartAnimationTime(coinRobBehaviour.transform.position));
		}

		private Vector3 GetOffsetInSwarm(Vector3 center, CoinRobBehaviour swarmUnit)
		{
			return swarmUnit.transform.position - center;
		}

		private float GetRandomStartAnimationTime(Vector3 position)
		{
			return (float)new System.Random(GetPositionHashCode(position)).NextDouble();
		}

		private int GetPositionHashCode(Vector3 position)
		{
			return HashCode.Combine(position.x, position.y, position.z);
		}
	}
}
