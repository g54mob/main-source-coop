using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.ItemsModule.Scripts;
using Features.Movement.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units
{
	[NetworkBehaviourWeaved(2)]
	public class CoinRobBehaviour : NetworkBehaviour, ITeleportable, IStateAuthorityChanged, IPublicFacingInterface, IEnemyTypeProvider
	{
		private const float NetworkQuantizeFactor = 10f;

		private const float NetworkDequantizeFactor = 0.1f;

		private const string SpeedParameter = "Speed";

		[SerializeField]
		private NavMeshAgent _agent;

		[SerializeField]
		private EnemyItemHolder _itemHolder;

		[SerializeField]
		private NetworkedAnimator _animator;

		[SerializeField]
		private int _animationsLayer;

		[SerializeField]
		private List<EventReference> _footstepsPoolReferences;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private MovableAnimationFunctionReactor _movableAnimationFunctionReactor;

		[SerializeField]
		private DamageableAnimationFunctionReactor _damageableAnimationFunctionReactor;

		[SerializeField]
		private EnemyStatHealthController _statHealthController;

		[SerializeField]
		private SimpleEnemyDamageable _damageable;

		[SerializeField]
		private bool _isSwarmKing;

		[SerializeField]
		private CoinRobLocomotionSettings _locomotionSettings;

		[SerializeField]
		private FearHoleAbsorbVisualController _fearHoleAbsorbVisualController;

		private IItem _attachedItem;

		private IAudioService _audioService;

		private float _kingAnimatorSpeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SwarmHostNetworkId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _SwarmHostNetworkId;

		[WeaverGenerated]
		[DefaultForProperty("LocomotionSpeedQuantized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _LocomotionSpeedQuantized;

		public EnemyType EnemyType => EnemyType.CoinRobSwarm;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe NetworkId SwarmHostNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobBehaviour.SwarmHostNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobBehaviour.SwarmHostNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe short LocomotionSpeedQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobBehaviour.LocomotionSpeedQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobBehaviour.LocomotionSpeedQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[2] = value;
			}
		}

		public bool IsSwarmKing => _isSwarmKing;

		public bool HasAttachedItem
		{
			get
			{
				IItem item;
				return TryGetAttachedItem(out item);
			}
		}

		public bool IsCarryingLoot => HasAttachedItem;

		public IItem AttachedItem
		{
			get
			{
				TryGetAttachedItem(out var item);
				return item;
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public NetworkedAnimator NetworkedAnimator => _animator;

		public int AnimationsLayer => _animationsLayer;

		public int LastDamageDealerPlayerId { get; private set; }

		public float LocomotionSpeed
		{
			get
			{
				return (float)LocomotionSpeedQuantized * 0.1f;
			}
			private set
			{
				short num = (short)Mathf.Clamp(Mathf.RoundToInt(Mathf.Max(0f, value) * 10f), 0, 32767);
				if (LocomotionSpeedQuantized != num)
				{
					LocomotionSpeedQuantized = num;
				}
			}
		}

		public float MaxLocomotionSpeed => _locomotionSettings.MaxSpeed;

		public float SpeedAnimationLerpSpeed => _locomotionSettings.SpeedAnimationLerpSpeed;

		public float SpeedAnimationDeadzone => _locomotionSettings.SpeedAnimationDeadzone;

		public float RunBlendFootstepThreshold => _locomotionSettings.RunBlendFootstepThreshold;

		public float RemainingDistance => _agent.remainingDistance;

		public Vector3 Velocity => _agent.velocity;

		public Vector3 DesiredVelocity => _agent.desiredVelocity;

		public float AgentSpeed => _agent.speed;

		public bool IsStopped => _agent.isStopped;

		public NavMeshAgent NavMeshAgent => _agent;

		public bool HasPath => _agent.hasPath;

		public bool PathPending => _agent.pathPending;

		public NavMeshPathStatus PathStatus => _agent.pathStatus;

		public float StoppingDistance => _agent.stoppingDistance;

		public event Action OnAttackPreformed;

		public event Action<CoinRobBehaviour> OnDeactivated;

		public event Action<DamageData> OnMemberDamaged;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void Awake()
		{
			_damageable.OnDamaged += OnDamaged;
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_damageable.OnDamaged -= OnDamaged;
		}

		public override void Spawned()
		{
			base.Spawned();
			_fearHoleAbsorbVisualController?.ResetVisual();
			_movableAnimationFunctionReactor.OnFootstepPerformed += PlayFootstep;
			_itemHolder.OnReleased += OnItemHolderReleased;
			ApplyStateAuthorityToAgentAndHooks();
			ApplyMaxLocomotion();
			TryRestoreAttachedItemFromHolder();
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority || base.Runner == null || !base.Runner.IsSharedModeMasterClient)
			{
				return;
			}
			if (!_agent.enabled || !_agent.isOnNavMesh)
			{
				LocomotionSpeed = 0f;
				return;
			}
			UpdateDistanceScaledLocomotion();
			float num = (_agent.isStopped ? 0f : _agent.velocity.magnitude);
			if (num < _locomotionSettings.LocomotionSpeedDeadzone)
			{
				num = 0f;
			}
			LocomotionSpeed = num;
		}

		public override void Render()
		{
			if (_isSwarmKing)
			{
				SyncKingLocomotionAnimation();
			}
		}

		private void SyncKingLocomotionAnimation()
		{
			float maxLocomotionSpeed = MaxLocomotionSpeed;
			float speedAnimationDeadzone = SpeedAnimationDeadzone;
			float num = ((maxLocomotionSpeed > 0f) ? Mathf.Clamp01(LocomotionSpeed / maxLocomotionSpeed) : 0f);
			if (num < speedAnimationDeadzone)
			{
				num = 0f;
			}
			_kingAnimatorSpeed = Mathf.Lerp(_kingAnimatorSpeed, num, SpeedAnimationLerpSpeed * Time.deltaTime);
			if (num < speedAnimationDeadzone && _kingAnimatorSpeed < speedAnimationDeadzone)
			{
				_kingAnimatorSpeed = 0f;
			}
			_animator.Animator.SetFloat("Speed", _kingAnimatorSpeed);
		}

		public void StateAuthorityChanged()
		{
			ApplyStateAuthorityRestore();
		}

		private void ApplyStateAuthorityRestore()
		{
			ApplyStateAuthorityToAgentAndHooks();
			TryRestoreAttachedItemFromHolder();
		}

		private void ApplyStateAuthorityToAgentAndHooks()
		{
			bool flag = base.HasStateAuthority && base.Runner != null && base.Runner.IsSharedModeMasterClient;
			_agent.enabled = flag;
			if (flag)
			{
				if (!_agent.isOnNavMesh)
				{
					_agent.Warp(base.transform.position);
				}
				_damageableAnimationFunctionReactor.TryDealBaseAttackDamage -= InvokeTryDealBaseAttackAttackPerformed;
				_damageableAnimationFunctionReactor.TryDealBaseAttackDamage += InvokeTryDealBaseAttackAttackPerformed;
				_statHealthController.OnEnemyDead -= OnEnemyDead;
				_statHealthController.OnEnemyDead += OnEnemyDead;
			}
			else
			{
				_damageableAnimationFunctionReactor.TryDealBaseAttackDamage -= InvokeTryDealBaseAttackAttackPerformed;
				_statHealthController.OnEnemyDead -= OnEnemyDead;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_fearHoleAbsorbVisualController?.Kill(resetScale: false);
			_movableAnimationFunctionReactor.OnFootstepPerformed -= PlayFootstep;
			_itemHolder.OnReleased -= OnItemHolderReleased;
			_damageableAnimationFunctionReactor.TryDealBaseAttackDamage -= InvokeTryDealBaseAttackAttackPerformed;
			_statHealthController.OnEnemyDead -= OnEnemyDead;
		}

		public void SetDestination(Vector3 destination)
		{
			if (_agent.isOnNavMesh)
			{
				_agent.isStopped = false;
				_agent.SetDestination(destination);
				UpdateDistanceScaledLocomotion();
			}
		}

		public void StopMovement()
		{
			if (_agent.isOnNavMesh)
			{
				_agent.isStopped = true;
				_agent.ResetPath();
				_agent.velocity = Vector3.zero;
				ApplyMaxLocomotion();
			}
		}

		public void Teleport(Vector3 position)
		{
			_agent.Warp(position);
		}

		public bool TryGetNavMeshQueryFilter(out NavMeshQueryFilter queryFilter)
		{
			queryFilter = default(NavMeshQueryFilter);
			if (_agent == null)
			{
				return false;
			}
			queryFilter = new NavMeshQueryFilter
			{
				agentTypeID = _agent.agentTypeID,
				areaMask = ((_agent.areaMask != 0) ? _agent.areaMask : (-1))
			};
			return true;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2179981780u)]
		public void PlayFearHoleAbsorbVisualRpc([RpcPayload(4)] float duration, [RpcPayload(4)] float endScale, [RpcPayload(4)] int ease)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2179981780u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units.CoinRobBehaviour::PlayFearHoleAbsorbVisualRpc(System.Single,System.Single,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(duration, 4);
						writer.Write(endScale, 4);
						writer.Write(ease, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_fearHoleAbsorbVisualController?.Play(duration, endScale, ease);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1117371241u)]
		public void PlayFearHoleAbsorbVisualToTargetRpc([RpcPayload(4)] float duration, [RpcPayload(4)] float endScale, [RpcPayload(4)] int ease, [RpcPayload(12)] Vector3 worldTarget)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1117371241u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units.CoinRobBehaviour::PlayFearHoleAbsorbVisualToTargetRpc(System.Single,System.Single,System.Int32,UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(duration, 4);
						writer.Write(endScale, 4);
						writer.Write(ease, 4);
						writer.Write(worldTarget, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_fearHoleAbsorbVisualController?.Play(duration, endScale, ease, worldTarget);
		}

		public void AttachItem(IItem item)
		{
			if (item != null && !(item.NetworkObject == null) && base.HasStateAuthority && !HasAttachedItem && !_itemHolder.IsGrabbing && item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component) && _itemHolder.TryGrab(component))
			{
				_attachedItem = item;
			}
		}

		public bool TryRestoreAttachedItemFromHolder()
		{
			if (IsAttachedItemReferenceValid())
			{
				return true;
			}
			SimplePointGrabable currentGrabable = _itemHolder.CurrentGrabable;
			if (currentGrabable == null)
			{
				_attachedItem = null;
				return false;
			}
			if (!currentGrabable.TryGetComponent<IItem>(out var component) || component == null || component.IsDespawned)
			{
				_attachedItem = null;
				return false;
			}
			_attachedItem = component;
			return true;
		}

		public bool IsAttachedItemSettledAtCarryAnchor(float settleDistance)
		{
			if (!_itemHolder.IsGrabbing || _itemHolder.CarryAnchor == null || _itemHolder.GrabbedTransform == null)
			{
				return false;
			}
			return Vector3.Distance(_itemHolder.CarryAnchor.position, _itemHolder.GrabbedTransform.position) <= settleDistance;
		}

		public void ReleaseAttachedItem()
		{
			_itemHolder.ReleaseGrab();
		}

		public void SetAutoRotationActive(bool isActive)
		{
			_agent.updateRotation = isActive;
		}

		public void BindSwarmHost(NetworkId swarmHostNetworkId)
		{
			if (base.HasStateAuthority)
			{
				SwarmHostNetworkId = swarmHostNetworkId;
			}
		}

		private void OnItemHolderReleased(GrabReleaseReason _)
		{
			_attachedItem = null;
		}

		private bool TryGetAttachedItem(out IItem item)
		{
			if (TryRestoreAttachedItemFromHolder())
			{
				item = _attachedItem;
				return true;
			}
			item = null;
			return false;
		}

		private bool IsAttachedItemReferenceValid()
		{
			if (_attachedItem != null && !_attachedItem.IsDespawned && _attachedItem.NetworkObject != null)
			{
				return _attachedItem.NetworkObject.IsValid;
			}
			return false;
		}

		private void UpdateDistanceScaledLocomotion()
		{
			if (_locomotionSettings == null)
			{
				ApplyMaxLocomotion();
				return;
			}
			if (_agent.isStopped || (!_agent.hasPath && !_agent.pathPending))
			{
				ApplyMaxLocomotion();
				return;
			}
			float num = Mathf.Max(0.01f, _locomotionSettings.FullSpeedPathDistance);
			float t = Mathf.Clamp01(((_agent.pathPending || float.IsInfinity(_agent.remainingDistance)) ? num : Mathf.Max(0f, _agent.remainingDistance)) / num);
			float num2 = Mathf.Lerp(_locomotionSettings.MinSpeed, _locomotionSettings.MaxSpeed, t);
			float angularSpeed = Mathf.Lerp(_locomotionSettings.MinAngularSpeed, _locomotionSettings.MaxAngularSpeed, t);
			if (!_agent.pathPending && _agent.hasPath)
			{
				Vector3 to = _agent.steeringTarget - base.transform.position;
				to.y = 0f;
				if (to.sqrMagnitude > 0.0001f)
				{
					float num3 = Vector3.Angle(base.transform.forward, to);
					float num4 = 1f - _locomotionSettings.TurnSlowdownStrength * Mathf.Clamp01(num3 / 180f);
					num2 = Mathf.Max(_locomotionSettings.MinSpeed, num2 * num4);
				}
			}
			_agent.speed = num2;
			_agent.angularSpeed = angularSpeed;
		}

		private void ApplyMaxLocomotion()
		{
			if (!(_locomotionSettings == null))
			{
				_agent.speed = _locomotionSettings.MaxSpeed;
				_agent.angularSpeed = _locomotionSettings.MaxAngularSpeed;
			}
		}

		private void OnItemHolderReleased()
		{
			_attachedItem = null;
		}

		private void PlayFootstep()
		{
			if (_footstepsPoolReferences.Count != 0 && (!_isSwarmKing || CanPlayKingRunFootstep()))
			{
				_audioService.PlayOneShotAttached(_footstepsPoolReferences[UnityEngine.Random.Range(0, _footstepsPoolReferences.Count)], _soundSourceBehaviour);
			}
		}

		private bool CanPlayKingRunFootstep()
		{
			if (_animator == null || _animator.Animator == null)
			{
				return false;
			}
			return _animator.Animator.GetFloat("Speed") >= RunBlendFootstepThreshold;
		}

		private void InvokeTryDealBaseAttackAttackPerformed()
		{
			this.OnAttackPreformed?.Invoke();
		}

		private void OnDamaged(DamageData damageData)
		{
			if (damageData.DamageDealerPlayerID > 0)
			{
				LastDamageDealerPlayerId = damageData.DamageDealerPlayerID;
			}
			this.OnMemberDamaged?.Invoke(damageData);
		}

		private void OnEnemyDead()
		{
			_itemHolder.ReleaseGrab();
			this.OnDeactivated?.Invoke(this);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SwarmHostNetworkId = _SwarmHostNetworkId;
			LocomotionSpeedQuantized = _LocomotionSpeedQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SwarmHostNetworkId = SwarmHostNetworkId;
			_LocomotionSpeedQuantized = LocomotionSpeedQuantized;
		}

		[NetworkRpcWeavedInvoker(2179981780u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayFearHoleAbsorbVisualRpc_0040Invoker2179981780([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out int value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CoinRobBehaviour)context.TargetBehaviour).PlayFearHoleAbsorbVisualRpc(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(1117371241u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayFearHoleAbsorbVisualToTargetRpc_0040Invoker1117371241([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out int value3, 4);
			payloadReader.Read(out Vector3 value4, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CoinRobBehaviour)context.TargetBehaviour).PlayFearHoleAbsorbVisualToTargetRpc(value, value2, value3, value4);
		}
	}
}
