using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.TeethModule.Scripts.Tooth
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerTooth : NetworkBehaviour
	{
		[SerializeField]
		private int _index;

		[SerializeField]
		private ToothPreset _defaultToothPreset;

		[SerializeField]
		private bool _registerByPlayer = true;

		[SerializeField]
		private bool _isDisableInRagdoll;

		[SerializeField]
		private RagdollEntity _ragdollEntity;

		private CharacterTeethModel _characterTeethModel;

		private ToothData _toothData;

		public ToothPreset DefaultToothPreset => _defaultToothPreset;

		public ToothData ToothData => _toothData;

		[Inject]
		private void InjectDependencies(CharacterTeethModel characterTeethModel)
		{
			_characterTeethModel = characterTeethModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.gameObject.activeSelf)
			{
				_toothData = new ToothData(_defaultToothPreset, isKnocked: false);
				if (_registerByPlayer)
				{
					_characterTeethModel.RegisterCharacterTooth(base.Object.InputAuthority.PlayerId, _index, this);
				}
				SubscribeToRagdoll();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnsubscribeFromRagdoll();
			base.Despawned(runner, hasState);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 474378936u)]
		public void EnableToothRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(474378936u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.TeethModule.Scripts.Tooth.PlayerTooth::EnableToothRPC()", invokeInfo, PlayerRef.None);
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
			if (_toothData == null)
			{
				_toothData = new ToothData(_defaultToothPreset, isKnocked: false);
			}
			_toothData.IsKnocked = false;
			ApplyVisibility();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1651455649u)]
		public void DisableToothRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1651455649u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.TeethModule.Scripts.Tooth.PlayerTooth::DisableToothRPC()", invokeInfo, PlayerRef.None);
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
			if (_toothData == null)
			{
				_toothData = new ToothData(_defaultToothPreset, isKnocked: false);
			}
			_toothData.IsKnocked = true;
			base.gameObject.SetActive(value: false);
		}

		private void SubscribeToRagdoll()
		{
			if (_isDisableInRagdoll && !(_ragdollEntity == null))
			{
				_ragdollEntity.OnSimulationStarted += OnRagdollSimulationStarted;
				_ragdollEntity.OnSimulationStopped += OnRagdollSimulationStopped;
				if (_ragdollEntity.IsSimulated)
				{
					OnRagdollSimulationStarted(_ragdollEntity);
				}
			}
		}

		private void UnsubscribeFromRagdoll()
		{
			if (!(_ragdollEntity == null))
			{
				_ragdollEntity.OnSimulationStarted -= OnRagdollSimulationStarted;
				_ragdollEntity.OnSimulationStopped -= OnRagdollSimulationStopped;
				_ragdollEntity = null;
			}
		}

		private void OnRagdollSimulationStarted(IRagdollEntity _)
		{
			ApplyVisibility();
		}

		private void OnRagdollSimulationStopped(IRagdollEntity _)
		{
			ApplyVisibility();
		}

		private void ApplyVisibility()
		{
			if (_toothData != null && _toothData.IsKnocked)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			bool flag = _isDisableInRagdoll && _ragdollEntity != null && _ragdollEntity.IsSimulated;
			base.gameObject.SetActive(!flag);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(474378936u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EnableToothRPC_0040Invoker474378936([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerTooth)context.TargetBehaviour).EnableToothRPC();
		}

		[NetworkRpcWeavedInvoker(1651455649u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DisableToothRPC_0040Invoker1651455649([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerTooth)context.TargetBehaviour).DisableToothRPC();
		}
	}
}
