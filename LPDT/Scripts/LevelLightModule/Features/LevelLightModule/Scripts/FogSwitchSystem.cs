using System;
using System.Collections;
using Features.CoroutineUtils.Scripts;
using Features.LevelGatesModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	public class FogSwitchSystem : IInitializable, IDisposable
	{
		private readonly OnLevelLoadedNetworkEvent _onLevelLoadedNetworkEvent;

		private readonly LevelGatesModel _levelGatesModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly FogSwitchConfiguration _fogSwitchConfiguration;

		private readonly MultiplayerModel _multiplayerModel;

		private Coroutine _runningFogSwitchRoutine;

		public FogSwitchSystem(OnLevelLoadedNetworkEvent onLevelLoadedNetworkEvent, LevelGatesModel levelGatesModel, ICoroutineRunner coroutineRunner, FogSwitchConfiguration fogSwitchConfiguration, MultiplayerModel multiplayerModel)
		{
			_onLevelLoadedNetworkEvent = onLevelLoadedNetworkEvent;
			_levelGatesModel = levelGatesModel;
			_coroutineRunner = coroutineRunner;
			_fogSwitchConfiguration = fogSwitchConfiguration;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_onLevelLoadedNetworkEvent.OnNetworkEventSend += InitializeFog;
			_levelGatesModel.OnLocalPlayerInsideGateChanged += SwitchFog;
		}

		public void Dispose()
		{
			_onLevelLoadedNetworkEvent.OnNetworkEventSend -= InitializeFog;
			_levelGatesModel.OnLocalPlayerInsideGateChanged -= SwitchFog;
		}

		private void InitializeFog(OnLevelLoadedNetworkEvent onLevelLoadedNetworkEvent)
		{
			if (onLevelLoadedNetworkEvent.PlayerID == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				SwitchFog(_levelGatesModel.IsLocalPlayerInsideGate);
			}
		}

		private void SwitchFog(bool isLocalPlayerInsideGate)
		{
			if (_runningFogSwitchRoutine != null)
			{
				_coroutineRunner.StopCoroutine(_runningFogSwitchRoutine);
			}
			_runningFogSwitchRoutine = _coroutineRunner.StartCoroutine(FogSwitchRoutine(_fogSwitchConfiguration.FogSwitchTime, GetDestDistance(isLocalPlayerInsideGate)));
		}

		private IEnumerator FogSwitchRoutine(float switchTime, float destEndDistance)
		{
			float currentTimer = 0f;
			float initialEndDistance = RenderSettings.fogEndDistance;
			while (currentTimer < switchTime)
			{
				float t = Mathf.Clamp01(currentTimer / switchTime);
				RenderSettings.fogEndDistance = Mathf.Lerp(initialEndDistance, destEndDistance, t);
				currentTimer += Time.deltaTime;
				yield return null;
			}
			RenderSettings.fogEndDistance = destEndDistance;
			_runningFogSwitchRoutine = null;
		}

		private float GetDestDistance(bool isLocalPlayerInsideGate)
		{
			if (!isLocalPlayerInsideGate)
			{
				return _fogSwitchConfiguration.BeachFogEndDistance;
			}
			return _fogSwitchConfiguration.LocationFogEndDistance;
		}
	}
}
