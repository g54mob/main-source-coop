using System.Collections.Generic;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.NavigationModule.Scripts;
using Features.OcclusionModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.QuotaModule.Scripts.PhysicsContainer;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterContext : MonoBehaviour
	{
		private const float BOAT_NAV_SAMPLE_RADIUS = 12f;

		private const float STEERING_DIRECTION_EPSILON = 0.0025f;

		private const float MIN_STEERING_SPEED = 0.5f;

		private const float LOOK_AHEAD_REFRESH_INTERVAL = 0.2f;

		private const float BLOCK_PROBE_HEIGHT_FACTOR = 0.5f;

		private const int BLOCK_PROBE_HIT_CAPACITY = 8;

		private const float ROAM_CLAMP_EPSILON = 0.01f;

		private const float ROAM_NAV_SAMPLE_RADIUS = 6f;

		private const float GOAL_FLOOR_SAMPLE_RADIUS = 3f;

		private const float THREAT_SIGHT_HEIGHT = 1f;

		private const float FLEE_PROBE_SAMPLE_RADIUS = 1.5f;

		private const float MIN_FLEE_PROBE_DISTANCE = 1.5f;

		private static readonly float[] _fleeProbeAngles = new float[10] { 0f, 25f, -25f, 55f, -55f, 90f, -90f, 130f, -130f, 180f };

		private static readonly float[] _fleeProbeDistanceScales = new float[4] { 1f, 0.6f, 0.35f, 0.2f };

		private static int _threatSightMask = -1;

		[SerializeField]
		private EnemyItemHolder _itemHolder;

		[SerializeField]
		private Transform _carryAnchor;

		[SerializeField]
		private Transform _handSocket;

		[SerializeField]
		private Transform _cartSlot;

		private readonly Vector3[] _pathCorners = new Vector3[8];

		private readonly RaycastHit[] _blockProbeHits = new RaycastHit[8];

		private float _baseSpeed;

		private float _lookAheadRefreshTimer;

		private int _pathCornerCount;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private INavigationService _navigationService;

		private QuotaContainerLocationModel _quotaContainerLocationModel;

		private IPlayerStateService _playerStateService;

		private MonkeyPorterSettings _monkeyPorterSettings;

		private PlayerRef _followedPlayer = PlayerRef.None;

		private static int ThreatSightMask
		{
			get
			{
				if (_threatSightMask < 0)
				{
					_threatSightMask = LayerMask.GetMask("Default", "Ground", "Floor", "Wall");
				}
				return _threatSightMask;
			}
		}

		[field: SerializeField]
		public NavMeshAgent Agent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public Transform CartSocket { get; private set; }

		[field: Header("Audio")]
		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: SerializeField]
		public EventReference ComplainSound { get; private set; }

		[field: SerializeField]
		public EventReference PanicSound { get; private set; }

		public EnemyItemHolder ItemHolder => _itemHolder;

		public Transform CarryAnchor => _carryAnchor;

		public Transform HandSocket => _handSocket;

		public Transform CartSlot => _cartSlot;

		public bool HasBoatDrop => _quotaContainerLocationModel.HasContainer;

		public bool HasBoatApproach => _quotaContainerLocationModel.HasApproach;

		public bool IsAgentOnOffMeshLink
		{
			get
			{
				if (Agent.enabled && Agent.isOnNavMesh)
				{
					return Agent.isOnOffMeshLink;
				}
				return false;
			}
		}

		public float AgentDesiredSpeed
		{
			get
			{
				if (!Agent.enabled || !Agent.isOnNavMesh)
				{
					return 0f;
				}
				return Agent.desiredVelocity.magnitude;
			}
		}

		public NavMeshQueryFilter AgentQueryFilter => new NavMeshQueryFilter
		{
			agentTypeID = Agent.agentTypeID,
			areaMask = Agent.areaMask
		};

		public float AgentRadius => Agent.radius;

		public int AvoidancePriority => Agent.avoidancePriority;

		public float AgentVelocity
		{
			get
			{
				if (!Agent.enabled)
				{
					return 0f;
				}
				return Agent.velocity.magnitude;
			}
		}

		public bool HasActivePath
		{
			get
			{
				if (Agent.enabled && Agent.isOnNavMesh)
				{
					if (!Agent.pathPending)
					{
						return Agent.hasPath;
					}
					return true;
				}
				return false;
			}
		}

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService, QuotaContainerLocationModel quotaContainerLocationModel, IPlayerStateService playerStateService, MonkeyPorterSettings monkeyPorterSettings)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
			_quotaContainerLocationModel = quotaContainerLocationModel;
			_playerStateService = playerStateService;
			_monkeyPorterSettings = monkeyPorterSettings;
		}

		public void ValidateRequiredReferences()
		{
			AssertAssigned(Agent, "Agent");
			AssertAssigned(Animator, "Animator");
			AssertAssigned(CartSocket, "CartSocket");
			AssertAssigned(ItemHolder, "ItemHolder");
			AssertAssigned(CarryAnchor, "CarryAnchor");
			AssertAssigned(HandSocket, "HandSocket");
			AssertAssigned(CartSlot, "CartSlot");
		}

		public void MoveCarryAnchorToHand()
		{
			CarryAnchor.SetPositionAndRotation(HandSocket.position, HandSocket.rotation);
		}

		public void MoveCarryAnchorToCart()
		{
			CarryAnchor.SetPositionAndRotation(CartSlot.position, CartSlot.rotation);
		}

		public void SetSpeed(float speed)
		{
			_baseSpeed = speed;
			Agent.speed = speed;
		}

		public void ApplySpeedScale(float scale)
		{
			if (Agent.enabled)
			{
				if (_baseSpeed <= 0f)
				{
					_baseSpeed = Agent.speed;
				}
				Agent.speed = ((scale > 0f) ? Mathf.Max(_baseSpeed * scale, 0.5f) : 0f);
			}
		}

		public void SetAgentRotationControl(bool isAgentControlled)
		{
			Agent.updateRotation = isAgentControlled;
		}

		public void MoveAgentBy(Vector3 offset)
		{
			if (Agent.enabled && Agent.isOnNavMesh)
			{
				Agent.Move(offset);
			}
		}

		public bool IsGoalReachable(Vector3 goal)
		{
			if (!Agent.enabled || !Agent.isOnNavMesh)
			{
				return true;
			}
			NavMeshPath path;
			return _navigationService.TryGetCompletePath(Agent, goal, out path);
		}

		public bool HasLineOfSightTo(Transform target)
		{
			if (target == null)
			{
				return false;
			}
			Vector3 end = target.position + Vector3.up * 1f;
			if (!Physics.Linecast(base.transform.position, end, out var hitInfo, ThreatSightMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return hitInfo.transform.root == target.root;
		}

		public bool TrySampleAgentPosition(Vector3 desired, float maxDistance, out Vector3 position)
		{
			NavMeshHit hit;
			bool flag = NavMesh.SamplePosition(desired, out hit, maxDistance, AgentQueryFilter);
			position = (flag ? hit.position : desired);
			return flag;
		}

		public bool TryGetDirectedFleePosition(Vector3 threatPosition, float radius, bool mustIncreaseThreatDistance, out Vector3 position)
		{
			position = default(Vector3);
			Vector3 position2 = base.transform.position;
			Vector3 vector = position2 - threatPosition;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0025f)
			{
				vector = base.transform.forward;
			}
			vector.Normalize();
			float sqrMagnitude = (position2 - threatPosition).sqrMagnitude;
			for (int i = 0; i < _fleeProbeDistanceScales.Length; i++)
			{
				float num = radius * _fleeProbeDistanceScales[i];
				if (num < 1.5f)
				{
					continue;
				}
				for (int j = 0; j < _fleeProbeAngles.Length; j++)
				{
					Vector3 vector2 = Quaternion.Euler(0f, _fleeProbeAngles[j], 0f) * vector;
					if (TrySampleAgentPosition(position2 + vector2 * num, 1.5f, out var position3) && (!mustIncreaseThreatDistance || !((position3 - threatPosition).sqrMagnitude <= sqrMagnitude)) && !((position3 - position2).sqrMagnitude < 2.25f) && IsGoalReachable(position3))
					{
						position = position3;
						return true;
					}
				}
			}
			return false;
		}

		public void SetAvoidancePriority(int priority)
		{
			Agent.avoidancePriority = priority;
		}

		public bool IsPlayerInTheWay(Vector3 direction, float distance)
		{
			direction.y = 0f;
			if (!Agent.enabled || direction.sqrMagnitude < 0.0025f)
			{
				return false;
			}
			int num = Physics.SphereCastNonAlloc(base.transform.position + Vector3.up * (Agent.baseOffset * 0.5f), Agent.radius, direction.normalized, _blockProbeHits, distance, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (IsPlayerCollider(_blockProbeHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryClampGoalToAllowedArea(Vector3 goal, Vector3 home, float roamRadius, float maxHeightDelta, bool hasFloorAnchor, float floorAnchor, out Vector3 allowedGoal)
		{
			Vector3 position = ResolveAllowedGoal(goal, home, roamRadius, maxHeightDelta, hasFloorAnchor, floorAnchor);
			bool flag = (position - goal).sqrMagnitude > 0.01f;
			if (flag)
			{
				TrySampleAgentPosition(position, 6f, out position);
			}
			allowedGoal = (flag ? position : goal);
			return flag;
		}

		public bool TryResolveFloorAnchor(Vector3 home, out float floorAnchor)
		{
			floorAnchor = 0f;
			if (!OcclusionRoomQuery.TryGetContainingRoom(base.transform.position, out var containingRoom))
			{
				return false;
			}
			if (OcclusionRoomQuery.TryGetContainingRoom(home, out var containingRoom2) && containingRoom == containingRoom2)
			{
				return false;
			}
			floorAnchor = containingRoom.RendererBounds.min.y;
			return true;
		}

		private Vector3 ResolveAllowedGoal(Vector3 goal, Vector3 home, float roamRadius, float maxHeightDelta, bool hasFloorAnchor, float floorAnchor)
		{
			if (hasFloorAnchor)
			{
				return ClampToFloor(goal, floorAnchor, home.y, maxHeightDelta);
			}
			if (OcclusionRoomQuery.TryGetContainingRoom(goal, out var _))
			{
				return goal;
			}
			return ClampToLeash(goal, home, roamRadius, maxHeightDelta);
		}

		private Vector3 ClampToFloor(Vector3 goal, float floorAnchor, float spawnHeight, float maxHeightDelta)
		{
			float num = ResolveGoalFloorHeight(goal);
			if (!(Mathf.Abs(num - floorAnchor) <= maxHeightDelta) && !(Mathf.Abs(num - spawnHeight) <= maxHeightDelta))
			{
				return base.transform.position;
			}
			return goal;
		}

		private float ResolveGoalFloorHeight(Vector3 goal)
		{
			if (TrySampleAgentPosition(goal, 3f, out var position))
			{
				return position.y;
			}
			if (!OcclusionRoomQuery.TryGetFloorHeight(goal, out var floorHeight))
			{
				return goal.y;
			}
			return floorHeight;
		}

		private Vector3 ClampToLeash(Vector3 goal, Vector3 home, float roamRadius, float maxHeightDelta)
		{
			Vector3 vector = goal - home;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			if (roamRadius > 0f && vector2.magnitude > roamRadius)
			{
				vector2 = vector2.normalized * roamRadius;
			}
			return home + vector2 + Vector3.up * Mathf.Clamp(vector.y, 0f - maxHeightDelta, maxHeightDelta);
		}

		private bool IsPlayerCollider(Collider other)
		{
			if (other == null)
			{
				return false;
			}
			Transform root = other.transform.root;
			foreach (PlayerDataHolder value in _spawnedPlayersModel.Players.Values)
			{
				if (!(value.NetworkObject == null) && value.NetworkObject.transform.root == root)
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetSteeringDirection(float lookAheadDistance, float deltaTime, out Vector3 direction)
		{
			direction = Vector3.zero;
			if (!Agent.enabled || !Agent.isOnNavMesh || Agent.pathPending || !Agent.hasPath)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Vector3 vector = Agent.steeringTarget - position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0025f)
			{
				vector = Agent.desiredVelocity;
				vector.y = 0f;
			}
			if (vector.sqrMagnitude < 0.0025f)
			{
				return false;
			}
			direction = vector.normalized;
			BlendFollowingCorner(lookAheadDistance, deltaTime, position, vector.magnitude, ref direction);
			return true;
		}

		private void BlendFollowingCorner(float lookAheadDistance, float deltaTime, Vector3 position, float distanceToSteeringTarget, ref Vector3 direction)
		{
			if (lookAheadDistance <= 0f || distanceToSteeringTarget >= lookAheadDistance || !TryGetFollowingCorner(deltaTime, out var corner))
			{
				return;
			}
			Vector3 vector = corner - position;
			vector.y = 0f;
			if (!(vector.sqrMagnitude < 0.0025f))
			{
				float t = 1f - distanceToSteeringTarget / lookAheadDistance;
				Vector3 vector2 = Vector3.Lerp(direction, vector.normalized, t);
				if (!(vector2.sqrMagnitude < 0.0025f))
				{
					direction = vector2.normalized;
				}
			}
		}

		private bool TryGetFollowingCorner(float deltaTime, out Vector3 corner)
		{
			corner = default(Vector3);
			_lookAheadRefreshTimer -= deltaTime;
			if (_lookAheadRefreshTimer <= 0f)
			{
				_lookAheadRefreshTimer = 0.2f;
				_pathCornerCount = Agent.path.GetCornersNonAlloc(_pathCorners);
			}
			if (_pathCornerCount < 3)
			{
				return false;
			}
			corner = _pathCorners[2];
			return true;
		}

		public void SetStoppingDistance(float stoppingDistance)
		{
			Agent.stoppingDistance = stoppingDistance;
		}

		public void MoveToPosition(Vector3 position)
		{
			if (Agent.enabled && Agent.isOnNavMesh)
			{
				NavMeshHit hit;
				Vector3 destination = (_navigationService.IsPointOnNavMeshProjected(position, Agent, out hit) ? hit.position : position);
				Agent.isStopped = false;
				Agent.SetDestination(destination);
			}
		}

		public void StopAgent()
		{
			if (Agent.enabled && Agent.isOnNavMesh)
			{
				Agent.isStopped = true;
				Agent.ResetPath();
			}
		}

		public bool WarpOntoNavMesh(float sampleRadius)
		{
			if (!TrySampleAgentPosition(base.transform.position, sampleRadius, out var position))
			{
				return false;
			}
			Agent.Warp(position);
			return Agent.isOnNavMesh;
		}

		public bool AgentReachedDestination(float arriveDistance)
		{
			if (!Agent.enabled || !Agent.isOnNavMesh)
			{
				return false;
			}
			if (Agent.pathPending || !Agent.hasPath)
			{
				return false;
			}
			bool flag = Agent.velocity.sqrMagnitude < 0.04f && Agent.desiredVelocity.sqrMagnitude < 0.04f;
			if (Agent.pathStatus == NavMeshPathStatus.PathComplete)
			{
				return Agent.remainingDistance <= arriveDistance && flag;
			}
			return flag;
		}

		public void EnsureHeadingTo(Vector3 position)
		{
			if (Agent.enabled && Agent.isOnNavMesh && !Agent.pathPending && (!Agent.hasPath || Agent.isStopped))
			{
				MoveToPosition(position);
			}
		}

		public float RotateTowards(Vector3 forward, float degreesPerSecond, float deltaTime)
		{
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0025f)
			{
				return 0f;
			}
			Quaternion quaternion = Quaternion.LookRotation(forward.normalized, Vector3.up);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, degreesPerSecond * deltaTime);
			return Quaternion.Angle(base.transform.rotation, quaternion);
		}

		public float HorizontalDistanceTo(Vector3 position)
		{
			Vector3 vector = position - base.transform.position;
			vector.y = 0f;
			return vector.magnitude;
		}

		public bool TryGetNearestPlayerPosition(out Vector3 position, out float distance)
		{
			position = default(Vector3);
			distance = float.PositiveInfinity;
			PlayerRef playerRef = PlayerRef.None;
			Vector3 vector = default(Vector3);
			float num = float.PositiveInfinity;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (!(value?.NetworkObject == null) && _playerStateService.IsPlayerAlive(player.Key.PlayerId))
				{
					Vector3 position2 = value.NetworkObject.transform.position;
					float num2 = Vector3.Distance(base.transform.position, position2);
					if (player.Key == _followedPlayer)
					{
						vector = position2;
						num = num2;
					}
					if (!(num2 >= distance))
					{
						distance = num2;
						position = position2;
						playerRef = player.Key;
					}
				}
			}
			if (playerRef == PlayerRef.None)
			{
				_followedPlayer = PlayerRef.None;
				return false;
			}
			if (num <= distance + _monkeyPorterSettings.FollowSwitchMargin)
			{
				position = vector;
				distance = num;
				return true;
			}
			_followedPlayer = playerRef;
			return true;
		}

		public bool TryGetBoatDropPosition(out Vector3 position)
		{
			position = default(Vector3);
			if (!_quotaContainerLocationModel.HasContainer)
			{
				return false;
			}
			Vector3 desired = (_quotaContainerLocationModel.HasApproach ? _quotaContainerLocationModel.Approach.position : _quotaContainerLocationModel.Drop.position);
			TrySampleAgentPosition(desired, 12f, out position);
			return true;
		}

		public bool IsWithinBoatThrowRange(float range)
		{
			if (!_quotaContainerLocationModel.HasContainer)
			{
				return false;
			}
			Vector3 vector = _quotaContainerLocationModel.Drop.position - base.transform.position;
			vector.y = 0f;
			return vector.sqrMagnitude <= range * range;
		}

		public Vector3 GetBoatWorldPosition()
		{
			if (!_quotaContainerLocationModel.HasContainer)
			{
				return base.transform.position;
			}
			return _quotaContainerLocationModel.Drop.position;
		}

		private void AssertAssigned(Object reference, string fieldName)
		{
			if (reference != null)
			{
				return;
			}
			throw new MissingReferenceException("MonkeyPorterContext on '" + base.name + "' requires '" + fieldName + "' to be assigned.");
		}
	}
}
