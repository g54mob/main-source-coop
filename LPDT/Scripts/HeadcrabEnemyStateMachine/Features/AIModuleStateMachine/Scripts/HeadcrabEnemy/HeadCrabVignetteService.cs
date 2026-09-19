using System;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings;
using Features.CustomUIVignetteModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.VignetteUIEffectModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public class HeadCrabVignetteService : IHeadCrabVignetteService, IInitializable, ITickable, IDisposable
	{
		private const VignetteUIEffectType HeadCrabEffectType = VignetteUIEffectType.HeadCrab;

		private readonly OnVignetteStartedNetworkEvent _onVignetteStartedNetworkEvent;

		private readonly OnVignetteDisabledNetworkEvent _onVignetteDisabledNetworkEvent;

		private readonly OnVignettePausedNetworkEvent _onVignettePausedNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly VignetteUIEffectModel _vignetteUIEffectModel;

		private readonly HeadcrabVignetteSettings _settings;

		private VignetteUIEffect _headCrabEffect;

		private bool _isFadeActive;

		private float _elapsed;

		private float _duration;

		private float _startIntensity;

		private bool _isPaused;

		public HeadCrabVignetteService(OnVignetteStartedNetworkEvent onVignetteStartedNetworkEvent, OnVignetteDisabledNetworkEvent onVignetteDisabledNetworkEvent, OnVignettePausedNetworkEvent onVignettePausedNetworkEvent, MultiplayerModel multiplayerModel, VignetteUIEffectModel vignetteUIEffectModel, HeadcrabVignetteSettings settings)
		{
			_onVignetteStartedNetworkEvent = onVignetteStartedNetworkEvent;
			_onVignetteDisabledNetworkEvent = onVignetteDisabledNetworkEvent;
			_onVignettePausedNetworkEvent = onVignettePausedNetworkEvent;
			_multiplayerModel = multiplayerModel;
			_vignetteUIEffectModel = vignetteUIEffectModel;
			_settings = settings;
		}

		public void Initialize()
		{
			_vignetteUIEffectModel.OnLayerRegistered += InitializeVignetteUIEffect;
			if (_vignetteUIEffectModel.ActiveLayers.ContainsKey(VignetteUIEffectType.HeadCrab))
			{
				InitializeVignetteUIEffect(VignetteUIEffectType.HeadCrab);
			}
			_onVignetteStartedNetworkEvent.OnNetworkEventSend += OnVignetteStarted;
			_onVignetteDisabledNetworkEvent.OnNetworkEventSend += OnVignetteDisabled;
			_onVignettePausedNetworkEvent.OnNetworkEventSend += OnVignettePaused;
		}

		public void Dispose()
		{
			_onVignetteStartedNetworkEvent.OnNetworkEventSend -= OnVignetteStarted;
			_onVignetteDisabledNetworkEvent.OnNetworkEventSend -= OnVignetteDisabled;
			_onVignettePausedNetworkEvent.OnNetworkEventSend -= OnVignettePaused;
			_vignetteUIEffectModel.OnLayerRegistered -= InitializeVignetteUIEffect;
			if (_headCrabEffect != null && _vignetteUIEffectModel.ActiveLayers.ContainsKey(VignetteUIEffectType.HeadCrab))
			{
				_vignetteUIEffectModel.RemoveVignetteEffect(VignetteUIEffectType.HeadCrab, _headCrabEffect);
			}
		}

		public void StartVignetteShowCoroutineForPlayer(int playerId, float time, float currentTime)
		{
			_onVignetteStartedNetworkEvent.SendEvent(playerId, time, currentTime);
		}

		public void DisableVignetteForPlayer(int playerId)
		{
			_onVignetteDisabledNetworkEvent.SendEvent(playerId);
		}

		public void SetVignettePausedForPlayer(int playerId, bool isPaused)
		{
			_onVignettePausedNetworkEvent.SendEvent(playerId, isPaused);
		}

		public void Tick()
		{
			if (_isFadeActive && _headCrabEffect != null && !(_duration <= 0f) && !_isPaused)
			{
				_elapsed += Time.deltaTime;
				float time = Mathf.Clamp01(_elapsed / _duration);
				float t = _settings.VignetteFadeCurve.Evaluate(time);
				_headCrabEffect.EffectIntensity = Mathf.Lerp(_startIntensity, 1f, t);
				if (_elapsed >= _duration)
				{
					_headCrabEffect.EffectIntensity = 1f;
				}
			}
		}

		private void OnVignetteStarted(OnVignetteStartedNetworkEvent networkEvent)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && networkEvent.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				EnsureHeadCrabEffectRegistered();
				_startIntensity = _headCrabEffect?.EffectIntensity ?? 0f;
				_elapsed = networkEvent.CurrentTime;
				_duration = networkEvent.Time;
				_isFadeActive = true;
			}
		}

		private void OnVignettePaused(OnVignettePausedNetworkEvent networkEvent)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && networkEvent.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_isPaused = networkEvent.IsPaused;
			}
		}

		private void OnVignetteDisabled(OnVignetteDisabledNetworkEvent networkEvent)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && networkEvent.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				DisableVignette();
			}
		}

		private void DisableVignette()
		{
			_isFadeActive = false;
			_isPaused = false;
			if (_headCrabEffect != null)
			{
				_headCrabEffect.EffectIntensity = 0f;
			}
		}

		private void EnsureHeadCrabEffectRegistered()
		{
			if (_headCrabEffect == null)
			{
				InitializeVignetteUIEffect(VignetteUIEffectType.HeadCrab);
			}
		}

		private void InitializeVignetteUIEffect(VignetteUIEffectType vignetteUIEffectType)
		{
			if (vignetteUIEffectType != VignetteUIEffectType.HeadCrab || !_vignetteUIEffectModel.ActiveLayers.ContainsKey(vignetteUIEffectType))
			{
				return;
			}
			if (_headCrabEffect != null)
			{
				if (!_vignetteUIEffectModel.ContainsEffect(vignetteUIEffectType, _headCrabEffect))
				{
					_vignetteUIEffectModel.ApplyVignetteEffect(vignetteUIEffectType, _headCrabEffect);
				}
			}
			else
			{
				_headCrabEffect = new VignetteUIEffect
				{
					EffectIntensity = 0f
				};
				_vignetteUIEffectModel.ApplyVignetteEffect(vignetteUIEffectType, _headCrabEffect);
			}
		}
	}
}
