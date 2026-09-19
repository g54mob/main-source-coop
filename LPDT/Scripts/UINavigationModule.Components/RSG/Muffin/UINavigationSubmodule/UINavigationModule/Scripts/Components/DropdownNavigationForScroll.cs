using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class DropdownNavigationForScroll : SelectableWithNavigationCallbacks
	{
		[SerializeField]
		private TMP_Dropdown _dropdown;

		[SerializeField]
		private ScrollWithNavigationContent _content;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private bool _isFocusOnCenter;

		[SerializeField]
		private bool _isAutomaticUpdate = true;

		private List<ISelectableWithNavigationCallbacks> _children;

		private INavigationService _navigationService;

		[Inject]
		public void InjectDependencies(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		protected override void Awake()
		{
			base.OnSelectEvent += SelectFirstElement;
			if (_isAutomaticUpdate)
			{
				_content.OnChildrenChanged += UpdateSelectables;
				UpdateSelectables();
			}
		}

		protected override void OnDestroy()
		{
			base.OnSelectEvent -= SelectFirstElement;
			if (_isAutomaticUpdate)
			{
				_content.OnChildrenChanged -= UpdateSelectables;
			}
		}

		public void UpdateSelectables()
		{
			ISelectableWithNavigationCallbacks[] allSelectableChildren = _content.GetAllSelectableChildren();
			ResetAllNavigation(allSelectableChildren);
			_children = allSelectableChildren.Where((ISelectableWithNavigationCallbacks s) => s.GetSelectable().interactable).ToList();
			if (_content.GetLayoutGroupType() == LayoutGroupType.Horizontal)
			{
				for (int num = 0; num < _children.Count; num++)
				{
					SetNavigationForElementHorizontal(num);
				}
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Vertical)
			{
				for (int num2 = 0; num2 < _children.Count; num2++)
				{
					SetNavigationForElementVertical(num2);
				}
			}
		}

		private void LateUpdate()
		{
			if (EventSystem.current == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			foreach (ISelectableWithNavigationCallbacks child in _children)
			{
				if (currentSelectedGameObject != null && currentSelectedGameObject.transform.IsChildOf(child.GetSelectable().gameObject.transform))
				{
					return;
				}
			}
			if (_children.Count == 0)
			{
				return;
			}
			_dropdown.Hide();
			if (currentSelectedGameObject != null)
			{
				if (currentSelectedGameObject.TryGetComponent<Selectable>(out var component))
				{
					_navigationService.SetNavigationToObject(component);
				}
			}
			else
			{
				_navigationService.SetNavigationToObject(_dropdown);
			}
		}

		private void SelectFirstElement(ISelectableWithNavigationCallbacks _)
		{
			_navigationService.SetNavigationToObject(_children[0].GetSelectable());
		}

		private void SetNavigationForElementVertical(int index)
		{
			ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks = _children[index];
			selectableWithNavigationCallbacks.OnSelectEvent -= ScrollTo;
			selectableWithNavigationCallbacks.OnSelectEvent += ScrollTo;
			SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnLeft);
			SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnRight);
			if (index == 0)
			{
				SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnUp);
				return;
			}
			SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			SetDownNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnDown);
			}
		}

		private void SetNavigationForElementHorizontal(int index)
		{
			ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks = _children[index];
			selectableWithNavigationCallbacks.OnSelectEvent -= ScrollTo;
			selectableWithNavigationCallbacks.OnSelectEvent += ScrollTo;
			SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnUp);
			SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnDown);
			if (index == 0)
			{
				SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnLeft);
				return;
			}
			SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			SetRightNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnRight);
			}
		}

		private void ScrollTo(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			Canvas.ForceUpdateCanvases();
			RectTransform component = selectableWithNavigationCallbacks.GetSelectableMainParent().GetComponent<RectTransform>();
			Vector2 anchoredPosition = component.anchoredPosition;
			if (_content.GetLayoutGroupType() == LayoutGroupType.Horizontal)
			{
				float width = component.rect.width;
				float x = ((!_isFocusOnCenter) ? (0f - anchoredPosition.x - width / 2f) : ((0f - _scrollRect.viewport.rect.width) / 2f - anchoredPosition.x));
				_content.RectTransform.anchoredPosition = new Vector2(x, _content.RectTransform.anchoredPosition.y);
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Vertical)
			{
				float height = component.rect.height;
				float y = ((!_isFocusOnCenter) ? (0f - anchoredPosition.y - height / 2f) : ((0f - _scrollRect.viewport.rect.height) / 2f - anchoredPosition.y));
				_content.RectTransform.anchoredPosition = new Vector2(_content.RectTransform.anchoredPosition.x, y);
			}
		}

		private void ResetAllNavigation(ISelectableWithNavigationCallbacks[] selectableWithNavigationCallbacksArray)
		{
			foreach (ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks in selectableWithNavigationCallbacksArray)
			{
				SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
			}
		}

		private void SetLeftNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnLeft = toSelectable;
			fromSelectable.navigation = navigation;
		}

		private void SetRightNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnRight = toSelectable;
			fromSelectable.navigation = navigation;
		}

		private void SetUpNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnUp = toSelectable;
			fromSelectable.navigation = navigation;
		}

		private void SetDownNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = toSelectable;
			fromSelectable.navigation = navigation;
		}
	}
}
