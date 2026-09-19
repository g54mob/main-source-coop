using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicVoicePlaybackService : IMimicVoicePlaybackService, ITickable, IDisposable
	{
		private sealed class PlaybackHandle
		{
			public int Key { get; }

			public Sound Sound { get; private set; }

			public EventInstance EventInstance { get; }

			public MimicVoiceSegment Segment { get; }

			public Transform Source { get; }

			public Func<Transform> ListenerProvider { get; }

			public float StartedAtRealtime { get; }

			public float SegmentDurationSeconds { get; }

			public float StopAtRealtime { get; }

			public bool HasRequestedStop { get; set; }

			public PlaybackHandle(int key, Sound sound, EventInstance eventInstance, MimicVoiceSegment segment, Transform source, Func<Transform> listenerProvider, float startedAtRealtime, float segmentDurationSeconds, float stopAtRealtime)
			{
				Key = key;
				Sound = sound;
				EventInstance = eventInstance;
				Segment = segment;
				Source = source;
				ListenerProvider = listenerProvider;
				StartedAtRealtime = startedAtRealtime;
				SegmentDurationSeconds = segmentDurationSeconds;
				StopAtRealtime = stopAtRealtime;
			}
		}

		private const float SegmentStopPaddingSeconds = 0.05f;

		private const float LoudnessWindowSeconds = 0.05f;

		private const float ShortToFloat = 3.051851E-05f;

		private static int _nextPlaybackKey;

		private static readonly Dictionary<int, PlaybackHandle> PlaybackByKey = new Dictionary<int, PlaybackHandle>();

		private readonly IMimicVoiceSegmentProvider _segmentProvider;

		private readonly MimicVoiceCaptureConfiguration _configuration;

		private readonly List<PlaybackHandle> _activePlaybacks = new List<PlaybackHandle>();

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[8];

		public MimicVoicePlaybackService(IMimicVoiceSegmentProvider segmentProvider, MimicVoiceCaptureConfiguration configuration)
		{
			_segmentProvider = segmentProvider;
			_configuration = configuration;
		}

		public void Dispose()
		{
			for (int num = _activePlaybacks.Count - 1; num >= 0; num--)
			{
				ReleasePlayback(_activePlaybacks[num]);
			}
			_activePlaybacks.Clear();
		}

		public void Tick()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int num = _activePlaybacks.Count - 1; num >= 0; num--)
			{
				PlaybackHandle playbackHandle = _activePlaybacks[num];
				if (playbackHandle.EventInstance.isValid() && playbackHandle.Source != null)
				{
					ProcessPlaybackEffects(playbackHandle);
				}
				if (!playbackHandle.HasRequestedStop && realtimeSinceStartup >= playbackHandle.StopAtRealtime)
				{
					playbackHandle.HasRequestedStop = true;
					if (playbackHandle.EventInstance.isValid())
					{
						playbackHandle.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					}
				}
				if (!playbackHandle.EventInstance.isValid() || playbackHandle.EventInstance.getPlaybackState(out var state) != RESULT.OK || state == PLAYBACK_STATE.STOPPED)
				{
					ReleasePlayback(playbackHandle);
					_activePlaybacks.RemoveAt(num);
				}
			}
		}

		public bool PlayRandomForPlayer(int playerId, Transform source, Func<Transform> listenerProvider, int seed, EventReference voiceEvent)
		{
			if (source == null)
			{
				return false;
			}
			if (voiceEvent.IsNull)
			{
				return false;
			}
			if (!_segmentProvider.TryGetSegmentBySeed(playerId, seed, out var segment))
			{
				return false;
			}
			if (!TryCreateSound(segment, out var sound))
			{
				return false;
			}
			EventInstance eventInstance = RuntimeManager.CreateInstance(voiceEvent);
			if (!eventInstance.isValid())
			{
				sound.release();
				sound.clearHandle();
				return false;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = Mathf.Max(0.05f, segment.DurationSeconds);
			float stopAtRealtime = realtimeSinceStartup + num + 0.05f;
			int num2 = ++_nextPlaybackKey;
			PlaybackHandle playbackHandle = new PlaybackHandle(num2, sound, eventInstance, segment, source, listenerProvider, realtimeSinceStartup, num, stopAtRealtime);
			lock (PlaybackByKey)
			{
				PlaybackByKey[num2] = playbackHandle;
			}
			eventInstance.setUserData(new IntPtr(num2));
			eventInstance.setCallback(ProgrammerSoundEventCallback);
			eventInstance.set3DAttributes(source.position.To3DAttributes());
			RuntimeManager.AttachInstanceToGameObject(eventInstance, source.gameObject);
			if (eventInstance.start() != RESULT.OK)
			{
				ReleasePlayback(playbackHandle);
				return false;
			}
			_segmentProvider.MarkSegmentPlayed(playerId, segment.SegmentId);
			ProcessPlaybackEffects(playbackHandle);
			_activePlaybacks.Add(playbackHandle);
			return true;
		}

		public bool TryGetNormalizedLoudness(Transform source, out float loudness)
		{
			loudness = 0f;
			if (source == null)
			{
				return false;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int i = 0; i < _activePlaybacks.Count; i++)
			{
				PlaybackHandle playbackHandle = _activePlaybacks[i];
				if (IsMatchingSource(playbackHandle.Source, source) && TryGetPlaybackLoudness(playbackHandle, realtimeSinceStartup, out var loudness2))
				{
					loudness = Mathf.Max(loudness, loudness2);
				}
			}
			return loudness > 0f;
		}

		private void ProcessPlaybackEffects(PlaybackHandle playback)
		{
			ProcessVoiceIntensity(playback, Time.realtimeSinceStartup);
			ProcessDistanceBasedAttenuation(playback);
			ProcessDistanceBasedEffects(playback);
			ProcessSoundOcclusion(playback);
		}

		private void ProcessVoiceIntensity(PlaybackHandle playback, float now)
		{
			if (!(playback.SegmentDurationSeconds <= 0f))
			{
				float num = Mathf.Clamp01((now - playback.StartedAtRealtime) / playback.SegmentDurationSeconds);
				float num2 = Mathf.Clamp01(_configuration.VoiceIntensityFadeStartNormalizedTime);
				float value = 1f;
				if (num >= num2)
				{
					float num3 = Mathf.Max(0.001f, 1f - num2);
					float t = Mathf.Clamp01((num - num2) / num3);
					value = Mathf.Lerp(1f, 0f, t);
				}
				SetParameter(playback.EventInstance, _configuration.VoiceIntensityParameterName, value);
				if (num >= 1f && !playback.HasRequestedStop)
				{
					playback.HasRequestedStop = true;
					playback.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _configuration.VerticalDistanceMultiplier;
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
			float t = Mathf.Abs(offset.y) * _configuration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _configuration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void ProcessSoundOcclusion(PlaybackHandle playback)
		{
			if (!TryGetListenerPosition(playback, out var position))
			{
				return;
			}
			Vector3 offset = playback.Source.position - position;
			float magnitude = offset.magnitude;
			if (magnitude <= Mathf.Epsilon)
			{
				SetParameter(playback.EventInstance, _configuration.LowPassParameterName, 1f);
				return;
			}
			int num = Physics.RaycastNonAlloc(position, offset.normalized, _occlusionHits, magnitude, _configuration.OcclusionLayerMask);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Transform transform = ((_occlusionHits[i].collider != null) ? _occlusionHits[i].collider.transform : null);
				if (transform != null && !transform.IsChildOf(playback.Source))
				{
					flag = true;
					break;
				}
			}
			float value = 1f;
			if (flag)
			{
				float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _configuration.MaxOcclusionDistance);
				value = Mathf.Lerp(1f, _configuration.MinLowPassValue, normalizedOcclusionDistance);
			}
			SetParameter(playback.EventInstance, _configuration.LowPassParameterName, value);
		}

		private void ProcessDistanceBasedAttenuation(PlaybackHandle playback)
		{
			if (_configuration.ProcessDistanceAttenuation && TryGetListenerPosition(playback, out var position))
			{
				Vector3 offset = playback.Source.position - position;
				float maxAttenuationDistance = _configuration.MaxAttenuationDistance;
				float num = ((maxAttenuationDistance > 0f) ? GetNormalizedOcclusionDistance(offset, maxAttenuationDistance) : 0f);
				float value = ((_configuration.AttenuationCurve != null) ? _configuration.AttenuationCurve.Evaluate(num) : (1f - num));
				SetParameter(playback.EventInstance, _configuration.DistanceParameterName, value);
			}
		}

		private void ProcessDistanceBasedEffects(PlaybackHandle playback)
		{
			if (TryGetListenerPosition(playback, out var position))
			{
				float weightedOcclusionDistance = GetWeightedOcclusionDistance(playback.Source.position - position);
				float value;
				float value2;
				if (weightedOcclusionDistance > _configuration.MinDistanceForEffects)
				{
					float num = _configuration.MaxDistanceForEffects - _configuration.MinDistanceForEffects;
					float t = ((num > 0f) ? Mathf.Clamp01((weightedOcclusionDistance - _configuration.MinDistanceForEffects) / num) : 1f);
					value = Mathf.Lerp(_configuration.MinDelay, _configuration.MaxDelay, t);
					value2 = Mathf.Lerp(_configuration.MinReverb, _configuration.MaxReverb, t);
				}
				else
				{
					value = _configuration.MinDelay;
					value2 = _configuration.MinReverb;
				}
				SetParameter(playback.EventInstance, _configuration.DelayParameterName, value);
				SetParameter(playback.EventInstance, _configuration.ReverbParameterName, value2);
			}
		}

		private bool TryGetListenerPosition(PlaybackHandle playback, out Vector3 position)
		{
			Transform transform = playback.ListenerProvider?.Invoke();
			if (transform == null)
			{
				position = default(Vector3);
				return false;
			}
			position = transform.position;
			return true;
		}

		private bool TryGetPlaybackLoudness(PlaybackHandle playback, float now, out float loudness)
		{
			loudness = 0f;
			MimicVoiceSegment segment = playback.Segment;
			if (segment?.Samples == null || segment.Samples.Length == 0 || segment.SamplingRate <= 0)
			{
				return false;
			}
			float num = now - playback.StartedAtRealtime;
			if (num < 0f || num > playback.SegmentDurationSeconds)
			{
				return false;
			}
			int num2 = Mathf.Clamp(Mathf.CeilToInt(num * (float)segment.SamplingRate), 0, segment.Samples.Length);
			if (num2 <= 0)
			{
				return false;
			}
			int num3 = Mathf.Max(1, Mathf.CeilToInt(0.05f * (float)segment.SamplingRate));
			int num4 = Mathf.Max(0, num2 - num3);
			int num5 = num2 - num4;
			if (num5 <= 0)
			{
				return false;
			}
			double num6 = 0.0;
			for (int i = num4; i < num2; i++)
			{
				float num7 = (float)segment.Samples[i] * 3.051851E-05f;
				num6 += (double)(num7 * num7);
			}
			loudness = Mathf.Clamp01(Mathf.Sqrt((float)(num6 / (double)num5)));
			return loudness > 0f;
		}

		private bool IsMatchingSource(Transform playbackSource, Transform requestedSource)
		{
			if (playbackSource == null || requestedSource == null)
			{
				return false;
			}
			if (!(playbackSource == requestedSource) && !playbackSource.IsChildOf(requestedSource))
			{
				return requestedSource.IsChildOf(playbackSource);
			}
			return true;
		}

		private void SetParameter(EventInstance eventInstance, string parameterName, float value)
		{
			if (!string.IsNullOrEmpty(parameterName) && eventInstance.isValid())
			{
				eventInstance.setParameterByName(parameterName, value);
			}
		}

		[MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
		private static RESULT ProgrammerSoundEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
		{
			EventInstance eventInstance = new EventInstance
			{
				handle = instancePtr
			};
			eventInstance.getUserData(out var userdata);
			int key = userdata.ToInt32();
			PlaybackHandle value;
			lock (PlaybackByKey)
			{
				if (!PlaybackByKey.TryGetValue(key, out value))
				{
					return RESULT.OK;
				}
			}
			switch (type)
			{
			case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
			{
				PROGRAMMER_SOUND_PROPERTIES structure = Marshal.PtrToStructure<PROGRAMMER_SOUND_PROPERTIES>(parameterPtr);
				structure.sound = value.Sound.handle;
				structure.subsoundIndex = -1;
				Marshal.StructureToPtr(structure, parameterPtr, fDeleteOld: false);
				break;
			}
			}
			return RESULT.OK;
		}

		private bool TryCreateSound(MimicVoiceSegment segment, out Sound sound)
		{
			sound = default(Sound);
			if (segment.Samples == null || segment.Samples.Length == 0)
			{
				return false;
			}
			CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO
			{
				cbsize = Marshal.SizeOf(typeof(CREATESOUNDEXINFO)),
				numchannels = segment.Channels,
				format = SOUND_FORMAT.PCM16,
				defaultfrequency = segment.SamplingRate,
				length = (uint)(segment.Samples.Length * 2)
			};
			if (RuntimeManager.CoreSystem.createSound($"MimicVoice_{segment.PlayerId}_{segment.SegmentId}", MODE.CREATECOMPRESSEDSAMPLE | MODE.OPENUSER, ref exinfo, out sound) != RESULT.OK)
			{
				return false;
			}
			if (sound.@lock(0u, exinfo.length, out var ptr, out var ptr2, out var len, out var len2) != RESULT.OK)
			{
				sound.release();
				sound.clearHandle();
				return false;
			}
			int num = (int)len / 2;
			int length = (int)len2 / 2;
			Marshal.Copy(segment.Samples, 0, ptr, num);
			if (ptr2 != IntPtr.Zero)
			{
				Marshal.Copy(segment.Samples, num, ptr2, length);
			}
			if (sound.unlock(ptr, ptr2, len, len2) != RESULT.OK)
			{
				sound.release();
				sound.clearHandle();
				return false;
			}
			return true;
		}

		private void ReleasePlayback(PlaybackHandle playback)
		{
			lock (PlaybackByKey)
			{
				PlaybackByKey.Remove(playback.Key);
			}
			if (playback.EventInstance.isValid())
			{
				playback.EventInstance.setCallback(null);
				playback.EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				if (playback.EventInstance.isValid())
				{
					playback.EventInstance.release();
				}
			}
			if (playback.Source != null)
			{
				RuntimeManager.DetachInstanceFromGameObject(playback.EventInstance);
			}
			if (playback.Sound.hasHandle())
			{
				playback.Sound.release();
				playback.Sound.clearHandle();
			}
		}
	}
}
