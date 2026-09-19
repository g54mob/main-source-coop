using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.JacuzziBeachInteractableModule.Scripts.VisualMusic;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class JacuzziBeachInteractableBehaviour : NetworkBehaviour
	{
		private enum FlushPhase
		{
			None = 0,
			Waiting = 1,
			Flushing = 2,
			Flushed = 3,
			Restoring = 4
		}

		private sealed class OccupantState
		{
			public float LastSeenTime;

			public float PhaseElapsed;

			public float CurrentBlend;

			public FlushPhase Phase;

			public PlayerJacuzziApplier Applier;

			public NetworkObject PlayerObject;
		}

		private const float BLEND_EPSILON = 0.001f;

		[SerializeField]
		private LayerMask _playerLayerMask;

		[Tooltip("How long presence survives without an OnTriggerStay report. Covers physics steps a sleeping or re-parented body skips.")]
		[SerializeField]
		private float _presenceTimeout = 0.3f;

		[SerializeField]
		private List<VisualMusicBehaviour> _visualMusicEffects = new List<VisualMusicBehaviour>();

		[SerializeField]
		private ParticleSystem _bubblesParticleSystem;

		[SerializeField]
		private List<EventReference> _musicEvents = new List<EventReference>();

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		[Min(0.01f)]
		private float _musicFadeOutDuration = 1f;

		private const int NO_VISUAL_MUSIC_EFFECT_INDEX = -1;

		[WeaverGenerated]
		[DefaultForProperty("VisualMusicEffectIndex", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _VisualMusicEffectIndex;

		[WeaverGenerated]
		[DefaultForProperty("IsMusicPlaying", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsMusicPlaying;

		[WeaverGenerated]
		[DefaultForProperty("MusicTrackIndex", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _MusicTrackIndex;

		private VisualMusicBehaviour _activeVisualMusicEffect;

		private JacuzziBeachInteractableConfiguration _configuration;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly Dictionary<int, OccupantState> _occupantsByPlayerId = new Dictionary<int, OccupantState>();

		private readonly List<int> _playersToRemove = new List<int>();

		private EventInstance _musicInstance;

		private bool _hasMusicInstance;

		private bool _isMusicFadingOut;

		private int _lastRenderedMusicTrackIndex = -1;

		private Coroutine _musicFadeOutRoutine;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		[Networked]
		[OnChangedRender("OnVisualMusicEffectRender")]
		[NetworkedWeaved(0, 1)]
		private unsafe int VisualMusicEffectIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.VisualMusicEffectIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.VisualMusicEffectIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkBool IsMusicPlaying
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.IsMusicPlaying. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.IsMusicPlaying. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int MusicTrackIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.MusicTrackIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing JacuzziBeachInteractableBehaviour.MusicTrackIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Inject]
		public void InjectDependencies(JacuzziBeachInteractableConfiguration configuration, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_configuration = configuration;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				VisualMusicEffectIndex = -1;
			}
			ApplyVisualMusicEffect();
			if ((bool)IsMusicPlaying)
			{
				StartMusicPlayback(MusicTrackIndex);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			StopAndReleaseMusicImmediate();
			base.Despawned(runner, hasState);
			foreach (KeyValuePair<int, OccupantState> item in _occupantsByPlayerId)
			{
				if (IsOccupantAlive(item.Value))
				{
					item.Value.Applier.ResetJacuzziPresence();
				}
			}
			_occupantsByPlayerId.Clear();
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			StopAndReleaseMusicImmediate();
		}

		public override void Render()
		{
			if ((bool)IsMusicPlaying)
			{
				if (!_hasMusicInstance || _isMusicFadingOut || _lastRenderedMusicTrackIndex != MusicTrackIndex)
				{
					StartMusicPlayback(MusicTrackIndex);
				}
				else
				{
					UpdateMusicPlayback3D();
				}
			}
			else if (_hasMusicInstance && !_isMusicFadingOut)
			{
				BeginMusicFadeOut();
			}
		}

		public void NotifyPlayerColliderEntered(Collider other)
		{
			if (TryResolvePlayer(other, out var playerId, out var applier, out var playerObject))
			{
				NotifyPlayerEntered(playerId, applier, playerObject);
			}
		}

		public void NotifyPlayerEntered(int playerId, PlayerJacuzziApplier applier, NetworkObject playerObject)
		{
			if (!_occupantsByPlayerId.TryGetValue(playerId, out var value))
			{
				value = new OccupantState
				{
					Applier = applier,
					PlayerObject = playerObject,
					Phase = FlushPhase.Waiting,
					PhaseElapsed = 0f,
					CurrentBlend = 0f
				};
				_occupantsByPlayerId[playerId] = value;
			}
			else if (value.Applier != applier)
			{
				value.Applier.ResetJacuzziPresence();
				value.Applier = applier;
				value.PlayerObject = playerObject;
			}
			if (value.Phase == FlushPhase.Restoring)
			{
				value.Phase = ((!(value.CurrentBlend > 0f)) ? FlushPhase.Waiting : FlushPhase.Flushing);
				value.PhaseElapsed = 0f;
			}
			value.LastSeenTime = Time.time;
		}

		public void SwitchVisualMusic(bool isTurnedOn)
		{
			if (base.HasStateAuthority)
			{
				if (!isTurnedOn || _visualMusicEffects.Count == 0)
				{
					VisualMusicEffectIndex = -1;
				}
				else
				{
					VisualMusicEffectIndex = UnityEngine.Random.Range(0, _visualMusicEffects.Count);
				}
			}
		}

		public void SwitchBubbles(bool isTurnedOn)
		{
			if (isTurnedOn)
			{
				_bubblesParticleSystem.Play(withChildren: true);
			}
			else
			{
				_bubblesParticleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			}
		}

		public void SwitchMusic(bool isTurnedOn)
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (isTurnedOn)
			{
				if (!IsMusicPlaying && TryPickRandomMusicTrackIndex(out var trackIndex))
				{
					MusicTrackIndex = trackIndex;
					IsMusicPlaying = true;
				}
			}
			else if ((bool)IsMusicPlaying)
			{
				IsMusicPlaying = false;
			}
		}

		private bool TryPickRandomMusicTrackIndex(out int trackIndex)
		{
			trackIndex = -1;
			if (_musicEvents == null || _musicEvents.Count == 0)
			{
				return false;
			}
			int count = _musicEvents.Count;
			int num = Mathf.Min(count, 8);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnityEngine.Random.Range(0, count);
				if (!_musicEvents[num2].IsNull)
				{
					trackIndex = num2;
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!_musicEvents[j].IsNull)
				{
					trackIndex = j;
					return true;
				}
			}
			return false;
		}

		private void StartMusicPlayback(int trackIndex)
		{
			StopAndReleaseMusicImmediate();
			if (_audioService != null && !(_soundSource == null) && _musicEvents != null && trackIndex >= 0 && trackIndex < _musicEvents.Count)
			{
				EventReference eventReference = _musicEvents[trackIndex];
				if (!eventReference.IsNull)
				{
					_musicInstance = _audioService.CreateInstance(eventReference);
					_hasMusicInstance = true;
					_lastRenderedMusicTrackIndex = trackIndex;
					_musicInstance.setVolume(1f);
					SetSoundImmediately(_musicInstance);
					_audioService.StartInstanceWith3DAttributes(_musicInstance, _soundSource);
				}
			}
		}

		private void UpdateMusicPlayback3D()
		{
			if (_hasMusicInstance && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				_musicInstance.set3DAttributes(_soundSource.SoundSourceTransform.To3DAttributes());
				ProcessSoundOcclusion(_musicInstance);
			}
		}

		private void BeginMusicFadeOut()
		{
			if (_musicFadeOutRoutine != null)
			{
				StopCoroutine(_musicFadeOutRoutine);
			}
			_musicFadeOutRoutine = StartCoroutine(FadeOutMusic());
		}

		private IEnumerator FadeOutMusic()
		{
			_isMusicFadingOut = true;
			float duration = Mathf.Max(0.01f, _musicFadeOutDuration);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				if (_hasMusicInstance && _musicInstance.isValid())
				{
					_musicInstance.setVolume(Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsed / duration)));
				}
				yield return null;
			}
			StopAndReleaseMusicImmediate();
		}

		private void StopAndReleaseMusicImmediate()
		{
			if (_musicFadeOutRoutine != null)
			{
				StopCoroutine(_musicFadeOutRoutine);
				_musicFadeOutRoutine = null;
			}
			_isMusicFadingOut = false;
			_lastRenderedMusicTrackIndex = -1;
			if (_hasMusicInstance)
			{
				if (_audioService != null)
				{
					_audioService.StopInstance(_musicInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
					_audioService.ReleaseInstance(_musicInstance);
				}
				_musicInstance = default(EventInstance);
				_hasMusicInstance = false;
			}
		}

		private void Update()
		{
			if (_occupantsByPlayerId.Count == 0)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			_playersToRemove.Clear();
			foreach (KeyValuePair<int, OccupantState> item in _occupantsByPlayerId)
			{
				OccupantState value = item.Value;
				if (!IsOccupantAlive(value))
				{
					_playersToRemove.Add(item.Key);
					continue;
				}
				bool flag = Time.time - value.LastSeenTime <= _presenceTimeout;
				if (!flag && value.Phase != FlushPhase.Waiting && value.Phase != FlushPhase.Restoring)
				{
					value.Phase = FlushPhase.Restoring;
					value.PhaseElapsed = 0f;
				}
				switch (value.Phase)
				{
				case FlushPhase.Waiting:
					if (!flag)
					{
						_playersToRemove.Add(item.Key);
						break;
					}
					value.PhaseElapsed += deltaTime;
					if (!(value.PhaseElapsed < _configuration.TimeBeforeReddening))
					{
						value.Phase = FlushPhase.Flushing;
						value.PhaseElapsed = 0f;
					}
					break;
				case FlushPhase.Flushing:
					TickBlend(value, 1f, _configuration.ReddeningDuration, deltaTime);
					if (value.CurrentBlend >= 0.999f)
					{
						value.CurrentBlend = 1f;
						value.Phase = FlushPhase.Flushed;
					}
					break;
				case FlushPhase.Restoring:
					TickBlend(value, 0f, _configuration.RestoreDuration, deltaTime);
					if (!(value.CurrentBlend > 0.001f))
					{
						value.CurrentBlend = 0f;
						if (!flag)
						{
							_playersToRemove.Add(item.Key);
							break;
						}
						value.Phase = FlushPhase.Waiting;
						value.PhaseElapsed = 0f;
					}
					break;
				}
				value.Applier.TickJacuzziPresence(value.CurrentBlend);
			}
			for (int i = 0; i < _playersToRemove.Count; i++)
			{
				OccupantState occupantState = _occupantsByPlayerId[_playersToRemove[i]];
				if (IsOccupantAlive(occupantState))
				{
					occupantState.Applier.ResetJacuzziPresence();
				}
				_occupantsByPlayerId.Remove(_playersToRemove[i]);
			}
		}

		private void OnVisualMusicEffectRender()
		{
			ApplyVisualMusicEffect();
		}

		private void ApplyVisualMusicEffect()
		{
			VisualMusicBehaviour visualMusicBehaviour = ((VisualMusicEffectIndex >= 0 && VisualMusicEffectIndex < _visualMusicEffects.Count) ? _visualMusicEffects[VisualMusicEffectIndex] : null);
			if (!(_activeVisualMusicEffect == visualMusicBehaviour))
			{
				if (_activeVisualMusicEffect != null)
				{
					_activeVisualMusicEffect.InvokeEffect(isTurnedOn: false);
				}
				_activeVisualMusicEffect = visualMusicBehaviour;
				if (_activeVisualMusicEffect != null)
				{
					_activeVisualMusicEffect.InvokeEffect(isTurnedOn: true);
				}
			}
		}

		private bool IsOccupantAlive(OccupantState state)
		{
			if (state.Applier != null && state.PlayerObject != null)
			{
				return state.PlayerObject.IsValid;
			}
			return false;
		}

		private void TickBlend(OccupantState state, float targetBlend, float duration, float deltaTime)
		{
			float maxDelta = deltaTime / duration;
			state.CurrentBlend = Mathf.MoveTowards(state.CurrentBlend, targetBlend, maxDelta);
		}

		private bool TryResolvePlayer(Collider other, out int playerId, out PlayerJacuzziApplier applier, out NetworkObject playerObject)
		{
			playerId = -1;
			applier = null;
			playerObject = other.GetComponentInParent<NetworkObject>();
			if (playerObject == null)
			{
				return false;
			}
			applier = playerObject.GetComponentInParent<PlayerJacuzziApplier>(includeInactive: true);
			if (applier == null)
			{
				return false;
			}
			playerId = playerObject.InputAuthority.PlayerId;
			return true;
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
			VisualMusicEffectIndex = _VisualMusicEffectIndex;
			IsMusicPlaying = _IsMusicPlaying;
			MusicTrackIndex = _MusicTrackIndex;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualMusicEffectIndex = VisualMusicEffectIndex;
			_IsMusicPlaying = IsMusicPlaying;
			_MusicTrackIndex = MusicTrackIndex;
		}
	}
}
