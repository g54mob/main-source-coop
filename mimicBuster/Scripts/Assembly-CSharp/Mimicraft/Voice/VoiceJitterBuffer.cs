using System;
using UnityEngine;

namespace Mimicraft.Voice
{
	public sealed class VoiceJitterBuffer : IDisposable
	{
		private const int TargetFrames = 3;

		private const int Slots = 16;

		private const int RingSamples = 48000;

		private const float IdleSeconds = 0.4f;

		private readonly IVoiceCodec codec;

		private readonly VoiceRing ring;

		private readonly float[] decoded;

		private readonly byte[][] payloads = new byte[16][];

		private readonly int[] lengths = new int[16];

		private readonly ushort[] sequences = new ushort[16];

		private readonly bool[] filled = new bool[16];

		private ushort next;

		private ushort newest;

		private bool started;

		private float lastArrival;

		private float level;

		public VoiceRing Samples => ring;

		public bool Live
		{
			get
			{
				if (started)
				{
					return Time.unscaledTime - lastArrival < 0.4f;
				}
				return false;
			}
		}

		public float Level => level;

		public int Pending
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 16; i++)
				{
					if (filled[i])
					{
						num++;
					}
				}
				return num;
			}
		}

		public int Drained => ring.TotalRead;

		public int Late { get; private set; }

		public int Concealed { get; private set; }

		public VoiceJitterBuffer(IVoiceCodec codec)
		{
			this.codec = codec ?? throw new ArgumentNullException("codec");
			ring = new VoiceRing(48000);
			decoded = new float[codec.FrameSamples];
		}

		public void Push(ushort sequence, byte[] data, int offset, int length)
		{
			if (data == null || length <= 0 || offset < 0 || offset + length > data.Length)
			{
				return;
			}
			lastArrival = Time.unscaledTime;
			if (!started)
			{
				started = true;
				next = sequence;
				newest = sequence;
			}
			int num = (short)(sequence - next);
			if (num < 0)
			{
				Late++;
				return;
			}
			if (num >= 16)
			{
				Reset();
				started = true;
				next = sequence;
			}
			int num2 = sequence % 16;
			if (payloads[num2] == null || payloads[num2].Length < length)
			{
				payloads[num2] = new byte[Mathf.Max(length, 512)];
			}
			Buffer.BlockCopy(data, offset, payloads[num2], 0, length);
			lengths[num2] = length;
			sequences[num2] = sequence;
			filled[num2] = true;
			if ((short)(sequence - newest) > 0)
			{
				newest = sequence;
			}
		}

		public void Pump()
		{
			if (started)
			{
				int num = 3 * codec.FrameSamples;
				while (ring.Count < num && ring.Free >= codec.FrameSamples && EmitOne())
				{
				}
			}
		}

		private bool EmitOne()
		{
			int num = next % 16;
			if (filled[num] && sequences[num] == next)
			{
				int num2 = codec.Decode(payloads[num], 0, lengths[num], decoded);
				filled[num] = false;
				next++;
				if (num2 <= 0)
				{
					return true;
				}
				Measure(num2);
				ring.Write(decoded, 0, num2);
				return true;
			}
			if (!started || (short)(newest - next) <= 0)
			{
				return false;
			}
			int num3 = (next + 1) % 16;
			int num4 = 0;
			if (filled[num3] && sequences[num3] == (ushort)(next + 1))
			{
				num4 = codec.DecodeForwardCorrection(payloads[num3], 0, lengths[num3], decoded);
			}
			if (num4 <= 0)
			{
				num4 = codec.DecodeLost(decoded);
			}
			Concealed++;
			next++;
			if (num4 > 0)
			{
				ring.Write(decoded, 0, num4);
			}
			return true;
		}

		private void Measure(int samples)
		{
			double num = 0.0;
			for (int i = 0; i < samples; i++)
			{
				num += (double)(decoded[i] * decoded[i]);
			}
			float num2 = Mathf.Sqrt((float)(num / (double)samples));
			level = Mathf.Lerp(level, num2, (num2 > level) ? 0.6f : 0.15f);
		}

		public void Reset()
		{
			for (int i = 0; i < 16; i++)
			{
				filled[i] = false;
			}
			started = false;
			level = 0f;
		}

		public void Dispose()
		{
			Reset();
			codec.Dispose();
		}
	}
}
