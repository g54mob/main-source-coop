using System.Collections.Generic;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public interface IMimicVoiceSegmentProvider
	{
		IReadOnlyDictionary<int, MimicVoiceDebugInfo> DebugInfos { get; }

		bool TryGetSegmentBySeed(int playerId, int seed, out MimicVoiceSegment segment);

		void MarkSegmentPlayed(int playerId, int segmentId);
	}
}
