using System;
using Ami.BroAudio.Data;
using Ami.BroAudio.Runtime;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	public class AudioPlayerInstanceWrapper : InstanceWrapper<AudioPlayer>, IAudioPlayer, IEffectDecoratable, IVolumeSettable, IMusicDecoratable, IAudioStoppable, ISchedulable, IMusicPlayer, IPlayerEffect, IParameterizedPlayer
	{
		public SoundID ID
		{
			get
			{
				if (!IsAvailable())
				{
					return SoundID.Invalid;
				}
				return base.Instance.ID;
			}
		}

		public bool IsActive
		{
			get
			{
				if (IsAvailable(logWarning: false))
				{
					return base.Instance.IsActive;
				}
				return false;
			}
		}

		public bool IsPlaying
		{
			get
			{
				if (IsAvailable(logWarning: false))
				{
					return base.Instance.IsPlaying;
				}
				return false;
			}
		}

		public IBroAudioClip CurrentPlayingClip => base.Instance?.CurrentPlayingClip;

		IAudioSourceProxy IAudioPlayer.AudioSource
		{
			get
			{
				if ((bool)base.Instance)
				{
					IAudioPlayer instance = base.Instance;
					if (instance != null)
					{
						return instance.AudioSource;
					}
				}
				return null;
			}
		}

		SoundID IMusicPlayer.ID => ID;

		public AudioPlayerInstanceWrapper(AudioPlayer instance)
			: base(instance)
		{
		}

		void IParameterizedPlayer.SetParameter(string name, bool value)
		{
			if (IsAvailable(logWarning: false))
			{
				base.Instance.SetParameter(name, value);
			}
		}

		void IParameterizedPlayer.SetParameter(string name, int value)
		{
			if (IsAvailable(logWarning: false))
			{
				base.Instance.SetParameter(name, value);
			}
		}

		void IParameterizedPlayer.SetParameter(string name, float value)
		{
			if (IsAvailable(logWarning: false))
			{
				base.Instance.SetParameter(name, value);
			}
		}

		bool IParameterizedPlayer.TryGetParameter(string name, out bool value)
		{
			if (IsAvailable(logWarning: false))
			{
				return base.Instance.TryGetParameter(name, out value);
			}
			value = false;
			return false;
		}

		bool IParameterizedPlayer.TryGetParameter(string name, out int value)
		{
			if (IsAvailable(logWarning: false))
			{
				return base.Instance.TryGetParameter(name, out value);
			}
			value = 0;
			return false;
		}

		bool IParameterizedPlayer.TryGetParameter(string name, out float value)
		{
			if (IsAvailable(logWarning: false))
			{
				return base.Instance.TryGetParameter(name, out value);
			}
			value = 0f;
			return false;
		}

		void IParameterizedPlayer.RequestLoopRegionExit()
		{
			if (IsAvailable(logWarning: false))
			{
				base.Instance.RequestLoopRegionExit();
			}
		}

		protected override void LogInstanceIsNull()
		{
			if (SoundManager.Instance.Setting.LogAccessRecycledPlayerWarning)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>Invalid operation. The audio player you're accessing has finished playing and has been recycled.");
			}
		}

		IMusicPlayer IMusicDecoratable.AsBGM()
		{
			if (!IsAvailable())
			{
				return Empty.MusicPlayer;
			}
			return Wrap(base.Instance.AsBGM());
		}

		IPlayerEffect IEffectDecoratable.AsDominator()
		{
			if (!IsAvailable())
			{
				return Empty.DominatorPlayer;
			}
			return Wrap(base.Instance.AsDominator());
		}

		IAudioPlayer IVolumeSettable.SetVolume(float vol, float fadeTime)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetVolume(vol, fadeTime));
		}

		IAudioPlayer IAudioPlayer.SetPitch(float pitch, float fadeTime)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetPitch(pitch, fadeTime));
		}

		IAudioPlayer IAudioPlayer.SetVelocity(int velocity)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetVelocity(velocity));
		}

		void IAudioStoppable.Stop()
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Stop();
			}
		}

		void IAudioStoppable.Stop(Action onFinished)
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Stop(onFinished);
			}
		}

		void IAudioStoppable.Stop(float fadeOut)
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Stop(fadeOut);
			}
		}

		void IAudioStoppable.Stop(float fadeOut, Action onFinished)
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Stop(fadeOut, onFinished);
			}
		}

		void IAudioStoppable.Pause()
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Pause();
			}
		}

		void IAudioStoppable.Pause(float fadeOut)
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.Pause(fadeOut);
			}
		}

		void IAudioStoppable.UnPause()
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.UnPause();
			}
		}

		void IAudioStoppable.UnPause(float fadeOut)
		{
			AudioPlayer instance = base.Instance;
			if ((object)instance != null)
			{
				instance.UnPause(fadeOut);
			}
		}

		IAudioPlayer IAudioPlayer.OnStart(Action<IAudioPlayer> onStart)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.OnStart(onStart));
		}

		IAudioPlayer IAudioPlayer.OnUpdate(Action<IAudioPlayer> onUpdate)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.OnUpdate(onUpdate));
		}

		IAudioPlayer IAudioPlayer.OnPause(Action<IAudioPlayer> onPause)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.OnPause(onPause));
		}

		IAudioPlayer IAudioPlayer.OnEnd(Action<SoundID> onEnd)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.OnEnd(onEnd));
		}

		public IAudioPlayer SetFadeInEase(Ease ease)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetFadeInEase(ease));
		}

		public IAudioPlayer SetFadeOutEase(Ease ease)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetFadeOutEase(ease));
		}

		IAudioPlayer ISchedulable.SetScheduledStartTime(double dspTime)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetScheduledStartTime(dspTime));
		}

		IAudioPlayer ISchedulable.SetScheduledEndTime(double dspTime)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetScheduledEndTime(dspTime));
		}

		IAudioPlayer ISchedulable.SetDelay(float time)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.SetDelay(time));
		}

		public void GetOutputData(float[] samples, int channels)
		{
			base.Instance?.GetOutputData(samples, channels);
		}

		public void GetSpectrumData(float[] samples, int channels, FFTWindow window)
		{
			base.Instance?.GetSpectrumData(samples, channels, window);
		}

		IAudioPlayer IAudioPlayer.AddAudioEffect<T, TProxy>(Action<TProxy> onSet)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(((IAudioPlayer)base.Instance).AddAudioEffect<T, TProxy>(onSet));
		}

		IAudioPlayer IAudioPlayer.RemoveAudioEffect<T>()
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(((IAudioPlayer)base.Instance).RemoveAudioEffect<T>());
		}

		IAudioPlayer IAudioPlayer.OnAudioFilterRead(Action<float[], int> onAudioFilterRead)
		{
			if (!IsAvailable())
			{
				return Empty.AudioPlayer;
			}
			return Wrap(base.Instance.OnAudioFilterRead(onAudioFilterRead));
		}

		IAudioPlayer IMusicPlayer.SetTransition(Transition transition, StopMode stopMode, float overrideFade)
		{
			MusicPlayer decorator = GetDecorator<MusicPlayer>();
			return Wrap((decorator != null) ? decorator.SetTransition(transition, stopMode, overrideFade) : null);
		}

		IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, float fadeTime)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.QuietOthers(othersVol, fadeTime) : null);
		}

		IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, Fading fading)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.QuietOthers(othersVol, fading) : null);
		}

		IPlayerEffect IPlayerEffect.LowPassOthers(float freq, float fadeTime)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.LowPassOthers(freq, fadeTime) : null);
		}

		IPlayerEffect IPlayerEffect.LowPassOthers(float freq, Fading fading)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.LowPassOthers(freq, fading) : null);
		}

		IPlayerEffect IPlayerEffect.HighPassOthers(float freq, float fadeTime)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.HighPassOthers(freq, fadeTime) : null);
		}

		IPlayerEffect IPlayerEffect.HighPassOthers(float freq, Fading fading)
		{
			DominatorPlayer decorator = GetDecorator<DominatorPlayer>();
			return Wrap((decorator != null) ? decorator.HighPassOthers(freq, fading) : null);
		}

		public override void UpdateInstance(AudioPlayer newInstance)
		{
			if (!IsAvailable(logWarning: false))
			{
				base.UpdateInstance(newInstance);
				return;
			}
			if (base.Instance.TransferOnUpdates(out var onUpdateDelegates))
			{
				Delegate[] array = onUpdateDelegates;
				foreach (Delegate obj in array)
				{
					newInstance.OnUpdate(obj as Action<IAudioPlayer>);
				}
			}
			if (base.Instance.TransferOnEnds(out var onEndDelegates))
			{
				Delegate[] array = onEndDelegates;
				foreach (Delegate obj2 in array)
				{
					newInstance.OnEnd(obj2 as Action<SoundID>);
				}
			}
			if (base.Instance.TransferOnPauses(out var onPauseDelegates))
			{
				Delegate[] array = onPauseDelegates;
				foreach (Delegate obj3 in array)
				{
					newInstance.OnPause(obj3 as Action<IAudioPlayer>);
				}
			}
			if (base.Instance.TransferDecorators(out var decorators))
			{
				foreach (AudioPlayerDecorator item in decorators)
				{
					item.UpdateInstance(newInstance);
				}
				newInstance.SetDecorators(decorators);
			}
			base.Instance.TransferAddedEffectComponents(newInstance);
			base.UpdateInstance(newInstance);
		}

		private IAudioPlayer Wrap(object method)
		{
			return this;
		}

		private IMusicPlayer Wrap(IMusicPlayer musicPlayer)
		{
			return this;
		}

		private IPlayerEffect Wrap(IPlayerEffect dominator)
		{
			return this;
		}

		private T GetDecorator<T>() where T : AudioPlayerDecorator
		{
			if (base.Instance.TryGetDecorator<T>(out var decorator))
			{
				return decorator;
			}
			return null;
		}

		public static explicit operator AudioPlayer(AudioPlayerInstanceWrapper wrapper)
		{
			if (!wrapper.IsAvailable(logWarning: false))
			{
				return null;
			}
			return wrapper.Instance;
		}
	}
}
