using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Voice
{
	public static class TauntVoiceClip
	{
		private const byte Version = 1;

		public const float MaxSeconds = 2f;

		public const float MinSeconds = 0.3f;

		public const int MaxFrames = 100;

		public const int MinFrames = 15;

		public const int MaxBytes = 16384;

		private const float MinLoudness = 0.02f;

		private const float MinLoudShare = 0.25f;

		private const float LoudFrameLevel = 0.015f;

		private const float MinVoiceBandShare = 0.4f;

		public static byte[] Pack(List<byte[]> frames)
		{
			if (frames == null || frames.Count == 0)
			{
				return Array.Empty<byte>();
			}
			List<byte[]> list = new List<byte[]>(Mathf.Min(frames.Count, 100));
			for (int i = 0; i < frames.Count; i++)
			{
				if (list.Count >= 100)
				{
					break;
				}
				byte[] array = frames[i];
				if (array != null && array.Length != 0 && array.Length <= 255)
				{
					list.Add(array);
				}
			}
			if (list.Count == 0)
			{
				return Array.Empty<byte>();
			}
			List<byte> list2 = new List<byte>(list.Count * 64 + 8)
			{
				1,
				1,
				(byte)list.Count
			};
			foreach (byte[] item in list)
			{
				list2.Add((byte)item.Length);
				list2.AddRange(item);
			}
			return list2.ToArray();
		}

		public static bool TryDecode(byte[] data, out float[] samples, out string rejection)
		{
			samples = null;
			rejection = null;
			if (data == null || data.Length < 3)
			{
				rejection = "Reject.TauntUnreadable";
				return false;
			}
			if (data.Length > 16384)
			{
				rejection = "Reject.TauntTooLarge";
				return false;
			}
			if (data[0] != 1 || data[1] != 1)
			{
				rejection = "Reject.TauntUnreadable";
				return false;
			}
			int num = data[2];
			if (num < 15 || num > 100)
			{
				rejection = ((num > 100) ? "Reject.TauntTooLong" : "Reject.TauntTooShort");
				return false;
			}
			float[] array = new float[num * 960];
			float[] array2 = new float[960];
			using (OpusVoiceCodec opusVoiceCodec = new OpusVoiceCodec(encode: false, decode: true))
			{
				int num2 = 3;
				for (int i = 0; i < num; i++)
				{
					if (num2 >= data.Length)
					{
						rejection = "Reject.TauntUnreadable";
						return false;
					}
					int num3 = data[num2++];
					if (num3 == 0 || num2 + num3 > data.Length)
					{
						rejection = "Reject.TauntUnreadable";
						return false;
					}
					int num4 = opusVoiceCodec.Decode(data, num2, num3, array2);
					num2 += num3;
					if (num4 != 960)
					{
						rejection = "Reject.TauntUnreadable";
						return false;
					}
					Array.Copy(array2, 0, array, i * 960, 960);
				}
			}
			samples = array;
			return true;
		}

		public static bool IsAudible(float[] samples, out string rejection)
		{
			rejection = null;
			if (samples == null || samples.Length < 14400)
			{
				rejection = "Reject.TauntTooShort";
				return false;
			}
			if (Rms(samples, 0, samples.Length) < 0.02f || LoudShare(samples) < 0.25f)
			{
				rejection = "Reject.TauntTooQuiet";
				return false;
			}
			if (VoiceBandShare(samples) < 0.4f)
			{
				rejection = "Reject.TauntNotVoice";
				return false;
			}
			return true;
		}

		public static bool Accepts(byte[] data, out string rejection)
		{
			if (TryDecode(data, out var samples, out rejection))
			{
				return IsAudible(samples, out rejection);
			}
			return false;
		}

		public static AudioClip ToClip(float[] samples, string name)
		{
			if (samples == null || samples.Length == 0)
			{
				return null;
			}
			AudioClip audioClip = AudioClip.Create(name, samples.Length, 1, 48000, stream: false);
			audioClip.SetData(samples, 0);
			return audioClip;
		}

		public static float[] Peaks(float[] samples)
		{
			if (samples == null || samples.Length == 0)
			{
				return Array.Empty<float>();
			}
			int num = Mathf.Max(1, samples.Length / 960);
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 960;
				int num3 = Mathf.Min(num2 + 960, samples.Length);
				float num4 = 0f;
				for (int j = num2; j < num3; j++)
				{
					num4 = Mathf.Max(num4, Mathf.Abs(samples[j]));
				}
				array[i] = Mathf.Clamp01(num4);
			}
			return array;
		}

		private static float Rms(float[] samples, int offset, int count)
		{
			if (count <= 0)
			{
				return 0f;
			}
			double num = 0.0;
			for (int i = offset; i < offset + count; i++)
			{
				num += (double)samples[i] * (double)samples[i];
			}
			return Mathf.Sqrt((float)(num / (double)count));
		}

		private static float LoudShare(float[] samples)
		{
			int num = samples.Length / 960;
			if (num == 0)
			{
				return 0f;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (Rms(samples, i * 960, 960) >= 0.015f)
				{
					num2++;
				}
			}
			return (float)num2 / (float)num;
		}

		private static float VoiceBandShare(float[] samples)
		{
			float num = HighPassAlpha(150f);
			float num2 = LowPassAlpha(4000f);
			double num3 = 0.0;
			double num4 = 0.0;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			foreach (float num8 in samples)
			{
				num3 += (double)num8 * (double)num8;
				num6 = num * (num6 + num8 - num5);
				num5 = num8;
				num7 += num2 * (num6 - num7);
				num4 += (double)num7 * (double)num7;
			}
			if (!(num3 <= 0.0))
			{
				return Mathf.Clamp01((float)(num4 / num3));
			}
			return 0f;
		}

		private static float LowPassAlpha(float cutoffHz)
		{
			if (cutoffHz >= 24000f)
			{
				return 1f;
			}
			float num = 1f / (MathF.PI * 2f * cutoffHz);
			float num2 = 2.0833333E-05f;
			return num2 / (num + num2);
		}

		private static float HighPassAlpha(float cutoffHz)
		{
			float num = 1f / (MathF.PI * 2f * cutoffHz);
			float num2 = 2.0833333E-05f;
			return num / (num + num2);
		}
	}
}
