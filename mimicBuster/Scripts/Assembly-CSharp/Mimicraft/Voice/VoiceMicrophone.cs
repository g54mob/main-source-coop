using System;
using Concentus;
using UnityEngine;

namespace Mimicraft.Voice
{
	public sealed class VoiceMicrophone : IDisposable
	{
		private const int ClipSeconds = 1;

		private const int ResamplerQuality = 3;

		private const float OpenLevel = 0.02f;

		private const float CloseLevel = 0.01f;

		private const float HoldSeconds = 0.35f;

		private const float LevelSmoothing = 0.25f;

		private float[] clipBuffer;

		private float[] linear;

		private readonly float[] resampleScratch;

		private readonly VoiceRing captured;

		private AudioClip clip;

		private IResampler resampler;

		private int lastPosition;

		private float openUntil;

		public string Device { get; private set; }

		public int DeviceRate { get; private set; }

		public bool Active => clip != null;

		public float Level { get; private set; }

		public bool Open { get; private set; }

		public int Buffered => captured.Count;

		public long Pumped { get; private set; }

		public float Gain { get; set; } = 1f;

		public VoiceMicrophone()
		{
			resampleScratch = new float[48000];
			captured = new VoiceRing(48000);
		}

		public bool Start(string device)
		{
			Stop();
			string[] devices = Microphone.devices;
			if (devices == null || devices.Length == 0)
			{
				return false;
			}
			if (string.IsNullOrEmpty(device) || Array.IndexOf(devices, device) < 0)
			{
				device = devices[0];
			}
			Device = device;
			try
			{
				clip = Microphone.Start(device, loop: true, 1, RequestRate(device));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Mikrofon açılamadı: " + ex.Message);
				clip = null;
				return false;
			}
			if (clip == null)
			{
				return false;
			}
			DeviceRate = clip.frequency;
			if (clipBuffer == null || clipBuffer.Length < clip.samples)
			{
				clipBuffer = new float[clip.samples];
				linear = new float[clip.samples];
			}
			lastPosition = 0;
			captured.Clear();
			resampler = ((DeviceRate == 48000) ? null : ResamplerFactory.CreateResampler(1, DeviceRate, 48000, 3));
			return true;
		}

		private static int RequestRate(string device)
		{
			Microphone.GetDeviceCaps(device, out var minFreq, out var maxFreq);
			if (minFreq == 0 && maxFreq == 0)
			{
				return 48000;
			}
			return Mathf.Clamp(48000, minFreq, maxFreq);
		}

		public void Stop()
		{
			if (clip != null)
			{
				Microphone.End(Device);
				UnityEngine.Object.Destroy(clip);
				clip = null;
			}
			resampler = null;
			captured.Clear();
			lastPosition = 0;
			Pumped = 0L;
			Level = 0f;
			Open = false;
			openUntil = 0f;
		}

		public bool TryReadFrame(float[] frame, out bool audible)
		{
			audible = false;
			if (clip == null || frame == null || frame.Length < 960)
			{
				return false;
			}
			Pump();
			if (captured.Count < 960)
			{
				return false;
			}
			captured.Read(frame, 0, 960);
			if (!Mathf.Approximately(Gain, 1f))
			{
				for (int i = 0; i < 960; i++)
				{
					frame[i] = Mathf.Clamp(frame[i] * Gain, -1f, 1f);
				}
			}
			float num = Rms(frame, 960);
			Level = Mathf.Lerp(Level, num, 0.25f);
			audible = Gate(num);
			return true;
		}

		private void Pump()
		{
			int position = Microphone.GetPosition(Device);
			if (position >= 0 && position != lastPosition)
			{
				int samples = clip.samples;
				int num = position - lastPosition;
				if (num < 0)
				{
					num += samples;
				}
				clip.GetData(clipBuffer, 0);
				int num2 = Mathf.Min(num, samples - lastPosition);
				Array.Copy(clipBuffer, lastPosition, linear, 0, num2);
				int num3 = num - num2;
				if (num3 > 0)
				{
					Array.Copy(clipBuffer, 0, linear, num2, num3);
				}
				lastPosition = position;
				Pumped += num;
				Deliver(num);
			}
		}

		private void Deliver(int samples)
		{
			if (resampler == null)
			{
				captured.Write(linear, 0, samples);
				return;
			}
			int in_len = samples;
			int out_len = resampleScratch.Length;
			resampler.ProcessInterleaved(new Span<float>(linear, 0, samples), ref in_len, new Span<float>(resampleScratch), ref out_len);
			if (out_len > 0)
			{
				captured.Write(resampleScratch, 0, out_len);
			}
		}

		private static float Rms(float[] samples, int count)
		{
			double num = 0.0;
			for (int i = 0; i < count; i++)
			{
				num += (double)(samples[i] * samples[i]);
			}
			return Mathf.Sqrt((float)(num / (double)count));
		}

		private bool Gate(float level)
		{
			if (level >= 0.02f)
			{
				Open = true;
				openUntil = Time.unscaledTime + 0.35f;
				return true;
			}
			if (Open && (level >= 0.01f || Time.unscaledTime < openUntil))
			{
				return true;
			}
			Open = false;
			return false;
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
