using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio.Runtime;
using Ami.Extension;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public class AudioEntity : ScriptableObject, IAudioEntity, IReadOnlyAudioEntity
	{
		public bool UseAddressables;

		[SerializeField]
		private List<AudioParameterBinding> bindings = new List<AudioParameterBinding>();

		[SerializeField]
		private bool hasLoopRegion;

		[SerializeField]
		private float loopStartSeconds;

		[SerializeField]
		private float loopEndSeconds;

		[SerializeField]
		private float loopSeamCrossfadeSeconds;

		[SerializeField]
		private LoopSeamCrossfadeCurve loopSeamCrossfadeCurve;

		[SerializeField]
		private List<AudioParameterDefinition> parameters = new List<AudioParameterDefinition>();

		[SerializeField]
		private List<AudioTransition> transitions = new List<AudioTransition>();

		[SerializeField]
		private MulticlipsPlayMode MulticlipsPlayMode;

		[SerializeField]
		private PlaybackGroup _group;

		public BroAudioClip[] Clips;

		private IClipSelectionStrategy _clipSelectionStrategy;

		public IReadOnlyList<AudioParameterBinding> Bindings => bindings;

		public bool HasLoopRegion => hasLoopRegion;

		public float LoopStartSeconds => loopStartSeconds;

		public float LoopEndSeconds => loopEndSeconds;

		public float LoopSeamCrossfadeSeconds => loopSeamCrossfadeSeconds;

		public LoopSeamCrossfadeCurve LoopSeamCrossfadeCurve => loopSeamCrossfadeCurve;

		public IReadOnlyList<AudioParameterDefinition> Parameters => parameters;

		public IReadOnlyList<AudioTransition> Transitions => transitions;

		public string Name => base.name;

		[Obsolete("IDs are superceded by direct references", true)]
		[field: SerializeField]
		public int ID { get; private set; }

		private PlaybackGroup _upperGroup
		{
			get
			{
				if (!(AudioAsset != null))
				{
					return null;
				}
				return AudioAsset.PlaybackGroup;
			}
		}

		[field: SerializeField]
		public AudioAsset AudioAsset { get; private set; }

		[field: SerializeField]
		public BroAudioType AudioType { get; private set; }

		[field: SerializeField]
		public float MasterVolume { get; private set; }

		[field: SerializeField]
		public bool Loop { get; private set; }

		[field: SerializeField]
		public bool SeamlessLoop { get; private set; }

		[field: SerializeField]
		public float TransitionTime { get; private set; }

		[field: SerializeField]
		public SpatialSetting SpatialSetting { get; private set; }

		[field: SerializeField]
		public int Priority { get; private set; }

		[field: SerializeField]
		public float Pitch { get; private set; }

		[field: SerializeField]
		public float PitchRandomRange { get; private set; }

		[field: SerializeField]
		public float VolumeRandomRange { get; private set; }

		[field: SerializeField]
		public RandomFlag RandomFlags { get; private set; }

		public PlaybackGroup PlaybackGroup
		{
			get
			{
				if (!_group)
				{
					return _upperGroup;
				}
				return _group;
			}
		}

		IReadOnlyList<IBroAudioClip> IReadOnlyAudioEntity.Clips => Clips;

		public MulticlipsPlayMode PlayMode => MulticlipsPlayMode;

		public IEnumerable GetAllAddressableKeys()
		{
			BroAudioClip[] clips = Clips;
			foreach (BroAudioClip broAudioClip in clips)
			{
				yield return broAudioClip.AddressableKey;
			}
		}

		public object GetAddressableKey(int index)
		{
			if (index >= 0 && index < Clips.Length)
			{
				return Clips[index].AddressableKey;
			}
			return string.Empty;
		}

		public bool IsLoaded()
		{
			BroAudioClip[] clips = Clips;
			for (int i = 0; i < clips.Length; i++)
			{
				if (!clips[i].IsLoaded)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsLoaded(int clipIndex)
		{
			return Clips[clipIndex].IsLoaded;
		}

		public bool IsValidClipIndex(int clipIndex, bool logError = true)
		{
			if (clipIndex < 0 || clipIndex >= Clips.Length)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Invalid clip index: {clipIndex}. The audio entity [{Name.ToWhiteBold()}] contains {Clips.Length} clips.");
				return false;
			}
			return true;
		}

		public AsyncOperationHandle<IList<AudioClip>> LoadAssetsAsync()
		{
			List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();
			BroAudioClip[] clips = Clips;
			foreach (BroAudioClip broAudioClip in clips)
			{
				if (broAudioClip.IsAddressablesAvailable() && !broAudioClip.IsLoaded && !broAudioClip.IsLoading)
				{
					handles.Add(broAudioClip.LoadAssetAsync());
				}
				else
				{
					handles.Add(broAudioClip.GetCurrentOperationHandle());
				}
			}
			if (handles.Count == 0)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + Name.ToWhiteBold() + " has no clips to load!");
				return default(AsyncOperationHandle<IList<AudioClip>>);
			}
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle = Addressables.ResourceManager.CreateGenericGroupOperation(handles);
			return Addressables.ResourceManager.CreateChainOperation((AsyncOperationHandle)asyncOperationHandle, (Func<AsyncOperationHandle, AsyncOperationHandle<IList<AudioClip>>>)delegate
			{
				List<AudioClip> list = new List<AudioClip>(handles.Count);
				foreach (AsyncOperationHandle item in handles)
				{
					list.Add(item.Result as AudioClip);
				}
				return Addressables.ResourceManager.CreateCompletedOperation((IList<AudioClip>)list, string.Empty);
			});
		}

		[Obsolete("Batch loading is no longer supported. Use individual clip loading instead.")]
		public void SetLoadedClipList(AsyncOperationHandle<IList<AudioClip>> handle)
		{
			handle.Completed -= SetLoadedClipList;
			if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result.Count != Clips.Length)
			{
				throw handle.OperationException;
			}
		}

		public void ReleaseAllAssets()
		{
			BroAudioClip[] clips = Clips;
			for (int i = 0; i < clips.Length; i++)
			{
				clips[i].ReleaseAsset();
			}
		}

		public bool TryFindParameterById(string id, out AudioParameterDefinition definition)
		{
			if (!string.IsNullOrEmpty(id) && parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (parameters[i].Id == id)
					{
						definition = parameters[i];
						return true;
					}
				}
			}
			BroAudioGlobalParameters activeInstance = BroAudioGlobalParameters.ActiveInstance;
			if (activeInstance != null && activeInstance.TryFindParameterById(id, out definition))
			{
				return true;
			}
			definition = default(AudioParameterDefinition);
			return false;
		}

		public bool TryFindParameterByName(string name, out AudioParameterDefinition definition)
		{
			if (!string.IsNullOrEmpty(name) && parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (parameters[i].Name == name)
					{
						definition = parameters[i];
						return true;
					}
				}
			}
			BroAudioGlobalParameters activeInstance = BroAudioGlobalParameters.ActiveInstance;
			if (activeInstance != null && activeInstance.TryFindParameterByName(name, out definition))
			{
				return true;
			}
			definition = default(AudioParameterDefinition);
			return false;
		}

		public bool TryFindParameter(string idOrName, out AudioParameterDefinition definition)
		{
			if (TryFindParameterById(idOrName, out definition))
			{
				return true;
			}
			return TryFindParameterByName(idOrName, out definition);
		}

		public IBroAudioClip PickNewClip()
		{
			int index;
			return PickNewClip(0, out index);
		}

		public IBroAudioClip PickNewClip(ClipSelectionContext context)
		{
			int index;
			return PickNewClip(context, out index);
		}

		public IBroAudioClip PickNewClip(ClipSelectionContext context, out int index)
		{
			switch (MulticlipsPlayMode)
			{
			case MulticlipsPlayMode.Single:
				EnsureClipSelectionStrategy<SingleClipStrategy>();
				break;
			case MulticlipsPlayMode.Sequence:
				EnsureClipSelectionStrategy<SequenceClipStrategy>();
				break;
			case MulticlipsPlayMode.Random:
				EnsureClipSelectionStrategy<RandomClipStrategy>();
				break;
			case MulticlipsPlayMode.Shuffle:
				EnsureClipSelectionStrategy<ShuffleClipStrategy>();
				break;
			case MulticlipsPlayMode.Chained:
				EnsureClipSelectionStrategy<ChainedClipStrategy>();
				break;
			case MulticlipsPlayMode.Velocity:
				EnsureClipSelectionStrategy<VelocityClipStrategy>();
				break;
			default:
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Invalid multiclips play mode: {MulticlipsPlayMode}");
				EnsureClipSelectionStrategy<SingleClipStrategy>();
				break;
			}
			return _clipSelectionStrategy.SelectClip(Clips, context, out index);
		}

		private void EnsureClipSelectionStrategy<T>() where T : IClipSelectionStrategy, new()
		{
			if (_clipSelectionStrategy == null || _clipSelectionStrategy.GetType() != typeof(T))
			{
				_clipSelectionStrategy = new T();
			}
		}

		public bool Validate()
		{
			return Utility.Validate(Name, Clips);
		}

		public float GetMasterVolume()
		{
			return GetRandomValue(MasterVolume, RandomFlag.Volume);
		}

		public float GetPitch()
		{
			return GetRandomValue(Pitch, RandomFlag.Pitch);
		}

		public float GetRandomValue(float baseValue, RandomFlag flag)
		{
			if (!RandomFlags.Contains(flag))
			{
				return baseValue;
			}
			return GetRandomValue(baseValue, flag switch
			{
				RandomFlag.Pitch => PitchRandomRange, 
				RandomFlag.Volume => VolumeRandomRange, 
				_ => throw new InvalidOperationException(), 
			});
		}

		public static float GetRandomValue(float baseValue, float range)
		{
			float num = range * 0.5f;
			return baseValue + UnityEngine.Random.Range(0f - num, num);
		}

		public bool HasLoop(out LoopType loopType, out float transitionTime)
		{
			loopType = LoopType.None;
			transitionTime = 0f;
			if (HasLoopRegion)
			{
				return false;
			}
			if (Loop)
			{
				loopType = LoopType.Loop;
			}
			else if (SeamlessLoop)
			{
				loopType = LoopType.SeamlessLoop;
				transitionTime = TransitionTime;
			}
			else if (MulticlipsPlayMode == MulticlipsPlayMode.Chained)
			{
				loopType = SoundManager.Instance.Setting.DefaultChainedPlayModeLoop;
				transitionTime = SoundManager.Instance.Setting.DefaultChainedPlayModeTransitionTime;
			}
			return loopType != LoopType.None;
		}

		public void ResetMultiClipStrategy()
		{
			if (_clipSelectionStrategy != null)
			{
				_clipSelectionStrategy.Reset();
			}
		}

		public override string ToString()
		{
			return Name;
		}

		[Obsolete("Only for conversion")]
		internal static AudioEntity ConvertLegacy(AudioEntity_LEGACY legacy, AudioAsset asset)
		{
			AudioEntity audioEntity = ScriptableObject.CreateInstance<AudioEntity>();
			audioEntity._group = legacy._group;
			audioEntity.AudioAsset = asset;
			audioEntity.MulticlipsPlayMode = legacy.MulticlipsPlayMode;
			audioEntity.ID = legacy.ID;
			audioEntity.name = legacy.Name;
			audioEntity.Clips = legacy.Clips;
			audioEntity.MasterVolume = legacy.MasterVolume;
			audioEntity.Loop = legacy.Loop;
			audioEntity.SeamlessLoop = legacy.SeamlessLoop;
			audioEntity.TransitionTime = legacy.TransitionTime;
			audioEntity.SpatialSetting = legacy.SpatialSetting;
			audioEntity.Priority = legacy.Priority;
			audioEntity.Pitch = legacy.Pitch;
			audioEntity.PitchRandomRange = legacy.PitchRandomRange;
			audioEntity.VolumeRandomRange = legacy.VolumeRandomRange;
			audioEntity.RandomFlags = legacy.RandomFlags;
			audioEntity.AudioType = Utility.GetAudioType(legacy.ID);
			audioEntity.UseAddressables = legacy.UseAddressables;
			return audioEntity;
		}

		public static AudioEntity CreateNewInstance(AudioAsset asset, string name, BroAudioType audioType)
		{
			AudioEntity audioEntity = ScriptableObject.CreateInstance<AudioEntity>();
			audioEntity.name = name;
			audioEntity.AudioAsset = asset;
			audioEntity.AudioType = audioType;
			audioEntity.MasterVolume = 1f;
			audioEntity.Pitch = 1f;
			audioEntity.Priority = 128;
			return audioEntity;
		}
	}
}
