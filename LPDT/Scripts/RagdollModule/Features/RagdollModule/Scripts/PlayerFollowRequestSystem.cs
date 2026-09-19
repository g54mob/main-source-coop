using System;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.RagdollModule.Scripts
{
	public class PlayerFollowRequestSystem : IInitializable, IDisposable
	{
		private readonly NetworkPlayerFollowRequest _networkPlayerFollowRequest;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ILocalPlayerFollowService _localPlayerFollowService;

		public PlayerFollowRequestSystem(NetworkPlayerFollowRequest networkPlayerFollowRequest, MultiplayerModel multiplayerModel, ILocalPlayerFollowService localPlayerFollowService)
		{
			_networkPlayerFollowRequest = networkPlayerFollowRequest;
			_multiplayerModel = multiplayerModel;
			_localPlayerFollowService = localPlayerFollowService;
		}

		public void Initialize()
		{
			_networkPlayerFollowRequest.OnNetworkEventSend += ProcessPlayerFollowRequest;
		}

		public void Dispose()
		{
			_networkPlayerFollowRequest.OnNetworkEventSend -= ProcessPlayerFollowRequest;
		}

		private void ProcessPlayerFollowRequest(NetworkPlayerFollowRequest networkPlayerFollowRequest)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == networkPlayerFollowRequest.PlayerId)
			{
				switch (networkPlayerFollowRequest.EventType)
				{
				case PlayerFollowEventType.Enter:
					_localPlayerFollowService.EnterFollowMode();
					break;
				case PlayerFollowEventType.Exit:
					_localPlayerFollowService.ExitFollowMode();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
