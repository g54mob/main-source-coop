using System;
using System.Runtime.InteropServices;
using POpusCodec.Enums;
using Photon.Voice;

namespace POpusCodec
{
	public class OpusDecoder<T> : IDisposable
	{
		private const bool UseInbandFEC = true;

		protected Action<FrameOut<T>> output;

		protected bool TisFloat;

		protected int sizeofT;

		protected FrameOut<T> frameOut = new FrameOut<T>(null, endOfStream: false);

		protected IntPtr handle = IntPtr.Zero;

		protected int channels;

		protected int frameSamples;

		protected static readonly T[] EmptyBuffer = new T[0];

		private T[] buffer;

		private bool prevPacketInvalid;

		public OpusDecoder(Action<FrameOut<T>> output, SamplingRate outputSamplingRateHz, Channels channels, int frameSamples)
		{
			this.output = output;
			TisFloat = default(T) is float;
			sizeofT = Marshal.SizeOf(default(T));
			if (outputSamplingRateHz != SamplingRate.Sampling08000 && outputSamplingRateHz != SamplingRate.Sampling12000 && outputSamplingRateHz != SamplingRate.Sampling16000 && outputSamplingRateHz != SamplingRate.Sampling24000 && outputSamplingRateHz != SamplingRate.Sampling48000)
			{
				throw new ArgumentOutOfRangeException("outputSamplingRateHz", "Must use one of the pre-defined sampling rates (" + outputSamplingRateHz.ToString() + ")");
			}
			if (channels != Channels.Mono && channels != Channels.Stereo)
			{
				throw new ArgumentOutOfRangeException("numChannels", "Must be Mono or Stereo");
			}
			this.channels = (int)channels;
			this.frameSamples = frameSamples;
			buffer = new T[frameSamples * this.channels];
			handle = Wrapper.opus_decoder_create(outputSamplingRateHz, channels);
			if (handle == IntPtr.Zero)
			{
				throw new OpusException(OpusStatusCode.AllocFail, "Memory was not allocated for the encoder");
			}
		}

		protected void decodePacket(FrameBuffer data, int decodeFEC, int channels, bool endOfStream)
		{
			if ((TisFloat ? Wrapper.opus_decode(handle, data, buffer as float[], frameSamples, decodeFEC) : Wrapper.opus_decode(handle, data, buffer as short[], frameSamples, decodeFEC)) != 0)
			{
				procOutput(buffer, endOfStream);
			}
		}

		protected void procOutput(T[] buffer, bool endOfStream)
		{
			output(frameOut.Set(buffer, endOfStream));
		}

		public void DecodePacket(ref FrameBuffer packetData, bool endOfStream)
		{
			bool flag = packetData.Array == null || Wrapper.opus_packet_get_bandwidth(packetData.Ptr) == -4;
			if (prevPacketInvalid)
			{
				if (flag)
				{
					decodePacket(default(FrameBuffer), 0, channels, endOfStream: false);
				}
				else
				{
					decodePacket(packetData, 1, channels, endOfStream: false);
				}
			}
			if (!flag)
			{
				decodePacket(packetData, 0, channels, endOfStream);
			}
			prevPacketInvalid = flag;
		}

		public virtual void Dispose()
		{
			if (handle != IntPtr.Zero)
			{
				Wrapper.opus_decoder_destroy(handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
