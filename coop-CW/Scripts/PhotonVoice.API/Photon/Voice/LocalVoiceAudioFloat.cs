namespace Photon.Voice
{
	public class LocalVoiceAudioFloat : LocalVoiceAudio<float>
	{
		internal LocalVoiceAudioFloat(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, IAudioDesc audioSourceDesc, int channelId, VoiceCreateOptions opt)
			: base(voiceClient, id, voiceInfo, audioSourceDesc, channelId, opt)
		{
			levelMeter = new AudioUtil.LevelMeterFloat(info.SamplingRate, info.Channels);
			voiceDetector = new AudioUtil.VoiceDetectorFloat(info.SamplingRate, info.Channels);
			initBuiltinProcessors();
		}
	}
}
