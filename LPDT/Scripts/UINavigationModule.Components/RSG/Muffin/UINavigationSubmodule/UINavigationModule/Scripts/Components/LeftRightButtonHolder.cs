using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class LeftRightButtonHolder : Selectable, ISelectableWithNavigationMoveCallbacks
	{
		private enum Axis
		{
			Horizontal = 0,
			Vertical = 1
		}

		[SerializeField]
		private Axis _axis;

		[SerializeField]
		private Transform _mainParentTransform;

		public List<string> Values = new List<string>();

		public int CurrentValueIndex { get; private set; }

		public string CurrentValue => Values[CurrentValueIndex];

		public event Action OnValueIncreased;

		public event Action OnValueDecreased;

		public event Action OnValueChanged;

		public event Action OnSelectEvent;

		public event Action<MoveDirection> OnMoveEvent;

		public event Action<MoveDirection> BeforeMoveEvent;

		public Selectable GetSelectable()
		{
			return this;
		}

		public Transform GetSelectableMainParent()
		{
			if (!(_mainParentTransform != null))
			{
				return base.transform;
			}
			return _mainParentTransform;
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.OnSelectEvent?.Invoke();
		}

		public override void OnMove(AxisEventData eventData)
		{
			this.BeforeMoveEvent?.Invoke(eventData.moveDir);
			if (!IsActive() || !IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (_axis == Axis.Horizontal && FindSelectableOnLeft() == null)
				{
					DecreaseValue();
					break;
				}
				SetNavigationFromToSelectable(FindSelectableOnLeft(), eventData.moveDir);
				base.OnMove(eventData);
				break;
			case MoveDirection.Right:
				if (_axis == Axis.Horizontal && FindSelectableOnRight() == null)
				{
					IncreaseValue();
					break;
				}
				SetNavigationFromToSelectable(FindSelectableOnRight(), eventData.moveDir);
				base.OnMove(eventData);
				break;
			case MoveDirection.Up:
				if (_axis == Axis.Vertical && FindSelectableOnUp() == null)
				{
					IncreaseValue();
					break;
				}
				SetNavigationFromToSelectable(FindSelectableOnUp(), eventData.moveDir);
				base.OnMove(eventData);
				break;
			case MoveDirection.Down:
				if (_axis == Axis.Vertical && FindSelectableOnDown() == null)
				{
					DecreaseValue();
					break;
				}
				SetNavigationFromToSelectable(FindSelectableOnDown(), eventData.moveDir);
				base.OnMove(eventData);
				break;
			}
		}

		private void SetNavigationFromToSelectable(Selectable selectable, MoveDirection moveDir)
		{
			if (!(selectable == null))
			{
				selectable.TryGetComponent<ISelectableWithNavigationCallbacks>(out var component);
				component?.SetNavigatedFrom(moveDir);
			}
		}

		void ISelectableWithNavigationMoveCallbacks.SetNavigatedFrom(MoveDirection moveDirection)
		{
			this.OnMoveEvent?.Invoke(moveDirection);
		}

		public void SetIndex(int index)
		{
			CurrentValueIndex = index;
			if (CurrentValueIndex > Values.Count - 1)
			{
				CurrentValueIndex = Values.Count - 1;
			}
			if (CurrentValueIndex < 0)
			{
				CurrentValueIndex = 0;
			}
			this.OnValueChanged?.Invoke();
		}

		public void IncreaseValue()
		{
			CurrentValueIndex++;
			if (CurrentValueIndex > Values.Count - 1)
			{
				CurrentValueIndex = Values.Count - 1;
			}
			this.OnValueIncreased?.Invoke();
			this.OnValueChanged?.Invoke();
		}

		public void DecreaseValue()
		{
			CurrentValueIndex--;
			if (CurrentValueIndex < 0)
			{
				CurrentValueIndex = 0;
			}
			this.OnValueDecreased?.Invoke();
			this.OnValueChanged?.Invoke();
		}
	}
}
