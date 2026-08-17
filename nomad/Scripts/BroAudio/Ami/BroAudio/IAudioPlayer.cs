using System;
using Ami.BroAudio.Data;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	public interface IAudioPlayer : IEffectDecoratable, IVolumeSettable, IMusicDecoratable, IAudioStoppable, ISchedulable
	{
		SoundID ID { get; }

		bool IsActive { get; }

		bool IsPlaying { get; }

		IBroAudioClip CurrentPlayingClip { get; }

		IAudioSourceProxy AudioSource { get; }

		internal IAudioPlayer SetVelocity(int velocity);

		internal IAudioPlayer SetPitch(float pitch, float fadeTime);

		IAudioPlayer OnStart(Action<IAudioPlayer> onStart);

		IAudioPlayer OnUpdate(Action<IAudioPlayer> onUpdate);

		IAudioPlayer OnPause(Action<IAudioPlayer> onPause);

		IAudioPlayer OnEnd(Action<SoundID> onEnd);

		IAudioPlayer SetFadeInEase(Ease ease);

		IAudioPlayer SetFadeOutEase(Ease ease);

		internal IAudioPlayer OnAudioFilterRead(Action<float[], int> onAudioFilterRead);

		void GetOutputData(float[] samples, int channels);

		void GetSpectrumData(float[] samples, int channels, FFTWindow window);

		internal IAudioPlayer AddAudioEffect<T, TProxy>(Action<TProxy> onSet) where T : Behaviour where TProxy : class;

		internal IAudioPlayer RemoveAudioEffect<T>() where T : Behaviour;
	}
}
