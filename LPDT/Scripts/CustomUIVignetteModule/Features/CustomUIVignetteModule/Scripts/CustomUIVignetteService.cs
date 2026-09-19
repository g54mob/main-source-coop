namespace Features.CustomUIVignetteModule.Scripts
{
	public class CustomUIVignetteService : ICustomUIVignetteService
	{
		private readonly OnVignetteStartedNetworkEvent _onVignetteStartedNetworkEvent;

		private readonly OnVignetteDisabledNetworkEvent _onVignetteDisabledNetworkEvent;

		public CustomUIVignetteService(OnVignetteStartedNetworkEvent onVignetteStartedNetworkEvent, OnVignetteDisabledNetworkEvent onVignetteDisabledNetworkEvent)
		{
			_onVignetteStartedNetworkEvent = onVignetteStartedNetworkEvent;
			_onVignetteDisabledNetworkEvent = onVignetteDisabledNetworkEvent;
		}

		public void StartVignetteShowCoroutineForPlayer(int playerId, float time, float currentTime)
		{
			_onVignetteStartedNetworkEvent.SendEvent(playerId, time, currentTime);
		}

		public void DisableVignetteForPlayer(int playerId)
		{
			_onVignetteDisabledNetworkEvent.SendEvent(playerId);
		}
	}
}
