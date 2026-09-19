using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicVoiceArchiveConverter : IMimicVoiceArchiveConverter
	{
		private const float ShortMaxValue = 32767f;

		private const float ShortToFloat = 3.051851E-05f;

		public float CalculateRms(float[] samples, int channels)
		{
			if (samples == null || samples.Length == 0 || channels <= 0)
			{
				return 0f;
			}
			int num = samples.Length / channels;
			if (num <= 0)
			{
				return 0f;
			}
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				float num3 = ReadMonoSample(samples, channels, i);
				num2 += (double)(num3 * num3);
			}
			return Mathf.Sqrt((float)(num2 / (double)num));
		}

		public float CalculateRms(short[] samples, int channels)
		{
			if (samples == null || samples.Length == 0 || channels <= 0)
			{
				return 0f;
			}
			int num = samples.Length / channels;
			if (num <= 0)
			{
				return 0f;
			}
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				float num3 = ReadMonoSample(samples, channels, i);
				num2 += (double)(num3 * num3);
			}
			return Mathf.Sqrt((float)(num2 / (double)num));
		}

		public void AppendResampledMonoPcm16(List<short> destination, float[] sourceSamples, int sourceSamplingRate, int sourceChannels, int targetSamplingRate, ref float previousMonoSample, ref bool hasPreviousMonoSample, ref double phase)
		{
			if (destination == null || sourceSamples == null || sourceSamples.Length == 0 || sourceSamplingRate <= 0 || sourceChannels <= 0 || targetSamplingRate <= 0)
			{
				return;
			}
			int num = sourceSamples.Length / sourceChannels;
			if (num <= 0)
			{
				return;
			}
			double num2 = (double)sourceSamplingRate / (double)targetSamplingRate;
			while (phase < (double)num)
			{
				int num3 = (int)Math.Floor(phase);
				double num4 = phase - (double)num3;
				if (num3 + 1 >= num && num4 > 0.0)
				{
					break;
				}
				float a = ((num3 < 0) ? previousMonoSample : ReadMonoSample(sourceSamples, sourceChannels, Mathf.Clamp(num3, 0, num - 1)));
				float num5 = ((num3 + 1 < 0) ? previousMonoSample : ReadMonoSample(sourceSamples, sourceChannels, Mathf.Clamp(num3 + 1, 0, num - 1)));
				if (num3 < 0 && !hasPreviousMonoSample)
				{
					a = num5;
				}
				destination.Add(FloatToPcm16(Mathf.Lerp(a, num5, (float)num4)));
				phase += num2;
			}
			previousMonoSample = ReadMonoSample(sourceSamples, sourceChannels, num - 1);
			hasPreviousMonoSample = true;
			phase -= num;
		}

		public void AppendResampledMonoPcm16(List<short> destination, short[] sourceSamples, int sourceSamplingRate, int sourceChannels, int targetSamplingRate, ref float previousMonoSample, ref bool hasPreviousMonoSample, ref double phase)
		{
			if (destination == null || sourceSamples == null || sourceSamples.Length == 0 || sourceSamplingRate <= 0 || sourceChannels <= 0 || targetSamplingRate <= 0)
			{
				return;
			}
			int num = sourceSamples.Length / sourceChannels;
			if (num <= 0)
			{
				return;
			}
			double num2 = (double)sourceSamplingRate / (double)targetSamplingRate;
			while (phase < (double)num)
			{
				int num3 = (int)Math.Floor(phase);
				double num4 = phase - (double)num3;
				if (num3 + 1 >= num && num4 > 0.0)
				{
					break;
				}
				float a = ((num3 < 0) ? previousMonoSample : ReadMonoSample(sourceSamples, sourceChannels, Mathf.Clamp(num3, 0, num - 1)));
				float num5 = ((num3 + 1 < 0) ? previousMonoSample : ReadMonoSample(sourceSamples, sourceChannels, Mathf.Clamp(num3 + 1, 0, num - 1)));
				if (num3 < 0 && !hasPreviousMonoSample)
				{
					a = num5;
				}
				destination.Add(FloatToPcm16(Mathf.Lerp(a, num5, (float)num4)));
				phase += num2;
			}
			previousMonoSample = ReadMonoSample(sourceSamples, sourceChannels, num - 1);
			hasPreviousMonoSample = true;
			phase -= num;
		}

		private static float ReadMonoSample(float[] samples, int channels, int frameIndex)
		{
			int num = frameIndex * channels;
			if (channels == 1)
			{
				return samples[num];
			}
			float num2 = 0f;
			for (int i = 0; i < channels; i++)
			{
				num2 += samples[num + i];
			}
			return num2 / (float)channels;
		}

		private static float ReadMonoSample(short[] samples, int channels, int frameIndex)
		{
			int num = frameIndex * channels;
			if (channels == 1)
			{
				return (float)samples[num] * 3.051851E-05f;
			}
			float num2 = 0f;
			for (int i = 0; i < channels; i++)
			{
				num2 += (float)samples[num + i] * 3.051851E-05f;
			}
			return num2 / (float)channels;
		}

		private static short FloatToPcm16(float sample)
		{
			return (short)Mathf.RoundToInt(Mathf.Clamp(sample, -1f, 1f) * 32767f);
		}
	}
}
