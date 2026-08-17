namespace Ami.BroAudio
{
	public interface ISchedulable
	{
		internal IAudioPlayer SetScheduledStartTime(double dspTime);

		internal IAudioPlayer SetScheduledEndTime(double dspTime);

		internal IAudioPlayer SetDelay(float time);
	}
}
