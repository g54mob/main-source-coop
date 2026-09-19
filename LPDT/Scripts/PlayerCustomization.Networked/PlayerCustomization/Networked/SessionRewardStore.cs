using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.SkinConfiguration.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace PlayerCustomization.Networked
{
	[NetworkBehaviourWeaved(1089)]
	public class SessionRewardStore : NetworkBehaviour
	{
		public const int CAPACITY = 16;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Rewards", 0, 1088)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private RewardSlot[] _Rewards;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("RewardsVersion", 1088, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RewardsVersion;

		private SessionRewardModel _sessionRewardModel;

		private int _lastObservedVersion = -1;

		[Networked]
		[Capacity(16)]
		[NetworkedWeaved(0, 1088)]
		[NetworkedWeavedArray(16, 68, typeof(ElementReaderWriterUnmanaged<RewardSlot, MetaConstant68>))]
		public unsafe NetworkArray<RewardSlot> Rewards
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionRewardStore.Rewards. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<RewardSlot>((byte*)Ptr + 0, 16, ElementReaderWriterUnmanaged<RewardSlot, MetaConstant68>.GetInstance());
			}
		}

		[Networked]
		[NetworkedWeaved(1088, 1)]
		public unsafe int RewardsVersion
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionRewardStore.RewardsVersion. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1088];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionRewardStore.RewardsVersion. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1088] = value;
			}
		}

		[Inject]
		public void InjectDependencies(SessionRewardModel sessionRewardModel)
		{
			_sessionRewardModel = sessionRewardModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_sessionRewardModel.RegisterStore(this);
		}

		public override void Render()
		{
			if (RewardsVersion != _lastObservedVersion)
			{
				_lastObservedVersion = RewardsVersion;
				_sessionRewardModel.NotifyRewardsChanged();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_sessionRewardModel.UnregisterStore(this);
		}

		public IEnumerable<RewardSlot> EnumerateRewards()
		{
			for (int i = 0; i < 16; i++)
			{
				yield return Rewards[i];
			}
		}

		public bool Grant(string persistentId, CosmeticRewardPart part, SkinType skinId)
		{
			if (string.IsNullOrEmpty(persistentId) || part == CosmeticRewardPart.None)
			{
				return false;
			}
			if (!base.Object || !base.Object.IsValid)
			{
				return false;
			}
			if (!base.HasStateAuthority)
			{
				GrantRpc(persistentId, (int)part, (int)skinId);
				return true;
			}
			return WriteGrant(persistentId, part, skinId);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3272042030u)]
		private void GrantRpc([RpcPayload] string persistentId, [RpcPayload(4)] int part, [RpcPayload(4)] int skinId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(persistentId);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3272042030u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void PlayerCustomization.Networked.SessionRewardStore::GrantRpc(System.String,System.Int32,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(persistentId);
						writer.Write(part, 4);
						writer.Write(skinId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			WriteGrant(persistentId, (CosmeticRewardPart)part, (SkinType)skinId);
		}

		private bool WriteGrant(string persistentId, CosmeticRewardPart part, SkinType skinId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			int num = FindSlot(persistentId, part);
			int num2 = ((num >= 0) ? num : FindFreeSlot());
			if (num2 < 0)
			{
				Debug.LogWarning($"[SessionRewardStore] full (CAPACITY {16}) — drop grant {part}/{skinId} for {persistentId}.");
				return false;
			}
			if (num >= 0 && Rewards[num].SkinId == skinId)
			{
				return true;
			}
			Rewards.Set(num2, new RewardSlot
			{
				PersistentId = persistentId,
				Part = part,
				SkinId = skinId,
				Occupied = true
			});
			RewardsVersion++;
			return true;
		}

		private int FindSlot(string persistentId, CosmeticRewardPart part)
		{
			for (int i = 0; i < 16; i++)
			{
				RewardSlot rewardSlot = Rewards[i];
				if ((bool)rewardSlot.Occupied && rewardSlot.Part == part && rewardSlot.PersistentId.Value == persistentId)
				{
					return i;
				}
			}
			return -1;
		}

		private int FindFreeSlot()
		{
			for (int i = 0; i < 16; i++)
			{
				if (!Rewards[i].Occupied)
				{
					return i;
				}
			}
			return -1;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkArray(Rewards, _Rewards, "Rewards");
			RewardsVersion = _RewardsVersion;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkArray(Rewards, ref _Rewards);
			_RewardsVersion = RewardsVersion;
		}

		[NetworkRpcWeavedInvoker(3272042030u)]
		[Preserve]
		[WeaverGenerated]
		protected static void GrantRpc_0040Invoker3272042030([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out string value);
			payloadReader.Read(out int value2, 4);
			payloadReader.Read(out int value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SessionRewardStore)context.TargetBehaviour).GrantRpc(value, value2, value3);
		}
	}
}
