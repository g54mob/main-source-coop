namespace Photon.Voice
{
	public class LocalVoiceAudioShort : LocalVoiceAudio<short>
	{
		internal LocalVoiceAudioShort(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, IAudioDesc audioSourceDesc, int channelId, VoiceCreateOptions opt)
			: base(voiceClient, id, voiceInfo, audioSourceDesc, channelId, opt)
		{
			levelMeter = new AudioUtil.LevelMeterShort(info.SamplingRate, info.Channels);
			voiceDetector = new AudioUtil.VoiceDetectorShort(info.SamplingRate, info.Channels);
			initBuiltinProcessors();
		}
	}
}
