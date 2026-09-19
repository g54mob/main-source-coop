using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.KrakenModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class TentacleAnimationController : NetworkBehaviour
	{
		[SerializeField]
		private TentacleContext _tentacleContext;

		[SerializeField]
		private List<Animator> _animator;

		public override void Spawned()
		{
			foreach (Animator item in _animator)
			{
				item.SetFloat("TentacleSide", (_tentacleContext.TentacleController.TentacleSide != TentacleSide.Left) ? 1 : 0);
			}
		}

		public void PlayCatchAnimation(bool isBigItem)
		{
			if (base.HasStateAuthority)
			{
				SetTrigger("Catch");
			}
		}

		public void SetTrigger(string triggerName)
		{
			SetTriggerRpc(triggerName);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1830012414u)]
		private void SetTriggerRpc([RpcPayload] string triggerName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(triggerName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1830012414u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.TentacleAnimationController::SetTriggerRpc(System.String)", invokeInfo, PlayerRef.None);
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
			foreach (Animator item in _animator)
			{
				item.SetTrigger(triggerName);
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

		[NetworkRpcWeavedInvoker(1830012414u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTriggerRpc_0040Invoker1830012414([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out string value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((TentacleAnimationController)context.TargetBehaviour).SetTriggerRpc(value);
		}
	}
}
