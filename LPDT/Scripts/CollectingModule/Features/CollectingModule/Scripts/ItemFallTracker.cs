using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ItemFallTracker : NetworkBehaviour
	{
		[SerializeField]
		private MonoItem _item;

		[SerializeField]
		private LayerMask _layersToTrack;

		private CollectItemFellEvent _collectItemFellEvent;

		[Inject]
		public void InjectDependencies(CollectItemFellEvent collectItemFellEvent)
		{
			_collectItemFellEvent = collectItemFellEvent;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (IsInLayerMask(other.gameObject.layer, _layersToTrack) && base.HasStateAuthority)
			{
				InvokeCollectItemFellEventRpc();
			}
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1501436886u)]
		private void InvokeCollectItemFellEventRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1501436886u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.ItemFallTracker::InvokeCollectItemFellEventRpc()", invokeInfo, PlayerRef.None);
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
			_collectItemFellEvent.Invoke(_item);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1501436886u)]
		[Preserve]
		[WeaverGenerated]
		protected static void InvokeCollectItemFellEventRpc_0040Invoker1501436886([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ItemFallTracker)context.TargetBehaviour).InvokeCollectItemFellEventRpc();
		}
	}
}
