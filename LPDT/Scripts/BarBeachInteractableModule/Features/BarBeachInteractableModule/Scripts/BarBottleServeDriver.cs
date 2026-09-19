using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CustomSynchronizersModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(5)]
	public class BarBottleServeDriver : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private MonoItem _monoItem;

		private BarBeachInteractableConfiguration _configuration;

		private BarServePath _servePath;

		private Coroutine _slideRoutine;

		private Action<BarBottleServeDriver> _onSlotFreed;

		private float _slideElapsed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ServeSlotIndex", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ServeSlotIndex;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsSliding", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsSliding;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsOnCounter", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsOnCounter;

		[WeaverGenerated]
		[DefaultForProperty("HasInitializedServeState", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasInitializedServeState;

		[WeaverGenerated]
		[DefaultForProperty("SlideElapsedSeconds", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SlideElapsedSeconds;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int ServeSlotIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.ServeSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.ServeSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnServeStateRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe NetworkBool IsSliding
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.IsSliding. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.IsSliding. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnServeStateRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe NetworkBool IsOnCounter
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.IsOnCounter. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.IsOnCounter. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe NetworkBool HasInitializedServeState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.HasInitializedServeState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.HasInitializedServeState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe float SlideElapsedSeconds
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.SlideElapsedSeconds. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBottleServeDriver.SlideElapsedSeconds. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 4) = value;
			}
		}

		public bool OccupiesServeSlot
		{
			get
			{
				if (ServeSlotIndex >= 0)
				{
					if (!IsSliding)
					{
						return IsOnCounter;
					}
					return true;
				}
				return false;
			}
		}

		public void Configure(BarBeachInteractableConfiguration configuration, BarServePath servePath, Action<BarBottleServeDriver> onSlotFreed)
		{
			_configuration = configuration;
			_servePath = servePath;
			_onSlotFreed = onSlotFreed;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_grabable != null)
			{
				_grabable.LocalOnGrab += OnLocalGrabbed;
			}
			if (base.Object.HasStateAuthority && !HasInitializedServeState)
			{
				HasInitializedServeState = true;
				ServeSlotIndex = -1;
				IsSliding = false;
				IsOnCounter = false;
				SlideElapsedSeconds = 0f;
				ApplyBarPrice();
			}
			ApplyVisualStateFromNetwork();
		}

		private void OnServeStateRender()
		{
			ApplyVisualStateFromNetwork();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority && (bool)IsOnCounter && !IsSliding)
			{
				TryReleaseSlotIfLeftServePoint();
			}
		}

		public void StateAuthorityChanged()
		{
			EnsureSlideOrCounterStateAfterAuthorityChanged();
		}

		public void EnsureSlideOrCounterStateAfterAuthorityChanged()
		{
			if (base.Object.HasStateAuthority)
			{
				if ((bool)IsSliding && ServeSlotIndex >= 0)
				{
					ResumeSlideFromElapsed();
				}
				else if ((bool)IsOnCounter && ServeSlotIndex >= 0)
				{
					ApplyOnCounterPhysics();
				}
			}
		}

		private void ApplyBarPrice()
		{
			if (!(_monoItem == null))
			{
				_monoItem.ForceInitialize(new ItemData(ItemType.None, 1, 1));
				_monoItem.IsNeedToShowPrice = false;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Debug.Log($"[BarServe] bottle despawned sa={base.Object.HasStateAuthority} " + $"slotIdx={ServeSlotIndex} slide={IsSliding} counter={IsOnCounter}");
			if (_grabable != null)
			{
				_grabable.LocalOnGrab -= OnLocalGrabbed;
			}
			if (_slideRoutine != null)
			{
				StopCoroutine(_slideRoutine);
				_slideRoutine = null;
			}
			NotifySlotFreed();
			base.Despawned(runner, hasState);
		}

		public void StartSlide(int serveSlotIndex)
		{
			if (base.Object.HasStateAuthority && !(_configuration == null) && !(_servePath == null) && _servePath.TryGetServePoint(serveSlotIndex, out var _))
			{
				if (_slideRoutine != null)
				{
					StopCoroutine(_slideRoutine);
				}
				ServeSlotIndex = serveSlotIndex;
				IsOnCounter = false;
				IsSliding = true;
				SlideElapsedSeconds = 0f;
				_slideElapsed = 0f;
				SetGrabEnabled(enabled: false);
				Vector3 vector = ((_servePath.StartPoint != null) ? _servePath.StartPoint.position : base.transform.position);
				Quaternion quaternion = ((_servePath.StartPoint != null) ? _servePath.StartPoint.rotation : base.transform.rotation);
				if (_physicsSynchronizer != null)
				{
					_physicsSynchronizer.Teleport(vector, quaternion);
				}
				else
				{
					base.transform.SetPositionAndRotation(vector, quaternion);
				}
				if (_rigidbody != null)
				{
					_rigidbody.isKinematic = true;
					_rigidbody.useGravity = false;
					_rigidbody.detectCollisions = true;
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
				_slideRoutine = StartCoroutine(SlideRoutine(serveSlotIndex));
			}
		}

		private void ResumeSlideFromElapsed()
		{
			if (!(_configuration == null) && !(_servePath == null) && ServeSlotIndex >= 0)
			{
				if (_slideRoutine != null)
				{
					StopCoroutine(_slideRoutine);
				}
				_slideElapsed = Mathf.Max(0f, SlideElapsedSeconds);
				SetGrabEnabled(enabled: false);
				if (_rigidbody != null)
				{
					_rigidbody.isKinematic = true;
					_rigidbody.useGravity = false;
					_rigidbody.detectCollisions = true;
				}
				_slideRoutine = StartCoroutine(SlideRoutine(ServeSlotIndex));
			}
		}

		private void OnLocalGrabbed(int playerId)
		{
			Debug.Log($"[BarServe] bottle grabbed by={playerId} sa={base.Object.HasStateAuthority} " + $"slotIdx={ServeSlotIndex} slide={IsSliding} counter={IsOnCounter}");
			if (base.Object.HasStateAuthority)
			{
				ClearCounterReservation();
			}
			else
			{
				ClearCounterReservationRpc();
			}
			if (_rigidbody != null)
			{
				_rigidbody.useGravity = true;
				_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			}
			NotifySlotFreed();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2604632273u)]
		private void ClearCounterReservationRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2604632273u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BarBeachInteractableModule.Scripts.BarBottleServeDriver::ClearCounterReservationRpc()", invokeInfo, PlayerRef.None);
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
			ClearCounterReservation();
		}

		private void ClearCounterReservation()
		{
			if (base.Object.HasStateAuthority)
			{
				IsOnCounter = false;
				IsSliding = false;
				ServeSlotIndex = -1;
				SlideElapsedSeconds = 0f;
			}
		}

		private void TryReleaseSlotIfLeftServePoint()
		{
			if (!(_configuration == null) && !(_servePath == null) && ServeSlotIndex >= 0)
			{
				Vector3 servePosition = _servePath.GetServePosition(ServeSlotIndex);
				Vector3 position = base.transform.position;
				float slotLeaveRadius = _configuration.SlotLeaveRadius;
				Vector3 vector = position - servePosition;
				vector.y = 0f;
				bool num = slotLeaveRadius > 0f && vector.sqrMagnitude > slotLeaveRadius * slotLeaveRadius;
				float slotLeaveDropY = _configuration.SlotLeaveDropY;
				bool flag = slotLeaveDropY > 0f && position.y < servePosition.y - slotLeaveDropY;
				if (num || flag)
				{
					Debug.Log($"[BarServe] bottle left slot sa={base.Object.HasStateAuthority} " + $"slotIdx={ServeSlotIndex} planar={Mathf.Sqrt(vector.sqrMagnitude):F3} drop={servePosition.y - position.y:F3}");
					ClearCounterReservation();
					NotifySlotFreed();
				}
			}
		}

		private void NotifySlotFreed()
		{
			Action<BarBottleServeDriver> onSlotFreed = _onSlotFreed;
			_onSlotFreed = null;
			onSlotFreed?.Invoke(this);
		}

		private IEnumerator SlideRoutine(int serveSlotIndex)
		{
			float duration = Mathf.Max(0.05f, _configuration.SlideDuration);
			AnimationCurve progressCurve = _configuration.SlideProgressCurve;
			AnimationCurve heightCurve = _configuration.SlideHeightCurve;
			float height = _configuration.SlideHeight;
			float yawSpin = _configuration.SlideYawSpinDegrees;
			while (_slideElapsed < duration)
			{
				_slideElapsed += Time.fixedDeltaTime;
				if (base.Object.HasStateAuthority)
				{
					SlideElapsedSeconds = _slideElapsed;
				}
				float num = Mathf.Clamp01(_slideElapsed / duration);
				float num2 = ((progressCurve != null && progressCurve.keys.Length != 0) ? Mathf.Clamp01(progressCurve.Evaluate(num)) : num);
				Vector3 position = _servePath.EvaluatePosition(num2, serveSlotIndex);
				float num3 = ((heightCurve != null && heightCurve.keys.Length != 0) ? (heightCurve.Evaluate(num) * height) : 0f);
				position.y += num3;
				Quaternion rot = _servePath.EvaluateRotation(num2, num, yawSpin, serveSlotIndex);
				if (_rigidbody != null)
				{
					_rigidbody.MovePosition(position);
					_rigidbody.MoveRotation(rot);
				}
				yield return new WaitForFixedUpdate();
			}
			FinishSlideOnCounter(serveSlotIndex);
		}

		private void FinishSlideOnCounter(int serveSlotIndex)
		{
			Vector3 servePosition = _servePath.GetServePosition(serveSlotIndex);
			Quaternion serveRotation = _servePath.GetServeRotation(serveSlotIndex);
			if (_rigidbody != null)
			{
				_rigidbody.MovePosition(servePosition);
				_rigidbody.MoveRotation(serveRotation);
			}
			if (_physicsSynchronizer != null)
			{
				_physicsSynchronizer.Teleport(servePosition, serveRotation);
			}
			if (base.Object.HasStateAuthority)
			{
				IsSliding = false;
				IsOnCounter = true;
				SlideElapsedSeconds = 0f;
			}
			ApplyOnCounterPhysics();
			_slideRoutine = null;
		}

		private void ApplyOnCounterPhysics()
		{
			if (_rigidbody != null)
			{
				_rigidbody.linearVelocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
				_rigidbody.isKinematic = false;
				_rigidbody.useGravity = true;
				_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			}
			SetGrabEnabled(enabled: true);
		}

		private void ApplyVisualStateFromNetwork()
		{
			if ((bool)IsSliding)
			{
				SetGrabEnabled(enabled: false);
				if (_rigidbody != null)
				{
					_rigidbody.isKinematic = true;
					_rigidbody.useGravity = false;
				}
			}
			else if ((bool)IsOnCounter)
			{
				ApplyOnCounterPhysics();
			}
			else
			{
				SetGrabEnabled(enabled: true);
			}
		}

		private void SetGrabEnabled(bool enabled)
		{
			if (!(_grabable == null))
			{
				_grabable.GrabBlocked = !enabled;
				_grabable.LocalGrabBlocked = !enabled;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ServeSlotIndex = _ServeSlotIndex;
			IsSliding = _IsSliding;
			IsOnCounter = _IsOnCounter;
			HasInitializedServeState = _HasInitializedServeState;
			SlideElapsedSeconds = _SlideElapsedSeconds;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ServeSlotIndex = ServeSlotIndex;
			_IsSliding = IsSliding;
			_IsOnCounter = IsOnCounter;
			_HasInitializedServeState = HasInitializedServeState;
			_SlideElapsedSeconds = SlideElapsedSeconds;
		}

		[NetworkRpcWeavedInvoker(2604632273u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ClearCounterReservationRpc_0040Invoker2604632273([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BarBottleServeDriver)context.TargetBehaviour).ClearCounterReservationRpc();
		}
	}
}
