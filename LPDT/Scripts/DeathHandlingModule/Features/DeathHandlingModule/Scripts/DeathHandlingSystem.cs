using System;
using System.Collections;
using System.Collections.Generic;
using Features.AudioServiceModule.Scripts;
using Features.CoroutineUtils.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.PostProcessingModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.DeathHandlingModule.Scripts
{
	public class DeathHandlingSystem : IInitializable, IDisposable
	{
		private const float DefaultIntensity = 0f;

		private const float DefaultSmoothness = 0f;

		private static readonly Color DefaultColor = Color.black;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DeathConfiguration _deathConfiguration;

		private readonly IPlayerStateService _playerStateService;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly PreDeadTimerModel _preDeadTimerModel;

		private readonly IAudioService _audioService;

		private Coroutine _deathWaitRoutine;

		private Coroutine _pendingReconnectDeathCoroutine;

		private bool _isReconnectDeathPending;

		private readonly VignetteEffect _deathVignetteEffect = new VignetteEffect();

		private readonly Dictionary<int, bool> _isDeadSoundPlayed = new Dictionary<int, bool>();

		public DeathHandlingSystem(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, DeathConfiguration deathConfiguration, IPlayerStateService playerStateService, ICoroutineRunner coroutineRunner, PostProcessingModel postProcessingModel, PlayerMovableModel playerMovableModel, PreDeadTimerModel preDeadTimerModel, IAudioService audioService)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_deathConfiguration = deathConfiguration;
			_playerStateService = playerStateService;
			_coroutineRunner = coroutineRunner;
			_postProcessingModel = postProcessingModel;
			_playerMovableModel = playerMovableModel;
			_preDeadTimerModel = preDeadTimerModel;
			_audioService = audioService;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerPlayerStateChanged;
			MultiplayerModel multiplayerModel = _multiplayerModel;
			multiplayerModel.ReconnectDeathAction = (Action<int>)Delegate.Combine(multiplayerModel.ReconnectDeathAction, new Action<int>(OnReconnectDeath));
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerPlayerStateChanged;
			MultiplayerModel multiplayerModel = _multiplayerModel;
			multiplayerModel.ReconnectDeathAction = (Action<int>)Delegate.Remove(multiplayerModel.ReconnectDeathAction, new Action<int>(OnReconnectDeath));
			if (_pendingReconnectDeathCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_pendingReconnectDeathCoroutine);
				_pendingReconnectDeathCoroutine = null;
			}
		}

		private void OnReconnectDeath(int playerId)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_isReconnectDeathPending = true;
			}
			if (_pendingReconnectDeathCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_pendingReconnectDeathCoroutine);
			}
			_pendingReconnectDeathCoroutine = _coroutineRunner.StartCoroutine(ApplyReconnectDeathEndOfFrame(playerId));
		}

		private IEnumerator ApplyReconnectDeathEndOfFrame(int playerId)
		{
			yield return new WaitForEndOfFrame();
			_pendingReconnectDeathCoroutine = null;
			_playersStatesSynchronizer.SetStateSynchronized(playerId, PlayerState.Dead);
			_isReconnectDeathPending = false;
		}

		private void OnSomePlayerPlayerStateChanged(PlayerStateData playerStateData)
		{
			ProcessDeathSound(playerStateData);
			if (playerStateData.PlayerId != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				return;
			}
			if (playerStateData.PlayerState == PlayerState.PreDeadCrouch)
			{
				if (_deathWaitRoutine != null)
				{
					TerminateDeathRoutine();
				}
				if (!_isReconnectDeathPending)
				{
					_deathWaitRoutine = _coroutineRunner.StartCoroutine(DeathWaitRoutine(_deathConfiguration.TimeToDie));
				}
			}
			else if (_playerStateService.IsPlayerAlive(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) && _deathWaitRoutine != null)
			{
				TerminateDeathRoutine();
			}
			else if (_playerStateService.GetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) == PlayerState.Store && _deathWaitRoutine != null)
			{
				TerminateDeathRoutine();
			}
		}

		private void ProcessDeathSound(PlayerStateData playerStateData)
		{
			if (_playerStateService.IsPlayerAlive(playerStateData.PlayerId))
			{
				_isDeadSoundPlayed[playerStateData.PlayerId] = false;
			}
			if (playerStateData.PlayerState != PlayerState.PreDeadCrouch || (_isDeadSoundPlayed.ContainsKey(playerStateData.PlayerId) && _isDeadSoundPlayed[playerStateData.PlayerId]))
			{
				return;
			}
			_isDeadSoundPlayed[playerStateData.PlayerId] = true;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (allCharacterMovable.Key.PlayerId == playerStateData.PlayerId)
				{
					_audioService.PlayOneShotAttached(_deathConfiguration.DeathEventInstance, new GenericTransformBasedSoundSource(allCharacterMovable.Value.transform));
				}
			}
		}

		private IEnumerator DeathWaitRoutine(float timeToDie)
		{
			float deathTimer = 0f;
			_deathVignetteEffect.EffectColor = DefaultColor;
			_deathVignetteEffect.EffectIntensity = 0f;
			_deathVignetteEffect.EffectSmoothness = 0f;
			Volume ppVolume;
			bool tryGetValue = _postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out ppVolume);
			if (tryGetValue)
			{
				_postProcessingModel.ApplyVignetteEffect(ppVolume, _deathVignetteEffect);
			}
			_preDeadTimerModel.StartTimer(timeToDie);
			for (; deathTimer < timeToDie; deathTimer += Time.deltaTime)
			{
				float t = Mathf.Clamp01(deathTimer / timeToDie);
				_deathVignetteEffect.EffectColor = Color.Lerp(DefaultColor, _deathConfiguration.DeathVignetteColor, t);
				_deathVignetteEffect.EffectIntensity = Mathf.Lerp(0f, _deathConfiguration.DeathVignetteIntensity, t);
				_deathVignetteEffect.EffectSmoothness = Mathf.Lerp(0f, _deathConfiguration.DeathVignetteSmoothness, t);
				_preDeadTimerModel.SetCurrent(Mathf.Max(0f, timeToDie - deathTimer));
				yield return null;
			}
			_preDeadTimerModel.StopTimer();
			if (tryGetValue)
			{
				_postProcessingModel.RemoveVignetteEffect(ppVolume, _deathVignetteEffect);
			}
			_playerStateService.ChangePlayerState(PlayerState.Dead);
			PlayerSessionPrefs.SaveHealth(0f);
		}

		private void TerminateDeathRoutine()
		{
			if (_deathWaitRoutine != null)
			{
				_coroutineRunner.StopCoroutine(_deathWaitRoutine);
				_deathWaitRoutine = null;
			}
			_preDeadTimerModel.StopTimer();
			if (_postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out var value))
			{
				_postProcessingModel.RemoveVignetteEffect(value, _deathVignetteEffect);
			}
		}
	}
}
