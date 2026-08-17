using System;
using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.Audio.Snapshots;
using UnityEngine;
using UnityEngine.Audio;

namespace EvilCore.Audio
{
	public interface IAudioManager
	{
		void PlayOneShot(SoundID id, Vector3 position = default(Vector3));

		void PlayOneShotAttached(SoundID id, GameObject attachTo);

		void PlayOneShotUI(SoundID id);

		AudioHandle PlayEvent(SoundID id, Vector3 position = default(Vector3));

		AudioHandle PlayEventAttached(SoundID id, GameObject attachTo);

		AudioHandle PlayLoopRegion(SoundID id, GameObject attachTo, bool skipIntro = false);

		void StopLoopRegion(AudioHandle handle);

		void StopEvent(AudioHandle handle, AudioStopMode stopMode = AudioStopMode.AllowFadeout, float fadeoutSeconds = 0.25f);

		void ReleaseInstance(AudioHandle handle);

		bool IsPlaying(AudioHandle handle);

		void SetParameter(AudioHandle handle, AudioParameter parameter, bool value);

		void SetParameter(AudioHandle handle, AudioParameter parameter, int value);

		void SetParameter(AudioHandle handle, AudioParameter parameter, float value);

		bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out bool value);

		bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out int value);

		bool TryGetParameter(AudioHandle handle, AudioParameter parameter, out float value);

		void SetParameter(AudioHandle handle, string parameterName, float value);

		[Obsolete("Use SetParameter(handle, AudioParameter, value) for entity parameters.")]
		void SetParameter(AudioHandle handle, string parameterName, bool value);

		[Obsolete("Use SetParameter(handle, AudioParameter, value) for entity parameters.")]
		void SetParameter(AudioHandle handle, string parameterName, int value);

		float GetParameter(AudioHandle handle, string parameterName);

		[Obsolete("Use TryGetParameter(handle, AudioParameter, out value) for entity parameters.")]
		bool TryGetParameter(AudioHandle handle, string parameterName, out bool value);

		[Obsolete("Use TryGetParameter(handle, AudioParameter, out value) for entity parameters.")]
		bool TryGetParameter(AudioHandle handle, string parameterName, out int value);

		bool TryGetParameter(AudioHandle handle, string parameterName, out float value);

		void FadeVolume(AudioHandle handle, float target, float duration, AnimationCurve curve = null);

		void FadePitch(AudioHandle handle, float target, float duration, AnimationCurve curve = null);

		void RampParameter(AudioHandle handle, string parameterName, float target, float duration, AnimationCurve curve = null);

		void SetBusVolume(AudioBus bus, float volume01);

		float GetBusVolume(AudioBus bus);

		void SetBusMuted(AudioBus bus, bool muted);

		void SetMasterVolume(float volume01);

		void SetMusicVolume(float volume01);

		void SetSfxVolume(float volume01);

		void SetAmbienceVolume(float volume01);

		void PauseAll();

		void ResumeAll();

		void PauseBus(AudioBus bus);

		void ResumeBus(AudioBus bus);

		void TransitionToSnapshot(AudioMixerSnapshot snapshot, float transitionSeconds);

		void TransitionToSnapshotPreset(AudioSnapshotPreset preset, float transitionSecondsOverride = -1f);

		void BlendSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float transitionSeconds);
	}
}
