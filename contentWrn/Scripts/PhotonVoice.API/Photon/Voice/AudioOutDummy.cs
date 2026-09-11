namespace Photon.Voice
{
	public class AudioOutDummy<T> : IAudioOut<T>
	{
		public bool IsPlaying => false;

		public int Lag => 0;

		public void Flush()
		{
		}

		public void Push(T[] frame)
		{
		}

		public void Service()
		{
		}

		public void Start(int frequency, int channels, int frameSamplesPerChannel)
		{
		}

		public void Stop()
		{
		}
	}
}
