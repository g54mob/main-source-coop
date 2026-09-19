namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public interface IMimicVoiceFrameSink
	{
		void CaptureShortFrame(int playerId, short[] samples, int samplingRate, int channels);

		void CaptureFloatFrame(int playerId, float[] samples, int samplingRate, int channels);

		void EndCapture(int playerId, string reason);
	}
}
