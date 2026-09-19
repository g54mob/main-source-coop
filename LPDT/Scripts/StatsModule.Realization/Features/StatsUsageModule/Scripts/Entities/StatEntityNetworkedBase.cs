using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StatsUsageModule.Scripts.Entities
{
	[NetworkBehaviourWeaved(0)]
	public abstract class StatEntityNetworkedBase<TStatEnum> : NetworkBehaviour, IModifierEntity<TStatEnum>, IStatEntity<TStatEnum> where TStatEnum : Enum
	{
		private IStatEntityFactory<TStatEnum> _statEntityFactory;

		private StatEntityBase<TStatEnum> _statEntityBase;

		private StatEntityBase<TStatEnum> StatEntityBase
		{
			get
			{
				return _statEntityBase ?? (_statEntityBase = _statEntityFactory.Create());
			}
			set
			{
				_statEntityBase = value;
			}
		}

		public IStat this[TStatEnum entityStatType] => StatEntityBase[entityStatType];

		public event Action<TStatEnum> OnReachedMinValue
		{
			add
			{
				StatEntityBase.OnReachedMinValue += value;
			}
			remove
			{
				StatEntityBase.OnReachedMinValue -= value;
			}
		}

		public event Action<TStatEnum> OnReachedMaxValue
		{
			add
			{
				StatEntityBase.OnReachedMaxValue += value;
			}
			remove
			{
				StatEntityBase.OnReachedMaxValue -= value;
			}
		}

		public event Action<TStatEnum> OnBaseValueChanged
		{
			add
			{
				StatEntityBase.OnBaseValueChanged += value;
			}
			remove
			{
				StatEntityBase.OnBaseValueChanged -= value;
			}
		}

		public event Action<TStatEnum> OnBonusValueChanged
		{
			add
			{
				StatEntityBase.OnBonusValueChanged += value;
			}
			remove
			{
				StatEntityBase.OnBonusValueChanged -= value;
			}
		}

		public event Action<TStatEnum> OnFullValueChanged
		{
			add
			{
				StatEntityBase.OnFullValueChanged += value;
			}
			remove
			{
				StatEntityBase.OnFullValueChanged -= value;
			}
		}

		public event Action<TStatEnum> OnMinValueChanged
		{
			add
			{
				StatEntityBase.OnMinValueChanged += value;
			}
			remove
			{
				StatEntityBase.OnMinValueChanged -= value;
			}
		}

		public event Action<TStatEnum> OnMaxValueChanged
		{
			add
			{
				StatEntityBase.OnMaxValueChanged += value;
			}
			remove
			{
				StatEntityBase.OnMaxValueChanged -= value;
			}
		}

		[Inject]
		private void InjectDependencies(IStatEntityFactory<TStatEnum> statEntityFactory)
		{
			_statEntityFactory = statEntityFactory;
		}

		public void AddModifier(TStatEnum entityStatType, StatModifier statModifier)
		{
			StatEntityBase.AddModifier(entityStatType, statModifier);
		}

		public void AddModifierSynchronized(TStatEnum entityStatType, StatModifier statModifier, RPCType rpcType)
		{
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthorityAddModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
				return;
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthorityAddModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
			}
			if (rpcType == RPCType.InAllWays)
			{
				FromAllAuthorityToAllAuthorityAddModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
			}
		}

		public void RemoveModifier(TStatEnum entityStatType, StatModifier statModifier)
		{
			StatEntityBase.RemoveModifier(entityStatType, statModifier);
		}

		public void RemoveModifierSynchronized(TStatEnum entityStatType, StatModifier statModifier, RPCType rpcType)
		{
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthorityRemoveModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
				return;
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthorityRemoveModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
			}
			if (rpcType == RPCType.InAllWays)
			{
				FromAllAuthorityToAllAuthorityRemoveModifierRPC(entityStatType, statModifier.ToStatModifierStruct());
			}
		}

		public void SynchronizeStatModifiers(TStatEnum entityStatType, RPCType rpcType)
		{
			StatModifierStruct[] array = StatEntityBase.GetStat(entityStatType).StatModifiers.ToArrayOfStatModifierStruct();
			if (array.Length != 0)
			{
				if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
				{
					FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC(entityStatType, array);
				}
				if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
				{
					FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC(entityStatType, array);
				}
				if (rpcType == RPCType.InAllWays)
				{
					FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC(entityStatType, array);
				}
			}
		}

		public void SynchronizeStatValues(TStatEnum entityStatType, RPCType rpcType)
		{
			IStat stat = StatEntityBase.GetStat(entityStatType);
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC(entityStatType, stat.NonModifiedMaxValue, stat.MinValue, stat.Value);
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC(entityStatType, stat.NonModifiedMaxValue, stat.MinValue, stat.Value);
			}
			if (rpcType == RPCType.InAllWays)
			{
				FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC(entityStatType, stat.NonModifiedMaxValue, stat.MinValue, stat.Value);
			}
		}

		public void SynchronizeStatFull(TStatEnum entityStatType, RPCType rpcType)
		{
			SynchronizeStatModifiers(entityStatType, rpcType);
			SynchronizeStatValues(entityStatType, rpcType);
		}

		public void SynchronizeAllStatsBaseValues(RPCType rpcType)
		{
			Array values = Enum.GetValues(typeof(TStatEnum));
			StatWithValue[] array = new StatWithValue[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				TStatEnum val = (TStatEnum)values.GetValue(i);
				IStat stat = StatEntityBase.GetStat(val);
				array[i] = new StatWithValue(Convert.ToInt32(val), stat.Value);
			}
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC(array);
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC(array);
			}
			if (rpcType == RPCType.InAllWays)
			{
				FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC(array);
			}
		}

		public void RemoveModifierThatEqual(TStatEnum statType, StatModifier statModifier)
		{
			StatEntityBase.RemoveModifierThatEqual(statType, statModifier);
		}

		public void RemoveAllModifiers()
		{
			StatEntityBase.RemoveAllModifiers();
		}

		public void RemoveAllModifiersSynchronized(RPCType rpcType)
		{
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthorityRemoveAllModifiersRPC();
				return;
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthorityRemoveAllModifiersRPC();
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority)
			{
				FromAllAuthorityToAllAuthorityRemoveAllModifiersRPC();
			}
		}

		public void AddStatValueSynchronized(TStatEnum entityStatType, float value, RPCType rpcType)
		{
			if (rpcType == RPCType.FromStateAuthorityToAll && base.HasStateAuthority)
			{
				FromStateAuthorityToInputAuthorityAddStatValueRPC(entityStatType, value);
				return;
			}
			if (rpcType == RPCType.FromInputAuthorityToStateAuthority && base.HasInputAuthority)
			{
				FromInputAuthorityToStateAuthorityAddStatValueRPC(entityStatType, value);
			}
			if (rpcType == RPCType.InAllWays)
			{
				FromAllAuthorityToAllAuthorityAddStatValueRPC(entityStatType, value);
			}
		}

		public IStat GetStat(TStatEnum entityStatType)
		{
			return StatEntityBase.GetStat(entityStatType);
		}

		public void ClearStats()
		{
			StatEntityBase.ClearStats();
		}

		protected abstract void FromStateAuthorityToInputAuthorityAddModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromInputAuthorityToStateAuthorityAddModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromAllAuthorityToAllAuthorityAddModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromStateAuthorityToInputAuthorityRemoveModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromInputAuthorityToStateAuthorityRemoveModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromAllAuthorityToAllAuthorityRemoveModifierRPC(TStatEnum entityStatType, StatModifierStruct statModifier);

		protected abstract void FromStateAuthorityToInputAuthoritySynchronizeStatModifiersRPC(TStatEnum entityStatType, StatModifierStruct[] statModifiers);

		protected abstract void FromInputAuthorityToStateAuthoritySynchronizeStatModifiersRPC(TStatEnum entityStatType, StatModifierStruct[] statModifiers);

		protected abstract void FromAllAuthorityToAllAuthoritySynchronizeStatModifiersRPC(TStatEnum entityStatType, StatModifierStruct[] statModifiers);

		protected abstract void FromStateAuthorityToInputAuthoritySynchronizeStatValuesRPC(TStatEnum entityStatType, float statMaxValue, float statMinValue, float statValue);

		protected abstract void FromInputAuthorityToStateAuthoritySynchronizeStatValuesRPC(TStatEnum entityStatType, float statMaxValue, float statMinValue, float statValue);

		protected abstract void FromAllAuthorityToAllAuthoritySynchronizeStatValuesRPC(TStatEnum entityStatType, float statMaxValue, float statMinValue, float statValue);

		protected abstract void FromInputAuthorityToStateAuthorityAddStatValueRPC(TStatEnum entityStatType, float value);

		protected abstract void FromStateAuthorityToInputAuthorityAddStatValueRPC(TStatEnum entityStatType, float value);

		protected abstract void FromAllAuthorityToAllAuthorityAddStatValueRPC(TStatEnum entityStatType, float value);

		protected abstract void FromStateAuthorityToInputAuthoritySynchronizeAllStatsBaseValuesRPC(StatWithValue[] allStatsValue);

		protected abstract void FromInputAuthorityToStateAuthoritySynchronizeAllStatsBaseValuesRPC(StatWithValue[] allStatsValue);

		protected abstract void FromAllAuthorityToAllAuthoritySynchronizeAllStatsBaseValuesRPC(StatWithValue[] allStatsValue);

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2154942242u)]
		private void FromStateAuthorityToInputAuthorityRemoveAllModifiersRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2154942242u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.StatEntityNetworkedBase`1::FromStateAuthorityToInputAuthorityRemoveAllModifiersRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, (MethodBase)null, invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveAllModifiers();
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Key = 410458018u)]
		private void FromInputAuthorityToStateAuthorityRemoveAllModifiersRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(410458018u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.StatEntityNetworkedBase`1::FromInputAuthorityToStateAuthorityRemoveAllModifiersRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, (MethodBase)null, invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveAllModifiers();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2045297567u)]
		private void FromAllAuthorityToAllAuthorityRemoveAllModifiersRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2045297567u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StatsUsageModule.Scripts.Entities.StatEntityNetworkedBase`1::FromAllAuthorityToAllAuthorityRemoveAllModifiersRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, (MethodBase)null, invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			RemoveAllModifiers();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2154942242u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromStateAuthorityToInputAuthorityRemoveAllModifiersRPC_0040Invoker2154942242([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, (MethodBase)null, RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StatEntityNetworkedBase<TStatEnum>)context.TargetBehaviour).FromStateAuthorityToInputAuthorityRemoveAllModifiersRPC();
		}

		[NetworkRpcWeavedInvoker(410458018u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromInputAuthorityToStateAuthorityRemoveAllModifiersRPC_0040Invoker410458018([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, (MethodBase)null, RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StatEntityNetworkedBase<TStatEnum>)context.TargetBehaviour).FromInputAuthorityToStateAuthorityRemoveAllModifiersRPC();
		}

		[NetworkRpcWeavedInvoker(2045297567u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FromAllAuthorityToAllAuthorityRemoveAllModifiersRPC_0040Invoker2045297567([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, (MethodBase)null, RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StatEntityNetworkedBase<TStatEnum>)context.TargetBehaviour).FromAllAuthorityToAllAuthorityRemoveAllModifiersRPC();
		}
	}
}
