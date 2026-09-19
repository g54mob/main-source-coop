using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerAlivePart : NetworkBehaviour
	{
		private int _deadPartUsageCount;

		private PlayerDeadPartModel _playerDeadPartModel;

		[field: SerializeField]
		public DeadPartJoinBoosterBehaviour DeadPartJoinBoosterBehaviour { get; private set; }

		[field: SerializeField]
		public Transform UpperPos { get; private set; }

		[field: SerializeField]
		public Transform DownPos { get; private set; }

		[field: SerializeField]
		public Transform DeadPartTarget { get; private set; }

		[field: SerializeField]
		public Rigidbody Rigidbody { get; private set; }

		public int DeadPartUsageCount
		{
			get
			{
				return _deadPartUsageCount;
			}
			private set
			{
				_deadPartUsageCount = value;
			}
		}

		[Inject]
		public void InjectDependencies(PlayerDeadPartModel playerDeadPartModel)
		{
			_playerDeadPartModel = playerDeadPartModel;
		}

		public override void Spawned()
		{
			if (base.Object.HasStateAuthority)
			{
				_playerDeadPartModel.RegisterLocalAlivePart(this);
			}
			_playerDeadPartModel.RegisterAlivePart(base.Object.InputAuthority, this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerDeadPartModel.UnRegisterAlivePart(base.Object.InputAuthority);
		}

		public void SetDeadPartUsageCount(int usageCount)
		{
			SetDeadPartUsageCountRpc(usageCount);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1318974385u)]
		private void SetDeadPartUsageCountRpc([RpcPayload(4)] int usageCount)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1318974385u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DeadPartsModule.Scripts.PlayerAlivePart::SetDeadPartUsageCountRpc(System.Int32)", invokeInfo, PlayerRef.None);
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
			DeadPartUsageCount = usageCount;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1318974385u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetDeadPartUsageCountRpc_0040Invoker1318974385([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerAlivePart)context.TargetBehaviour).SetDeadPartUsageCountRpc(value);
		}
	}
}
