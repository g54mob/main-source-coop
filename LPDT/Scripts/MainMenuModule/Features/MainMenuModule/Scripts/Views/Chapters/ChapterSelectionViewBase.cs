using System;
using DG.Tweening;
using Features.UINavigationModuleRealization.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class ChapterSelectionViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _optionsContainer;

		[Header("Rebuild fade")]
		[SerializeField]
		private CanvasGroup _optionsCanvasGroup;

		[SerializeField]
		private float _fadeOutDuration = 0.15f;

		[SerializeField]
		private float _fadeInDuration = 0.25f;

		private Sequence _rebuildFadeSequence;

		[field: SerializeField]
		public ChapterOptionViewBase ChapterOptionViewBase { get; private set; }

		[field: SerializeField]
		public ChapterOptionViewBase FinalChapterOptionViewBase { get; private set; }

		[field: SerializeField]
		public ScrollNavigation ScrollNavigation { get; private set; }

		[field: SerializeField]
		public Button LeftNavigationButton { get; private set; }

		[field: SerializeField]
		public Button RightNavigationButton { get; private set; }

		[field: SerializeField]
		public RectTransform RightNavigationContainer { get; private set; }

		[field: SerializeField]
		public RectTransform LeftNavigationContainer { get; private set; }

		private new void OnDisable()
		{
			ShowOptionsInstantly();
		}

		public Transform GetOptionsContainer()
		{
			return _optionsContainer;
		}

		public void PlayRebuildFade(Action rebuild)
		{
			KillRebuildFade();
			SetOptionsInteractable(isInteractable: false);
			_rebuildFadeSequence = DOTween.Sequence().SetUpdate(isIndependentUpdate: true).Append(_optionsCanvasGroup.DOFade(0f, _fadeOutDuration).SetEase(Ease.OutSine))
				.AppendCallback(delegate
				{
					rebuild();
				})
				.Append(_optionsCanvasGroup.DOFade(1f, _fadeInDuration).SetEase(Ease.OutSine))
				.OnComplete(delegate
				{
					_rebuildFadeSequence = null;
					SetOptionsInteractable(isInteractable: true);
				});
		}

		public void ShowOptionsInstantly()
		{
			KillRebuildFade();
			_optionsCanvasGroup.alpha = 1f;
			SetOptionsInteractable(isInteractable: true);
		}

		private void SetOptionsInteractable(bool isInteractable)
		{
			_optionsCanvasGroup.interactable = isInteractable;
			_optionsCanvasGroup.blocksRaycasts = isInteractable;
		}

		private void KillRebuildFade()
		{
			if (_rebuildFadeSequence != null)
			{
				_rebuildFadeSequence.Kill();
				_rebuildFadeSequence = null;
			}
		}
	}
}
