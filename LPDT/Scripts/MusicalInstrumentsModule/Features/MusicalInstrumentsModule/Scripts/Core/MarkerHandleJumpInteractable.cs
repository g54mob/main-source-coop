using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Fusion;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.MusicalInstrumentsModule.Scripts.Core
{
	[NetworkBehaviourWeaved(5)]
	public class MarkerHandleJumpInteractable : InteractableBase, IToggleableInteractable
	{
		private sealed class MarkerCallbackState
		{
			public readonly ConcurrentQueue<string> PendingMarkerNames = new ConcurrentQueue<string>();
		}

		private const float VolumeFadeDuration = 0.5f;

		private const int InitialWaypointIndex = -1;

		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

		private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

		private static readonly ConcurrentDictionary<int, MarkerCallbackState> CallbackStates = new ConcurrentDictionary<int, MarkerCallbackState>();

		private static int _nextCallbackKey;

		[Header("Music")]
		[SerializeField]
		private EventReference _musicEvent;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private ParticleSystem[] _playingParticleRoots;

		[Header("Handle Jump")]
		[SerializeField]
		private Transform _handle;

		[SerializeField]
		private List<Transform> _waypoints = new List<Transform>();

		[SerializeField]
		private string[] _acceptedMarkerNames = Array.Empty<string>();

		[SerializeField]
		[Min(0.01f)]
		private float _jumpDuration = 0.25f;

		[SerializeField]
		[Min(0f)]
		private float _parabolaHeight = 0.08f;

		[Header("Synchronization")]
		[SerializeField]
		[Min(0.05f)]
		private float _timelineCheckInterval = 0.5f;

		[SerializeField]
		[Min(1f)]
		private int _timelineCorrectionThresholdMs = 200;

		private IAudioService _audioService;

		private PlayerCustomizationModel _playerCustomizationModel;

		private EventInstance _eventInstance;

		private EVENT_CALLBACK _markerCallback;

		private MarkerCallbackState _markerCallbackState;

		private int _callbackKey;

		private bool _hasEventInstance;

		private bool _isPendingInteraction;

		private bool _lastRenderedPlaying;

		private int _lastRenderedStartTick;

		private int _timelineLengthMs;

		private float _nextTimelineCheckTime;

		private Coroutine _volumeFadeCoroutine;

		private bool _isFadingOut;

		private bool _arePlayingParticlesActive;

		private MaterialPropertyBlock _particlePropertyBlock;

		private Vector3 _initialHandleLocalPosition;

		private Quaternion _initialHandleLocalRotation;

		private bool _hasInitialHandlePose;

		private int _animTargetIndex = -1;

		private int _animStartTick;

		private bool _isJumpAnimating;

		private float _localJumpStartUnscaledTime;

		private Vector3 _jumpStartLocalPosition;

		private Quaternion _jumpStartLocalRotation;

		private Vector3 _jumpEndLocalPosition;

		private Quaternion _jumpEndLocalRotation;

		[WeaverGenerated]
		[DefaultForProperty("IsPlayingNetworked", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsPlayingNetworked;

		[WeaverGenerated]
		[DefaultForProperty("PerformanceStartTick", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PerformanceStartTick;

		[WeaverGenerated]
		[DefaultForProperty("ActiveWaypointIndex", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ActiveWaypointIndex;

		[WeaverGenerated]
		[DefaultForProperty("JumpFromWaypointIndex", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _JumpFromWaypointIndex;

		[WeaverGenerated]
		[DefaultForProperty("JumpStartTick", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _JumpStartTick;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsPlayingNetworked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.IsPlayingNetworked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.IsPlayingNetworked. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int PerformanceStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.PerformanceStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.PerformanceStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int ActiveWaypointIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.ActiveWaypointIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.ActiveWaypointIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe int JumpFromWaypointIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.JumpFromWaypointIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.JumpFromWaypointIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe int JumpStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.JumpStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MarkerHandleJumpInteractable.JumpStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		public bool IsPlaying => IsPlayingNetworked;

		public bool IsToggledOn => IsPlaying;

		public event Action OnToggleChanged;

		[Inject]
		private void InjectDependencies(IAudioService audioService, PlayerCustomizationModel playerCustomizationModel)
		{
			_audioService = audioService;
			_playerCustomizationModel = playerCustomizationModel;
		}

		public override void Spawned()
		{
			CacheInitialHandlePose();
			if (base.HasStateAuthority)
			{
				ActiveWaypointIndex = -1;
				JumpFromWaypointIndex = -1;
				JumpStartTick = 0;
			}
			SnapHandleToWaypoint(ActiveWaypointIndex);
			_lastRenderedPlaying = IsPlaying;
			_lastRenderedStartTick = PerformanceStartTick;
			if (IsPlaying)
			{
				StartLocalPlayback(PerformanceStartTick);
				SetPlayingParticlesActive(isActive: true);
			}
			else
			{
				SetPlayingParticlesActive(isActive: false);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			StopAndReleaseLocalPlayback();
			SetPlayingParticlesActive(isActive: false);
			CancelHandleJump();
			RestoreHandlePose();
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
			if (!base.HasStateAuthority)
			{
				DrainPendingMarkers();
				return;
			}
			if (IsPlaying && !IsHeldByActivePlayer())
			{
				SetPlaying(isPlaying: false);
			}
			if (!IsPlaying)
			{
				DrainPendingMarkers();
			}
			else
			{
				ProcessPendingMarkersOnAuthority();
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
			UpdateHandleJumpVisual();
			_lastRenderedStartTick = performanceStartTick;
		}

		private void ToggleInteraction()
		{
			SetPlaying(!IsPlaying);
		}

		private void SetPlaying(bool isPlaying)
		{
			if (isPlaying)
			{
				PerformanceStartTick = base.Runner.Tick;
				ActiveWaypointIndex = -1;
				JumpFromWaypointIndex = -1;
				JumpStartTick = 0;
			}
			else
			{
				ActiveWaypointIndex = -1;
				JumpFromWaypointIndex = -1;
				JumpStartTick = 0;
				DrainPendingMarkers();
			}
			IsPlayingNetworked = isPlaying;
			_lastRenderedPlaying = isPlaying;
			_lastRenderedStartTick = PerformanceStartTick;
			this.OnToggleChanged?.Invoke();
			SetPlayingParticlesActive(isPlaying);
			SnapHandleToWaypoint(ActiveWaypointIndex);
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

		private void ProcessPendingMarkersOnAuthority()
		{
			if (_markerCallbackState == null)
			{
				return;
			}
			int? num = null;
			string result;
			while (_markerCallbackState.PendingMarkerNames.TryDequeue(out result))
			{
				if (IsAcceptedMarker(result) && TryPickRandomWaypointIndex(out var waypointIndex))
				{
					num = waypointIndex;
				}
			}
			if (num.HasValue)
			{
				JumpFromWaypointIndex = ActiveWaypointIndex;
				ActiveWaypointIndex = num.Value;
				JumpStartTick = base.Runner.Tick;
			}
		}

		private void DrainPendingMarkers()
		{
			if (_markerCallbackState != null)
			{
				string result;
				while (_markerCallbackState.PendingMarkerNames.TryDequeue(out result))
				{
				}
			}
		}

		private bool IsAcceptedMarker(string markerName)
		{
			if (_acceptedMarkerNames == null || _acceptedMarkerNames.Length == 0)
			{
				return true;
			}
			for (int i = 0; i < _acceptedMarkerNames.Length; i++)
			{
				if (string.Equals(_acceptedMarkerNames[i], markerName, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryPickRandomWaypointIndex(out int waypointIndex)
		{
			waypointIndex = -1;
			if (_waypoints == null || _waypoints.Count == 0)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < _waypoints.Count; i++)
			{
				if (_waypoints[i] != null)
				{
					num++;
				}
			}
			switch (num)
			{
			case 0:
				return false;
			case 1:
			{
				for (int j = 0; j < _waypoints.Count; j++)
				{
					if (!(_waypoints[j] == null))
					{
						waypointIndex = j;
						return true;
					}
				}
				break;
			}
			}
			int activeWaypointIndex = ActiveWaypointIndex;
			for (int k = 0; k < 16; k++)
			{
				waypointIndex = UnityEngine.Random.Range(0, _waypoints.Count);
				if (_waypoints[waypointIndex] != null && waypointIndex != activeWaypointIndex)
				{
					return true;
				}
			}
			for (int l = 0; l < _waypoints.Count; l++)
			{
				if (!(_waypoints[l] == null) && l != activeWaypointIndex)
				{
					waypointIndex = l;
					return true;
				}
			}
			return false;
		}

		private void CacheInitialHandlePose()
		{
			if (_handle == null)
			{
				_hasInitialHandlePose = false;
				return;
			}
			_initialHandleLocalPosition = _handle.localPosition;
			_initialHandleLocalRotation = _handle.localRotation;
			_hasInitialHandlePose = true;
		}

		private void UpdateHandleJumpVisual()
		{
			if (_handle == null)
			{
				return;
			}
			if (_animTargetIndex != ActiveWaypointIndex || _animStartTick != JumpStartTick)
			{
				BeginHandleJump(ActiveWaypointIndex, JumpStartTick);
			}
			if (_isJumpAnimating)
			{
				float jumpProgress = GetJumpProgress01();
				ApplyParabolicHandlePose(jumpProgress);
				if (jumpProgress >= 1f)
				{
					_isJumpAnimating = false;
				}
			}
		}

		private void BeginHandleJump(int targetIndex, int startTick)
		{
			_animTargetIndex = targetIndex;
			_animStartTick = startTick;
			if (_handle == null)
			{
				_isJumpAnimating = false;
				return;
			}
			if (targetIndex < 0 || !TryGetWaypointLocalPose(targetIndex, out _jumpEndLocalPosition, out _jumpEndLocalRotation))
			{
				CancelHandleJump();
				RestoreHandlePose();
				return;
			}
			if (startTick <= 0 || _jumpDuration <= 0f)
			{
				SnapHandleToWaypoint(targetIndex);
				return;
			}
			float num = 0f;
			if (base.Runner != null)
			{
				num = Mathf.Max(0f, (float)((int)base.Runner.Tick - startTick) / (float)Mathf.Max(1, base.Runner.TickRate));
			}
			if (!_isJumpAnimating && num > 0.001f)
			{
				ResolveJumpStartPose(JumpFromWaypointIndex);
			}
			else
			{
				_jumpStartLocalPosition = _handle.localPosition;
				_jumpStartLocalRotation = _handle.localRotation;
			}
			_localJumpStartUnscaledTime = Time.unscaledTime - num;
			_isJumpAnimating = true;
			float jumpProgress = GetJumpProgress01();
			ApplyParabolicHandlePose(jumpProgress);
			if (jumpProgress >= 1f)
			{
				_isJumpAnimating = false;
			}
		}

		private void ResolveJumpStartPose(int fromWaypointIndex)
		{
			if (!TryGetWaypointLocalPose(fromWaypointIndex, out _jumpStartLocalPosition, out _jumpStartLocalRotation))
			{
				if (_hasInitialHandlePose)
				{
					_jumpStartLocalPosition = _initialHandleLocalPosition;
					_jumpStartLocalRotation = _initialHandleLocalRotation;
				}
				else
				{
					_jumpStartLocalPosition = _handle.localPosition;
					_jumpStartLocalRotation = _handle.localRotation;
				}
			}
		}

		private float GetJumpProgress01()
		{
			if (_jumpDuration <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01((Time.unscaledTime - _localJumpStartUnscaledTime) / _jumpDuration);
		}

		private void ApplyParabolicHandlePose(float t)
		{
			Vector3 vector = Vector3.Lerp(_jumpStartLocalPosition, _jumpEndLocalPosition, t);
			Vector3 handleParentLocalUp = GetHandleParentLocalUp();
			float num = 4f * _parabolaHeight * t * (1f - t);
			_handle.localPosition = vector + handleParentLocalUp * num;
			_handle.localRotation = Quaternion.Slerp(_jumpStartLocalRotation, _jumpEndLocalRotation, t);
		}

		private Vector3 GetHandleParentLocalUp()
		{
			Transform transform = ((_handle != null) ? _handle.parent : null);
			if (transform == null)
			{
				return Vector3.up;
			}
			Vector3 vector = transform.InverseTransformDirection(Vector3.up);
			if (!(vector.sqrMagnitude > 0.0001f))
			{
				return Vector3.up;
			}
			return vector.normalized;
		}

		private void SnapHandleToWaypoint(int waypointIndex)
		{
			CancelHandleJump();
			if (!(_handle == null))
			{
				if (!TryGetWaypointLocalPose(waypointIndex, out var localPosition, out var localRotation))
				{
					RestoreHandlePose();
					return;
				}
				_handle.localPosition = localPosition;
				_handle.localRotation = localRotation;
				_animTargetIndex = waypointIndex;
				_animStartTick = JumpStartTick;
			}
		}

		private bool TryGetWaypointLocalPose(int waypointIndex, out Vector3 localPosition, out Quaternion localRotation)
		{
			localPosition = default(Vector3);
			localRotation = default(Quaternion);
			if (waypointIndex < 0 || _waypoints == null || waypointIndex >= _waypoints.Count || _waypoints[waypointIndex] == null)
			{
				return false;
			}
			Transform transform = _waypoints[waypointIndex];
			Transform transform2 = ((_handle != null) ? _handle.parent : null);
			if (transform2 != null)
			{
				localPosition = transform2.InverseTransformPoint(transform.position);
				localRotation = Quaternion.Inverse(transform2.rotation) * transform.rotation;
			}
			else
			{
				localPosition = transform.position;
				localRotation = transform.rotation;
			}
			return true;
		}

		private void CancelHandleJump()
		{
			_isJumpAnimating = false;
			_animTargetIndex = ActiveWaypointIndex;
			_animStartTick = JumpStartTick;
		}

		private void RestoreHandlePose()
		{
			if (!(_handle == null) && _hasInitialHandlePose)
			{
				_handle.localPosition = _initialHandleLocalPosition;
				_handle.localRotation = _initialHandleLocalRotation;
				_animTargetIndex = -1;
			}
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
			if (_handle == null && _simplePointGrabable != null && _simplePointGrabable.Handles != null && _simplePointGrabable.Handles.Count > 0)
			{
				_handle = _simplePointGrabable.Handles[0];
			}
		}

		private void StartLocalPlayback(int startTick)
		{
			StopAndReleaseLocalPlayback();
			if (!_musicEvent.IsNull && !(_soundSource == null) && _audioService != null)
			{
				_callbackKey = Interlocked.Increment(ref _nextCallbackKey);
				_markerCallbackState = new MarkerCallbackState();
				CallbackStates[_callbackKey] = _markerCallbackState;
				_markerCallback = MarkerEventCallback;
				_eventInstance = _audioService.CreateInstance(_musicEvent);
				_hasEventInstance = true;
				_eventInstance.setUserData(new IntPtr(_callbackKey));
				_eventInstance.setCallback(_markerCallback, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
				CacheTimelineLength();
				SeekToExpectedTimeline(startTick);
				_eventInstance.setVolume(0f);
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
			UnregisterCallbackState();
			if (_hasEventInstance)
			{
				_eventInstance.setCallback(null);
				_eventInstance.setUserData(IntPtr.Zero);
				_audioService.StopInstance(_eventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_eventInstance);
				_eventInstance.clearHandle();
				_hasEventInstance = false;
				_timelineLengthMs = 0;
				_markerCallback = null;
			}
		}

		private void UnregisterCallbackState()
		{
			if (_callbackKey != 0)
			{
				CallbackStates.TryRemove(_callbackKey, out var _);
			}
			_callbackKey = 0;
			_markerCallbackState = null;
		}

		[MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
		private static RESULT MarkerEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
		{
			if (type != EVENT_CALLBACK_TYPE.TIMELINE_MARKER || parameterPtr == IntPtr.Zero)
			{
				return RESULT.OK;
			}
			if (new EventInstance(instancePtr).getUserData(out var userdata) != RESULT.OK || userdata == IntPtr.Zero)
			{
				return RESULT.OK;
			}
			if (!CallbackStates.TryGetValue(userdata.ToInt32(), out var value))
			{
				return RESULT.OK;
			}
			string text = Marshal.PtrToStructure<TIMELINE_MARKER_PROPERTIES>(parameterPtr).name;
			if (!string.IsNullOrEmpty(text))
			{
				value.PendingMarkerNames.Enqueue(text);
			}
			return RESULT.OK;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsPlayingNetworked = _IsPlayingNetworked;
			PerformanceStartTick = _PerformanceStartTick;
			ActiveWaypointIndex = _ActiveWaypointIndex;
			JumpFromWaypointIndex = _JumpFromWaypointIndex;
			JumpStartTick = _JumpStartTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsPlayingNetworked = IsPlayingNetworked;
			_PerformanceStartTick = PerformanceStartTick;
			_ActiveWaypointIndex = ActiveWaypointIndex;
			_JumpFromWaypointIndex = JumpFromWaypointIndex;
			_JumpStartTick = JumpStartTick;
		}
	}
}
