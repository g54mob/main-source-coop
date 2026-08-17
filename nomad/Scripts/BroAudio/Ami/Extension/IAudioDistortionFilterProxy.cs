namespace Ami.Extension
{
	public interface IAudioDistortionFilterProxy
	{
		float distortionLevel { get; set; }

		bool enabled { get; set; }
	}
}
