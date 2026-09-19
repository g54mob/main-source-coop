using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.QuotaModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BellFeature.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BellCollisionHandler : NetworkBehaviour
	{
		[SerializeField]
		private EventReference _bellSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _forceThreshold;

		[SerializeField]
		private int _targetCollisionCount;

		[SerializeField]
		private CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		private ScreenShakeData _screenShakeData;

		private QuotaCompletionModel _quotaCompletionModel;

		private QuotaSynchronizedModel _quotaSynchronizedModel;

		private IScreenShakeService _screenShakeService;

		private LevelModel _levelModel;

		private IAudioService _audioService;

		[Inject]
		public void InjectDependencies(QuotaCompletionModel quotaCompletionModel, QuotaSynchronizedModel quotaSynchronizedModel, IScreenShakeService screenShakeService, LevelModel levelModel, IAudioService audioService)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_screenShakeService = screenShakeService;
			_levelModel = levelModel;
			_audioService = audioService;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (base.HasStateAuthority && !(collision.impulse.magnitude / Time.fixedDeltaTime < _forceThreshold))
			{
				TryRegisterBellStrike();
				PlayBellSoundRPC();
			}
		}

		private void TryRegisterBellStrike()
		{
			if (!_quotaCompletionModel.IsBellActivated.Value && QuotaBellReadiness.IsQuotaMet(_quotaCompletionModel, _quotaSynchronizedModel))
			{
				RegisterBellStrikeRpc();
			}
		}

		private void EnsureQuotaCompletedFlag()
		{
			if (!_quotaCompletionModel.IsQuotaCompleted.Value)
			{
				_quotaCompletionModel.IsQuotaCompleted.Value = true;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1625627558u)]
		private void RegisterBellStrikeRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1625627558u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BellFeature.Scripts.BellCollisionHandler::RegisterBellStrikeRpc()", invokeInfo, PlayerRef.None);
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
			if (base.Runner.IsSharedModeMasterClient && !_quotaCompletionModel.IsBellActivated.Value)
			{
				EnsureQuotaCompletedFlag();
				_levelModel.BellStrikeCount.Value++;
				if (_levelModel.BellStrikeCount.Value >= _targetCollisionCount)
				{
					_levelModel.CountdownStartTick.Value = base.Runner.Tick;
					_quotaCompletionModel.IsBellActivated.Value = true;
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2186488382u)]
		private void PlayBellSoundRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2186488382u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BellFeature.Scripts.BellCollisionHandler::PlayBellSoundRPC()", invokeInfo, PlayerRef.None);
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
			if (!(_cinemachineImpulseSource == null))
			{
				_screenShakeService.TriggerScreenShake(_cinemachineImpulseSource, _screenShakeData);
				_audioService.PlayOneShotAttached(_bellSound, _soundSourceBehaviour);
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

		[NetworkRpcWeavedInvoker(1625627558u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RegisterBellStrikeRpc_0040Invoker1625627558([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BellCollisionHandler)context.TargetBehaviour).RegisterBellStrikeRpc();
		}

		[NetworkRpcWeavedInvoker(2186488382u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayBellSoundRPC_0040Invoker2186488382([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BellCollisionHandler)context.TargetBehaviour).PlayBellSoundRPC();
		}
	}
}
