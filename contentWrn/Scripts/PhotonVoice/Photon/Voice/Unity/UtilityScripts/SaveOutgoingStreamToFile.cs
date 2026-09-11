using System;
using System.IO;
using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	[RequireComponent(typeof(Recorder))]
	[DisallowMultipleComponent]
	public class SaveOutgoingStreamToFile : VoiceComponent
	{
		private class OutgoingStreamSaverFloat : IProcessor<float>, IDisposable
		{
			private WaveWriter wavWriter;

			public OutgoingStreamSaverFloat(WaveWriter waveWriter)
			{
				wavWriter = waveWriter;
			}

			public float[] Process(float[] buf)
			{
				wavWriter.WriteSamples(buf, 0, buf.Length);
				return buf;
			}

			public void Dispose()
			{
				wavWriter.Dispose();
			}
		}

		private class OutgoingStreamSaverShort : IProcessor<short>, IDisposable
		{
			private WaveWriter wavWriter;

			public OutgoingStreamSaverShort(WaveWriter waveWriter)
			{
				wavWriter = waveWriter;
			}

			public short[] Process(short[] buf)
			{
				for (int i = 0; i < buf.Length; i++)
				{
					wavWriter.Write(buf[i]);
				}
				return buf;
			}

			public void Dispose()
			{
				wavWriter.Dispose();
			}
		}

		private WaveWriter wavWriter;

		private void PhotonVoiceCreated(PhotonVoiceCreatedParams photonVoiceCreatedParams)
		{
			VoiceInfo info = photonVoiceCreatedParams.Voice.Info;
			string filePath = GetFilePath();
			if (photonVoiceCreatedParams.Voice is LocalVoiceAudioFloat)
			{
				wavWriter = new WaveWriter(filePath, info.SamplingRate, 32, info.Channels);
				base.Logger.LogInfo("Outgoing 32 bit stream {0}, output file path: {1}", info, filePath);
				(photonVoiceCreatedParams.Voice as LocalVoiceAudioFloat).AddPostProcessor(new OutgoingStreamSaverFloat(wavWriter));
			}
			else if (photonVoiceCreatedParams.Voice is LocalVoiceAudioShort)
			{
				wavWriter = new WaveWriter(filePath, info.SamplingRate, 16, info.Channels);
				base.Logger.LogInfo("Outgoing 16 bit stream {0}, output file path: {1}", info, filePath);
				(photonVoiceCreatedParams.Voice as LocalVoiceAudioShort).AddPostProcessor(new OutgoingStreamSaverShort(wavWriter));
			}
		}

		private string GetFilePath()
		{
			string path = string.Format("out_{0}_{1}.wav", DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss-ffff"), UnityEngine.Random.Range(0, 1000));
			return Path.Combine(Application.persistentDataPath, path);
		}

		private void PhotonVoiceRemoved()
		{
			wavWriter.Dispose();
			base.Logger.LogInfo("Recording stopped: Saving wav file.");
		}
	}
}
