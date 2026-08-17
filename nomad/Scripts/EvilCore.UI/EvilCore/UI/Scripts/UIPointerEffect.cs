using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public abstract class UIPointerEffect : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		[Header("Timing")]
		[SerializeField]
		protected float duration = 0.15f;

		[SerializeField]
		protected Ease enterEase = Ease.OutBack;

		[SerializeField]
		protected Ease exitEase = Ease.InOutSine;

		[Header("Behaviour")]
		[SerializeField]
		private bool respectInteractable = true;

		[SerializeField]
		private bool triggerOnSelect = true;

		private Selectable _selectable;

		private bool _isActive;

		private bool _initialized;

		protected virtual void Awake()
		{
			EnsureInitialized();
		}

		private void EnsureInitialized()
		{
			if (!_initialized)
			{
				_selectable = GetComponent<Selectable>();
				CacheState();
				_initialized = true;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Begin();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			End();
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (triggerOnSelect)
			{
				Begin();
			}
		}

		public void OnDeselect(BaseEventData eventData)
		{
			if (triggerOnSelect)
			{
				End();
			}
		}

		private void Begin()
		{
			EnsureInitialized();
			if ((!respectInteractable || !(_selectable != null) || _selectable.interactable) && !_isActive)
			{
				_isActive = true;
				OnHoverBegin();
			}
		}

		private void End()
		{
			EnsureInitialized();
			if (_isActive)
			{
				_isActive = false;
				OnHoverEnd();
			}
		}

		protected virtual void OnDisable()
		{
			_isActive = false;
			if (_initialized)
			{
				ResetImmediate();
			}
		}

		protected abstract void CacheState();

		protected abstract void OnHoverBegin();

		protected abstract void OnHoverEnd();

		protected abstract void ResetImmediate();
	}
}
