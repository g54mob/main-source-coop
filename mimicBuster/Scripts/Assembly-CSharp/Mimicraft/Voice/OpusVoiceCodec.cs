using System;
using Concentus;
using Concentus.Enums;
using UnityEngine;

namespace Mimicraft.Voice
{
	public sealed class OpusVoiceCodec : IVoiceCodec, IDisposable
	{
		public const byte CodecId = 1;

		public const int Rate = 48000;

		public const int FrameMilliseconds = 20;

		public const int SamplesPerFrame = 960;

		public const int MaxEncodedBytes = 512;

		private const int Bitrate = 24000;

		private const int Complexity = 5;

		private const int ExpectedPacketLossPercent = 10;

		private readonly IOpusEncoder encoder;

		private readonly IOpusDecoder decoder;

		public byte Id => 1;

		public int SampleRate => 48000;

		public int FrameSamples => 960;

		public OpusVoiceCodec(bool encode, bool decode)
		{
			if (encode)
			{
				encoder = OpusCodecFactory.CreateEncoder(48000, 1, OpusApplication.OPUS_APPLICATION_VOIP);
				encoder.Bitrate = 24000;
				encoder.Complexity = 5;
				encoder.SignalType = OpusSignal.OPUS_SIGNAL_VOICE;
				encoder.UseVBR = true;
				encoder.UseConstrainedVBR = true;
				encoder.UseInbandFEC = true;
				encoder.PacketLossPercent = 10;
				encoder.UseDTX = false;
			}
			if (decode)
			{
				decoder = OpusCodecFactory.CreateDecoder(48000, 1);
			}
		}

		public int Encode(float[] pcm, int offset, byte[] destination)
		{
			if (encoder == null || pcm == null || destination == null || offset < 0 || offset + 960 > pcm.Length)
			{
				return 0;
			}
			try
			{
				return encoder.Encode(new ReadOnlySpan<float>(pcm, offset, 960), 960, new Span<byte>(destination), Mathf.Min(destination.Length, 512));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Ses kodlanamadı: " + ex.Message);
				return 0;
			}
		}

		public int Decode(byte[] data, int offset, int length, float[] destination)
		{
			if (decoder == null || data == null || destination == null || length <= 0 || offset < 0 || offset + length > data.Length || destination.Length < 960)
			{
				return 0;
			}
			try
			{
				return decoder.Decode(new ReadOnlySpan<byte>(data, offset, length), new Span<float>(destination, 0, 960), 960);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Ses çözülemedi: " + ex.Message);
				return DecodeLost(destination);
			}
		}

		public int DecodeLost(float[] destination)
		{
			if (decoder == null || destination == null || destination.Length < 960)
			{
				return 0;
			}
			try
			{
				return decoder.Decode(ReadOnlySpan<byte>.Empty, new Span<float>(destination, 0, 960), 960);
			}
			catch (Exception)
			{
				Array.Clear(destination, 0, 960);
				return 960;
			}
		}

		public int DecodeForwardCorrection(byte[] data, int offset, int length, float[] destination)
		{
			if (decoder == null || data == null || destination == null || length <= 0 || offset < 0 || offset + length > data.Length || destination.Length < 960)
			{
				return 0;
			}
			try
			{
				return decoder.Decode(new ReadOnlySpan<byte>(data, offset, length), new Span<float>(destination, 0, 960), 960, decode_fec: true);
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public void Dispose()
		{
			encoder?.Dispose();
			decoder?.Dispose();
		}
	}
}
