namespace Photon.Realtime
{
	public interface IOnMessageCallback
	{
		void OnMessage(bool isRawMessage, object message);
	}
}
