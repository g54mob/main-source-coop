using System;
using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenStoreAudioMuteSystem : IInitializable, IDisposable
	{
		private readonly KrakenBehaviourConfiguration _configuration;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IAudioService _audioService;

		private EventInstance _snapshotInstance;

		private bool _isPlaying;

		public KrakenStoreAudioMuteSystem(KrakenBehaviourConfiguration configuration, PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, IAudioService audioService)
		{
			_configuration = configuration;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_audioService = audioService;
		}

		public void Initialize()
		{
			if (!_configuration.StoreKrakenMuteSnapshot.IsNull)
			{
				_snapshotInstance = _audioService.CreateInstance(_configuration.StoreKrakenMuteSnapshot);
			}
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit += OnPlayerStateExit;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit -= OnPlayerStateExit;
			StopSnapshot();
			if (_snapshotInstance.isValid())
			{
				_audioService.ReleaseInstance(_snapshotInstance);
				_snapshotInstance.clearHandle();
			}
		}

		private void OnPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (IsLocalPlayer(playerStateData.PlayerId) && playerStateData.PlayerState == PlayerState.Store)
			{
				StartSnapshot();
			}
		}

		private void OnPlayerStateExit(PlayerStateData playerStateData)
		{
			if (IsLocalPlayer(playerStateData.PlayerId) && playerStateData.PlayerState == PlayerState.Store)
			{
				StopSnapshot();
			}
		}

		private bool IsLocalPlayer(int playerId)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner != null && networkRunner.IsRunning)
			{
				return networkRunner.LocalPlayer.PlayerId == playerId;
			}
			return false;
		}

		private void StartSnapshot()
		{
			if (!_configuration.StoreKrakenMuteSnapshot.IsNull)
			{
				if (!_snapshotInstance.isValid())
				{
					_snapshotInstance = _audioService.CreateInstance(_configuration.StoreKrakenMuteSnapshot);
				}
				if (!_isPlaying)
				{
					_isPlaying = true;
					_audioService.StartSnapshotInstance(_snapshotInstance);
				}
			}
		}

		private void StopSnapshot()
		{
			if (_isPlaying)
			{
				_isPlaying = false;
				if (_snapshotInstance.isValid())
				{
					_audioService.StopInstance(_snapshotInstance, STOP_MODE.ALLOWFADEOUT);
				}
			}
		}
	}
}
