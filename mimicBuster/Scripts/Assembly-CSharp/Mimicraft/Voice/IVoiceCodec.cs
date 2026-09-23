using System;

namespace Mimicraft.Voice
{
	public interface IVoiceCodec : IDisposable
	{
		byte Id { get; }

		int SampleRate { get; }

		int FrameSamples { get; }

		int Encode(float[] pcm, int offset, byte[] destination);

		int Decode(byte[] data, int offset, int length, float[] destination);

		int DecodeLost(float[] destination);

		int DecodeForwardCorrection(byte[] data, int offset, int length, float[] destination);
	}
}
