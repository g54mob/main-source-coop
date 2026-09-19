using System.Collections.Generic;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public interface IMimicVoiceArchiveConverter
	{
		float CalculateRms(float[] samples, int channels);

		float CalculateRms(short[] samples, int channels);

		void AppendResampledMonoPcm16(List<short> destination, float[] sourceSamples, int sourceSamplingRate, int sourceChannels, int targetSamplingRate, ref float previousMonoSample, ref bool hasPreviousMonoSample, ref double phase);

		void AppendResampledMonoPcm16(List<short> destination, short[] sourceSamples, int sourceSamplingRate, int sourceChannels, int targetSamplingRate, ref float previousMonoSample, ref bool hasPreviousMonoSample, ref double phase);
	}
}
