using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class NavigationForScroll : SelectableWithNavigationCallbacks
	{
		[SerializeField]
		private ScrollWithNavigationContent _content;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private bool _isFocusOnCenter;

		[SerializeField]
		private bool _isAutomaticUpdate = true;

		[SerializeField]
		private bool _isWithDirectionDepending;

		[SerializeField]
		private bool _isNeedsToUpdateOnEnable;

		private List<ISelectableWithNavigationCallbacks> _children;

		private INavigationService _navigationService;

		private bool _isActive = true;

		[Inject]
		public void InjectDependencies(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		protected override void Awake()
		{
			base.OnSelectEvent += SelectFirstElement;
			if (_isAutomaticUpdate && Application.isPlaying)
			{
				UpdateSelectables();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_isAutomaticUpdate)
			{
				_content.OnChildrenChanged += UpdateSelectables;
			}
			if (Application.isPlaying && _isNeedsToUpdateOnEnable)
			{
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

		protected override void OnDisable()
		{
			base.OnDisable();
			if (_isAutomaticUpdate)
			{
				_content.OnChildrenChanged -= UpdateSelectables;
			}
		}

		public void UpdateSelectables()
		{
			if (!_isActive)
			{
				return;
			}
			ISelectableWithNavigationCallbacks[] allSelectableChildren = _content.GetAllSelectableChildren();
			if (allSelectableChildren.Length == 0 && _navigationService != null && _navigationService.IsCurrentSelectedObjectActive())
			{
				_navigationService.SetNavigationToObject(GetSelectable());
			}
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

		private void SelectFirstElement(ISelectableWithNavigationCallbacks _)
		{
			if (!_isActive || _children.Count == 0)
			{
				return;
			}
			if (!_isWithDirectionDepending)
			{
				_navigationService.SetNavigationToObject(_children[0].GetSelectable());
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Vertical)
			{
				if (MoveDirectionFrom == MoveDirection.Up)
				{
					INavigationService navigationService = _navigationService;
					List<ISelectableWithNavigationCallbacks> children = _children;
					navigationService.SetNavigationToObject(children[children.Count - 1].GetSelectable());
				}
				else if (MoveDirectionFrom == MoveDirection.Down)
				{
					_navigationService.SetNavigationToObject(_children[0].GetSelectable());
				}
				else
				{
					_navigationService.SetNavigationToObject(_children[0].GetSelectable());
				}
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Horizontal)
			{
				if (MoveDirectionFrom == MoveDirection.Left)
				{
					INavigationService navigationService2 = _navigationService;
					List<ISelectableWithNavigationCallbacks> children2 = _children;
					navigationService2.SetNavigationToObject(children2[children2.Count - 1].GetSelectable());
				}
				else if (MoveDirectionFrom == MoveDirection.Right)
				{
					_navigationService.SetNavigationToObject(_children[0].GetSelectable());
				}
				else
				{
					_navigationService.SetNavigationToObject(_children[0].GetSelectable());
				}
			}
		}

		public void SetActive(bool isActive)
		{
			_isActive = isActive;
		}

		private void SetNavigationForElementVertical(int index)
		{
			ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks = _children[index];
			selectableWithNavigationCallbacks.OnSelectEvent -= ScrollTo;
			selectableWithNavigationCallbacks.OnSelectEvent += ScrollTo;
			_navigationService.SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnLeft);
			_navigationService.SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnRight);
			if (index == 0)
			{
				_navigationService.SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnUp);
				return;
			}
			_navigationService.SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			_navigationService.SetDownNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				_navigationService.SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnDown);
			}
		}

		private void SetNavigationForElementHorizontal(int index)
		{
			ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks = _children[index];
			selectableWithNavigationCallbacks.OnSelectEvent -= ScrollTo;
			selectableWithNavigationCallbacks.OnSelectEvent += ScrollTo;
			_navigationService.SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnUp);
			_navigationService.SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnDown);
			if (index == 0)
			{
				_navigationService.SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnLeft);
				return;
			}
			_navigationService.SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			_navigationService.SetRightNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				_navigationService.SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), base.navigation.selectOnRight);
			}
		}

		private void ScrollTo(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			if (_isActive)
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
		}

		private void ResetAllNavigation(ISelectableWithNavigationCallbacks[] selectableWithNavigationCallbacksArray)
		{
			foreach (ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks in selectableWithNavigationCallbacksArray)
			{
				_navigationService.SetUpNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				_navigationService.SetDownNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				_navigationService.SetLeftNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
				_navigationService.SetRightNavigation(selectableWithNavigationCallbacks.GetSelectable(), null);
			}
		}
	}
}
