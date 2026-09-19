using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public interface ISelectableWithNavigationMoveCallbacks
	{
		event Action<MoveDirection> OnMoveEvent;

		event Action<MoveDirection> BeforeMoveEvent;

		Selectable GetSelectable();

		Transform GetSelectableMainParent();

		internal void SetNavigatedFrom(MoveDirection eventDataMoveDir);
	}
}
