using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public interface ISelectableWithNavigationCallbacks
	{
		event Action<ISelectableWithNavigationCallbacks> OnSelectEvent;

		event Action<ISelectableWithNavigationCallbacks> OnDeSelectEvent;

		event Action<ISelectableWithNavigationCallbacks> OnHighlightEvent;

		event Action<ISelectableWithNavigationCallbacks> OnPressedEvent;

		event Action<ISelectableWithNavigationCallbacks> OnUnActiveEvent;

		event Action<ISelectableWithNavigationCallbacks> OnActiveEvent;

		Selectable GetSelectable();

		Transform GetSelectableMainParent();

		internal void SetNavigatedFrom(MoveDirection eventDataMoveDir);
	}
}
