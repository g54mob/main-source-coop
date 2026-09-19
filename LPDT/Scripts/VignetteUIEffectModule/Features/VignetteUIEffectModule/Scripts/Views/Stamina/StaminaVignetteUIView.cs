using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Features.VignetteUIEffectModule.Scripts.Views.Stamina
{
	public class StaminaVignetteUIView : VignetteUIEffectUIViewBase
	{
		[SerializeField]
		private float _scaleMultiplier = 1.1f;

		[SerializeField]
		private List<Image> _vignetteImages = new List<Image>();

		[SerializeField]
		private CanvasGroup _vignetteCanvasGroup;

		private Sequence _focusAnimationSequence;

		public override void PlayFocusAnimation()
		{
			_focusAnimationSequence?.Kill();
			_focusAnimationSequence = DOTween.Sequence();
			foreach (Image vignetteImage in _vignetteImages)
			{
				_focusAnimationSequence.Join(vignetteImage.DOColor(Color.red, 0.2f).SetEase(Ease.OutSine));
				_focusAnimationSequence.Join(vignetteImage.rectTransform.DOScale(Vector3.one * _scaleMultiplier, 0.2f).SetEase(Ease.InSine));
			}
			_focusAnimationSequence.SetLoops(2, LoopType.Yoyo);
		}

		public override void ApplyIntensity(float intensity)
		{
			if (!(_vignetteCanvasGroup == null) && !Mathf.Approximately(_vignetteCanvasGroup.alpha, intensity))
			{
				_vignetteCanvasGroup.alpha = intensity;
			}
		}

		public override void DisposeView()
		{
			_focusAnimationSequence?.Kill();
		}
	}
}
