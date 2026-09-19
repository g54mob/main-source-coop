using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Zenject;

namespace Features.SceneTransitionsModule.Scripts
{
	public class NetworkTransitionProcessSystem : IInitializable, IDisposable
	{
		private readonly StartFadeTransitionNetworkEvent _startFadeTransitionNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ILoadingScreenService _loadingScreenService;

		public NetworkTransitionProcessSystem(StartFadeTransitionNetworkEvent startFadeTransitionNetworkEvent, MultiplayerModel multiplayerModel, ILoadingScreenService loadingScreenService)
		{
			_startFadeTransitionNetworkEvent = startFadeTransitionNetworkEvent;
			_multiplayerModel = multiplayerModel;
			_loadingScreenService = loadingScreenService;
		}

		public void Initialize()
		{
			_startFadeTransitionNetworkEvent.OnNetworkEventSend += ProcessFadeTransition;
		}

		public void Dispose()
		{
			_startFadeTransitionNetworkEvent.OnNetworkEventSend -= ProcessFadeTransition;
		}

		private void ProcessFadeTransition(StartFadeTransitionNetworkEvent fadeTransitionNetworkEvent)
		{
			if (fadeTransitionNetworkEvent == null)
			{
				return;
			}
			if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.IsRunning && fadeTransitionNetworkEvent.OnlyOnClient && fadeTransitionNetworkEvent.Owner != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
				{
					_ = fadeTransitionNetworkEvent.Owner;
					_ = 1;
				}
			}
			else
			{
				StartTransition(fadeTransitionNetworkEvent.TransitionType);
			}
		}

		private void StartTransition(FadeTransitionType type)
		{
			switch (type)
			{
			case FadeTransitionType.FadeId:
				if (!_loadingScreenService.IsContentLoadingActive)
				{
					_loadingScreenService.Show(LoadingScreenShowType.ShowUntilPlayersLoading);
				}
				break;
			case FadeTransitionType.FadeOut:
				_loadingScreenService.FadeOut();
				break;
			}
		}
	}
}
