using System.Collections;
using Features.CoroutineUtils.Scripts;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.LevelModule.Scripts.LevelTransition
{
	[PublicAPI]
	public class LevelTransitionTimerPresenter : PresenterBehaviour<LevelTransitionTimerViewBase>
	{
		private readonly LevelTransitionTimerSynchronizedModel _levelTransitionTimerSynchronizedModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private Coroutine _fadeCoroutine;

		private readonly ILocalizationService _localizationService;

		public LevelTransitionTimerPresenter(LevelTransitionTimerSynchronizedModel levelTransitionTimerSynchronizedModel, ICoroutineRunner coroutineRunner, ILocalizationService localizationService)
		{
			_levelTransitionTimerSynchronizedModel = levelTransitionTimerSynchronizedModel;
			_coroutineRunner = coroutineRunner;
			_localizationService = localizationService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			base.View.CanvasGroup.alpha = 0f;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			SetTimerSlider(_levelTransitionTimerSynchronizedModel.CurrentTimer, _levelTransitionTimerSynchronizedModel.MaxTimer);
			_levelTransitionTimerSynchronizedModel.OnTimerChanged += SetTimerSlider;
			_levelTransitionTimerSynchronizedModel.OnTimerRunningChanged += FadeTimer;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_levelTransitionTimerSynchronizedModel.OnTimerChanged -= SetTimerSlider;
			_levelTransitionTimerSynchronizedModel.OnTimerRunningChanged -= FadeTimer;
		}

		private void SetTimerSlider(float value, float max)
		{
			if (!_levelTransitionTimerSynchronizedModel.IsTimerRunning)
			{
				value = max;
			}
			float num = ((max != 0f) ? (value / max) : 0f);
			base.View.Fill.fillAmount = num;
			base.View.Arrow.localRotation = Quaternion.Euler(0f, 0f, (0f - num) * 360f);
		}

		private void FadeTimer(bool isRunning)
		{
			if (_fadeCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_fadeCoroutine);
			}
			if (isRunning)
			{
				_fadeCoroutine = _coroutineRunner.StartCoroutine(LerpFadeTimer(1f));
			}
			else
			{
				base.View.CanvasGroup.alpha = 0f;
			}
		}

		private IEnumerator LerpFadeTimer(float targetAlpha)
		{
			yield return new WaitForSeconds(base.View.DelayBeforeFade);
			float startAlpha = base.View.CanvasGroup.alpha;
			float elapsed = 0f;
			while (elapsed < base.View.FadeDuration)
			{
				elapsed += Time.deltaTime;
				float t = base.View.LerpCurve.Evaluate(Mathf.Clamp01(elapsed / base.View.FadeDuration));
				base.View.CanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
				yield return null;
			}
			base.View.CanvasGroup.alpha = targetAlpha;
		}
	}
}
