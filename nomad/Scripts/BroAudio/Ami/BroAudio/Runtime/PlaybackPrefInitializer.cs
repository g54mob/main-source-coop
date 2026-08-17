using System.Collections.Generic;

namespace Ami.BroAudio.Runtime
{
	public struct PlaybackPrefInitializer : IAudioTypeIterable
	{
		public Dictionary<BroAudioType, AudioTypePlaybackPreference> AudioTypePref;

		public void OnEachAudioType(BroAudioType audioType)
		{
			AudioTypePref?.Add(audioType, new AudioTypePlaybackPreference());
		}
	}
}
