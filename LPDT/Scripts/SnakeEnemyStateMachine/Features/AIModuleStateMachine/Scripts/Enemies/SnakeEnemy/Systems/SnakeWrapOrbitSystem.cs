using System;
using System.Collections.Generic;
using Features.CustomSynchronizersModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeWrapOrbitSystem : NetworkBehaviour
	{
		private const float ARRIVE_EPSILON = 0.001f;

		private const float MIN_APPROACH_DURATION = 0.2f;

		private const float MIN_RETURN_DURATION = 0.75f;

		private const float ORBIT_CENTER_SYNC_SQR = 0.0001f;

		private const float ANGLE_CORRECT_SPEED = 6f;

		private const float SPAWN_WRAP_GRACE_SECONDS = 2f;

		[SerializeField]
		private Transform _visual;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[Tooltip("If approach start is farther than this from orbit (sky desync), snap instead of lerp. Keep above pit depths.")]
		[SerializeField]
		private float _approachSnapDistance = 40f;

		[Tooltip("Return-home speed at short distances (world units/sec).")]
		[SerializeField]
		private float _returnHomeSpeedNear = 4f;

		[Tooltip("Return-home speed at long distances (pits). Still below wrap approach so exits are not whip-fast.")]
		[SerializeField]
		private float _returnHomeSpeedFar = 12f;

		[Tooltip("Distance at which return uses Near speed.")]
		[SerializeField]
		private float _returnHomeDistanceNear = 2f;

		[Tooltip("Distance at which return uses Far speed.")]
		[SerializeField]
		private float _returnHomeDistanceFar = 12f;

		[Tooltip("Overrides SnakeController Visual Rotation Speed while wrap drives Visual.")]
		[SerializeField]
		private float _visualRotationSpeed = 20f;

		private SnakeEnemyContext _context;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private bool _isDriven;

		private bool _hasReachedOrbitStart;

		private bool _isReturningHome;

		private bool _hasOrbitCenter;

		private float _localOrbitAngleRadians;

		private float _coilElapsed;

		private Vector3 _orbitCenter;

		private Vector3 _orbitStartWorld;

		private Vector3 _approachStartWorld;

		private float _approachDuration;

		private float _approachElapsed;

		private Vector3 _returnStartWorld;

		private float _returnDuration;

		private float _returnElapsed;

		private Vector3 _restLocalPosition;

		private bool _hasCachedRestLocalPosition;

		private float _forceMaxOrbitSpeedRemaining;

		private float _spawnedAtUnscaledTime;

		private bool _pendingSpawnWrapBoost;

		private bool _spawnedIntoActiveWrap;

		public bool IsVisualBusy
		{
			get
			{
				if (!_isDriven && !_isReturningHome)
				{
					if (_context != null)
					{
						return _context.IsWrapOrbitActive;
					}
					return false;
				}
				return true;
			}
		}

		[Inject]
		public void InjectDependencies(SnakeEnemyContext context, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Spawned()
		{
			CacheRestLocalPosition();
			_spawnedAtUnscaledTime = Time.unscaledTime;
			_pendingSpawnWrapBoost = !base.HasStateAuthority;
			_spawnedIntoActiveWrap = false;
			if (_physicsSynchronizer == null)
			{
				_physicsSynchronizer = GetComponentInParent<PhysicsSynchronizer>();
			}
			TryStartSpawnWrapMaxSpeedBoost();
		}

		public void Begin(float orbitAngleRadians, float orbitHeight, int targetPlayerId)
		{
			CacheRestLocalPosition();
			_isDriven = true;
			_hasReachedOrbitStart = false;
			_isReturningHome = false;
			_spawnedIntoActiveWrap = false;
			_localOrbitAngleRadians = orbitAngleRadians;
			_coilElapsed = 0f;
			_forceMaxOrbitSpeedRemaining = 0f;
			_pendingSpawnWrapBoost = false;
			ApplyVisualRotationSpeedOverride();
			RefreshOrbitCenter(targetPlayerId, forceNetworkSync: true);
			BeginApproachFromCurrentVisual();
			if (base.HasStateAuthority)
			{
				_context.SetWrapOrbitState(isActive: true, orbitAngleRadians, orbitHeight, targetPlayerId, _orbitCenter);
			}
		}

		public void End()
		{
			BeginReturnHome();
			if (base.HasStateAuthority)
			{
				_context.SetWrapOrbitState(isActive: false, 0f, 0f, 0, Vector3.zero);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ClearVisualRotationSpeedOverride();
			SnapVisualToRestLocalPosition();
			ResetDriveState();
		}

		private void LateUpdate()
		{
			if (_context == null || !base.Object || !base.Object.IsValid || _visual == null)
			{
				return;
			}
			if ((bool)_context.IsWrapOrbitActive)
			{
				EnsureWrapDriveLatched();
				RefreshOrbitCenter(_context.WrapTargetPlayerId, forceNetworkSync: false);
				if (!_hasReachedOrbitStart)
				{
					TickApproachOrbitStart();
				}
				else
				{
					TickOrbit();
				}
				return;
			}
			if (_isDriven)
			{
				BeginReturnHome();
			}
			if (_isReturningHome)
			{
				TickReturnHome();
			}
		}

		private void TickOrbit()
		{
			_coilElapsed += Time.deltaTime;
			float num = EvaluateOrbitAngularSpeed();
			_localOrbitAngleRadians += num * (MathF.PI / 180f) * Time.deltaTime;
			if (base.HasStateAuthority)
			{
				_context.SetWrapOrbitAngle(_localOrbitAngleRadians);
			}
			else if (_forceMaxOrbitSpeedRemaining <= 0f)
			{
				SoftCorrectAngleTowardNetwork();
			}
			ApplyOrbitPosition(_localOrbitAngleRadians);
			FaceVisualAlongOrbit(_localOrbitAngleRadians);
		}

		private float EvaluateOrbitAngularSpeed()
		{
			if (_forceMaxOrbitSpeedRemaining > 0f)
			{
				_forceMaxOrbitSpeedRemaining -= Time.deltaTime;
				return _context.StepAggroWrapAngularSpeed;
			}
			return _context.EvaluateWrapOrbitAngularSpeed(_coilElapsed);
		}

		private void SoftCorrectAngleTowardNetwork()
		{
			float wrapOrbitAngleRadians = _context.WrapOrbitAngleRadians;
			float num = Mathf.DeltaAngle(_localOrbitAngleRadians * 57.29578f, wrapOrbitAngleRadians * 57.29578f);
			float num2 = 1f - Mathf.Exp(-6f * Time.deltaTime);
			_localOrbitAngleRadians += num * (MathF.PI / 180f) * num2;
		}

		private void EnsureWrapDriveLatched()
		{
			TryStartSpawnWrapMaxSpeedBoost();
			if (!_isDriven && (base.HasStateAuthority || IsRootPoseReadyForVisual()))
			{
				if (!base.HasStateAuthority)
				{
					_physicsSynchronizer?.SnapProxyToNetworkedPoseImmediate();
				}
				_isDriven = true;
				_isReturningHome = false;
				_localOrbitAngleRadians = _context.WrapOrbitAngleRadians;
				_coilElapsed = 0f;
				ApplyVisualRotationSpeedOverride();
				if (_spawnedIntoActiveWrap)
				{
					RefreshOrbitCenterForLateJoinLatch();
					ApplyOrbitPosition(_localOrbitAngleRadians);
					FaceVisualAlongOrbit(_localOrbitAngleRadians);
					_hasReachedOrbitStart = true;
					ReseedBodyTrail();
					_spawnedIntoActiveWrap = false;
				}
				else
				{
					_hasReachedOrbitStart = false;
					RefreshOrbitCenter(_context.WrapTargetPlayerId, forceNetworkSync: false);
					BeginApproachFromCurrentVisual();
				}
			}
		}

		private bool IsRootPoseReadyForVisual()
		{
			if (_physicsSynchronizer == null)
			{
				return true;
			}
			return _physicsSynchronizer.HasValidSyncedPose;
		}

		private void TryStartSpawnWrapMaxSpeedBoost()
		{
			if (_pendingSpawnWrapBoost && !base.HasStateAuthority && !(_context == null))
			{
				if (Time.unscaledTime - _spawnedAtUnscaledTime > 2f)
				{
					_pendingSpawnWrapBoost = false;
				}
				else if ((bool)_context.IsWrapOrbitActive)
				{
					_pendingSpawnWrapBoost = false;
					_spawnedIntoActiveWrap = true;
					float stepAggroWrapCoilDuration = _context.StepAggroWrapCoilDuration;
					_forceMaxOrbitSpeedRemaining = ((stepAggroWrapCoilDuration > 0f) ? stepAggroWrapCoilDuration : 1f);
				}
			}
		}

		private void RefreshOrbitCenterForLateJoinLatch()
		{
			CacheRestLocalPosition();
			if ((bool)_context.IsWrapOrbitActive)
			{
				_orbitCenter = _context.WrapOrbitCenter;
				_hasOrbitCenter = true;
			}
			RefreshOrbitCenter(_context.WrapTargetPlayerId, forceNetworkSync: false);
		}

		private void BeginApproachFromCurrentVisual()
		{
			if (_visual == null)
			{
				return;
			}
			_approachStartWorld = _visual.position;
			_approachElapsed = 0f;
			_hasReachedOrbitStart = false;
			if (!TryGetOrbitWorldPosition(_localOrbitAngleRadians, out _orbitStartWorld))
			{
				_approachDuration = 0.2f;
				return;
			}
			float num = Vector3.Distance(_approachStartWorld, _orbitStartWorld);
			if (num > _approachSnapDistance)
			{
				SnapVisualToOrbitAndReseed();
				return;
			}
			float num2 = Mathf.Max(0.01f, _context.StepAggroWrapMoveSpeed);
			_approachDuration = Mathf.Max(0.2f, num / num2);
		}

		private void SnapVisualToOrbitAndReseed()
		{
			_visual.position = _orbitStartWorld;
			_hasReachedOrbitStart = true;
			ReseedBodyTrail();
		}

		private void ReseedBodyTrail()
		{
			((_context != null) ? _context.SnakeController : null)?.ReseedBodyTrailFromHead();
		}

		private void TickApproachOrbitStart()
		{
			if (!TryGetOrbitWorldPosition(_localOrbitAngleRadians, out _orbitStartWorld))
			{
				return;
			}
			if (Vector3.Distance(_visual.position, _orbitStartWorld) > _approachSnapDistance)
			{
				SnapVisualToOrbitAndReseed();
				return;
			}
			_approachElapsed += Time.deltaTime;
			float num = Mathf.Clamp01(_approachElapsed / Mathf.Max(0.001f, _approachDuration));
			float t = Mathf.SmoothStep(0f, 1f, num);
			_visual.position = Vector3.Lerp(_approachStartWorld, _orbitStartWorld, t);
			FaceVisualTowardWorldPoint(_orbitStartWorld);
			if (!(num < 1f))
			{
				_visual.position = _orbitStartWorld;
				_hasReachedOrbitStart = true;
			}
		}

		private void BeginReturnHome()
		{
			if (_visual == null)
			{
				ClearVisualRotationSpeedOverride();
				ResetDriveState();
			}
			else if (!_isReturningHome)
			{
				CacheRestLocalPosition();
				_isDriven = false;
				_hasReachedOrbitStart = false;
				_hasOrbitCenter = false;
				_forceMaxOrbitSpeedRemaining = 0f;
				_pendingSpawnWrapBoost = false;
				_spawnedIntoActiveWrap = false;
				_isReturningHome = true;
				ApplyVisualRotationSpeedOverride();
				_returnStartWorld = _visual.position;
				_returnElapsed = 0f;
				Vector3 restWorldPosition = GetRestWorldPosition();
				float num = Vector3.Distance(_returnStartWorld, restWorldPosition);
				float num2 = EvaluateDistanceScaledSpeed(num, _returnHomeSpeedNear, _returnHomeSpeedFar, _returnHomeDistanceNear, _returnHomeDistanceFar);
				_returnDuration = Mathf.Max(0.75f, num / num2);
			}
		}

		private static float EvaluateDistanceScaledSpeed(float distance, float speedNear, float speedFar, float distanceNear, float distanceFar)
		{
			float t = Mathf.InverseLerp(distanceNear, Mathf.Max(distanceNear + 0.01f, distanceFar), distance);
			return Mathf.Max(0.01f, Mathf.Lerp(speedNear, speedFar, t));
		}

		private void TickReturnHome()
		{
			Vector3 restWorldPosition = GetRestWorldPosition();
			_returnElapsed += Time.deltaTime;
			float num = Mathf.Clamp01(_returnElapsed / Mathf.Max(0.001f, _returnDuration));
			float t = Mathf.SmoothStep(0f, 1f, num);
			_visual.position = Vector3.Lerp(_returnStartWorld, restWorldPosition, t);
			FaceVisualTowardWorldPoint(restWorldPosition);
			if (!(num < 1f))
			{
				SnapVisualToRestLocalPosition();
				ClearVisualRotationSpeedOverride();
				_isReturningHome = false;
			}
		}

		private Vector3 GetRestWorldPosition()
		{
			if (_visual != null && _visual.parent != null)
			{
				return _visual.parent.TransformPoint(_restLocalPosition);
			}
			return _restLocalPosition;
		}

		private void ApplyOrbitPosition(float orbitAngleRadians)
		{
			if (TryGetOrbitWorldPosition(orbitAngleRadians, out var orbitPosition))
			{
				_visual.position = orbitPosition;
			}
		}

		private bool TryGetOrbitWorldPosition(float orbitAngleRadians, out Vector3 orbitPosition)
		{
			orbitPosition = default(Vector3);
			if (!_hasOrbitCenter)
			{
				return false;
			}
			Vector3 vector = new Vector3(Mathf.Cos(orbitAngleRadians), 0f, Mathf.Sin(orbitAngleRadians)) * _context.StepAggroWrapOrbitRadius;
			orbitPosition = _orbitCenter + vector;
			orbitPosition.y = _orbitCenter.y - _context.WrapOrbitPlayerPivotHeight;
			return true;
		}

		private void RefreshOrbitCenter(int targetPlayerId, bool forceNetworkSync)
		{
			CacheRestLocalPosition();
			if (TryGetPlayerPosition(targetPlayerId, out var position))
			{
				_orbitCenter = position;
				_hasOrbitCenter = true;
			}
			else if ((bool)_context.IsWrapOrbitActive)
			{
				_orbitCenter = _context.WrapOrbitCenter;
				_hasOrbitCenter = true;
			}
			else if (!_hasOrbitCenter)
			{
				return;
			}
			if (base.HasStateAuthority && (forceNetworkSync || !((_context.WrapOrbitCenter - _orbitCenter).sqrMagnitude < 0.0001f)))
			{
				_context.SetWrapOrbitCenter(_orbitCenter);
			}
		}

		private bool TryGetPlayerPosition(int playerId, out Vector3 position)
		{
			position = default(Vector3);
			if (playerId <= 0 || _spawnedPlayersModel == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					PlayerDataHolder value = player.Value;
					if (value == null || value.NetworkObject == null)
					{
						return false;
					}
					position = value.NetworkObject.transform.position;
					return true;
				}
			}
			return false;
		}

		private void CacheRestLocalPosition()
		{
			if (!_hasCachedRestLocalPosition && !(_visual == null))
			{
				_restLocalPosition = _visual.localPosition;
				_hasCachedRestLocalPosition = true;
			}
		}

		private void SnapVisualToRestLocalPosition()
		{
			if (!(_visual == null))
			{
				CacheRestLocalPosition();
				_visual.localPosition = _restLocalPosition;
			}
		}

		private void ResetDriveState()
		{
			ClearVisualRotationSpeedOverride();
			_isDriven = false;
			_hasReachedOrbitStart = false;
			_isReturningHome = false;
			_hasOrbitCenter = false;
			_coilElapsed = 0f;
			_forceMaxOrbitSpeedRemaining = 0f;
			_pendingSpawnWrapBoost = false;
			_spawnedIntoActiveWrap = false;
		}

		private void ApplyVisualRotationSpeedOverride()
		{
			((_context != null) ? _context.SnakeController : null)?.SetVisualRotationSpeedOverride(_visualRotationSpeed, suppressVelocityYaw: true);
		}

		private void ClearVisualRotationSpeedOverride()
		{
			((_context != null) ? _context.SnakeController : null)?.ClearVisualRotationSpeedOverride();
		}

		private void FaceVisualAlongOrbit(float orbitAngleRadians)
		{
			Vector3 worldDirection = new Vector3(0f - Mathf.Sin(orbitAngleRadians), 0f, Mathf.Cos(orbitAngleRadians));
			_context.SnakeController?.FaceVisualToward(worldDirection);
		}

		private void FaceVisualTowardWorldPoint(Vector3 worldPoint)
		{
			if (!(_visual == null))
			{
				_context.SnakeController?.FaceVisualToward(worldPoint - _visual.position);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
