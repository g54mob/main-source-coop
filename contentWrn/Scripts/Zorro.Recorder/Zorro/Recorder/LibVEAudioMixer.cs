using System;
using Unity.Collections;

namespace Zorro.Recorder
{
	public class LibVEAudioMixer
	{
		public static readonly int MaxExpectedSamplesPerSecond = 48000;

		public static readonly int RemixToChannels = 2;

		public static void Resample(NativeList<float> audioData, int channels, int inputSampleRate, int outputSampleRate, ref NativeList<float> resampledAudioData)
		{
			if (inputSampleRate == outputSampleRate)
			{
				resampledAudioData.Dispose();
				resampledAudioData = new NativeList<float>(audioData.Length, Allocator.Persistent);
				resampledAudioData.CopyFrom(in audioData);
				return;
			}
			int num = audioData.Length / channels;
			int num2 = (int)((double)num * (double)outputSampleRate / (double)inputSampleRate);
			int length = num2 * channels;
			resampledAudioData.ResizeUninitialized(length);
			for (int i = 0; i < num2; i++)
			{
				double num3 = (double)i * (double)inputSampleRate / (double)outputSampleRate;
				int num4 = (int)num3;
				int num5 = Math.Min(num4 + 1, num - 1);
				double num6 = num3 - (double)num4;
				for (int j = 0; j < channels; j++)
				{
					float num7 = audioData[num4 * channels + j];
					float num8 = audioData[num5 * channels + j];
					float value = (float)((1.0 - num6) * (double)num7 + num6 * (double)num8);
					resampledAudioData[i * channels + j] = value;
				}
			}
		}

		public static void MixAudio(NativeList<float> audio1, int channels1, NativeList<float> audio2, int channels2, int outChannels, ref NativeList<float> mixedAudioData)
		{
			int num = Math.Max(audio1.Length / channels1, audio2.Length / channels2);
			int length = num * outChannels;
			mixedAudioData.ResizeUninitialized(length);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < outChannels; j++)
				{
					float num2 = 0f;
					if (i * channels1 + j < audio1.Length)
					{
						num2 += audio1[i * channels1 + j];
					}
					if (i * channels2 + j < audio2.Length)
					{
						num2 += audio2[i * channels2 + j];
					}
					mixedAudioData[i * outChannels + j] = num2 * 0.5f;
				}
			}
		}

		public static void DownmixToStereo(float[] surroundAudio, float gainMultiplier, int channels, ref NativeList<float> stereoAudio)
		{
			int num = surroundAudio.Length / channels;
			for (int i = 0; i < num; i++)
			{
				float num2 = 0f;
				float num3 = 0f;
				int num4 = 0;
				int num5 = 0;
				float num6 = surroundAudio[i * channels] * gainMultiplier;
				float num7 = surroundAudio[i * channels + 1] * gainMultiplier;
				float num8 = surroundAudio[i * channels + 2] * gainMultiplier;
				_ = surroundAudio[i * channels + 3];
				float num9 = surroundAudio[i * channels + 4] * gainMultiplier;
				float num10 = surroundAudio[i * channels + 5] * gainMultiplier;
				num2 += num6;
				num3 += num7;
				num4++;
				num5++;
				num2 += num8 * 0.707f;
				num3 += num8 * 0.707f;
				num4++;
				num5++;
				num2 += num9 * 0.707f;
				num3 += num10 * 0.707f;
				num4++;
				num5++;
				if (channels == 8)
				{
					float num11 = surroundAudio[i * channels + 6];
					float num12 = surroundAudio[i * channels + 7];
					num2 += num11 * 0.5f;
					num3 += num12 * 0.5f;
					num4++;
					num5++;
				}
				stereoAudio.Add(num2 / (float)num4);
				stereoAudio.Add(num3 / (float)num5);
			}
		}

		public static void UpmixMonoToStereo(float[] monoAudio, float gainMultiplier, ref NativeList<float> stereoAudio)
		{
			int num = monoAudio.Length;
			for (int i = 0; i < num; i++)
			{
				float value = monoAudio[i] * gainMultiplier;
				stereoAudio.Add(in value);
				stereoAudio.Add(in value);
			}
		}

		public static void RemixToStereo(float[] audio, float gainMultiplier, int channels, ref NativeList<float> stereoAudio)
		{
			switch (channels)
			{
			case 2:
			{
				for (int i = 0; i < audio.Length; i++)
				{
					stereoAudio.Add(audio[i] * gainMultiplier);
				}
				break;
			}
			case 1:
				UpmixMonoToStereo(audio, gainMultiplier, ref stereoAudio);
				break;
			default:
				DownmixToStereo(audio, gainMultiplier, channels, ref stereoAudio);
				break;
			}
		}
	}
}
