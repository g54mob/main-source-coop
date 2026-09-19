using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Features.GrabModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(9)]
	public class PlayerDeadPart : NetworkBehaviour, IDespawned, IPublicFacingInterface
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CustomizationData", 0, 7)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private DeadPartCustomizationData _CustomizationData;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("UsageCount", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _UsageCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsUsed", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsUsed;

		[SerializeField]
		private ScreenShakeData _screenShakeData;

		[SerializeField]
		private CapsuleCollider _bodyCollider;

		[SerializeField]
		private Rigidbody _bodyRigidbody;

		private IScreenShakeService _screenShakeService;

		private PlayerDeadPartModel _playerDeadPartModel;

		[field: SerializeField]
		public TemporaryAddStatToPlayerBoosterSettings BoosterSetting { get; private set; }

		[field: SerializeField]
		public Transform UpperPos { get; private set; }

		[field: SerializeField]
		public List<SimplePointGrabable> Grabbables { get; private set; }

		[field: SerializeField]
		public List<RequestStateAuthorityOnCollide> RequestStateAuthority { get; private set; }

		[field: SerializeField]
		public DeadPartType DeadPartType { get; private set; }

		[field: SerializeField]
		public RagdollEntity RagdollEntity { get; private set; }

		[field: SerializeField]
		public SimplePointGrabable ParentGrabbable { get; private set; }

		[Networked]
		[OnChangedRender("OnCustomizationDataRender")]
		[NetworkedWeaved(0, 7)]
		public unsafe DeadPartCustomizationData CustomizationData
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.CustomizationData. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(DeadPartCustomizationData*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.CustomizationData. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(DeadPartCustomizationData*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnUsageCountRender")]
		[NetworkedWeaved(7, 1)]
		public unsafe int UsageCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.UsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[7];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.UsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[7] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(8, 1)]
		public unsafe NetworkBool IsUsed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.IsUsed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 8);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerDeadPart.IsUsed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 8) = value;
			}
		}

		public float SpawnTimer { get; private set; }

		public CapsuleCollider BodyCollider => _bodyCollider;

		public Rigidbody BodyRigidbody
		{
			get
			{
				if (!RagdollEntity.IsSimulated)
				{
					return _bodyRigidbody;
				}
				return RagdollEntity.RootPhysData.RigidBody;
			}
		}

		public event Action<DeadPartCustomizationData> OnCustomizationDataChanged;

		public event Action<int> OnUsageCountChanged;

		[Inject]
		private void InjectDependencies(IScreenShakeService screenShakeService, PlayerDeadPartModel playerDeadPartModel)
		{
			_screenShakeService = screenShakeService;
			_playerDeadPartModel = playerDeadPartModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			SpawnTimer = 0f;
			this.OnCustomizationDataChanged?.Invoke(CustomizationData);
			this.OnUsageCountChanged?.Invoke(UsageCount);
			if (!IsUsed)
			{
				_playerDeadPartModel.RegisterSpawnedDeadPart(this);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playerDeadPartModel.UnregisterSpawnedDeadPart(this);
			base.Despawned(runner, hasState);
			_screenShakeService.TriggerLocalScreenShake(_screenShakeData);
		}

		private void OnCustomizationDataRender()
		{
			this.OnCustomizationDataChanged?.Invoke(CustomizationData);
		}

		private void OnUsageCountRender()
		{
			this.OnUsageCountChanged?.Invoke(UsageCount);
		}

		private void Update()
		{
			SpawnTimer += Time.deltaTime;
		}

		public void OverrideCustomizationData(DeadPartCustomizationData customizationData)
		{
			OverrideCustomizationDataRpc(customizationData);
		}

		public void Use()
		{
			UseRpc();
		}

		public void SetUsageCount(int usageCount)
		{
			SetUsageCountRpc(usageCount);
		}

		public void DespawnPart()
		{
			DespawnPartRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1854511869u)]
		private void DespawnPartRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1854511869u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DeadPartsModule.Scripts.PlayerDeadPart::DespawnPartRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			foreach (RequestStateAuthorityOnCollide item in RequestStateAuthority)
			{
				item.Disable();
			}
			base.gameObject.SetActive(value: false);
			foreach (SimplePointGrabable grabbable in Grabbables)
			{
				grabbable.GrabBlocked = true;
			}
			if (base.HasStateAuthority)
			{
				base.Object.DespawnHierarchy();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 247881531u)]
		private void UseRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(247881531u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DeadPartsModule.Scripts.PlayerDeadPart::UseRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.HasStateAuthority)
			{
				IsUsed = true;
			}
			_playerDeadPartModel.UnregisterSpawnedDeadPart(this);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 559327704u)]
		private void OverrideCustomizationDataRpc([RpcPayload(28)] DeadPartCustomizationData customizationData)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(28);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(559327704u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DeadPartsModule.Scripts.PlayerDeadPart::OverrideCustomizationDataRpc(Features.DeadPartsModule.Scripts.DeadPartCustomizationData)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(customizationData, 28);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.HasStateAuthority)
			{
				CustomizationData = customizationData;
			}
			this.OnCustomizationDataChanged?.Invoke(customizationData);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2866607901u)]
		private void SetUsageCountRpc([RpcPayload(4)] int usageCount)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2866607901u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DeadPartsModule.Scripts.PlayerDeadPart::SetUsageCountRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(usageCount, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.HasStateAuthority)
			{
				UsageCount = usageCount;
			}
			this.OnUsageCountChanged?.Invoke(usageCount);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CustomizationData = _CustomizationData;
			UsageCount = _UsageCount;
			IsUsed = _IsUsed;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CustomizationData = CustomizationData;
			_UsageCount = UsageCount;
			_IsUsed = IsUsed;
		}

		[NetworkRpcWeavedInvoker(1854511869u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DespawnPartRpc_0040Invoker1854511869([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDeadPart)context.TargetBehaviour).DespawnPartRpc();
		}

		[NetworkRpcWeavedInvoker(247881531u)]
		[Preserve]
		[WeaverGenerated]
		protected static void UseRpc_0040Invoker247881531([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDeadPart)context.TargetBehaviour).UseRpc();
		}

		[NetworkRpcWeavedInvoker(559327704u)]
		[Preserve]
		[WeaverGenerated]
		protected static void OverrideCustomizationDataRpc_0040Invoker559327704([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out DeadPartCustomizationData value, 28);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDeadPart)context.TargetBehaviour).OverrideCustomizationDataRpc(value);
		}

		[NetworkRpcWeavedInvoker(2866607901u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetUsageCountRpc_0040Invoker2866607901([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDeadPart)context.TargetBehaviour).SetUsageCountRpc(value);
		}
	}
}
