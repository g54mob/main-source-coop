namespace Ami.Extension
{
	public interface IAudioHighPassFilterProxy
	{
		float cutoffFrequency { get; set; }

		float highpassResonanceQ { get; set; }

		bool enabled { get; set; }
	}
}
