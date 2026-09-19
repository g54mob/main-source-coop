using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.LevelGatesModule.Data;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateVisibilityTargetDetector : EnemyTargetDetectorBase
	{
		[SerializeField]
		private LayerMask _obstacleLayerMask;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private Transform _eyesSight;

		[SerializeField]
		private float _hardDetectDistance;

		[SerializeField]
		private float _availablePointRange = 12f;

		[SerializeField]
		private SerializableDictionary<MovementState, float> _playerVisibilityAngles;

		private PlayerMovableModel _playerMovableModel;

		private INavigationService _navigationService;

		private IPlayerStateService _playerStateService;

		private PlayerRaycastPointsModel _raycastPointsModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private bool _targetWasDetected;

		public override event Action<PlayerRef> OnTargetDetectedInRange;

		public override event Action OnTargetDetecting;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, INavigationService navigationService, IPlayerStateService playerStateService, PlayerRaycastPointsModel raycastPointsModel, PlayersGatesModelSynchronizedModel playersGatesModel, SessionAnalyticsModel sessionAnalyticsModel)
		{
			_playerMovableModel = playerMovableModel;
			_navigationService = navigationService;
			_playerStateService = playerStateService;
			_raycastPointsModel = raycastPointsModel;
			_playersGatesModel = playersGatesModel;
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		public override float GetDistanceToPlayer(PlayerRef player)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position))
			{
				return Vector3.Distance(position, new Vector3(base.transform.position.x, position.y, base.transform.position.z));
			}
			return base.GetDistanceToPlayer(player);
		}

		protected override void DetectPlayers()
		{
			_targetWasDetected = false;
			base.DetectTargets.Clear();
			base.DetectTargetsType.Clear();
			if (MultiplayerModel.NetworkRunner == null || !MultiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (PlayerRef activePlayer in MultiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (IsPlayerDetected(activePlayer, out var detectionType))
				{
					_sessionAnalyticsModel.RegisterEnemyTarget(activePlayer.PlayerId);
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
			if (!_playersGatesModel.TryGetPlayerState(player.PlayerId, out var state) || !state.PlayerInsideGate)
			{
				return false;
			}
			if (!TryGetCharacterMovable(player, value, out var characterMovable))
			{
				return false;
			}
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position) && IsObjectInPov(characterMovable, player, position, out detectionType))
			{
				return true;
			}
			foreach (Transform raycastPoint in characterMovable.RaycastPoints)
			{
				if (IsObjectInPov(characterMovable, player, raycastPoint.position, out detectionType))
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
			return _playerMovableModel.AllCharacterMovables.TryGetValue(playerData.NetworkObject.InputAuthority, out characterMovable);
		}

		private bool IsObjectInPov(PlayerCharacterMovableBase characterMovable, PlayerRef player, Vector3 playerPosition, out DetectionType detectionType)
		{
			detectionType = DetectionType.None;
			if (!IsNavMeshAgentReadyForPathQueries())
			{
				return false;
			}
			if (GetDistanceToPlayer(player) > _detectionRadius)
			{
				return false;
			}
			if (!TryGetNavigationRaycastPoint(player, out var raycastPoint))
			{
				return false;
			}
			if (!_navigationService.HasAvailablePointInRange(_navMeshAgent, raycastPoint, _availablePointRange, out var _))
			{
				return false;
			}
			Vector3 position = _eyesSight.position;
			Vector3 vector = playerPosition - position;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.001f)
			{
				return false;
			}
			if (Physics.Raycast(position, vector.normalized, out var _, magnitude, _obstacleLayerMask))
			{
				return false;
			}
			if (GetDistanceToPlayer(player) < _hardDetectDistance)
			{
				detectionType = DetectionType.HardDetection;
				return true;
			}
			float num = 0f;
			if (_playerVisibilityAngles != null && _playerVisibilityAngles.TryGetValue(characterMovable.MovementState, out var value))
			{
				num = value;
			}
			if (!(Vector3.Dot(_eyesSight.forward, vector.normalized) >= Mathf.Cos(num * 0.5f * (MathF.PI / 180f))))
			{
				return false;
			}
			detectionType = DetectionType.Default;
			return true;
		}

		private bool IsNavMeshAgentReadyForPathQueries()
		{
			if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled)
			{
				return _navMeshAgent.isOnNavMesh;
			}
			return false;
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

		private void OnDrawGizmosSelected()
		{
			if (_eyesSight == null)
			{
				return;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_eyesSight.position, _eyesSight.position + _eyesSight.forward * 2f);
			Gizmos.DrawWireSphere(base.transform.position, _hardDetectDistance);
			if (_playerVisibilityAngles == null)
			{
				return;
			}
			foreach (KeyValuePair<MovementState, float> playerVisibilityAngle in _playerVisibilityAngles)
			{
				DrawFov(playerVisibilityAngle.Value, GetColorFromState(playerVisibilityAngle.Key));
			}
		}

		private void DrawFov(float visibilityAngle, Color color)
		{
			Vector3 position = _eyesSight.position;
			float num = visibilityAngle * 0.5f;
			int num2 = Mathf.Max(1, (int)num / 3);
			Vector3 vector = position;
			for (int i = 0; i <= num2; i++)
			{
				float y = 0f - num + visibilityAngle / (float)num2 * (float)i;
				Vector3 vector2 = Quaternion.Euler(0f, y, 0f) * _eyesSight.forward;
				Vector3 vector3 = position + vector2 * _detectionRadius;
				Gizmos.color = color;
				Gizmos.DrawLine(position, vector3);
				if (i > 0)
				{
					Gizmos.DrawLine(vector, vector3);
				}
				vector = vector3;
			}
		}

		private Color GetColorFromState(MovementState state)
		{
			return Color.HSVToRGB((float)state * 0.15f % 1f, 0.8f, 1f);
		}
	}
}
