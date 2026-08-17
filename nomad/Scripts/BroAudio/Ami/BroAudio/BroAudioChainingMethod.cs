using System;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	public static class BroAudioChainingMethod
	{
		public static void Stop(this IAudioStoppable player)
		{
			player?.Stop();
		}

		public static void Stop(this IAudioStoppable player, Action onFinished)
		{
			player?.Stop(onFinished);
		}

		public static void Stop(this IAudioStoppable player, float fadeOut)
		{
			player?.Stop(fadeOut);
		}

		public static void Stop(this IAudioStoppable player, float fadeOut, Action onFinished)
		{
			player?.Stop(fadeOut, onFinished);
		}

		public static void Pause(this IAudioStoppable player)
		{
			player?.Pause();
		}

		public static void Pause(this IAudioStoppable player, float fadeOut)
		{
			player?.Pause(fadeOut);
		}

		public static void UnPause(this IAudioStoppable player)
		{
			player?.UnPause();
		}

		public static void UnPause(this IAudioStoppable player, float fadeOut)
		{
			player?.UnPause(fadeOut);
		}

		public static IAudioPlayer SetVolume(this IAudioPlayer player, float vol, float fadeTime = 0f)
		{
			return player?.SetVolume(vol, fadeTime);
		}

		public static IAudioPlayer SetVolume(this IMusicPlayer player, float vol, float fadeTime = 0f)
		{
			return player?.SetVolume(vol, fadeTime);
		}

		public static IAudioPlayer SetVolume(this IPlayerEffect player, float vol, float fadeTime = 0f)
		{
			return player?.SetVolume(vol, fadeTime);
		}

		public static IAudioPlayer SetPitch(this IAudioPlayer player, float pitch, float fadeTime = 0f)
		{
			return player?.SetPitch(pitch, fadeTime);
		}

		public static IAudioPlayer SetPitch(this IMusicPlayer player, float pitch, float fadeTime = 0f)
		{
			return player?.SetPitch(pitch, fadeTime);
		}

		public static IAudioPlayer SetPitch(this IPlayerEffect player, float pitch, float fadeTime = 0f)
		{
			return player?.SetPitch(pitch, fadeTime);
		}

		public static IAudioPlayer SetScheduledStartTime(this IAudioPlayer player, double dspTime)
		{
			return player?.SetScheduledStartTime(dspTime);
		}

		public static IAudioPlayer SetScheduledEndTime(this IAudioPlayer player, double dspTime)
		{
			return player?.SetScheduledEndTime(dspTime);
		}

		public static IAudioPlayer SetDelay(this IAudioPlayer player, float time)
		{
			return player?.SetDelay(time);
		}

		public static IAudioPlayer SetVelocity(this IAudioPlayer player, int velocity)
		{
			return player?.SetVelocity(velocity);
		}

		public static IMusicPlayer AsBGM(this IAudioPlayer player)
		{
			return player?.AsBGM();
		}

		public static IMusicPlayer AsBGM(this IPlayerEffect player)
		{
			return player?.AsBGM();
		}

		public static IAudioPlayer SetTransition(this IMusicPlayer player, Transition transition)
		{
			return player?.SetTransition(transition, -1f);
		}

		public static IAudioPlayer SetTransition(this IMusicPlayer player, Transition transition, float overrideFade)
		{
			return player?.SetTransition(transition, StopMode.Stop, overrideFade);
		}

		public static IAudioPlayer SetTransition(this IMusicPlayer player, Transition transition, StopMode stopMode)
		{
			return player?.SetTransition(transition, stopMode, -1f);
		}

		public static IAudioPlayer SetTransition(this IMusicPlayer player, Transition transition, StopMode stopMode, float overrideFade)
		{
			return player?.SetTransition(transition, stopMode, overrideFade);
		}

		public static IAudioPlayer OnAudioFilterRead(this IAudioPlayer player, Action<float[], int> onAudioFilterRead)
		{
			return player?.OnAudioFilterRead(onAudioFilterRead);
		}

		public static IAudioPlayer AddChorusEffect(this IAudioPlayer player, Action<IAudioChorusFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioChorusFilter, IAudioChorusFilterProxy>(onSet);
		}

		public static IAudioPlayer AddDistortionEffect(this IAudioPlayer player, Action<IAudioDistortionFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioDistortionFilter, IAudioDistortionFilterProxy>(onSet);
		}

		public static IAudioPlayer AddEchoEffect(this IAudioPlayer player, Action<IAudioEchoFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioEchoFilter, IAudioEchoFilterProxy>(onSet);
		}

		public static IAudioPlayer AddHighPassEffect(this IAudioPlayer player, Action<IAudioHighPassFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioHighPassFilter, IAudioHighPassFilterProxy>(onSet);
		}

		public static IAudioPlayer AddLowPassEffect(this IAudioPlayer player, Action<IAudioLowPassFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioLowPassFilter, IAudioLowPassFilterProxy>(onSet);
		}

		public static IAudioPlayer AddReverbEffect(this IAudioPlayer player, Action<IAudioReverbFilterProxy> onSet = null)
		{
			return player?.AddAudioEffect<AudioReverbFilter, IAudioReverbFilterProxy>(onSet);
		}

		public static IAudioPlayer RemoveChorusEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioChorusFilter>();
		}

		public static IAudioPlayer RemoveDistortionEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioDistortionFilter>();
		}

		public static IAudioPlayer RemoveEchoEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioEchoFilter>();
		}

		public static IAudioPlayer RemoveHighPassEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioHighPassFilter>();
		}

		public static IAudioPlayer RemoveLowPassEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioLowPassFilter>();
		}

		public static IAudioPlayer RemoveReverbEffect(this IAudioPlayer player)
		{
			return player?.RemoveAudioEffect<AudioReverbFilter>();
		}

		public static IPlayerEffect AsDominator(this IAudioPlayer player)
		{
			return player?.AsDominator();
		}

		public static IPlayerEffect AsDominator(this IMusicPlayer player)
		{
			return player?.AsDominator();
		}

		public static IPlayerEffect QuietOthers(this IPlayerEffect player, float othersVol, float fadeTime = 0.5f)
		{
			return player?.QuietOthers(othersVol, fadeTime);
		}

		public static IPlayerEffect QuietOthers(this IPlayerEffect player, float othersVol, Fading fading)
		{
			return player?.QuietOthers(othersVol, fading);
		}

		public static IPlayerEffect LowPassOthers(this IPlayerEffect player, float freq = 300f, float fadeTime = 0.5f)
		{
			return player?.LowPassOthers(freq, fadeTime);
		}

		public static IPlayerEffect LowPassOthers(this IPlayerEffect player, float freq, Fading fading)
		{
			return player?.LowPassOthers(freq, fading);
		}

		public static IPlayerEffect HighPassOthers(this IPlayerEffect player, float freq = 2000f, float fadeTime = 0.5f)
		{
			return player?.HighPassOthers(freq, fadeTime);
		}

		public static IPlayerEffect HighPassOthers(this IPlayerEffect player, float freq, Fading fading)
		{
			return player?.HighPassOthers(freq, fading);
		}
	}
}
