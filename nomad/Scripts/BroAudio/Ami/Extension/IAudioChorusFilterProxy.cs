namespace Ami.Extension
{
	public interface IAudioChorusFilterProxy
	{
		float dryMix { get; set; }

		float wetMix1 { get; set; }

		float wetMix2 { get; set; }

		float wetMix3 { get; set; }

		float delay { get; set; }

		float rate { get; set; }

		float depth { get; set; }

		bool enabled { get; set; }
	}
}
