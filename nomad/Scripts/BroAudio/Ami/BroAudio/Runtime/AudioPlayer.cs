using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ami.BroAudio.Data;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.BroAudio.Runtime
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("")]
	public class AudioPlayer : MonoBehaviour, IAudioPlayer, IEffectDecoratable, IVolumeSettable, IMusicDecoratable, IAudioStoppable, ISchedulable, IPlayable, IRecyclable<AudioPlayer>, IParameterizedPlayer, IAudioBus
	{
		public delegate void PlaybackHandover(SoundID id, InstanceWrapper<AudioPlayer> wrapper, PlaybackPreference pref, EffectType effectType, float trackVolume, float pitch);

		private struct AddedEffect
		{
			public Component Component;

			public IAudioEffectModifier Modifier;
		}

		private float _bindingVolumeMultiplier = 1f;

		private float _bindingPitchMultiplier = 1f;

		private float[] _bindingCurrentValues;

		private float[] _bindingTargetValues;

		private float[] _bindingVelocity;

		private int _bindingCount;

		private AudioEntity _bindingEntity;

		private float _pitchBase = 1f;

		private Coroutine _pitchCoroutine;

		private Dictionary<string, object> _parameterValues;

		private Dictionary<string, object> _pendingParameterWrites;

		private bool _exitLoopRegionRequested;

		private bool _exitLoopRegionAtBoundary;

		private AudioSource _seamSecondary;

		private float _primarySeamGain = 1f;

		private float _secondarySeamGain;

		private bool _isXfading;

		public PlaybackHandover OnPlaybackHandover;

		private PlaybackPreference _pref;

		private StopMode _stopMode;

		private Coroutine _playbackControlCoroutine;

		private InstanceWrapper<AudioPlayer> _instanceWrapper;

		private float _timeBeforeStartSchedule;

		public const float DefaultClipVolume = 0f;

		public const float DefaultTrackVolume = 1f;

		public const float UnSetMixerDecibelVolume = -3.4028235E+38f;

		private Fader _trackVolume;

		private Fader _clipVolume;

		private Fader _audioTypeVolume;

		private float _mixerDecibelVolume = -3.4028235E+38f;

		[SerializeField]
		private AudioSource AudioSource;

		private IBroAudioClip _clip;

		private List<AudioPlayerDecorator> _decorators;

		private string _sendParaName;

		private string _currTrackName;

		private IDisposable _proxy;

		private AudioFilterReader _audioFilterReader;

		private List<AddedEffect> _addedEffects;

		public float BindingVolumeMultiplier => _bindingVolumeMultiplier;

		public float BindingPitchMultiplier => _bindingPitchMultiplier;

		public float StaticPitch { get; private set; } = 1f;

		public int PlaybackStartingTime { get; private set; }

		public bool HasStartedPlaying => PlaybackStartingTime > 0;

		private bool IsOnHold
		{
			get
			{
				if (_stopMode == StopMode.Pause)
				{
					return !HasStartedPlaying;
				}
				return false;
			}
		}

		public SoundID ID { get; private set; } = SoundID.Invalid;

		public bool IsActive => ID.IsValid();

		public bool IsPlaying => AudioSource.isPlaying;

		public Vector3 PlayingPosition => _pref.Position;

		public bool IsStopping { get; private set; }

		public bool IsFadingOut { get; private set; }

		public EffectType CurrentActiveTrackEffects { get; private set; }

		public bool IsUsingTrackEffect => CurrentActiveTrackEffects != EffectType.None;

		public bool IsDominator => HasDecoratorOf<DominatorPlayer>();

		public bool IsBGM => HasDecoratorOf<MusicPlayer>();

		public IBroAudioClip CurrentPlayingClip => _clip;

		IAudioSourceProxy IAudioPlayer.AudioSource
		{
			get
			{
				if (!IsActive)
				{
					Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>The audio player is not playing! Please consider accessing the AudioSource via OnStart() or OnUpdate() methods.");
					return Empty.AudioSource;
				}
				if (_proxy == null)
				{
					_proxy = new AudioSourceProxy(AudioSource);
				}
				return _proxy as IAudioSourceProxy;
			}
		}

		private AudioTrackType TrackType { get; set; }

		private AudioMixerGroup AudioTrack
		{
			set
			{
				AudioSource.outputAudioMixerGroup = value;
				if (value == null)
				{
					_currTrackName = null;
					_sendParaName = null;
				}
			}
		}

		private string VolumeParaName
		{
			get
			{
				if (!IsUsingTrackEffect)
				{
					return GetCurrentTrackName();
				}
				return GetSendParaName();
			}
		}

		private IAudioMixerPool MixerPool => SoundManager.Instance;

		private event Action<SoundID> _onEnd;

		private event Action<IAudioPlayer> _onUpdate;

		private event Action<IAudioPlayer> _onStart;

		private event Action<IAudioPlayer> _onPaused;

		private void InitializeBindings(AudioEntity entity)
		{
			_bindingEntity = entity;
			_bindingVolumeMultiplier = 1f;
			_bindingPitchMultiplier = 1f;
			_bindingCount = (entity?.Bindings)?.Count ?? 0;
			if (_bindingCount != 0)
			{
				EnsureBindingArrays(_bindingCount);
				RecomputeBindingTargets();
				for (int i = 0; i < _bindingCount; i++)
				{
					_bindingCurrentValues[i] = _bindingTargetValues[i];
					_bindingVelocity[i] = 0f;
				}
				AggregateMultipliers();
			}
		}

		private void EnsureBindingArrays(int count)
		{
			if (_bindingCurrentValues == null || _bindingCurrentValues.Length < count)
			{
				_bindingCurrentValues = new float[count];
				_bindingTargetValues = new float[count];
				_bindingVelocity = new float[count];
			}
		}

		private void RecomputeBindingTargets()
		{
			if (_bindingEntity == null || _bindingCount == 0)
			{
				return;
			}
			IReadOnlyList<AudioParameterBinding> bindings = _bindingEntity.Bindings;
			if (bindings != null)
			{
				int num = Mathf.Min(_bindingCount, bindings.Count);
				for (int i = 0; i < num; i++)
				{
					_bindingTargetValues[i] = EvaluateBinding(_bindingEntity, bindings[i], _parameterValues);
				}
			}
		}

		public static float EvaluateBinding(AudioEntity entity, AudioParameterBinding binding, IReadOnlyDictionary<string, object> paramValues)
		{
			if (binding.Target == AudioBindingTarget.None)
			{
				return 1f;
			}
			if (entity == null || string.IsNullOrEmpty(binding.ParameterId))
			{
				return 1f;
			}
			if (!entity.TryFindParameterById(binding.ParameterId, out var definition))
			{
				return 1f;
			}
			object value = null;
			if (paramValues == null || !paramValues.TryGetValue(definition.Id, out value))
			{
				value = definition.GetDefault();
			}
			switch (definition.Type)
			{
			case AudioParameterType.Bool:
				if (!((value is bool flag) ? flag : definition.DefaultBool))
				{
					return binding.BoolFalseValue;
				}
				return binding.BoolTrueValue;
			case AudioParameterType.Int:
			{
				int num2 = ((value is int num3) ? num3 : definition.DefaultInt);
				return SampleCurveRemap(binding, num2);
			}
			case AudioParameterType.Float:
			{
				float rawInput = ((value is float num) ? num : definition.DefaultFloat);
				return SampleCurveRemap(binding, rawInput);
			}
			default:
				return 1f;
			}
		}

		private static float SampleCurveRemap(AudioParameterBinding b, float rawInput)
		{
			float num = b.InputMax - b.InputMin;
			float num2 = (Mathf.Approximately(num, 0f) ? 0f : Mathf.Clamp01((rawInput - b.InputMin) / num));
			float t = ((b.Curve != null) ? b.Curve.Evaluate(num2) : num2);
			return Mathf.LerpUnclamped(b.OutputMin, b.OutputMax, t);
		}

		private void TickBindingSmoothingAndAggregate(float deltaTime)
		{
			if (_bindingCount == 0)
			{
				return;
			}
			IReadOnlyList<AudioParameterBinding> readOnlyList = _bindingEntity?.Bindings;
			if (readOnlyList == null)
			{
				return;
			}
			int num = Mathf.Min(_bindingCount, readOnlyList.Count);
			for (int i = 0; i < num; i++)
			{
				float num2 = _bindingTargetValues[i];
				float current = _bindingCurrentValues[i];
				float smoothingTime = readOnlyList[i].SmoothingTime;
				if (smoothingTime <= 0f)
				{
					_bindingCurrentValues[i] = num2;
					_bindingVelocity[i] = 0f;
				}
				else
				{
					_bindingCurrentValues[i] = Mathf.SmoothDamp(current, num2, ref _bindingVelocity[i], smoothingTime, 1f / 0f, deltaTime);
				}
			}
			float bindingVolumeMultiplier = _bindingVolumeMultiplier;
			float bindingPitchMultiplier = _bindingPitchMultiplier;
			AggregateMultipliers();
			if (!Mathf.Approximately(bindingVolumeMultiplier, _bindingVolumeMultiplier))
			{
				UpdateVolume();
			}
			if (!Mathf.Approximately(bindingPitchMultiplier, _bindingPitchMultiplier))
			{
				ApplyPitchToSource();
			}
		}

		private void AggregateMultipliers()
		{
			if (_bindingCount == 0 || _bindingEntity == null)
			{
				_bindingVolumeMultiplier = 1f;
				_bindingPitchMultiplier = 1f;
				return;
			}
			IReadOnlyList<AudioParameterBinding> bindings = _bindingEntity.Bindings;
			if (bindings == null)
			{
				_bindingVolumeMultiplier = 1f;
				_bindingPitchMultiplier = 1f;
				return;
			}
			float num = 1f;
			float num2 = 1f;
			int num3 = Mathf.Min(_bindingCount, bindings.Count);
			for (int i = 0; i < num3; i++)
			{
				switch (bindings[i].Target)
				{
				case AudioBindingTarget.Volume:
					num *= _bindingCurrentValues[i];
					break;
				case AudioBindingTarget.Pitch:
					num2 *= _bindingCurrentValues[i];
					break;
				}
			}
			_bindingVolumeMultiplier = num;
			_bindingPitchMultiplier = num2;
		}

		private void ResetBindings()
		{
			_bindingVolumeMultiplier = 1f;
			_bindingPitchMultiplier = 1f;
			_bindingCount = 0;
			_bindingEntity = null;
			_pitchBase = 1f;
			if (_bindingCurrentValues != null)
			{
				Array.Clear(_bindingCurrentValues, 0, _bindingCurrentValues.Length);
				Array.Clear(_bindingTargetValues, 0, _bindingTargetValues.Length);
				Array.Clear(_bindingVelocity, 0, _bindingVelocity.Length);
			}
		}

		private void ApplyPitchToSource()
		{
			if (!(AudioSource == null))
			{
				float value = _pitchBase * _bindingPitchMultiplier;
				value = Mathf.Clamp(value, -3f, 3f);
				AudioSource.pitch = value;
			}
		}

		IAudioPlayer IAudioPlayer.SetPitch(float pitch, float fadeTime)
		{
			StaticPitch = pitch;
			PitchShiftingSetting pitchSetting = SoundManager.PitchSetting;
			if (pitchSetting != PitchShiftingSetting.AudioMixer && pitchSetting == PitchShiftingSetting.AudioSource)
			{
				pitch = Mathf.Clamp(pitch, -3f, 3f);
				if (fadeTime > 0f)
				{
					this.StartCoroutineAndReassign(PitchControl(pitch, fadeTime), ref _pitchCoroutine);
				}
				else
				{
					_pitchBase = pitch;
					ApplyPitchToSource();
				}
			}
			return this;
		}

		private void SetInitialPitch(IAudioEntity entity, IAudioPlaybackPref audioTypePlaybackPref)
		{
			float pitchBase = ((!Mathf.Approximately(StaticPitch, 1f)) ? entity.GetRandomValue(StaticPitch, RandomFlag.Pitch) : (Mathf.Approximately(audioTypePlaybackPref.Pitch, 1f) ? entity.GetPitch() : entity.GetRandomValue(audioTypePlaybackPref.Pitch, RandomFlag.Pitch)));
			_pitchBase = pitchBase;
			ApplyPitchToSource();
		}

		private IEnumerator PitchControl(float targetPitch, float fadeTime)
		{
			float startPitch = _pitchBase;
			float currentTime = 0f;
			while (currentTime < fadeTime)
			{
				currentTime += Utility.GetDeltaTime();
				_pitchBase = Mathf.Lerp(startPitch, targetPitch, (currentTime / fadeTime).SetEase(Ease.Linear));
				ApplyPitchToSource();
				yield return null;
			}
			_pitchBase = targetPitch;
			ApplyPitchToSource();
		}

		private void ResetPitch()
		{
			StaticPitch = 1f;
			_pitchBase = 1f;
			AudioSource.pitch = 1f;
		}

		public void SetPlaybackData(SoundID id, PlaybackPreference pref)
		{
			ID = id;
			_pref = pref;
		}

		public void Play()
		{
			if (!IsStopping && !IsOnHold && !(_pref.ScheduledStartTime > 0.0))
			{
				PlayInternal();
			}
		}

		private void PlayInternal()
		{
			if (!ID.IsValid() || _pref.Entity == null || !SoundManager.Instance.TryGetAudioTypePref(ID.ToAudioType(), out var result))
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Cannot play audio. Invalid ID:{ID} or Entity is null.");
				return;
			}
			try
			{
				this.StartCoroutineAndReassign(PlayControl(result), ref _playbackControlCoroutine);
			}
			catch (Exception exception)
			{
				ClearEvents();
				EndPlaying();
				Debug.LogException(exception);
			}
		}

		private IEnumerator PlayControl(IAudioPlaybackPref audioTypePref)
		{
			if (!Mathf.Approximately(audioTypePref.Volume, 1f) && !_audioTypeVolume.IsFading)
			{
				_audioTypeVolume.Complete(audioTypePref.Volume, updateBus: false);
			}
			_clipVolume.Complete(0f, updateBus: false);
			int sampleRate = ((_clip != null) ? _clip.GetAudioClip().frequency : 0);
			bool hasScheduledPlay = false;
			if (!HasStartedPlaying)
			{
				_clip = _pref.PickNewClip();
				if (_clip is BroAudioClip broAudioClip && broAudioClip.IsAddressablesAvailable() && !broAudioClip.IsLoaded)
				{
					if (!SoundManager.Instance.Setting.AutomaticallyLoadAddressableAudioClips)
					{
						LogNotPreloadedMessage(broAudioClip);
					}
					yield return WaitForAddressablesToLoad(broAudioClip);
				}
				AudioClip audioClip = _clip.GetAudioClip();
				sampleRate = audioClip.frequency;
				AudioSource.clip = audioClip;
				AudioSource.priority = _pref.Entity.Priority;
				SetPlayPosition(sampleRate);
				if (_pref.Entity is AudioEntity audioEntity)
				{
					bool num = audioEntity.Parameters != null && audioEntity.Parameters.Count > 0;
					bool flag = audioEntity.Bindings != null && audioEntity.Bindings.Count > 0;
					BroAudioGlobalParameters activeInstance = BroAudioGlobalParameters.ActiveInstance;
					bool flag2 = activeInstance != null && activeInstance.Parameters != null && activeInstance.Parameters.Count > 0;
					if (num || flag || flag2)
					{
						InitializeParameterState(audioEntity);
						InitializeBindings(audioEntity);
					}
				}
				SetInitialPitch(_pref.Entity, audioTypePref);
				SetSpatial(_pref);
				if (IsDominator)
				{
					TrackType = AudioTrackType.Dominator;
				}
				else
				{
					SetTrackEffect(audioTypePref.EffectType, SetEffectMode.Add);
				}
				SchedulePlayback(out hasScheduledPlay);
				if (hasScheduledPlay)
				{
					yield return WaitForScheduledStartTime();
				}
				if (_decorators.TryGetDecorator<MusicPlayer>(out var musicPlayer))
				{
					AudioSource.reverbZoneMix = 0f;
					AudioSource.priority = 0;
					musicPlayer.DoTransition(ref _pref);
					while (musicPlayer.IsWaitingForTransition)
					{
						yield return null;
					}
				}
				AudioTrack = MixerPool.GetTrack(TrackType);
			}
			do
			{
				if (!hasScheduledPlay)
				{
					StartPlaying(sampleRate);
				}
				if (!HasStartedPlaying)
				{
					PlaybackStartingTime = TimeExtension.UnscaledCurrentFrameBeganTime;
					this._onStart?.Invoke(this);
					this._onStart = null;
					this._onUpdate?.Invoke(this);
					hasScheduledPlay = false;
				}
				float num2 = _clip.Volume * _pref.Entity.GetMasterVolume();
				float elapsedTime = 0f;
				bool num3 = _pref.SkipLoopRegionIntro && _pref.Entity is AudioEntity audioEntity2 && audioEntity2.HasLoopRegion;
				if (num3)
				{
					AudioSource.timeSamples = Utility.GetSample(sampleRate, ((AudioEntity)_pref.Entity).LoopStartSeconds);
				}
				float fadeIn;
				Ease fadeInEase;
				if (num3)
				{
					_clipVolume.Complete(num2);
				}
				else if (_pref.HasFadeIn(_clip.FadeIn, out fadeIn, out fadeInEase))
				{
					_clipVolume.SetTarget(num2);
					while (_clipVolume.Update(ref elapsedTime, fadeIn, fadeInEase))
					{
						yield return null;
						if (!OnUpdate())
						{
							yield break;
						}
					}
				}
				else
				{
					_clipVolume.Complete(num2);
				}
				if (_pref.Entity is AudioEntity { HasLoopRegion: not false } audioEntity3)
				{
					if (_parameterValues == null)
					{
						InitializeParameterState(audioEntity3);
					}
					yield return RunLoopRegionPlayback(audioEntity3, sampleRate);
					EndPlaying();
					yield break;
				}
				if (_pref.IsLoop(LoopType.SeamlessLoop))
				{
					_pref.ScheduledStartTime = 0.0;
					_pref.ApplySeamlessFade();
				}
				int endSample = AudioSource.clip.samples - Utility.GetSample(sampleRate, _clip.EndPosition);
				if (_pref.HasFadeOut(_clip.FadeOut, out var fadeOut, out var fadeOutEase))
				{
					while ((float)(endSample - AudioSource.timeSamples) > fadeOut * (float)sampleRate)
					{
						yield return null;
						if (!OnUpdate())
						{
							yield break;
						}
					}
					TriggerPlaybackHandover();
					_clipVolume.SetTarget(0f);
					elapsedTime = 0f;
					IsFadingOut = true;
					while (_clipVolume.Update(ref elapsedTime, fadeOut, fadeOutEase))
					{
						yield return null;
						if (!OnUpdate())
						{
							yield break;
						}
					}
					IsFadingOut = false;
					continue;
				}
				bool hasPlayed = false;
				while (!HasEndPlaying(ref hasPlayed, endSample, sampleRate))
				{
					yield return null;
					if (!OnUpdate())
					{
						yield break;
					}
				}
				TriggerPlaybackHandover();
			}
			while (_pref.IsLoop(LoopType.Loop) && CanLoopIfIsChainedMode());
			EndPlaying();
		}

		private void StartPlaying(int sampleRate)
		{
			switch (_stopMode)
			{
			case StopMode.Pause:
				if (HasStartedPlaying)
				{
					AudioSource.UnPause();
					break;
				}
				goto case StopMode.Stop;
			case StopMode.Mute:
				if (AudioSource.isPlaying)
				{
					break;
				}
				goto case StopMode.Stop;
			case StopMode.Stop:
				PlayFromPos(sampleRate);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			_stopMode = StopMode.Stop;
		}

		private void PlayFromPos(int sampleRate)
		{
			SetPlayPosition(sampleRate);
			AudioSource.Play();
		}

		private void SetPlayPosition(int sampleRate)
		{
			AudioSource.Stop();
			AudioSource.timeSamples = Utility.GetSample(sampleRate, _clip.StartPosition);
		}

		private bool HasEndPlaying(ref bool hasPlayed, int endSample, int sampleRate)
		{
			int timeSamples = AudioSource.timeSamples;
			int sample = Utility.GetSample(sampleRate, _clip.StartPosition);
			if (!hasPlayed)
			{
				hasPlayed = timeSamples > sample;
			}
			if (hasPlayed)
			{
				if (timeSamples > sample)
				{
					return timeSamples >= endSample;
				}
				return true;
			}
			return false;
		}

		private void TriggerPlaybackHandover(bool isEnd = false)
		{
			if ((!isEnd || _pref.CanHandoverToEnd()) && (isEnd || _pref.CanHandoverToLoop()))
			{
				PlaybackPreference pref = _pref;
				if (pref.IsChainedMode())
				{
					pref.ChainedModeStage = (isEnd ? PlaybackStage.End : PlaybackStage.Loop);
				}
				ClearScheduleEndEvents();
				OnPlaybackHandover?.Invoke(ID, _instanceWrapper, pref, CurrentActiveTrackEffects, _trackVolume.Target, StaticPitch);
				OnPlaybackHandover = null;
				_instanceWrapper = null;
			}
		}

		void IAudioStoppable.Pause()
		{
			this.Pause(-1f);
		}

		void IAudioStoppable.Pause(float fadeOut)
		{
			Stop(fadeOut, StopMode.Pause, null);
		}

		void IAudioStoppable.UnPause()
		{
			this.UnPause(-1f);
		}

		void IAudioStoppable.UnPause(float fadeIn)
		{
			if (_stopMode != StopMode.Pause)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Cannot UnPause: The player is not paused. Sound:{ID}", this);
				return;
			}
			_pref.SetNextFadeIn(fadeIn);
			PlayInternal();
		}

		void IAudioStoppable.Stop()
		{
			this.Stop(-1f);
		}

		void IAudioStoppable.Stop(float fadeOut)
		{
			this.Stop(fadeOut, null);
		}

		void IAudioStoppable.Stop(Action onFinished)
		{
			this.Stop(-1f, onFinished);
		}

		void IAudioStoppable.Stop(float fadeOut, Action onFinished)
		{
			Stop(fadeOut, StopMode.Stop, onFinished);
		}

		public void Stop(float overrideFade, StopMode stopMode, Action onFinished)
		{
			if (IsStopping && !Mathf.Approximately(overrideFade, 0f))
			{
				return;
			}
			bool isPlaying = AudioSource.isPlaying;
			if (stopMode == StopMode.Pause && !isPlaying)
			{
				bool num = _stopMode == StopMode.Pause;
				_stopMode = StopMode.Pause;
				if (!num)
				{
					this._onPaused?.Invoke(this);
				}
			}
			else if (!ID.IsValid() || !isPlaying)
			{
				onFinished?.Invoke();
				EndPlaying();
			}
			else
			{
				this.StartCoroutineAndReassign(StopControl(overrideFade, stopMode, onFinished), ref _playbackControlCoroutine);
			}
		}

		private IEnumerator StopControl(float overrideFade, StopMode stopMode, Action onFinished)
		{
			_stopMode = stopMode;
			IsStopping = true;
			_pref.SetNextFadeOut(overrideFade);
			TriggerPlaybackHandover(isEnd: true);
			if (_pref.HasFadeOut(_clip.FadeOut, out var fadeOut, out var fadeOutEase))
			{
				if (IsFadingOut)
				{
					AudioClip clip = AudioSource.clip;
					float endSample = (float)clip.samples - _clip.EndPosition * (float)clip.frequency;
					while ((float)AudioSource.timeSamples < endSample)
					{
						yield return null;
						if (!OnUpdate())
						{
							yield break;
						}
					}
				}
				else
				{
					float endSample = 0f;
					_clipVolume.SetTarget(0f);
					while (_clipVolume.Update(ref endSample, fadeOut, fadeOutEase))
					{
						yield return null;
						if (!OnUpdate())
						{
							yield break;
						}
					}
				}
			}
			switch (stopMode)
			{
			case StopMode.Stop:
				EndPlaying();
				break;
			case StopMode.Pause:
				AudioSource.Pause();
				this._onPaused?.Invoke(this);
				break;
			case StopMode.Mute:
				this.SetVolume(0f);
				break;
			}
			IsStopping = false;
			onFinished?.Invoke();
		}

		private bool OnUpdate()
		{
			this._onUpdate?.Invoke(this);
			return IsActive;
		}

		private void EndPlaying()
		{
			PlaybackStartingTime = 0;
			_stopMode = StopMode.Stop;
			_pref = default(PlaybackPreference);
			IsFadingOut = false;
			IsStopping = false;
			_parameterValues?.Clear();
			_pendingParameterWrites?.Clear();
			_exitLoopRegionRequested = false;
			_exitLoopRegionAtBoundary = false;
			ResetBindings();
			ResetVolume();
			ResetPitch();
			AudioSource.Stop();
			AudioSource.clip = null;
			_clip = null;
			ResetSpatial();
			ResetEffect();
			_trackVolume.StopCoroutine();
			_audioTypeVolume.StopCoroutine();
			this._onEnd?.Invoke(ID);
			this._onEnd = null;
			Recycle();
		}

		private bool CanLoopIfIsChainedMode()
		{
			if (_pref.IsChainedMode())
			{
				if (_pref.IsChainedMode())
				{
					return _pref.ChainedModeStage == PlaybackStage.Loop;
				}
				return false;
			}
			return true;
		}

		private void InitializeParameterState(AudioEntity entity)
		{
			_exitLoopRegionRequested = false;
			_exitLoopRegionAtBoundary = false;
			IReadOnlyList<AudioParameterDefinition> parameters = entity.Parameters;
			BroAudioGlobalParameters activeInstance = BroAudioGlobalParameters.ActiveInstance;
			IReadOnlyList<AudioParameterDefinition> readOnlyList = ((activeInstance != null) ? activeInstance.Parameters : null);
			int num = parameters?.Count ?? 0;
			int num2 = readOnlyList?.Count ?? 0;
			if (num == 0 && num2 == 0)
			{
				_parameterValues = null;
				return;
			}
			if (_parameterValues == null)
			{
				_parameterValues = new Dictionary<string, object>(num + num2);
			}
			else
			{
				_parameterValues.Clear();
			}
			for (int i = 0; i < num2; i++)
			{
				AudioParameterDefinition audioParameterDefinition = readOnlyList[i];
				if (!string.IsNullOrEmpty(audioParameterDefinition.Id))
				{
					_parameterValues[audioParameterDefinition.Id] = audioParameterDefinition.GetDefault();
				}
			}
			for (int j = 0; j < num; j++)
			{
				AudioParameterDefinition audioParameterDefinition2 = parameters[j];
				if (!string.IsNullOrEmpty(audioParameterDefinition2.Id))
				{
					_parameterValues[audioParameterDefinition2.Id] = audioParameterDefinition2.GetDefault();
				}
			}
			if (_pendingParameterWrites == null || _pendingParameterWrites.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<string, object> pendingParameterWrite in _pendingParameterWrites)
			{
				if (_parameterValues.ContainsKey(pendingParameterWrite.Key))
				{
					_parameterValues[pendingParameterWrite.Key] = pendingParameterWrite.Value;
				}
			}
			_pendingParameterWrites.Clear();
		}

		private IEnumerator RunLoopRegionPlayback(AudioEntity entity, int sampleRate)
		{
			int num = ((AudioSource.clip != null) ? AudioSource.clip.samples : 0);
			int sample = Utility.GetSample(sampleRate, _clip.StartPosition);
			int trimEndSample = num - Utility.GetSample(sampleRate, _clip.EndPosition);
			int loopStartSample = Utility.GetSample(sampleRate, entity.LoopStartSeconds);
			int loopEndSample = Utility.GetSample(sampleRate, entity.LoopEndSeconds);
			loopStartSample = Mathf.Clamp(loopStartSample, sample, trimEndSample);
			loopEndSample = Mathf.Clamp(loopEndSample, loopStartSample, trimEndSample);
			if (num <= 0 || loopEndSample <= loopStartSample || loopEndSample > trimEndSample)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Invalid loop region on '" + entity.Name + "': " + $"start={entity.LoopStartSeconds}s end={entity.LoopEndSeconds}s " + $"trim=[{(float)sample / (float)sampleRate:0.000}s .. {(float)trimEndSample / (float)sampleRate:0.000}s]");
				yield break;
			}
			if (_pref.SkipLoopRegionIntro && AudioSource.timeSamples < loopStartSample)
			{
				AudioSource.timeSamples = loopStartSample;
			}
			float num2 = Mathf.Max(0f, entity.LoopSeamCrossfadeSeconds);
			int xfadeSamples = Mathf.RoundToInt(num2 * (float)sampleRate);
			int num3 = loopEndSample - loopStartSample;
			xfadeSamples = Mathf.Clamp(xfadeSamples, 0, num3 / 2);
			int xfadeStartSample = loopEndSample - xfadeSamples;
			bool useCrossfade = xfadeSamples > 0;
			while (!_exitLoopRegionRequested && !IsStopping)
			{
				EvaluateTransitions(entity);
				if (_exitLoopRegionRequested)
				{
					break;
				}
				int timeSamples = AudioSource.timeSamples;
				if (useCrossfade && !_isXfading && timeSamples >= xfadeStartSample && !_exitLoopRegionAtBoundary)
				{
					BeginSeamCrossfade(loopStartSample);
				}
				if (_isXfading)
				{
					int num4 = timeSamples - xfadeStartSample;
					float num5 = ((xfadeSamples > 0) ? Mathf.Clamp01((float)num4 / (float)xfadeSamples) : 1f);
					ComputeSeamGains(entity.LoopSeamCrossfadeCurve, num5, out _primarySeamGain, out _secondarySeamGain);
					UpdateVolume();
					if (timeSamples >= loopEndSample || num5 >= 1f)
					{
						FinishSeamCrossfadeAndSwap();
					}
				}
				else if (timeSamples >= loopEndSample)
				{
					if (_exitLoopRegionAtBoundary)
					{
						break;
					}
					int num6 = timeSamples - loopEndSample;
					int num7 = loopEndSample - loopStartSample;
					if (num7 > 0 && num6 >= num7)
					{
						num6 %= num7;
					}
					AudioSource.timeSamples = loopStartSample + num6;
				}
				yield return null;
				if (!OnUpdate())
				{
					EndSeamCrossfadeImmediate();
					yield break;
				}
			}
			EndSeamCrossfadeImmediate();
			int outroEndSample = trimEndSample;
			if (_pref.HasFadeOut(_clip.FadeOut, out var fadeOut, out var fadeOutEase))
			{
				while ((float)(outroEndSample - AudioSource.timeSamples) > fadeOut * (float)sampleRate)
				{
					yield return null;
					if (!OnUpdate())
					{
						yield break;
					}
				}
				_clipVolume.SetTarget(0f);
				float elapsedTime = 0f;
				IsFadingOut = true;
				while (_clipVolume.Update(ref elapsedTime, fadeOut, fadeOutEase))
				{
					yield return null;
					if (!OnUpdate())
					{
						yield break;
					}
				}
				IsFadingOut = false;
				yield break;
			}
			while (AudioSource.timeSamples < outroEndSample && AudioSource.isPlaying)
			{
				yield return null;
				if (!OnUpdate())
				{
					break;
				}
			}
		}

		private void BeginSeamCrossfade(int loopStartSample)
		{
			EnsureSeamSecondary();
			if (!(_seamSecondary == null))
			{
				_seamSecondary.Stop();
				_seamSecondary.clip = AudioSource.clip;
				_seamSecondary.outputAudioMixerGroup = AudioSource.outputAudioMixerGroup;
				_seamSecondary.pitch = AudioSource.pitch;
				_seamSecondary.spatialBlend = AudioSource.spatialBlend;
				_seamSecondary.bypassEffects = AudioSource.bypassEffects;
				_seamSecondary.bypassListenerEffects = AudioSource.bypassListenerEffects;
				_seamSecondary.bypassReverbZones = AudioSource.bypassReverbZones;
				_seamSecondary.panStereo = AudioSource.panStereo;
				_seamSecondary.dopplerLevel = AudioSource.dopplerLevel;
				_seamSecondary.minDistance = AudioSource.minDistance;
				_seamSecondary.maxDistance = AudioSource.maxDistance;
				_seamSecondary.rolloffMode = AudioSource.rolloffMode;
				_seamSecondary.reverbZoneMix = AudioSource.reverbZoneMix;
				_seamSecondary.spread = AudioSource.spread;
				_seamSecondary.timeSamples = loopStartSample;
				_seamSecondary.volume = 0f;
				_seamSecondary.loop = false;
				_seamSecondary.Play();
				_isXfading = true;
				_primarySeamGain = 1f;
				_secondarySeamGain = 0f;
			}
		}

		private void FinishSeamCrossfadeAndSwap()
		{
			if (!(_seamSecondary == null))
			{
				AudioSource audioSource = AudioSource;
				AudioSource seamSecondary = _seamSecondary;
				if (audioSource != null)
				{
					audioSource.Stop();
				}
				AudioSource = seamSecondary;
				_seamSecondary = audioSource;
				_isXfading = false;
				_primarySeamGain = 1f;
				_secondarySeamGain = 0f;
				UpdateVolume();
			}
		}

		private void EndSeamCrossfadeImmediate()
		{
			if (_isXfading && _seamSecondary != null)
			{
				_seamSecondary.Stop();
				_seamSecondary.volume = 0f;
			}
			_isXfading = false;
			_primarySeamGain = 1f;
			_secondarySeamGain = 0f;
			if (HasStartedPlaying)
			{
				UpdateVolume();
			}
		}

		private void EnsureSeamSecondary()
		{
			if (!(_seamSecondary != null))
			{
				_seamSecondary = base.gameObject.AddComponent<AudioSource>();
				_seamSecondary.playOnAwake = false;
				_seamSecondary.loop = false;
				_seamSecondary.volume = 0f;
			}
		}

		public static void ComputeSeamGains(LoopSeamCrossfadeCurve curve, float tau, out float primaryGain, out float secondaryGain)
		{
			tau = Mathf.Clamp01(tau);
			if (curve != LoopSeamCrossfadeCurve.Linear && curve == LoopSeamCrossfadeCurve.EqualPower)
			{
				primaryGain = Mathf.Cos(tau * (float)Math.PI * 0.5f);
				secondaryGain = Mathf.Sin(tau * (float)Math.PI * 0.5f);
			}
			else
			{
				primaryGain = 1f - tau;
				secondaryGain = tau;
			}
		}

		private void EvaluateTransitions(AudioEntity entity)
		{
			if (_parameterValues == null)
			{
				return;
			}
			IReadOnlyList<AudioTransition> transitions = entity.Transitions;
			if (transitions == null || transitions.Count == 0)
			{
				return;
			}
			for (int i = 0; i < transitions.Count; i++)
			{
				AudioTransition audioTransition = transitions[i];
				if (string.IsNullOrEmpty(audioTransition.ParameterId) || !_parameterValues.TryGetValue(audioTransition.ParameterId, out var value) || !entity.TryFindParameterById(audioTransition.ParameterId, out var definition) || !audioTransition.Evaluate(value, definition.Type) || audioTransition.Type != AudioTransitionType.ExitLoopRegion)
				{
					continue;
				}
				if (audioTransition.Timing == AudioTransitionTiming.Instant)
				{
					_exitLoopRegionRequested = true;
					if (AudioSource != null && AudioSource.clip != null)
					{
						int sample = Utility.GetSample(AudioSource.clip.frequency, entity.LoopEndSeconds);
						if (AudioSource.timeSamples < sample)
						{
							AudioSource.timeSamples = sample;
						}
					}
					break;
				}
				_exitLoopRegionAtBoundary = true;
			}
		}

		public void SetSkipLoopRegionIntro(bool skipIntro)
		{
			_pref.SkipLoopRegionIntro = skipIntro;
		}

		public void SetParameter(string name, bool value)
		{
			SetParameterInternal(name, value, AudioParameterType.Bool);
		}

		public void SetParameter(string name, int value)
		{
			SetParameterInternal(name, value, AudioParameterType.Int);
		}

		public void SetParameter(string name, float value)
		{
			SetParameterInternal(name, value, AudioParameterType.Float);
		}

		public void RequestLoopRegionExit()
		{
			_exitLoopRegionRequested = true;
			if (_pref.Entity is AudioEntity { HasLoopRegion: not false } audioEntity && AudioSource != null && AudioSource.clip != null)
			{
				int sample = Utility.GetSample(AudioSource.clip.frequency, audioEntity.LoopEndSeconds);
				if (AudioSource.timeSamples < sample)
				{
					AudioSource.timeSamples = sample;
				}
			}
		}

		private void SetParameterInternal(string idOrName, object value, AudioParameterType valueType)
		{
			if (string.IsNullOrEmpty(idOrName) || !(_pref.Entity is AudioEntity audioEntity))
			{
				return;
			}
			if (!audioEntity.TryFindParameter(idOrName, out var definition))
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"SetParameter: parameter '{idOrName}' is not defined on entity for SoundID={ID}.");
			}
			else if (definition.Type != valueType)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"SetParameter: parameter '{definition.Name}' is type {definition.Type} but received {valueType}; ignored.");
			}
			else if (_parameterValues == null)
			{
				if (_pendingParameterWrites == null)
				{
					_pendingParameterWrites = new Dictionary<string, object>(1);
				}
				_pendingParameterWrites[definition.Id] = value;
			}
			else
			{
				_parameterValues[definition.Id] = value;
				RecomputeBindingTargets();
			}
		}

		public bool TryGetParameter(string name, out bool value)
		{
			if (TryGetParameterBoxed(name, AudioParameterType.Bool, out var value2))
			{
				value = (bool)value2;
				return true;
			}
			value = false;
			return false;
		}

		public bool TryGetParameter(string name, out int value)
		{
			if (TryGetParameterBoxed(name, AudioParameterType.Int, out var value2))
			{
				value = (int)value2;
				return true;
			}
			value = 0;
			return false;
		}

		public bool TryGetParameter(string name, out float value)
		{
			if (TryGetParameterBoxed(name, AudioParameterType.Float, out var value2))
			{
				value = (float)value2;
				return true;
			}
			value = 0f;
			return false;
		}

		private bool TryGetParameterBoxed(string idOrName, AudioParameterType expectedType, out object value)
		{
			value = null;
			if (string.IsNullOrEmpty(idOrName) || _parameterValues == null)
			{
				return false;
			}
			if (!(_pref.Entity is AudioEntity audioEntity))
			{
				return false;
			}
			if (!audioEntity.TryFindParameter(idOrName, out var definition))
			{
				return false;
			}
			if (definition.Type != expectedType)
			{
				return false;
			}
			return _parameterValues.TryGetValue(definition.Id, out value);
		}

		public IAudioPlayer OnEnd(Action<SoundID> onEnd)
		{
			_onEnd -= onEnd;
			_onEnd += onEnd;
			return this;
		}

		public IAudioPlayer OnUpdate(Action<IAudioPlayer> onUpdate)
		{
			_onUpdate -= onUpdate;
			_onUpdate += onUpdate;
			return this;
		}

		public IAudioPlayer OnStart(Action<IAudioPlayer> onStart)
		{
			_onStart -= onStart;
			_onStart += onStart;
			return this;
		}

		public IAudioPlayer OnPause(Action<IAudioPlayer> onPause)
		{
			_onPaused -= onPause;
			_onPaused += onPause;
			return this;
		}

		public IAudioPlayer SetFadeInEase(Ease ease)
		{
			_pref.SetFadeInEase(ease);
			return this;
		}

		public IAudioPlayer SetFadeOutEase(Ease ease)
		{
			_pref.SetFadeOutEase(ease);
			return this;
		}

		private void LogNotPreloadedMessage(BroAudioClip broAudioClip)
		{
			Utility.Log(broAudioClip.IsLoading ? ("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Entity: '{ID}' is still loading. You should wait for it to finish before playback, or it <b>may have caused a playback delay</b>.") : ("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Entity: '{ID}' is marked as Addressables but was not preloaded — it will be loaded on demand and <b>may have caused a playback delay</b>.\n" + "Call BroAudio.LoadAssetAsync() before playback, or enable <b>Automatically Load Addressable Audio Clips</b> in Preferences to suppress this error."), SoundManager.Instance.Setting.AddressablesNonPreloadedLogLevel);
		}

		private IEnumerator WaitForAddressablesToLoad(BroAudioClip broAudioClip)
		{
			if (broAudioClip.IsLoading)
			{
				yield return broAudioClip.GetCurrentOperationHandle();
			}
			else
			{
				yield return broAudioClip.LoadAssetAsync();
				if (_clip.GetAudioClip() == null)
				{
					Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Failed to load addressable audio clip for {ID}");
				}
			}
			SoundManager.Instance.UpdateLoadedEntityLastPlayedTime(ID);
		}

		internal void SetInstanceWrapper(InstanceWrapper<AudioPlayer> instance)
		{
			_instanceWrapper = instance;
		}

		internal IAudioPlayer GetInstanceWrapper()
		{
			return _instanceWrapper as IAudioPlayer;
		}

		public void Recycle()
		{
			ResetAudioSource();
			ResetSeamCrossfade();
			DestroyAudioFilterReader();
			DestroyAddedEffectComponents();
			ClearEvents();
			if (TryGetMixerAndTrack(out var _, out var track))
			{
				MixerPool.ReturnTrack(TrackType, track);
				TrackType = AudioTrackType.Generic;
			}
			MixerPool.ReturnPlayer(this);
			if (_decorators != null)
			{
				foreach (AudioPlayerDecorator decorator in _decorators)
				{
					decorator.Recycle();
				}
			}
			_decorators = null;
			_instanceWrapper?.Recycle();
			_instanceWrapper = null;
			OnPlaybackHandover = null;
			AudioTrack = null;
			ID = SoundID.Invalid;
		}

		private void ClearEvents()
		{
			this._onStart = null;
			this._onUpdate = null;
			this._onPaused = null;
			this._onEnd = null;
		}

		private void ResetAudioSource()
		{
			if (!AudioSource)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b> AudioSource is missing!");
				return;
			}
			_proxy?.Dispose();
			_proxy = null;
		}

		private void ResetSeamCrossfade()
		{
			if (_seamSecondary != null)
			{
				_seamSecondary.Stop();
				_seamSecondary.clip = null;
				_seamSecondary.volume = 0f;
			}
			_isXfading = false;
			_primarySeamGain = 1f;
			_secondarySeamGain = 0f;
		}

		private void DestroyAudioFilterReader()
		{
			if ((bool)_audioFilterReader)
			{
				UnityEngine.Object.Destroy(_audioFilterReader);
			}
		}

		private void DestroyAddedEffectComponents()
		{
			if (_addedEffects == null)
			{
				return;
			}
			foreach (AddedEffect addedEffect in _addedEffects)
			{
				if ((bool)addedEffect.Component)
				{
					UnityEngine.Object.Destroy(addedEffect.Component);
				}
			}
			_addedEffects = null;
		}

		private void SchedulePlayback(out bool hasScheduledPlay)
		{
			hasScheduledPlay = false;
			if (_pref.ScheduledStartTime > 0.0)
			{
				AudioSource.PlayScheduled(_pref.ScheduledStartTime);
				_timeBeforeStartSchedule = (float)(_pref.ScheduledStartTime - AudioSettings.dspTime);
				hasScheduledPlay = true;
			}
			else if (_clip.Delay > 0f)
			{
				AudioSource.PlayDelayed(_clip.Delay);
				_timeBeforeStartSchedule = _clip.Delay;
				hasScheduledPlay = true;
			}
			if (_pref.ScheduledEndTime > 0.0)
			{
				AudioSource.SetScheduledEndTime(_pref.ScheduledEndTime);
			}
		}

		IAudioPlayer ISchedulable.SetScheduledStartTime(double dspTime)
		{
			if (_pref.ScheduledStartTime > 0.0)
			{
				_timeBeforeStartSchedule += (float)(dspTime - _pref.ScheduledStartTime);
			}
			_pref.ScheduledStartTime = dspTime;
			if (AudioSource.isPlaying)
			{
				AudioSource.SetScheduledStartTime(dspTime);
			}
			else
			{
				PlayInternal();
			}
			return this;
		}

		IAudioPlayer ISchedulable.SetScheduledEndTime(double dspTime)
		{
			_pref.ScheduledEndTime = dspTime;
			_onUpdate -= CheckScheduledEnd;
			_onUpdate += CheckScheduledEnd;
			if (AudioSource.isPlaying)
			{
				AudioSource.SetScheduledEndTime(dspTime);
			}
			return this;
		}

		private void CheckScheduledEnd(IAudioPlayer player)
		{
			if (!AudioSource.isPlaying)
			{
				this.SafeStopCoroutine(_playbackControlCoroutine);
				EndPlaying();
				_onUpdate -= CheckScheduledEnd;
			}
		}

		IAudioPlayer ISchedulable.SetDelay(float delay)
		{
			return this.SetScheduledStartTime(AudioSettings.dspTime + (double)delay);
		}

		private IEnumerator WaitForScheduledStartTime()
		{
			while (_timeBeforeStartSchedule > 0f)
			{
				yield return null;
				_timeBeforeStartSchedule -= Utility.GetDeltaTime();
			}
		}

		private void ClearScheduleEndEvents()
		{
			_onUpdate -= CheckScheduledEnd;
		}

		private bool TryGetMixerDecibelVolume(out float vol)
		{
			if (_mixerDecibelVolume == -3.4028235E+38f && TryGetMixerAndTrack(out var mixer, out var _) && mixer.SafeGetFloat(VolumeParaName, out var value))
			{
				_mixerDecibelVolume = value;
			}
			vol = _mixerDecibelVolume;
			return vol > -3.4028235E+38f;
		}

		private bool TrySetMixerDecibelVolume(float vol)
		{
			if (TryGetMixerAndTrack(out var mixer, out var _))
			{
				_mixerDecibelVolume = vol.ClampDecibel(allowBoost: true);
				return mixer.SafeSetFloat(VolumeParaName, _mixerDecibelVolume);
			}
			return false;
		}

		private void InitVolumeModule()
		{
			_trackVolume = new Fader(1f, this);
			_clipVolume = new Fader(0f, this);
			_audioTypeVolume = new Fader(1f, this);
		}

		public void UpdateVolume()
		{
			if (HasStartedPlaying)
			{
				float num = _clipVolume.Current * _trackVolume.Current * _audioTypeVolume.Current * _bindingVolumeMultiplier;
				bool flag = TrySetMixerDecibelVolume(num.ToDecibel());
				if (flag)
				{
					AudioSource.volume = Mathf.Clamp01(_primarySeamGain);
				}
				else
				{
					AudioSource.volume = (num * _primarySeamGain).ClampNormalize();
				}
				if (_seamSecondary != null && _seamSecondary.isPlaying)
				{
					_seamSecondary.volume = (flag ? Mathf.Clamp01(_secondarySeamGain) : (num * _secondarySeamGain).ClampNormalize());
				}
			}
		}

		IAudioPlayer IVolumeSettable.SetVolume(float vol, float fadeTime)
		{
			SetVolumeInternal(_trackVolume, vol, fadeTime);
			return this;
		}

		public void SetAudioTypeVolume(float vol, float fadeTime)
		{
			SetVolumeInternal(_audioTypeVolume, vol, fadeTime);
		}

		private void SetVolumeInternal(Fader module, float vol, float fadeTime)
		{
			module.SetTarget(vol);
			if (fadeTime > 0f)
			{
				Ease ease = ((module.Current < vol) ? SoundManager.FadeInEase : SoundManager.FadeOutEase);
				module.StartCoroutineAndReassign(Fade(module, fadeTime, ease));
			}
			else
			{
				module.Complete(vol);
			}
		}

		private IEnumerator Fade(Fader volume, float fadeTime, Ease ease)
		{
			float elapsedTime = 0f;
			while (volume.Update(ref elapsedTime, fadeTime, ease))
			{
				yield return null;
			}
		}

		private void ResetVolume()
		{
			_clipVolume.Complete(0f, updateBus: false);
			_trackVolume.Complete(1f, updateBus: false);
			_audioTypeVolume.Complete(1f, updateBus: false);
			UpdateVolume();
		}

		private bool TryGetMixerAndTrack(out AudioMixer mixer, out AudioMixerGroup track)
		{
			track = AudioSource.outputAudioMixerGroup;
			mixer = track?.audioMixer;
			if ((object)mixer != null)
			{
				return (object)track != null;
			}
			return false;
		}

		protected virtual void Awake()
		{
			if ((object)AudioSource == null)
			{
				AudioSource = GetComponent<AudioSource>();
			}
			InitVolumeModule();
		}

		private void Update()
		{
			if (IsActive)
			{
				if (_pref.HasFollowTarget(out var target))
				{
					base.transform.position = target.position;
				}
				TickBindingSmoothingAndAggregate(Time.deltaTime);
			}
		}

		private void SetSpatial(PlaybackPreference pref)
		{
			SpatialSetting setting = pref.Entity.SpatialSetting;
			SetSpatialBlend();
			if (setting == null)
			{
				return;
			}
			AudioSource.panStereo = setting.StereoPan;
			AudioSource.dopplerLevel = setting.DopplerLevel;
			AudioSource.minDistance = setting.MinDistance;
			AudioSource.maxDistance = setting.MaxDistance;
			AudioSource.SetCustomCurveOrResetDefault(setting.ReverbZoneMix, AudioSourceCurveType.ReverbZoneMix);
			AudioSource.SetCustomCurveOrResetDefault(setting.Spread, AudioSourceCurveType.Spread);
			AudioSource.rolloffMode = setting.RolloffMode;
			if (setting.RolloffMode == AudioRolloffMode.Custom)
			{
				AudioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, setting.CustomRolloff);
			}
			if (setting.HasLowPassFilter && setting.LowpassLevelCustomCurve != null && _addedEffects == null)
			{
				this.AddLowPassEffect(delegate(IAudioLowPassFilterProxy x)
				{
					x.customCutoffCurve = setting.LowpassLevelCustomCurve;
				});
			}
			void SetSpatialBlend()
			{
				Vector3 position;
				if (pref.HasFollowTarget(out var target))
				{
					base.transform.position = target.position;
					SetTo3D();
				}
				else if (pref.HasSpecifiedPosition(out position))
				{
					base.transform.position = position;
					SetTo3D();
				}
			}
			void SetTo3D()
			{
				if (setting != null && !setting.SpatialBlend.IsDefaultCurve(0f))
				{
					AudioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend, setting.SpatialBlend);
				}
				else
				{
					AudioSource.spatialBlend = 1f;
				}
			}
		}

		private void ResetSpatial()
		{
			AudioSource.spatialBlend = 0f;
			base.transform.position = Vector3.zero;
			AudioSource.panStereo = 0f;
			AudioSource.dopplerLevel = 1f;
			AudioSource.minDistance = 1f;
			AudioSource.maxDistance = 500f;
			AudioSource.reverbZoneMix = 1f;
			AudioSource.spread = 0f;
			AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		IAudioPlayer IAudioPlayer.SetVelocity(int velocity)
		{
			_pref.SetVelocity(velocity);
			return this;
		}

		public void SetTrackEffect(EffectType effect, SetEffectMode mode)
		{
			if (ID.IsValid() && (effect != EffectType.None || mode == SetEffectMode.Override) && TryGetMixerAndTrack(out var mixer, out var _) && TryGetMixerDecibelVolume(out var vol))
			{
				bool isUsingTrackEffect = IsUsingTrackEffect;
				switch (mode)
				{
				case SetEffectMode.Add:
					CurrentActiveTrackEffects |= effect;
					break;
				case SetEffectMode.Remove:
					CurrentActiveTrackEffects &= ~effect;
					break;
				case SetEffectMode.Override:
					CurrentActiveTrackEffects = effect;
					break;
				}
				bool isUsingTrackEffect2 = IsUsingTrackEffect;
				if (isUsingTrackEffect != isUsingTrackEffect2)
				{
					string text = (IsUsingTrackEffect ? GetCurrentTrackName() : GetSendParaName());
					string to = (IsUsingTrackEffect ? GetSendParaName() : GetCurrentTrackName());
					mixer.ChangeChannel(text, to, vol);
				}
			}
		}

		private void ResetEffect()
		{
			if (IsUsingTrackEffect && TryGetMixerAndTrack(out var mixer, out var _))
			{
				mixer.SafeSetFloat(GetSendParaName(), -80f);
			}
			CurrentActiveTrackEffects = EffectType.None;
		}

		private string GetSendParaName()
		{
			if (IsUsingTrackEffect && _sendParaName == null)
			{
				_sendParaName = GetCurrentTrackName() + "_Effect";
			}
			return _sendParaName ?? string.Empty;
		}

		private string GetCurrentTrackName()
		{
			if (_currTrackName == null)
			{
				_currTrackName = AudioSource.outputAudioMixerGroup.name;
			}
			return _currTrackName ?? string.Empty;
		}

		IMusicPlayer IMusicDecoratable.AsBGM()
		{
			return Utility.GetOrCreateDecorator(ref _decorators, () => new MusicPlayer(this));
		}

		IPlayerEffect IEffectDecoratable.AsDominator()
		{
			return Utility.GetOrCreateDecorator(ref _decorators, () => new DominatorPlayer(this));
		}

		public void GetOutputData(float[] samples, int channels)
		{
			AudioSource.GetOutputData(samples, channels);
		}

		public void GetSpectrumData(float[] samples, int channels, FFTWindow window)
		{
			AudioSource.GetSpectrumData(samples, channels, window);
		}

		IAudioPlayer IAudioPlayer.OnAudioFilterRead(Action<float[], int> onAudioFilterRead)
		{
			if (!_audioFilterReader)
			{
				_audioFilterReader = base.gameObject.AddComponent<AudioFilterReader>();
			}
			_audioFilterReader.OnTriggerAudioFilterRead = onAudioFilterRead;
			return this;
		}

		IAudioPlayer IAudioPlayer.AddAudioEffect<T, TProxy>(Action<TProxy> onSet)
		{
			if (!IsActive)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Cannot add " + typeof(T).Name + " to inactive audio player!");
				return this;
			}
			if (_addedEffects != null && _addedEffects.Any((AddedEffect x) => x.Component is T))
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>Effect " + typeof(T).Name + " already exists!", this);
				return this;
			}
			T component = base.gameObject.AddComponent<T>();
			IAudioEffectModifier audioEffectModifier = Utility.CreateAudioEffectProxy(component);
			if (_addedEffects == null)
			{
				_addedEffects = new List<AddedEffect>();
			}
			_addedEffects.Add(new AddedEffect
			{
				Component = component,
				Modifier = audioEffectModifier
			});
			onSet?.Invoke(audioEffectModifier as TProxy);
			return this;
		}

		IAudioPlayer IAudioPlayer.RemoveAudioEffect<T>()
		{
			if (!IsActive)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Cannot remove " + typeof(T).Name + " from inactive audio player!");
				return this;
			}
			if (_addedEffects == null || _addedEffects.Count == 0)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>No effects to remove from audio player!");
				return this;
			}
			for (int num = _addedEffects.Count - 1; num >= 0; num--)
			{
				AddedEffect addedEffect = _addedEffects[num];
				if (addedEffect.Component is T)
				{
					if (addedEffect.Component != null)
					{
						UnityEngine.Object.Destroy(addedEffect.Component);
					}
					_addedEffects.RemoveAt(num);
					break;
				}
			}
			return this;
		}

		internal bool TransferOnUpdates(out Delegate[] onUpdateDelegates)
		{
			onUpdateDelegates = null;
			if (this._onUpdate != null)
			{
				onUpdateDelegates = this._onUpdate.GetInvocationList();
				this._onUpdate = null;
			}
			return onUpdateDelegates != null;
		}

		internal bool TransferOnEnds(out Delegate[] onEndDelegates)
		{
			onEndDelegates = null;
			if (this._onEnd != null)
			{
				onEndDelegates = this._onEnd.GetInvocationList();
				this._onEnd = null;
			}
			return onEndDelegates != null;
		}

		internal bool TransferOnPauses(out Delegate[] onPauseDelegates)
		{
			onPauseDelegates = null;
			if (this._onPaused != null)
			{
				onPauseDelegates = this._onPaused.GetInvocationList();
				this._onPaused = null;
			}
			return onPauseDelegates != null;
		}

		internal bool TransferDecorators(out IReadOnlyList<AudioPlayerDecorator> decorators)
		{
			decorators = _decorators;
			_decorators = null;
			return decorators != null;
		}

		internal void SetDecorators(IReadOnlyList<AudioPlayerDecorator> decorators)
		{
			_decorators = decorators as List<AudioPlayerDecorator>;
		}

		internal void TransferAddedEffectComponents(AudioPlayer newInstance)
		{
			newInstance.SetAddedEffectComponents(_addedEffects);
		}

		private void SetAddedEffectComponents(IReadOnlyList<AddedEffect> previousPlayerEffects)
		{
			if (previousPlayerEffects == null)
			{
				return;
			}
			GameObject gameObject = base.gameObject;
			for (int i = 0; i < previousPlayerEffects.Count; i++)
			{
				AddedEffect item = previousPlayerEffects[i];
				Component component = gameObject.AddComponent(Utility.GetFilterTypeFromProxy(item.Modifier));
				item.Modifier.TransferValueTo(component as Behaviour);
				item.Component = component;
				if (_addedEffects == null)
				{
					_addedEffects = new List<AddedEffect>();
				}
				_addedEffects.Add(item);
			}
		}

		internal bool TryGetDecorator<T>(out T decorator) where T : AudioPlayerDecorator
		{
			decorator = null;
			if (_decorators != null)
			{
				return _decorators.TryGetDecorator<T>(out decorator);
			}
			return false;
		}

		private bool HasDecoratorOf<T>() where T : AudioPlayerDecorator
		{
			T result;
			if (_decorators != null)
			{
				return _decorators.TryGetDecorator<T>(out result);
			}
			return false;
		}
	}
}
