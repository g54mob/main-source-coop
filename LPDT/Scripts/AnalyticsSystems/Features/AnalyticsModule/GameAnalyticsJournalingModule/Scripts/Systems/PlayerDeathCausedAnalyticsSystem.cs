using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.AnalyticsExtensions;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class PlayerDeathCausedAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CustomPlayerEventsSynchronizedModel _customPlayerEventsSynchronizedModel;

		private bool _deathEventSentForCurrentPreDead;

		public PlayerDeathCausedAnalyticsSystem(PlayersStatesSynchronizer playersStatesSynchronizer, PlayerDamageablesTrackModel playerDamageablesTrackModel, MultiplayerModel multiplayerModel, CustomPlayerEventsSynchronizedModel customPlayerEventsSynchronizedModel)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_multiplayerModel = multiplayerModel;
			_customPlayerEventsSynchronizedModel = customPlayerEventsSynchronizedModel;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
		}

		private void OnSomePlayerStateChanged(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (playerStateData.PlayerId != playerId)
			{
				return;
			}
			if (playerStateData.PlayerState != PlayerState.PreDeadCrouch)
			{
				if (playerStateData.PlayerState == PlayerState.Alive)
				{
					_deathEventSentForCurrentPreDead = false;
				}
			}
			else if (!_deathEventSentForCurrentPreDead)
			{
				_deathEventSentForCurrentPreDead = true;
				TrySendDeathCausedEvent(playerStateData.PlayerId);
			}
		}

		private void TrySendDeathCausedEvent(int localPlayerId)
		{
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(localPlayerId, out var value) && value is PlayerDamageable playerDamageable && playerDamageable.TryGetLastDamageSource(out var source))
			{
				string evenName = SessionAnalyticsEventNameExtensions.BuildDeathCausedEventName(source);
				_customPlayerEventsSynchronizedModel.SendPlayerEvent(localPlayerId, evenName);
			}
		}
	}
}
