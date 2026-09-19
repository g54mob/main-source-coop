using System;
using System.Threading;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public class MicWrapperPusher : IAudioPusher<float>, IAudioDesc, IDisposable
	{
		private AudioSource audioSource;

		private AudioClip mic;

		private string device;

		private ILogger logger;

		private MicWrapperPusherOnAudioFilterRead onRead;

		private int sampleRate;

		private int channels;

		public int SamplingRate
		{
			get
			{
				if (Error != null)
				{
					return 0;
				}
				return sampleRate;
			}
		}

		public int Channels
		{
			get
			{
				if (Error != null)
				{
					return 0;
				}
				return channels;
			}
		}

		public string Error { get; private set; }

		public MicWrapperPusher(GameObject parent, string device, int suggestedFrequency, ILogger logger)
		{
			try
			{
				this.device = device;
				this.logger = logger;
				sampleRate = AudioSettings.outputSampleRate;
				switch (AudioSettings.speakerMode)
				{
				case AudioSpeakerMode.Mono:
					channels = 1;
					break;
				case AudioSpeakerMode.Stereo:
					channels = 2;
					break;
				default:
					Error = "Only Mono and Stereo project speaker mode supported. Current mode is " + AudioSettings.speakerMode;
					logger.Log(LogLevel.Error, "[PV] MicWrapperPusher: " + Error);
					return;
				}
				Error = UnityMicrophone.CheckDevice(logger, "[PV] MicWrapperPusher: ", device, suggestedFrequency, out var frequency);
				if (Error != null)
				{
					return;
				}
				GameObject gameObject = new GameObject("[PV] MicWrapperPusher: AudioSource + AudioOutCapture");
				gameObject.transform.SetParent(parent.transform, worldPositionStays: false);
				audioSource = gameObject.AddComponent<AudioSource>();
				logger.Log(LogLevel.Info, "[PV] MicWrapperPusher: new AudioSource created.");
				onRead = gameObject.AddComponent<MicWrapperPusherOnAudioFilterRead>();
				mic = UnityMicrophone.Start(device, loop: true, 1, frequency);
				audioSource.clip = mic;
				audioSource.loop = true;
				for (int i = 0; i < 1000; i++)
				{
					if (UnityMicrophone.GetPosition(device) > 0)
					{
						break;
					}
					Thread.Sleep(1);
				}
				if (UnityMicrophone.GetPosition(device) <= 0)
				{
					logger.Log(LogLevel.Warning, "[PV] MicWrapperPusher: microphone start takes too long, Playing audio source without waiting for the microphone. Captured data may be delayed.");
				}
				audioSource.Play();
				logger.Log(LogLevel.Info, "[PV] MicWrapperPusher: microphone '{0}' initialized, frequency = {1}, channels = {2}.", device, mic.frequency, mic.channels);
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				if (Error == null)
				{
					Error = "Exception in MicWrapperPusher constructor";
				}
				logger.Log(LogLevel.Error, "[PV] MicWrapperPusher: " + Error);
			}
		}

		public void SetCallback(Action<float[]> callback, ObjectFactory<float[], int> bufferFactory, int optimalFrameSize)
		{
			onRead.OnAudioFrame += delegate(float[] buf, int ch)
			{
				callback(buf);
			};
		}

		public void Dispose()
		{
			UnityMicrophone.End(device);
			if (audioSource != null)
			{
				UnityEngine.Object.Destroy(audioSource.gameObject);
				logger.Log(LogLevel.Info, "[PV] MicWrapperPusher: AudioSource removed.");
			}
		}
	}
}
