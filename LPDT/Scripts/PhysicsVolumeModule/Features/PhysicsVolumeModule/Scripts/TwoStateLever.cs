using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.PhysicsVolumeModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class TwoStateLever : NetworkBehaviour, IVolumeUncapturable
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Vector3 _hingeAxis = Vector3.forward;

		[SerializeField]
		private float _downAngle = -30f;

		[SerializeField]
		private float _upAngle = 30f;

		[SerializeField]
		private float _snapDegreesPerSecond = 240f;

		[SerializeField]
		private LeverState _initialState = LeverState.Down;

		[WeaverGenerated]
		[DefaultForProperty("Angle", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Angle;

		[WeaverGenerated]
		[DefaultForProperty("State", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LeverState _State;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PullCount", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PullCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsPullBlocked", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsPullBlocked;

		private LeverState _reportedState;

		private bool _isMomentary;

		private bool _requestedBlock;

		private bool _hasRequestedBlock;

		private Quaternion _baseLocalRotation;

		private LeverState _requestedState;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe float Angle
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.Angle. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.Angle. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe LeverState State
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.State. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (LeverState)Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.State. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe int PullCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.PullCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.PullCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe bool IsPullBlocked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.IsPullBlocked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TwoStateLever.IsPullBlocked. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		public LeverState CurrentState => State;

		public LeverState RestState => _initialState;

		public event Action<LeverState> OnStateChanged;

		public override void Spawned()
		{
			_baseLocalRotation = base.transform.localRotation * Quaternion.Inverse(Quaternion.AngleAxis(TargetAngle(_initialState), _hingeAxis.normalized));
			if (base.HasStateAuthority && State == LeverState.None)
			{
				State = _initialState;
				Angle = TargetAngle(_initialState);
			}
			ApplyAngle();
			ReportStateIfChanged();
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			bool flag = !IsPullBlocked && _grabable.GrabObject.Grabbers.Count > 0;
			_rigidbody.isKinematic = !flag;
			if (IsPullBlocked)
			{
				CommitState(_initialState);
				Angle = Mathf.MoveTowards(Angle, TargetAngle(_initialState), _snapDegreesPerSecond * base.Runner.DeltaTime);
				ApplyAngle();
				ReportStateIfChanged();
				return;
			}
			if (flag)
			{
				Angle = MeasuredAngle();
				float num = (_downAngle + _upAngle) * 0.5f;
				if (Angle > num)
				{
					CommitState(LeverState.Up);
				}
				else if (Angle < num)
				{
					CommitState(LeverState.Down);
				}
				if (_isMomentary && State != _initialState)
				{
					CommitState(_initialState);
					Angle = TargetAngle(_initialState);
					ApplyAngle();
				}
			}
			else
			{
				Angle = Mathf.MoveTowards(Angle, TargetAngle(State), _snapDegreesPerSecond * base.Runner.DeltaTime);
				ApplyAngle();
			}
			ReportStateIfChanged();
		}

		public override void Render()
		{
			if (!base.HasStateAuthority)
			{
				ApplyAngle();
			}
			ReportStateIfChanged();
		}

		public void SetState(LeverState state)
		{
			if (!base.HasStateAuthority)
			{
				if (base.Object != null && base.Object.IsValid && _requestedState != state)
				{
					_requestedState = state;
					SetStateRpc(state);
				}
				return;
			}
			_requestedState = state;
			if (!IsPullBlocked || state == _initialState)
			{
				CommitState(state);
				Angle = TargetAngle(state);
				ApplyAngle();
				ReportStateIfChanged();
			}
		}

		public void MakeMomentary()
		{
			_isMomentary = true;
		}

		public void SetPullBlocked(bool blocked)
		{
			if (!base.HasStateAuthority)
			{
				if (base.Object != null && base.Object.IsValid && (!_hasRequestedBlock || _requestedBlock != blocked))
				{
					_hasRequestedBlock = true;
					_requestedBlock = blocked;
					SetPullBlockedRpc(blocked);
				}
			}
			else
			{
				_hasRequestedBlock = true;
				_requestedBlock = blocked;
				if (IsPullBlocked != blocked)
				{
					IsPullBlocked = blocked;
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3768334378u)]
		private void SetPullBlockedRpc([RpcPayload(4)] bool blocked)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(blocked);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3768334378u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PhysicsVolumeModule.Scripts.TwoStateLever::SetPullBlockedRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(blocked);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SetPullBlocked(blocked);
		}

		private void CommitState(LeverState state)
		{
			if (State != state)
			{
				State = state;
				if (state != _initialState)
				{
					PullCount++;
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2992702486u)]
		private void SetStateRpc([RpcPayload(4)] LeverState state)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2992702486u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PhysicsVolumeModule.Scripts.TwoStateLever::SetStateRpc(Features.PhysicsVolumeModule.Scripts.LeverState)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(state, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SetState(state);
		}

		private float TargetAngle(LeverState state)
		{
			if (state != LeverState.Up)
			{
				return _downAngle;
			}
			return _upAngle;
		}

		private float MeasuredAngle()
		{
			(Quaternion.Inverse(_baseLocalRotation) * base.transform.localRotation).ToAngleAxis(out var angle, out var axis);
			return angle * Mathf.Sign(Vector3.Dot(axis, _hingeAxis));
		}

		private void ApplyAngle()
		{
			base.transform.localRotation = _baseLocalRotation * Quaternion.AngleAxis(Angle, _hingeAxis.normalized);
		}

		private void ReportStateIfChanged()
		{
			if (State != _reportedState)
			{
				_reportedState = State;
				this.OnStateChanged?.Invoke(State);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Angle = _Angle;
			State = _State;
			PullCount = _PullCount;
			IsPullBlocked = _IsPullBlocked;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Angle = Angle;
			_State = State;
			_PullCount = PullCount;
			_IsPullBlocked = IsPullBlocked;
		}

		[NetworkRpcWeavedInvoker(3768334378u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetPullBlockedRpc_0040Invoker3768334378([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((TwoStateLever)context.TargetBehaviour).SetPullBlockedRpc(value);
		}

		[NetworkRpcWeavedInvoker(2992702486u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetStateRpc_0040Invoker2992702486([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out LeverState value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((TwoStateLever)context.TargetBehaviour).SetStateRpc(value);
		}
	}
}
