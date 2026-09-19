using Features.CustomNetworkEventsModule.Scripts;

namespace Features.SceneManagement
{
	public class PeerDespawnedObjectsNetworkEvent : NetworkEventBase<PeerDespawnedObjectsNetworkEvent>
	{
		public void SendEvent()
		{
			Send();
		}
	}
}
