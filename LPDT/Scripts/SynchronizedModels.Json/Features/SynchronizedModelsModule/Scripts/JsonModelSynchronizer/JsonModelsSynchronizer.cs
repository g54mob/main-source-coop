using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	[NetworkBehaviourWeaved(0)]
	public class JsonModelsSynchronizer : NetworkBehaviour
	{
		private readonly Dictionary<string, IJsonSynchronizable> _synchronizables = new Dictionary<string, IJsonSynchronizable>();

		private readonly Dictionary<string, ICustomJsonSynchronizable> _customSynchronizables = new Dictionary<string, ICustomJsonSynchronizable>();

		[Inject]
		public void InjectDependencies(List<IJsonSynchronizable> synchronizables, List<ICustomJsonSynchronizable> customSynchronizables)
		{
			foreach (IJsonSynchronizable synchronizable in synchronizables)
			{
				_synchronizables.TryAdd(synchronizable.GetType().Name, synchronizable);
			}
			foreach (ICustomJsonSynchronizable customSynchronizable in customSynchronizables)
			{
				_customSynchronizables.TryAdd(customSynchronizable.GetType().Name, customSynchronizable);
			}
		}

		public override void Spawned()
		{
			if (_synchronizables.Count == 0 && _customSynchronizables.Count == 0)
			{
				return;
			}
			foreach (IJsonSynchronizable value in _synchronizables.Values)
			{
				value.OnSpawned();
				value.CallSynchronize += Synchronize;
			}
			foreach (ICustomJsonSynchronizable value2 in _customSynchronizables.Values)
			{
				value2.OnSpawned();
				value2.OnCallCustomSynchronize += CustomSynchronize;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			foreach (IJsonSynchronizable value in _synchronizables.Values)
			{
				value.OnDespawned();
				value.CallSynchronize -= Synchronize;
			}
			foreach (ICustomJsonSynchronizable value2 in _customSynchronizables.Values)
			{
				value2.OnDespawned();
				value2.OnCallCustomSynchronize -= CustomSynchronize;
			}
		}

		private void Synchronize(IJsonSynchronizable jsonSynchronizable)
		{
			if (base.Object.InputAuthority != base.Runner.LocalPlayer)
			{
				return;
			}
			string synchronizableTypeName = jsonSynchronizable.GetType().Name;
			if (jsonSynchronizable.RPCType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				if (jsonSynchronizable.TryGetValue(out var value))
				{
					FromStateAuthorityToAllRPC(synchronizableTypeName, value);
				}
				else
				{
					FromStateAuthorityToAllRPC(synchronizableTypeName);
				}
			}
			if (jsonSynchronizable.RPCType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				if (jsonSynchronizable.TryGetValue(out var value2))
				{
					FromInputAuthorityToStateAuthorityRPC(synchronizableTypeName, value2);
				}
				else
				{
					FromInputAuthorityToStateAuthorityRPC(synchronizableTypeName);
				}
			}
			if (jsonSynchronizable.RPCType != RPCType.InAllWays)
			{
				return;
			}
			if (base.HasInputAuthority)
			{
				if (jsonSynchronizable.TryGetValue(out var value3))
				{
					FromInputAuthorityToAllRPC(synchronizableTypeName, value3);
				}
				else
				{
					FromInputAuthorityToAllRPC(synchronizableTypeName);
				}
			}
			else if (base.HasStateAuthority)
			{
				if (jsonSynchronizable.TryGetValue(out var value4))
				{
					FromStateAuthorityToAllRPC(synchronizableTypeName, value4);
				}
				else
				{
					FromStateAuthorityToAllRPC(synchronizableTypeName);
				}
			}
		}

		private void CustomSynchronize(ICustomJsonSynchronizable customSynchronizable)
		{
			if (base.Object.InputAuthority != base.Runner.LocalPlayer)
			{
				return;
			}
			string synchronizableTypeName = customSynchronizable.GetType().Name;
			byte[] customValue = customSynchronizable.GetCustomValue();
			if (customSynchronizable.RPCType == RPCType.FromStateAuthorityToAll)
			{
				if (base.HasStateAuthority)
				{
					FromStateAuthorityToAllCustomDataRPC(synchronizableTypeName, customValue);
				}
			}
			else if (customSynchronizable.RPCType == RPCType.FromInputAuthorityToStateAuthority)
			{
				if (base.HasInputAuthority)
				{
					FromInputAuthorityToStateAuthorityCustomDataRPC(synchronizableTypeName, customValue);
				}
			}
			else if (customSynchronizable.RPCType == RPCType.InAllWays)
			{
				if (base.HasInputAuthority)
				{
					FromInputAuthorityToAllCustomDataRPC(synchronizableTypeName, customValue);
				}
				else if (base.HasStateAuthority)
				{
					FromStateAuthorityToAllCustomDataRPC(synchronizableTypeName, customValue);
				}
			}
		}

		private bool TryGetSynchronizable(string synchronizableTypeName, out IJsonSynchronizable synchronizable)
		{
			if (string.IsNullOrEmpty(synchronizableTypeName) || !_synchronizables.TryGetValue(synchronizableTypeName, out synchronizable))
			{
				Debug.LogWarning("[JsonModelsSynchronizer] Unknown synchronizable type '" + synchronizableTypeName + "'.");
				synchronizable = null;
				return false;
			}
			return true;
		}

		private bool TryGetCustomSynchronizable(string synchronizableTypeName, out ICustomJsonSynchronizable synchronizable)
		{
			if (string.IsNullOrEmpty(synchronizableTypeName) || !_customSynchronizables.TryGetValue(synchronizableTypeName, out synchronizable))
			{
				Debug.LogWarning("[JsonModelsSynchronizer] Unknown custom synchronizable type '" + synchronizableTypeName + "'.");
				synchronizable = null;
				return false;
			}
			return true;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3746751654u)]
		private void FromStateAuthorityToAllRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3746751654u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromStateAuthorityToAllRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.SynchronizeInternal(value);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 3694938364u)]
		private void FromInputAuthorityToStateAuthorityRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3694938364u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToStateAuthorityRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.SynchronizeInternal(value);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.All, Key = 74732381u)]
		private void FromInputAuthorityToAllRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(74732381u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToAllRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.SynchronizeInternal(value);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3642406519u)]
		private void FromStateAuthorityToAllRPC([RpcPayload] string synchronizableTypeName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3642406519u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromStateAuthorityToAllRPC(System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.ReSynchronizeInternal();
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 3139262829u)]
		private void FromInputAuthorityToStateAuthorityRPC([RpcPayload] string synchronizableTypeName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3139262829u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToStateAuthorityRPC(System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.ReSynchronizeInternal();
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.All, Key = 1334859114u)]
		private void FromInputAuthorityToAllRPC([RpcPayload] string synchronizableTypeName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1334859114u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToAllRPC(System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (TryGetSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.ReSynchronizeInternal();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3682753267u)]
		private void FromStateAuthorityToAllCustomDataRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3682753267u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromStateAuthorityToAllCustomDataRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetCustomSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.CustomSynchronizeInternal(value);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 289396217u)]
		private void FromInputAuthorityToStateAuthorityCustomDataRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(289396217u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToStateAuthorityCustomDataRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetCustomSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.CustomSynchronizeInternal(value);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.All, Key = 3577188064u)]
		private void FromInputAuthorityToAllCustomDataRPC([RpcPayload] string synchronizableTypeName, [RpcPayload(1)] byte[] value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(synchronizableTypeName);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(value.Length, 1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3577188064u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.JsonModelsSynchronizer::FromInputAuthorityToAllCustomDataRPC(System.String,System.Byte[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(synchronizableTypeName);
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
			if (TryGetCustomSynchronizable(synchronizableTypeName, out var synchronizable))
			{
				synchronizable.CustomSynchronizeInternal(value);
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

		[NetworkRpcWeavedInvoker(3746751654u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToAllRPC_0040Invoker3746751654([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromStateAuthorityToAllRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3694938364u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityRPC_0040Invoker3694938364([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToStateAuthorityRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(74732381u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToAllRPC_0040Invoker74732381([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToAllRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3642406519u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToAllRPC_0040Invoker3642406519([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromStateAuthorityToAllRPC(value);
		}

		[NetworkRpcWeavedInvoker(3139262829u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityRPC_0040Invoker3139262829([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToStateAuthorityRPC(value);
		}

		[NetworkRpcWeavedInvoker(1334859114u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToAllRPC_0040Invoker1334859114([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToAllRPC(value);
		}

		[NetworkRpcWeavedInvoker(3682753267u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToAllCustomDataRPC_0040Invoker3682753267([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromStateAuthorityToAllCustomDataRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(289396217u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityCustomDataRPC_0040Invoker289396217([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToStateAuthorityCustomDataRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3577188064u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToAllCustomDataRPC_0040Invoker3577188064([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out byte[] value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((JsonModelsSynchronizer)context.TargetBehaviour).FromInputAuthorityToAllCustomDataRPC(value, value2);
		}
	}
}
