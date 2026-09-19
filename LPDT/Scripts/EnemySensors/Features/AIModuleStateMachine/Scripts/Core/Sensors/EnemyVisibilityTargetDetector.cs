using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public class EnemyVisibilityTargetDetector : EnemyTargetDetectorBase
	{
		private const float AVAILABLE_POINT_RANGE_MULTIPLIER = 0.75f;

		private readonly RaycastHit[] _blockingSafeZoneRaycastHits = new RaycastHit[32];

		[SerializeField]
		private LayerMask _obstacleLayerMask;

		[SerializeField]
		private LayerMask _playerSafeZoneMask;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private Transform _eyesSight;

		[SerializeField]
		private EnemyPovDetector _povDetector;

		private PlayerMovableModel _playerMovableModel;

		private INavigationService _navigationService;

		private IPlayerStateService _playerStateService;

		private PlayerRaycastPointsModel _raycastPointsModel;

		private bool _targetWasDetected;

		private float _availablePointRange;

		public override event Action<PlayerRef> OnTargetDetectedInRange;

		public override event Action OnTargetDetecting;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, INavigationService navigationService, IPlayerStateService playerStateService, PlayerRaycastPointsModel raycastPointsModel)
		{
			_playerMovableModel = playerMovableModel;
			_navigationService = navigationService;
			_playerStateService = playerStateService;
			_raycastPointsModel = raycastPointsModel;
		}

		public override float GetDistanceToPlayer(PlayerRef player)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position))
			{
				return Vector3.Distance(position, new Vector3(base.transform.position.x, position.y, base.transform.position.z));
			}
			return base.GetDistanceToPlayer(player);
		}

		public void Init(float availablePointRange)
		{
			_availablePointRange = availablePointRange;
		}

		protected override void DetectPlayers()
		{
			if (!MultiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			_targetWasDetected = false;
			base.DetectTargets.Clear();
			foreach (PlayerRef activePlayer in MultiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (IsPlayerDetected(activePlayer, out var detectionType))
				{
					base.DetectTargets.Add(activePlayer);
					base.DetectTargetsType[activePlayer] = detectionType;
					if (!_targetWasDetected || !_soloDetecting)
					{
						OnTargetDetectedInRange?.Invoke(activePlayer);
					}
					_targetWasDetected = true;
				}
			}
			OnTargetDetecting?.Invoke();
		}

		public override bool IsPlayerDetected(PlayerRef player, out DetectionType detectionType)
		{
			detectionType = DetectionType.None;
			if (!CanSelectPlayerAsTarget(player))
			{
				return false;
			}
			if (!SpawnedPlayersModel.Players.TryGetValue(player, out var value))
			{
				return false;
			}
			if (!_playerStateService.IsPlayerAlive(player.PlayerId))
			{
				return false;
			}
			if (!TryGetCharacterMovable(player, value, out var characterMovable))
			{
				return false;
			}
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position) && IsObjectDetected(characterMovable, player, position, out detectionType))
			{
				return true;
			}
			foreach (Transform raycastPoint in characterMovable.RaycastPoints)
			{
				if (IsObjectDetected(characterMovable, player, raycastPoint.position, out detectionType))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsObstacleOnPath(PlayerRef targetPlayer)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(targetPlayer, out var position))
			{
				return IsObstacleBetween(position);
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(targetPlayer, out var value))
			{
				return false;
			}
			foreach (Transform raycastPoint in value.RaycastPoints)
			{
				if (IsObstacleBetween(raycastPoint.position))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetCharacterMovable(PlayerRef player, PlayerDataHolder playerData, out PlayerCharacterMovableBase characterMovable)
		{
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(player, out characterMovable))
			{
				return true;
			}
			PlayerRef inputAuthority = playerData.NetworkObject.InputAuthority;
			if (inputAuthority != PlayerRef.None && _playerMovableModel.AllCharacterMovables.TryGetValue(inputAuthority, out characterMovable))
			{
				return true;
			}
			characterMovable = null;
			return false;
		}

		private bool IsObjectDetected(PlayerCharacterMovableBase characterMovable, PlayerRef player, Vector3 playerPosition, out DetectionType detectionType)
		{
			detectionType = DetectionType.None;
			float distanceToPlayer = GetDistanceToPlayer(player);
			if (distanceToPlayer > _detectionRadius)
			{
				return false;
			}
			if (!TryGetNavigationRaycastPoint(player, out var raycastPoint))
			{
				return false;
			}
			float range = _availablePointRange * 0.75f;
			if (!_navigationService.HasAvailablePointInRange(_navMeshAgent, raycastPoint, range, out var availablePosition))
			{
				return false;
			}
			if (TryFindBlockingSafeZone(availablePosition, raycastPoint))
			{
				return false;
			}
			if (IsObstacleBetween(playerPosition))
			{
				return false;
			}
			return _povDetector.IsObjectInPov(characterMovable, playerPosition, distanceToPlayer, out detectionType);
		}

		private bool TryGetNavigationRaycastPoint(PlayerRef player, out Vector3 raycastPoint)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(player, out raycastPoint))
			{
				return true;
			}
			if (_raycastPointsModel.TryGetRaycastPoint(player, PlayerRaycastPoint.MiddleBelt, out var point))
			{
				raycastPoint = point.position;
				return true;
			}
			raycastPoint = Vector3.zero;
			return false;
		}

		private bool IsObstacleBetween(Vector3 targetPosition)
		{
			Vector3 position = _eyesSight.position;
			Vector3 vector = targetPosition - position;
			float magnitude = vector.magnitude;
			RaycastHit hitInfo;
			if (magnitude > 0.001f)
			{
				return Physics.Raycast(position, vector.normalized, out hitInfo, magnitude, _obstacleLayerMask);
			}
			return false;
		}

		private bool TryFindBlockingSafeZone(Vector3 enemyPosition, Vector3 playerPosition)
		{
			float num = Vector3.Distance(playerPosition, new Vector3(enemyPosition.x, playerPosition.y, enemyPosition.z));
			Vector3 vector = playerPosition - enemyPosition;
			Vector3 origin = enemyPosition + Vector3.up * 0.5f;
			Vector3 normalized = vector.normalized;
			int num2 = Physics.RaycastNonAlloc(new Ray(origin, normalized), _blockingSafeZoneRaycastHits, 5f, _playerSafeZoneMask);
			Array.Sort(_blockingSafeZoneRaycastHits, 0, num2, Comparer<RaycastHit>.Create((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance)));
			for (int num3 = 0; num3 < num2; num3++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[num3];
				if (Vector3.Distance(raycastHit.point, new Vector3(enemyPosition.x, raycastHit.point.y, enemyPosition.z)) <= num * 1.1f && raycastHit.collider != null && raycastHit.collider.TryGetComponent<PlayerSafeZone>(out var _))
				{
					return true;
				}
			}
			return TryFindBlockingSafeZoneUnderPlayer(playerPosition);
		}

		private bool TryFindBlockingSafeZoneUnderPlayer(Vector3 playerPosition)
		{
			int num = Physics.RaycastNonAlloc(playerPosition, Vector3.up, _blockingSafeZoneRaycastHits, 2f, _playerSafeZoneMask);
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[i];
				if (raycastHit.collider != null && raycastHit.collider.TryGetComponent<PlayerSafeZone>(out var _))
				{
					return true;
				}
			}
			return false;
		}

		private void OnDrawGizmos()
		{
			if (!(_eyesSight == null))
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(_eyesSight.position, _eyesSight.position + _eyesSight.forward * _detectionRadius);
			}
		}
	}
}
