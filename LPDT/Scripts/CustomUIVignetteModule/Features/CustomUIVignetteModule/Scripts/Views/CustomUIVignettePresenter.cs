using System.Collections;
using Features.MultiplayerSessionServices.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.CustomUIVignetteModule.Scripts.Views
{
	[PublicAPI]
	public class CustomUIVignettePresenter : PresenterBehaviour<CustomUIVignetteViewBase>
	{
		private readonly OnVignetteStartedNetworkEvent _onVignetteStartedNetworkEvent;

		private readonly OnVignetteDisabledNetworkEvent _onVignetteDisabledNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private Coroutine _vignetteCoroutine;

		public CustomUIVignettePresenter(OnVignetteStartedNetworkEvent onVignetteStartedNetworkEvent, OnVignetteDisabledNetworkEvent onVignetteDisabledNetworkEvent, MultiplayerModel multiplayerModel)
		{
			_onVignetteStartedNetworkEvent = onVignetteStartedNetworkEvent;
			_onVignetteDisabledNetworkEvent = onVignetteDisabledNetworkEvent;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			DisableVignette();
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_onVignetteStartedNetworkEvent.OnNetworkEventSend += StartVignette;
			_onVignetteDisabledNetworkEvent.OnNetworkEventSend += DisableVignette;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_onVignetteStartedNetworkEvent.OnNetworkEventSend -= StartVignette;
			_onVignetteDisabledNetworkEvent.OnNetworkEventSend -= DisableVignette;
		}

		private void StartVignette(OnVignetteStartedNetworkEvent onVignetteStartedNetworkEvent)
		{
			if (onVignetteStartedNetworkEvent.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				if (_vignetteCoroutine != null)
				{
					base.View.StopCoroutine(_vignetteCoroutine);
				}
				_vignetteCoroutine = base.View.StartCoroutine(VignetteCoroutine(onVignetteStartedNetworkEvent.Time, onVignetteStartedNetworkEvent.CurrentTime));
			}
		}

		private void DisableVignette(OnVignetteDisabledNetworkEvent onvignetteDisabledNetworkEvent)
		{
			if (onvignetteDisabledNetworkEvent.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				DisableVignette();
			}
		}

		private void DisableVignette()
		{
			if (_vignetteCoroutine != null)
			{
				base.View.StopCoroutine(_vignetteCoroutine);
				_vignetteCoroutine = null;
			}
			base.View.VignetteCanvasGroup.alpha = 0f;
		}

		private IEnumerator VignetteCoroutine(float time, float currentTime)
		{
			float elapsed = currentTime;
			float startAlpha = base.View.VignetteCanvasGroup.alpha;
			while (elapsed < time)
			{
				elapsed += Time.deltaTime;
				float time2 = Mathf.Clamp01(elapsed / time);
				float t = base.View.VignetteCurve.Evaluate(time2);
				base.View.VignetteCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
				yield return null;
			}
			base.View.VignetteCanvasGroup.alpha = 1f;
			_vignetteCoroutine = null;
		}
	}
}
