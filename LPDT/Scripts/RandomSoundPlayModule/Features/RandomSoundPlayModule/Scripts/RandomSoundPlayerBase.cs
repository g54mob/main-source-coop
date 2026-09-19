using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using Zenject;

namespace Features.RandomSoundPlayModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public abstract class RandomSoundPlayerBase : NetworkBehaviour
	{
		[SerializeField]
		private RandomSoundType _soundType = RandomSoundType.DoorKnock;

		[SerializeField]
		[FormerlySerializedAs("_knockSound")]
		private EventReference _sound;

		[SerializeField]
		[Tooltip("If enabled, only the triggering player hears the sound, as a 2D event.")]
		private bool _isLocalSound;

		[SerializeField]
		[Range(0f, 1f)]
		private float _playChance = 0.35f;

		[SerializeField]
		private bool _canPlayNearEnemy = true;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[WeaverGenerated]
		[DefaultForProperty("SoundPlayed", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _SoundPlayed;

		private RandomSoundPlayedModel _randomSoundPlayedModel;

		private RandomSoundPlayConfiguration _randomSoundPlayConfiguration;

		private EnemyTransformsModel _enemyTransformsModel;

		private IAudioService _audioService;

		private bool _isSpawned;

		private EventInstance _soundInstance;

		private bool _hasSoundInstance;

		private bool _isFadingOut;

		private Coroutine _fadeOutRoutine;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool SoundPlayed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RandomSoundPlayerBase.SoundPlayed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RandomSoundPlayerBase.SoundPlayed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		protected bool HasActiveSoundInstance
		{
			get
			{
				if (_hasSoundInstance)
				{
					return _soundInstance.isValid();
				}
				return false;
			}
		}

		protected bool IsFadingSound => _isFadingOut;

		[Inject]
		private void InjectDependencies(RandomSoundPlayedModel randomSoundPlayedModel, RandomSoundPlayConfiguration randomSoundPlayConfiguration, EnemyTransformsModel enemyTransformsModel, IAudioService audioService)
		{
			_randomSoundPlayedModel = randomSoundPlayedModel;
			_randomSoundPlayConfiguration = randomSoundPlayConfiguration;
			_enemyTransformsModel = enemyTransformsModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_isSpawned = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			StopPlayingSoundImmediate();
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			StopPlayingSoundImmediate();
		}

		protected virtual void Update()
		{
			ReleaseSoundIfFinished();
		}

		protected bool TryPlayRandomSound(Vector3 soundPosition, int triggeringPlayerId)
		{
			if (!CanTryPlay())
			{
				return false;
			}
			if (IsBlockedByEnemyProximity(triggeringPlayerId))
			{
				return false;
			}
			if (UnityEngine.Random.value > _playChance)
			{
				return false;
			}
			int maxSoundsPerLevel = _randomSoundPlayConfiguration.GetMaxSoundsPerLevel(_soundType);
			if (!_randomSoundPlayedModel.TryRegisterPlayed(_soundType, maxSoundsPerLevel))
			{
				return false;
			}
			SoundPlayed = true;
			PlaySoundRPC(soundPosition, triggeringPlayerId);
			return true;
		}

		protected bool CanTryPlay()
		{
			if (!_isSpawned || base.Object == null || !base.Object.IsValid || (bool)SoundPlayed || !base.Object.HasStateAuthority)
			{
				return false;
			}
			if (_randomSoundPlayedModel == null || _randomSoundPlayConfiguration == null)
			{
				return false;
			}
			int maxSoundsPerLevel = _randomSoundPlayConfiguration.GetMaxSoundsPerLevel(_soundType);
			return !_randomSoundPlayedModel.IsLimitReached(_soundType, maxSoundsPerLevel);
		}

		protected void FadeOutPlayingSound(float duration)
		{
			if (base.HasStateAuthority && !_isFadingOut && (_isLocalSound || _hasSoundInstance))
			{
				FadeOutPlayingSoundRpc(duration);
			}
		}

		private bool IsBlockedByEnemyProximity(int triggeringPlayerId)
		{
			if (_canPlayNearEnemy || _enemyTransformsModel == null)
			{
				return false;
			}
			return _enemyTransformsModel.HasEnemiesNearPlayer(triggeringPlayerId);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 213067781u)]
		private void PlaySoundRPC([RpcPayload(12)] Vector3 position, [RpcPayload(4)] int triggeringPlayerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(213067781u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RandomSoundPlayModule.Scripts.RandomSoundPlayerBase::PlaySoundRPC(UnityEngine.Vector3,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(position, 12);
						writer.Write(triggeringPlayerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			StopPlayingSoundImmediate();
			if ((!_isLocalSound || IsLocalPlayer(triggeringPlayerId)) && !_sound.IsNull && _audioService != null && (_isLocalSound || !(_soundSourceBehaviour == null)))
			{
				_soundInstance = _audioService.CreateInstance(_sound);
				_hasSoundInstance = true;
				if (_isLocalSound)
				{
					_soundInstance.start();
				}
				else
				{
					_audioService.StartInstanceWith3DAttributes(_soundInstance, new GenericSoundSource(position, _soundSourceBehaviour.ID));
				}
			}
		}

		private bool IsLocalPlayer(int playerId)
		{
			if (base.Runner == null)
			{
				return false;
			}
			return base.Runner.LocalPlayer.PlayerId == playerId;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 216790050u)]
		private void FadeOutPlayingSoundRpc([RpcPayload(4)] float duration)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(216790050u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RandomSoundPlayModule.Scripts.RandomSoundPlayerBase::FadeOutPlayingSoundRpc(System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(duration, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_fadeOutRoutine != null)
			{
				StopCoroutine(_fadeOutRoutine);
			}
			_fadeOutRoutine = StartCoroutine(FadeOutPlayingSoundRoutine(duration));
		}

		private IEnumerator FadeOutPlayingSoundRoutine(float duration)
		{
			_isFadingOut = true;
			float fadeDuration = Mathf.Max(0.01f, duration);
			float elapsed = 0f;
			while (elapsed < fadeDuration)
			{
				elapsed += Time.deltaTime;
				if (_hasSoundInstance && _soundInstance.isValid())
				{
					_soundInstance.setVolume(Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsed / fadeDuration)));
				}
				yield return null;
			}
			StopPlayingSoundImmediate();
		}

		private void ReleaseSoundIfFinished()
		{
			if (!_hasSoundInstance || _isFadingOut)
			{
				return;
			}
			if (!_soundInstance.isValid())
			{
				_hasSoundInstance = false;
				_soundInstance = default(EventInstance);
				return;
			}
			_soundInstance.getPlaybackState(out var state);
			if (state == PLAYBACK_STATE.STOPPED)
			{
				StopPlayingSoundImmediate();
			}
		}

		private void StopPlayingSoundImmediate()
		{
			if (_fadeOutRoutine != null)
			{
				StopCoroutine(_fadeOutRoutine);
				_fadeOutRoutine = null;
			}
			_isFadingOut = false;
			if (_hasSoundInstance)
			{
				if (_audioService != null)
				{
					_audioService.StopInstance(_soundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
					_audioService.ReleaseInstance(_soundInstance);
				}
				_soundInstance = default(EventInstance);
				_hasSoundInstance = false;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SoundPlayed = _SoundPlayed;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SoundPlayed = SoundPlayed;
		}

		[NetworkRpcWeavedInvoker(213067781u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaySoundRPC_0040Invoker213067781([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out Vector3 value, 12);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RandomSoundPlayerBase)context.TargetBehaviour).PlaySoundRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(216790050u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FadeOutPlayingSoundRpc_0040Invoker216790050([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out float value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RandomSoundPlayerBase)context.TargetBehaviour).FadeOutPlayingSoundRpc(value);
		}
	}
}
