using System;
using Features.KrakenModule.Scripts.Data;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenPlayerDisconnectSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly KrakenRuntimeModel _runtimeModel;

		public KrakenPlayerDisconnectSystem(NetworkRunnerEventBus eventBus, KrakenRuntimeModel runtimeModel)
		{
			_eventBus = eventBus;
			_runtimeModel = runtimeModel;
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
			if (!(playerLeftEvent.Runner == null) && playerLeftEvent.Runner.IsRunning && !(playerLeftEvent.Player == playerLeftEvent.Runner.LocalPlayer) && _runtimeModel.TryGetController(out var controller))
			{
				controller.HandlePlayerDisconnected(playerLeftEvent.Player);
			}
		}
	}
}
