using Photon.Client;

namespace Photon.Realtime
{
	public interface IOnEventCallback
	{
		void OnEvent(EventData photonEvent);
	}
}
