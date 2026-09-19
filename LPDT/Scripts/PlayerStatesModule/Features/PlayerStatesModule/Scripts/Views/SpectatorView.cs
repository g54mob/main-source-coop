using DG.Tweening;
using Features.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayerStatesModule.Scripts.Views
{
	internal class SpectatorView : SpectatorViewBase
	{
		[SerializeField]
		private Image _fadeImage;

		[SerializeField]
		private float _fadeDuration = 1f;

		private Tween _fadeTween;

		public override void StartFade()
		{
			ClearFade();
			_fadeImage.enabled = true;
			_fadeImage.SetAlpha(1f);
			_fadeTween = _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(delegate
			{
				_fadeImage.enabled = false;
			});
		}

		public override void DisableFade()
		{
			_fadeImage.enabled = false;
			_fadeImage.SetAlpha(0f);
		}

		public override void ClearFade()
		{
			_fadeTween?.Kill();
		}
	}
}
