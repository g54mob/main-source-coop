using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using Zenject;

namespace Features.EmotesModule.Scripts
{
	public class EmoteAvailabilityByPlayerStateSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly EmoteAvailabilityModel _emoteAvailabilityModel;

		public EmoteAvailabilityByPlayerStateSystem(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, EmoteAvailabilityModel emoteAvailabilityModel)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_emoteAvailabilityModel = emoteAvailabilityModel;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
		}

		private void OnSomePlayerStateChanged(PlayerStateData data)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && data.PlayerId == networkRunner.LocalPlayer.PlayerId)
			{
				_emoteAvailabilityModel.SetEmoteActive(data.PlayerState != PlayerState.Dead && data.PlayerState != PlayerState.PreDeadCrouch && data.PlayerState != PlayerState.Store);
			}
		}
	}
}
