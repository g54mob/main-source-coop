using System.Collections;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.DamageableTrackModule.Scripts;
using Features.HeadwearModule.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.States
{
	public class HeadcrabAttachedState : StateBase<HeadcrabStateId>
	{
		private readonly HeadcrabEnemy _enemy;

		private readonly HeadcrabEnemyContext _context;

		private readonly HeadcrabEnemySettings _enemySettings;

		private readonly HeadcrabAttachSettings _attachSettings;

		private readonly HeadcrabResnapSensor _resnapSensor;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		private readonly IHeadCrabVignetteService _vignetteService;

		private readonly PlayerDamageablesTrackModel _playerDamageables;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private readonly HeadwearModel _headwearModel;

		private Coroutine _attachRoutine;

		private bool _isResnapping;

		private float _cauldronedElapsed;

		private bool _wasTargetWearingCauldron;

		private bool _isVignetteStarted;

		public HeadcrabAttachedState(HeadcrabEnemy enemy, HeadcrabEnemyContext context, HeadcrabEnemySettings enemySettings, HeadcrabAttachSettings attachSettings, HeadcrabResnapSensor resnapSensor, BusyByHeadCrabPlayers busyPlayers, IHeadCrabVignetteService vignetteService, PlayerDamageablesTrackModel playerDamageables, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, HeadwearModel headwearModel)
			: base(false, false)
		{
			_headwearModel = headwearModel;
			_enemy = enemy;
			_context = context;
			_enemySettings = enemySettings;
			_attachSettings = attachSettings;
			_resnapSensor = resnapSensor;
			_busyPlayers = busyPlayers;
			_vignetteService = vignetteService;
			_playerDamageables = playerDamageables;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public override void OnEnter()
		{
			_isResnapping = false;
			_enemy.SetVisualState(HeadcrabVisualState.Jumping);
			_context.SnapElapsed = 0f;
			_context.ResnapCooldownElapsed = 0f;
			_cauldronedElapsed = 0f;
			_wasTargetWearingCauldron = false;
			_isVignetteStarted = false;
			if (_attachRoutine != null)
			{
				_enemy.StopCoroutine(_attachRoutine);
				_attachRoutine = null;
			}
			_attachRoutine = _enemy.StartCoroutine(AttachRoutine());
		}

		public override void OnExit()
		{
			if (_attachRoutine != null)
			{
				_enemy.StopCoroutine(_attachRoutine);
				_attachRoutine = null;
			}
			if (!_isResnapping)
			{
				ReleaseTargetInteractionState();
				_enemy.ReturnToOrigin();
			}
		}

		private void ReleaseTargetInteractionState()
		{
			if (!(_context.TargetPlayer == PlayerRef.None))
			{
				int playerId = _context.TargetPlayer.PlayerId;
				_enemy.RaiseLoopedSittingSound(playerId, play: false);
				if (_context.HomeInstance != null)
				{
					_context.HomeInstance.SetOutlineActiveForPlayer(isActive: false, playerId);
				}
				_vignetteService?.DisableVignetteForPlayer(playerId);
				if (_busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
			}
		}

		public override void OnLogic()
		{
			if (!_context.IsAttached || _context.TargetPlayer == PlayerRef.None)
			{
				return;
			}
			if (!_context.CanEnemyInteractWithTargetPlayer())
			{
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
				return;
			}
			if (!_context.TryGetPlayerObject(_context.TargetPlayer, out var _))
			{
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
				return;
			}
			float tickDelta = _enemy.GetTickDelta();
			_context.ResnapCooldownElapsed += tickDelta;
			bool flag = IsTargetWearingCauldron();
			UpdateCauldronedTimers(tickDelta, flag);
			PlayerRef candidate;
			if (_context.HomeInstance != null && Vector3.Distance(_enemy.transform.position, _context.HomeInstance.transform.position) <= _attachSettings.DistanceToGoHome)
			{
				GoHomeAndDespawn();
			}
			else if (_context.ResnapCooldownElapsed >= _attachSettings.ResnappingCooldown && _resnapSensor.TryGetResnapCandidate(_context.TargetPlayer, out candidate))
			{
				TryResnapTo(candidate);
			}
			else if (flag)
			{
				if (_cauldronedElapsed >= _attachSettings.CauldronedSittingTime)
				{
					GoHomeAndDespawn();
				}
			}
			else if (_context.SnapElapsed >= _attachSettings.SnappingTime)
			{
				KillTargetAndDespawn();
			}
		}

		private void UpdateCauldronedTimers(float dt, bool isTargetWearingCauldron)
		{
			if (isTargetWearingCauldron)
			{
				if (!_wasTargetWearingCauldron)
				{
					SetVignettePaused(isPaused: true);
				}
				_cauldronedElapsed += dt;
			}
			else
			{
				if (_wasTargetWearingCauldron)
				{
					_cauldronedElapsed = 0f;
					SetVignettePaused(isPaused: false);
					if (!_isVignetteStarted)
					{
						StartVignette();
					}
				}
				_context.SnapElapsed += dt;
			}
			_wasTargetWearingCauldron = isTargetWearingCauldron;
		}

		private bool IsTargetWearingCauldron()
		{
			if (_context.TargetPlayer != PlayerRef.None && _headwearModel.TryGetHeadwear(_context.TargetPlayer.PlayerId, out var headwear) && headwear != null)
			{
				return headwear.IsWorn;
			}
			return false;
		}

		private void SetVignettePaused(bool isPaused)
		{
			_vignetteService?.SetVignettePausedForPlayer(_context.TargetPlayer.PlayerId, isPaused);
		}

		private void StartVignette()
		{
			_isVignetteStarted = true;
			_vignetteService?.StartVignetteShowCoroutineForPlayer(_context.TargetPlayer.PlayerId, _attachSettings.SnappingTime, _context.SnapElapsed);
		}

		private IEnumerator AttachRoutine()
		{
			_context.IsAttached = false;
			_enemy.SetAttachedTarget(null);
			_enemy.RaiseDeattachSound();
			_enemy.RaiseJumpStart();
			yield return new WaitForSeconds(_attachSettings.TimeBeforeDeattaching);
			_context.CurrentParentWithPosition = null;
			if (_context.TargetObject == null && _context.TryGetPlayerObject(_context.TargetPlayer, out var playerObject))
			{
				_context.TargetObject = playerObject;
			}
			if (_context.TargetObject == null)
			{
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
				yield break;
			}
			Transform targetTransform = _context.TargetObject.transform;
			Vector3 targetPosition = targetTransform.position;
			float elapsed = 0f;
			Vector3 startPosition = _enemy.transform.position;
			bool jumpEndTriggered = false;
			while (elapsed < _attachSettings.TimeToMoveToPosition + _attachSettings.TimeBeforeAttaching)
			{
				elapsed += Time.deltaTime;
				if (!jumpEndTriggered && elapsed >= _attachSettings.TimeToMoveToPosition)
				{
					jumpEndTriggered = true;
					_enemy.RaiseJumpEnd();
				}
				float t = Mathf.Clamp01(elapsed / (_attachSettings.TimeToMoveToPosition + _attachSettings.TimeBeforeAttaching));
				_enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
				yield return null;
			}
			_enemy.RaiseAttachToPlayerSound();
			AttachToTargetImmediate(targetTransform);
			_enemy.SetAttachedTarget(_context.TargetObject);
			_enemy.SetVisualState(HeadcrabVisualState.Attached);
			if (_context.HomeInstance != null)
			{
				_context.HomeInstance.SetOutlineActiveForPlayer(isActive: true, _context.TargetPlayer.PlayerId);
			}
			_wasTargetWearingCauldron = IsTargetWearingCauldron();
			if (!_wasTargetWearingCauldron)
			{
				StartVignette();
			}
			if (_busyPlayers != null && !_busyPlayers.BusyPlayers.Contains(_context.TargetPlayer))
			{
				_busyPlayers.BusyPlayers.Add(_context.TargetPlayer);
			}
			_enemy.RaiseLoopedSittingSound(_context.TargetPlayer.PlayerId, play: true);
			_context.IsAttached = true;
		}

		private void AttachToTargetImmediate(Transform objectTransform)
		{
			if (objectTransform != null && objectTransform.TryGetComponent<HeadCrabTarget>(out var component))
			{
				_context.CurrentParentWithPosition = new ParentWithPosition(component.TargetTransform, Vector3.zero, Quaternion.identity);
			}
			else if (objectTransform != null)
			{
				_context.CurrentParentWithPosition = new ParentWithPosition(objectTransform, Vector3.zero, Quaternion.identity);
			}
			else
			{
				_context.CurrentParentWithPosition = null;
			}
		}

		private void TryResnapTo(PlayerRef newTarget)
		{
			int playerId = _context.TargetPlayer.PlayerId;
			if (_context.HomeInstance != null)
			{
				_context.HomeInstance.SetOutlineActiveForPlayer(isActive: false, playerId);
			}
			_vignetteService?.DisableVignetteForPlayer(playerId);
			if (_busyPlayers != null)
			{
				_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
			}
			_context.TargetPlayer = newTarget;
			if (!_context.TryGetPlayerObject(newTarget, out var playerObject))
			{
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
				return;
			}
			_context.TargetObject = playerObject;
			_context.ResnapCooldownElapsed = 0f;
			_isResnapping = true;
			_enemy.TriggerEvent(HeadcrabEvent.OnResnap);
		}

		private void KillTargetAndDespawn()
		{
			_enemy.RaiseLoopedSittingSound(_context.TargetPlayer.PlayerId, play: false);
			_enemy.RaiseKillPlayerSound();
			ApplyDamageToTarget();
			_enemy.RequestDespawnNoItemDrop();
		}

		private void GoHomeAndDespawn()
		{
			_enemy.RaiseLoopedSittingSound(_context.TargetPlayer.PlayerId, play: false);
			_enemy.RaiseGoHomeSound();
			_enemy.RequestDespawnNoItemDrop();
		}

		private void ApplyDamageToTarget()
		{
			if (!(_context.TargetPlayer == PlayerRef.None))
			{
				int playerId = _context.TargetPlayer.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(playerId) && _playerDamageables != null && _playerDamageables.AllPlayerDamageables.TryGetValue(playerId, out var value))
				{
					Vector3 direction = value.Transform.position - _enemy.transform.position;
					value.Damage(new DamageData
					{
						Damage = ((_enemySettings != null) ? _enemySettings.Damage : 100f),
						Position = _enemy.transform.position,
						Direction = direction,
						DamageDealerPlayerID = ((_enemy.Object != null) ? _enemy.Object.StateAuthority.PlayerId : 0),
						Force = ((_enemySettings != null) ? _enemySettings.DamageImpulseStrength : 5f),
						ForceMode = ForceMode.Impulse,
						Source = DamageDataSourceExtensions.ForEnemyAttack(_enemy.transform, EnemyType.HeadCrab.ToString(), DamageType.Melee)
					});
				}
			}
		}
	}
}
