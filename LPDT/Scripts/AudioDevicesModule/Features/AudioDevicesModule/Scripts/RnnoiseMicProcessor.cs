using System;
using Adrenak.RNNoise4Unity;
using UnityEngine;

namespace Features.AudioDevicesModule.Scripts
{
	public sealed class RnnoiseMicProcessor : IDisposable
	{
		private const int RNNOISE_RATE = 48000;

		private const float SHORT_TO_FLOAT = 3.0517578E-05f;

		private const float FLOAT_TO_SHORT = 32767f;

		private const string LOG_PREFIX = "[RnnoiseMicProcessor] ";

		private Denoiser _denoiser;

		private bool _isNativeUnavailable;

		private float[] _monoSource;

		private float[] _mono48k;

		private float[] _monoOut;

		public void Reset()
		{
			DisposeDenoiser();
		}

		public void Process(short[] buffer, int samplingRate, int channels)
		{
			if (buffer == null || buffer.Length == 0 || samplingRate <= 0 || channels <= 0 || _isNativeUnavailable || !TryEnsureDenoiser())
			{
				return;
			}
			int num = buffer.Length / channels;
			if (num <= 0)
			{
				return;
			}
			EnsureBuffer(ref _monoSource, num);
			DownmixToMono(buffer, channels, num, _monoSource);
			float[] array;
			int num2;
			if (samplingRate == 48000)
			{
				array = _monoSource;
				num2 = num;
			}
			else
			{
				num2 = (int)((long)num * 48000L / samplingRate);
				if (num2 <= 0)
				{
					return;
				}
				EnsureBuffer(ref _mono48k, num2);
				Resample(_monoSource, num, _mono48k, num2);
				array = _mono48k;
			}
			_denoiser.Denoise(array.AsSpan(0, num2));
			float[] mono;
			if (samplingRate == 48000)
			{
				mono = array;
			}
			else
			{
				EnsureBuffer(ref _monoOut, num);
				Resample(array, num2, _monoOut, num);
				mono = _monoOut;
			}
			WriteMonoToBuffer(mono, num, buffer, channels);
		}

		public void Dispose()
		{
			DisposeDenoiser();
		}

		private bool TryEnsureDenoiser()
		{
			if (_denoiser != null)
			{
				return true;
			}
			try
			{
				_denoiser = new Denoiser();
				return true;
			}
			catch (Exception ex)
			{
				_isNativeUnavailable = true;
				Debug.LogWarning("[RnnoiseMicProcessor] Failed to create denoiser: " + ex.Message);
				return false;
			}
		}

		private void DisposeDenoiser()
		{
			if (_denoiser != null)
			{
				_denoiser.Dispose();
				_denoiser = null;
			}
		}

		private static void EnsureBuffer(ref float[] buffer, int length)
		{
			if (buffer == null || buffer.Length < length)
			{
				buffer = new float[length];
			}
		}

		private static void DownmixToMono(short[] buffer, int channels, int frames, float[] mono)
		{
			for (int i = 0; i < frames; i++)
			{
				float num = 0f;
				int num2 = i * channels;
				for (int j = 0; j < channels; j++)
				{
					num += (float)buffer[num2 + j] * 3.0517578E-05f;
				}
				mono[i] = num / (float)channels;
			}
		}

		private static void Resample(float[] source, int sourceLength, float[] destination, int destinationLength)
		{
			if (sourceLength == destinationLength)
			{
				Array.Copy(source, destination, destinationLength);
				return;
			}
			if (destinationLength == 1)
			{
				destination[0] = source[0];
				return;
			}
			float num = (float)(sourceLength - 1) / (float)(destinationLength - 1);
			for (int i = 0; i < destinationLength; i++)
			{
				float num2 = (float)i * num;
				int num3 = (int)num2;
				float num4 = num2 - (float)num3;
				if (num3 >= sourceLength - 1)
				{
					destination[i] = source[sourceLength - 1];
				}
				else
				{
					destination[i] = source[num3] + (source[num3 + 1] - source[num3]) * num4;
				}
			}
		}

		private static void WriteMonoToBuffer(float[] mono, int frames, short[] buffer, int channels)
		{
			for (int i = 0; i < frames; i++)
			{
				float num = mono[i];
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < -1f)
				{
					num = -1f;
				}
				short num2 = (short)(num * 32767f);
				int num3 = i * channels;
				for (int j = 0; j < channels; j++)
				{
					buffer[num3 + j] = num2;
				}
			}
		}
	}
}
