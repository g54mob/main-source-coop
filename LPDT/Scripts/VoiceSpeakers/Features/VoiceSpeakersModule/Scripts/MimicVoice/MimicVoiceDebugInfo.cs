namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicVoiceDebugInfo
	{
		public int PlayerId { get; set; }

		public int SegmentCount { get; set; }

		public int ArchivedSegmentCount { get; set; }

		public bool IsRecording { get; set; }

		public float CurrentDurationSeconds { get; set; }

		public float LastRms { get; set; }

		public float LastCommittedDurationSeconds { get; set; }

		public string LastDropReason { get; set; }

		public string LastPlaybackResult { get; set; }
	}
}
