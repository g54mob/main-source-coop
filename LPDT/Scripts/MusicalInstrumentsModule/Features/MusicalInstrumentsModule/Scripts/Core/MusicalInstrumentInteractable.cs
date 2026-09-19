using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.MusicalInstrumentsModule.Scripts.Core
{
	[NetworkBehaviourWeaved(2)]
	public class MusicalInstrumentInteractable : InteractableBase, IToggleableInteractable, IMusicalInstrumentTimelineState
	{
		private const float ActiveMinScrollDistance = 0.6f;

		private const float VolumeFadeDuration = 0.5f;

		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

		private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

		[Header("Instrument")]
		[SerializeField]
		private MusicalInstrumentType _instrumentType;

		[SerializeField]
		private EventReference _musicEvent;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private ParticleSystem[] _playingParticleRoots;

		[Header("Synchronization")]
		[SerializeField]
		[Tooltip("When enabled, this instrument registers in the shared timeline and seeks to the earliest active start tick. Disable to play independently from its own start.")]
		private bool _isSynchronizedWithOtherInstruments = true;

		[SerializeField]
		[Min(0.05f)]
		private float _timelineCheckInterval = 0.5f;

		[SerializeField]
		[Min(1f)]
		private int _timelineCorrectionThresholdMs = 200;

		private IAudioService _audioService;

		private MusicalInstrumentModel _musicalInstrumentModel;

		private LineArmsModel _lineArmsModel;

		private PlayerCustomizationModel _playerCustomizationModel;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		private EventInstance _eventInstance;

		private bool _hasEventInstance;

		private bool _isPendingInteraction;

		private bool _lastRenderedPlaying;

		private int _lastRenderedStartTick;

		private int _timelineLengthMs;

		private float _nextTimelineCheckTime;

		private float _minDistanceScrollBeforeInteract;

		private bool _isInitialized;

		private Coroutine _volumeFadeCoroutine;

		private bool _isFadingOut;

		private bool _arePlayingParticlesActive;

		private MaterialPropertyBlock _particlePropertyBlock;

		[WeaverGenerated]
		[DefaultForProperty("IsPlayingNetworked", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsPlayingNetworked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PerformanceStartTick", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PerformanceStartTick;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsPlayingNetworked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MusicalInstrumentInteractable.IsPlayingNetworked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MusicalInstrumentInteractable.IsPlayingNetworked. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int PerformanceStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MusicalInstrumentInteractable.PerformanceStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MusicalInstrumentInteractable.PerformanceStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		public MusicalInstrumentType InstrumentType => _instrumentType;

		public bool IsPlaying => IsPlayingNetworked;

		public bool IsToggledOn => IsPlaying;

		public event Action OnToggleChanged;

		[Inject]
		private void InjectDependencies(IAudioService audioService, MusicalInstrumentModel model, LineArmsModel lineArmsModel, PlayerCustomizationModel playerCustomizationModel, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_audioService = audioService;
			_musicalInstrumentModel = model;
			_lineArmsModel = lineArmsModel;
			_playerCustomizationModel = playerCustomizationModel;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			if (_isSynchronizedWithOtherInstruments)
			{
				_musicalInstrumentModel.Register(this);
			}
			_isInitialized = true;
			_lastRenderedPlaying = IsPlaying;
			_lastRenderedStartTick = PerformanceStartTick;
			if (IsPlaying)
			{
				StartLocalPlayback(PerformanceStartTick);
				SetPlayingParticlesActive(isActive: true);
				ProcessArmHoldPose();
			}
			else
			{
				SetPlayingParticlesActive(isActive: false);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_isInitialized && IsPlaying)
			{
				RestoreArmHoldPose();
			}
			if (_isSynchronizedWithOtherInstruments)
			{
				_musicalInstrumentModel.Unregister(this);
			}
			StopAndReleaseLocalPlayback();
			SetPlayingParticlesActive(isActive: false);
			_isInitialized = false;
		}

		public override void Interact()
		{
			if (IsInteractable)
			{
				if (base.HasStateAuthority)
				{
					ToggleInteraction();
					return;
				}
				_isPendingInteraction = true;
				base.Object.RequestStateAuthority();
			}
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				_isPendingInteraction = false;
				ToggleInteraction();
			}
		}

		public override void OnInteractEnd()
		{
			if (base.HasStateAuthority && IsPlaying)
			{
				SetPlaying(isPlaying: false);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				if (IsPlaying && !IsHeldByActivePlayer())
				{
					SetPlaying(isPlaying: false);
				}
				if (IsPlaying && _isSynchronizedWithOtherInstruments && _musicalInstrumentModel.TryGetEarliestActiveStartTick(this, out var startTick) && startTick < PerformanceStartTick)
				{
					PerformanceStartTick = startTick;
				}
			}
		}

		public override void Render()
		{
			bool isPlaying = IsPlaying;
			int performanceStartTick = PerformanceStartTick;
			if (_lastRenderedPlaying != isPlaying)
			{
				_lastRenderedPlaying = isPlaying;
				this.OnToggleChanged?.Invoke();
				SetPlayingParticlesActive(isPlaying);
			}
			if (isPlaying)
			{
				if (!_hasEventInstance || _lastRenderedStartTick != performanceStartTick)
				{
					StartLocalPlayback(performanceStartTick);
				}
				else
				{
					UpdateLocalPlayback();
				}
			}
			else if (_hasEventInstance)
			{
				if (_isFadingOut)
				{
					UpdateLocalPlayback();
				}
				else
				{
					BeginFadeOutAndRelease();
				}
			}
			_lastRenderedStartTick = performanceStartTick;
		}

		private void ToggleInteraction()
		{
			SetPlaying(!IsPlaying);
		}

		private int ResolvePerformanceStartTick()
		{
			if (_isSynchronizedWithOtherInstruments && _musicalInstrumentModel.TryGetEarliestActiveStartTick(this, out var startTick))
			{
				return startTick;
			}
			return base.Runner.Tick;
		}

		private void SetPlaying(bool isPlaying)
		{
			if (isPlaying)
			{
				PerformanceStartTick = ResolvePerformanceStartTick();
			}
			IsPlayingNetworked = isPlaying;
			_lastRenderedPlaying = isPlaying;
			_lastRenderedStartTick = PerformanceStartTick;
			this.OnToggleChanged?.Invoke();
			ProcessArmHoldPose();
			SetPlayingParticlesActive(isPlaying);
			if (isPlaying)
			{
				StartLocalPlayback(PerformanceStartTick);
			}
			else
			{
				BeginFadeOutAndRelease();
			}
		}

		private void SetPlayingParticlesActive(bool isActive)
		{
			if (_playingParticleRoots == null || _playingParticleRoots.Length == 0)
			{
				return;
			}
			if (isActive == _arePlayingParticlesActive)
			{
				if (isActive)
				{
					ApplyPlayerColorToPlayingParticles(GetActivatingPlayerColor());
				}
				return;
			}
			_arePlayingParticlesActive = isActive;
			if (isActive)
			{
				ApplyPlayerColorToPlayingParticles(GetActivatingPlayerColor());
				for (int i = 0; i < _playingParticleRoots.Length; i++)
				{
					ParticleSystem particleSystem = _playingParticleRoots[i];
					if (!(particleSystem == null))
					{
						particleSystem.Play(withChildren: true);
					}
				}
				return;
			}
			for (int j = 0; j < _playingParticleRoots.Length; j++)
			{
				ParticleSystem particleSystem2 = _playingParticleRoots[j];
				if (!(particleSystem2 == null))
				{
					particleSystem2.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
				}
			}
		}

		private void ApplyPlayerColorToPlayingParticles(Color playerColor)
		{
			if (_particlePropertyBlock == null)
			{
				_particlePropertyBlock = new MaterialPropertyBlock();
			}
			for (int i = 0; i < _playingParticleRoots.Length; i++)
			{
				ParticleSystem particleSystem = _playingParticleRoots[i];
				if (particleSystem == null)
				{
					continue;
				}
				ParticleSystemRenderer[] componentsInChildren = particleSystem.GetComponentsInChildren<ParticleSystemRenderer>(includeInactive: true);
				foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren)
				{
					if (!(particleSystemRenderer == null))
					{
						particleSystemRenderer.GetPropertyBlock(_particlePropertyBlock);
						_particlePropertyBlock.SetColor(BaseColorId, playerColor);
						_particlePropertyBlock.SetColor(EmissionColorId, playerColor);
						particleSystemRenderer.SetPropertyBlock(_particlePropertyBlock);
					}
				}
			}
		}

		private Color GetActivatingPlayerColor()
		{
			int activatingPlayerId = GetActivatingPlayerId();
			if (_playerCustomizationModel == null)
			{
				return Color.white;
			}
			for (int i = 0; i < _playerCustomizationModel.Slots.Count; i++)
			{
				PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots[i];
				if (playerCustomizationSlotData != null && playerCustomizationSlotData.PlayerId == activatingPlayerId)
				{
					return playerCustomizationSlotData.PrimaryColor;
				}
			}
			return Color.white;
		}

		private int GetActivatingPlayerId()
		{
			if (_simplePointGrabable != null && _simplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				return _simplePointGrabable.GrabbedByPlayers[0];
			}
			if (!(base.Object != null))
			{
				return 0;
			}
			return base.Object.StateAuthority.PlayerId;
		}

		private bool IsHeldByActivePlayer()
		{
			if (_simplePointGrabable == null || _simplePointGrabable.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			int num = _simplePointGrabable.GrabbedByPlayers[0];
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == num)
				{
					return true;
				}
			}
			return false;
		}

		private void ProcessArmHoldPose()
		{
			if (_isInitialized && _lineArmsModel.TryGetLineArmForPlayer(base.Object.StateAuthority.PlayerId, out var lineArm))
			{
				if (IsPlaying)
				{
					_minDistanceScrollBeforeInteract = lineArm.MinScrollDistanceValue;
					lineArm.MinScrollDistanceValue = 0.6f;
					lineArm.MouseScrollValue = new Vector2(0f, -100f);
					lineArm.DisableScroll = true;
					lineArm.HandleJointScroll(forced: true);
				}
				else
				{
					RestoreArmHoldPose(lineArm);
				}
			}
		}

		private void OnValidate()
		{
			if (_soundSource == null)
			{
				_soundSource = GetComponentInChildren<SoundSourceBehaviour>(includeInactive: true);
			}
			if (_simplePointGrabable == null)
			{
				_simplePointGrabable = GetComponent<SimplePointGrabable>();
			}
		}

		private void RestoreArmHoldPose(LineArmControllerBase lineArm = null)
		{
			if (!(lineArm == null) || _lineArmsModel.TryGetLineArmForPlayer(base.Object.StateAuthority.PlayerId, out lineArm))
			{
				lineArm.DisableScroll = false;
				lineArm.MinScrollDistanceValue = _minDistanceScrollBeforeInteract;
				_minDistanceScrollBeforeInteract = 0f;
			}
		}

		private void StartLocalPlayback(int startTick)
		{
			StopAndReleaseLocalPlayback();
			if (!_musicEvent.IsNull && !(_soundSource == null))
			{
				_eventInstance = _audioService.CreateInstance(_musicEvent);
				_hasEventInstance = true;
				CacheTimelineLength();
				SeekToExpectedTimeline(startTick);
				_eventInstance.setVolume(0f);
				SetSoundImmediately(_eventInstance);
				_audioService.StartInstanceWith3DAttributes(_eventInstance, _soundSource);
				_nextTimelineCheckTime = Time.unscaledTime + _timelineCheckInterval;
				_volumeFadeCoroutine = StartCoroutine(FadeVolumeRoutine(0f, 1f));
			}
		}

		private void BeginFadeOutAndRelease()
		{
			if (_hasEventInstance && !_isFadingOut)
			{
				CancelVolumeFade();
				_isFadingOut = true;
				_volumeFadeCoroutine = StartCoroutine(FadeOutAndReleaseRoutine());
			}
		}

		private IEnumerator FadeVolumeRoutine(float fromVolume, float toVolume)
		{
			float timer = 0f;
			if (_hasEventInstance)
			{
				_eventInstance.setVolume(fromVolume);
			}
			while (timer < 0.5f)
			{
				timer += Time.deltaTime;
				if (_hasEventInstance)
				{
					_eventInstance.setVolume(Mathf.Lerp(fromVolume, toVolume, timer / 0.5f));
				}
				yield return null;
			}
			if (_hasEventInstance)
			{
				_eventInstance.setVolume(toVolume);
			}
			_volumeFadeCoroutine = null;
		}

		private IEnumerator FadeOutAndReleaseRoutine()
		{
			float startVolume = 1f;
			if (_hasEventInstance)
			{
				_eventInstance.getVolume(out startVolume);
			}
			float timer = 0f;
			while (timer < 0.5f)
			{
				timer += Time.deltaTime;
				if (_hasEventInstance)
				{
					_eventInstance.setVolume(Mathf.Lerp(startVolume, 0f, timer / 0.5f));
				}
				yield return null;
			}
			_volumeFadeCoroutine = null;
			_isFadingOut = false;
			StopAndReleaseLocalPlayback();
		}

		private void CancelVolumeFade()
		{
			if (_volumeFadeCoroutine != null)
			{
				StopCoroutine(_volumeFadeCoroutine);
				_volumeFadeCoroutine = null;
			}
			_isFadingOut = false;
		}

		private void UpdateLocalPlayback()
		{
			if (_hasEventInstance)
			{
				if (_soundSource != null)
				{
					_eventInstance.set3DAttributes(_soundSource.SoundSourceTransform.To3DAttributes());
					ProcessSoundOcclusion(_eventInstance);
				}
				if (!_isFadingOut && !(Time.unscaledTime < _nextTimelineCheckTime))
				{
					_nextTimelineCheckTime = Time.unscaledTime + _timelineCheckInterval;
					CorrectTimelineIfNeeded();
				}
			}
		}

		private void CacheTimelineLength()
		{
			_timelineLengthMs = 0;
			_eventInstance.getDescription(out var description);
			if (description.isValid())
			{
				description.getLength(out _timelineLengthMs);
			}
		}

		private void CorrectTimelineIfNeeded()
		{
			_eventInstance.getTimelinePosition(out var position);
			int expectedTimelineMs = GetExpectedTimelineMs(PerformanceStartTick);
			int num = Math.Abs(position - expectedTimelineMs);
			if (_timelineLengthMs > 0)
			{
				num %= _timelineLengthMs;
				num = Math.Min(num, _timelineLengthMs - num);
			}
			if (num >= _timelineCorrectionThresholdMs)
			{
				_eventInstance.setTimelinePosition(expectedTimelineMs);
			}
		}

		private void SeekToExpectedTimeline(int startTick)
		{
			_eventInstance.setTimelinePosition(GetExpectedTimelineMs(startTick));
		}

		private int GetExpectedTimelineMs(int startTick)
		{
			int num = (int)((long)Math.Max(0, (int)base.Runner.Tick - startTick) * 1000L / Math.Max(1, base.Runner.TickRate));
			if (_timelineLengthMs <= 0)
			{
				return num;
			}
			return num % _timelineLengthMs;
		}

		private void StopAndReleaseLocalPlayback()
		{
			CancelVolumeFade();
			if (_hasEventInstance)
			{
				_audioService.StopInstance(_eventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_eventInstance);
				_eventInstance.clearHandle();
				_hasEventInstance = false;
				_timelineLengthMs = 0;
			}
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, out var value);
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float b = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value3);
				}
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
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
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsPlayingNetworked = _IsPlayingNetworked;
			PerformanceStartTick = _PerformanceStartTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsPlayingNetworked = IsPlayingNetworked;
			_PerformanceStartTick = PerformanceStartTick;
		}
	}
}
