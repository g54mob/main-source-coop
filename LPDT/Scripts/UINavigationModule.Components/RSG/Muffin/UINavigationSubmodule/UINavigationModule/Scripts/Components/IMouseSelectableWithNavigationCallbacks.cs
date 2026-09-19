using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public interface IMouseSelectableWithNavigationCallbacks
	{
		event Action<IMouseSelectableWithNavigationCallbacks> OnMouseSelectEvent;

		event Action<IMouseSelectableWithNavigationCallbacks> OnMouseDeSelectEvent;

		Selectable GetSelectable();

		Transform GetSelectableMainParent();

		internal void SetNavigatedFrom(MoveDirection eventDataMoveDir);
	}
}
