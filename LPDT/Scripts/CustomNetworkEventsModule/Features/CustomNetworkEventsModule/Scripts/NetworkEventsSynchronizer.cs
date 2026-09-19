using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CustomNetworkEventsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkEventsSynchronizer : NetworkBehaviour
	{
		private readonly Dictionary<string, ICustomNetworkEvent> _synchronizableEvents = new Dictionary<string, ICustomNetworkEvent>();

		[Inject]
		public void InjectDependencies(List<ICustomNetworkEvent> synchronizableEvents)
		{
			foreach (ICustomNetworkEvent synchronizableEvent in synchronizableEvents)
			{
				_synchronizableEvents.Add(synchronizableEvent.GetType().ToString(), synchronizableEvent);
			}
		}

		public override void Spawned()
		{
			foreach (ICustomNetworkEvent value in _synchronizableEvents.Values)
			{
				value.OnInternalNetworkEventSend += InternalNetworkEventSend;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			foreach (ICustomNetworkEvent value in _synchronizableEvents.Values)
			{
				value.OnInternalNetworkEventSend -= InternalNetworkEventSend;
			}
		}

		private void InternalNetworkEventSend(ICustomNetworkEvent customNetwork)
		{
			string text = customNetwork.GetType().ToString();
			byte[] value = customNetwork.GetValue();
			if (base.HasInputAuthority)
			{
				FromInputAuthorityToAllRPC(text, value);
			}
			else if (base.HasStateAuthority)
			{
				FromStateAuthorityToAllRPC(text, value);
			}
			else if (base.Object.InputAuthority == base.Runner.LocalPlayer)
			{
				Debug.LogWarning("[NetworkEvents] Dropped " + text + ": local synchronizer has no input/state authority");
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.All, Key = 3497898917u)]
		private void FromInputAuthorityToAllRPC([RpcPayload] string synchronizableType, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableType);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3497898917u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CustomNetworkEventsModule.Scripts.NetworkEventsSynchronizer::FromInputAuthorityToAllRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableType);
						writer.Write(value, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_synchronizableEvents[synchronizableType].SynchronizeInternal(value);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1589247780u)]
		private void FromStateAuthorityToAllRPC([RpcPayload] string synchronizableType, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableType);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1589247780u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CustomNetworkEventsModule.Scripts.NetworkEventsSynchronizer::FromStateAuthorityToAllRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableType);
						writer.Write(value, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_synchronizableEvents[synchronizableType].SynchronizeInternal(value);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3497898917u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToAllRPC_0040Invoker3497898917([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkEventsSynchronizer)context.TargetBehaviour).FromInputAuthorityToAllRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(1589247780u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToAllRPC_0040Invoker1589247780([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((NetworkEventsSynchronizer)context.TargetBehaviour).FromStateAuthorityToAllRPC(value, value2);
		}
	}
}
