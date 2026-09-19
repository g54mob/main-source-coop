using System;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearPlayerDisconnectSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly HeadwearModel _headwearModel;

		public HeadwearPlayerDisconnectSystem(NetworkRunnerEventBus eventBus, HeadwearModel headwearModel)
		{
			_eventBus = eventBus;
			_headwearModel = headwearModel;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
		}

		private void HandlePlayerLeft(OnPlayerLeftEvent playerLeftEvent)
		{
			if (!(playerLeftEvent.Runner == null) && playerLeftEvent.Runner.IsRunning && playerLeftEvent.Runner.IsSharedModeMasterClient && _headwearModel.TryGetHeadwear(playerLeftEvent.Player.PlayerId, out var headwear) && !(headwear == null) && !(headwear.Object == null) && headwear.Object.IsValid)
			{
				headwear.DropOnWearerLeft();
			}
		}
	}
}
