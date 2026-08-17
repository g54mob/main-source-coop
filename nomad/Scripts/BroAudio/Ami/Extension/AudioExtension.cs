using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.Extension
{
	public static class AudioExtension
	{
		public struct AudioClipSetting
		{
			public readonly int Frequency;

			public readonly int Channels;

			public readonly int Samples;

			public readonly bool Ambisonic;

			public readonly AudioClipLoadType LoadType;

			public readonly bool PreloadAudioData;

			public readonly bool LoadInBackground;

			public readonly AudioDataLoadState LoadState;

			public AudioClipSetting(AudioClip originClip, bool isMono)
			{
				Frequency = originClip.frequency;
				Channels = (isMono ? 1 : originClip.channels);
				Samples = originClip.samples;
				Ambisonic = originClip.ambisonic;
				LoadType = originClip.loadType;
				PreloadAudioData = originClip.preloadAudioData;
				LoadInBackground = originClip.loadInBackground;
				LoadState = originClip.loadState;
			}
		}

		private const float SecondsPerMinute = 60f;

		public static float ToDecibel(this float vol, bool allowBoost = true)
		{
			return Mathf.Log10(vol.ClampNormalize(allowBoost)) * 20f;
		}

		public static float ToNormalizeVolume(this float dB, bool allowBoost = true)
		{
			float num = (allowBoost ? 20f : 0f);
			if (dB >= num)
			{
				if (!allowBoost)
				{
					return 1f;
				}
				return 10f;
			}
			return Mathf.Pow(10f, dB.ClampDecibel(allowBoost) / 20f);
		}

		public static float ClampNormalize(this float vol, bool allowBoost = false)
		{
			return Mathf.Clamp(vol, 0.0001f, allowBoost ? 10f : 1f);
		}

		public static float ClampDecibel(this float dB, bool allowBoost = false)
		{
			return Mathf.Clamp(dB, -80f, allowBoost ? 20f : 0f);
		}

		public static bool TryGetSampleData(this AudioClip originClip, out float[] sampleArray, float startPosInSecond, float endPosInSecond)
		{
			int dataSample = originClip.GetDataSample(originClip.length - endPosInSecond - startPosInSecond);
			sampleArray = new float[dataSample];
			bool data = originClip.GetData(sampleArray, originClip.GetTimeSample(startPosInSecond));
			if (!data)
			{
				Debug.LogError("Can't get audio clip : " + originClip.name + " 's sample data!");
			}
			return data;
		}

		public static float[] GetSampleData(this AudioClip originClip, float startPosInSecond = 0f, float endPosInSecond = 0f)
		{
			if (originClip.TryGetSampleData(out var sampleArray, startPosInSecond, endPosInSecond))
			{
				return sampleArray;
			}
			return null;
		}

		public static AudioClip CreateAudioClip(string name, float[] samples, AudioClipSetting setting)
		{
			AudioClip audioClip = AudioClip.Create(name, samples.Length / setting.Channels, setting.Channels, setting.Frequency, setting.LoadType == AudioClipLoadType.Streaming);
			audioClip.SetData(samples, 0);
			return audioClip;
		}

		public static double GetPreciseLength(this AudioClip clip)
		{
			return (double)clip.samples / (double)clip.frequency;
		}

		public static double GetPreciseTime(this AudioSource source)
		{
			return (double)source.timeSamples / (double)source.clip.frequency;
		}

		public static int GetDataSample(this AudioClip clip, float time, MidpointRounding rounding = MidpointRounding.AwayFromZero)
		{
			return (int)Math.Round((float)(clip.frequency * clip.channels) * time, rounding);
		}

		public static int GetTimeSample(this AudioClip clip, float time, MidpointRounding rounding = MidpointRounding.AwayFromZero)
		{
			return (int)Math.Round((float)clip.frequency * time, rounding);
		}

		public static AudioClipSetting GetAudioClipSetting(this AudioClip audioClip, bool isMono = false)
		{
			return new AudioClipSetting(audioClip, isMono);
		}

		public static bool IsValidFrequency(float freq)
		{
			if (freq < 10f || freq > 22000f)
			{
				Debug.LogError($"The given frequency should be in {10f}Hz ~ {22000f}Hz.");
				return false;
			}
			return true;
		}

		public static float TempoToTime(float bpm, int beats)
		{
			if (bpm == 0f)
			{
				return 0f;
			}
			return 60f / bpm * (float)beats;
		}

		public static void ChangeChannel(this AudioMixer mixer, string from, string to, float targetVol)
		{
			mixer.SafeSetFloat(from, -80f);
			mixer.SafeSetFloat(to, targetVol);
		}

		public static bool SafeSetFloat(this AudioMixer mixer, string parameterName, float value)
		{
			if ((bool)mixer && !string.IsNullOrEmpty(parameterName))
			{
				mixer.SetFloat(parameterName, value);
				return true;
			}
			return false;
		}

		public static bool SafeGetFloat(this AudioMixer mixer, string parameterName, out float value)
		{
			value = 0f;
			if ((bool)mixer && !string.IsNullOrEmpty(parameterName))
			{
				return mixer.GetFloat(parameterName, out value);
			}
			return false;
		}
	}
}
