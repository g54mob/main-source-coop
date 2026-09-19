using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class VisualController : NetworkBehaviour
	{
		[SerializeField]
		private Transform _visualsRoot;

		[SerializeField]
		private float _interpolationSpeed = 5f;

		[SerializeField]
		private Transform _cachedParent;

		private Transform _targetInterpolationPosition;

		private Vector3 _cachedPosition;

		private Vector3 _cachedRotation;

		public Transform VisualsRoot => _visualsRoot;

		public Transform TargetInterpolationPosition => _targetInterpolationPosition;

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2282261815u)]
		public void SetTargetPositionForInterpolation_RPC([RpcPayload(4)] NetworkId id)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2282261815u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.VisualController::SetTargetPositionForInterpolation_RPC(Fusion.NetworkId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(id, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!base.HasStateAuthority)
			{
				NetworkObject networkObject = base.Runner.FindObject(id);
				if (networkObject.transform == _cachedParent)
				{
					_targetInterpolationPosition = null;
					_visualsRoot.transform.SetParent(_cachedParent, worldPositionStays: true);
					_visualsRoot.transform.localPosition = _cachedPosition;
					_visualsRoot.transform.localEulerAngles = _cachedRotation;
				}
				else
				{
					_targetInterpolationPosition = networkObject.transform;
					_cachedPosition = _visualsRoot.localPosition;
					_cachedRotation = _visualsRoot.localEulerAngles;
					_visualsRoot.transform.SetParent(networkObject.transform, worldPositionStays: true);
					_visualsRoot.transform.localPosition = Vector3.zero;
					_visualsRoot.transform.rotation = networkObject.transform.rotation;
				}
			}
		}

		public void SetTargetPositionForInterpolation(Transform targetTransform)
		{
			base.Object.RequestStateAuthority();
			if (targetTransform == null)
			{
				SetTargetPositionForInterpolation_RPC(_cachedParent.GetComponent<NetworkObject>().Id);
				if (base.HasStateAuthority)
				{
					_targetInterpolationPosition = targetTransform;
					_visualsRoot.transform.SetParent(_cachedParent, worldPositionStays: true);
					_visualsRoot.transform.localPosition = _cachedPosition;
					_visualsRoot.transform.localEulerAngles = _cachedRotation;
				}
				return;
			}
			SetTargetPositionForInterpolation_RPC(targetTransform.GetComponent<NetworkObject>().Id);
			if (base.HasStateAuthority)
			{
				_targetInterpolationPosition = targetTransform;
				_cachedPosition = _visualsRoot.localPosition;
				_cachedRotation = _visualsRoot.localEulerAngles;
				_visualsRoot.transform.SetParent(targetTransform, worldPositionStays: true);
				_visualsRoot.transform.localPosition = Vector3.zero;
				_visualsRoot.transform.rotation = targetTransform.rotation;
			}
		}

		public override void Render()
		{
			base.Render();
			if (_targetInterpolationPosition != null)
			{
				if (_visualsRoot.parent != _targetInterpolationPosition)
				{
					_visualsRoot.transform.SetParent(_targetInterpolationPosition, worldPositionStays: true);
				}
				_visualsRoot.transform.localPosition = Vector3.zero;
				_visualsRoot.transform.rotation = _targetInterpolationPosition.rotation;
			}
			else if (_visualsRoot.parent != _cachedParent)
			{
				_visualsRoot.transform.SetParent(_cachedParent, worldPositionStays: true);
				_visualsRoot.transform.localPosition = _cachedPosition;
				_visualsRoot.transform.localEulerAngles = _cachedRotation;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2282261815u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTargetPositionForInterpolation_RPC_0040Invoker2282261815([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out NetworkId value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((VisualController)context.TargetBehaviour).SetTargetPositionForInterpolation_RPC(value);
		}
	}
}
