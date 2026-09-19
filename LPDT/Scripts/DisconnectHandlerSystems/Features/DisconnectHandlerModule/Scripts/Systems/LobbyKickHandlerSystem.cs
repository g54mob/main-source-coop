using System;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Systems
{
	public class LobbyKickHandlerSystem : IInitializable, IDisposable
	{
		private readonly KickPlayerRequestNetworkEvent _kickPlayerRequestNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		public LobbyKickHandlerSystem(KickPlayerRequestNetworkEvent kickPlayerRequestNetworkEvent, MultiplayerModel multiplayerModel, DisconnectRequestEventClass disconnectRequestEventClass)
		{
			_kickPlayerRequestNetworkEvent = kickPlayerRequestNetworkEvent;
			_multiplayerModel = multiplayerModel;
			_disconnectRequestEventClass = disconnectRequestEventClass;
		}

		public void Initialize()
		{
			_kickPlayerRequestNetworkEvent.OnNetworkEventSend += OnKickRequested;
		}

		public void Dispose()
		{
			_kickPlayerRequestNetworkEvent.OnNetworkEventSend -= OnKickRequested;
		}

		private void OnKickRequested(KickPlayerRequestNetworkEvent kickRequest)
		{
			if (kickRequest.PlayerToKickId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_disconnectRequestEventClass.Publish(DisconnectRequestReason.LobbyLeave);
			}
		}
	}
}
