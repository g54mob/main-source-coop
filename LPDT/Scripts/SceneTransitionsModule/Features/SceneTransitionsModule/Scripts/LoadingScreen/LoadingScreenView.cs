using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenView : LoadingScreenViewBase
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private GameObject _background;

		[SerializeField]
		private ScreensView _screensView;

		private Tween _activeTween;

		private UniTaskCompletionSource _activeTweenCompletionSource;

		public override ScreensViewBase ScreensView => _screensView;

		public override void SetVisible(bool visible)
		{
			KillActiveTween();
			base.gameObject.SetActive(visible);
			if (visible)
			{
				base.transform.SetAsLastSibling();
			}
		}

		public override void SetAlpha(float alpha)
		{
			KillActiveTween();
			_canvasGroup.alpha = alpha;
		}

		public override void SetInteractable(bool interactable)
		{
			_canvasGroup.blocksRaycasts = interactable;
		}

		public override UniTask FadeAsync(float targetAlpha, float duration, bool useUnscaledTime)
		{
			KillActiveTween();
			if (duration <= 0f)
			{
				_canvasGroup.alpha = targetAlpha;
				return UniTask.CompletedTask;
			}
			UniTaskCompletionSource completionSource = new UniTaskCompletionSource();
			_activeTweenCompletionSource = completionSource;
			_activeTween = _canvasGroup.DOFade(targetAlpha, duration).SetUpdate(useUnscaledTime).OnComplete(delegate
			{
				CompleteActiveTween(completionSource);
			});
			return completionSource.Task;
		}

		public override void SetBackgroundActive(bool active)
		{
			_background.SetActive(active);
		}

		public override void SetScreensRootVisible(bool visible)
		{
			_screensView.SetRootActive(visible);
		}

		private void KillActiveTween()
		{
			_activeTween?.Kill();
			_activeTween = null;
			_activeTweenCompletionSource?.TrySetResult();
			_activeTweenCompletionSource = null;
		}

		private void CompleteActiveTween(UniTaskCompletionSource completionSource)
		{
			if (_activeTweenCompletionSource == completionSource)
			{
				_activeTween = null;
				_activeTweenCompletionSource = null;
				completionSource.TrySetResult();
			}
		}
	}
}
