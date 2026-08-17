using System.Collections.Generic;
using Ami.Extension;

namespace Ami.BroAudio.Data
{
	public interface IReadOnlyAudioEntity
	{
		IReadOnlyList<IBroAudioClip> Clips { get; }

		float MasterVolume { get; }

		bool Loop { get; }

		bool SeamlessLoop { get; }

		float TransitionTime { get; }

		SpatialSetting SpatialSetting { get; }

		int Priority { get; }

		float Pitch { get; }

		float PitchRandomRange { get; }

		float VolumeRandomRange { get; }

		RandomFlag RandomFlags { get; }

		PlaybackGroup PlaybackGroup { get; }

		MulticlipsPlayMode PlayMode { get; }
	}
}
