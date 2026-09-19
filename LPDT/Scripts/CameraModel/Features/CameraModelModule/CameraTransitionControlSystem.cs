using System;
using System.Collections;
using Features.CoroutineUtils.Scripts;
using Features.PlayerRenderModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.CameraModelModule
{
	public class CameraTransitionControlSystem : IInitializable, IDisposable
	{
		private readonly CameraModel _cameraModel;

		private readonly CameraTransitionConfiguration _cameraTransitionConfiguration;

		private readonly IPlayerRenderService _playerRenderService;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly CameraTransitionSkinUpdateRequest _cameraTransitionSkinUpdateRequest;

		private Coroutine _transitionRoutine;

		public CameraTransitionControlSystem(CameraModel cameraModel, CameraTransitionConfiguration cameraTransitionConfiguration, IPlayerRenderService playerRenderService, ICoroutineRunner coroutineRunner, CameraTransitionSkinUpdateRequest cameraTransitionSkinUpdateRequest)
		{
			_cameraModel = cameraModel;
			_cameraTransitionConfiguration = cameraTransitionConfiguration;
			_playerRenderService = playerRenderService;
			_coroutineRunner = coroutineRunner;
			_cameraTransitionSkinUpdateRequest = cameraTransitionSkinUpdateRequest;
		}

		public void Initialize()
		{
			_cameraModel.OnCameraTransitionEnqueued += ProcessCameraTransition;
		}

		public void Dispose()
		{
			_cameraModel.OnCameraTransitionEnqueued -= ProcessCameraTransition;
		}

		private void ProcessCameraTransition(CameraType currentType, CameraType transitedType)
		{
			CameraTransitionKey key = new CameraTransitionKey(currentType, transitedType);
			if (_cameraTransitionConfiguration.CameraTransitionsMap.TryGetValue(key, out var value))
			{
				if (value.TransitionViewType == CameraTransitionViewType.AlignWithPrevView)
				{
					Vector3 forward = _cameraModel.CameraObject.transform.forward;
					_cameraModel.Cameras[transitedType].SetHorizontalRotation(forward);
					_cameraModel.Cameras[transitedType].SetVerticalRotation(forward);
				}
				if (_transitionRoutine != null)
				{
					_coroutineRunner.StopCoroutine(_transitionRoutine);
				}
				_transitionRoutine = _coroutineRunner.StartCoroutine(ProcessTransitionRoutine(value, transitedType));
			}
		}

		private IEnumerator ProcessTransitionRoutine(CameraTransitionPreset cameraTransitionPreset, CameraType transitedType)
		{
			if (_cameraModel.CameraBrain == null)
			{
				yield break;
			}
			CameraSkinUpdateTiming skinUpdateTiming = GetSkinUpdateTiming(cameraTransitionPreset);
			RequestSkinUpdateIfTimingMatches(skinUpdateTiming, CameraSkinUpdateTiming.TransitionStart, transitedType);
			if (cameraTransitionPreset.PlayerRenderTransition.Enabled)
			{
				_playerRenderService.SetPlayerSurfaceType(cameraTransitionPreset.PlayerRenderTransition.PlayerStartSurface);
				_playerRenderService.SetPlayerSurfaceRenderQueue(cameraTransitionPreset.PlayerRenderTransition.PlayerStartRenderQueue);
			}
			float timer = 0f;
			while (timer <= _cameraModel.CameraBrain.DefaultBlend.BlendTime)
			{
				float time = Mathf.Clamp01(timer / _cameraModel.CameraBrain.DefaultBlend.BlendTime);
				if (cameraTransitionPreset.PlayerRenderTransition.Enabled)
				{
					float t = Mathf.Clamp01(cameraTransitionPreset.PlayerRenderTransition.RenderUpdateCurve.Evaluate(time));
					float playerAlpha = Mathf.Lerp(cameraTransitionPreset.PlayerRenderTransition.PlayerStartTransitionAlpha, cameraTransitionPreset.PlayerRenderTransition.PlayerEndTransitionAlpha, t);
					_playerRenderService.SetPlayerAlpha(playerAlpha);
				}
				timer += Time.deltaTime;
				yield return null;
			}
			if (cameraTransitionPreset.PlayerRenderTransition.Enabled)
			{
				_playerRenderService.SetPlayerAlpha(cameraTransitionPreset.PlayerRenderTransition.PlayerEndTransitionAlpha);
				_playerRenderService.SetPlayerSurfaceType(cameraTransitionPreset.PlayerRenderTransition.PlayerEndSurface);
				_playerRenderService.SetPlayerSurfaceRenderQueue(cameraTransitionPreset.PlayerRenderTransition.PlayerEndRenderQueue);
			}
			RequestSkinUpdateIfTimingMatches(skinUpdateTiming, CameraSkinUpdateTiming.TransitionEnd, transitedType);
		}

		private CameraSkinUpdateTiming GetSkinUpdateTiming(CameraTransitionPreset cameraTransitionPreset)
		{
			if (cameraTransitionPreset.CameraSkinUpdateTimingOverride == CameraSkinUpdateTiming.None)
			{
				return _cameraTransitionConfiguration.CameraSkinUpdateTiming;
			}
			return cameraTransitionPreset.CameraSkinUpdateTimingOverride;
		}

		private void RequestSkinUpdateIfTimingMatches(CameraSkinUpdateTiming resolvedTiming, CameraSkinUpdateTiming currentTiming, CameraType transitedType)
		{
			if (resolvedTiming == currentTiming)
			{
				_cameraTransitionSkinUpdateRequest.Request(transitedType == CameraType.TPCamera);
			}
		}
	}
}
