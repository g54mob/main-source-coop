using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio.Data;
using Ami.BroAudio.Runtime;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ami.BroAudio
{
	public static class SoundIDExtension
	{
		public static BroAudioType ToAudioType(this SoundID id)
		{
			if (!(id.Entity != null))
			{
				return BroAudioType.None;
			}
			return id.Entity.AudioType;
		}

		public static bool IsValid(this SoundID id)
		{
			return id.Entity != null;
		}

		public static AudioClip GetAudioClip(this SoundID id)
		{
			return SoundManager.Instance.GetAudioClip(id);
		}

		public static AudioClip GetAudioClip(this SoundID id, int velocity)
		{
			return SoundManager.Instance.GetAudioClip(id, velocity);
		}

		public static AudioClip GetAudioClip(this SoundID id, PlaybackStage chainedModeStage)
		{
			return SoundManager.Instance.GetAudioClip(id, chainedModeStage);
		}

		public static bool HasAnyPlayingInstances(this SoundID id)
		{
			return SoundManager.Instance.HasAnyPlayingInstances(id);
		}

		public static IAudioPlayer Play(this SoundID id, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, playableValidator);
		}

		public static IAudioPlayer Play(this SoundID id, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, fadeIn, playableValidator);
		}

		public static IAudioPlayer Play(this SoundID id, Vector3 position, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, position, playableValidator);
		}

		public static IAudioPlayer Play(this SoundID id, Vector3 position, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, position, fadeIn, playableValidator);
		}

		public static IAudioPlayer Play(this SoundID id, Transform followTarget, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, followTarget, playableValidator);
		}

		public static IAudioPlayer Play(this SoundID id, Transform followTarget, float fadeIn, IPlayableValidator playableValidator = null)
		{
			return BroAudio.Play(id, followTarget, fadeIn, playableValidator);
		}

		public static AsyncOperationHandle<IList<AudioClip>> LoadAllAssetsAsync(this SoundID id)
		{
			return SoundManager.Instance.LoadAllAssetsAsync(id);
		}

		public static AsyncOperationHandle<AudioClip> LoadAssetAsync(this SoundID id)
		{
			return id.LoadAssetAsync(0);
		}

		public static AsyncOperationHandle<AudioClip> LoadAssetAsync(this SoundID id, int clipIndex)
		{
			return SoundManager.Instance.LoadAssetAsync(id, clipIndex);
		}

		public static void ReleaseAllAssets(this SoundID id)
		{
			SoundManager.Instance.ReleaseAllAssets(id);
		}

		public static void ReleaseAsset(this SoundID id)
		{
			id.ReleaseAsset(0);
		}

		public static void ReleaseAsset(this SoundID id, int clipIndex)
		{
			SoundManager.Instance.ReleaseAsset(id, clipIndex);
		}

		public static IEnumerable GetAllAddressablesKeys(this SoundID id)
		{
			return SoundManager.Instance.GetAddressableKeys(id);
		}

		public static object GetAddressablesKey(this SoundID id)
		{
			return id.GetAddressablesKey(0);
		}

		public static object GetAddressablesKey(this SoundID id, int index)
		{
			return SoundManager.Instance.GetAddressableKey(id, index);
		}

		[Obsolete("Only for backwards compatibility")]
		public static bool TryConvertIdToEntity(int id, out AudioEntity entity)
		{
			if (id == 0 || id == -1)
			{
				entity = null;
				return false;
			}
			if (SoundManager.HasInstance && SoundManager.Instance.TryConvertIdToEntity(id, out entity))
			{
				return true;
			}
			Debug.LogError($"Could not find entity with ID {id} to convert SoundID to entity with");
			entity = null;
			return false;
		}
	}
}
