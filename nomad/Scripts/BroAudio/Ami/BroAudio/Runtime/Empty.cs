using System;
using Ami.BroAudio.Data;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public static class Empty
	{
		public class EmptyAudioPlayer : IAudioPlayer, IEffectDecoratable, IVolumeSettable, IMusicDecoratable, IAudioStoppable, ISchedulable
		{
			SoundID IAudioPlayer.ID => SoundID.Invalid;

			bool IAudioPlayer.IsActive => false;

			bool IAudioPlayer.IsPlaying => false;

			IAudioSourceProxy IAudioPlayer.AudioSource => null;

			public IBroAudioClip CurrentPlayingClip => null;

			IMusicPlayer IMusicDecoratable.AsBGM()
			{
				return MusicPlayer;
			}

			IPlayerEffect IEffectDecoratable.AsDominator()
			{
				return DominatorPlayer;
			}

			void IAudioPlayer.GetOutputData(float[] samples, int channels)
			{
			}

			void IAudioPlayer.GetSpectrumData(float[] samples, int channels, FFTWindow window)
			{
			}

			IAudioPlayer IAudioPlayer.AddAudioEffect<T, TProxy>(Action<TProxy> onSet)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.RemoveAudioEffect<T>()
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.OnAudioFilterRead(Action<float[], int> onAudioFilterRead)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.OnEnd(Action<SoundID> onEnd)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.OnStart(Action<IAudioPlayer> onStart)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.OnUpdate(Action<IAudioPlayer> onUpdate)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.OnPause(Action<IAudioPlayer> onPause)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.SetFadeInEase(Ease ease)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.SetFadeOutEase(Ease ease)
			{
				return this;
			}

			void IAudioStoppable.Pause()
			{
			}

			void IAudioStoppable.Pause(float fadeOut)
			{
			}

			void IAudioStoppable.UnPause()
			{
			}

			void IAudioStoppable.UnPause(float fadeOut)
			{
			}

			IAudioPlayer IAudioPlayer.SetPitch(float pitch, float fadeTime)
			{
				return this;
			}

			IAudioPlayer IAudioPlayer.SetVelocity(int velocity)
			{
				return this;
			}

			IAudioPlayer IVolumeSettable.SetVolume(float vol, float fadeTime)
			{
				return this;
			}

			void IAudioStoppable.Stop()
			{
			}

			void IAudioStoppable.Stop(Action onFinished)
			{
			}

			void IAudioStoppable.Stop(float fadeOut)
			{
			}

			void IAudioStoppable.Stop(float fadeOut, Action onFinished)
			{
			}

			IAudioPlayer ISchedulable.SetScheduledStartTime(double dspTime)
			{
				return this;
			}

			IAudioPlayer ISchedulable.SetScheduledEndTime(double dspTime)
			{
				return this;
			}

			IAudioPlayer ISchedulable.SetDelay(float time)
			{
				return this;
			}
		}

		public class EmptyMusicPlayer : EmptyAudioPlayer, IMusicPlayer, IEffectDecoratable, IVolumeSettable, IAudioStoppable
		{
			SoundID IMusicPlayer.ID => SoundID.Invalid;

			IAudioPlayer IMusicPlayer.SetTransition(Transition transition, StopMode stopMode, float overrideFade)
			{
				return AudioPlayer;
			}
		}

		public class EmptyDominator : EmptyAudioPlayer, IPlayerEffect, IVolumeSettable, IMusicDecoratable, IAudioStoppable
		{
			IPlayerEffect IPlayerEffect.HighPassOthers(float freq, float fadeTime)
			{
				return this;
			}

			IPlayerEffect IPlayerEffect.HighPassOthers(float freq, Fading fading)
			{
				return this;
			}

			IPlayerEffect IPlayerEffect.LowPassOthers(float freq, float fadeTime)
			{
				return this;
			}

			IPlayerEffect IPlayerEffect.LowPassOthers(float freq, Fading fading)
			{
				return this;
			}

			IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, float fadeTime)
			{
				return this;
			}

			IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, Fading fading)
			{
				return this;
			}
		}

		public static EmptyAudioPlayer AudioPlayer = new EmptyAudioPlayer();

		public static EmptyMusicPlayer MusicPlayer = new EmptyMusicPlayer();

		public static EmptyAudioSourceProxy AudioSource = new EmptyAudioSourceProxy();

		public static EmptyDominator DominatorPlayer = new EmptyDominator();
	}
}
