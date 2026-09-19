using System;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicVoiceSegment
	{
		public int PlayerId { get; }

		public int SegmentId { get; }

		public int SamplingRate { get; }

		public int Channels => 1;

		public short[] Samples { get; }

		public float DurationSeconds { get; }

		public DateTime CreatedAtUtc { get; }

		public string DebugLabel { get; }

		public DateTime ArchivedAtUtc { get; set; }

		public float LastPlayedAtRealtime { get; set; } = float.NegativeInfinity;

		public MimicVoiceSegment(int playerId, int segmentId, int samplingRate, short[] samples, float durationSeconds, DateTime createdAtUtc, string debugLabel)
		{
			PlayerId = playerId;
			SegmentId = segmentId;
			SamplingRate = samplingRate;
			Samples = samples;
			DurationSeconds = durationSeconds;
			CreatedAtUtc = createdAtUtc;
			DebugLabel = debugLabel;
		}
	}
}
