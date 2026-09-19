using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public interface ISubmittableWithNavigationCallbacks
	{
		event Action<ISubmittableWithNavigationCallbacks> OnSubmitEvent;

		Selectable GetSelectable();

		Transform GetSelectableMainParent();

		internal void SetNavigatedFrom(MoveDirection eventDataMoveDir);
	}
}
