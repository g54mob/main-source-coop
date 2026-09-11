using UnityEngine;

namespace Photon.Voice.Unity
{
	[AddComponentMenu("Photon Voice/Speaker AudioFilterRead")]
	public class SpeakerAudioFilterRead : Speaker
	{
		private AudioSyncBuffer<float> outBuffer;

		private int outputSampleRate;

		protected override IAudioOut<float> CreateAudioOut()
		{
			outBuffer = new AudioSyncBuffer<float>(playDelayConfig, base.Logger, string.Empty, debugInfo: true);
			outputSampleRate = AudioSettings.outputSampleRate;
			return outBuffer;
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (outBuffer != null)
			{
				outBuffer.Read(data, channels, outputSampleRate);
			}
		}
	}
}
