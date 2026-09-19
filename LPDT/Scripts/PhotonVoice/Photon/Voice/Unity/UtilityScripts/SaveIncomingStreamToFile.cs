using System;
using System.IO;
using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	[RequireComponent(typeof(VoiceConnection))]
	[DisallowMultipleComponent]
	public class SaveIncomingStreamToFile : VoiceComponent
	{
		private VoiceConnection voiceConnection;

		[SerializeField]
		private bool muteLocalSpeaker;

		protected override void Awake()
		{
			base.Awake();
			voiceConnection = GetComponent<VoiceConnection>();
			voiceConnection.RemoteVoiceAdded += OnRemoteVoiceAdded;
			voiceConnection.SpeakerLinked += OnSpeakerLinked;
		}

		private void OnSpeakerLinked(Speaker speaker)
		{
			if (muteLocalSpeaker && speaker.RemoteVoice.PlayerId == voiceConnection.Client.LocalPlayer.ActorNumber)
			{
				AudioSource component = speaker.GetComponent<AudioSource>();
				component.mute = true;
				component.volume = 0f;
			}
		}

		private void OnDestroy()
		{
			voiceConnection.RemoteVoiceAdded -= OnRemoteVoiceAdded;
		}

		private void OnRemoteVoiceAdded(RemoteVoiceLink remoteVoiceLink)
		{
			int bits = 32;
			string filePath = GetFilePath(remoteVoiceLink);
			base.Logger.Log(LogLevel.Info, "Incoming stream {0}, output file path: {1}", remoteVoiceLink.VoiceInfo, filePath);
			WaveWriter waveWriter = new WaveWriter(filePath, remoteVoiceLink.VoiceInfo.SamplingRate, bits, remoteVoiceLink.VoiceInfo.Channels);
			remoteVoiceLink.FloatFrameDecoded += delegate(FrameOut<float> f)
			{
				waveWriter.WriteSamples(f.Buf, 0, f.Buf.Length);
			};
			remoteVoiceLink.RemoteVoiceRemoved += delegate
			{
				base.Logger.Log(LogLevel.Info, "Remote voice stream removed: Saving wav file.");
				waveWriter.Dispose();
			};
		}

		private string GetFilePath(RemoteVoiceLink remoteVoiceLink)
		{
			string path = string.Format("in_{0}_{1}_{2}_{3}_{4}.wav", DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss-ffff"), UnityEngine.Random.Range(0, 1000), remoteVoiceLink.ChannelId, remoteVoiceLink.PlayerId, remoteVoiceLink.VoiceId);
			return Path.Combine(Application.persistentDataPath, path);
		}
	}
}
