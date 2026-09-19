using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	[NetworkBehaviourWeaved(0)]
	public class PirateAudioController : NetworkBehaviour
	{
		[SerializeField]
		private PirateEnemyContext _context;

		[SerializeField]
		private EventReference _melleeAttackAudioEvent;

		[SerializeField]
		private EventReference _pistolAttackAudioEvent;

		[SerializeField]
		private EventReference _woodenStepAudioEvent;

		[SerializeField]
		private EventReference _stepAudioEvent;

		[SerializeField]
		private EventReference _goAwayEvent;

		[SerializeField]
		private EventReference _breathEvent;

		[SerializeField]
		private EventReference _safeZoneEvent;

		private PlayerMovableModel _playerMovableModel;

		private EventInstance _breathInstance;

		private IAudioService _audioService;

		[field: SerializeField]
		public string VoiceOcclusionLowPassParameterName { get; private set; } = "VoiceOcclusionLowPass";

		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float OcclusionMaxDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; private set; } = 2f;

		[field: SerializeField]
		public float LowPassMinValue { get; private set; } = 0.35f;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, IAudioService audioService)
		{
			_playerMovableModel = playerMovableModel;
			_audioService = audioService;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_breathInstance = _audioService.CreateInstance(_breathEvent);
			_audioService.StartInstanceWith3DAttributes(_breathInstance, _context.SoundSourceBehaviour);
			_context.PirateAnimationController.OnMeleeAttackStarted += PlayMeleeAttackAudio;
			_context.PirateAnimationController.OnPistolAttackStarted += PlayPistolAttackAudio;
			_context.PirateAnimationController.OnWoodenStep += PlayWoodenStepAudio;
			_context.PirateAnimationController.OnStep += PlayStepAudio;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_audioService.StopInstance(_breathInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(_breathInstance);
			_context.PirateAnimationController.OnMeleeAttackStarted -= PlayMeleeAttackAudio;
			_context.PirateAnimationController.OnPistolAttackStarted -= PlayPistolAttackAudio;
			_context.PirateAnimationController.OnWoodenStep -= PlayWoodenStepAudio;
			_context.PirateAnimationController.OnStep += PlayStepAudio;
		}

		private void Update()
		{
			if (_breathInstance.isValid())
			{
				_breathInstance.set3DAttributes(_context.SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
				SetSoundImmediately(_breathInstance);
			}
		}

		private void PlayStepAudio()
		{
			PlayOneShot(_stepAudioEvent);
		}

		private void PlayWoodenStepAudio()
		{
			PlayOneShot(_woodenStepAudioEvent);
		}

		private void PlayMeleeAttackAudio()
		{
			PlayOneShot(_melleeAttackAudioEvent);
		}

		private void PlayPistolAttackAudio()
		{
			PlayOneShot(_pistolAttackAudioEvent);
		}

		public void PlayGoAwaySound()
		{
			PlayGoAwaySoundRpc();
		}

		public void PlaySafeZoneSound()
		{
			PlaySafeZoneSoundRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1027559954u)]
		private void PlaySafeZoneSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1027559954u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Pirato.PirateAudioController::PlaySafeZoneSoundRpc()", invokeInfo, PlayerRef.None);
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
			PlayOneShot(_safeZoneEvent);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1312131937u)]
		private void PlayGoAwaySoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1312131937u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Pirato.PirateAudioController::PlayGoAwaySoundRpc()", invokeInfo, PlayerRef.None);
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
			PlayOneShot(_goAwayEvent);
		}

		private void PlayOneShot(EventReference reference)
		{
			if (!reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _context.SoundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(VoiceOcclusionLowPassParameterName, out var _);
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, OcclusionMaxDistance);
					float value2 = Mathf.Lerp(1f, LowPassMinValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, 1f);
				}
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

		[NetworkRpcWeavedInvoker(1027559954u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaySafeZoneSoundRpc_0040Invoker1027559954([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PirateAudioController)context.TargetBehaviour).PlaySafeZoneSoundRpc();
		}

		[NetworkRpcWeavedInvoker(1312131937u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayGoAwaySoundRpc_0040Invoker1312131937([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PirateAudioController)context.TargetBehaviour).PlayGoAwaySoundRpc();
		}
	}
}
