using Features.CustomNetworkEventsModule.Scripts;

namespace Features.MainMenuModule.Scripts
{
	public class StartGameLoadingNetworkEvent : NetworkEventBase<StartGameLoadingNetworkEvent>
	{
		public void SendEvent()
		{
			Send();
		}
	}
}
