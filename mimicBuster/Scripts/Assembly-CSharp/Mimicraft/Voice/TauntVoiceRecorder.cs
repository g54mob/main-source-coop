using System;
using System.Collections.Generic;
using Mimicraft.Settings;
using UnityEngine;

namespace Mimicraft.Voice
{
	public sealed class TauntVoiceRecorder : IDisposable
	{
		private VoiceMicrophone microphone;

		private IVoiceCodec encoder;

		private readonly List<byte[]> frames = new List<byte[]>();

		private readonly float[] frame = new float[960];

		private readonly byte[] encoded = new byte[512];

		private readonly List<float> samples = new List<float>(96000);

		private readonly List<float> peaks = new List<float>(100);

		public IReadOnlyList<float> Peaks => peaks;

		public bool IsRecording { get; private set; }

		public float Level
		{
			get
			{
				if (microphone == null)
				{
					return 0f;
				}
				return microphone.Level;
			}
		}

		public bool Hearing
		{
			get
			{
				if (microphone != null)
				{
					return microphone.Open;
				}
				return false;
			}
		}

		public float Seconds => (float)frames.Count * 0.02f;

		public bool Full => frames.Count >= 100;

		public bool Listen(out string problem)
		{
			problem = null;
			if (microphone != null && microphone.Active)
			{
				return true;
			}
			string text = ResolveDevice();
			if (string.IsNullOrEmpty(text))
			{
				problem = "Taunt.NoMicrophone";
				return false;
			}
			if (microphone == null)
			{
				microphone = new VoiceMicrophone();
			}
			if (!microphone.Start(text))
			{
				problem = "Taunt.NoMicrophone";
				return false;
			}
			return true;
		}

		public bool Begin(out string problem)
		{
			if (!Listen(out problem))
			{
				return false;
			}
			frames.Clear();
			samples.Clear();
			peaks.Clear();
			encoder?.Dispose();
			encoder = new OpusVoiceCodec(encode: true, decode: false);
			Drain();
			IsRecording = true;
			return true;
		}

		public void Tick()
		{
			if (microphone == null || !microphone.Active)
			{
				return;
			}
			if (!IsRecording)
			{
				Drain();
				return;
			}
			bool audible;
			while (frames.Count < 100 && microphone.TryReadFrame(frame, out audible))
			{
				for (int i = 0; i < 960; i++)
				{
					samples.Add(frame[i]);
				}
				peaks.Add(Peak());
				int num = encoder.Encode(frame, 0, encoded);
				if (num > 0 && num <= 255)
				{
					byte[] array = new byte[num];
					Buffer.BlockCopy(encoded, 0, array, 0, num);
					frames.Add(array);
				}
			}
		}

		public bool End(out byte[] payload, out string rejection)
		{
			payload = null;
			rejection = null;
			IsRecording = false;
			if (frames.Count < 15)
			{
				rejection = "Reject.TauntTooShort";
				return false;
			}
			if (!TauntVoiceClip.IsAudible(samples.ToArray(), out rejection))
			{
				return false;
			}
			byte[] array = TauntVoiceClip.Pack(frames);
			if (array.Length == 0 || array.Length > 16384)
			{
				rejection = "Reject.TauntTooLarge";
				return false;
			}
			payload = array;
			return true;
		}

		public AudioClip BuildPreview()
		{
			if (samples.Count != 0)
			{
				return TauntVoiceClip.ToClip(samples.ToArray(), "TauntPreview");
			}
			return null;
		}

		public void Close()
		{
			IsRecording = false;
			microphone?.Stop();
		}

		public void Dispose()
		{
			Close();
			microphone?.Dispose();
			microphone = null;
			encoder?.Dispose();
			encoder = null;
		}

		private void Drain()
		{
			bool audible;
			while (microphone.TryReadFrame(frame, out audible))
			{
			}
		}

		private float Peak()
		{
			float num = 0f;
			for (int i = 0; i < 960; i++)
			{
				num = Mathf.Max(num, Mathf.Abs(frame[i]));
			}
			return Mathf.Clamp01(num);
		}

		private static string ResolveDevice()
		{
			string[] devices = Microphone.devices;
			if (devices == null || devices.Length == 0)
			{
				return null;
			}
			string micDevice = GameSettings.MicDevice;
			if (!string.IsNullOrEmpty(micDevice) && Array.IndexOf(devices, micDevice) >= 0)
			{
				return micDevice;
			}
			return devices[0];
		}
	}
}
