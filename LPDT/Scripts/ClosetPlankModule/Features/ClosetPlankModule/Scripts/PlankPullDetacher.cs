using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.ClosetPlankModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class PlankPullDetacher : NetworkBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private GrabObjectBase _grabObject;

		[SerializeField]
		private float _extraPullToDetach = 0.9f;

		[SerializeField]
		private float _detachImpulsePerMass = 5f;

		[SerializeField]
		private float _detachUpwardBias = 0.2f;

		[WeaverGenerated]
		[DefaultForProperty("IsDetached", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsDetached;

		private bool _detachApplied;

		private float _pullBaseline;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsDetached
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlankPullDetacher.IsDetached. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlankPullDetacher.IsDetached. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public override void Spawned()
		{
			if (IsDetached)
			{
				ApplyDetachedState();
			}
			else
			{
				ApplyAttachedState();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (IsDetached || !base.HasStateAuthority)
			{
				return;
			}
			float distance;
			Vector3 direction;
			if (_grabObject.Grabbers.Count == 0)
			{
				_pullBaseline = 0f;
			}
			else if (TryGetStrongestPull(out distance, out direction))
			{
				if (_pullBaseline <= 0f)
				{
					_pullBaseline = distance;
				}
				else if (distance - _pullBaseline >= _extraPullToDetach)
				{
					Detach();
				}
			}
		}

		private bool TryGetStrongestPull(out float distance, out Vector3 direction)
		{
			distance = 0f;
			direction = Vector3.zero;
			foreach (PhysGrabber grabber in _grabObject.Grabbers)
			{
				if (grabber.physGrabPoints.TryGetValue(_grabObject, out var value) && !(value == null))
				{
					Vector3 vector = grabber.physGrabPointPullerPosition.position - value.position;
					float magnitude = vector.magnitude;
					if (!(magnitude <= distance))
					{
						distance = magnitude;
						direction = vector / magnitude;
					}
				}
			}
			return distance > 0f;
		}

		private void Detach()
		{
			TryGetStrongestPull(out var _, out var direction);
			IsDetached = true;
			DetachRpc(direction);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3757617705u)]
		private void DetachRpc([RpcPayload(12)] Vector3 pullDirection)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3757617705u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ClosetPlankModule.Scripts.PlankPullDetacher::DetachRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(pullDirection, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyDetachedState();
			if (base.HasStateAuthority)
			{
				if (pullDirection.sqrMagnitude < 0.0001f)
				{
					pullDirection = base.transform.forward;
				}
				Vector3 normalized = (pullDirection + Vector3.up * _detachUpwardBias).normalized;
				_rigidbody.AddForce(normalized * (_rigidbody.mass * _detachImpulsePerMass), ForceMode.Impulse);
			}
		}

		private void ApplyAttachedState()
		{
			_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			_grabObject.GrabbingPhysicsBlocked = true;
		}

		private void ApplyDetachedState()
		{
			if (!_detachApplied)
			{
				_detachApplied = true;
				_rigidbody.constraints = RigidbodyConstraints.None;
				_grabObject.GrabbingPhysicsBlocked = false;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsDetached = _IsDetached;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsDetached = IsDetached;
		}

		[NetworkRpcWeavedInvoker(3757617705u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DetachRpc_0040Invoker3757617705([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlankPullDetacher)context.TargetBehaviour).DetachRpc(value);
		}
	}
}
