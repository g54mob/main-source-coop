using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CollectItemDiceTwentyFaceVfx : NetworkBehaviour
	{
		[SerializeField]
		private Transform _twentyFaceUp;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private CollectItemDiceConfig _config;

		private bool _wasNatTwenty;

		public override void Spawned()
		{
			_config = _monoItem.DefaultConfig as CollectItemDiceConfig;
			_wasNatTwenty = IsNatTwentySettled();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				bool flag = IsNatTwentySettled();
				if (flag && !_wasNatTwenty)
				{
					PlayNatTwentyVfxRpc();
				}
				_wasNatTwenty = flag;
			}
		}

		private bool IsNatTwentySettled()
		{
			if (_config == null)
			{
				return false;
			}
			if (_simplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				return false;
			}
			if (!IsAtRest())
			{
				return false;
			}
			return Vector3.Dot(_twentyFaceUp.up, Vector3.up) >= _config.UpAlignmentDot;
		}

		private bool IsAtRest()
		{
			float restLinearSpeed = _config.RestLinearSpeed;
			float restAngularSpeed = _config.RestAngularSpeed;
			if (_rigidbody.linearVelocity.sqrMagnitude <= restLinearSpeed * restLinearSpeed)
			{
				return _rigidbody.angularVelocity.sqrMagnitude <= restAngularSpeed * restAngularSpeed;
			}
			return false;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1920304998u)]
		private void PlayNatTwentyVfxRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1920304998u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.CollectItemDiceTwentyFaceVfx::PlayNatTwentyVfxRpc()", invokeInfo, PlayerRef.None);
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
			if (!(_config == null) && !(_config.NatTwentyVfxPrefab == null))
			{
				UnityEngine.Object.Instantiate(_config.NatTwentyVfxPrefab, _rigidbody.position, Quaternion.identity).transform.localScale = Vector3.one * _config.VfxScale;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(_twentyFaceUp.position, _twentyFaceUp.up * 0.25f);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1920304998u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayNatTwentyVfxRpc_0040Invoker1920304998([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CollectItemDiceTwentyFaceVfx)context.TargetBehaviour).PlayNatTwentyVfxRpc();
		}
	}
}
