using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class MusicSystem : IInitializable, IDisposable
	{
		private readonly MusicConfiguration _musicConfiguration;

		private EventInstance _menuMusicInstance;

		private readonly IWindowsService _windowsService;

		private readonly ICoroutineRunner _coroutineRunner;

		private Coroutine _fadeCoroutine;

		private Coroutine _storeAmbianceFadeCoroutine;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private EventInstance _storeAmbianceInstance;

		public MusicSystem(MusicConfiguration musicConfiguration, IWindowsService windowsService, ICoroutineRunner coroutineRunner, PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel)
		{
			_musicConfiguration = musicConfiguration;
			_windowsService = windowsService;
			_coroutineRunner = coroutineRunner;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_menuMusicInstance = RuntimeManager.CreateInstance(_musicConfiguration.MenuMusic);
			_storeAmbianceInstance = RuntimeManager.CreateInstance(_musicConfiguration.StoreAmbiance);
			_menuMusicInstance.start();
			_windowsService.OnWindowOpened += ProcessMenuMusicOnWindowOpened;
			_windowsService.OnWindowClosed += ProcessMenuMusicOnWindowClosed;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += CheckPlayerSomePlayerState;
			_playersStatesSynchronizer.OnSomePlayerStateExit += CheckPlayerSomePlayerStateExit;
		}

		private void CheckPlayerSomePlayerStateExit(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerStateData.PlayerId && playerStateData.PlayerState == PlayerState.Store)
			{
				if (_storeAmbianceFadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_storeAmbianceFadeCoroutine);
				}
				_storeAmbianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		private void CheckPlayerSomePlayerState(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerStateData.PlayerId && playerStateData.PlayerState == PlayerState.Store)
			{
				if (_storeAmbianceFadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_storeAmbianceFadeCoroutine);
				}
				_storeAmbianceFadeCoroutine = _coroutineRunner.StartCoroutine(LowPassFadeOutRoutine(1f, _musicConfiguration.MenuFadeDuration, _storeAmbianceInstance));
				_storeAmbianceInstance.start();
			}
		}

		public void Dispose()
		{
			_windowsService.OnWindowOpened -= ProcessMenuMusicOnWindowOpened;
			_windowsService.OnWindowClosed -= ProcessMenuMusicOnWindowClosed;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= CheckPlayerSomePlayerState;
			_playersStatesSynchronizer.OnSomePlayerStateExit -= CheckPlayerSomePlayerStateExit;
			_menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			_menuMusicInstance.release();
			_storeAmbianceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			_storeAmbianceInstance.release();
		}

		private void ProcessMenuMusicOnWindowClosed(Type windowType)
		{
			if (windowType == typeof(MenuWindow))
			{
				if (_fadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_fadeCoroutine);
				}
				_fadeCoroutine = _coroutineRunner.StartCoroutine(LowPassFadeOutRoutine(0f, 2f, _menuMusicInstance));
			}
			if (windowType == typeof(LobbyWindow))
			{
				if (_fadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_fadeCoroutine);
				}
				_menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		private void ProcessMenuMusicOnWindowOpened(Type windowType)
		{
			if (windowType == typeof(MenuWindow))
			{
				if (_fadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_fadeCoroutine);
				}
				_fadeCoroutine = _coroutineRunner.StartCoroutine(LowPassFadeOutRoutine(1f, _musicConfiguration.MenuFadeDuration, _menuMusicInstance));
				_menuMusicInstance.start();
			}
			if (windowType == typeof(SessionWindow))
			{
				if (_fadeCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_fadeCoroutine);
				}
				_menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		private IEnumerator LowPassFadeOutRoutine(float targetValue, float fadeOutTime, EventInstance instance)
		{
			float timer = 0f;
			instance.getParameterByName(_musicConfiguration.ParameterName, out var lowPassParameter);
			while (timer < fadeOutTime)
			{
				timer += Time.deltaTime;
				float t = timer / fadeOutTime;
				instance.setParameterByName(_musicConfiguration.ParameterName, Mathf.Lerp(lowPassParameter, targetValue, t));
				yield return null;
			}
			instance.setParameterByName(_musicConfiguration.ParameterName, targetValue);
		}
	}
}
