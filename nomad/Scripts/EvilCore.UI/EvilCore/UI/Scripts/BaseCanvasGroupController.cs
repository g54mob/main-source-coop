using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	[RequireComponent(typeof(CanvasGroup))]
	public class BaseCanvasGroupController : MonoBehaviour
	{
		protected CanvasGroup _canvasGroup;

		public bool IsVisible => _canvasGroup.alpha > 0f;

		public bool BlockRaycasts => _canvasGroup.blocksRaycasts;

		public bool IsInteractable => _canvasGroup.interactable;

		public void Initialize()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			Hide();
		}

		public virtual void Show(bool interactable, bool blockRaycast)
		{
			_canvasGroup.alpha = 1f;
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;
		}

		public virtual void Hide()
		{
			_canvasGroup.alpha = 0f;
			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;
		}

		public virtual void FadeIn(float duration)
		{
			Tween.CompleteAll(_canvasGroup);
			Tween.Alpha(_canvasGroup, 1f, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		public virtual void FadeOut(float duration)
		{
			Tween.CompleteAll(_canvasGroup);
			Tween.Alpha(_canvasGroup, 0f, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		public void ShowDebug()
		{
			_canvasGroup.alpha = 1f;
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;
		}

		public void HideDebug()
		{
			_canvasGroup.alpha = 0f;
			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;
		}
	}
}
