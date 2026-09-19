namespace Photon.Client
{
	public interface ITrafficRecorder
	{
		void Record(byte[] data, int dataLength, bool incoming, short peerId, PhotonSocket connection);
	}
}
