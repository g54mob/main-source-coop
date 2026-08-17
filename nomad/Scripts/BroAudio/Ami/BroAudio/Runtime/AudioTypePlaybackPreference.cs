using System;

namespace Ami.BroAudio.Runtime
{
	public class AudioTypePlaybackPreference : IAudioPlaybackPref
	{
		public struct SetEffectParameter
		{
			public EffectType EffectType;

			public SetEffectMode Mode;
		}

		public static readonly Action<AudioTypePlaybackPreference, float> OnSetVolume = SetVolume;

		public static readonly Action<AudioTypePlaybackPreference, float> OnSetpitch = SetPitch;

		public static readonly Action<AudioTypePlaybackPreference, SetEffectParameter> OnSetEffect = SetEffect;

		public float Volume { get; private set; } = 1f;

		public float Pitch { get; private set; } = 1f;

		public EffectType EffectType { get; private set; }

		private static void SetVolume(AudioTypePlaybackPreference pref, float vol)
		{
			pref.Volume = vol;
		}

		private static void SetPitch(AudioTypePlaybackPreference pref, float pitch)
		{
			pref.Pitch = pitch;
		}

		private static void SetEffect(AudioTypePlaybackPreference pref, SetEffectParameter parameter)
		{
			switch (parameter.Mode)
			{
			case SetEffectMode.Add:
				pref.EffectType |= parameter.EffectType;
				break;
			case SetEffectMode.Remove:
				pref.EffectType &= ~parameter.EffectType;
				break;
			case SetEffectMode.Override:
				pref.EffectType = parameter.EffectType;
				break;
			}
		}
	}
}
