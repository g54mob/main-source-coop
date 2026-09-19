using System.Collections;
using System.Linq;
using Features.CoroutineUtils.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using PlayerCustomization;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.QuotaModule.Scripts.Views
{
	[PublicAPI]
	public class QuotaPresenter : PresenterBehaviour<QuotaViewBase>
	{
		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly ItemDamageDisplayModel _itemDamageDisplayModel;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly MultiplayerModel _multiplayerModel;

		private Coroutine _quotaFadeCoroutine;

		private Coroutine _quotaSliderCoroutine;

		private Coroutine _quotaInHandSliderCoroutine;

		private Coroutine _quotaAllCoroutine;

		private float _startTarget;

		private float _startInHandTarget;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly ILocalizationService _localizationService;

		public QuotaPresenter(QuotaSynchronizedModel quotaSynchronizedModel, ICoroutineRunner coroutineRunner, QuotaCompletionModel quotaCompletionModel, ItemDamageDisplayModel itemDamageDisplayModel, PlayerCustomizationModel playerCustomizationModel, MultiplayerModel multiplayerModel, PlayersStatesSynchronizer playersStatesSynchronizer, ILocalizationService localizationService)
		{
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_coroutineRunner = coroutineRunner;
			_quotaCompletionModel = quotaCompletionModel;
			_itemDamageDisplayModel = itemDamageDisplayModel;
			_playerCustomizationModel = playerCustomizationModel;
			_multiplayerModel = multiplayerModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_localizationService = localizationService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			SetQuotaSlider(_quotaSynchronizedModel.CurrentQuota.Value, _quotaSynchronizedModel.MaxQuota.Value);
			_quotaSynchronizedModel.OnQuotaChanged += SetQuotaSlider;
			_quotaCompletionModel.OnQuotaCompleted += FadeQuota;
			_itemDamageDisplayModel.OnCurrentCurrencyChanged += CheckForInHandQuota;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += CheckPlayerSomePlayerState;
			_playersStatesSynchronizer.OnSomePlayerStateExit += CheckPlayerSomePlayerStateExit;
			_playerCustomizationModel.OnSlotsChanged += ApplyInHandSliderColor;
			ApplyInHandSliderColor();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_quotaSynchronizedModel.OnQuotaChanged -= SetQuotaSlider;
			_quotaCompletionModel.OnQuotaCompleted -= FadeQuota;
			_itemDamageDisplayModel.OnCurrentCurrencyChanged -= CheckForInHandQuota;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= CheckPlayerSomePlayerState;
			_playersStatesSynchronizer.OnSomePlayerStateExit -= CheckPlayerSomePlayerStateExit;
			_playerCustomizationModel.OnSlotsChanged -= ApplyInHandSliderColor;
			StopCoroutine(ref _quotaFadeCoroutine);
			StopCoroutine(ref _quotaSliderCoroutine);
			StopCoroutine(ref _quotaInHandSliderCoroutine);
			StopCoroutine(ref _quotaAllCoroutine);
		}

		private void StopCoroutine(ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				_coroutineRunner.StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private void ApplyInHandSliderColor()
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData s) => s.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (playerCustomizationSlotData != null)
			{
				base.View.SetInHandSliderColor(playerCustomizationSlotData.PrimaryColor);
			}
		}

		private void CheckPlayerSomePlayerStateExit(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerStateData.PlayerId && playerStateData.PlayerState == PlayerState.Store)
			{
				StopCoroutine(ref _quotaAllCoroutine);
				_quotaAllCoroutine = _coroutineRunner.StartCoroutine(LerpAll(1f, base.View.AllFadeDuration));
			}
		}

		private void CheckPlayerSomePlayerState(PlayerStateData playerStateData)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerStateData.PlayerId && playerStateData.PlayerState == PlayerState.Store)
			{
				StopCoroutine(ref _quotaAllCoroutine);
				_quotaAllCoroutine = _coroutineRunner.StartCoroutine(LerpAll(0f, base.View.AllFadeDurationExit));
			}
		}

		private void SetQuotaSlider(float value, float max)
		{
			if (_quotaSliderCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_quotaSliderCoroutine);
			}
			_quotaSliderCoroutine = _coroutineRunner.StartCoroutine(LerpQuotaSlider(value, max));
			CheckForInHandQuota(_itemDamageDisplayModel.CurrentCurrency);
		}

		private void CheckForInHandQuota(int currency)
		{
			ApplyInHandSliderColor();
			float value = (float)currency + _quotaSynchronizedModel.CurrentQuota.Value;
			if (_quotaCompletionModel.IsQuotaCompleted.Value)
			{
				value = 0f;
			}
			value = Mathf.Clamp(value, 0f, _quotaSynchronizedModel.MaxQuota.Value);
			if (_quotaInHandSliderCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_quotaInHandSliderCoroutine);
			}
			_quotaInHandSliderCoroutine = _coroutineRunner.StartCoroutine(LerpInHandQuotaSlider(value, _quotaSynchronizedModel.MaxQuota.Value));
		}

		private IEnumerator LerpQuotaSlider(float targetValue, float targetMax)
		{
			float startNormalized = base.View.QuotaHandleSlider.NormalizedValue;
			float targetNormalized = ((targetMax != 0f) ? (targetValue / targetMax) : 0f);
			targetNormalized = Mathf.Clamp01(targetNormalized);
			float startTarget = _startTarget;
			float elapsed = 0f;
			while (elapsed < base.View.LerpDuration)
			{
				elapsed += Time.deltaTime;
				float t = base.View.LerpCurve.Evaluate(Mathf.Clamp01(elapsed / base.View.LerpDuration));
				string arg = string.Empty;
				_startTarget = Mathf.Lerp(startTarget, targetValue, t);
				if (targetMax < _startTarget)
				{
					arg = $"<#3EEF00>(+{(int)(_startTarget - targetMax)}¢)</color>";
				}
				base.View.QuotaText.SetText($"<#FFF932>¢</color>{(int)_startTarget}/<#FFF932>¢</color><u>{(int)targetMax}</u>{arg}");
				base.View.QuotaHandleSlider.NormalizedValue = Mathf.Lerp(startNormalized, targetNormalized, t);
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.View.Container);
				yield return null;
			}
			string arg2 = string.Empty;
			if (targetMax < targetValue)
			{
				arg2 = $"<#3EEF00>(+{(int)(targetValue - targetMax)}¢)</color>";
			}
			base.View.QuotaHandleSlider.NormalizedValue = targetNormalized;
			base.View.QuotaText.SetText($"<#FFF932>¢</color>{(int)targetValue}/<#FFF932>¢</color><u>{(int)targetMax}</u>{arg2}");
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.View.Container);
		}

		private IEnumerator LerpInHandQuotaSlider(float targetValue, float targetMax)
		{
			float startNormalized = ((base.View.InHandHandleSlider.NormalizedValue < base.View.QuotaHandleSlider.NormalizedValue) ? base.View.QuotaHandleSlider.NormalizedValue : base.View.InHandHandleSlider.NormalizedValue);
			float targetNormalized = ((targetMax != 0f) ? (targetValue / targetMax) : 0f);
			float startTarget = _startInHandTarget;
			float elapsed = 0f;
			while (elapsed < base.View.LerpDuration)
			{
				elapsed += Time.deltaTime;
				float t = base.View.LerpCurve.Evaluate(Mathf.Clamp01(elapsed / base.View.LerpDuration));
				_startInHandTarget = Mathf.Lerp(startTarget, targetValue, t);
				base.View.InHandHandleSlider.NormalizedValue = Mathf.Lerp(startNormalized, targetNormalized, t);
				yield return null;
			}
			base.View.InHandHandleSlider.NormalizedValue = targetNormalized;
		}

		private void FadeQuota(bool isCompleted)
		{
			if (_quotaFadeCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_quotaFadeCoroutine);
			}
			_quotaFadeCoroutine = _coroutineRunner.StartCoroutine(LerpFadeQuota(isCompleted ? 0f : 1f));
		}

		private IEnumerator LerpFadeQuota(float targetAlpha)
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

		private IEnumerator LerpAll(float targetAlpha, float fadeTime)
		{
			float startAlpha = base.View.GlobalCanvasGroup.alpha;
			float elapsed = 0f;
			while (elapsed < fadeTime)
			{
				elapsed += Time.deltaTime;
				base.View.LerpCurve.Evaluate(Mathf.Clamp01(elapsed / fadeTime));
				base.View.GlobalCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeTime);
				yield return null;
			}
			base.View.GlobalCanvasGroup.alpha = targetAlpha;
		}
	}
}
