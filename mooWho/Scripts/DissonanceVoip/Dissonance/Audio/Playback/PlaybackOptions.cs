namespace Dissonance.Audio.Playback
{
	public readonly struct PlaybackOptions
	{
		public bool IsPositional { get; }

		public float AmplitudeMultiplier { get; }

		public ChannelPriority Priority { get; }

		public PlaybackOptions(bool isPositional, float amplitudeMultiplier, ChannelPriority priority)
		{
			IsPositional = isPositional;
			AmplitudeMultiplier = amplitudeMultiplier;
			Priority = priority;
		}
	}
}
