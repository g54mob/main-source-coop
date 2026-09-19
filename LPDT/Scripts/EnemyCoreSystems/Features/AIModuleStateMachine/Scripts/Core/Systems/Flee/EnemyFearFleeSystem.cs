using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Flee
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyFearFleeSystem : MonoSystem
	{
		private const int FLEE_POSITION_ATTEMPTS = 20;

		private const float FLEE_MIN_DISTANCE_FROM_CENTER_FACTOR = 0.35f;

		private const float FLEE_AVERAGE_AVOID_DISTANCE_WEIGHT = 5f;

		private const float FLEE_TOO_CLOSE_PENALTY_RADIUS = 3f;

		private const float FLEE_TOO_CLOSE_PENALTY_WEIGHT = 10f;

		private readonly List<Vector3> _avoidPositions = new List<Vector3>();

		private readonly List<EnemyFearFleePlayerCache> _playerLookDetectionCache = new List<EnemyFearFleePlayerCache>();

		[SerializeField]
		private float _fleeRadius = 25f;

		[SerializeField]
		private float _visibilityDistance = 25f;

		[SerializeField]
		private float _visibilityCheckInterval = 0.2f;

		[SerializeField]
		private float _destinationUpdateInterval = 0.5f;

		[SerializeField]
		private float _destinationReachedDistance = 1.5f;

		[SerializeField]
		private float _timeout = 12f;

		[SerializeField]
		private bool _completeOnTimeout = true;

		private IMovementContext _movementContext;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private INavigationService _navigationService;

		private bool _isEnabled;

		private bool _hasFleeDestination;

		private bool _hasVisibilitySample;

		private bool _isVisibleByPlayers;

		private float _destinationTimer;

		private float _visibilityTimer;

		private float _elapsedTime;

		public override bool IsEnabled => _isEnabled;

		public event Action OnCompleted;

		[Inject]
		public void InjectDependencies(IMovementContext movementContext, SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService)
		{
			_movementContext = movementContext;
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			if (base.HasStateAuthority)
			{
				Clear();
				_isEnabled = true;
				RebuildPlayerLookDetectionCache();
				_hasFleeDestination = TryPrepareFleeDestination();
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = false;
				Clear();
			}
		}

		public override void Clear()
		{
			_playerLookDetectionCache.Clear();
			_avoidPositions.Clear();
			_hasFleeDestination = false;
			_hasVisibilitySample = false;
			_isVisibleByPlayers = true;
			_destinationTimer = Mathf.Max(0f, _destinationUpdateInterval);
			_visibilityTimer = 0f;
			_elapsedTime = 0f;
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				return;
			}
			float num = ((base.Runner != null) ? base.Runner.DeltaTime : Time.fixedDeltaTime);
			_elapsedTime += Mathf.Max(0f, num);
			if (!RefreshVisibilitySampleIfNeeded(num))
			{
				Complete();
				return;
			}
			float num2 = (_completeOnTimeout ? _timeout : 0f);
			if (num2 > 0f && _elapsedTime >= num2)
			{
				Complete();
				return;
			}
			_destinationTimer += Mathf.Max(0f, num);
			float num3 = Mathf.Max(0f, _destinationUpdateInterval);
			if (!(_destinationTimer < num3) && (!_hasFleeDestination || HasReachedDestination()))
			{
				_destinationTimer = 0f;
				_hasFleeDestination = TryPrepareFleeDestination();
			}
		}

		private void Complete()
		{
			_isEnabled = false;
			this.OnCompleted?.Invoke();
		}

		private bool RefreshVisibilitySampleIfNeeded(float deltaTime)
		{
			float num = Mathf.Max(0f, _visibilityCheckInterval);
			_visibilityTimer += Mathf.Max(0f, deltaTime);
			if (_hasVisibilitySample && num > 0f && _visibilityTimer < num)
			{
				return _isVisibleByPlayers;
			}
			_visibilityTimer = 0f;
			_isVisibleByPlayers = IsVisibleByPlayers();
			_hasVisibilitySample = true;
			return _isVisibleByPlayers;
		}

		private bool IsVisibleByPlayers()
		{
			float num = Mathf.Max(1f, _visibilityDistance);
			EnsurePlayerLookDetectionCache();
			Transform transform = _movementContext.NavMeshAgent.transform;
			foreach (EnemyFearFleePlayerCache item in _playerLookDetectionCache)
			{
				if (item.LookDetection != null && item.LookDetection.IsLookingAtObject(transform, angleCullEnabled: false, num / 2f))
				{
					return true;
				}
				PlayerReachableData reachableData = null;
				if (_navigationService.IsPlayerOnReachablePoint(item.PlayerRef, _movementContext.NavMeshAgent, num, out reachableData))
				{
					return true;
				}
				if (reachableData != null && Vector3.Distance(transform.position, reachableData.NavMeshProjectedHit.position) < num / 2f)
				{
					return true;
				}
			}
			return false;
		}

		private void EnsurePlayerLookDetectionCache()
		{
			if (!IsPlayerLookDetectionCacheValid())
			{
				RebuildPlayerLookDetectionCache();
			}
		}

		private bool IsPlayerLookDetectionCacheValid()
		{
			if (_playerLookDetectionCache.Count != _spawnedPlayersModel.Players.Count)
			{
				return false;
			}
			foreach (EnemyFearFleePlayerCache item in _playerLookDetectionCache)
			{
				if (!_spawnedPlayersModel.Players.TryGetValue(item.PlayerRef, out var value))
				{
					return false;
				}
				if (value == null || value.NetworkObject == null || !value.NetworkObject.IsValid || value.NetworkObject != item.NetworkObject)
				{
					return false;
				}
			}
			return true;
		}

		private void RebuildPlayerLookDetectionCache()
		{
			_playerLookDetectionCache.Clear();
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (value != null && !(value.NetworkObject == null) && value.NetworkObject.IsValid)
				{
					value.NetworkObject.TryGetComponent<PlayerLookDetection>(out var component);
					_playerLookDetectionCache.Add(new EnemyFearFleePlayerCache(player.Key, value.NetworkObject, component));
				}
			}
		}

		private bool TryPrepareFleeDestination()
		{
			NavMeshAgent navMeshAgent = _movementContext.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			Vector3 position = navMeshAgent.transform.position;
			float num = Mathf.Max(1f, _fleeRadius);
			CollectAvoidPositions();
			Vector3 bestPosition = default(Vector3);
			bool flag = false;
			if (_avoidPositions.Count > 0 && _navigationService.TryGetRandomSafeNavmeshPosition(position, num, 20, _avoidPositions, navMeshAgent.agentTypeID, -1, out bestPosition, num * 0.35f, 1f, 5f, 3f, 10f) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag && TryGetOppositeFleePosition(num, out bestPosition) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag && _navigationService.TryGetRandomNavmeshPosition(position, num, out bestPosition) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			_movementContext.SetTargetPosition(bestPosition);
			_movementContext.SetTargetPositionCompleted(isCompleted: false);
			_movementContext.NeedToFindTargetPosition = false;
			navMeshAgent.stoppingDistance = 0f;
			navMeshAgent.isStopped = false;
			navMeshAgent.ResetPath();
			navMeshAgent.SetDestination(bestPosition);
			return true;
		}

		private void CollectAvoidPositions()
		{
			_avoidPositions.Clear();
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				TryAddAvoidPosition(player.Key, player.Value);
			}
		}

		private void TryAddAvoidPosition(PlayerRef playerRef, PlayerDataHolder holder)
		{
			if (holder != null && !(holder.NetworkObject == null) && holder.NetworkObject.IsValid)
			{
				if (_navigationService.TryGetPlayerTrackingPosition(playerRef, out var position))
				{
					_avoidPositions.Add(position);
				}
				else
				{
					_avoidPositions.Add(holder.NetworkObject.transform.position);
				}
			}
		}

		private bool TryGetOppositeFleePosition(float fleeRadius, out Vector3 fleeTarget)
		{
			fleeTarget = default(Vector3);
			if (_avoidPositions.Count == 0)
			{
				return false;
			}
			Vector3 position = _movementContext.NavMeshAgent.transform.position;
			Vector3 vector = _avoidPositions[0];
			float num = GetHorizontalDistanceSqr(position, vector);
			for (int i = 1; i < _avoidPositions.Count; i++)
			{
				float horizontalDistanceSqr = GetHorizontalDistanceSqr(position, _avoidPositions[i]);
				if (!(horizontalDistanceSqr >= num))
				{
					num = horizontalDistanceSqr;
					vector = _avoidPositions[i];
				}
			}
			Vector3 vector2 = position - vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = UnityEngine.Random.insideUnitSphere;
			}
			vector2.y = 0f;
			vector2.Normalize();
			Vector3 point = position + vector2 * fleeRadius;
			return _navigationService.TryGetPointOnNavMeshProjected(point, fleeRadius, out fleeTarget);
		}

		private bool HasReachedDestination()
		{
			NavMeshAgent navMeshAgent = _movementContext.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.hasPath)
			{
				return true;
			}
			return Vector3.Distance(navMeshAgent.pathEndPosition, navMeshAgent.transform.position) <= Mathf.Max(0.1f, _destinationReachedDistance);
		}

		private static bool HasCompletePath(NavMeshAgent agent, Vector3 destination)
		{
			if (agent == null || !agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!agent.CalculatePath(destination, navMeshPath))
			{
				return false;
			}
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		private static float GetHorizontalDistanceSqr(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return new Vector2(vector.x, vector.z).sqrMagnitude;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
