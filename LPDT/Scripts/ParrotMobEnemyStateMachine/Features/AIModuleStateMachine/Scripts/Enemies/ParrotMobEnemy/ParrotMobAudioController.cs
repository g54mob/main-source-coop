using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobAudioController : NetworkBehaviour
	{
		private const float ScreamMin3DDistance = 1f;

		private ParrotMobScreamSettings _screamSettings;

		private ParrotMobAlertSettings _alertSettings;

		private EventInstance _screamInstance;

		private bool _hasInstance;

		private bool _screamPlaying;

		private float _growElapsed;

		[Inject]
		public void InjectDependencies(ParrotMobScreamSettings screamSettings, ParrotMobAlertSettings alertSettings)
		{
			_screamSettings = screamSettings;
			_alertSettings = alertSettings;
		}

		public void PlayAlert()
		{
			if (base.HasStateAuthority)
			{
				RpcPlayAlert();
			}
		}

		public void PlayScream()
		{
			if (base.HasStateAuthority)
			{
				RpcPlayScream();
			}
		}

		public void StopScream()
		{
			if (base.HasStateAuthority)
			{
				RpcStopScream();
			}
		}

		private void Update()
		{
			if (_screamPlaying)
			{
				_growElapsed += Time.deltaTime;
				ApplyAudibleRadius(CalculateBarrierSize(_growElapsed));
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true, Key = 249568274u)]
		private void RpcPlayAlert()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(249568274u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.ParrotMobAudioController::RpcPlayAlert()", invokeInfo, PlayerRef.None);
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
			RuntimeManager.PlayOneShot(_alertSettings.AlertSound, base.transform.position);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true, Key = 2309233125u)]
		private void RpcPlayScream()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2309233125u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.ParrotMobAudioController::RpcPlayScream()", invokeInfo, PlayerRef.None);
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
			if (!_screamPlaying && !(_screamSettings == null) && !_screamSettings.ScreamSound.IsNull)
			{
				if (!_hasInstance)
				{
					_screamInstance = RuntimeManager.CreateInstance(_screamSettings.ScreamSound);
					_hasInstance = true;
				}
				_growElapsed = 0f;
				_screamInstance.set3DAttributes(base.transform.To3DAttributes());
				_screamInstance.start();
				_screamPlaying = true;
				ApplyAudibleRadius(CalculateBarrierSize(0f));
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true, Key = 34872245u)]
		private void RpcStopScream()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(34872245u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.ParrotMobAudioController::RpcStopScream()", invokeInfo, PlayerRef.None);
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
			if (_screamPlaying)
			{
				_screamInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				_screamPlaying = false;
				_growElapsed = 0f;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseInstance();
			base.Despawned(runner, hasState);
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			ReleaseInstance();
		}

		private void ReleaseInstance()
		{
			if (_screamPlaying)
			{
				_screamInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				_screamPlaying = false;
			}
			_growElapsed = 0f;
			if (_hasInstance)
			{
				_screamInstance.release();
				_hasInstance = false;
			}
		}

		private float CalculateBarrierSize(float growElapsed)
		{
			float num = _screamSettings.AttractRadius * _screamSettings.ScreamVfxSizeMultiplier;
			if (_screamSettings.ScreamVfxGrowDuration <= 0f)
			{
				return num;
			}
			float t = Mathf.Clamp01(growElapsed / _screamSettings.ScreamVfxGrowDuration);
			return Mathf.Lerp(0f, num, t);
		}

		private void ApplyAudibleRadius(float barrierRadius)
		{
			if (_hasInstance)
			{
				float b = barrierRadius * _screamSettings.ScreamAudibleDistanceMultiplier;
				float value = Mathf.Max(_screamSettings.ScreamAudibleMinDistance, b);
				_screamInstance.set3DAttributes(base.transform.To3DAttributes());
				_screamInstance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, 1f);
				_screamInstance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, value);
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

		[NetworkRpcWeavedInvoker(249568274u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcPlayAlert_0040Invoker249568274([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ParrotMobAudioController)context.TargetBehaviour).RpcPlayAlert();
		}

		[NetworkRpcWeavedInvoker(2309233125u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcPlayScream_0040Invoker2309233125([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ParrotMobAudioController)context.TargetBehaviour).RpcPlayScream();
		}

		[NetworkRpcWeavedInvoker(34872245u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcStopScream_0040Invoker34872245([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ParrotMobAudioController)context.TargetBehaviour).RpcStopScream();
		}
	}
}
