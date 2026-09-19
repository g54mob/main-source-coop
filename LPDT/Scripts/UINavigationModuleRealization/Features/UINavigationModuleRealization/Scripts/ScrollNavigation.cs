using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.UINavigationModuleRealization.Scripts
{
	public class ScrollNavigation : SelectableWithNavigationCallbacks
	{
		[SerializeField]
		private ScrollWithNavigationContent _content;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private bool _isFocusOnCenter;

		[SerializeField]
		private bool _isAutomaticUpdate = true;

		[Header("Animation")]
		[SerializeField]
		private bool _isAnimated;

		[SerializeField]
		private float _animationSpeed = 12f;

		[Header("Editor test")]
		[SerializeField]
		private int _testTargetIndex;

		private readonly Dictionary<ISelectableWithNavigationMoveCallbacks, Action> _selectEventHandlers = new Dictionary<ISelectableWithNavigationMoveCallbacks, Action>();

		private List<ISelectableWithNavigationMoveCallbacks> _children;

		private INavigationService _navigationService;

		private bool _isNavigationEnabled = true;

		private Vector2 _targetAnchoredPosition;

		private bool _isAnimating;

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
			UnsubscribeScrollEvents();
		}

		public void ScrollToIndex(int index)
		{
			ISelectableWithNavigationMoveCallbacks[] navigationMoveCallbacks = GetNavigationMoveCallbacks();
			if (navigationMoveCallbacks.Length != 0)
			{
				index = Mathf.Clamp(index, 0, navigationMoveCallbacks.Length - 1);
				ScrollTo(navigationMoveCallbacks[index]);
			}
		}

		public void SetNavigationEnabled(bool isEnabled)
		{
			if (_isNavigationEnabled != isEnabled)
			{
				_isNavigationEnabled = isEnabled;
				UpdateSelectables();
			}
		}

		public void UpdateSelectables()
		{
			UnsubscribeScrollEvents();
			ISelectableWithNavigationMoveCallbacks[] navigationMoveCallbacks = GetNavigationMoveCallbacks();
			ResetAllNavigation(navigationMoveCallbacks);
			_children = navigationMoveCallbacks.Where(IsNavigable).ToList();
			foreach (ISelectableWithNavigationMoveCallbacks child in _children)
			{
				SubscribeScrollEvent(child);
			}
			if (!_isNavigationEnabled)
			{
				return;
			}
			if (_content.GetLayoutGroupType() == LayoutGroupType.Horizontal)
			{
				for (int i = 0; i < _children.Count; i++)
				{
					SetNavigationForElementHorizontal(i);
				}
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Vertical)
			{
				for (int j = 0; j < _children.Count; j++)
				{
					SetNavigationForElementVertical(j);
				}
			}
		}

		private ISelectableWithNavigationMoveCallbacks[] GetNavigationMoveCallbacks()
		{
			RectTransform rectTransform = _content.RectTransform;
			if (rectTransform == null)
			{
				return GetFallbackMoveCallbacks();
			}
			List<ISelectableWithNavigationMoveCallbacks> list = new List<ISelectableWithNavigationMoveCallbacks>();
			HashSet<Selectable> hashSet = new HashSet<Selectable>();
			for (int i = 0; i < rectTransform.childCount; i++)
			{
				Transform child = rectTransform.GetChild(i);
				ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks = ResolveRowSelectable(child);
				if (selectableWithNavigationMoveCallbacks != null)
				{
					Selectable selectable = selectableWithNavigationMoveCallbacks.GetSelectable();
					if (!(selectable == null) && hashSet.Add(selectable))
					{
						list.Add(selectableWithNavigationMoveCallbacks);
					}
				}
			}
			if (list.Count <= 0)
			{
				return GetFallbackMoveCallbacks();
			}
			return list.ToArray();
		}

		private ISelectableWithNavigationMoveCallbacks[] GetFallbackMoveCallbacks()
		{
			return _content.GetAllSelectableChildren().OfType<ISelectableWithNavigationMoveCallbacks>().ToArray();
		}

		private ISelectableWithNavigationMoveCallbacks ResolveRowSelectable(Transform row)
		{
			ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks = row.GetComponents<ISelectableWithNavigationMoveCallbacks>().FirstOrDefault();
			if (selectableWithNavigationMoveCallbacks != null)
			{
				return selectableWithNavigationMoveCallbacks;
			}
			return row.GetComponentsInChildren<ISelectableWithNavigationMoveCallbacks>().FirstOrDefault();
		}

		private void SubscribeScrollEvent(ISelectableWithNavigationMoveCallbacks selectableWithNavigationCallbacks)
		{
			if (selectableWithNavigationCallbacks is ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks2)
			{
				selectableWithNavigationCallbacks2.OnSelectEvent -= ScrollTo;
				selectableWithNavigationCallbacks2.OnSelectEvent += ScrollTo;
				return;
			}
			Action value = delegate
			{
				ScrollTo(selectableWithNavigationCallbacks);
			};
			if (selectableWithNavigationCallbacks is LeftRightButtonHolder leftRightButtonHolder)
			{
				leftRightButtonHolder.OnSelectEvent += value;
				_selectEventHandlers[selectableWithNavigationCallbacks] = value;
			}
			else if (selectableWithNavigationCallbacks is IDropdownSelectableWithNavigationCallbacks dropdownSelectableWithNavigationCallbacks)
			{
				dropdownSelectableWithNavigationCallbacks.OnSelectEvent += value;
				_selectEventHandlers[selectableWithNavigationCallbacks] = value;
			}
		}

		private void UnsubscribeScrollEvents()
		{
			if (_children != null)
			{
				foreach (ISelectableWithNavigationMoveCallbacks child in _children)
				{
					if (child is ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
					{
						selectableWithNavigationCallbacks.OnSelectEvent -= ScrollTo;
					}
				}
			}
			foreach (KeyValuePair<ISelectableWithNavigationMoveCallbacks, Action> selectEventHandler in _selectEventHandlers)
			{
				if (selectEventHandler.Key is LeftRightButtonHolder leftRightButtonHolder)
				{
					leftRightButtonHolder.OnSelectEvent -= selectEventHandler.Value;
				}
				if (selectEventHandler.Key is IDropdownSelectableWithNavigationCallbacks dropdownSelectableWithNavigationCallbacks)
				{
					dropdownSelectableWithNavigationCallbacks.OnSelectEvent -= selectEventHandler.Value;
				}
			}
			_selectEventHandlers.Clear();
		}

		private bool IsNavigable(ISelectableWithNavigationMoveCallbacks selectableWithNavigationCallbacks)
		{
			Selectable selectable = selectableWithNavigationCallbacks.GetSelectable();
			if (selectable != null)
			{
				return selectable.interactable;
			}
			return false;
		}

		private void SelectFirstElement(ISelectableWithNavigationCallbacks _)
		{
			if (_children != null && _children.Count != 0)
			{
				_navigationService.SetNavigationToObject(_children[0].GetSelectable());
			}
		}

		private void SetNavigationForElementVertical(int index)
		{
			ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks = _children[index];
			SetLeftNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnLeft);
			SetRightNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnRight);
			if (index == 0)
			{
				SetUpNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnUp);
				return;
			}
			SetUpNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			SetDownNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationMoveCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				SetDownNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnDown);
			}
		}

		private void SetNavigationForElementHorizontal(int index)
		{
			ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks = _children[index];
			SetUpNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnUp);
			SetDownNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnDown);
			if (index == 0)
			{
				SetLeftNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnLeft);
				return;
			}
			SetLeftNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), _children[index - 1].GetSelectable());
			SetRightNavigation(_children[index - 1].GetSelectable(), selectableWithNavigationMoveCallbacks.GetSelectable());
			if (index == _children.Count - 1)
			{
				SetRightNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), base.navigation.selectOnRight);
			}
		}

		[ContextMenu("Test Scroll To Index")]
		private void TestScrollToIndex()
		{
			ISelectableWithNavigationMoveCallbacks[] navigationMoveCallbacks = GetNavigationMoveCallbacks();
			if (navigationMoveCallbacks.Length != 0)
			{
				int num = Mathf.Clamp(_testTargetIndex, 0, navigationMoveCallbacks.Length - 1);
				ScrollTo(navigationMoveCallbacks[num]);
			}
		}

		private void ScrollTo(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			ScrollToRect(selectableWithNavigationCallbacks.GetSelectableMainParent().GetComponent<RectTransform>());
		}

		private void ScrollTo(ISelectableWithNavigationMoveCallbacks selectableWithNavigationCallbacks)
		{
			ScrollToRect(selectableWithNavigationCallbacks.GetSelectableMainParent().GetComponent<RectTransform>());
		}

		private void ScrollToRect(RectTransform rectTransform)
		{
			RectTransform rectTransform2 = _content.RectTransform;
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform2);
			RectTransform viewport = _scrollRect.viewport;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			if (_content.GetLayoutGroupType() == LayoutGroupType.Horizontal)
			{
				float width = rectTransform.rect.width;
				float value = ((!_isFocusOnCenter) ? (width / 2f - anchoredPosition.x) : (viewport.rect.width / 2f - anchoredPosition.x));
				float num = Mathf.Max(0f, rectTransform2.rect.width - viewport.rect.width);
				float x = Mathf.Clamp(value, 0f - num, 0f);
				ApplyTarget(new Vector2(x, rectTransform2.anchoredPosition.y));
			}
			else if (_content.GetLayoutGroupType() == LayoutGroupType.Vertical)
			{
				float height = rectTransform.rect.height;
				float value2 = ((!_isFocusOnCenter) ? (0f - anchoredPosition.y - height / 2f) : ((0f - viewport.rect.height) / 2f - anchoredPosition.y));
				float max = Mathf.Max(0f, rectTransform2.rect.height - viewport.rect.height);
				float y = Mathf.Clamp(value2, 0f, max);
				ApplyTarget(new Vector2(rectTransform2.anchoredPosition.x, y));
			}
		}

		private void ApplyTarget(Vector2 target)
		{
			_targetAnchoredPosition = target;
			if (_isAnimated && Application.isPlaying)
			{
				_isAnimating = true;
				return;
			}
			_content.RectTransform.anchoredPosition = target;
			_isAnimating = false;
		}

		private new void Update()
		{
			if (_isAnimating)
			{
				RectTransform rectTransform = _content.RectTransform;
				Vector2 vector = Vector2.Lerp(rectTransform.anchoredPosition, _targetAnchoredPosition, Time.unscaledDeltaTime * _animationSpeed);
				if ((vector - _targetAnchoredPosition).sqrMagnitude < 0.25f)
				{
					vector = _targetAnchoredPosition;
					_isAnimating = false;
				}
				rectTransform.anchoredPosition = vector;
			}
		}

		private void ResetAllNavigation(ISelectableWithNavigationMoveCallbacks[] selectableWithNavigationCallbacksArray)
		{
			foreach (ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks in selectableWithNavigationCallbacksArray)
			{
				SetUpNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), null);
				SetDownNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), null);
				SetLeftNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), null);
				SetRightNavigation(selectableWithNavigationMoveCallbacks.GetSelectable(), null);
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
