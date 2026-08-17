using System.Collections.Generic;

namespace Ami.BroAudio.Runtime
{
	public struct OriginVolumeRecorder : IAudioTypeIterable
	{
		public BroAudioType TargetType;

		public Dictionary<BroAudioType, float> SystemOriginalVolumes;

		public void OnEachAudioType(BroAudioType audioType)
		{
			if (TargetType.Contains(audioType) && SoundManager.Instance.TryGetAudioTypePref(audioType, out var result))
			{
				SystemOriginalVolumes[audioType] = result.Volume;
			}
		}
	}
}
