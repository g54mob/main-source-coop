using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors
{
	public class SharkWaterTargetSensor
	{
		private readonly SharkEnemyContext _context;

		private readonly SharkTargetingSettings _settings;

		private readonly SharkMovementSettings _movement;

		private readonly IPlayerStateService _playerStateService;

		private readonly SpawnedPlayersModel _spawnedPlayers;

		private readonly PlayerDamageablesTrackModel _playerDamageables;

		private readonly EnemyTransformsModel _enemyTransforms;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private readonly EnemyPovDetector _povDetector;

		private readonly EnemyLineOfSightDetector _lineOfSightDetector;

		private readonly INavigationService _navigationService;

		private readonly CauldronStealthModel _cauldronStealthModel;

		public SharkWaterTargetSensor(SharkEnemyContext context, SharkTargetingSettings settings, SharkMovementSettings movement, IPlayerStateService playerStateService, SpawnedPlayersModel spawnedPlayers, PlayerDamageablesTrackModel playerDamageables, EnemyTransformsModel enemyTransforms, PlayerMovableModel playerMovableModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, EnemyPovDetector povDetector, EnemyLineOfSightDetector lineOfSightDetector, INavigationService navigationService, CauldronStealthModel cauldronStealthModel)
		{
			_cauldronStealthModel = cauldronStealthModel;
			_context = context;
			_settings = settings;
			_movement = movement;
			_playerStateService = playerStateService;
			_spawnedPlayers = spawnedPlayers;
			_playerDamageables = playerDamageables;
			_enemyTransforms = enemyTransforms;
			_playerMovableModel = playerMovableModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_povDetector = povDetector;
			_lineOfSightDetector = lineOfSightDetector;
			_navigationService = navigationService;
		}

		public bool TryAcquireTarget(Vector3 fromPosition, out PlayerRef target, out NetworkObject targetObject)
		{
			target = PlayerRef.None;
			targetObject = null;
			if (_spawnedPlayers == null || _spawnedPlayers.Players == null)
			{
				return false;
			}
			float num = Mathf.Max(0f, _settings.MaxHuntRadius);
			float num2 = ((num > 0f) ? (num * num) : float.MaxValue);
			float num3 = float.MaxValue;
			PlayerRef playerRef = PlayerRef.None;
			NetworkObject networkObject = null;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayers.Players)
			{
				PlayerRef key = player.Key;
				if (!_playerStateService.IsPlayerAlive(key.PlayerId) || !_enemyPlayerAttackabilityService.CanEnemyTargetPlayer(key.PlayerId) || _cauldronStealthModel.IsStealthed(key.PlayerId) || !_context.TryGetPlayerObject(key, out var playerObject) || playerObject == null || !_playerMovableModel.AllCharacterMovables.TryGetValue(key, out var value))
				{
					continue;
				}
				Vector3 position = playerObject.transform.position;
				float num4 = HorizontalSqrDistance(fromPosition, position);
				if (!(num4 > num2) && (!_settings.RequireHuntVolume || !(_context.HuntBounds != null) || _context.HuntBounds.bounds.Contains(position)) && !IsPlayerProtectedFromAttack(position, fromPosition) && _context.IsChaseTargetReachableOnNavMesh(position, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange) && HasLineOfSightToPlayer(key, playerObject, value))
				{
					float num5 = num4;
					float distanceToTarget = Mathf.Sqrt(num5);
					if (_povDetector.IsObjectInPov(value, position, distanceToTarget, out var _) && num5 < num3)
					{
						num3 = num5;
						playerRef = key;
						networkObject = playerObject;
					}
				}
			}
			if (playerRef == PlayerRef.None)
			{
				return false;
			}
			target = playerRef;
			targetObject = networkObject;
			return true;
		}

		public bool HasLineOfSightToPlayer(PlayerRef player, NetworkObject playerObject, PlayerCharacterMovableBase characterMovable)
		{
			if (!_settings.UseLineOfSight)
			{
				return true;
			}
			if (_lineOfSightDetector == null || playerObject == null)
			{
				return true;
			}
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position) && _lineOfSightDetector.HasLineOfSight(position))
			{
				return true;
			}
			if (_context.TryGetPlayerObject(player, out var playerObject2) && playerObject2 == playerObject && _lineOfSightDetector.HasLineOfSight(playerObject.transform.position))
			{
				return true;
			}
			if (characterMovable != null)
			{
				if (_lineOfSightDetector.HasLineOfSight(characterMovable.transform.position))
				{
					return true;
				}
				if (characterMovable.RaycastPoints != null)
				{
					foreach (Transform raycastPoint in characterMovable.RaycastPoints)
					{
						if (raycastPoint != null && _lineOfSightDetector.HasLineOfSight(raycastPoint.position))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool HasLineOfSightToTargetPlayer()
		{
			if (_context.TargetPlayer == PlayerRef.None || _context.TargetObject == null || !_context.TargetObject.IsValid)
			{
				return false;
			}
			_playerMovableModel.AllCharacterMovables.TryGetValue(_context.TargetPlayer, out var value);
			return HasLineOfSightToPlayer(_context.TargetPlayer, _context.TargetObject, value);
		}

		public bool TryAcquireMimicTarget(Vector3 fromPosition, out NetworkObject mimicObject)
		{
			mimicObject = null;
			if (!_settings.HuntMimicWhenNoPlayerInRange || _enemyTransforms?.EnemyNetworkObjects == null)
			{
				return false;
			}
			if (!_enemyTransforms.EnemyNetworkObjects.TryGetValue(EnemyType.Mimic, out var value) || value == null || value.Count == 0)
			{
				return false;
			}
			float num = Mathf.Max(0f, _settings.MaxHuntRadius);
			float num2 = ((num > 0f) ? (num * num) : float.MaxValue);
			float num3 = float.MaxValue;
			NetworkObject networkObject = null;
			for (int i = 0; i < value.Count; i++)
			{
				NetworkObject networkObject2 = value[i];
				if (!(networkObject2 == null) && networkObject2.IsValid)
				{
					Vector3 position = networkObject2.transform.position;
					float num4 = HorizontalSqrDistance(fromPosition, position);
					if (!(num4 > num2) && (!_settings.RequireHuntVolume || !(_context.HuntBounds != null) || _context.HuntBounds.bounds.Contains(position)) && _context.IsChaseTargetReachableOnNavMesh(position, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange) && IsMimicEligibleForAttack(position) && num4 < num3)
					{
						num3 = num4;
						networkObject = networkObject2;
					}
				}
			}
			if (networkObject == null)
			{
				return false;
			}
			mimicObject = networkObject;
			return true;
		}

		public bool IsPlayerOnSafeStand(Vector3 worldPosition)
		{
			if (_settings.SafeStandLayers.value == 0)
			{
				return false;
			}
			if (TryProbeSurfaceUnderPlayer(worldPosition, out var hit))
			{
				return IsLayerInMask(hit.collider.gameObject.layer, _settings.SafeStandLayers);
			}
			return false;
		}

		public bool IsPlayerProtectedFromAttack(Vector3 playerWorldPos, Vector3 sharkWorldPos)
		{
			bool flag = playerWorldPos.y - sharkWorldPos.y >= _settings.MinHighGroundDelta;
			RaycastHit hit;
			bool num = TryProbeSurfaceUnderPlayer(playerWorldPos, out hit);
			bool flag2 = num && IsLayerInMask(hit.collider.gameObject.layer, _settings.SafeStandLayers);
			PlayerSafeZone safeZone;
			bool flag3 = num && TryGetPlayerSafeZoneFromCollider(hit.collider, out safeZone);
			bool flag4 = _context.SafeZoneDetector != null && _context.SafeZoneDetector.TryFindBlockingSafeZoneUnderPlayer(playerWorldPos, out safeZone);
			if (!flag2)
			{
				if (flag)
				{
					return flag3 || flag4;
				}
				return false;
			}
			return true;
		}

		public bool CanResumePlayerHuntAfterBackoff(SharkEnemy enemy, PlayerRef target, NetworkObject targetObject)
		{
			if (target == PlayerRef.None || targetObject == null || !targetObject.IsValid)
			{
				return false;
			}
			if (!_playerStateService.IsPlayerAlive(target.PlayerId))
			{
				return false;
			}
			if (!_context.TryGetPlayerObject(target, out var playerObject) || playerObject != targetObject)
			{
				return false;
			}
			Vector3 position = enemy.transform.position;
			Vector3 position2 = targetObject.transform.position;
			if (position2.y - position.y >= _settings.MinHighGroundDelta)
			{
				return false;
			}
			if (IsPlayerProtectedFromAttack(position2, position))
			{
				return false;
			}
			if (_settings.RequireHuntVolume && _context.HuntBounds != null && !_context.HuntBounds.bounds.Contains(position2))
			{
				return false;
			}
			float num = Mathf.Max(0f, _settings.MaxHuntRadius);
			if (num > 0f && HorizontalSqrDistance(position, position2) > num * num)
			{
				return false;
			}
			return _context.IsChaseTargetReachableOnNavMesh(position2, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange);
		}

		public bool CanResumeMimicHuntAfterBackoff(NetworkObject targetObject)
		{
			if (targetObject != null && targetObject.IsValid)
			{
				return IsMimicEligibleForAttack(targetObject.transform.position);
			}
			return false;
		}

		public bool IsMimicInPlayerSafeZone(Vector3 mimicWorldPos)
		{
			if (_context.SafeZoneDetector != null)
			{
				return _context.SafeZoneDetector.IsPlayerInSafeZone(mimicWorldPos);
			}
			return false;
		}

		public bool IsMimicEligibleForAttack(Vector3 mimicWorldPos)
		{
			if (IsMimicInPlayerSafeZone(mimicWorldPos))
			{
				return false;
			}
			if (IsPlayerOnSafeStand(mimicWorldPos))
			{
				return false;
			}
			return IsMimicNearAnyPlayer(mimicWorldPos, _settings.MimicAttackNearPlayerRadius);
		}

		public bool IsMimicNearAnyPlayer(Vector3 mimicWorldPos, float radius)
		{
			if (radius <= 0f || _spawnedPlayers?.Players == null)
			{
				return false;
			}
			float num = radius * radius;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayers.Players)
			{
				PlayerRef key = player.Key;
				if (_playerStateService.IsPlayerAlive(key.PlayerId) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(key.PlayerId) && _context.TryGetPlayerObject(key, out var playerObject) && !(playerObject == null) && HorizontalSqrDistance(mimicWorldPos, playerObject.transform.position) <= num)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsTargetAirborne(PlayerRef playerRef)
		{
			if (_playerMovableModel?.AllCharacterMovables == null || !_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value) || value == null || !value.Object.IsValid)
			{
				return false;
			}
			if (!value.IsGrounded)
			{
				return true;
			}
			return value.GetVelocity().y > _settings.JumpAirborneVelocityThreshold;
		}

		public bool TryResolveJumpBiteOverSurface(Vector3 worldPosition, out bool useRealBite)
		{
			useRealBite = false;
			if (!TryProbeSurfaceUnderPlayer(worldPosition, out var hit))
			{
				return false;
			}
			useRealBite = _settings.IgnoreJumpProtectionLayers.value != 0 && IsLayerInMask(hit.collider.gameObject.layer, _settings.IgnoreJumpProtectionLayers);
			return true;
		}

		private static bool TryGetPlayerSafeZoneFromCollider(Collider collider, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			if (collider == null)
			{
				return false;
			}
			if (collider.TryGetComponent<PlayerSafeZone>(out safeZone))
			{
				return true;
			}
			safeZone = collider.GetComponentInParent<PlayerSafeZone>();
			return safeZone != null;
		}

		private bool TryProbeSurfaceUnderPlayer(Vector3 worldPosition, out RaycastHit hit)
		{
			return Physics.SphereCast(worldPosition + Vector3.up * 0.25f, 0.35f, Vector3.down, out hit, 2f, -1, QueryTriggerInteraction.Collide);
		}

		private bool IsLayerInMask(int layer, LayerMask mask)
		{
			int value = mask.value;
			if (value != 0)
			{
				return ((1 << layer) & value) != 0;
			}
			return false;
		}

		public bool HasCoverBetween(Vector3 from, Vector3 to)
		{
			if (_settings.CoverLayerMask.value == 0)
			{
				return false;
			}
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude < 0.01f)
			{
				return false;
			}
			return Physics.Raycast(from, vector / magnitude, magnitude, _settings.CoverLayerMask);
		}

		public bool WouldSimpleKill(int playerId, float simpleDamage)
		{
			if (_playerDamageables == null || !_playerDamageables.AllPlayerDamageables.TryGetValue(playerId, out var value))
			{
				return false;
			}
			if (value.Health > 0f)
			{
				return value.Health <= simpleDamage;
			}
			return false;
		}

		private static float HorizontalSqrDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return num * num + num2 * num2;
		}
	}
}
