using Features.CustomNetworkEventsModule.Scripts;

namespace Features.LevelModule.Scripts
{
	public class StartLevelLoadingTransitionNetworkEvent : NetworkEventBase<StartLevelLoadingTransitionNetworkEvent>
	{
		public void SendEvent()
		{
			Send();
		}
	}
}
