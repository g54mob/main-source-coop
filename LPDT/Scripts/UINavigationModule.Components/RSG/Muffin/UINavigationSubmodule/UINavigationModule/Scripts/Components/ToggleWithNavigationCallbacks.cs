using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class ToggleWithNavigationCallbacks : Toggle, ISelectableWithNavigationCallbacks, ISelectableWithNavigationMoveCallbacks
	{
		[SerializeField]
		private Transform _mainParentTransform;

		private bool _previousInteractableState;

		internal MoveDirection MoveDirectionFrom;

		public event Action<ISelectableWithNavigationCallbacks> OnSelectEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnDeSelectEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnHighlightEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnPressedEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnUnActiveEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnActiveEvent;

		public event Action<MoveDirection> OnMoveEvent;

		public event Action<MoveDirection> BeforeMoveEvent;

		public Transform GetSelectableMainParent()
		{
			if (!(_mainParentTransform != null))
			{
				return base.transform;
			}
			return _mainParentTransform;
		}

		public Selectable GetSelectable()
		{
			return this;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.gameObject.activeSelf)
			{
				OnActiveEventInvoke();
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (!base.gameObject.activeSelf)
			{
				this.OnUnActiveEvent?.Invoke(this);
			}
		}

		protected override void Start()
		{
			base.Start();
			_previousInteractableState = base.interactable;
		}

		protected void Update()
		{
			if (_previousInteractableState != base.interactable)
			{
				this.OnUnActiveEvent?.Invoke(this);
				_previousInteractableState = base.interactable;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.OnSelectEvent = null;
			this.OnDeSelectEvent = null;
			this.OnHighlightEvent = null;
			this.OnPressedEvent = null;
			this.OnUnActiveEvent = null;
			this.OnActiveEvent = null;
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (!(eventData is PointerEventData))
			{
				this.OnSelectEvent?.Invoke(this);
			}
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			if (!(eventData is PointerEventData))
			{
				this.OnDeSelectEvent?.Invoke(this);
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this.OnHighlightEvent?.Invoke(this);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			this.OnPressedEvent?.Invoke(this);
		}

		protected void OnActiveEventInvoke()
		{
			this.OnActiveEvent?.Invoke(this);
		}

		public override void OnMove(AxisEventData eventData)
		{
			this.BeforeMoveEvent?.Invoke(eventData.moveDir);
			switch (eventData.moveDir)
			{
			case MoveDirection.Right:
				SetNavigationFromToSelectable(FindSelectableOnRight(), eventData.moveDir);
				break;
			case MoveDirection.Up:
				SetNavigationFromToSelectable(FindSelectableOnUp(), eventData.moveDir);
				break;
			case MoveDirection.Left:
				SetNavigationFromToSelectable(FindSelectableOnLeft(), eventData.moveDir);
				break;
			case MoveDirection.Down:
				SetNavigationFromToSelectable(FindSelectableOnDown(), eventData.moveDir);
				break;
			}
			base.OnMove(eventData);
		}

		void ISelectableWithNavigationCallbacks.SetNavigatedFrom(MoveDirection moveDirection)
		{
			MoveDirectionFrom = moveDirection;
			this.OnMoveEvent?.Invoke(moveDirection);
		}

		void ISelectableWithNavigationMoveCallbacks.SetNavigatedFrom(MoveDirection moveDirection)
		{
			MoveDirectionFrom = moveDirection;
			this.OnMoveEvent?.Invoke(moveDirection);
		}

		private void SetNavigationFromToSelectable(Selectable selectable, MoveDirection moveDir)
		{
			if (!(selectable == null))
			{
				selectable.TryGetComponent<ISelectableWithNavigationCallbacks>(out var component);
				component?.SetNavigatedFrom(moveDir);
			}
		}
	}
}
