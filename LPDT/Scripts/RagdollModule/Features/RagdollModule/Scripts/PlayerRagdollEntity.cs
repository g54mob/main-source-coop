using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class PlayerRagdollEntity : PhysicsRagdollEntity
	{
		[SerializeField]
		private Transform _headTransform;

		[SerializeField]
		private Transform _cameraPositionTransform;

		[SerializeField]
		private Transform _cameraPositionWrapper;

		[SerializeField]
		private Transform _trackingPoint;

		[SerializeField]
		private float _cameraPositionResetDuration = 0.5f;

		[SerializeField]
		private float _voiceChangeDuration = 2f;

		private bool _simulateHandsRagdoll = true;

		private Transform _initialCameraPosTransform;

		private Coroutine _resetCoroutine;

		public bool BindCameraToHead { get; set; } = true;

		public Transform TrackingPoint => _trackingPoint;

		public bool SimulateHandsRagdoll
		{
			get
			{
				return _simulateHandsRagdoll;
			}
			private set
			{
				_simulateHandsRagdoll = value;
				this.OnSimulateHandsRagdollChanged?.Invoke(_simulateHandsRagdoll);
			}
		}

		public int PlayerId { get; set; }

		public float VoiceChangeDuration
		{
			get
			{
				return _voiceChangeDuration;
			}
			set
			{
				_voiceChangeDuration = value;
			}
		}

		public bool IsHandsPhysicsActive
		{
			get
			{
				if (base.IsSimulated)
				{
					return SimulateHandsRagdoll;
				}
				return false;
			}
		}

		public event Action<bool> OnSimulateHandsRagdollChanged;

		public void SetSimulateHandsRagdoll(bool simulate)
		{
			SetSimulateHandsRagdollRpc(simulate);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 887435226u)]
		private void SetSimulateHandsRagdollRpc([RpcPayload(4)] bool simulate)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(simulate);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(887435226u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.PlayerRagdollEntity::SetSimulateHandsRagdollRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(simulate);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SimulateHandsRagdoll = simulate;
		}

		protected override bool TryGetStandUpCapsule(out CapsuleCollider capsule)
		{
			capsule = _entityCollider as CapsuleCollider;
			return capsule != null;
		}

		protected override void EnableRagdoll()
		{
			base.EnableRagdoll();
			SwitchRagdollCameraBinding(active: true);
		}

		protected override void OnRagdollBlendOutStarted()
		{
			base.OnRagdollBlendOutStarted();
			SwitchRagdollCameraBinding(active: false);
		}

		private void SwitchRagdollCameraBinding(bool active)
		{
			if (!BindCameraToHead)
			{
				_cameraPositionTransform.SetParent(_cameraPositionWrapper);
				ResetLocalTransform(_cameraPositionTransform, _cameraPositionResetDuration);
			}
			else
			{
				_cameraPositionTransform.SetParent(active ? _headTransform : _cameraPositionWrapper);
				ResetLocalTransform(_cameraPositionTransform, _cameraPositionResetDuration);
			}
		}

		private void ResetLocalTransform(Transform targetTransform, float duration)
		{
			if (_resetCoroutine != null)
			{
				StopCoroutine(_resetCoroutine);
			}
			_resetCoroutine = StartCoroutine(SmoothResetCoroutine(targetTransform, duration));
		}

		private IEnumerator SmoothResetCoroutine(Transform targetTransform, float duration)
		{
			Vector3 startPosition = targetTransform.localPosition;
			Quaternion startRotation = targetTransform.localRotation;
			Vector3 startScale = targetTransform.localScale;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / duration);
				float t2 = Mathf.SmoothStep(0f, 1f, t);
				targetTransform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, t2);
				targetTransform.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, t2);
				targetTransform.localScale = Vector3.Lerp(startScale, Vector3.one, t2);
				yield return null;
			}
			targetTransform.localPosition = Vector3.zero;
			targetTransform.localRotation = Quaternion.identity;
			targetTransform.localScale = Vector3.one;
			_resetCoroutine = null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}

		[NetworkRpcWeavedInvoker(887435226u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetSimulateHandsRagdollRpc_0040Invoker887435226([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerRagdollEntity)context.TargetBehaviour).SetSimulateHandsRagdollRpc(value);
		}
	}
}
