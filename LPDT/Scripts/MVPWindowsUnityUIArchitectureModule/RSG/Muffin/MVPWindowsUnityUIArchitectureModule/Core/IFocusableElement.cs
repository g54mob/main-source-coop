using System;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IFocusableElement
	{
		bool IsFocused { get; }

		bool IsFocusable { get; }

		event Action OnFocused;

		event Action OnUnFocused;

		void Focus(Action onInteract);

		void UnFocus();

		void MakeFocusable();

		void MakeUnFocusable();
	}
}
