using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.AnimationModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkedAnimationController : NetworkedAnimationControllerBase
	{
		[SerializeField]
		private Animator _animator;

		public override void PlayAnimation(AnimationType animationType)
		{
			PlayAnimationRpc(animationType);
		}

		public override void ResetAnimation(AnimationType animationType)
		{
			ResetAnimationRpc(animationType);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1962656439u)]
		private void ResetAnimationRpc([RpcPayload(4)] AnimationType animationType)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1962656439u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AnimationModule.Scripts.NetworkedAnimationController::ResetAnimationRpc(Features.AnimationModule.Scripts.AnimationType)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(animationType, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.ResetTrigger(animationType.ToString());
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1244994460u)]
		private void PlayAnimationRpc([RpcPayload(4)] AnimationType animationType)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1244994460u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AnimationModule.Scripts.NetworkedAnimationController::PlayAnimationRpc(Features.AnimationModule.Scripts.AnimationType)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(animationType, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.SetTrigger(animationType.ToString());
		}

		public override void PlayAnimationLocal(AnimationType animationType)
		{
			_animator.SetTrigger(animationType.ToString());
		}

		public override void ResetAnimationLocal(AnimationType animationType)
		{
			_animator.ResetTrigger(animationType.ToString());
		}

		public void SetTrigger(string triggerName)
		{
			SetTriggerRpc(triggerName);
		}

		public void SetTriggerLocal(string triggerName)
		{
			_animator.SetTrigger(triggerName);
		}

		public void ResetTrigger(string triggerName)
		{
			ResetTriggerRpc(triggerName);
		}

		public void ResetTriggerLocal(string triggerName)
		{
			_animator.ResetTrigger(triggerName);
		}

		public void SwapTrigger(string triggerToReset, string triggerToSet)
		{
			SwapTriggerRpc(triggerToReset, triggerToSet);
		}

		public void SwapTriggerLocal(string triggerToReset, string triggerToSet)
		{
			_animator.ResetTrigger(triggerToReset);
			_animator.SetTrigger(triggerToSet);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 209115351u)]
		private void SetTriggerRpc([RpcPayload] string triggerName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(triggerName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(209115351u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AnimationModule.Scripts.NetworkedAnimationController::SetTriggerRpc(System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(triggerName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.SetTrigger(triggerName);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 965134412u)]
		private void ResetTriggerRpc([RpcPayload] string triggerName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(triggerName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(965134412u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AnimationModule.Scripts.NetworkedAnimationController::ResetTriggerRpc(System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(triggerName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.ResetTrigger(triggerName);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3685263058u)]
		private void SwapTriggerRpc([RpcPayload] string triggerToReset, [RpcPayload] string triggerToSet)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(triggerToReset);
				payloadSize += Fusion.RpcDataWriter.GetPayloadSize(triggerToSet);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3685263058u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AnimationModule.Scripts.NetworkedAnimationController::SwapTriggerRpc(System.String,System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(triggerToReset);
						writer.Write(triggerToSet);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.ResetTrigger(triggerToReset);
			_animator.SetTrigger(triggerToSet);
		}

		public override void SetFloat(string parameter, float value)
		{
			_animator.SetFloat(parameter, value);
		}

		public override void SetFloat(int parameter, float value)
		{
			_animator.SetFloat(parameter, value);
		}

		public override float GetFloat(string parameter)
		{
			return _animator.GetFloat(parameter);
		}

		public override void SetBool(string parameter, bool value)
		{
			_animator.SetBool(parameter, value);
		}

		public override void SetBool(int parameter, bool value)
		{
			_animator.SetBool(parameter, value);
		}

		public override AnimatorStateInfo GetCurrentAnimatorStateInfo(string layerName)
		{
			int layerIndex = _animator.GetLayerIndex(layerName);
			return _animator.GetCurrentAnimatorStateInfo(layerIndex);
		}

		public override AnimatorStateInfo GetNextAnimatorStateInfo(string layerName)
		{
			int layerIndex = _animator.GetLayerIndex(layerName);
			return _animator.GetNextAnimatorStateInfo(layerIndex);
		}

		public override bool IsInTransition(string attack)
		{
			int layerIndex = _animator.GetLayerIndex(attack);
			return _animator.IsInTransition(layerIndex);
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

		[NetworkRpcWeavedInvoker(1962656439u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ResetAnimationRpc_0040Invoker1962656439([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out AnimationType value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedAnimationController)context.TargetBehaviour).ResetAnimationRpc(value);
		}

		[NetworkRpcWeavedInvoker(1244994460u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAnimationRpc_0040Invoker1244994460([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out AnimationType value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedAnimationController)context.TargetBehaviour).PlayAnimationRpc(value);
		}

		[NetworkRpcWeavedInvoker(209115351u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTriggerRpc_0040Invoker209115351([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedAnimationController)context.TargetBehaviour).SetTriggerRpc(value);
		}

		[NetworkRpcWeavedInvoker(965134412u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ResetTriggerRpc_0040Invoker965134412([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedAnimationController)context.TargetBehaviour).ResetTriggerRpc(value);
		}

		[NetworkRpcWeavedInvoker(3685263058u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SwapTriggerRpc_0040Invoker3685263058([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out string value2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedAnimationController)context.TargetBehaviour).SwapTriggerRpc(value, value2);
		}
	}
}
