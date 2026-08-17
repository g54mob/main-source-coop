using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[SerializeField]
		private float scale = 1.1f;

		[SerializeField]
		private float duration = 0.15f;

		[SerializeField]
		private Ease ease = Ease.OutBack;

		private Vector3 _defaultScale;

		private Button _button;

		private void Awake()
		{
			_defaultScale = base.transform.localScale;
			_button = GetComponent<Button>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!(_button != null) || _button.interactable)
			{
				Tween.Scale(base.transform, _defaultScale * scale, duration, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!(_button != null) || _button.interactable)
			{
				Tween.Scale(base.transform, _defaultScale, duration, Ease.InOutSine, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		private void OnDisable()
		{
			Tween.CompleteAll(base.transform);
			base.transform.localScale = _defaultScale;
		}
	}
}
