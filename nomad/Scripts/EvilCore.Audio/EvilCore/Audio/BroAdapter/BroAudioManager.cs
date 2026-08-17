using System;
using System.Collections.Generic;
using Ami.BroAudio;
using Ami.BroAudio.Data;
using Ami.BroAudio.Runtime;
using EvilCore.Audio.Snapshots;
using UnityEngine;
using UnityEngine.Audio;

namespace EvilCore.Audio.BroAdapter
{
	public class BroAudioManager : MonoBehaviour, IAudioManager, IAudioHandleOwner
	{
		private readonly struct HandleEntry
		{
			public readonly IAudioPlayer Player;

			public readonly SoundID SoundId;

			public HandleEntry(IAudioPlayer player, SoundID soundId)
			{
				Player = player;
				SoundId = soundId;
			}
		}

		[Header("Entity Registry")]
		[SerializeField]
		[Tooltip("Project index of every AudioEntity asset. Used by the Mirror serializer to resolve a SoundID from the entity name sent over the network.")]
		private BroAudioEntityRegistry entityRegistry;

		[Header("Global Parameters")]
		[SerializeField]
		[Tooltip("Project-wide parameter registry. Globals defined here are visible from every AudioEntity's Parameters tab and reachable from any AudioParameter picker.")]
		private BroAudioGlobalParameters globalParameters;

		private readonly Dictionary<uint, HandleEntry> _activeHandles = new Dictionary<uint, HandleEntry>();

		private uint _nextHandleId = 1u;

		private BroAudioFadeRunner _fadeRunner;

		private readonly Dictionary<AudioBus, float> _busVolumeCache = new Dictionary<AudioBus, float>();

		public BroAudioEntityRegistry EntityRegistry => entityRegistry;

		private void Awake()
		{
			AudioHandleResolver.Register(this);
			_fadeRunner = base.gameObject.GetComponent<BroAudioFadeRunner>();
			if (_fadeRunner == null)
			{
				_fadeRunner = base.gameObject.AddComponent<BroAudioFadeRunner>();
			}
			if (entityRegistry != null)
			{
				entityRegistry.SetActive();
			}
			if (globalParameters != null)
			{
				globalParameters.SetActive();
			}
			_busVolumeCache[AudioBus.Master] = 1f;
			_busVolumeCache[AudioBus.Music] = 1f;
			_busVolumeCache[AudioBus.Sfx] = 1f;
			_busVolumeCache[AudioBus.Ambience] = 1f;
		}

		private void OnDestroy()
		{
			AudioHandleResolver.Unregister(this);
			entityRegistry?.ClearActive();
			globalParameters?.ClearActive();
		}

		public bool IsHandleActive(AudioHandle handle)
		{
			if (handle.Id == 0)
			{
				return false;
			}
			if (!_activeHandles.TryGetValue(handle.Id, out var value))
			{
				return false;
			}
			if (value.Player != null)
			{
				return value.Player.IsActive;
			}
			return false;
		}

		public void PlayOneShot(SoundID id, Vector3 position = default(Vector3))
		{
			if (id.IsValid())
			{
				BroAudio.Play(id, position);
			}
		}

		public void PlayOneShotUI(SoundID id)
		{
			if (id.IsValid())
			{
				BroAudio.Play(id);
			}
		}

		public void PlayOneShotAttached(SoundID id, GameObject attachTo)
		{
			if (id.IsValid() && !(attachTo == null))
			{
				BroAudio.Play(id, attachTo.transform);
			}
		}

		public AudioHandle PlayEvent(SoundID id, Vector3 position = default(Vector3))
		{
			if (!id.IsValid())
			{
				return AudioHandle.Invalid;
			}
			return Track(BroAudio.Play(id, position));
		}

		public AudioHandle PlayEventAttached(SoundID id, GameObject attachTo)
		{
			if (!id.IsValid() || attachTo == null)
			{
				return AudioHandle.Invalid;
			}
			return Track(BroAudio.Play(id, attachTo.transform));
		}

		public AudioHandle PlayLoopRegion(SoundID id, GameObject attachTo, bool skipIntro = false)
		{
			if (!id.IsValid() || attachTo == null)
			{
				return AudioHandle.Invalid;
			}
			return Track(BroAudio.Play(id, attachTo.transform, skipIntro));
		}

		public void StopLoopRegion(AudioHandle handle)
		{
			if (TryGetEntry(handle, out var entry))
			{
				if (entry.Player is IParameterizedPlayer parameterizedPlayer)
				{
					parameterizedPlayer.RequestLoopRegionExit();
				}
				else
				{
					StopEvent(handle);
				}
			}
		}

		public void StopEvent(AudioHandle handle, AudioStopMode stopMode = AudioStopMode.AllowFadeout, float fadeoutSeconds = 0.25f)
		{
			if (handle.Id != 0 && _activeHandles.TryGetValue(handle.Id, out var value))
			{
				_activeHandles.Remove(handle.Id);
				if (SoundManager.HasInstance)
				{
					float fadeOut = ((stopMode == AudioStopMode.Immediate) ? 0f : Mathf.Max(0f, fadeoutSeconds));
					BroAudio.Stop(value.SoundId, fadeOut);
					_fadeRunner?.CancelAll(value.SoundId);
				}
			}
		}

		public void ReleaseInstance(AudioHandle handle)
		{
			StopEvent(handle, AudioStopMode.Immediate, 0f);
		}

		public bool IsPlaying(AudioHandle handle)
		{
			if (!TryGetEntry(handle, out var entry))
			{
				return false;
			}
			return entry.Player.IsPlaying;
		}

		public void SetParameter(AudioHandle handle, AudioParameter parameter, bool value)
		{
			if (TryResolveTypedParameter(handle, parameter, AudioParameterType.Bool, out var player, out var parameterId) && player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterId, value);
			}
		}

		public void SetParameter(AudioHandle handle, AudioParameter parameter, int value)
		{
			if (TryResolveTypedParameter(handle, parameter, AudioParameterType.Int, out var player, out var parameterId) && player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterId, value);
			}
		}

		public void SetParameter(AudioHandle handle, AudioParameter parameter, float value)
		{
			if (TryResolveTypedParameter(handle, parameter, AudioParameterType.Float, out var player, out var parameterId) && player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterId, value);
			}
		}

		public bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out bool value)
		{
			value = false;
			if (!TryResolveTypedParameter(handle, parameter, AudioParameterType.Bool, out var player, out var parameterId))
			{
				return false;
			}
			if (player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterId, out value);
			}
			return false;
		}

		public bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out int value)
		{
			value = 0;
			if (!TryResolveTypedParameter(handle, parameter, AudioParameterType.Int, out var player, out var parameterId))
			{
				return false;
			}
			if (player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterId, out value);
			}
			return false;
		}

		public bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out float value)
		{
			value = 0f;
			if (!TryResolveTypedParameter(handle, parameter, AudioParameterType.Float, out var player, out var parameterId))
			{
				return false;
			}
			if (player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterId, out value);
			}
			return false;
		}

		private bool TryResolveTypedParameter(AudioHandle handle, AudioParameter parameter, AudioParameterType expectedType, out IAudioPlayer player, out string parameterId)
		{
			parameterId = null;
			player = null;
			if (!TryGetEntry(handle, out var entry))
			{
				return false;
			}
			player = entry.Player;
			if (!parameter.TryGetDefinition(out var definition))
			{
				return false;
			}
			if (definition.Type != expectedType)
			{
				return false;
			}
			if ((!(BroAudioGlobalParameters.ActiveInstance != null) || !BroAudioGlobalParameters.ActiveInstance.TryFindParameterById(definition.Id, out var _)) && parameter.Entity != null)
			{
				string b = entry.SoundId.ToString();
				string.Equals(parameter.Entity.Name, b, StringComparison.Ordinal);
			}
			parameterId = definition.Id;
			return true;
		}

		public void SetParameter(AudioHandle handle, string parameterName, float value)
		{
			if (!TryGetEntry(handle, out var entry))
			{
				return;
			}
			switch (parameterName)
			{
			case "volume":
			case "Volume":
				_fadeRunner?.CancelAll(entry.SoundId);
				BroAudio.SetVolume(entry.SoundId, value);
				return;
			case "pitch":
			case "Pitch":
				_fadeRunner?.CancelAll(entry.SoundId);
				BroAudio.SetPitch(entry.SoundId, value);
				return;
			}
			if (entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterName, value);
			}
		}

		public void SetParameter(AudioHandle handle, string parameterName, bool value)
		{
			if (TryGetEntry(handle, out var entry) && entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterName, value);
			}
		}

		public void SetParameter(AudioHandle handle, string parameterName, int value)
		{
			if (TryGetEntry(handle, out var entry) && entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				parameterizedPlayer.SetParameter(parameterName, value);
			}
		}

		public bool TryGetParameter(AudioHandle handle, string parameterName, out bool value)
		{
			value = false;
			if (!TryGetEntry(handle, out var entry))
			{
				return false;
			}
			if (entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterName, out value);
			}
			return false;
		}

		public bool TryGetParameter(AudioHandle handle, string parameterName, out int value)
		{
			value = 0;
			if (!TryGetEntry(handle, out var entry))
			{
				return false;
			}
			if (entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterName, out value);
			}
			return false;
		}

		public bool TryGetParameter(AudioHandle handle, string parameterName, out float value)
		{
			value = 0f;
			if (!TryGetEntry(handle, out var entry))
			{
				return false;
			}
			if (entry.Player is IParameterizedPlayer parameterizedPlayer)
			{
				return parameterizedPlayer.TryGetParameter(parameterName, out value);
			}
			return false;
		}

		public float GetParameter(AudioHandle handle, string parameterName)
		{
			switch (parameterName)
			{
			case "volume":
			case "Volume":
			case "pitch":
			case "Pitch":
				return 1f;
			default:
				return 0f;
			}
		}

		public void FadeVolume(AudioHandle handle, float target, float duration, AnimationCurve curve = null)
		{
			if (TryGetEntry(handle, out var entry))
			{
				if (curve == null && duration > 0f)
				{
					BroAudio.SetVolume(entry.SoundId, target, duration);
				}
				else
				{
					_fadeRunner?.FadeVolume(entry.SoundId, 1f, target, Mathf.Max(0f, duration), curve);
				}
			}
		}

		public void FadePitch(AudioHandle handle, float target, float duration, AnimationCurve curve = null)
		{
			if (TryGetEntry(handle, out var entry))
			{
				if (curve == null && duration > 0f)
				{
					BroAudio.SetPitch(entry.SoundId, target, duration);
				}
				else
				{
					_fadeRunner?.FadePitch(entry.SoundId, 1f, target, Mathf.Max(0f, duration), curve);
				}
			}
		}

		public void RampParameter(AudioHandle handle, string parameterName, float target, float duration, AnimationCurve curve = null)
		{
			switch (parameterName)
			{
			case "volume":
			case "Volume":
				FadeVolume(handle, target, duration, curve);
				break;
			case "pitch":
			case "Pitch":
				FadePitch(handle, target, duration, curve);
				break;
			}
		}

		public void SetBusVolume(AudioBus bus, float volume01)
		{
			float num = Mathf.Clamp(volume01, 0f, 1f);
			_busVolumeCache[bus] = num;
			BroAudio.SetVolume(MapBus(bus), num);
		}

		public float GetBusVolume(AudioBus bus)
		{
			if (!_busVolumeCache.TryGetValue(bus, out var value))
			{
				return 1f;
			}
			return value;
		}

		public void SetBusMuted(AudioBus bus, bool muted)
		{
			SetBusVolume(bus, muted ? 0f : 1f);
		}

		public void SetMasterVolume(float volume01)
		{
			SetBusVolume(AudioBus.Master, volume01);
		}

		public void SetMusicVolume(float volume01)
		{
			SetBusVolume(AudioBus.Music, volume01);
		}

		public void SetSfxVolume(float volume01)
		{
			SetBusVolume(AudioBus.Sfx, volume01);
		}

		public void SetAmbienceVolume(float volume01)
		{
			SetBusVolume(AudioBus.Ambience, volume01);
		}

		public void PauseAll()
		{
			AudioListener.pause = true;
		}

		public void ResumeAll()
		{
			AudioListener.pause = false;
		}

		public void PauseBus(AudioBus bus)
		{
			BroAudio.Pause(MapBus(bus));
		}

		public void ResumeBus(AudioBus bus)
		{
			BroAudio.UnPause(MapBus(bus));
		}

		public void TransitionToSnapshot(AudioMixerSnapshot snapshot, float transitionSeconds)
		{
			if (!(snapshot == null))
			{
				snapshot.TransitionTo(Mathf.Max(0f, transitionSeconds));
			}
		}

		public void TransitionToSnapshotPreset(AudioSnapshotPreset preset, float transitionSecondsOverride = -1f)
		{
			if (!(preset == null) && !(preset.snapshot == null))
			{
				float b = ((transitionSecondsOverride >= 0f) ? transitionSecondsOverride : preset.defaultTransitionSeconds);
				preset.snapshot.TransitionTo(Mathf.Max(0f, b));
			}
		}

		public void BlendSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float transitionSeconds)
		{
			if (snapshots != null && weights != null && snapshots.Length != 0 && snapshots.Length == weights.Length)
			{
				AudioMixer audioMixer = ((snapshots[0] != null) ? snapshots[0].audioMixer : null);
				if (!(audioMixer == null))
				{
					audioMixer.TransitionToSnapshots(snapshots, weights, Mathf.Max(0f, transitionSeconds));
				}
			}
		}

		private bool TryGetEntry(AudioHandle handle, out HandleEntry entry)
		{
			if (handle.Id != 0 && _activeHandles.TryGetValue(handle.Id, out entry) && entry.Player != null && entry.Player.IsActive)
			{
				return true;
			}
			entry = default(HandleEntry);
			return false;
		}

		private AudioHandle Track(IAudioPlayer player)
		{
			if (player == null || !player.IsActive)
			{
				return AudioHandle.Invalid;
			}
			SoundID iD = player.ID;
			uint id = AllocateId();
			_activeHandles[id] = new HandleEntry(player, iD);
			player.OnEnd(delegate
			{
				_activeHandles.Remove(id);
			});
			return new AudioHandle(id);
		}

		private uint AllocateId()
		{
			uint result = _nextHandleId++;
			if (_nextHandleId == 0)
			{
				_nextHandleId = 1u;
			}
			return result;
		}

		private static BroAudioType MapBus(AudioBus bus)
		{
			return bus switch
			{
				AudioBus.Master => BroAudioType.All, 
				AudioBus.Music => BroAudioType.Music, 
				AudioBus.Sfx => BroAudioType.SFX, 
				AudioBus.Ambience => BroAudioType.Ambience, 
				_ => BroAudioType.All, 
			};
		}
	}
}
