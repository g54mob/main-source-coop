using System;
using System.Collections.Generic;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IFocusableContainer
	{
		bool IsContainerFocusable { get; }

		IEnumerable<IFocusableElement> FocusableElements { get; }

		event Action OnContainerBecomeFocusable;

		event Action OnContainerBecomeUnFocusable;

		void MakeFocusable();

		void MakeUnFocusable();

		void SetFocusableElements(IEnumerable<IFocusableElement> focusableElements);

		void RegisterFocusableElement(IFocusableElement focusableElement);

		void RegisterFocusableElementRange(IEnumerable<IFocusableElement> focusableElements);

		void UnRegisterFocusableElement(IFocusableElement focusableElement);

		void UnRegisterFocusableElementRange(IEnumerable<IFocusableElement> focusableElements);
	}
}
