using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIPressScale : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler
	{
		[Header("Press")]
		[SerializeField]
		private float pressScale = 0.92f;

		[SerializeField]
		private float duration = 0.08f;

		[SerializeField]
		private Ease ease = Ease.OutQuad;

		[SerializeField]
		private bool respectInteractable = true;

		private Vector3 _defaultScale;

		private Selectable _selectable;

		private void Awake()
		{
			_defaultScale = base.transform.localScale;
			_selectable = GetComponent<Selectable>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!respectInteractable || !(_selectable != null) || _selectable.interactable)
			{
				Tween.Scale(base.transform, _defaultScale * pressScale, duration, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Release();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Release();
		}

		private void Release()
		{
			Tween.Scale(base.transform, _defaultScale, duration, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		private void OnDisable()
		{
			Tween.CompleteAll(base.transform);
			base.transform.localScale = _defaultScale;
		}
	}
}
