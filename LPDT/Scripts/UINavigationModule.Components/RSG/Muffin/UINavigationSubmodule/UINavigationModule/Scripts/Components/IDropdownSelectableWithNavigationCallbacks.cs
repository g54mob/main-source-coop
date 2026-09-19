using System;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public interface IDropdownSelectableWithNavigationCallbacks
	{
		event Action OnSelectEvent;

		event Action OnOpenEvent;

		event Action OnItemChangeEvent;

		event Action OnItemHoverEvent;
	}
}
