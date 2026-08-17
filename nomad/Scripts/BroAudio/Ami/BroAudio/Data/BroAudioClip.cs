using System;
using Ami.BroAudio.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public class BroAudioClip : IBroAudioClip
	{
		public static class NameOf
		{
			public const string AudioClip = "AudioClip";

			public const string AudioClipAssetReference = "AudioClipAssetReference";
		}

		[SerializeField]
		private AssetReferenceT<AudioClip> AudioClipAssetReference;

		[SerializeField]
		private AudioClip AudioClip;

		public float Volume = 1f;

		public float Delay;

		public float StartPosition;

		public float EndPosition;

		public float FadeIn;

		public float FadeOut;

		public int Weight;

		public IKeyEvaluator AddressableKey => AudioClipAssetReference;

		public bool IsLoaded
		{
			get
			{
				if (!(AudioClip != null))
				{
					return AudioClipAssetReference.Asset != null;
				}
				return true;
			}
		}

		public bool IsLoading
		{
			get
			{
				if (AudioClipAssetReference.OperationHandle.IsValid())
				{
					return !AudioClipAssetReference.OperationHandle.IsDone;
				}
				return false;
			}
		}

		float IBroAudioClip.Volume => Volume;

		float IBroAudioClip.Delay => Delay;

		float IBroAudioClip.StartPosition => StartPosition;

		float IBroAudioClip.EndPosition => EndPosition;

		float IBroAudioClip.FadeIn => FadeIn;

		float IBroAudioClip.FadeOut => FadeOut;

		public int Velocity => Weight;

		public bool IsSet
		{
			get
			{
				if (AudioClip != null)
				{
					return true;
				}
				return IsAddressablesAvailable();
			}
		}

		public AsyncOperationHandle<AudioClip> LoadAssetAsync()
		{
			if (IsLoading || IsLoaded)
			{
				return AudioClipAssetReference.OperationHandle.Convert<AudioClip>();
			}
			return AudioClipAssetReference.LoadAssetAsync();
		}

		public AsyncOperationHandle GetCurrentOperationHandle()
		{
			return AudioClipAssetReference.OperationHandle;
		}

		public void ReleaseAsset()
		{
			if (AudioClipAssetReference.OperationHandle.IsValid())
			{
				Addressables.Release(AudioClipAssetReference.OperationHandle);
			}
		}

		[Obsolete("You cannot set the loaded asset for an addressable clip.")]
		public void SetLoadedAsset(AudioClip clip)
		{
		}

		public AudioClip GetAudioClip()
		{
			if (AudioClip != null)
			{
				return AudioClip;
			}
			string text = null;
			if (IsAddressablesAvailable())
			{
				text = AudioClipAssetReference.AssetGUID;
				if (AudioClipAssetReference.Asset is AudioClip result)
				{
					return result;
				}
				AsyncOperationHandle<AudioClip> asyncOperationHandle = AudioClipAssetReference.LoadAssetAsync();
				asyncOperationHandle.WaitForCompletion();
				if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
				{
					return asyncOperationHandle.Result;
				}
				throw new BroAudioException("Failed to synchronously load AudioClip [<b>" + text + "</b>]");
			}
			return AudioClip;
		}

		public bool IsAddressablesAvailable()
		{
			return !string.IsNullOrEmpty(AudioClipAssetReference.AssetGUID);
		}

		public bool IsValid()
		{
			if (AudioClip != null)
			{
				return true;
			}
			return IsAddressablesAvailable();
		}
	}
}
