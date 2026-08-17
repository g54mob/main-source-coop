using System;
using System.Collections.Generic;

namespace Ami.BroAudio.Runtime
{
	public struct PlaybackPrefSetter<TParameter> : IAudioTypeIterable
	{
		public BroAudioType TargetType;

		public IReadOnlyDictionary<BroAudioType, AudioTypePlaybackPreference> AudioTypePref;

		public Action<AudioTypePlaybackPreference, TParameter> OnModifyPref;

		public TParameter Parameter;

		public void OnEachAudioType(BroAudioType audioType)
		{
			if (TargetType.Contains(audioType) && AudioTypePref.TryGetValue(audioType, out var value))
			{
				OnModifyPref(value, Parameter);
			}
		}
	}
}
