using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using UnityEngine.Rendering;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class StorePostProcessingSystem : IInitializable, IDisposable
	{
		private readonly PostProcessingModel _postProcessingModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private bool _isStore;

		private readonly IGameUpdater _gameUpdater;

		public StorePostProcessingSystem(PostProcessingModel postProcessingModel, MultiplayerModel multiplayerModel, PlayersStatesSynchronizer playersStatesSynchronizer, IGameUpdater gameUpdater)
		{
			_postProcessingModel = postProcessingModel;
			_multiplayerModel = multiplayerModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += HandlePlayerSomePlayerStateChanged;
			_gameUpdater.OnUpdate += UpdatePpWeight;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= HandlePlayerSomePlayerStateChanged;
			_gameUpdater.OnUpdate -= UpdatePpWeight;
		}

		private void UpdatePpWeight()
		{
			foreach (KeyValuePair<PostProcessingType, Volume> activeVolume in _postProcessingModel.ActiveVolumes)
			{
				if (activeVolume.Key == PostProcessingType.Store)
				{
					activeVolume.Value.weight = (_isStore ? 1 : 0);
				}
				else if (activeVolume.Key != PostProcessingType.Flicker)
				{
					activeVolume.Value.weight = ((!_isStore) ? 1 : 0);
				}
			}
		}

		private void HandlePlayerSomePlayerStateChanged(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerStateData.PlayerId)
			{
				if (playerStateData.PlayerState == PlayerState.Store)
				{
					_isStore = true;
				}
				else
				{
					_isStore = false;
				}
			}
		}
	}
}
