using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkedCompositeAnimator : NetworkBehaviour
	{
		[field: SerializeField]
		public CompositeAnimator CompositeAnimator { get; private set; }

		public void SetBool(int boolName, bool boolValue)
		{
			SetBoolRPC(boolName, boolValue);
		}

		public void SetTrigger(int triggerName)
		{
			SetTriggerRPC(triggerName);
		}

		public void Play(int stateNameHash, int layer, float normalizedTime)
		{
			PlayRpc(stateNameHash, layer, normalizedTime);
		}

		public void SetLayerWeight(int layer, float value)
		{
			SetLayerWeightRpc(layer, value);
		}

		public void SetLayerWeight(string layerName, float value)
		{
			SetLayerWeightRpc(CompositeAnimator.GetLayerIndex(layerName), value);
		}

		public void ResetTrigger(int id)
		{
			ResetTriggerRpc(id);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2058594334u)]
		private void SetBoolRPC([RpcPayload(4)] int boolName, [RpcPayload(4)] bool boolValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetPayloadSize(boolValue);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2058594334u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.NetworkedCompositeAnimator::SetBoolRPC(System.Int32,System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(boolName, 4);
						writer.Write(boolValue);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CompositeAnimator.SetBool(boolName, boolValue);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 490485831u)]
		private void SetTriggerRPC([RpcPayload(4)] int triggerName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(490485831u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.NetworkedCompositeAnimator::SetTriggerRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(triggerName, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CompositeAnimator.SetTrigger(triggerName);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1099293877u)]
		private void PlayRpc([RpcPayload(4)] int stateNameHash, [RpcPayload(4)] int layer, [RpcPayload(4)] float normalizedTime)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1099293877u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.NetworkedCompositeAnimator::PlayRpc(System.Int32,System.Int32,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(stateNameHash, 4);
						writer.Write(layer, 4);
						writer.Write(normalizedTime, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CompositeAnimator.Play(stateNameHash, layer, normalizedTime);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1984513333u)]
		private void SetLayerWeightRpc([RpcPayload(4)] int layer, [RpcPayload(4)] float value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1984513333u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.NetworkedCompositeAnimator::SetLayerWeightRpc(System.Int32,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(layer, 4);
						writer.Write(value, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CompositeAnimator.SetLayerWeight(layer, value);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1154434340u)]
		private void ResetTriggerRpc([RpcPayload(4)] int id)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1154434340u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.NetworkedCompositeAnimator::ResetTriggerRpc(System.Int32)", invokeInfo, PlayerRef.None);
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
			CompositeAnimator.ResetTrigger(id);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2058594334u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetBoolRPC_0040Invoker2058594334([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out bool value2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedCompositeAnimator)context.TargetBehaviour).SetBoolRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(490485831u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTriggerRPC_0040Invoker490485831([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedCompositeAnimator)context.TargetBehaviour).SetTriggerRPC(value);
		}

		[NetworkRpcWeavedInvoker(1099293877u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayRpc_0040Invoker1099293877([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out int value2, 4);
			payloadReader.Read(out float value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedCompositeAnimator)context.TargetBehaviour).PlayRpc(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(1984513333u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetLayerWeightRpc_0040Invoker1984513333([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedCompositeAnimator)context.TargetBehaviour).SetLayerWeightRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(1154434340u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ResetTriggerRpc_0040Invoker1154434340([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkedCompositeAnimator)context.TargetBehaviour).ResetTriggerRpc(value);
		}
	}
}
