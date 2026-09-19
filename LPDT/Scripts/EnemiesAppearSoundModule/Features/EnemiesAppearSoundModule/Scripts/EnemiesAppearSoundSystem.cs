using System;
using System.Collections.Generic;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.EnemiesAppearSoundModule.Scripts
{
	public class EnemiesAppearSoundSystem : IInitializable, IDisposable
	{
		private readonly EnemiesAppearSoundTransformsModel _enemiesAppearSoundTransformsModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly EnemiesAppearSoundConfiguration _configuration;

		private readonly IPlayerStateService _playerStateService;

		private readonly HashSet<ISoundSource> _seenEnemyTransforms = new HashSet<ISoundSource>();

		private readonly IGameUpdater _gameUpdater;

		private readonly IAudioService _audioService;

		public EnemiesAppearSoundSystem(EnemiesAppearSoundTransformsModel enemiesAppearSoundTransformsModel, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, EnemiesAppearSoundConfiguration configuration, IPlayerStateService playerStateService, IGameUpdater gameUpdater, IAudioService audioService)
		{
			_enemiesAppearSoundTransformsModel = enemiesAppearSoundTransformsModel;
			_playerMovableModel = playerMovableModel;
			_multiplayerModel = multiplayerModel;
			_configuration = configuration;
			_playerStateService = playerStateService;
			_gameUpdater = gameUpdater;
			_audioService = audioService;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += UpdateVision;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= UpdateVision;
			_seenEnemyTransforms.Clear();
		}

		public void UpdateVision()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			PlayerLookDetection playerLookDetection = _playerMovableModel.PlayerLookDetection;
			if (playerLookDetection == null)
			{
				return;
			}
			int playerId = networkRunner.LocalPlayer.PlayerId;
			if (!_playerStateService.IsPlayerAlive(playerId))
			{
				return;
			}
			PruneDestroyedEnemies();
			Vector3 vector = ((_playerMovableModel.LocalMovable != null) ? _playerMovableModel.LocalMovable.transform.position : playerLookDetection.transform.position);
			float num = _configuration.MaxDistanceToPlayAppearSound * _configuration.MaxDistanceToPlayAppearSound;
			float num2 = _configuration.DistanceToResetAppearSound * _configuration.DistanceToResetAppearSound;
			foreach (KeyValuePair<EnemyType, List<ITransformBasedSoundSource>> enemy in _enemiesAppearSoundTransformsModel.Enemies)
			{
				if (enemy.Key == EnemyType.None)
				{
					continue;
				}
				EventReference appearSound = _configuration.GetAppearSound(enemy.Key);
				if (appearSound.IsNull)
				{
					continue;
				}
				foreach (ITransformBasedSoundSource item in enemy.Value)
				{
					if (item == null)
					{
						continue;
					}
					float sqrMagnitude = (item.SourcePosition - vector).sqrMagnitude;
					bool flag = playerLookDetection.IsLookingAtObject(item.SoundSourceTransform);
					if (_seenEnemyTransforms.Contains(item))
					{
						if (!flag && sqrMagnitude > num2)
						{
							_seenEnemyTransforms.Remove(item);
						}
					}
					else if (flag && !(sqrMagnitude > num))
					{
						_seenEnemyTransforms.Add(item);
						_audioService.PlayOneShot(appearSound, item);
					}
				}
			}
		}

		private void PruneDestroyedEnemies()
		{
			if (_seenEnemyTransforms.Count != 0)
			{
				_seenEnemyTransforms.RemoveWhere((ISoundSource transform) => transform == null);
			}
		}
	}
}
