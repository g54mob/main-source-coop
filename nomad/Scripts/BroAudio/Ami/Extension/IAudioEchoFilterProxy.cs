namespace Ami.Extension
{
	public interface IAudioEchoFilterProxy
	{
		float delay { get; set; }

		float decayRatio { get; set; }

		float dryMix { get; set; }

		float wetMix { get; set; }

		bool enabled { get; set; }
	}
}
