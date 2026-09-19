using System;
using System.Collections.Generic;
using System.Linq;
using Features.LevelGatesModule.Data;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.PostProcessingModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.ManInShadows
{
	public class ManInShadowFlickerAdjustingService : IManInShadowFlickerAdjustingService, IInitializable, IDisposable
	{
		private readonly FlickerAdjustingModel _flickerAdjustingModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly INavigationService _navigationService;

		private readonly ManInShadowFlickerConfiguration _manInShadowFlickerConfiguration;

		private readonly IPlayerStateService _playerStateService;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private readonly Dictionary<GameObject, float> _instanceBlends = new Dictionary<GameObject, float>();

		public ManInShadowFlickerAdjustingService(FlickerAdjustingModel flickerAdjustingModel, MultiplayerModel multiplayerModel, INavigationService navigationService, ManInShadowFlickerConfiguration manInShadowFlickerConfiguration, IPlayerStateService playerStateService, PlayersStatesSynchronizer playersStatesSynchronizer, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel)
		{
			_flickerAdjustingModel = flickerAdjustingModel;
			_multiplayerModel = multiplayerModel;
			_navigationService = navigationService;
			_manInShadowFlickerConfiguration = manInShadowFlickerConfiguration;
			_playerStateService = playerStateService;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
			_playersGatesModelSynchronizedModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
			_playersGatesModelSynchronizedModel.OnPlayerInsideGateChanged -= OnPlayerInsideGateChanged;
		}

		public void RegisterManInShadow(GameObject manInShadow)
		{
			_instanceBlends.TryAdd(manInShadow, 0f);
		}

		public void SetInstanceBlend(GameObject manInShadow, float blend)
		{
			if (_instanceBlends.ContainsKey(manInShadow))
			{
				_instanceBlends[manInShadow] = Mathf.Clamp01(IsLocalPlayerFlickerBlocked() ? 0f : blend);
				ApplyAggregatedBlend();
			}
		}

		public void UnregisterManInShadow(GameObject manInShadow)
		{
			_instanceBlends.Remove(manInShadow);
			ApplyAggregatedBlend();
		}

		private void ApplyAggregatedBlend()
		{
			if (_instanceBlends.Count == 0)
			{
				_flickerAdjustingModel.ChangeBlend(0f);
				return;
			}
			List<KeyValuePair<GameObject, float>> list = _instanceBlends.Where((KeyValuePair<GameObject, float> instanceBlend) => instanceBlend.Key != null).ToList();
			if (list.Count == 0)
			{
				_flickerAdjustingModel.ChangeBlend(0f);
				return;
			}
			float blend = list.Max((KeyValuePair<GameObject, float> instanceBlend) => instanceBlend.Value * CalculateDistanceBlend(instanceBlend.Key));
			_flickerAdjustingModel.ChangeBlend(blend);
		}

		private float CalculateDistanceBlend(GameObject manInShadow)
		{
			if (IsLocalPlayerFlickerBlocked())
			{
				return 0f;
			}
			if (!_multiplayerModel.NetworkRunner.IsRunning || !_multiplayerModel.NetworkRunner.LocalPlayer.IsValid)
			{
				return 0f;
			}
			if (!_navigationService.TryGetPlayerTrackingPosition(_multiplayerModel.NetworkRunner.LocalPlayer, out var position))
			{
				return 0f;
			}
			float value = Vector3.Distance(manInShadow.transform.position, position);
			return Mathf.InverseLerp(_manInShadowFlickerConfiguration.MaxDistanceForBlend, 0f, value);
		}

		private void OnPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.IsRunning && _multiplayerModel.NetworkRunner.LocalPlayer.IsValid && playerStateData.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && IsFlickerBlockedState(playerStateData.PlayerState))
			{
				ClearLocalPlayerFlicker();
			}
		}

		private void OnPlayerInsideGateChanged(int ownerId, bool insideGate)
		{
			if (_multiplayerModel.NetworkRunner.IsRunning && _multiplayerModel.NetworkRunner.LocalPlayer.IsValid && ownerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && !insideGate)
			{
				ClearLocalPlayerFlicker();
			}
		}

		private void ClearLocalPlayerFlicker()
		{
			foreach (GameObject item in _instanceBlends.Keys.ToList())
			{
				_instanceBlends[item] = 0f;
			}
			_flickerAdjustingModel.ChangeBlend(0f);
		}

		private bool IsLocalPlayerFlickerBlocked()
		{
			if (!_multiplayerModel.NetworkRunner.IsRunning || !_multiplayerModel.NetworkRunner.LocalPlayer.IsValid)
			{
				return false;
			}
			if (IsFlickerBlockedState(_playerStateService.GetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)))
			{
				return true;
			}
			if (!_playersGatesModelSynchronizedModel.TryGetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var state))
			{
				return false;
			}
			return !state.PlayerInsideGate;
		}

		private static bool IsFlickerBlockedState(PlayerState state)
		{
			if (state != PlayerState.Dead)
			{
				return state == PlayerState.PreDeadCrouch;
			}
			return true;
		}
	}
}
