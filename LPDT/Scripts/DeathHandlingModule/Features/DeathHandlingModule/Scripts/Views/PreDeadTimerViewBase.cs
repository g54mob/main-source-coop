using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DeathHandlingModule.Scripts.Views
{
	public abstract class PreDeadTimerViewBase : ViewBehaviour
	{
		private static readonly int Activate = Animator.StringToHash("Activate");

		[SerializeField]
		private RectTransform _indicator;

		[SerializeField]
		private float _minX;

		[SerializeField]
		private float _maxX;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private Animator _ghostAnimator;

		[field: SerializeField]
		public float GhostDuration { get; private set; } = 2f;

		public virtual void SetNormalizedValue(float normalized)
		{
			if (!(_indicator == null))
			{
				float x = Mathf.Lerp(_minX, _maxX, Mathf.Clamp01(normalized));
				Vector2 anchoredPosition = _indicator.anchoredPosition;
				anchoredPosition.x = x;
				_indicator.anchoredPosition = anchoredPosition;
			}
		}

		public virtual void SetVisible(bool visible)
		{
			if (!(_canvasGroup == null))
			{
				_canvasGroup.alpha = (visible ? 1f : 0f);
				_canvasGroup.blocksRaycasts = visible;
				_canvasGroup.interactable = visible;
			}
		}

		public void ActivateGhost()
		{
			_ghostAnimator.SetTrigger(Activate);
		}
	}
}
