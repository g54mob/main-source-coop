using Ami.BroAudio.Runtime;
using Ami.Extension;

namespace Ami.BroAudio.Data
{
	public interface IAudioEntity
	{
		PlaybackGroup PlaybackGroup { get; }

		SpatialSetting SpatialSetting { get; }

		int Priority { get; }

		MulticlipsPlayMode PlayMode { get; }

		bool HasLoop(out LoopType loopType, out float transitionTime);

		IBroAudioClip PickNewClip();

		IBroAudioClip PickNewClip(ClipSelectionContext context);

		float GetMasterVolume();

		float GetPitch();

		float GetRandomValue(float baseValue, RandomFlag flags);

		void ResetMultiClipStrategy();
	}
}
