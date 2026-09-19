using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.WeaponModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class Headwear : NetworkBehaviour, IEnemyHeadwear, IProjectileDeflector, IDamageAbsorber
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private UnityEngine.Behaviour[] _behavioursToDisableWhenWorn;

		[SerializeField]
		private GameObject[] _gameObjectsToDeactivateWhenWorn;

		[SerializeField]
		private GameObject _grabColliderRoot;

		[SerializeField]
		private Transform _headMount;

		[SerializeField]
		private float _removalArmDelay = 0.4f;

		[SerializeField]
		private float _reequipClearDistance = 0.45f;

		[SerializeField]
		private float _invertedDotThreshold = -0.25f;

		[SerializeField]
		private float _enemyDropReequipBlockTime = 1.5f;

		[SerializeField]
		private float _bearerCollisionRestoreDelay = 0.6f;

		[SerializeField]
		private float _selfEquipGraceTime = 1.2f;

		[SerializeField]
		private float _selfEquipFallSpeed = 1.5f;

		[SerializeField]
		private float _damageDropImpulse = 16.5f;

		[SerializeField]
		private float _damageDropUpImpulse = 22.5f;

		[SerializeField]
		private float _shedSideImpulse = 15f;

		[SerializeField]
		private float _shedUpImpulse = 20f;

		[Header("Carrier veto release")]
		[SerializeField]
		private float _refusedCarrierReleaseDistance = 0.9f;

		[SerializeField]
		private float _refusedCarrierReleaseTimeout = 3f;

		[Header("Removal headroom check")]
		[SerializeField]
		private float _coverCheckDistance = 0.7f;

		[SerializeField]
		private float _coverProbeRadius = 0.35f;

		[SerializeField]
		private LayerMask _coverBlockingMask = -1679833019;

		[Header("Bullet ricochet")]
		[SerializeField]
		private Vector3 _ricochetLocalCenter = new Vector3(0f, 0.325f, 0f);

		[SerializeField]
		private float _ricochetRadius = 0.49f;

		[SerializeField]
		private bool _ricochetsWhenUnworn;

		[WeaverGenerated]
		[DefaultForProperty("IsWornInternal", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsWornInternal;

		[WeaverGenerated]
		[DefaultForProperty("WearerObject", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkObject _WearerObject;

		[WeaverGenerated]
		[DefaultForProperty("WearerPlayer", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerRef _WearerPlayer;

		[WeaverGenerated]
		[DefaultForProperty("WornStartTick", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _WornStartTick;

		private LineArmsModel _lineArmsModel;

		private HeadwearModel _headwearModel;

		private EnemyHeadwearModel _enemyHeadwearModel;

		private IPlayerStateService _playerStateService;

		private Transform _anchor;

		private HeadwearSlot _slot;

		private bool _wornApplied;

		private bool _hasPlayerWearer;

		private int _appliedWearerId;

		private uint _appliedEnemyId;

		private Vector3 _mountLocalPosition;

		private Quaternion _mountLocalRotation = Quaternion.identity;

		private bool _equipBlocked;

		private Transform _blockAnchor;

		private bool _masterDropPending;

		private bool _unequipPending;

		private bool _hasPendingRemover;

		private int _pendingRemoverId;

		private Vector3 _pendingDropImpulse;

		private bool _hasPendingDropImpulse;

		private float _equipBlockTimer;

		private const float SHED_REEQUIP_CLEAR_DISTANCE = 0.8f;

		private const int DEFAULT_COVER_MASK = -1679833019;

		private RagdollEntity _wearerRagdoll;

		private PlayerCharacterMovableBase _wearerMovable;

		private Transform _refusedCarrierWearer;

		private float _refusedCarrierReleaseTimer;

		private Collider[] _ownColliders;

		private Collider[] _bearerColliders;

		private int _shellLayerMask;

		private bool _isShellLayerMaskCached;

		private readonly RaycastHit[] _shellSweepHits = new RaycastHit[16];

		private bool _bearerCollisionIgnored;

		private float _collisionRestoreTimer;

		private int _lastGrabberId = -1;

		private float _selfEquipGraceTimer;

		private float _blockedUnheldSeconds;

		private const float REEQUIP_BLOCK_UNHELD_TIMEOUT = 1.5f;

		private CancellationTokenSource _stampLoopCts;

		private readonly RaycastHit[] _coverHits = new RaycastHit[8];

		private bool _damageAbsorbed;

		private float _nextBlockedSlotPushTime;

		private PlayerDamageAbsorbersModel _playerDamageAbsorbersModel;

		[Networked]
		[OnChangedRender("OnWornChanged")]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsWornInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.IsWornInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.IsWornInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkObject WearerObject
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WearerObject. Networked properties can only be accessed when Spawned() has been called.");
				}
				NetworkObject result = null;
				NetworkObject.NetworkUnwrap(base.Runner, *(NetworkId*)(Ptr + 1), ref result);
				return result;
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WearerObject. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 1) = NetworkObject.NetworkWrap(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe PlayerRef WearerPlayer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WearerPlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerRef*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WearerPlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerRef*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe int WornStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WornStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing Headwear.WornStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		public bool IsWorn
		{
			get
			{
				if (base.Object != null && base.Object.IsValid)
				{
					return IsWornInternal;
				}
				return false;
			}
		}

		public PlayerRef Wearer => WearerPlayer;

		public NetworkObject WearerNetworkObject => WearerObject;

		public Transform Anchor => _anchor;

		public IPointGrabable Grabable => _grabable;

		public bool IsWearerUnderCover
		{
			get
			{
				if (!IsWorn)
				{
					return false;
				}
				Vector3 vector = ((_headMount != null) ? _headMount.position : base.transform.position);
				if (!HasCoverAbove(vector) && !HasCoverAbove(vector + Vector3.right * _coverProbeRadius) && !HasCoverAbove(vector - Vector3.right * _coverProbeRadius) && !HasCoverAbove(vector + Vector3.forward * _coverProbeRadius))
				{
					return HasCoverAbove(vector - Vector3.forward * _coverProbeRadius);
				}
				return true;
			}
		}

		public bool IsWearerRagdolling
		{
			get
			{
				if (!IsWornInternal || WearerPlayer == PlayerRef.None)
				{
					return false;
				}
				RagdollEntity ragdollEntity = ResolveWearerRagdoll();
				if (ragdollEntity == null || !ragdollEntity.IsSimulated)
				{
					return false;
				}
				return !ragdollEntity.HasSimulationReason(RagdollSimulationReasonEnum.Swim);
			}
		}

		private Vector3 RicochetCenter => base.transform.TransformPoint(_ricochetLocalCenter);

		private float RicochetRadius
		{
			get
			{
				Vector3 lossyScale = base.transform.lossyScale;
				return _ricochetRadius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
			}
		}

		private int ShellLayerMask
		{
			get
			{
				if (_isShellLayerMaskCached)
				{
					return _shellLayerMask;
				}
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
				foreach (Collider collider in componentsInChildren)
				{
					if (!collider.isTrigger)
					{
						_shellLayerMask |= 1 << collider.gameObject.layer;
					}
				}
				_isShellLayerMaskCached = true;
				return _shellLayerMask;
			}
		}

		private bool HasCoverAbove(Vector3 origin)
		{
			int num = Physics.RaycastNonAlloc(origin, Vector3.up, _coverHits, _coverCheckDistance, _coverBlockingMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Transform transform = _coverHits[i].collider.transform;
				if (!transform.IsChildOf(base.transform) && (!(WearerObject != null) || !transform.IsChildOf(WearerObject.transform)))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasEquipPriorityOver(Headwear other)
		{
			if (WornStartTick == other.WornStartTick)
			{
				return base.Object.Id.Raw < other.Object.Id.Raw;
			}
			return WornStartTick < other.WornStartTick;
		}

		private RagdollEntity ResolveWearerRagdoll()
		{
			if (_wearerRagdoll == null && WearerObject != null)
			{
				_wearerRagdoll = WearerObject.GetComponentInChildren<RagdollEntity>(includeInactive: true);
			}
			return _wearerRagdoll;
		}

		[Inject]
		public void InjectDependencies(LineArmsModel lineArmsModel, HeadwearModel headwearModel, EnemyHeadwearModel enemyHeadwearModel, IPlayerStateService playerStateService, PlayerDamageAbsorbersModel playerDamageAbsorbersModel)
		{
			_lineArmsModel = lineArmsModel;
			_headwearModel = headwearModel;
			_enemyHeadwearModel = enemyHeadwearModel;
			_playerStateService = playerStateService;
			_playerDamageAbsorbersModel = playerDamageAbsorbersModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_grabable.LocalOnGrab += OnGrabbedWhileWorn;
			_grabable.LocalOnUnGrab += OnReleasedByGrabber;
			_stampLoopCts = new CancellationTokenSource();
			RunWornStampLoop(_stampLoopCts.Token).Forget();
			if ((bool)IsWornInternal)
			{
				ApplyWorn();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_grabable.LocalOnGrab -= OnGrabbedWhileWorn;
			_grabable.LocalOnUnGrab -= OnReleasedByGrabber;
			_stampLoopCts?.Cancel();
			_stampLoopCts?.Dispose();
			_stampLoopCts = null;
			if (_wornApplied)
			{
				if (_hasPlayerWearer)
				{
					_playerDamageAbsorbersModel.Clear(_appliedWearerId, this);
				}
				NotifyWornRegistry(isWorn: false);
			}
			ReleaseRefusedCarrier();
			ReleaseSlot();
		}

		public override void FixedUpdateNetwork()
		{
			if (_equipBlockTimer > 0f)
			{
				_equipBlockTimer -= base.Runner.DeltaTime;
			}
			if (_selfEquipGraceTimer > 0f)
			{
				_selfEquipGraceTimer -= base.Runner.DeltaTime;
			}
			if (_hasPendingDropImpulse && base.HasStateAuthority && !IsWornInternal && !_rigidbody.isKinematic)
			{
				_rigidbody.AddForce(_pendingDropImpulse, ForceMode.Impulse);
				_hasPendingDropImpulse = false;
			}
			if (_masterDropPending && base.HasStateAuthority)
			{
				_masterDropPending = false;
				if ((bool)IsWornInternal)
				{
					IsWornInternal = false;
				}
			}
			if (base.HasStateAuthority && (bool)IsWornInternal && _slot != null && _slot.IsTakenByOther(this))
			{
				IsWornInternal = false;
			}
			if (base.HasStateAuthority && !IsWornInternal && WearerPlayer != PlayerRef.None)
			{
				RagdollEntity ragdollEntity = ResolveWearerRagdoll();
				bool num = ragdollEntity != null && (ragdollEntity.IsSimulated || ragdollEntity.IsBlendingOut);
				bool flag = WearerObject == null || Vector3.Distance(base.transform.position, WearerObject.transform.position) > 0.8f;
				if (!num && flag)
				{
					WearerPlayer = PlayerRef.None;
					WearerObject = null;
					_wearerRagdoll = null;
				}
			}
			if (_unequipPending)
			{
				ProcessPendingUnequip();
			}
		}

		public override void Render()
		{
			if (_collisionRestoreTimer > 0f)
			{
				_collisionRestoreTimer -= Time.deltaTime;
				if (_collisionRestoreTimer <= 0f)
				{
					SetBearerCollisionIgnored(ignore: false);
				}
			}
			UpdateRefusedCarrierRelease();
			UpdateReequipLock();
			if ((bool)IsWornInternal && _wornApplied)
			{
				if (_anchor == null)
				{
					ResolveAnchor();
				}
				ReassertWornBody();
				if (!_bearerCollisionIgnored)
				{
					_bearerCollisionIgnored = SetBearerCollisionIgnored(ignore: true);
				}
			}
		}

		private void ReassertWornBody()
		{
			if (!(_anchor == null) && !_rigidbody.isKinematic && !_unequipPending && _grabable.GrabbedByPlayersCount <= 0)
			{
				_rigidbody.isKinematic = true;
				_grabable.ChangeRigidbodyKinematic = false;
				StampWornPose();
			}
		}

		private async UniTaskVoid RunWornStampLoop(CancellationToken token)
		{
			while (!(await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, token).SuppressCancellationThrow()))
			{
				StampWornPose();
			}
		}

		private void StampWornPose()
		{
			if (!(base.Object == null) && base.Object.IsValid && (bool)IsWornInternal && !(_anchor == null))
			{
				Quaternion quaternion = _anchor.rotation * Quaternion.Inverse(_mountLocalRotation);
				Vector3 position = _anchor.position - quaternion * _mountLocalPosition;
				base.transform.SetPositionAndRotation(position, quaternion);
			}
		}

		public void TryEquip(HeadwearSlot slot)
		{
			if (base.HasStateAuthority && !IsWornInternal && !_equipBlocked && !(_equipBlockTimer > 0f) && (!slot.HasPlayerOwner || _playerStateService.IsPlayerAlive(slot.PlayerOwner.PlayerId)) && !slot.IsTakenByOther(this) && (!slot.HasPlayerOwner || !(WearerPlayer != PlayerRef.None) || slot.PlayerOwner.PlayerId != WearerPlayer.PlayerId) && !(Vector3.Dot(base.transform.up, Vector3.up) > _invertedDotThreshold) && (!(_selfEquipGraceTimer > 0f) || !slot.HasPlayerOwner || slot.PlayerOwner.PlayerId != _lastGrabberId || !(_rigidbody.linearVelocity.y > 0f - _selfEquipFallSpeed)))
			{
				ReleaseGrabbers();
				WearerObject = slot.Bearer;
				WearerPlayer = slot.PlayerOwner;
				WornStartTick = base.Runner.Tick;
				IsWornInternal = true;
				_slot = slot;
				slot.Claim(this);
			}
		}

		private bool IsOwnShellCollider(Collider collider)
		{
			if (collider != null && !collider.isTrigger)
			{
				return collider.transform.IsChildOf(base.transform);
			}
			return false;
		}

		private bool HasShellOnPath(Vector3 travelPoint, Vector3 travelDirection, float travelBack, float projectileRadius)
		{
			int num = Physics.SphereCastNonAlloc(travelPoint - travelDirection * travelBack, projectileRadius, travelDirection, _shellSweepHits, travelBack + projectileRadius, ShellLayerMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (_shellSweepHits[i].collider.transform.IsChildOf(base.transform))
				{
					return true;
				}
			}
			return false;
		}

		public ProjectileHitResponse ResolveHit(Collider hitCollider, Vector3 travelPoint, Vector3 travelDirection, float travelBack, float projectileRadius, out ProjectileDeflection deflection)
		{
			deflection = default(ProjectileDeflection);
			if (!IsWorn && !_ricochetsWhenUnworn)
			{
				return ProjectileHitResponse.None;
			}
			if (!HeadwearRicochetSolver.TrySolve(RicochetCenter, RicochetRadius, projectileRadius, travelPoint, travelDirection, travelBack, out var point, out var normal))
			{
				return ProjectileHitResponse.PassThrough;
			}
			if (!IsOwnShellCollider(hitCollider) && !HasShellOnPath(travelPoint, travelDirection, travelBack, projectileRadius))
			{
				return ProjectileHitResponse.PassThrough;
			}
			Transform shieldedRoot = ((IsWorn && WearerObject != null) ? WearerObject.transform : null);
			deflection = new ProjectileDeflection(point, normal, base.transform, shieldedRoot);
			return ProjectileHitResponse.Deflect;
		}

		public bool TryAbsorb(DamageData damageData)
		{
			if (!IsWorn || _damageAbsorbed || !_hasPlayerWearer)
			{
				return false;
			}
			if (damageData == null || !damageData.Source.HasValue || damageData.Source.Value.Category != DamageCauseCategory.Enemy)
			{
				return false;
			}
			_damageAbsorbed = true;
			DropFromDamage(damageData.Direction);
			return true;
		}

		public void RequestUnequip()
		{
			RequestUnequipRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2929742890u)]
		private void RequestUnequipRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2929742890u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.HeadwearModule.Scripts.Headwear::RequestUnequipRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			MarkUnequipPending(hasRemover: false, 0);
		}

		private void OnGrabbedWhileWorn(int playerId)
		{
			_lastGrabberId = playerId;
			if ((bool)IsWornInternal && base.Runner.LocalPlayer.PlayerId == playerId && !((float)((int)base.Runner.Tick - WornStartTick) * base.Runner.DeltaTime < _removalArmDelay) && !IsWearerUnderCover)
			{
				RemoveByGrabRpc(playerId);
			}
		}

		private void OnReleasedByGrabber()
		{
			_selfEquipGraceTimer = _selfEquipGraceTime;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3557093936u)]
		private void RemoveByGrabRpc([RpcPayload(4)] int grabberId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3557093936u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.HeadwearModule.Scripts.Headwear::RemoveByGrabRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(grabberId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			MarkUnequipPending(hasRemover: true, grabberId);
		}

		private void MarkUnequipPending(bool hasRemover, int removerId)
		{
			_unequipPending = true;
			_hasPendingRemover = hasRemover;
			_pendingRemoverId = removerId;
		}

		private void ProcessPendingUnequip()
		{
			if (!IsWornInternal)
			{
				ClearPendingUnequip();
			}
			else
			{
				if (!base.HasStateAuthority)
				{
					return;
				}
				if (IsWearerUnderCover)
				{
					ClearPendingUnequip();
					return;
				}
				if (_hasPendingRemover && !_hasPlayerWearer)
				{
					_enemyHeadwearModel.SetRemovedByPlayer(_appliedEnemyId, _pendingRemoverId);
				}
				ClearPendingUnequip();
				IsWornInternal = false;
			}
		}

		private void ClearPendingUnequip()
		{
			_unequipPending = false;
			_hasPendingRemover = false;
			_pendingRemoverId = 0;
		}

		public void DropOnWearerLeft()
		{
			DropOnWearerLost(pushAside: false);
		}

		public void DropOnWearerRagdolled()
		{
			DropOnWearerLost(pushAside: true);
		}

		private void DropOnWearerLost(bool pushAside)
		{
			if (base.Runner.IsSharedModeMasterClient)
			{
				_masterDropPending = true;
				if (pushAside)
				{
					_pendingDropImpulse = ResolveShedImpulse();
					_hasPendingDropImpulse = true;
				}
				if (!base.Object.HasStateAuthority)
				{
					base.Object.RequestStateAuthority();
				}
			}
		}

		private Vector3 ResolveShedImpulse()
		{
			Vector3 vector = ((WearerObject != null) ? WearerObject.transform.right : base.transform.right);
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.right;
			}
			return vector.normalized * _shedSideImpulse + Vector3.up * _shedUpImpulse;
		}

		public void DropFromDamage(Vector3 attackDirection)
		{
			Vector3 vector = attackDirection;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = ((WearerObject != null) ? (-WearerObject.transform.forward) : (-base.transform.forward));
			}
			DropFromEnemy(vector.normalized * _damageDropImpulse + Vector3.up * _damageDropUpImpulse);
		}

		public void DropFromEnemy(Vector3 impulse)
		{
			if (base.Runner.IsSharedModeMasterClient)
			{
				DropFromEnemyRpc(impulse);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 4056424818u)]
		private void DropFromEnemyRpc([RpcPayload(12)] Vector3 impulse)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4056424818u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.HeadwearModule.Scripts.Headwear::DropFromEnemyRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(impulse, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if ((bool)IsWornInternal)
			{
				_pendingDropImpulse = impulse;
				_hasPendingDropImpulse = true;
				_equipBlockTimer = _enemyDropReequipBlockTime;
				IsWornInternal = false;
			}
		}

		private void OnWornChanged()
		{
			if ((bool)IsWornInternal)
			{
				ApplyWorn();
			}
			else
			{
				RestoreWorn();
			}
		}

		private void ApplyWorn()
		{
			if (_wornApplied)
			{
				return;
			}
			ClearPendingUnequip();
			if (base.Runner.IsSharedModeMasterClient && !base.Object.HasStateAuthority)
			{
				base.Object.RequestStateAuthority();
			}
			_hasPlayerWearer = WearerPlayer != PlayerRef.None;
			ResolveAnchor();
			if (_slot != null && _slot.RefusesClaim(this))
			{
				return;
			}
			CacheMountLocalPose();
			UnityEngine.Behaviour[] behavioursToDisableWhenWorn = _behavioursToDisableWhenWorn;
			for (int i = 0; i < behavioursToDisableWhenWorn.Length; i++)
			{
				behavioursToDisableWhenWorn[i].enabled = false;
			}
			GameObject[] gameObjectsToDeactivateWhenWorn = _gameObjectsToDeactivateWhenWorn;
			for (int i = 0; i < gameObjectsToDeactivateWhenWorn.Length; i++)
			{
				gameObjectsToDeactivateWhenWorn[i].SetActive(value: false);
			}
			_grabable.ChangeRigidbodyKinematic = false;
			_rigidbody.isKinematic = true;
			if (_grabColliderRoot != null)
			{
				_grabColliderRoot.SetActive(value: true);
			}
			_grabable.enabled = true;
			_collisionRestoreTimer = 0f;
			_bearerCollisionIgnored = SetBearerCollisionIgnored(ignore: true);
			if (_hasPlayerWearer && WearerObject != null)
			{
				ReleaseRefusedCarrier();
				_wearerMovable = WearerObject.GetComponentInChildren<PlayerCharacterMovableBase>(includeInactive: true);
				if (_wearerMovable != null)
				{
					_wearerMovable.SetRefusedCarrier(base.transform);
				}
			}
			_wornApplied = true;
			_appliedWearerId = WearerPlayer.PlayerId;
			_appliedEnemyId = ((WearerObject != null) ? WearerObject.Id.Raw : 0u);
			_damageAbsorbed = false;
			if (_hasPlayerWearer)
			{
				_playerDamageAbsorbersModel.Set(_appliedWearerId, this);
			}
			NotifyWornRegistry(isWorn: true);
		}

		private void RestoreWorn()
		{
			if (_wornApplied)
			{
				ClearPendingUnequip();
				UnityEngine.Behaviour[] behavioursToDisableWhenWorn = _behavioursToDisableWhenWorn;
				for (int i = 0; i < behavioursToDisableWhenWorn.Length; i++)
				{
					behavioursToDisableWhenWorn[i].enabled = true;
				}
				GameObject[] gameObjectsToDeactivateWhenWorn = _gameObjectsToDeactivateWhenWorn;
				for (int i = 0; i < gameObjectsToDeactivateWhenWorn.Length; i++)
				{
					gameObjectsToDeactivateWhenWorn[i].SetActive(value: true);
				}
				if (_grabColliderRoot != null)
				{
					_grabColliderRoot.SetActive(value: true);
				}
				_grabable.ChangeRigidbodyKinematic = true;
				_rigidbody.isKinematic = !base.HasStateAuthority;
				if (_wearerMovable != null)
				{
					_refusedCarrierWearer = ((WearerObject != null) ? WearerObject.transform : null);
					_refusedCarrierReleaseTimer = _refusedCarrierReleaseTimeout;
				}
				if (_hasPlayerWearer)
				{
					_playerDamageAbsorbersModel.Clear(_appliedWearerId, this);
				}
				NotifyWornRegistry(isWorn: false);
				ReleaseSlot();
				_wornApplied = false;
				_equipBlocked = true;
				_blockedUnheldSeconds = 0f;
				_blockAnchor = _anchor;
				_anchor = null;
				_wearerRagdoll = null;
				_bearerCollisionIgnored = false;
				_collisionRestoreTimer = _bearerCollisionRestoreDelay;
			}
		}

		private bool SetBearerCollisionIgnored(bool ignore)
		{
			if (ignore)
			{
				_ownColliders = GetComponentsInChildren<Collider>(includeInactive: true);
				_bearerColliders = ((WearerObject != null) ? WearerObject.GetComponentsInChildren<Collider>(includeInactive: true) : null);
			}
			if (_ownColliders == null || _bearerColliders == null)
			{
				return false;
			}
			Collider[] ownColliders = _ownColliders;
			foreach (Collider collider in ownColliders)
			{
				if (collider == null)
				{
					continue;
				}
				Collider[] bearerColliders = _bearerColliders;
				foreach (Collider collider2 in bearerColliders)
				{
					if (!(collider2 == null))
					{
						Physics.IgnoreCollision(collider, collider2, ignore);
					}
				}
			}
			if (!ignore)
			{
				_ownColliders = null;
				_bearerColliders = null;
			}
			return true;
		}

		private void UpdateRefusedCarrierRelease()
		{
			if (_refusedCarrierReleaseTimer <= 0f)
			{
				return;
			}
			if ((bool)IsWornInternal)
			{
				_refusedCarrierReleaseTimer = 0f;
				_refusedCarrierWearer = null;
				return;
			}
			_refusedCarrierReleaseTimer -= Time.deltaTime;
			if (_refusedCarrierWearer == null || Vector3.Distance(base.transform.position, _refusedCarrierWearer.position) > _refusedCarrierReleaseDistance || _refusedCarrierReleaseTimer <= 0f)
			{
				ReleaseRefusedCarrier();
			}
		}

		private void ReleaseRefusedCarrier()
		{
			_refusedCarrierReleaseTimer = 0f;
			_refusedCarrierWearer = null;
			if (!(_wearerMovable == null))
			{
				_wearerMovable.ClearRefusedCarrier(base.transform);
				_wearerMovable = null;
			}
		}

		private void UpdateReequipLock()
		{
			if (!_equipBlocked)
			{
				return;
			}
			if (_blockAnchor == null)
			{
				_equipBlocked = false;
			}
			else if (Vector3.Distance((_headMount != null) ? _headMount.position : base.transform.position, _blockAnchor.position) > _reequipClearDistance)
			{
				_equipBlocked = false;
				_blockAnchor = null;
				_blockedUnheldSeconds = 0f;
			}
			else if (_grabable.GrabbedByPlayersCount == 0)
			{
				_blockedUnheldSeconds += Time.deltaTime;
				if (_blockedUnheldSeconds >= 1.5f)
				{
					_equipBlocked = false;
					_blockAnchor = null;
					_blockedUnheldSeconds = 0f;
				}
			}
			else
			{
				_blockedUnheldSeconds = 0f;
			}
		}

		private void ReleaseGrabbers()
		{
			foreach (int item in new List<int>(_grabable.GrabbedByPlayers))
			{
				if (_lineArmsModel.TryGetLineArmForPlayer(item, out var lineArm))
				{
					lineArm.UnJoinAll(throwItem: false);
				}
			}
		}

		private void CacheMountLocalPose()
		{
			if (_headMount == null)
			{
				_mountLocalPosition = Vector3.zero;
				_mountLocalRotation = Quaternion.identity;
			}
			else
			{
				_mountLocalPosition = base.transform.InverseTransformPoint(_headMount.position);
				_mountLocalRotation = Quaternion.Inverse(base.transform.rotation) * _headMount.rotation;
			}
		}

		private void ResolveAnchor()
		{
			if (WearerObject == null)
			{
				return;
			}
			HeadwearSlot componentInChildren = WearerObject.GetComponentInChildren<HeadwearSlot>(includeInactive: true);
			if (!(componentInChildren == null))
			{
				_slot = componentInChildren;
				if (!componentInChildren.RefusesClaim(this))
				{
					componentInChildren.Claim(this);
					_anchor = componentInChildren.Anchor;
				}
			}
		}

		private void ReleaseSlot()
		{
			if (_slot == null)
			{
				_slot = null;
				return;
			}
			_slot.Release(this);
			_slot = null;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1f, 0.15f, 0.2f, 0.9f);
			Gizmos.DrawWireSphere(RicochetCenter, RicochetRadius);
		}

		private void NotifyWornRegistry(bool isWorn)
		{
			if (_hasPlayerWearer)
			{
				_headwearModel.SetHeadwear(_appliedWearerId, isWorn, this);
			}
			else
			{
				_enemyHeadwearModel.Set(_appliedEnemyId, isWorn, this);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsWornInternal = _IsWornInternal;
			WearerObject = _WearerObject;
			WearerPlayer = _WearerPlayer;
			WornStartTick = _WornStartTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsWornInternal = IsWornInternal;
			_WearerObject = WearerObject;
			_WearerPlayer = WearerPlayer;
			_WornStartTick = WornStartTick;
		}

		[NetworkRpcWeavedInvoker(2929742890u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestUnequipRpc_0040Invoker2929742890([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((Headwear)context.TargetBehaviour).RequestUnequipRpc();
		}

		[NetworkRpcWeavedInvoker(3557093936u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RemoveByGrabRpc_0040Invoker3557093936([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((Headwear)context.TargetBehaviour).RemoveByGrabRpc(value);
		}

		[NetworkRpcWeavedInvoker(4056424818u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DropFromEnemyRpc_0040Invoker4056424818([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((Headwear)context.TargetBehaviour).DropFromEnemyRpc(value);
		}
	}
}
