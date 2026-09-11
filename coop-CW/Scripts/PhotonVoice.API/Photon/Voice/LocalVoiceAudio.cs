using System;

namespace Photon.Voice
{
	public abstract class LocalVoiceAudio<T> : LocalVoiceFramed<T>, ILocalVoiceAudio
	{
		protected AudioUtil.VoiceDetector<T> voiceDetector;

		protected AudioUtil.VoiceDetectorCalibration<T> voiceDetectorCalibration;

		protected AudioUtil.LevelMeter<T> levelMeter;

		protected int channels;

		public virtual AudioUtil.IVoiceDetector VoiceDetector => voiceDetector;

		public virtual AudioUtil.ILevelMeter LevelMeter => levelMeter;

		public bool VoiceDetectorCalibrating => voiceDetectorCalibration.IsCalibrating;

		internal static LocalVoiceAudio<T> Create(VoiceClient voiceClient, byte voiceId, VoiceInfo voiceInfo, IAudioDesc audioSourceDesc, int channelId, VoiceCreateOptions options = default(VoiceCreateOptions))
		{
			if (typeof(T) == typeof(float))
			{
				return new LocalVoiceAudioFloat(voiceClient, voiceId, voiceInfo, audioSourceDesc, channelId, options) as LocalVoiceAudio<T>;
			}
			if (typeof(T) == typeof(short))
			{
				return new LocalVoiceAudioShort(voiceClient, voiceId, voiceInfo, audioSourceDesc, channelId, options) as LocalVoiceAudio<T>;
			}
			throw new UnsupportedSampleTypeException(typeof(T));
		}

		public void VoiceDetectorCalibrate(int durationMs, Action<float> onCalibrated = null)
		{
			voiceDetectorCalibration.Calibrate(durationMs, onCalibrated);
		}

		internal LocalVoiceAudio(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, IAudioDesc audioSourceDesc, int channelId, VoiceCreateOptions opt)
			: base(voiceClient, id, voiceInfo, audioSourceDesc.SamplingRate, channelId, opt)
		{
			channels = voiceInfo.Channels;
		}

		protected void initBuiltinProcessors()
		{
			voiceDetectorCalibration = new AudioUtil.VoiceDetectorCalibration<T>(voiceDetector, levelMeter, info.SamplingRate, channels);
			AddPostProcessor(levelMeter, voiceDetectorCalibration, voiceDetector);
		}
	}
}
