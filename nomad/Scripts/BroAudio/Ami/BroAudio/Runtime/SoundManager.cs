using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio.Data;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ami.BroAudio.Runtime
{
	[DisallowMultipleComponent]
	[AddComponentMenu("")]
	public class SoundManager : MonoBehaviour, IAudioMixerPool
	{
		private readonly Queue<IPlayable> _playbackQueue = new Queue<IPlayable>();

		private AudioPlayer.PlaybackHandover _playbackHandoverDelegate;

		private RuntimeSetting _setting;

		private static SoundManager _instance;

		[SerializeField]
		private AudioPlayer _audioPlayerPrefab;

		private AudioPlayerObjectPool _audioPlayerPool;

		private ObjectPool<AudioMixerGroup> _audioTrackPool;

		private ObjectPool<AudioMixerGroup> _dominatorTrackPool;

		[SerializeField]
		private AudioMixer _broAudioMixer;

		[Obsolete("Only for backwards compatibility")]
		[SerializeField]
		private BroAudioData _data;

		private Dictionary<BroAudioType, AudioTypePlaybackPreference> _auidoTypePref = new Dictionary<BroAudioType, AudioTypePlaybackPreference>();

		private EffectAutomationHelper _automationHelper;

		private EffectAutomationHelper _dominatorAutomationHelper;

		private Dictionary<SoundID, AudioPlayer> _combFilteringPreventer;

		private Coroutine _masterVolumeCoroutine;

		private Dictionary<SoundID, double> _loadedEntityLastPlayedTime = new Dictionary<SoundID, double>();

		private Coroutine _addressableCleanupCoroutine;

		private WaitForSecondsRealtime _addressableCleanupInterval;

		private readonly List<SoundID> _addressableCleanupEntitiesList = new List<SoundID>();

		public RuntimeSetting Setting
		{
			get
			{
				if ((object)_setting == null)
				{
					_setting = Resources.Load<RuntimeSetting>("BroRuntimeSetting");
				}
				if (!_setting)
				{
					_setting = ScriptableObject.CreateInstance<RuntimeSetting>();
					Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>Can't load BroRuntimeSetting.asset, all setting values will be as default. If your setting file is missing. Please reimport it from the asset package");
				}
				return _setting;
			}
		}

		public static Ease FadeInEase => Instance.Setting.DefaultFadeInEase;

		public static Ease FadeOutEase => Instance.Setting.DefaultFadeOutEase;

		public static Ease SeamlessFadeIn => Instance.Setting.SeamlessFadeInEase;

		public static Ease SeamlessFadeOut => Instance.Setting.SeamlessFadeOutEase;

		public static PitchShiftingSetting PitchSetting => Instance.Setting.PitchSetting;

		public static SoundManager Instance
		{
			get
			{
				if (HasInstance)
				{
					return _instance;
				}
				throw new BroAudioException("Bro Audio is not initialized! Please call <b>BroAudio.Init()</b> first when using manual initialization.");
			}
		}

		public static bool HasInstance => _instance != null;

		public AudioMixer AudioMixer => _broAudioMixer;

		public IEnumerable GetAddressableKeys(SoundID id)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				return entity.GetAllAddressableKeys();
			}
			return null;
		}

		public object GetAddressableKey(SoundID id, int index)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				return entity.GetAddressableKey(index);
			}
			return null;
		}

		public bool IsLoaded(SoundID id)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				return entity.IsLoaded();
			}
			return false;
		}

		public bool IsLoaded(SoundID id, int clipIndex)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				return entity.IsLoaded(clipIndex);
			}
			return false;
		}

		public AsyncOperationHandle<IList<AudioClip>> LoadAllAssetsAsync(SoundID id)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				AsyncOperationHandle<IList<AudioClip>> result = entity.LoadAssetsAsync();
				UpdateLoadedEntityLastPlayedTime(id);
				return result;
			}
			return default(AsyncOperationHandle<IList<AudioClip>>);
		}

		public AsyncOperationHandle<AudioClip> LoadAssetAsync(SoundID id, int clipIndex)
		{
			if (TryGetAddressableEntity(id, out var entity) && clipIndex >= 0 && clipIndex < entity.Clips.Length)
			{
				AsyncOperationHandle<AudioClip> result = entity.Clips[clipIndex].LoadAssetAsync();
				UpdateLoadedEntityLastPlayedTime(id);
				return result;
			}
			return default(AsyncOperationHandle<AudioClip>);
		}

		public void ReleaseAllAssets(SoundID id)
		{
			if (TryGetAddressableEntity(id, out var entity))
			{
				entity.ReleaseAllAssets();
			}
		}

		public void ReleaseAsset(SoundID id, int clipIndex)
		{
			if (TryGetAddressableEntity(id, out var entity) && clipIndex >= 0 && clipIndex < entity.Clips.Length)
			{
				entity.Clips[clipIndex].ReleaseAsset();
			}
		}

		private bool TryGetAddressableEntity(SoundID id, out AudioEntity entity)
		{
			entity = null;
			if (TryGetEntity(id, out var entity2))
			{
				entity = entity2 as AudioEntity;
				if (!entity.UseAddressables)
				{
					Debug.LogError("The entity " + id.ToString().ToBold() + " isn’t marked as addressable. Please check its settings.");
				}
				return entity.UseAddressables;
			}
			return false;
		}

		public AudioClip GetAudioClip(SoundID id)
		{
			return GetAudioClip(id, (IAudioEntity x) => x.PickNewClip());
		}

		public AudioClip GetAudioClip(SoundID id, int velocity)
		{
			return GetAudioClip(id, (IAudioEntity x) => x.PickNewClip(velocity));
		}

		public AudioClip GetAudioClip(SoundID id, PlaybackStage chainedModeStage)
		{
			return GetAudioClip(id, (IAudioEntity x) => x.PickNewClip((int)chainedModeStage));
		}

		private AudioClip GetAudioClip(SoundID id, Func<IAudioEntity, IBroAudioClip> onGetAudioClip)
		{
			if (TryGetEntity(id, out var entity))
			{
				IBroAudioClip broAudioClip = onGetAudioClip?.Invoke(entity);
				if (broAudioClip != null)
				{
					return broAudioClip.GetAudioClip();
				}
			}
			return null;
		}

		public void ResetMultiClipStrategy(SoundID id)
		{
			if (TryGetEntity(id, out var entity))
			{
				entity.ResetMultiClipStrategy();
			}
		}

		public string GetNameByID(SoundID id)
		{
			if (!IsAvailable())
			{
				return string.Empty;
			}
			return id.ToString();
		}

		public bool TryGetEntity(SoundID id, out IAudioEntity entity, bool logError = true)
		{
			entity = id.Entity;
			if (logError)
			{
				if (!id.IsValid())
				{
					Debug.LogError("The SoundID hasn't been assigned yet! " + GetDebugObjectName(), GetDebugObject());
					return false;
				}
				return true;
			}
			return true;
			static GameObject GetDebugObject()
			{
				return null;
			}
			static string GetDebugObjectName()
			{
				GameObject gameObject = GetDebugObject();
				if (gameObject != null)
				{
					return "Source:" + gameObject.name.ToBold();
				}
				return string.Empty;
			}
		}

		private bool IsAvailable(bool logError = true)
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>The method " + "GetNameByID".ToWhiteBold() + " is " + "Runtime Only".ToBold().SetColor(Color.green));
				return false;
			}
			return true;
		}

		public IAudioPlayer Play(SoundID id, float fadeIn, IPlayableValidator customValidator = null)
		{
			if (IsPlayable(id, customValidator, Utility.GloballyPlayedPosition, out var entity, out var player))
			{
				PlaybackPreference pref = new PlaybackPreference(entity).SetNextFadeIn(fadeIn);
				return PlayerToPlay(id, player, pref);
			}
			return Empty.AudioPlayer;
		}

		public IAudioPlayer Play(SoundID id, Vector3 position, float fadeIn, IPlayableValidator customValidator = null)
		{
			if (IsPlayable(id, customValidator, position, out var entity, out var player))
			{
				PlaybackPreference pref = new PlaybackPreference(entity, position).SetNextFadeIn(fadeIn);
				return PlayerToPlay(id, player, pref);
			}
			return Empty.AudioPlayer;
		}

		public IAudioPlayer Play(SoundID id, Transform followTarget, float fadeIn, IPlayableValidator customValidator = null)
		{
			if (IsPlayable(id, customValidator, followTarget.position, out var entity, out var player))
			{
				PlaybackPreference pref = new PlaybackPreference(entity, followTarget).SetNextFadeIn(fadeIn);
				return PlayerToPlay(id, player, pref);
			}
			return Empty.AudioPlayer;
		}

		private bool IsPlayable(SoundID id, IPlayableValidator customValidator, Vector3 position, out IAudioEntity entity, out AudioPlayer player)
		{
			player = null;
			if (!TryGetEntity(id, out entity))
			{
				return false;
			}
			IPlayableValidator playableValidator = customValidator ?? entity.PlaybackGroup;
			if (playableValidator != null && !playableValidator.IsPlayable(id, position))
			{
				return false;
			}
			player = _audioPlayerPool.Extract();
			playableValidator?.OnGetPlayer(player);
			return true;
		}

		private IAudioPlayer PlayerToPlay(SoundID id, AudioPlayer player, PlaybackPreference pref)
		{
			BroAudioType broAudioType = id.ToAudioType();
			AudioPlayerInstanceWrapper audioPlayerInstanceWrapper = new AudioPlayerInstanceWrapper(player);
			player.SetInstanceWrapper(audioPlayerInstanceWrapper);
			player.SetPlaybackData(id, pref);
			_playbackQueue.Enqueue(player);
			if (Setting.AlwaysPlayMusicAsBGM && broAudioType == BroAudioType.Music)
			{
				player.AsBGM().SetTransition(Setting.DefaultBGMTransition, Setting.DefaultBGMTransitionTime);
			}
			if (_combFilteringPreventer == null)
			{
				_combFilteringPreventer = new Dictionary<SoundID, AudioPlayer>();
			}
			_combFilteringPreventer[id] = player;
			if (pref.IsLoop(LoopType.SeamlessLoop) || pref.Entity.PlayMode == MulticlipsPlayMode.Chained)
			{
				if (_playbackHandoverDelegate == null)
				{
					_playbackHandoverDelegate = PlaybackHandover;
				}
				player.OnPlaybackHandover = _playbackHandoverDelegate;
			}
			StartLoadingAddressableClips(pref.Entity, id);
			return audioPlayerInstanceWrapper;
		}

		private void StartLoadingAddressableClips(IAudioEntity entity, SoundID id)
		{
			if (!Setting.AutomaticallyLoadAddressableAudioClips || !(entity is AudioEntity { UseAddressables: not false, Clips: var clips }))
			{
				return;
			}
			foreach (BroAudioClip broAudioClip in clips)
			{
				if (broAudioClip.IsAddressablesAvailable() && !broAudioClip.IsLoaded && !broAudioClip.IsLoading)
				{
					broAudioClip.LoadAssetAsync();
					UpdateLoadedEntityLastPlayedTime(id);
				}
			}
		}

		private void PlaybackHandover(SoundID id, InstanceWrapper<AudioPlayer> wrapper, PlaybackPreference pref, EffectType prevTrackEffect, float trackVolume, float pitch)
		{
			AudioPlayer audioPlayer = _audioPlayerPool.Extract();
			wrapper.UpdateInstance(audioPlayer);
			audioPlayer.SetInstanceWrapper(wrapper);
			audioPlayer.SetVolume(trackVolume);
			audioPlayer.SetPitch(pitch);
			audioPlayer.SetPlaybackData(id, pref);
			audioPlayer.Play();
			if (pref.ScheduledEndTime > 0.0)
			{
				audioPlayer.SetScheduledEndTime(pref.ScheduledEndTime);
			}
			audioPlayer.SetTrackEffect(prevTrackEffect, SetEffectMode.Override);
			audioPlayer.OnPlaybackHandover = PlaybackHandover;
		}

		private void RemoveFromPreventer(AudioPlayer target)
		{
			if (!target.IsActive)
			{
				throw new InvalidOperationException("Invalid target player");
			}
			if (_combFilteringPreventer.TryGetValue(target.ID, out var value) && value == target)
			{
				_combFilteringPreventer.Remove(target.ID);
			}
		}

		private void LateUpdate()
		{
			while (_playbackQueue.Count > 0)
			{
				IPlayable playable = _playbackQueue.Dequeue();
				if (!(playable is AudioPlayer audioPlayer) || audioPlayer.ID.IsValid())
				{
					playable.Play();
				}
			}
		}

		public void Stop(BroAudioType targetType)
		{
			Stop(targetType, -1f);
		}

		public void Stop(SoundID id)
		{
			Stop(id, -1f);
		}

		public void Stop(SoundID id, float fadeTime)
		{
			StopPlayer(fadeTime, id);
		}

		public void Stop(BroAudioType targetType, float fadeTime)
		{
			targetType = targetType.ConvertEverythingFlag();
			IReadOnlyList<AudioPlayer> currentAudioPlayers = GetCurrentAudioPlayers();
			for (int num = currentAudioPlayers.Count - 1; num >= 0; num--)
			{
				AudioPlayer audioPlayer = currentAudioPlayers[num];
				if (audioPlayer.IsActive && targetType.Contains(audioPlayer.ID.ToAudioType()))
				{
					audioPlayer.Stop(fadeTime);
				}
			}
		}

		private void StopPlayer(float fadeTime, SoundID identity)
		{
			IReadOnlyList<AudioPlayer> currentAudioPlayers = GetCurrentAudioPlayers();
			for (int num = currentAudioPlayers.Count - 1; num >= 0; num--)
			{
				AudioPlayer audioPlayer = currentAudioPlayers[num];
				if (audioPlayer.IsActive && audioPlayer.ID.Equals(identity))
				{
					audioPlayer.Stop(fadeTime);
				}
			}
		}

		public void Pause(SoundID id, bool isPause)
		{
			Pause(id, -1f, isPause);
		}

		public void Pause(SoundID id, float fadeTime, bool isPause)
		{
			IReadOnlyList<AudioPlayer> currentAudioPlayers = GetCurrentAudioPlayers();
			for (int num = currentAudioPlayers.Count - 1; num >= 0; num--)
			{
				AudioPlayer audioPlayer = currentAudioPlayers[num];
				if (audioPlayer.IsActive && audioPlayer.ID.Equals(id))
				{
					PausePlayer(audioPlayer, isPause, fadeTime);
				}
			}
		}

		public void Pause(BroAudioType targetType, bool isPause)
		{
			Pause(targetType, -1f, isPause);
		}

		public void Pause(BroAudioType targetType, float fadeTime, bool isPause)
		{
			targetType = targetType.ConvertEverythingFlag();
			IReadOnlyList<AudioPlayer> currentAudioPlayers = GetCurrentAudioPlayers();
			for (int num = currentAudioPlayers.Count - 1; num >= 0; num--)
			{
				AudioPlayer audioPlayer = currentAudioPlayers[num];
				if (audioPlayer.IsActive && targetType.Contains(audioPlayer.ID.ToAudioType()))
				{
					PausePlayer(audioPlayer, isPause, fadeTime);
				}
			}
		}

		private static void PausePlayer(AudioPlayer player, bool isPause, float fadeTime)
		{
			if (isPause)
			{
				player.Pause(fadeTime);
			}
			else
			{
				player.UnPause(fadeTime);
			}
		}

		public bool TryGetPreviousPlayerFromCombFilteringPreventer(SoundID id, out AudioPlayer previousPlayer)
		{
			previousPlayer = null;
			if (_combFilteringPreventer != null)
			{
				return _combFilteringPreventer.TryGetValue(id, out previousPlayer);
			}
			return false;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Init()
		{
			GameObject gameObject = Resources.Load("SoundManager") as GameObject;
			if (gameObject == null)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Initialize failed, please check SoundManager.prefab in your Resources folder!");
				return;
			}
			gameObject.SetActive(value: false);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			if (gameObject2.TryGetComponent<SoundManager>(out var component))
			{
				_instance = component;
				UnityEngine.Object.DontDestroyOnLoad(gameObject2);
				gameObject2.SetActive(value: true);
			}
			else
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Initialize failed ,please add SoundManager component to SoundManager.prefab");
			}
		}

		private void Awake()
		{
			if (!_broAudioMixer)
			{
				Debug.LogError(string.Format("<b><color=#F3E9D7>[BroAudio] </color></b>Please assign {{0}} in SoundManager.prefab", "BroAudioMixer"));
				return;
			}
			if (!_audioPlayerPrefab)
			{
				Debug.LogError(string.Format("<b><color=#F3E9D7>[BroAudio] </color></b>Please assign {{0}} in SoundManager.prefab", "AudioPlayer"));
				return;
			}
			_audioPlayerPool = new AudioPlayerObjectPool(_audioPlayerPrefab, base.transform, Setting.DefaultAudioPlayerPoolSize);
			AudioMixerGroup[] audioMixerGroups = _broAudioMixer.FindMatchingGroups("Track");
			AudioMixerGroup[] audioMixerGroups2 = _broAudioMixer.FindMatchingGroups("Dominator");
			_audioTrackPool = new AudioTrackObjectPool(audioMixerGroups);
			_dominatorTrackPool = new AudioTrackObjectPool(audioMixerGroups2, isDominator: true);
			InitBank();
			_automationHelper = new EffectAutomationHelper(this, _broAudioMixer);
			_dominatorAutomationHelper = new EffectAutomationHelper(this, _broAudioMixer);
			_addressableCleanupCoroutine = StartCoroutine(AddressableCleanupRoutine());
		}

		private void OnDestroy()
		{
			MusicPlayer.CleanUp();
			if (_addressableCleanupCoroutine != null)
			{
				StopCoroutine(_addressableCleanupCoroutine);
				_addressableCleanupCoroutine = null;
			}
		}

		private void InitBank()
		{
			Utility.ForeachConcreteAudioType(new PlaybackPrefInitializer
			{
				AudioTypePref = _auidoTypePref
			});
		}

		public void SetVolume(float vol, BroAudioType targetType, float fadeTime)
		{
			targetType = targetType.ConvertEverythingFlag();
			switch (targetType)
			{
			case BroAudioType.None:
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"SetVolume with {targetType} is meaningless");
				return;
			case BroAudioType.All:
				SetMasterVolume(vol, fadeTime);
				return;
			}
			SetPlaybackPrefByType(targetType, vol, AudioTypePlaybackPreference.OnSetVolume);
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && targetType.Contains(currentAudioPlayer.ID.ToAudioType()))
				{
					currentAudioPlayer.SetAudioTypeVolume(vol, fadeTime);
				}
			}
		}

		private void SetMasterVolume(float targetVol, float fadeTime)
		{
			targetVol = targetVol.ToDecibel();
			if (_broAudioMixer.SafeGetFloat("Master", out var value) && value != targetVol)
			{
				if (fadeTime != 0f)
				{
					this.StartCoroutineAndReassign(SetMasterVolume(value, targetVol, fadeTime), ref _masterVolumeCoroutine);
				}
				else
				{
					_broAudioMixer.SafeSetFloat("Master", targetVol);
				}
			}
			IEnumerator SetMasterVolume(float currentVol, float num, float num2)
			{
				Ease ease = ((currentVol < num) ? FadeInEase : FadeOutEase);
				float currentTime = 0f;
				while (currentTime < num2)
				{
					yield return null;
					currentTime += Utility.GetDeltaTime();
					float value2 = Mathf.Lerp(currentVol, num, (currentTime / num2).SetEase(ease));
					_broAudioMixer.SafeSetFloat("Master", value2);
				}
			}
		}

		public void SetVolume(SoundID id, float vol, float fadeTime)
		{
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && currentAudioPlayer.ID.Equals(id))
				{
					currentAudioPlayer.SetVolume(vol, fadeTime);
				}
			}
		}

		public IAutoResetWaitable SetEffect(Effect effect)
		{
			return SetEffect(BroAudioType.All, effect);
		}

		public IAutoResetWaitable SetEffect(BroAudioType targetType, Effect effect)
		{
			targetType = targetType.ConvertEverythingFlag();
			SetEffectMode setEffectMode = SetEffectMode.Add;
			if (effect.Type == EffectType.None)
			{
				setEffectMode = SetEffectMode.Override;
			}
			else if (effect.IsDefault())
			{
				setEffectMode = SetEffectMode.Remove;
			}
			Action<EffectType> onReset = null;
			EffectAutomationHelper effectAutomationHelper = null;
			if (effect.IsDominator)
			{
				effectAutomationHelper = _dominatorAutomationHelper;
			}
			else
			{
				if (setEffectMode == SetEffectMode.Remove)
				{
					onReset = delegate(EffectType resetType)
					{
						SetPlayerEffect(targetType, resetType, SetEffectMode.Remove);
					};
				}
				else
				{
					SetPlayerEffect(targetType, effect.Type, setEffectMode);
				}
				effectAutomationHelper = _automationHelper;
			}
			effectAutomationHelper.SetEffectTrackParameter(effect, onReset);
			return effectAutomationHelper;
		}

		private void SetPlayerEffect(BroAudioType targetType, EffectType effectType, SetEffectMode mode)
		{
			AudioTypePlaybackPreference.SetEffectParameter parameter = new AudioTypePlaybackPreference.SetEffectParameter
			{
				EffectType = effectType,
				Mode = mode
			};
			SetPlaybackPrefByType(targetType, parameter, AudioTypePlaybackPreference.OnSetEffect);
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && targetType.Contains(currentAudioPlayer.ID.ToAudioType()) && !currentAudioPlayer.IsDominator)
				{
					currentAudioPlayer.SetTrackEffect(effectType, mode);
				}
			}
		}

		public void SetPitch(SoundID id, float pitch, float fadeTime)
		{
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && currentAudioPlayer.ID.Equals(id))
				{
					currentAudioPlayer.SetPitch(pitch, fadeTime);
				}
			}
		}

		public void SetPitch(float pitch, BroAudioType targetType, float fadeTime)
		{
			targetType = targetType.ConvertEverythingFlag();
			if (targetType == BroAudioType.None)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"SetPitch with {targetType} is meaningless");
				return;
			}
			SetPlaybackPrefByType(targetType, pitch, AudioTypePlaybackPreference.OnSetpitch);
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && targetType.Contains(currentAudioPlayer.ID.ToAudioType()))
				{
					currentAudioPlayer.SetPitch(pitch, fadeTime);
				}
			}
		}

		public bool TryGetAudioTypePref(BroAudioType audioType, out IAudioPlaybackPref result)
		{
			result = null;
			if (_auidoTypePref.TryGetValue(audioType, out var value))
			{
				result = value;
			}
			return result != null;
		}

		public bool HasAnyPlayingInstances(SoundID id)
		{
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && currentAudioPlayer.ID.Equals(id) && currentAudioPlayer.IsPlaying)
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetActivePlayer(SoundID id, out AudioPlayer player)
		{
			foreach (AudioPlayer currentAudioPlayer in GetCurrentAudioPlayers())
			{
				if (currentAudioPlayer.IsActive && currentAudioPlayer.ID.Equals(id))
				{
					player = currentAudioPlayer;
					return true;
				}
			}
			player = null;
			return false;
		}

		AudioMixerGroup IAudioMixerPool.GetTrack(AudioTrackType trackType)
		{
			return trackType switch
			{
				AudioTrackType.Generic => _audioTrackPool.Extract(), 
				AudioTrackType.Dominator => _dominatorTrackPool.Extract(), 
				_ => null, 
			};
		}

		void IAudioMixerPool.ReturnTrack(AudioTrackType trackType, AudioMixerGroup track)
		{
			switch (trackType)
			{
			case AudioTrackType.Generic:
				_audioTrackPool.Recycle(track);
				break;
			case AudioTrackType.Dominator:
				_dominatorTrackPool.Recycle(track);
				break;
			}
		}

		private void SetPlaybackPrefByType<TParameter>(BroAudioType targetType, TParameter parameter, Action<AudioTypePlaybackPreference, TParameter> onModifyPref) where TParameter : struct
		{
			Utility.ForeachConcreteAudioType(new PlaybackPrefSetter<TParameter>
			{
				TargetType = targetType,
				AudioTypePref = _auidoTypePref,
				OnModifyPref = onModifyPref,
				Parameter = parameter
			});
		}

		void IAudioMixerPool.ReturnPlayer(AudioPlayer player)
		{
			RemoveFromPreventer(player);
			_audioPlayerPool.Recycle(player);
		}

		private IReadOnlyList<AudioPlayer> GetCurrentAudioPlayers()
		{
			return _audioPlayerPool.GetCurrentAudioPlayers();
		}

		private IEnumerator AddressableCleanupRoutine()
		{
			if (_addressableCleanupInterval == null)
			{
				_addressableCleanupInterval = new WaitForSecondsRealtime(Mathf.Clamp(Setting.AutomaticallyUnloadUnusedAddressableAudioClipsAfter, 1f, 5f));
			}
			while (true)
			{
				yield return _addressableCleanupInterval;
				if (!Setting.AutomaticallyLoadAddressableAudioClips)
				{
					continue;
				}
				double unscaledTimeAsDouble = Time.unscaledTimeAsDouble;
				_addressableCleanupEntitiesList.AddRange(_loadedEntityLastPlayedTime.Keys);
				foreach (SoundID addressableCleanupEntities in _addressableCleanupEntitiesList)
				{
					if (!_loadedEntityLastPlayedTime.TryGetValue(addressableCleanupEntities, out var value))
					{
						continue;
					}
					if (!HasAnyPlayingInstances(addressableCleanupEntities))
					{
						if (unscaledTimeAsDouble - value > 60.0)
						{
							UnloadAddressableEntity(addressableCleanupEntities);
							_loadedEntityLastPlayedTime.Remove(addressableCleanupEntities);
						}
					}
					else
					{
						_loadedEntityLastPlayedTime[addressableCleanupEntities] = unscaledTimeAsDouble;
					}
				}
				_addressableCleanupEntitiesList.Clear();
			}
		}

		private void UnloadAddressableEntity(SoundID id)
		{
			if (TryGetEntity(id, out var entity) && entity is AudioEntity { UseAddressables: not false } audioEntity)
			{
				audioEntity.ReleaseAllAssets();
			}
		}

		public void UpdateLoadedEntityLastPlayedTime(SoundID id)
		{
			if (Setting.AutomaticallyLoadAddressableAudioClips && _loadedEntityLastPlayedTime.ContainsKey(id))
			{
				_loadedEntityLastPlayedTime[id] = Time.unscaledTimeAsDouble;
			}
		}

		[Obsolete("Only for backwards compatibility")]
		public bool TryConvertIdToEntity(int id, out AudioEntity entity)
		{
			if (id == 0 || id == -1)
			{
				entity = null;
				return false;
			}
			if (_data == null)
			{
				entity = null;
				return false;
			}
			foreach (IAudioAsset asset in _data.Assets)
			{
				if (asset is AudioAsset audioAsset && audioAsset.TryGetEntityFromId(id, out entity))
				{
					return true;
				}
			}
			Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Can't find entity with id {id}");
			entity = null;
			return false;
		}
	}
}
