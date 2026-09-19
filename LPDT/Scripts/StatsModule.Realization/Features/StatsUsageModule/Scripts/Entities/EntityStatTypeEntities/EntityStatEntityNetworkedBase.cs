using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine.Scripting;

namespace Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities
{
	[NetworkBehaviourWeaved(0)]
	public class EntityStatEntityNetworkedBase : StatEntityNetworkedBase<EntityStatType>
	{
		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3513810130u)]
		protected override void FromStateAuthorityToInputAuthorityAddModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3513810130u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthorityAddModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AddModifier(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 2678498258u)]
		protected override void FromInputAuthorityToStateAuthorityAddModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2678498258u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthorityAddModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AddModifier(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3757697867u)]
		protected override void FromAllAuthorityToAllAuthorityAddModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3757697867u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthorityAddModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AddModifier(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3120905533u)]
		protected override void FromStateAuthorityToInputAuthorityRemoveModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3120905533u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthorityRemoveModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveModifierThatEqual(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 3019203645u)]
		protected override void FromInputAuthorityToStateAuthorityRemoveModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3019203645u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthorityRemoveModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveModifierThatEqual(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3194491752u)]
		protected override void FromAllAuthorityToAllAuthorityRemoveModifierRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct statModifier)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3194491752u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthorityRemoveModifierRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifier, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveModifierThatEqual(entityStatType, statModifier.ToStatModifierClass());
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 115522084u)]
		protected override void FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct[] statModifiers)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(statModifiers.Length, 12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(115522084u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifiers, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).ClearModifiers();
			foreach (StatModifier item in statModifiers.ToListOfStatModifierClass())
			{
				AddModifier(entityStatType, item);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 3558088996u)]
		protected override void FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct[] statModifiers)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(statModifiers.Length, 12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3558088996u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifiers, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).ClearModifiers();
			foreach (StatModifier item in statModifiers.ToListOfStatModifierClass())
			{
				AddModifier(entityStatType, item);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 403166219u)]
		protected override void FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(12)] StatModifierStruct[] statModifiers)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(statModifiers.Length, 12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(403166219u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,Features.StatsUsageModule.Scripts.Entities.StatModifierStruct[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statModifiers, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).ClearModifiers();
			foreach (StatModifier item in statModifiers.ToListOfStatModifierClass())
			{
				AddModifier(entityStatType, item);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2609330775u)]
		protected override void FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float statMaxValue, [RpcPayload(4)] float statMinValue, [RpcPayload(4)] float statValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2609330775u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single,System.Single,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statMaxValue, 4);
						writer.Write(statMinValue, 4);
						writer.Write(statValue, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).MaxValue = statMaxValue;
			GetStat(entityStatType).MinValue = statMinValue;
			GetStat(entityStatType).OverrideValue(statValue);
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 780730711u)]
		protected override void FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float statMaxValue, [RpcPayload(4)] float statMinValue, [RpcPayload(4)] float statValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(780730711u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single,System.Single,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statMaxValue, 4);
						writer.Write(statMinValue, 4);
						writer.Write(statValue, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).MaxValue = statMaxValue;
			GetStat(entityStatType).MinValue = statMinValue;
			GetStat(entityStatType).OverrideValue(statValue);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2413543086u)]
		protected override void FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float statMaxValue, [RpcPayload(4)] float statMinValue, [RpcPayload(4)] float statValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2413543086u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single,System.Single,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
						writer.Write(statMaxValue, 4);
						writer.Write(statMinValue, 4);
						writer.Write(statValue, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GetStat(entityStatType).MaxValue = statMaxValue;
			GetStat(entityStatType).MinValue = statMinValue;
			GetStat(entityStatType).OverrideValue(statValue);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3420092759u)]
		protected override void FromInputAuthorityToStateAuthorityAddStatValueRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3420092759u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthorityAddStatValueRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
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
			GetStat(entityStatType).AddValue(value);
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 1473786455u)]
		protected override void FromStateAuthorityToInputAuthorityAddStatValueRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1473786455u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthorityAddStatValueRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
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
			GetStat(entityStatType).AddValue(value);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2071455854u)]
		protected override void FromAllAuthorityToAllAuthorityAddStatValueRPC([RpcPayload(4)] EntityStatType entityStatType, [RpcPayload(4)] float value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2071455854u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthorityAddStatValueRPC(Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatType,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(entityStatType, 4);
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
			GetStat(entityStatType).AddValue(value);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2989455935u)]
		protected override void FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC([RpcPayload(8)] StatWithValue[] allStatsValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(allStatsValue.Length, 8);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2989455935u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC(Features.StatsUsageModule.Scripts.Entities.StatWithValue[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(allStatsValue, 8);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			for (int i = 0; i < allStatsValue.Length; i++)
			{
				StatWithValue statWithValue = allStatsValue[i];
				GetStat((EntityStatType)statWithValue.StatType).MaxValue = statWithValue.Value;
				GetStat((EntityStatType)statWithValue.StatType).OverrideValue(statWithValue.Value);
			}
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 4083204415u)]
		protected override void FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC([RpcPayload(8)] StatWithValue[] allStatsValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(allStatsValue.Length, 8);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4083204415u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC(Features.StatsUsageModule.Scripts.Entities.StatWithValue[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(allStatsValue, 8);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			for (int i = 0; i < allStatsValue.Length; i++)
			{
				StatWithValue statWithValue = allStatsValue[i];
				GetStat((EntityStatType)statWithValue.StatType).MaxValue = statWithValue.Value;
				GetStat((EntityStatType)statWithValue.StatType).OverrideValue(statWithValue.Value);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3741882744u)]
		protected override void FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC([RpcPayload(8)] StatWithValue[] allStatsValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(allStatsValue.Length, 8);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3741882744u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities.EntityStatEntityNetworkedBase::FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC(Features.StatsUsageModule.Scripts.Entities.StatWithValue[])", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(allStatsValue, 8);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			for (int i = 0; i < allStatsValue.Length; i++)
			{
				StatWithValue statWithValue = allStatsValue[i];
				GetStat((EntityStatType)statWithValue.StatType).MaxValue = statWithValue.Value;
				GetStat((EntityStatType)statWithValue.StatType).OverrideValue(statWithValue.Value);
			}
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

		[NetworkRpcWeavedInvoker(3513810130u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthorityAddModifierRPC_0040Invoker3513810130([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthorityAddModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2678498258u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityAddModifierRPC_0040Invoker2678498258([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthorityAddModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3757697867u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthorityAddModifierRPC_0040Invoker3757697867([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthorityAddModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3120905533u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthorityRemoveModifierRPC_0040Invoker3120905533([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthorityRemoveModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3019203645u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityRemoveModifierRPC_0040Invoker3019203645([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthorityRemoveModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3194491752u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthorityRemoveModifierRPC_0040Invoker3194491752([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthorityRemoveModifierRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(115522084u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC_0040Invoker115522084([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct[] value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(3558088996u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC_0040Invoker3558088996([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct[] value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(403166219u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC_0040Invoker403166219([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out StatModifierStruct[] value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2609330775u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC_0040Invoker2609330775([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out float value3, 4);
			payloadReader.Read(out float value4, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC(value, value2, value3, value4);
		}

		[NetworkRpcWeavedInvoker(780730711u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC_0040Invoker780730711([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out float value3, 4);
			payloadReader.Read(out float value4, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC(value, value2, value3, value4);
		}

		[NetworkRpcWeavedInvoker(2413543086u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC_0040Invoker2413543086([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out float value3, 4);
			payloadReader.Read(out float value4, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC(value, value2, value3, value4);
		}

		[NetworkRpcWeavedInvoker(3420092759u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityAddStatValueRPC_0040Invoker3420092759([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthorityAddStatValueRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(1473786455u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthorityAddStatValueRPC_0040Invoker1473786455([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthorityAddStatValueRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2071455854u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthorityAddStatValueRPC_0040Invoker2071455854([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out EntityStatType value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthorityAddStatValueRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2989455935u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC_0040Invoker2989455935([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out StatWithValue[] value, 8);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC(value);
		}

		[NetworkRpcWeavedInvoker(4083204415u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC_0040Invoker4083204415([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out StatWithValue[] value, 8);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC(value);
		}

		[NetworkRpcWeavedInvoker(3741882744u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC_0040Invoker3741882744([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out StatWithValue[] value, 8);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EntityStatEntityNetworkedBase)context.TargetBehaviour).FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC(value);
		}
	}
}
