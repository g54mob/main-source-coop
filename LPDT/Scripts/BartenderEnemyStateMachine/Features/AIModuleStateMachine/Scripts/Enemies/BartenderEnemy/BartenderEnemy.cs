using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.States;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy
{
	[NetworkBehaviourWeaved(24)]
	public class BartenderEnemy : EnemyBase<BartenderStateId, BartenderEvent>
	{
		private const byte HandPhaseIdle = 0;

		private const byte HandPhaseReach = 1;

		private const byte HandPhaseReturn = 2;

		private const float HandReachDurationSeconds = 0.35f;

		private const float HandReturnDurationSeconds = 0.25f;

		[SerializeField]
		private Transform _counterLookTarget;

		[SerializeField]
		private Transform _handReachTransform;

		private BartenderEnemyContext _context;

		private BartenderAppearanceSettings _appearanceSettings;

		private bool _hasServeStock = true;

		private bool _isMusicPlaying;

		private IItem _activeCoin;

		private Action _onCoinConsumed;

		private Transform _activeCoinTransform;

		private byte _lastRenderedHandPhase;

		private float _localHandPhaseElapsed;

		[WeaverGenerated]
		[DefaultForProperty("HandReachPhase", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _HandReachPhase;

		[WeaverGenerated]
		[DefaultForProperty("HandPhaseStartTick", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _HandPhaseStartTick;

		[WeaverGenerated]
		[DefaultForProperty("HandPhaseDuration", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _HandPhaseDuration;

		[WeaverGenerated]
		[DefaultForProperty("HandStartWorldPosition", 3, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HandStartWorldPosition;

		[WeaverGenerated]
		[DefaultForProperty("HandStartWorldRotation", 6, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _HandStartWorldRotation;

		[WeaverGenerated]
		[DefaultForProperty("HandTargetWorldPosition", 10, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HandTargetWorldPosition;

		[WeaverGenerated]
		[DefaultForProperty("HandTargetWorldRotation", 13, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _HandTargetWorldRotation;

		[WeaverGenerated]
		[DefaultForProperty("HandRestLocalPosition", 17, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HandRestLocalPosition;

		[WeaverGenerated]
		[DefaultForProperty("HandRestLocalRotation", 20, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _HandRestLocalRotation;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe byte HandReachPhase
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandReachPhase. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[0];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandReachPhase. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[0] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int HandPhaseStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandPhaseStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandPhaseStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe float HandPhaseDuration
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandPhaseDuration. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandPhaseDuration. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 3)]
		private unsafe Vector3 HandStartWorldPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandStartWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandStartWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 4)]
		private unsafe Quaternion HandStartWorldRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandStartWorldRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandStartWorldRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(10, 3)]
		private unsafe Vector3 HandTargetWorldPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandTargetWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 10);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandTargetWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 10) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 4)]
		private unsafe Quaternion HandTargetWorldRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandTargetWorldRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 13);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandTargetWorldRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 13) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(17, 3)]
		private unsafe Vector3 HandRestLocalPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandRestLocalPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 17);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandRestLocalPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 17) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(20, 4)]
		private unsafe Quaternion HandRestLocalRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandRestLocalRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 20);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemy.HandRestLocalRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 20) = value;
			}
		}

		public override EnemyType EnemyType => EnemyType.Bartender;

		protected override bool UsesNavMeshAgent => false;

		public BartenderEnemyContext CastedContext => _context;

		public bool HasServeStock => _hasServeStock;

		public Transform HandReachTransform => _handReachTransform;

		public Transform ActiveCoinTransform => _activeCoinTransform;

		public Transform CounterLookTarget => _counterLookTarget;

		[Inject]
		public void InjectDependencies(BartenderEnemyContext context, IInstantiator instantiator, BartenderAppearanceSettings appearanceSettings)
		{
			_context = context;
			_appearanceSettings = appearanceSettings;
			InjectBaseDependencies(context, instantiator);
		}

		protected override StateMachine<BartenderStateId, BartenderStateId, BartenderEvent> BuildFsm()
		{
			StateMachine<BartenderStateId, BartenderStateId, BartenderEvent> stateMachine = new StateMachine<BartenderStateId, BartenderStateId, BartenderEvent>();
			stateMachine.AddState(BartenderStateId.Idle, Instantiator.Instantiate<BartenderIdleState>());
			stateMachine.AddState(BartenderStateId.React, Instantiator.Instantiate<BartenderReactState>());
			stateMachine.AddState(BartenderStateId.WaitingPayment, Instantiator.Instantiate<BartenderWaitingPaymentState>());
			stateMachine.AddState(BartenderStateId.TakingCoin, Instantiator.Instantiate<BartenderTakingCoinState>());
			stateMachine.AddTriggerTransition(BartenderEvent.OnBellGrabbed, BartenderStateId.Idle, BartenderStateId.React);
			stateMachine.AddTriggerTransition(BartenderEvent.OnBellGrabbed, BartenderStateId.WaitingPayment, BartenderStateId.React);
			stateMachine.AddTriggerTransition(BartenderEvent.OnReactFinished, BartenderStateId.React, BartenderStateId.Idle);
			stateMachine.AddTriggerTransition(BartenderEvent.OnStockEmpty, BartenderStateId.Idle, BartenderStateId.WaitingPayment);
			stateMachine.AddTriggerTransition(BartenderEvent.OnStockRestored, BartenderStateId.WaitingPayment, BartenderStateId.Idle);
			stateMachine.AddTriggerTransition(BartenderEvent.OnTakeCoinStarted, BartenderStateId.WaitingPayment, BartenderStateId.TakingCoin);
			stateMachine.AddTriggerTransition(BartenderEvent.OnTakeCoinFinished, BartenderStateId.TakingCoin, BartenderStateId.Idle);
			stateMachine.SetStartState(BartenderStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_context.NavMeshAgent != null)
			{
				_context.NavMeshAgent.enabled = false;
			}
			if (base.HasStateAuthority && _context.AppearanceSeed <= 0)
			{
				_context.NeedGenerateAppearance = true;
			}
			_context.AnimationSystem?.Enable();
			_context.ReplicateTargetSystem?.Enable();
			FaceCounterLookTarget();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_context.AnimationSystem?.Disable();
			_context.ReplicateTargetSystem?.Clear();
			_context.ReplicateTargetSystem?.Disable();
			_activeCoin = null;
			_onCoinConsumed = null;
			_activeCoinTransform = null;
			SnapHandToRestLocal();
			_lastRenderedHandPhase = 0;
			base.Despawned(runner, hasState);
		}

		public override void Render()
		{
			base.Render();
			if (_handReachTransform == null)
			{
				_lastRenderedHandPhase = HandReachPhase;
				return;
			}
			if (HandReachPhase != _lastRenderedHandPhase)
			{
				_localHandPhaseElapsed = 0f;
				_lastRenderedHandPhase = HandReachPhase;
				if (HandReachPhase == 0)
				{
					SnapHandToRestLocal();
					return;
				}
			}
			if (HandReachPhase != 0)
			{
				_localHandPhaseElapsed += Time.deltaTime;
				float num = Mathf.Max(0.01f, HandPhaseDuration);
				float num2 = Mathf.Clamp01(_localHandPhaseElapsed / num);
				float t = num2 * num2 * (3f - 2f * num2);
				_handReachTransform.position = Vector3.Lerp(HandStartWorldPosition, HandTargetWorldPosition, t);
				_handReachTransform.rotation = Quaternion.Slerp(HandStartWorldRotation, HandTargetWorldRotation, t);
			}
		}

		public void SetCounterLookTarget(Transform lookTarget)
		{
			if (lookTarget != null)
			{
				_counterLookTarget = lookTarget;
			}
			FaceCounterLookTarget();
		}

		public void SetHasServeStock(bool hasStock)
		{
			if (_hasServeStock == hasStock)
			{
				RefreshDancing();
				return;
			}
			_hasServeStock = hasStock;
			if (base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid)
			{
				if (!hasStock)
				{
					TriggerEvent(BartenderEvent.OnStockEmpty);
				}
				else
				{
					TriggerEvent(BartenderEvent.OnStockRestored);
				}
				RefreshDancing();
			}
		}

		public void SetMusicPlaying(bool isPlaying)
		{
			if (_isMusicPlaying == isPlaying)
			{
				RefreshDancing();
				return;
			}
			_isMusicPlaying = isPlaying;
			RefreshDancing();
		}

		public void RefreshDancing()
		{
			if (base.HasStateAuthority && !(_context == null) && !(base.Object == null) && base.Object.IsValid)
			{
				if (!_isMusicPlaying || !_hasServeStock || base.CurrentStateId != BartenderStateId.Idle)
				{
					_context.SetIsDancing(value: false);
					_context.SetActiveDanceIndex(-1);
				}
				else if (!_context.IsDancing || _context.ActiveDanceIndex < 0)
				{
					int num = PickDanceIndex();
					_context.SetActiveDanceIndex(num);
					_context.SetIsDancing(num >= 0);
				}
			}
		}

		private int PickDanceIndex()
		{
			IReadOnlyList<string> readOnlyList = ((_appearanceSettings != null) ? _appearanceSettings.DanceAnimatorTriggers : null);
			if (readOnlyList == null || readOnlyList.Count == 0)
			{
				return -1;
			}
			List<int> list = null;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				if (!string.IsNullOrWhiteSpace(readOnlyList[i]))
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(i);
				}
			}
			if (list == null || list.Count == 0)
			{
				return -1;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		public void FaceCounterLookTarget()
		{
			if (!(_counterLookTarget == null))
			{
				Vector3 vector = _counterLookTarget.position - base.transform.position;
				vector.y = 0f;
				if (!(vector.sqrMagnitude < 0.0001f))
				{
					base.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				}
			}
		}

		public void NotifyBellGrabbed()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (base.HasStateAuthority)
				{
					TriggerBellGrabbed();
				}
				else
				{
					NotifyBellGrabbedRpc();
				}
			}
		}

		public bool TryBeginTakeCoin(IItem coin, Action onCoinConsumed)
		{
			if (!base.HasStateAuthority || base.Object == null || !base.Object.IsValid)
			{
				return false;
			}
			if (coin == null || _hasServeStock)
			{
				return false;
			}
			if (base.CurrentStateId != BartenderStateId.WaitingPayment)
			{
				return false;
			}
			_activeCoin = coin;
			_onCoinConsumed = onCoinConsumed;
			_activeCoinTransform = ResolveCoinTransform(coin);
			TriggerEvent(BartenderEvent.OnTakeCoinStarted);
			return true;
		}

		public void ConsumeActiveCoin()
		{
			IItem activeCoin = _activeCoin;
			Action onCoinConsumed = _onCoinConsumed;
			_activeCoin = null;
			_onCoinConsumed = null;
			_activeCoinTransform = null;
			if (activeCoin != null)
			{
				try
				{
					activeCoin.Consume();
				}
				catch (Exception)
				{
				}
			}
			onCoinConsumed?.Invoke();
		}

		public bool TryBeginHandReachToActiveCoin()
		{
			if (!base.HasStateAuthority || _handReachTransform == null)
			{
				return false;
			}
			Vector3 vector = ((_activeCoinTransform != null) ? _activeCoinTransform.position : _handReachTransform.position);
			HandRestLocalPosition = _handReachTransform.localPosition;
			HandRestLocalRotation = _handReachTransform.localRotation;
			HandStartWorldPosition = _handReachTransform.position;
			HandStartWorldRotation = _handReachTransform.rotation;
			HandTargetWorldPosition = vector;
			Vector3 vector2 = vector - HandStartWorldPosition;
			HandTargetWorldRotation = ((vector2.sqrMagnitude > 0.0001f) ? Quaternion.LookRotation(vector2.normalized, Vector3.up) : HandStartWorldRotation);
			HandPhaseDuration = 0.35f;
			HandPhaseStartTick = base.Runner.Tick;
			HandReachPhase = 1;
			return true;
		}

		public void BeginHandReturn()
		{
			if (base.HasStateAuthority && !(_handReachTransform == null))
			{
				HandStartWorldPosition = _handReachTransform.position;
				HandStartWorldRotation = _handReachTransform.rotation;
				Vector3 handTargetWorldPosition = ((_handReachTransform.parent != null) ? _handReachTransform.parent.TransformPoint(HandRestLocalPosition) : HandRestLocalPosition);
				Quaternion handTargetWorldRotation = ((_handReachTransform.parent != null) ? (_handReachTransform.parent.rotation * HandRestLocalRotation) : HandRestLocalRotation);
				HandTargetWorldPosition = handTargetWorldPosition;
				HandTargetWorldRotation = handTargetWorldRotation;
				HandPhaseDuration = 0.25f;
				HandPhaseStartTick = base.Runner.Tick;
				HandReachPhase = 2;
			}
		}

		public bool IsHandReachPhaseFinished()
		{
			if (HandReachPhase == 0 || HandPhaseDuration <= 0f || base.Runner == null)
			{
				return true;
			}
			return (float)((int)base.Runner.Tick - HandPhaseStartTick) * base.Runner.DeltaTime >= HandPhaseDuration;
		}

		public void ClearHandReach()
		{
			if (base.HasStateAuthority)
			{
				HandReachPhase = 0;
			}
		}

		public void EnsureHomeStateForStock()
		{
			if (base.HasStateAuthority && !_hasServeStock && base.CurrentStateId == BartenderStateId.Idle)
			{
				TriggerEvent(BartenderEvent.OnStockEmpty);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 4098450803u)]
		private void NotifyBellGrabbedRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4098450803u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.BartenderEnemy::NotifyBellGrabbedRpc()", invokeInfo, PlayerRef.None);
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
			TriggerBellGrabbed();
		}

		private void TriggerBellGrabbed()
		{
			if (base.HasStateAuthority)
			{
				TriggerEvent(BartenderEvent.OnBellGrabbed);
			}
		}

		private void SnapHandToRestLocal()
		{
			if (!(_handReachTransform == null))
			{
				_handReachTransform.localPosition = HandRestLocalPosition;
				_handReachTransform.localRotation = HandRestLocalRotation;
			}
		}

		private static Transform ResolveCoinTransform(IItem coin)
		{
			if (coin is MonoBehaviour monoBehaviour)
			{
				return monoBehaviour.transform;
			}
			return null;
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			HandReachPhase = _HandReachPhase;
			HandPhaseStartTick = _HandPhaseStartTick;
			HandPhaseDuration = _HandPhaseDuration;
			HandStartWorldPosition = _HandStartWorldPosition;
			HandStartWorldRotation = _HandStartWorldRotation;
			HandTargetWorldPosition = _HandTargetWorldPosition;
			HandTargetWorldRotation = _HandTargetWorldRotation;
			HandRestLocalPosition = _HandRestLocalPosition;
			HandRestLocalRotation = _HandRestLocalRotation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_HandReachPhase = HandReachPhase;
			_HandPhaseStartTick = HandPhaseStartTick;
			_HandPhaseDuration = HandPhaseDuration;
			_HandStartWorldPosition = HandStartWorldPosition;
			_HandStartWorldRotation = HandStartWorldRotation;
			_HandTargetWorldPosition = HandTargetWorldPosition;
			_HandTargetWorldRotation = HandTargetWorldRotation;
			_HandRestLocalPosition = HandRestLocalPosition;
			_HandRestLocalRotation = HandRestLocalRotation;
		}

		[NetworkRpcWeavedInvoker(4098450803u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyBellGrabbedRpc_0040Invoker4098450803([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BartenderEnemy)context.TargetBehaviour).NotifyBellGrabbedRpc();
		}
	}
}
