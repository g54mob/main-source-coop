using System;
using System.Collections.Generic;
using Ami.BroAudio.Data;
using Ami.BroAudio.Runtime;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ami.BroAudio
{
	public static class BroAudio
	{
		public static event Action<IAudioPlayer> OnBGMChanged
		{
			add
			{
				MusicPlayer.OnBGMChanged += value;
			}
			remove
			{
				MusicPlayer.OnBGMChanged -= value;
			}
		}

		public static IAudioPlayer Play(SoundID id, IPlayableValidator playableValidator = null)
		{
			return Play(id, -1f, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return SoundManager.Instance.Play(id, fadeIn, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, Vector3 position, IPlayableValidator playableValidator = null)
		{
			return Play(id, position, -1f, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, Vector3 position, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return SoundManager.Instance.Play(id, position, fadeIn, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, Transform followTarget, IPlayableValidator playableValidator = null)
		{
			return Play(id, followTarget, -1f, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, Transform followTarget, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return SoundManager.Instance.Play(id, followTarget, fadeIn, playableValidator);
		}

		public static IAudioPlayer Play(SoundID id, Transform followTarget, bool skipLoopRegionIntro, IPlayableValidator playableValidator = null)
		{
			return ApplySkipLoopRegionIntro(SoundManager.Instance.Play(id, followTarget, -1f, playableValidator), skipLoopRegionIntro);
		}

		public static IAudioPlayer Play(SoundID id, Transform followTarget, float fadeIn, bool skipLoopRegionIntro, IPlayableValidator playableValidator = null)
		{
			return ApplySkipLoopRegionIntro(SoundManager.Instance.Play(id, followTarget, fadeIn, playableValidator), skipLoopRegionIntro);
		}

		private static IAudioPlayer ApplySkipLoopRegionIntro(IAudioPlayer player, bool skipLoopRegionIntro)
		{
			if (!skipLoopRegionIntro)
			{
				return player;
			}
			if (player is AudioPlayerInstanceWrapper audioPlayerInstanceWrapper)
			{
				((AudioPlayer)audioPlayerInstanceWrapper)?.SetSkipLoopRegionIntro(skipIntro: true);
			}
			return player;
		}

		public static void Stop(BroAudioType audioType)
		{
			SoundManager.Instance.Stop(audioType);
		}

		public static void Stop(BroAudioType audioType, float fadeOut)
		{
			SoundManager.Instance.Stop(audioType, fadeOut);
		}

		public static void Stop(SoundID id)
		{
			SoundManager.Instance.Stop(id);
		}

		public static void Stop(SoundID id, float fadeOut)
		{
			SoundManager.Instance.Stop(id, fadeOut);
		}

		public static void Pause(SoundID id)
		{
			SoundManager.Instance.Pause(id, isPause: true);
		}

		public static void Pause(SoundID id, float fadeOut)
		{
			SoundManager.Instance.Pause(id, fadeOut, isPause: true);
		}

		public static void UnPause(SoundID id)
		{
			SoundManager.Instance.Pause(id, isPause: false);
		}

		public static void UnPause(SoundID id, float fadeIn)
		{
			SoundManager.Instance.Pause(id, fadeIn, isPause: false);
		}

		public static void Pause(BroAudioType audioType)
		{
			SoundManager.Instance.Pause(audioType, isPause: true);
		}

		public static void Pause(BroAudioType audioType, float fadeOut)
		{
			SoundManager.Instance.Pause(audioType, fadeOut, isPause: true);
		}

		public static void UnPause(BroAudioType audioType)
		{
			SoundManager.Instance.Pause(audioType, isPause: false);
		}

		public static void UnPause(BroAudioType audioType, float fadeIn)
		{
			SoundManager.Instance.Pause(audioType, fadeIn, isPause: false);
		}

		public static void SetVolume(float vol, float fadeTime = 0f)
		{
			SetVolume(BroAudioType.All, vol, fadeTime);
		}

		public static void SetVolume(BroAudioType audioType, float vol, float fadeTime = 0f)
		{
			SoundManager.Instance.SetVolume(vol, audioType, fadeTime);
		}

		public static void SetVolume(SoundID id, float vol)
		{
			SetVolume(id, vol, 0f);
		}

		public static void SetVolume(SoundID id, float vol, float fadeTime)
		{
			SoundManager.Instance.SetVolume(id, vol, fadeTime);
		}

		public static void SetPitch(SoundID id, float pitch)
		{
			SoundManager.Instance.SetPitch(id, pitch, 0f);
		}

		public static void SetPitch(SoundID id, float pitch, float fadeTime)
		{
			SoundManager.Instance.SetPitch(id, pitch, fadeTime);
		}

		public static void SetPitch(float pitch)
		{
			SoundManager.Instance.SetPitch(pitch, BroAudioType.All, 0f);
		}

		[Obsolete]
		public static void SetPitch(float pitch, BroAudioType audioType)
		{
			SoundManager.Instance.SetPitch(pitch, audioType, 0f);
		}

		public static void SetPitch(BroAudioType audioType, float pitch)
		{
			SoundManager.Instance.SetPitch(pitch, audioType, 0f);
		}

		public static void SetPitch(float pitch, float fadeTime)
		{
			SoundManager.Instance.SetPitch(pitch, BroAudioType.All, fadeTime);
		}

		[Obsolete]
		public static void SetPitch(float pitch, BroAudioType audioType, float fadeTime)
		{
			SoundManager.Instance.SetPitch(pitch, audioType, fadeTime);
		}

		public static void SetPitch(BroAudioType audioType, float pitch, float fadeTime)
		{
			SoundManager.Instance.SetPitch(pitch, audioType, fadeTime);
		}

		public static void ResetMultiClipStrategy(SoundID id)
		{
			if (id.Entity != null)
			{
				id.Entity.ResetMultiClipStrategy();
			}
		}

		public static bool HasAnyPlayingInstances(SoundID id)
		{
			return SoundManager.Instance.HasAnyPlayingInstances(id);
		}

		public static bool TryGetEntityInfo(SoundID id, out IReadOnlyAudioEntity entityInfo)
		{
			entityInfo = null;
			if (SoundManager.Instance.TryGetEntity(id, out var entity, logError: false))
			{
				entityInfo = entity as IReadOnlyAudioEntity;
				return entityInfo != null;
			}
			return false;
		}

		public static IAutoResetWaitable SetEffect(Effect effect)
		{
			return SoundManager.Instance.SetEffect(effect);
		}

		public static IAutoResetWaitable SetEffect(Effect effect, BroAudioType audioType)
		{
			return SoundManager.Instance.SetEffect(audioType, effect);
		}

		public static bool IsLoaded(SoundID id)
		{
			return SoundManager.Instance.IsLoaded(id);
		}

		public static bool IsLoaded(SoundID id, int clipIndex)
		{
			return SoundManager.Instance.IsLoaded(id, clipIndex);
		}

		public static AsyncOperationHandle<IList<AudioClip>> LoadAllAssetsAsync(SoundID id)
		{
			return SoundManager.Instance.LoadAllAssetsAsync(id);
		}

		public static AsyncOperationHandle<AudioClip> LoadAssetAsync(SoundID id)
		{
			return LoadAssetAsync(id, 0);
		}

		public static AsyncOperationHandle<AudioClip> LoadAssetAsync(SoundID id, int clipIndex)
		{
			return SoundManager.Instance.LoadAssetAsync(id, clipIndex);
		}

		public static void ReleaseAllAssets(SoundID id)
		{
			SoundManager.Instance.ReleaseAllAssets(id);
		}

		public static void ReleaseAsset(SoundID id)
		{
			SoundManager.Instance.ReleaseAsset(id, 0);
		}

		public static void ReleaseAsset(SoundID id, int clipIndex)
		{
			SoundManager.Instance.ReleaseAsset(id, clipIndex);
		}
	}
}
