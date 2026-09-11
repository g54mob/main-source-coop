using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using POpusCodec.Enums;
using Photon.Voice;

namespace POpusCodec
{
	public class OpusEncoder : IDisposable
	{
		public const int BitrateMax = -1;

		private IntPtr handle = IntPtr.Zero;

		private const int RecommendedMaxPacketSize = 4000;

		private int frameSamples = 960;

		private SamplingRate inputSamplingRate = SamplingRate.Sampling48000;

		private Channels channels = Channels.Stereo;

		private readonly byte[] writePacket = new byte[4000];

		private static readonly ArraySegment<byte> EmptyBuffer = new ArraySegment<byte>(new byte[0]);

		private Delay _encoderDelay = Delay.Delay20ms;

		public static Dictionary<IntPtr, OpusEncoder> handles = new Dictionary<IntPtr, OpusEncoder>();

		private byte[] bufOut;

		public Action<ArraySegment<byte>, FrameFlags> Output;

		public SamplingRate InputSamplingRate => inputSamplingRate;

		public Channels InputChannels => channels;

		public Delay EncoderDelay
		{
			get
			{
				return _encoderDelay;
			}
			set
			{
				_encoderDelay = value;
				frameSamples = (int)((decimal)((int)inputSamplingRate / 1000) * (decimal)(int)_encoderDelay / 2m);
			}
		}

		public int FrameSizePerChannel => frameSamples;

		public int Bitrate
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.Bitrate);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.Bitrate, value);
			}
		}

		public Bandwidth MaxBandwidth
		{
			get
			{
				return (Bandwidth)Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.MaxBandwidth);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.MaxBandwidth, (int)value);
			}
		}

		public Complexity Complexity
		{
			get
			{
				return (Complexity)Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.Complexity);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.Complexity, (int)value);
			}
		}

		public int ExpectedPacketLossPercentage
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.PacketLossPercentage);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.PacketLossPercentage, value);
			}
		}

		public SignalHint SignalHint
		{
			get
			{
				return (SignalHint)Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.Signal);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.Signal, (int)value);
			}
		}

		public ForceChannels ForceChannels
		{
			get
			{
				return (ForceChannels)Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.ForceChannels);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.ForceChannels, (int)value);
			}
		}

		public bool UseInbandFEC
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.InbandFec) == 1;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.InbandFec, value ? 1 : 0);
			}
		}

		public int PacketLossPercentage
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.PacketLossPercentage);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.PacketLossPercentage, value);
			}
		}

		public bool UseUnconstrainedVBR
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.VBRConstraint) == 0;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.VBRConstraint, (!value) ? 1 : 0);
			}
		}

		public bool DtxEnabled
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(handle, OpusCtlGetRequest.Dtx) == 1;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(handle, OpusCtlSetRequest.Dtx, value ? 1 : 0);
			}
		}

		public OpusEncoder(SamplingRate inputSamplingRateHz, Channels numChannels, int bitrate, OpusApplicationType applicationType, Delay encoderDelay)
		{
			if (inputSamplingRateHz != SamplingRate.Sampling08000 && inputSamplingRateHz != SamplingRate.Sampling12000 && inputSamplingRateHz != SamplingRate.Sampling16000 && inputSamplingRateHz != SamplingRate.Sampling24000 && inputSamplingRateHz != SamplingRate.Sampling48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRateHz", "Must use one of the pre-defined sampling rates(" + inputSamplingRateHz.ToString() + ")");
			}
			if (numChannels != Channels.Mono && numChannels != Channels.Stereo)
			{
				throw new ArgumentOutOfRangeException("numChannels", "Must be Mono or Stereo");
			}
			if (applicationType != OpusApplicationType.Audio && applicationType != OpusApplicationType.RestrictedLowDelay && applicationType != OpusApplicationType.Voip)
			{
				throw new ArgumentOutOfRangeException("applicationType", "Must use one of the pre-defined application types (" + applicationType.ToString() + ")");
			}
			if (encoderDelay != Delay.Delay10ms && encoderDelay != Delay.Delay20ms && encoderDelay != Delay.Delay2dot5ms && encoderDelay != Delay.Delay40ms && encoderDelay != Delay.Delay5ms && encoderDelay != Delay.Delay60ms)
			{
				throw new ArgumentOutOfRangeException("encoderDelay", "Must use one of the pre-defined delay values (" + encoderDelay.ToString() + ")");
			}
			inputSamplingRate = inputSamplingRateHz;
			channels = numChannels;
			handle = Wrapper.opus_encoder_create(inputSamplingRateHz, numChannels, applicationType);
			handles[handle] = this;
			if (handle == IntPtr.Zero)
			{
				throw new OpusException(OpusStatusCode.AllocFail, "Memory was not allocated for the encoder");
			}
			EncoderDelay = encoderDelay;
			Bitrate = bitrate;
			UseInbandFEC = true;
			PacketLossPercentage = 30;
		}

		[MonoPInvokeCallback(typeof(Action<IntPtr, IntPtr, int>))]
		public static void DataCallbackStatic(IntPtr handle, IntPtr p, int count)
		{
			if (handles.TryGetValue(handle, out var value))
			{
				value.dataCallback(p, count);
			}
		}

		private void dataCallback(IntPtr p, int count)
		{
			if (Output != null)
			{
				if (bufOut == null || bufOut.Length < count)
				{
					bufOut = new byte[count];
				}
				Marshal.Copy(p, bufOut, 0, count);
				Output(new ArraySegment<byte>(bufOut, 0, count), (FrameFlags)0);
			}
		}

		public void Encode(float[] pcmSamples)
		{
			int num = Wrapper.opus_encode(handle, pcmSamples, frameSamples, writePacket);
			if (num > 1)
			{
				Output(new ArraySegment<byte>(writePacket, 0, num), (FrameFlags)0);
			}
		}

		public void Encode(short[] pcmSamples)
		{
			int num = Wrapper.opus_encode(handle, pcmSamples, frameSamples, writePacket);
			if (num > 1)
			{
				Output(new ArraySegment<byte>(writePacket, 0, num), (FrameFlags)0);
			}
		}

		public void Dispose()
		{
			if (handle != IntPtr.Zero)
			{
				handles.Remove(handle);
				Wrapper.opus_encoder_destroy(handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
