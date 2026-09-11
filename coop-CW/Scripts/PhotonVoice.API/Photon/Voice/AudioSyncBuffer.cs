using System;

namespace Photon.Voice
{
	public class AudioSyncBuffer<T> : AudioOutDelayControl<T>
	{
		private T[] buffer;

		private int readPosSamples;

		private int sampleRate;

		private int channels;

		private int bufferSamples;

		private bool started;

		public override long OutPos => readPosSamples;

		public AudioSyncBuffer(PlayDelayConfig playDelayConfig, ILogger logger, string logPrefix, bool debugInfo)
			: base(false, playDelayConfig, logger, "[PV] [Unity] AudioSyncBuffer" + ((logPrefix == "") ? "" : (" " + logPrefix)), debugInfo)
		{
		}

		public override void OutCreate(int frequency, int channels, int bufferSamples)
		{
			sampleRate = frequency;
			this.channels = channels;
			this.bufferSamples = bufferSamples;
			buffer = new T[channels * bufferSamples];
		}

		public override void OutStart()
		{
			started = true;
		}

		public override void OutWrite(T[] data, int offsetSamples)
		{
			int num = offsetSamples * channels;
			int num2 = buffer.Length - num;
			if (data.Length > num2)
			{
				Array.Copy(data, 0, buffer, num, num2);
				Array.Copy(data, num2, buffer, 0, data.Length - num2);
			}
			else
			{
				Array.Copy(data, 0, buffer, num, data.Length);
			}
		}

		public override void Stop()
		{
			started = false;
		}

		public void Read(T[] outBuf, int outChannels, int outSampleRate)
		{
			if (!started)
			{
				return;
			}
			int num = outBuf.Length / outChannels * sampleRate / outSampleRate;
			int num2 = readPosSamples * channels;
			int num3 = buffer.Length - num2;
			if (sampleRate == outSampleRate && channels == outChannels)
			{
				if (outBuf.Length > num3)
				{
					Array.Copy(buffer, num2, outBuf, 0, num3);
					Array.Copy(buffer, 0, outBuf, num3, outBuf.Length - num3);
				}
				else
				{
					Array.Copy(buffer, num2, outBuf, 0, outBuf.Length);
				}
			}
			else
			{
				int num4 = num * channels;
				if (num4 > num3)
				{
					int num5 = num3 * outSampleRate / sampleRate * outChannels / channels;
					AudioUtil.Resample(buffer, num2, num3, channels, outBuf, 0, num5, outChannels);
					AudioUtil.Resample(buffer, 0, num4 - num3, channels, outBuf, num5, outBuf.Length - num5, outChannels);
				}
				else
				{
					AudioUtil.Resample(buffer, num2, num4, channels, outBuf, 0, outBuf.Length, outChannels);
				}
			}
			readPosSamples = (readPosSamples + num) % bufferSamples;
		}
	}
}
