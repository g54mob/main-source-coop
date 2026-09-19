using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class ScrollWithNavigationContent : MonoBehaviour
	{
		[SerializeField]
		private LayoutGroupType _layoutGroupType;

		private ISelectableWithNavigationCallbacks[] _allSelectablesInChildren;

		private INavigationService _navigationService;

		[field: SerializeField]
		public RectTransform RectTransform { get; private set; }

		public event Action OnChildrenChanged;

		[Inject]
		private void InjectDependencies(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		private void OnTransformChildrenChanged()
		{
			this.OnChildrenChanged?.Invoke();
		}

		public ISelectableWithNavigationCallbacks[] GetAllSelectableChildren()
		{
			ReInitializeSelectables();
			return _allSelectablesInChildren;
		}

		private void ReInitializeSelectables()
		{
			ISelectableWithNavigationCallbacks[] allSelectablesInChildren;
			if (_allSelectablesInChildren != null)
			{
				allSelectablesInChildren = _allSelectablesInChildren;
				foreach (ISelectableWithNavigationCallbacks obj in allSelectablesInChildren)
				{
					obj.OnUnActiveEvent -= OnChildSelectableDisabled;
					obj.OnActiveEvent -= OnChildSelectableEnabled;
				}
			}
			_allSelectablesInChildren = GetComponentsInChildren<ISelectableWithNavigationCallbacks>();
			allSelectablesInChildren = _allSelectablesInChildren;
			foreach (ISelectableWithNavigationCallbacks obj2 in allSelectablesInChildren)
			{
				obj2.OnUnActiveEvent += OnChildSelectableDisabled;
				obj2.OnActiveEvent += OnChildSelectableEnabled;
			}
		}

		private void OnChildSelectableDisabled(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			SelectNearestSelectable(selectableWithNavigationCallbacks);
			this.OnChildrenChanged?.Invoke();
		}

		private void SelectNearestSelectable(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			if (_navigationService.GetLastSelectedObject() != selectableWithNavigationCallbacks.GetSelectable() || _allSelectablesInChildren == null)
			{
				return;
			}
			ISelectableWithNavigationCallbacks[] array = _allSelectablesInChildren.Where((ISelectableWithNavigationCallbacks s) => s.GetSelectable().interactable || s == selectableWithNavigationCallbacks).ToArray();
			for (int num = 0; num < array.Length; num++)
			{
				if (array[num] == selectableWithNavigationCallbacks)
				{
					if (num + 1 < array.Length)
					{
						_navigationService.SetNavigationToObject(array[num + 1].GetSelectable());
					}
					else if (num - 1 >= 0)
					{
						_navigationService.SetNavigationToObject(array[num - 1].GetSelectable());
					}
					break;
				}
			}
		}

		private void OnChildSelectableEnabled(ISelectableWithNavigationCallbacks selectableWithNavigationCallbacks)
		{
			this.OnChildrenChanged?.Invoke();
		}

		public LayoutGroupType GetLayoutGroupType()
		{
			return _layoutGroupType;
		}
	}
}
