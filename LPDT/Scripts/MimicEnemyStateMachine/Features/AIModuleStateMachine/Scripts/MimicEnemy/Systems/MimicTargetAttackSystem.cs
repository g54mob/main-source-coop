using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AnimationModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(2)]
	public class MimicTargetAttackSystem : MonoSystem
	{
		private const float SNAPSHOT_INTENSITY_QUANTIZE_FACTOR = 255f;

		private const int LINE_OF_SIGHT_HITS_BUFFER_SIZE = 8;

		private const float MAX_FOLLOW_MODE_DURATION = 6f;

		private const float FOLLOW_MODE_DURATION_SLACK = 1f;

		[SerializeField]
		private KillEndedAnimationFunctionReactor _killEndedAnimationFunctionReactor;

		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		[SerializeField]
		private MimicAudioController _mimicAudioController;

		[SerializeField]
		private ParticleSystem _roarParticle;

		[SerializeField]
		private Transform _lookAtTarget;

		[SerializeField]
		private Transform _viewPoint;

		[SerializeField]
		private EnemyRotator _enemyRotator;

		[SerializeField]
		private MimicLipSyncController _lipSyncController;

		[SerializeField]
		private float _damageImpulseStrength;

		[SerializeField]
		private float _raycastDistance = 10f;

		[SerializeField]
		private float _timeToKill = 2f;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private IPlayerStateService _playerStateService;

		private PlayerMovableModel _playerMovableModel;

		private StatModifier _speedDecreaseModifier;

		private ILocalPlayerFollowService _localPlayerFollowService;

		private MimicEnemyContext _context;

		private MimicEnemy _mimicEnemy;

		private INavigationService _navigationService;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private float _killTimer;

		private bool _playerCaught;

		private PlayerDataHolder _attackTarget;

		private bool _isLocalSnapshotActive;

		private bool _isLocalFollowActive;

		private bool _isLocalRotationOverrideActive;

		private bool _isSpeedEffectApplied;

		private bool _isLipSyncAttackOverrideActive;

		private float _localFollowStartedAt;

		private int _speedEffectPlayerId;

		private readonly RaycastHit[] _lineOfSightHits = new RaycastHit[8];

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SnapshotIntensityQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SnapshotIntensityQuantized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TargetPlayerId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _TargetPlayerId;

		private bool _enabled;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe byte SnapshotIntensityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicTargetAttackSystem.SnapshotIntensityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[0];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicTargetAttackSystem.SnapshotIntensityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[0] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicTargetAttackSystem.TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicTargetAttackSystem.TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		public float SnapshotIntensity => (float)(int)SnapshotIntensityQuantized / 255f;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, MimicEnemy mimicEnemy, SpawnedEntityStatsModel spawnedEntityStatsModel, PlayerMovableModel playerMovableModel, IPlayerStateService playerStateService, PlayerDamageablesTrackModel playerDamageablesTrackModel, ILocalPlayerFollowService localPlayerFollowService, INavigationService navigationService, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_context = context;
			_mimicEnemy = mimicEnemy;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_playerMovableModel = playerMovableModel;
			_playerStateService = playerStateService;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_localPlayerFollowService = localPlayerFollowService;
			_navigationService = navigationService;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseLocalAttackEffects();
			if (_isSpeedEffectApplied)
			{
				DiscardSpeedEffect(_speedEffectPlayerId);
			}
			base.Despawned(runner, hasState);
		}

		private void ReleaseLocalAttackEffects()
		{
			if (_isLocalFollowActive)
			{
				ExitLocalFollowMode();
			}
			if (_isLocalRotationOverrideActive)
			{
				_isLocalRotationOverrideActive = false;
				if (_playerMovableModel.Rotator != null && _playerMovableModel.Rotator.RotationObject == _lookAtTarget)
				{
					_playerMovableModel.Rotator.RotationObject = null;
				}
			}
			if (_isLocalSnapshotActive)
			{
				_isLocalSnapshotActive = false;
				_mimicAudioController.StopSnapshot();
			}
		}

		private void Awake()
		{
			_speedDecreaseModifier = new StatModifier(0f, ModifierType.PercentMulti);
		}

		public override void Enable()
		{
			if (base.HasStateAuthority)
			{
				_killEndedAnimationFunctionReactor.OnKillEnded += HandleKill;
				_enabled = true;
				_context.SetIsAggressive(value: true);
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_killEndedAnimationFunctionReactor.OnKillEnded -= HandleKill;
				_enabled = false;
				Clear();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !base.HasStateAuthority || !_enabled)
			{
				return;
			}
			if (ShouldAbortAttackBecauseTargetHeld())
			{
				AbortAttackBecauseTargetHeld();
				return;
			}
			if (_context.TargetPositionCompleted && !_context.IsAttackInProcess)
			{
				StartAttack();
			}
			if (_context.IsAttackInProcess)
			{
				WaitForKillProcess();
			}
		}

		private void Update()
		{
			if (base.Initialized)
			{
				if (base.Runner.LocalPlayer.PlayerId == TargetPlayerId)
				{
					_mimicAudioController.SetParameterValue(SnapshotIntensity);
				}
				ProcessLipSyncAttackOverride();
				ProcessLocalFollowTimeout();
				if (_enabled && base.HasStateAuthority && _context.IsAttackInProcess && _attackTarget?.NetworkObject != null)
				{
					int playerId = _attackTarget.NetworkObject.InputAuthority.PlayerId;
					SetTargetPlayerId(playerId);
				}
			}
		}

		private void StartAttack()
		{
			if (!(_context.PriorityPlayer?.NetworkObject == null))
			{
				int playerId = _context.PriorityPlayer.NetworkObject.InputAuthority.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(playerId))
				{
					_attackTarget = _context.PriorityPlayer;
					SetTargetPlayerId(playerId);
					_context.IsAttackInProcess = true;
					EnableSnapshotRPC(playerId);
					ApplySpeedEffect(playerId);
				}
			}
		}

		private bool ShouldAbortAttackBecauseTargetHeld()
		{
			int currentAttackPlayerId = GetCurrentAttackPlayerId();
			if (currentAttackPlayerId != 0)
			{
				return !_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(currentAttackPlayerId);
			}
			return false;
		}

		private int GetCurrentAttackPlayerId()
		{
			if (_context.IsAttackInProcess && TargetPlayerId != 0)
			{
				return TargetPlayerId;
			}
			if (_context.PriorityPlayer?.NetworkObject == null)
			{
				return 0;
			}
			return _context.PriorityPlayer.NetworkObject.InputAuthority.PlayerId;
		}

		private void AbortAttackBecauseTargetHeld()
		{
			Clear();
			_mimicEnemy?.TriggerEvent(MimicEvent.OnTargetLost);
		}

		private void WaitForKillProcess()
		{
			if (!TryGetAttackTarget(out var attackTarget))
			{
				AbortAttackBecauseTargetLost();
				return;
			}
			if (!_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayerId))
			{
				AbortAttackBecauseTargetHeld();
				return;
			}
			_killTimer += base.Runner.DeltaTime;
			if (_killTimer < _timeToKill)
			{
				SetSnapshotIntensity(_killTimer / _timeToKill);
				ProcessRotation();
				return;
			}
			if (!_playerCaught)
			{
				_playerCaught = true;
				_animatorController.PlayAnimation(AnimationType.Kill);
				EnableRotationObjectRPC(TargetPlayerId);
				if (_playerStateService.IsPlayerAlive(TargetPlayerId))
				{
					EnterFollowModeRpc(TargetPlayerId);
				}
				PlayerRoarParticleRPC(TargetPlayerId);
			}
			if (_playerCaught)
			{
				Vector3 attackTargetWorldPosition = GetAttackTargetWorldPosition(attackTarget);
				_enemyRotator.RotateTowardsDirection(attackTargetWorldPosition - _viewPoint.position);
			}
		}

		private void ProcessRotation()
		{
			if (!TryGetAttackTarget(out var attackTarget))
			{
				return;
			}
			Vector3 position = _viewPoint.position;
			Vector3 attackTargetWorldPosition = GetAttackTargetWorldPosition(attackTarget);
			Vector3 direction = attackTargetWorldPosition - position;
			if (direction.sqrMagnitude <= 0f)
			{
				_enemyRotator.RotateTowardsDirection(_context.NavMeshAgent.velocity);
				return;
			}
			if (TryRaycastClosest(position, direction.normalized, _raycastDistance, out var closestHit))
			{
				if (!IsHitFromPlayer(attackTarget, closestHit, attackTargetWorldPosition))
				{
					direction = _context.NavMeshAgent.velocity;
				}
			}
			else
			{
				direction = _context.NavMeshAgent.velocity;
			}
			_enemyRotator.RotateTowardsDirection(direction);
		}

		private Vector3 GetAttackTargetWorldPosition(PlayerDataHolder attackTarget)
		{
			PlayerRef inputAuthority = attackTarget.NetworkObject.InputAuthority;
			if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
			{
				return position;
			}
			return attackTarget.NetworkObject.transform.position;
		}

		private static bool IsHitFromPlayer(PlayerDataHolder attackTarget, RaycastHit hit, Vector3 targetWorldPos)
		{
			Transform transform = attackTarget.NetworkObject.transform;
			if (hit.transform == transform || hit.transform.IsChildOf(transform))
			{
				return true;
			}
			return Vector3.Distance(hit.point, targetWorldPos) < 1.5f;
		}

		private bool TryRaycastClosest(Vector3 origin, Vector3 direction, float distance, out RaycastHit closestHit)
		{
			int num = Physics.RaycastNonAlloc(origin, direction, _lineOfSightHits, distance);
			closestHit = default(RaycastHit);
			if (num <= 0)
			{
				return false;
			}
			int num2 = 0;
			float distance2 = _lineOfSightHits[0].distance;
			int num3 = Mathf.Min(num, _lineOfSightHits.Length);
			for (int i = 1; i < num3; i++)
			{
				if (!(_lineOfSightHits[i].distance >= distance2))
				{
					distance2 = _lineOfSightHits[i].distance;
					num2 = i;
				}
			}
			closestHit = _lineOfSightHits[num2];
			return true;
		}

		private void HandleKill()
		{
			if (!_playerCaught)
			{
				return;
			}
			if (!TryGetAttackTarget(out var attackTarget))
			{
				AbortAttackBecauseTargetLost();
				return;
			}
			DisableSnapshotRPC(TargetPlayerId);
			DiscardSpeedEffect(TargetPlayerId);
			ExitFollowModeRpc(TargetPlayerId);
			if (_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayerId) && _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(TargetPlayerId, out var value))
			{
				Vector3 attackTargetWorldPosition = GetAttackTargetWorldPosition(attackTarget);
				value.Damage(new DamageData
				{
					Damage = _context.GetStatValue(EntityStatType.Damage),
					Position = base.transform.position,
					Direction = attackTargetWorldPosition - base.transform.position,
					DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
					Force = _damageImpulseStrength,
					ForceMode = ForceMode.Impulse,
					Source = DamageDataSourceExtensions.ForEnemyAttack(_context.NavMeshAgent.transform, EnemyType.Mimic.ToString(), DamageType.Melee)
				});
			}
			DisableRotationObjectRPC(TargetPlayerId);
			_playerCaught = false;
			_killTimer = 0f;
			_context.AttackCooldown = _context.AttackCooldownTime;
		}

		private bool TryGetAttackTarget(out PlayerDataHolder attackTarget)
		{
			attackTarget = _attackTarget;
			if (attackTarget?.NetworkObject != null)
			{
				return attackTarget.NetworkObject.IsSpawnedAndValid();
			}
			return false;
		}

		private void SetSnapshotIntensity(float value)
		{
			byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp01(value) * 255f), 0, 255);
			if (SnapshotIntensityQuantized != b)
			{
				SnapshotIntensityQuantized = b;
			}
		}

		private void SetTargetPlayerId(int value)
		{
			if (TargetPlayerId != value)
			{
				TargetPlayerId = value;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1591079561u)]
		private void EnterFollowModeRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1591079561u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::EnterFollowModeRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_isLocalFollowActive = true;
				_localFollowStartedAt = Time.time;
				_localPlayerFollowService.EnterFollowMode();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1076628777u)]
		private void ExitFollowModeRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1076628777u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::ExitFollowModeRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				ExitLocalFollowMode();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3744510118u)]
		private void EnableSnapshotRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3744510118u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::EnableSnapshotRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_isLocalSnapshotActive = true;
				_mimicAudioController.ActivateSnapshot();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4156169019u)]
		private void DisableSnapshotRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4156169019u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::DisableSnapshotRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_isLocalSnapshotActive = false;
				_mimicAudioController.StopSnapshot();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2188191859u)]
		private void EnableRotationObjectRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2188191859u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::EnableRotationObjectRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_isLocalRotationOverrideActive = true;
				_playerMovableModel.Rotator.RotationObject = _lookAtTarget;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 292098868u)]
		private void DisableRotationObjectRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(292098868u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::DisableRotationObjectRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_isLocalRotationOverrideActive = false;
				_playerMovableModel.Rotator.RotationObject = null;
			}
		}

		private void ApplySpeedEffect(int playerId)
		{
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				value.AddModifierSynchronized(EntityStatType.WalkSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.AddModifierSynchronized(EntityStatType.SprintSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.AddModifierSynchronized(EntityStatType.CrouchSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				_isSpeedEffectApplied = true;
				_speedEffectPlayerId = playerId;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1761636962u)]
		private void PlayerRoarParticleRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1761636962u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems.MimicTargetAttackSystem::PlayerRoarParticleRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_mimicAudioController.PlaySoundBite();
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_roarParticle.Play();
			}
		}

		private void DiscardSpeedEffect(int playerId)
		{
			_isSpeedEffectApplied = false;
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				value.RemoveModifierSynchronized(EntityStatType.WalkSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.RemoveModifierSynchronized(EntityStatType.SprintSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.RemoveModifierSynchronized(EntityStatType.CrouchSpeed, _speedDecreaseModifier, RPCType.InAllWays);
			}
		}

		public override void Clear()
		{
			DisableSnapshotRPC(TargetPlayerId);
			ExitFollowModeRpc(TargetPlayerId);
			DisableRotationObjectRPC(TargetPlayerId);
			DiscardSpeedEffect(TargetPlayerId);
			ResetAttackVisualState();
			_playerCaught = false;
			_killTimer = 0f;
			SetSnapshotIntensity(0f);
			SetTargetPlayerId(0);
			_attackTarget = null;
			_context.IsAttackInProcess = false;
			_context.SetIsAggressive(value: false);
		}

		private void ResetAttackVisualState()
		{
			_animatorController.ResetAnimation(AnimationType.Kill);
			_context.SetIsAggressive(value: false);
			_lipSyncController.SetMouthClosed();
			_isLipSyncAttackOverrideActive = false;
		}

		private void ProcessLocalFollowTimeout()
		{
			if (_isLocalFollowActive && !(Time.time - _localFollowStartedAt < ResolveMaxFollowModeDuration()))
			{
				ExitLocalFollowMode();
			}
		}

		private void ExitLocalFollowMode()
		{
			_isLocalFollowActive = false;
			_localFollowStartedAt = 0f;
			_localPlayerFollowService.ExitFollowMode();
		}

		private float ResolveMaxFollowModeDuration()
		{
			return Mathf.Max(6f, _timeToKill + 1f);
		}

		private void AbortAttackBecauseTargetLost()
		{
			Clear();
			_mimicEnemy?.TriggerEvent(MimicEvent.OnTargetLost);
		}

		private void ProcessLipSyncAttackOverride()
		{
			if (_context.IsAttackInProcess)
			{
				if (!_isLipSyncAttackOverrideActive)
				{
					_isLipSyncAttackOverrideActive = true;
					_lipSyncController.SetMouthOpenWithTongue();
				}
			}
			else if (_isLipSyncAttackOverrideActive)
			{
				_isLipSyncAttackOverrideActive = false;
				_lipSyncController.SetMouthClosed();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			SnapshotIntensityQuantized = _SnapshotIntensityQuantized;
			TargetPlayerId = _TargetPlayerId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_SnapshotIntensityQuantized = SnapshotIntensityQuantized;
			_TargetPlayerId = TargetPlayerId;
		}

		[NetworkRpcWeavedInvoker(1591079561u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EnterFollowModeRpc_0040Invoker1591079561([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).EnterFollowModeRpc(value);
		}

		[NetworkRpcWeavedInvoker(1076628777u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ExitFollowModeRpc_0040Invoker1076628777([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).ExitFollowModeRpc(value);
		}

		[NetworkRpcWeavedInvoker(3744510118u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EnableSnapshotRPC_0040Invoker3744510118([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).EnableSnapshotRPC(value);
		}

		[NetworkRpcWeavedInvoker(4156169019u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DisableSnapshotRPC_0040Invoker4156169019([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).DisableSnapshotRPC(value);
		}

		[NetworkRpcWeavedInvoker(2188191859u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EnableRotationObjectRPC_0040Invoker2188191859([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).EnableRotationObjectRPC(value);
		}

		[NetworkRpcWeavedInvoker(292098868u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DisableRotationObjectRPC_0040Invoker292098868([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).DisableRotationObjectRPC(value);
		}

		[NetworkRpcWeavedInvoker(1761636962u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayerRoarParticleRPC_0040Invoker1761636962([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicTargetAttackSystem)context.TargetBehaviour).PlayerRoarParticleRPC(value);
		}
	}
}
