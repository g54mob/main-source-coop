using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class ButtonWithNavigationCallbacksWithoutMouse : Button, ISelectableWithNavigationCallbacks, ISelectableWithNavigationMoveCallbacks, IMouseSelectableWithNavigationCallbacks
	{
		[SerializeField]
		private Transform _mainParentTransform;

		private bool _previousInteractableState;

		internal MoveDirection MoveDirectionFrom;

		private INavigationService _navigationService;

		public event Action<IMouseSelectableWithNavigationCallbacks> OnMouseSelectEvent;

		public event Action<IMouseSelectableWithNavigationCallbacks> OnMouseDeSelectEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnSelectEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnDeSelectEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnHighlightEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnPressedEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnUnActiveEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnActiveEvent;

		public event Action<MoveDirection> OnMoveEvent;

		public event Action<MoveDirection> BeforeMoveEvent;

		[Inject]
		private void InjectDependencies(NavigationModel navigationModel, INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public override void OnSelect(BaseEventData eventData)
		{
			if (!base.interactable)
			{
				Selectable lastSelectedObject = _navigationService.GetLastSelectedObject();
				if (!(lastSelectedObject == null))
				{
					_navigationService.SetNavigationToObject(lastSelectedObject);
				}
			}
			else
			{
				base.OnSelect(eventData);
				if (eventData is PointerEventData)
				{
					OnMouseSelectEventInvoke();
				}
				else
				{
					OnSelectEventInvoke();
				}
			}
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			if (eventData is PointerEventData)
			{
				OnMouseDeSelectEventInvoke();
			}
			else
			{
				OnDeSelectEventInvoke();
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			OnHighlightEventInvoke();
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			OnPressedEventInvoke();
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
				OnUnActiveEventInvoke();
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

		protected override void Start()
		{
			base.Start();
			_previousInteractableState = base.interactable;
		}

		protected void Update()
		{
			if (_previousInteractableState != base.interactable)
			{
				OnUnActiveEventInvoke();
				_previousInteractableState = base.interactable;
			}
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

		private void SetNavigationFromToSelectable(Selectable selectable, MoveDirection moveDir)
		{
			if (!(selectable == null))
			{
				selectable.TryGetComponent<ISelectableWithNavigationCallbacks>(out var component);
				component.SetNavigatedFrom(moveDir);
			}
		}

		public virtual Transform GetSelectableMainParent()
		{
			if (!(_mainParentTransform != null))
			{
				return base.transform;
			}
			return _mainParentTransform;
		}

		public virtual Selectable GetSelectable()
		{
			return this;
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

		void IMouseSelectableWithNavigationCallbacks.SetNavigatedFrom(MoveDirection moveDirection)
		{
			MoveDirectionFrom = moveDirection;
			this.OnMoveEvent?.Invoke(moveDirection);
		}

		public void OnSelectEventInvoke()
		{
			this.OnSelectEvent?.Invoke(this);
		}

		protected void OnDeSelectEventInvoke()
		{
			this.OnDeSelectEvent?.Invoke(this);
		}

		protected void OnHighlightEventInvoke()
		{
			this.OnHighlightEvent?.Invoke(this);
		}

		protected void OnPressedEventInvoke()
		{
			this.OnPressedEvent?.Invoke(this);
		}

		protected void OnUnActiveEventInvoke()
		{
			this.OnUnActiveEvent?.Invoke(this);
		}

		protected void OnMouseSelectEventInvoke()
		{
			this.OnMouseSelectEvent?.Invoke(this);
		}

		protected void OnMouseDeSelectEventInvoke()
		{
			this.OnMouseDeSelectEvent?.Invoke(this);
		}

		protected void OnActiveEventInvoke()
		{
			this.OnActiveEvent?.Invoke(this);
		}
	}
}
