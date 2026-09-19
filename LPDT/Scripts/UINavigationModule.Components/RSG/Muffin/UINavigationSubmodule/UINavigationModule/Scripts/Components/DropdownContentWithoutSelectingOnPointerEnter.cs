using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class DropdownContentWithoutSelectingOnPointerEnter : TMP_Dropdown, IDropdownSelectableWithNavigationCallbacks, ISelectableWithNavigationMoveCallbacks
	{
		[SerializeField]
		private Transform _mainParentTransform;

		private IInstantiator _instantiator;

		private bool _isPointerInside;

		internal MoveDirection MoveDirectionFrom;

		public event Action OnSelectEvent;

		public event Action OnOpenEvent;

		public event Action OnItemChangeEvent;

		public event Action OnItemHoverEvent;

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

		[Inject]
		private void InjectDependencies(IInstantiator instantiator)
		{
			_instantiator = instantiator;
		}

		protected override void Start()
		{
			base.Start();
			if (Application.isPlaying)
			{
				base.onValueChanged.AddListener(OnValueChangedSound);
			}
		}

		protected override void OnDestroy()
		{
			if (Application.isPlaying)
			{
				base.onValueChanged.RemoveListener(OnValueChangedSound);
			}
			base.OnDestroy();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			_isPointerInside = true;
			this.OnSelectEvent?.Invoke();
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			_isPointerInside = false;
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (!_isPointerInside)
			{
				this.OnSelectEvent?.Invoke();
			}
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			DestroyDropdownItems();
			this.OnOpenEvent?.Invoke();
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			DestroyDropdownItems();
			this.OnOpenEvent?.Invoke();
		}

		protected override DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			DropdownItem dropdownItem = base.CreateItem(itemTemplate);
			if (dropdownItem != null)
			{
				TMPDropdownItemSelectable tMPDropdownItemSelectable = dropdownItem.gameObject.GetComponent<TMPDropdownItemSelectable>();
				if (tMPDropdownItemSelectable == null)
				{
					tMPDropdownItemSelectable = dropdownItem.gameObject.AddComponent<TMPDropdownItemSelectable>();
				}
				tMPDropdownItemSelectable.Initialize(this);
			}
			return dropdownItem;
		}

		private void DestroyDropdownItems()
		{
			DropdownItem[] componentsInChildren = GetComponentsInChildren<DropdownItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}

		protected override GameObject CreateDropdownList(GameObject dropdownTemplate)
		{
			return _instantiator.InstantiatePrefab(dropdownTemplate, base.transform);
		}

		private void OnValueChangedSound(int value)
		{
			this.OnItemChangeEvent?.Invoke();
		}

		internal void OnItemHover()
		{
			this.OnItemHoverEvent?.Invoke();
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
