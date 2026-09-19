using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.CollectingModule.Scripts.New
{
	[RequireComponent(typeof(Rigidbody))]
	[NetworkBehaviourWeaved(4)]
	public class PhysicsItemUpVectorLimiter : NetworkBehaviour
	{
		private const float FLIP_ANGLE_THRESHOLD = 90f;

		[Header("Up Vector Alignment")]
		[Tooltip("How strongly the item aligns its UP vector to world up")]
		[Range(0.1f, 100f)]
		public float CorrectionStrength = 5f;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private CartItemsGrabber _cartItemsGrabber;

		[SerializeField]
		private float _lerpSpeed = 10f;

		[SerializeField]
		private float _flipSpeed = 2f;

		[SerializeField]
		private bool _disableYVelocity = true;

		private Rigidbody _rb;

		[WeaverGenerated]
		[DefaultForProperty("CurrentUpVectorInternal", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _CurrentUpVectorInternal = Vector3.up;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DisableCorrection", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _DisableCorrection;

		private float _alignSpeed;

		private bool _isSpawned;

		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3 CurrentUpVectorInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsItemUpVectorLimiter.CurrentUpVectorInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsItemUpVectorLimiter.CurrentUpVectorInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(3, 1)]
		public unsafe bool DisableCorrection
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsItemUpVectorLimiter.DisableCorrection. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsItemUpVectorLimiter.DisableCorrection. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		public Vector3 CurrentUpVector
		{
			get
			{
				if (!_isSpawned)
				{
					return Vector3.zero;
				}
				return CurrentUpVectorInternal;
			}
			set
			{
				CurrentUpVectorInternal = value;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_isSpawned = false;
		}

		private void Awake()
		{
			_rb = _simplePointGrabable.Rigidbody;
		}

		public void FixedUpdate()
		{
			if (_rb == null || _simplePointGrabable == null || _simplePointGrabable.GrabbedByPlayers.Count == 0 || DisableCorrection)
			{
				return;
			}
			if (_cartItemsGrabber != null)
			{
				foreach (IPointGrabable item in _cartItemsGrabber.Items)
				{
					if (item != null && !(item.NetworkObject == null) && !_cartItemsGrabber.ParentGrabbers.ContainsKey(item) && item.NetworkObject.StateAuthority != base.Object.StateAuthority)
					{
						return;
					}
				}
			}
			ApplyUpVectorCorrection();
		}

		private void ApplyUpVectorCorrection()
		{
			Vector3 up = base.transform.up;
			Vector3 currentUpVector = CurrentUpVector;
			if (Vector3.Angle(up, currentUpVector) < 0.5f)
			{
				return;
			}
			Quaternion.FromToRotation(up, currentUpVector).ToAngleAxis(out var angle, out var axis);
			_alignSpeed = ((angle > 90f) ? _flipSpeed : _lerpSpeed);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (angle != 0f && !float.IsNaN(axis.x))
			{
				Vector3 b = axis.normalized * (angle * (MathF.PI / 180f) * CorrectionStrength);
				if (_disableYVelocity)
				{
					b.y = 0f;
				}
				_rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, b, Time.fixedDeltaTime * _alignSpeed);
			}
		}

		public void SetTargetUpVector(Vector3 upVector)
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				SetTargetUpVectorRpc(upVector);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4143174872u)]
		private void SetTargetUpVectorRpc([RpcPayload(12)] Vector3Compressed upVector)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4143174872u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.New.PhysicsItemUpVectorLimiter::SetTargetUpVectorRpc(Fusion.Vector3Compressed)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(upVector, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CurrentUpVector = upVector;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentUpVectorInternal = _CurrentUpVectorInternal;
			DisableCorrection = _DisableCorrection;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentUpVectorInternal = CurrentUpVectorInternal;
			_DisableCorrection = DisableCorrection;
		}

		[NetworkRpcWeavedInvoker(4143174872u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTargetUpVectorRpc_0040Invoker4143174872([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3Compressed value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PhysicsItemUpVectorLimiter)context.TargetBehaviour).SetTargetUpVectorRpc(value);
		}
	}
}
